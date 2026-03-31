#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8601 // Possible null reference assignment.

#:package UiPath.Workflow@6.0.0-20240401-07
#:package UiPath.Activities.Api@24.10.1
#:package System.Activities.ViewModels@1.20260216.2
#:package System.Activities.Metadata@6.0.0-20240517.13
#:project D:\Development\Autossential-4\Autossential.Activities\Autossential.Activities.csproj

using System.Activities;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;


// <?xml version="1.0" encoding="UTF-8" standalone="no"?>
// <!DOCTYPE svg PUBLIC "-//W3C//DTD SVG 1.1//EN" "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd">

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
        if (p.Name == "Id" || p.Name == "DisplayName")
            continue;

        var browsable = p.GetCustomAttribute<BrowsableAttribute>();
        if (browsable != null && !browsable.Browsable)
            continue;  

        var propPrefix = GetPropertyPrefix(name, p.Name);

        var requiredAttr = p.GetCustomAttribute<RequiredArgumentAttribute>();
        var property = new Property(p.PropertyType, t)
        {
            Name = p.Name,
            DisplayNameKey = $"{propPrefix}_DisplayName",
            TooltipKey = $"{propPrefix}_Description",
            IsRequired = requiredAttr != null,
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
                "WaitFile" or
                "CleanUpFolder" or
                "Zip" or
                "Unzip" => "ActivityCategories_Files",

                "RandomString" or
                "Increment" or
                "Decrement" or
                "ReplaceTokens" or
                "ParseData" or
                "LoadDataFile" or
                "MergeData" or
                "CultureScope" => "ActivityCategories_Programming",

                "IfActivity" or
                "TimeLoop" or
                "Container" or
                "CheckPoint" or
                "Exit" => "ActivityCategories_Workflow",

                "PromoteHeaders" or
                "RemoveEmptyRows" or
                "RemoveDataColumns" or
                "TransposeData" or
                "AddRangeToCollection" or
                "UpdateDictionary" or
                "DataTableToText" => "ActivityCategories_Data",

                _ => "ActivityCategories_System"
            };
        }
    }
    public List<Property> Properties { get; set; }
}

public class Category(string name, string displayNameKey)
{
    public string Name { get; set; } = name;
    public string DisplayNameKey { get; set; } = displayNameKey;
}

public class Property
{
    private readonly Type _activityType;

    public Property(Type propType, Type activityType)
    {
        if (propType == typeof(InArgument) || (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(InArgument<>)))
        {
            Category = new Category("Input", "Categories_Input");
        }
        else if (propType == typeof(OutArgument) || (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(OutArgument<>)))
        {
            Category = new Category("Output", "Categories_Output");
        }
        else if (propType == typeof(InOutArgument) || (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(InOutArgument<>)))
        {
            Category = new Category("InputOutput", "Categories_InputOutput");
        }
        else
        {
            Category = new Category("Options", "Categories_Options");
        }

        _activityType = activityType;
    }
    public string Name { get; set; }
    public string DisplayNameKey { get; set; }
    public string TooltipKey { get; set; }
    public bool IsVisible { get; set; }
    public bool IsRequired { get; set; }
    public Category Category { get; }
    // public bool IsPrincipal
    // {
    //     get
    //     {
    //         return _activityType.Name == "CultureScope" && Name == "CultureName";
    //     }
    // }
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

