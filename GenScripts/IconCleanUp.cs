var icons = Directory.GetFiles("..\\Autossential.Activities\\Resources\\Icons", "*.svg", SearchOption.AllDirectories);

foreach (var icon in icons)
{
    var svg = File.ReadAllText(icon);
    if (svg.Contains("<?xml"))
    {
        svg = svg.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>", "")
                .Replace("<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">", "").Trim();

        File.WriteAllText(icon, svg);
        Console.WriteLine(icon);
    }
}
