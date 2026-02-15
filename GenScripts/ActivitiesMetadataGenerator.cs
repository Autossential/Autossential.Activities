#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8601 // Possible null reference assignment.

#:package UiPath.Workflow@6.0.0-20240401-07
#:package UiPath.Activities.Api@24.10.1
#:package System.Activities.ViewModels@1.20251127.3
#:project D:\Development\Autossential-4\Autossential.Activities\Autossential.Activities.csproj

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

var types = Assembly.Load("Autossential.Activities").GetTypes()
    .Where(t =>
        t.IsClass
        && t.IsPublic
        && typeof(System.Activities.Activity).IsAssignableFrom(t)
        && !t.IsAbstract
    ).ToList();

var root = new Root
{
    ResourceManagerName = "Autossential.Activities.Properties.Resources",
    Activities = new List<Activity>()
};

var files = Directory.GetFiles("..\\Autossential.Activities", "*.cs", SearchOption.AllDirectories);

foreach (var t in types)
{
    var name = t.Name;
    var index = name.IndexOf("`");
    var suffix = "";
    if (index > 0)
    {
        suffix = name[index..];
        name = name[..index];
    }

    var fileLocation = files.FirstOrDefault(f => f.EndsWith($"{name}.cs"));

    var activity = new Activity
    {
        FullName = t.FullName,
        ShortName = name,
        IconKey = $"{name}.svg",
        ViewModelType = $"Autossential.Activities.ViewModels.{name}ViewModel{suffix}",
        DisplayNameKey = $"{name}_DisplayName",
        DescriptionKey = $"{name}_Description",
        Properties = new List<Property>()
    };

    root.Activities.Add(activity);

    foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
    {
        var propPrefix = GetPropertyPrefix(name, p.Name);

        var property = new Property
        {
            Name = p.Name,
            DisplayNameKey = $"{propPrefix}_DisplayName",
            TooltipKey = $"{propPrefix}_Description",
            // IsPrincipal = false,
            // IsRequired = false,
            IsVisible = true
        };

        activity.Properties.Add(property);
    }
}


var content = JsonSerializer.Serialize(root, new JsonSerializerOptions
{
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    TypeInfoResolver = new DefaultJsonTypeInfoResolver()
});

File.WriteAllText("..\\Autossential.Activities\\Resources\\ActivitiesMetadata.json", content);


static string GetPropertyPrefix(string activityName, string propertyName)
{
    switch (propertyName)
    {
        case "SearchPattern":
        case "ContinueOnError":
        case "TimeoutSeconds":
            activityName = "Common";
            break;
    }
    return $"{activityName}_{propertyName}";
}


public class Activity
{
    public string FullName { get; set; }
    public string ShortName { get; set; }
    public string DisplayNameKey { get; set; }
    public string DescriptionKey { get; set; }
    public string IconKey { get; set; }
    public string ViewModelType { get; set; }
    public string CategoryKey
    {
        get
        {
            return ShortName switch
            {
                "WaitFile" or "CleanUpFolder" or "Zip" or "Unzip" => "Autossential.Files",
                "RandomString" => "Autossential.Programming",
                _ => "Autossential.Misc"
            };
        }
    }
    public List<Property> Properties { get; set; }
}

public class Category
{
    public string Name { get; set; }
    public string DisplayNameKey { get; set; }
}

public class Property
{
    public string Name { get; set; }
    public string DisplayNameKey { get; set; }
    public string TooltipKey { get; set; }
    // public bool IsRequired { get; set; }
    // public bool IsPrincipal { get; set; }
    public bool IsVisible { get; set; }
}

public class Root
{
    public string AssemblyRelativePath { get; set; }
    public List<Activity> Activities { get; set; }
    public string DefaultActivityColor { get; set; }
    public string DefaultActivityNameBackgroundColor { get; set; }
    public string AssemblyIconKey { get; set; }
    public string ResourceManagerName { get; set; }
}

