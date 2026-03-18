namespace Autossential.Activities.Models
{
    public enum NodeType
    {
        Scalar,
        Sequence,
        Map
    }
}



/*public class DataNode
{
    public NodeType Type { get; }
    private readonly object _value;

    public DataNode(object value)
    {
        (_value, Type) = Normalize(value);
    }

    private static (object, NodeType) Normalize(object value)
    {
        // ── Scalar ───────────────────────────────────────────────────────
        if (value is null)   return (null, NodeType.Scalar);
        if (value is string) return (value, NodeType.Scalar);

        var type = value.GetType();

        // ── Map: qualquer IDictionary vira Dictionary<string, object> ────
        if (value is IDictionary dict)
        {
            var map = new Dictionary<string, object>(StringComparer.Ordinal);
            foreach (DictionaryEntry entry in dict)
                map[entry.Key.ToString()] = NormalizeValue(entry.Value);
            return (map, NodeType.Map);
        }

        // ── Sequence: qualquer IEnumerable vira List<object> ─────────────
        if (value is IEnumerable enumerable)
        {
            var list = enumerable.Cast<object>()
                                 .Select(NormalizeValue)
                                 .ToList();
            return (list, NodeType.Sequence);
        }

        // ── Scalar: tipos primitivos e qualquer outro valor atômico ──────
        return (value, NodeType.Scalar);
    }

    // Normaliza recursivamente os valores internos das coleções
    private static object NormalizeValue(object value) =>
        new DataNode(value)._value;
}*/