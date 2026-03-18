using System.Collections;
using System.Globalization;

namespace Autossential.Activities.Models
{
    // ─────────────────────────────────────────────────────────────────────────
    //  DataNode
    //
    //  A normalized, immutable wrapper around any value coming from YAML/JSON
    //  parsers or user-supplied objects.
    //
    //  Internal canonical types (guaranteed after construction):
    //    Scalar   → string | null | any primitive (int, bool, double, …)
    //    Sequence → List<object>  (elements are recursively normalized)
    //    Map      → Dictionary<string, object>  (values are recursively normalized)
    //
    //  Key-path syntax for navigation methods:
    //    "person.address.city"          — dot-separated map keys
    //    "servers[0].host"              — index into a sequence
    //    "servers[0].ports[1]"          — chained indexes
    //    "metrics['error.rate'].value"  — quoted key (for keys that contain dots)
    // ─────────────────────────────────────────────────────────────────────────
    public sealed class DataNode
    {
        // ── Internal state ────────────────────────────────────────────────────

        private readonly object _value;

        public NodeType Type { get; }
        public bool HasValue => _value is not null;

        // ── Construction ──────────────────────────────────────────────────────

        public DataNode(object value, CultureInfo culture = null)
        {
            (_value, Type) = Normalize(value);
            Culture = culture ?? CultureInfo.InvariantCulture;
        }

        // ── Normalization ─────────────────────────────────────────────────────

        private (object normalized, NodeType type) Normalize(object value)
        {
            object NormalizeChild(object value) => new DataNode(value, Culture)._value;

            if (value is null || value is string)
                return (value, NodeType.Scalar);

            if (value is DataNode dn)
                return (dn._value, dn.Type);  // unwrap nested DataNode

            // Map: any IDictionary → Dictionary<string, object>
            if (value is IDictionary dict)
            {
                var map = new Dictionary<string, object>(StringComparer.Ordinal);
                foreach (DictionaryEntry entry in dict)
                    map[entry.Key.ToString()!] = NormalizeChild(entry.Value);
                return (map, NodeType.Map);
            }

            // Sequence: any IEnumerable (except string, already handled) → List<object>
            if (value is IEnumerable enumerable)
            {
                var list = enumerable.Cast<object>().Select(NormalizeChild).ToList();
                return (list, NodeType.Sequence);
            }

            // Scalar: primitives, DateTime, Guid, etc.
            return (value, NodeType.Scalar);
        }

        // ── Raw access ────────────────────────────────────────────────────────

        /// <summary>Returns the raw internal value without any conversion.</summary>
        public object RawValue => _value;

        public CultureInfo Culture { get; }

        /// <summary>Returns the internal map. Throws if Type is not Map.</summary>
        public Dictionary<string, object> AsDictionary()
        {
            EnsureType(NodeType.Map);
            return (Dictionary<string, object>)_value;
        }

        /// <summary>Returns the internal sequence. Throws if Type is not Sequence.</summary>
        public List<object> AsList()
        {
            EnsureType(NodeType.Sequence);
            return (List<object>)_value;
        }

        // ── Scalar accessors (no key path) ────────────────────────────────────

        /// <summary>Returns the scalar value as string, or null if HasValue is false.</summary>
        public string AsString() => _value?.ToString();

        /// <summary>Returns the scalar value as string, or <paramref name="defaultValue"/> if null.</summary>
        public string AsStringOrDefault(string defaultValue) => _value is not null ? _value.ToString()! : defaultValue;
        public int AsInt(int defaultValue = 0) => TryConvert(_value, defaultValue, v => Convert.ToInt32(v, Culture));
        public long AsLong(long defaultValue = 0L) => TryConvert(_value, defaultValue, v => Convert.ToInt64(v, Culture));
        public double AsDouble(double defaultValue = 0d) => TryConvert(_value, defaultValue, v => Convert.ToDouble(v, Culture));
        public bool AsBool(bool defaultValue = false) => TryConvert(_value, defaultValue, ParseBool);
        public DateTime AsDateTime(DateTime defaultValue = default) => TryConvert(_value, defaultValue, v => Convert.ToDateTime(v, Culture));
        public decimal AsDecimal(decimal defaultValue = 0m) => TryConvert(_value, defaultValue, v => Convert.ToDecimal(v, Culture));

        // ── Key-path navigation ───────────────────────────────────────────────

        /// <summary>
        /// Navigates to the node at <paramref name="keyPath"/> and returns a DataNode.
        /// Returns an empty Scalar DataNode (null value) if the path does not exist.
        /// </summary>
        internal DataNode Navigate(string keyPath)
        {
            object current = _value;
            foreach (PathSegment seg in ParsePath(keyPath))
            {
                if (current is null) return new DataNode(null);
                current = seg.IsIndex
                    ? ResolveIndex(current, seg.Index, keyPath)
                    : ResolveKey(current, seg.Key, keyPath);
            }
            return new DataNode(current);
        }

        // Typed accessors with key-path ───────────────────────────────────────

        /// <summary>Navigate to <paramref name="keyPath"/> and return value as string.</summary>
        public string AsString(string keyPath, string defaultValue = null) =>
            Navigate(keyPath).AsStringOrDefault(defaultValue);

        public int AsInt(string keyPath, int defaultValue = 0) =>
            Navigate(keyPath).AsInt(defaultValue);

        public long AsLong(string keyPath, long defaultValue = 0L) =>
            Navigate(keyPath).AsLong(defaultValue);

