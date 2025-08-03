#r "D:\Users\alexa\source\Autossential_ORG\Autossential.Activities\source\Autossential.Activities\bin\Debug\net6.0-windows\Autossential.Activities.dll"
#r "D:\Users\alexa\source\Autossential_ORG\Autossential.Activities\source\Autossential.Core\bin\Debug\net6.0\Autossential.Core.dll"
#r "D:\Users\alexa\.nuget\packages\system.activities\5.0.0-20210730-02\lib\net5.0-windows7.0\System.Activities.dll"
#r "D:\Users\alexa\.nuget\packages\system.activities.viewmodels\1.0.0-20250625.2\lib\net6.0-windows7.0\System.Activities.ViewModels.dll"

using Autossential.Activities;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Activities;

System.Diagnostics.Debugger.Launch();

using (var sw = new StreamWriter("ActivitiesMetadata.json"))
{
    var types = typeof(MapDrive).Assembly.GetTypes().Where(t =>
        t.IsClass
        && t.IsPublic
        && typeof(Activity).IsAssignableFrom(t)
        && !t.IsAbstract);

    sw.Write($$"""
    {
      "resourceManagerName": "Autossential.Activities.Properties.Resources",
      "activities": [
        {{BuildSchema()}}
      ]
    }
    """);

}

string BuildSchema()
{
    var sw = new StringWriter();

    var types = typeof(MapDrive).Assembly.GetTypes().Where(t =>
        t.IsClass
        && t.IsPublic
        && typeof(Activity).IsAssignableFrom(t)
        && !t.IsAbstract);

    var files = Directory.GetFiles("..\\", "*.cs", SearchOption.AllDirectories);

    return string.Join(",\n", types.Select(type =>
    {
        var name = type.Name;
        var index = name.IndexOf("`");
        var suffix = "";
        if (index > 0)
        {
            suffix = name.Substring(index);
            name = name.Substring(0, index);
        }

        var fileLocation = files.FirstOrDefault(f => f.EndsWith($"{name}.cs"));
        if (fileLocation == null)
            return "";

        var parts = fileLocation.Trim('.', '\\').Split("\\");
        var category = string.Join(".", parts[..^1]);
        return $$"""
            {
              "fullName": "{{type.FullName}}",
              "shortName": "{{name}}",
              "categoryKey": "Autossential.{{category}}",
              "iconKey": "{{name}}.svg",
              "viewModelType": "Autossential.Activities.ViewModels.{{category}}.{{name}}ViewModel{{suffix}}",
              "displayNameKey": "{{name}}_DisplayName",
              "descriptionKey": "{{name}}_Description"
            }
        """;
    }));
}