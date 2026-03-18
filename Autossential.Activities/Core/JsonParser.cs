using System.Text.Json;

namespace Autossential.Activities.Core
{
    // ─────────────────────────────────────────────────────────────────────────
    //  Numbers and booleans are returned as string to match YAML behaviour.
    //  If you want typed scalars, see the note in ConvertScalar below.
    // ─────────────────────────────────────────────────────────────────────────
    public static class JsonParser
    {
        private static readonly JsonDocumentOptions _options = new()
        {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip,
        };

        public static object Parse(string input)
        {
            using JsonDocument doc = JsonDocument.Parse(input, _options);
            return Convert(doc.RootElement);
        }

        // ── Recursive converter ───────────────────────────────────────────────

        private static object Convert(JsonElement el) => el.ValueKind switch
        {
            JsonValueKind.Object => ConvertObject(el),
            JsonValueKind.Array => ConvertArray(el),
            _ => ConvertScalar(el),
        };

        private static Dictionary<string, object> ConvertObject(JsonElement el)
        {
            var map = new Dictionary<string, object>(StringComparer.Ordinal);
            foreach (JsonProperty prop in el.EnumerateObject())
                map[prop.Name] = Convert(prop.Value);
            return map;
        }

        private static List<object> ConvertArray(JsonElement el)
        {
            var list = new List<object>(el.GetArrayLength());
            foreach (JsonElement item in el.EnumerateArray())
                list.Add(Convert(item));
            return list;
        }

        // Returns string (or null) to stay consistent with YamlParser,
        // where all unquoted scalars are plain strings.
        // If typed scalars are wanted in future, mirror the logic from
        // YamlParser.ParseScalar here — the ValueKind makes it trivial:
        //
        //   JsonValueKind.True/False  → bool
        //   JsonValueKind.Number      → el.TryGetInt64 / el.GetDouble()
        //
        private static object ConvertScalar(JsonElement el) => el.ValueKind switch
        {
            JsonValueKind.Null => null,
            JsonValueKind.True => "true",
            JsonValueKind.False => "false",
            JsonValueKind.Number => el.GetRawText(),
            JsonValueKind.String => el.GetString(),
            _ => el.GetRawText(),
        };
    }
}