        public double AsDouble(string keyPath, double defaultValue = 0d) =>
            Navigate(keyPath).AsDouble(defaultValue);

        public bool AsBool(string keyPath, bool defaultValue = false) =>
            Navigate(keyPath).AsBool(defaultValue);

        public DateTime AsDateTime(string keyPath, DateTime defaultValue = default) =>
            Navigate(keyPath).AsDateTime(defaultValue);
        public decimal AsDecimal(string keyPath, decimal defaultValue = 0) =>
            Navigate(keyPath).AsDecimal(defaultValue);

        /// <summary>
        /// Navigate to <paramref name="keyPath"/> and return the child node as a DataNode.
        /// Useful when the target is a Map or Sequence that you want to keep navigating.
        /// </summary>
        public DataNode Get(string keyPath) => Navigate(keyPath);

        /// <summary>
        /// Navigate to <paramref name="keyPath"/> and return all children as a list of DataNodes.
        /// Returns an empty list if the path does not exist or is not a Sequence.
        /// </summary>
        public List<DataNode> GetSequence(string keyPath)
        {
            DataNode node = Navigate(keyPath);
            if (node.Type != NodeType.Sequence) return [];
            return [.. node.AsList().Select(item => new DataNode(item))];
        }

        /// <summary>
        /// Returns whether the given key path exists and has a non-null value.
        /// </summary>
        public bool Exists(string keyPath)
        {
            try { return Navigate(keyPath).HasValue; }
            catch { return false; }
        }

        // ── Path parsing ──────────────────────────────────────────────────────

        private record PathSegment(bool IsIndex, string Key = null, int Index = 0);

        // Supports:
        //   "person.address.city"        → ["person", "address", "city"]
        //   "servers[0].host"            → ["servers", 0, "host"]
        //   "metrics['error.rate'].value"→ ["metrics", "error.rate", "value"]
        //   "metrics[\"error.rate\"]"    → ["metrics", "error.rate"]
        private static IEnumerable<PathSegment> ParsePath(string keyPath)
        {
            if (string.IsNullOrEmpty(keyPath)) 
                yield break;

            // Tokenize: split on '.' but respect [brackets] and quoted keys
            var segments = new List<PathSegment>();
            int i = 0;
            while (i < keyPath.Length)
            {
                if (keyPath[i] == '.') { i++; continue; }

                // Bracket index or quoted key: [0]  ['key']  ["key"]
                if (keyPath[i] == '[')
                {
                    int close = keyPath.IndexOf(']', i);
                    if (close < 0) throw new ArgumentException($"Unclosed '[' in key path: {keyPath}");
                    string inner = keyPath[(i + 1)..close].Trim();

                    if (inner.StartsWith('\'') || inner.StartsWith('"'))
                    {
                        // Quoted string key
                        string key = inner[1..^1];
                        segments.Add(new PathSegment(false, Key: key));
                    }
                    else if (int.TryParse(inner, out int idx))
                    {
                        segments.Add(new PathSegment(true, Index: idx));
                    }
                    else
                    {
                        // Treat as string key without quotes
                        segments.Add(new PathSegment(false, Key: inner));
                    }
                    i = close + 1;
                    continue;
                }

                // Plain key: read until '.' or '['
                int end = i;
                while (end < keyPath.Length && keyPath[end] != '.' && keyPath[end] != '[') end++;
                string plainKey = keyPath[i..end];
                if (!string.IsNullOrEmpty(plainKey))
                    segments.Add(new PathSegment(false, Key: plainKey));
                i = end;
            }

            foreach (var seg in segments)
                yield return seg;
        }

        // ── Resolution helpers ────────────────────────────────────────────────

        private static object ResolveKey(object current, string key, string keyPath)
        {
            if (current is Dictionary<string, object> map)
                return map.TryGetValue(key, out object val) ? val : null;

            throw new InvalidOperationException(
                $"Expected a Map at segment '{key}' in path '{keyPath}', " +
                $"but found {current.GetType().Name}.");
        }

        private static object ResolveIndex(object current, int index, string keyPath)
        {
            if (current is List<object> list)
            {
                if (index < 0 || index >= list.Count) return null;
                return list[index];
            }

            throw new InvalidOperationException(
                $"Expected a Sequence at index [{index}] in path '{keyPath}', " +
                $"but found {current.GetType().Name}.");
        }

        // ── Type conversion helpers ───────────────────────────────────────────

        private static T TryConvert<T>(object value, T defaultValue, Func<object, T> converter)
        {
            if (value is null) return defaultValue;
            try { return converter(value); }
            catch { return defaultValue; }
        }

        private static bool ParseBool(object value)
        {
            if (value is bool b)
                return b;

            return value.ToString()!.ToLowerInvariant() switch
            {
                "true" or "yes" or "on" or "1" => true,
                "false" or "no" or "off" or "0" => false,
                _ => throw new FormatException($"Cannot convert '{value}' to bool.")
            };
        }

        // ── Guard ─────────────────────────────────────────────────────────────

        private void EnsureType(NodeType expected)
        {
            if (Type != expected)
                throw new InvalidOperationException(
                    $"Expected NodeType.{expected} but node is NodeType.{Type}.");
        }

        // ── Equality & display ────────────────────────────────────────────────

        public override string ToString() => Type switch
        {
            NodeType.Scalar => _value?.ToString() ?? "(null)",
            NodeType.Sequence => $"[Sequence, {((List<object>)_value).Count} items]",
            NodeType.Map => $"[Map, {((Dictionary<string, object>)_value).Count} keys]",
            _ => "(unknown)"
        };
    }
}