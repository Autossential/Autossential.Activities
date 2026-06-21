using Autossential.Activities.Core;

public class JsonParserTests
{
    [Test]
    public async Task Parse_ObjectWithScalars_ReturnsDictionary()
    {
        string json = @"{ ""a"": ""text"", ""b"": 123, ""c"": true, ""d"": false, ""e"": null }";
        var result = (Dictionary<string, object>)JsonParser.Parse(json);

        await Assert.That(result["a"]).IsEqualTo("text");
        await Assert.That(result["b"]).IsEqualTo("123"); // number como string
        await Assert.That(result["c"]).IsEqualTo("true");
        await Assert.That(result["d"]).IsEqualTo("false");
        await Assert.That(result["e"]).IsNull();
    }

    [Test]
    public async Task Parse_ArrayWithMixedTypes_ReturnsList()
    {
        string json = @"[ ""x"", 42, null, true ]";
        var result = (List<object>)JsonParser.Parse(json);

        await Assert.That(result).IsEquivalentTo(new object[] { "x", "42", null, "true" });
    }

    [Test]
    public async Task Parse_NestedObjectAndArray_ReturnsRecursiveStructure()
    {
        string json = @"{ ""arr"": [ { ""k"": ""v"" }, 99 ] }";
        var result = (Dictionary<string, object>)JsonParser.Parse(json);

        var arr = (List<object>)result["arr"];
        var nested = (Dictionary<string, object>)arr[0];

        await Assert.That(nested["k"]).IsEqualTo("v");
        await Assert.That(arr[1]).IsEqualTo("99");
    }

    [Test]
    public async Task Parse_StringScalar_ReturnsString()
    {
        string json = @"""hello""";
        var result = JsonParser.Parse(json);
        await Assert.That(result).IsEqualTo("hello");
    }

    [Test]
    public async Task Parse_NumberScalar_ReturnsRawText()
    {
        string json = "123.45";
        var result = JsonParser.Parse(json);
        await Assert.That(result).IsEqualTo("123.45");
    }

    [Test]
    public async Task Parse_BooleanScalars_ReturnsStringTrueFalse()
    {
        await Assert.That(JsonParser.Parse("true")).IsEqualTo("true");
        await Assert.That(JsonParser.Parse("false")).IsEqualTo("false");
    }

    [Test]
    public async Task Parse_NullScalar_ReturnsNull()
    {
        await Assert.That(JsonParser.Parse("null")).IsNull();
    }

    [Test]
    public async Task Parse_WithCommentsAndTrailingCommas_IgnoresThem()
    {
        string json = @"{
        // comentário
        ""a"": [1,2,3,],
    }";
        var result = (Dictionary<string, object>)JsonParser.Parse(json);
        var arr = (List<object>)result["a"];

        await Assert.That(arr).IsEquivalentTo(new[] { "1", "2", "3" });
    }
}
