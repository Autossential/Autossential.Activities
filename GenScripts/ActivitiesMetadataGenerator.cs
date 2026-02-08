#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code

#:package UiPath.Workflow@6.0.0-20240401-07
#:package UiPath.Activities.Api@24.10.1
#:package System.Activities.ViewModels@1.0.0-20250625.2
#:project D:\Development\Autossential-4\Autossential.Activities\Autossential.Activities.csproj

using System.Reflection;

using var sw = new StreamWriter("..\\Autossential.Activities\\Resources\\ActivitiesMetadata.json");

sw.Write($$"""
{
    "resourceManagerName": "Autossential.Activities.Properties.Resources",
    "activities": [
        {{BuildSchema()}}
    ]
}
""");

static string BuildSchema()
{
    var types = Assembly.Load("Autossential.Activities").GetTypes()
        .Where(t => 
            t.IsClass
            && t.IsPublic
            && typeof(System.Activities.Activity).IsAssignableFrom(t)
            && !t.IsAbstract
        );

    var files = Directory.GetFiles("..\\Autossential.Activities", "*.cs", SearchOption.AllDirectories);
    
    return string.Join(",\n", types.Select(t =>
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
        if (fileLocation == null)
            return "";

        var parts = fileLocation.Trim('.', '\\').Split("\\");
        var category = parts[^2];
        return $$"""
        {
                    "fullName": "{{t.FullName}}",
                    "shortName": "{{name}}",
                    "categoryKey": "Autossential.{{category}}",
                    "iconKey": "{{name}}.svg",
                    "viewModelType": "Autossential.Activities.ViewModels.{{name}}ViewModel{{suffix}}",
                    "displayNameKey": "{{name}}_DisplayName",
                    "descriptionKey": "{{name}}_Description"
                }
        """;
    }));
}