#!/usr/bin/env dotnet-script
// generate-activity-docs.cs
// Usage: dotnet generate-activity-docs.cs [project-root] [output-dir]
//
// Scans all .cs files under [project-root] for classes that inherit from any *Activity base,
// reads the nearest Resources.resx for display names / descriptions,
// and writes one Markdown file per activity to [output-dir].

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

// ── Entry point ──────────────────────────────────────────────────────────────

string projectRoot = args.Length > 0 ? args[0] : Directory.GetCurrentDirectory();
string outputDir   = args.Length > 1 ? args[1] : Path.Combine(projectRoot, "docs", "activities");

Console.WriteLine($"Project root : {projectRoot}");
Console.WriteLine($"Output dir   : {outputDir}");
Console.WriteLine();

Directory.CreateDirectory(outputDir);

// ── 1. Collect activity class names from .cs files ───────────────────────────

// Matches: class Foo : CodeActivity, class Foo : NativeActivity<T>, etc.
var classRegex = new Regex(
    @"(?:public|internal|private|protected)?\s*(?:sealed\s+|abstract\s+)?class\s+(?<name>\w+)\s*(?:<[^>]*>)?\s*:\s*[^{]*\b(?:NativeActivity|CodeActivity|AsyncCodeActivity|Activity)\b",
    RegexOptions.Compiled);

// Also captures public properties with their types and any [Required] / [DefaultValue] attributes
var propRegex = new Regex(
    @"(?<attrs>(?:\[[^\]]+\]\s*)*)public\s+(?<type>In(?:Out)?Argument<[^>]+>|Out(?:Argument)?<[^>]+>|[\w<>, \[\]]+)\s+(?<name>\w+)\s*\{\s*get;",
    RegexOptions.Compiled);

var activities = new List<ActivityInfo>();

foreach (var csFile in Directory.EnumerateFiles(projectRoot, "*.cs", SearchOption.AllDirectories))
{
    // Skip generated / designer files
    if (csFile.EndsWith(".Designer.cs") || csFile.EndsWith(".g.cs"))
        continue;

    string src = File.ReadAllText(csFile);

    foreach (Match cm in classRegex.Matches(src))
    {
        string className = cm.Groups["name"].Value;

        // Collect properties defined in the same file
        var props = new List<PropertyInfo>();
        foreach (Match pm in propRegex.Matches(src))
        {
            string propName  = pm.Groups["name"].Value;
            string propType  = pm.Groups["type"].Value.Trim();
            string attrBlock = pm.Groups["attrs"].Value;

            bool required = attrBlock.Contains("RequiredArgument") ||
                            attrBlock.Contains("Required");

            props.Add(new PropertyInfo(propName, propType, required));
        }

        activities.Add(new ActivityInfo(className, csFile, props));
        Console.WriteLine($"  Found activity: {className}  ({Path.GetFileName(csFile)})");
    }
}

Console.WriteLine();

if (activities.Count == 0)
{
    Console.WriteLine("No activity classes found. Check the project root path.");
    return;
}

// ── 2. Load Resources.resx files (all of them, merged) ───────────────────────

var resources = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

foreach (var resx in Directory.EnumerateFiles(projectRoot, "*.resx", SearchOption.AllDirectories))
{
    // Skip satellite / localised resx (Resources.pt-BR.resx, etc.)
    string fileName = Path.GetFileNameWithoutExtension(resx);
    if (fileName.Contains('.'))
        continue;

    Console.WriteLine($"  Loading resources: {resx}");

    try
    {
        var doc = XDocument.Load(resx);
        foreach (var data in doc.Descendants("data"))
        {
            string? name  = data.Attribute("name")?.Value;
            string? value = data.Element("value")?.Value;
            if (name is not null && value is not null)
                resources.TryAdd(name, value);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"    [WARN] Could not parse {resx}: {ex.Message}");
    }
}

Console.WriteLine($"  Total resource entries loaded: {resources.Count}");
Console.WriteLine();

// ── 3. Generate one Markdown per activity ────────────────────────────────────

int generated = 0;

foreach (var activity in activities)
{
    string cls = activity.ClassName;

    // Look up well-known keys: <ClassName>_DisplayName / <ClassName>_Description
    string displayName  = Lookup(resources, $"{cls}_DisplayName",  cls);
    string description  = Lookup(resources, $"{cls}_Description",  string.Empty);

    var sb = new StringBuilder();

    // Title
    sb.AppendLine($"# {displayName}");
    sb.AppendLine();

    // Description
    if (!string.IsNullOrWhiteSpace(description))
    {
        sb.AppendLine(description);
        sb.AppendLine();
    }

    // Properties table — only InArgument / OutArgument / InOutArgument make sense to document
    var docProps = activity.Properties
        .Where(p => IsDocumentableArgument(p.Type))
        .ToList();

    if (docProps.Count > 0)
    {
        sb.AppendLine("## Properties");
        sb.AppendLine();
        sb.AppendLine("| Name | Description | Required |");
        sb.AppendLine("|------|-------------|----------|");

        foreach (var prop in docProps)
        {
            string propDisplay = Lookup(resources, $"{cls}_{prop.Name}_DisplayName", prop.Name);
            string propDesc    = Lookup(resources, $"{cls}_{prop.Name}_Description",  string.Empty);
            string req         = prop.Required ? "✓" : "";

            sb.AppendLine($"| {propDisplay} | {EscapeMd(propDesc)} | {req} |");
        }

        sb.AppendLine();
    }

    // Write file
    string safeFileName = Regex.Replace(displayName, @"[^\w\- ]", "").Trim().Replace(' ', '-');
    if (string.IsNullOrWhiteSpace(safeFileName)) safeFileName = cls;

    string outPath = Path.Combine(outputDir, $"{safeFileName}.md");
    File.WriteAllText(outPath, sb.ToString(), Encoding.UTF8);
    Console.WriteLine($"  Written: {outPath}");
    generated++;
}

Console.WriteLine();
Console.WriteLine($"Done. {generated} file(s) generated in: {outputDir}");

// ── Helpers ───────────────────────────────────────────────────────────────────

static string Lookup(Dictionary<string, string> res, string key, string fallback)
    => res.TryGetValue(key, out var v) ? v : fallback;

static bool IsDocumentableArgument(string type)
    => type.StartsWith("InArgument")    ||
       type.StartsWith("OutArgument")   ||
       type.StartsWith("InOutArgument") ||
       type.StartsWith("Out<");

static string EscapeMd(string s)
    => s.Replace("|", "\\|").Replace("\n", " ").Replace("\r", "");

// ── Data types ────────────────────────────────────────────────────────────────

record PropertyInfo(string Name, string Type, bool Required);
record ActivityInfo(string ClassName, string SourceFile, List<PropertyInfo> Properties);
