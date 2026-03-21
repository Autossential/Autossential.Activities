using System.Activities.Statements;
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
        /// <summary>
        /// Gets the type of the node represented by this instance.
        /// </summary>
        public NodeType Type { get; private set; }

        private object _value;
        /// <summary>
        /// Returns the raw internal value without any conversion.
        /// </summary>
        public object RawValue
        {
            get => _value;
            private set
            {
                _value = value;

                if (value is null || value is string)
                {
                    Type = NodeType.Scalar;
                    return;
                }

                if (value is IDictionary)
                {
                    Type = NodeType.Map;
                    return;
                }

                if (value is IEnumerable)
                {
                    Type = NodeType.Sequence;
                    return;
                }

                Type = NodeType.Scalar;
            }
        }

        /// <summary>
        /// Gets the culture information associated with the current context.
        /// </summary>
        public CultureInfo Culture { get; }

        /// <summary>
        /// Initializes a new instance of the DataNode class with the specified value and culture information.
        /// </summary>
        /// <param name="value">The value to be stored in the data node. Can be of any type supported by the node.</param>
        /// <param name="culture">The culture information to use for formatting and parsing operations. If null, the invariant culture is
        /// used.</param>
        public DataNode(object value, CultureInfo culture = null)
        {
            RawValue = Normalize(value);
            Culture = culture ?? CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Initializes a new instance of the DataNode class with default values.
        /// </summary>
        /// <remarks>This constructor creates a DataNode using an empty dictionary and the invariant
        /// culture. It is equivalent to calling the main constructor with default parameters.</remarks>
        public DataNode() : this(new Dictionary<string, object>(), CultureInfo.InvariantCulture)
        {

        }

        /// <summary>
        /// Creates an empty DataNode instance with the specified culture information.
        /// </summary>
        /// <param name="culture">The culture to associate with the DataNode. If null, the default culture is used.</param>
        /// <returns>A DataNode instance that contains no data and is associated with the specified culture.</returns>
        public static DataNode Empty(CultureInfo culture = null) => new(new Dictionary<string, object>(), culture);


        private object Normalize(object value)
        {
            object NormalizeChild(object value) => new DataNode(value, Culture).RawValue;

            if (value is null || value is string) return value;
            if (value is DataNode dn) return dn.RawValue;

            // Map: any IDictionary -> Dictionary<string, object>
            if (value is IDictionary dict)
            {
                var map = new Dictionary<string, object>(StringComparer.Ordinal);
                foreach (DictionaryEntry entry in dict)
                    map[entry.Key.ToString()!] = NormalizeChild(entry.Value);

                return map;
            }

            // Sequence: any IEnumerable (except string, already handled) -> List<object>
            if (value is IEnumerable enumerable)
                return enumerable.Cast<object>().Select(NormalizeChild).ToList();

            // Scalar: primitives, DateTime, Guid, etc.
            return value;
        }

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

        /// <summary>
        /// Returns whether the given key path exists
        /// </summary>
        public bool Exists(string keyPath)
        {
            try
            {
                var segments = ParsePath(keyPath).ToList();
                if (segments.Count == 0) return false;

                object current = RawValue;
                foreach (var seg in segments.Take(segments.Count - 1))
                {
                    if (current is null) return false;
                    current = seg.IsIndex
                        ? ResolveIndex(current, seg.Index, keyPath)
                        : ResolveKey(current, seg.Key, keyPath);
                }

                var last = segments[^1];
                if (last.IsIndex)
                    return current is List<object> list && last.Index < list.Count;
                else
                    return current is Dictionary<string, object> map && map.ContainsKey(last.Key);
            }
            catch { return false; }
        }



        /// <summary>
        /// Gets an enumerable collection containing the keys of the map node.
        /// </summary>
        /// <remarks>If the node is not of type Map, the collection is empty. The order of the keys is not
        /// guaranteed.</remarks>
        public IEnumerable<string> Keys => Type == NodeType.Map ? ((Dictionary<string, object>)RawValue).Keys : [];

        /// <summary>
        /// Determines whether the current instance contains a non-null value.
        /// </summary>
        /// <returns>true if the underlying value is not null; otherwise, false.</returns>
        public bool HasValue() => RawValue is not null;

        /// <summary>
        /// Determines whether the specified key path exists and has an associated value.
        /// </summary>
        /// <param name="keyPath">The hierarchical path of the key to check for a value. Cannot be null or empty.</param>
        /// <returns>true if the key path exists and has a value; otherwise, false.</returns>
        public bool HasValue(string keyPath) => GetNode(keyPath).HasValue();

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

                var plainKey = keyPath[i..end];
                if (!string.IsNullOrEmpty(plainKey))
                    segments.Add(new PathSegment(false, Key: plainKey));

                i = end;
            }

            foreach (var seg in segments)
                yield return seg;
        }

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
                "true" or "1" => true,
                "false" or "0" => false,
                _ => throw new FormatException($"Cannot convert '{value}' to bool.")
            };
        }

        private void EnsureType(NodeType expected)
        {
            if (Type != expected)
                throw new InvalidOperationException(
                    $"Expected NodeType.{expected} but node is NodeType.{Type}.");
        }

        public override string ToString() => Type switch
        {
            NodeType.Scalar => RawValue?.ToString() ?? "(null)",
            NodeType.Sequence => $"[Sequence, {((List<object>)RawValue).Count} items]",
            NodeType.Map => $"[Map, {((Dictionary<string, object>)RawValue).Count} keys]",
            _ => "(unknown)"
        };

        /// <summary>
        /// Merges another DataNode into the current instance.
        /// Scalar and Sequence values from <paramref name="other"/> overwrite existing ones.
        /// Map values are merged recursively.
        /// </summary>
        /// <param name="other">The DataNode to merge into the current instance.</param>
        public void Merge(DataNode other)
        {
            EnsureType(NodeType.Map);

            if (other is null || !other.HasValue() || other.Type != NodeType.Map)
                return;

            MergeMaps(
                (Dictionary<string, object>)RawValue,
                (Dictionary<string, object>)other.RawValue
            );
        }

        private static void MergeMaps(Dictionary<string, object> target, Dictionary<string, object> source)
        {
            foreach (var (key, sourceValue) in source)
            {
                // Se ambos são Map, merge recursivo
                if (target.TryGetValue(key, out object targetValue)
                    && targetValue is Dictionary<string, object> targetMap
                    && sourceValue is Dictionary<string, object> sourceMap)
                {
                    MergeMaps(targetMap, sourceMap);
                }
                else
                {
                    // Scalar ou Sequence: source sobrescreve target
                    target[key] = sourceValue;
                }
            }
        }

        public DataNode this[string keyPath]
        {
            get => GetNode(keyPath);
            set
            {
                EnsureType(NodeType.Map);

                var segments = ParsePath(keyPath).ToList();
                object current = RawValue;

                for (int i = 0; i < segments.Count - 1; i++)
                {
                    var seg = segments[i];
                    if (seg.IsIndex)
                        current = ((List<object>)current)[seg.Index];
                    else
                    {
                        var map = (Dictionary<string, object>)current;
                        if (!map.ContainsKey(seg.Key))
                            map[seg.Key] = new Dictionary<string, object>();
                        current = map[seg.Key];
                    }
                }

                var last = segments[^1];
                if (last.IsIndex)
                    ((List<object>)current)[last.Index] = value?.RawValue;
                else
                    ((Dictionary<string, object>)current)[last.Key] = value?.RawValue;
            }
        }

        /// <summary>Returns the internal map. Throws if Type is not Map.</summary>
        public Dictionary<string, object> AsMap()
        {
            EnsureType(NodeType.Map);
            return (Dictionary<string, object>)RawValue;
        }

        /// <summary>Returns the internal sequence. Throws if Type is not Sequence.</summary>
        public List<object> AsSequence()
        {
            EnsureType(NodeType.Sequence);
            return (List<object>)RawValue;
        }

        /// <summary>
        /// Navigates to the node at <paramref name="keyPath"/> and returns a DataNode.
        /// Returns an empty Scalar DataNode (null value) if the path does not exist.
        /// </summary>
        public DataNode GetNode(string keyPath)
        {
            object current = RawValue;
            foreach (PathSegment seg in ParsePath(keyPath))
            {
                if (current is null) return new DataNode(null, Culture);
                current = seg.IsIndex
                    ? ResolveIndex(current, seg.Index, keyPath)
                    : ResolveKey(current, seg.Key, keyPath);
            }
            return new DataNode(current, Culture);
        }

        /// <summary>
        /// Navigate to <paramref name="keyPath"/> and return all children as a list of DataNodes.
        /// Returns an empty list if the path does not exist or is not a Sequence.
        /// </summary>
        public List<DataNode> GetSequenceNode(string keyPath)
        {
            DataNode node = GetNode(keyPath);
            if (node.Type != NodeType.Sequence) return [];
            return [.. node.AsSequence().Select(item => new DataNode(item, Culture))];
        }

        public string AsString() => RawValue?.ToString();
        public string AsString(string keyPath) => GetNode(keyPath).AsString();
        public string AsStringOrDefault(string defaultValue) => RawValue is not null ? RawValue.ToString()! : defaultValue;
        public string AsStringOrDefault(string keyPath, string defaultValue) => GetNode(keyPath).AsStringOrDefault(defaultValue);

        public int AsInt() => Convert.ToInt32(RawValue, Culture);
        public int AsInt(string keyPath) => GetNode(keyPath).AsInt();
        public int AsIntOrDefault(int defaultValue) => TryConvert(RawValue, defaultValue, v => Convert.ToInt32(v, Culture));
        public int AsIntOrDefault(string keyPath, int defaultValue) => GetNode(keyPath).AsIntOrDefault(defaultValue);

        public long AsLong() => Convert.ToInt64(RawValue, Culture);
        public long AsLong(string keyPath) => GetNode(keyPath).AsLong();
        public long AsLongOrDefault(long defaultValue) => TryConvert(RawValue, defaultValue, v => Convert.ToInt64(v, Culture));
        public long AsLongOrDefault(string keyPath, long defaultValue) => GetNode(keyPath).AsLongOrDefault(defaultValue);

        public double AsDouble() => Convert.ToDouble(RawValue, Culture);
        public double AsDouble(string keyPath) => GetNode(keyPath).AsDouble();
        public double AsDoubleOrDefault(double defaultValue) => TryConvert(RawValue, defaultValue, v => Convert.ToDouble(v, Culture));
        public double AsDoubleOrDefault(string keyPath, double defaultValue) => GetNode(keyPath).AsDoubleOrDefault(defaultValue);

        public decimal AsDecimal() => Convert.ToDecimal(RawValue, Culture);
        public decimal AsDecimal(string keyPath) => GetNode(keyPath).AsDecimal();
        public decimal AsDecimalOrDefault(decimal defaultValue) => TryConvert(RawValue, defaultValue, v => Convert.ToDecimal(v, Culture));
        public decimal AsDecimalOrDefault(string keyPath, decimal defaultValue) => GetNode(keyPath).AsDecimalOrDefault(defaultValue);

        public DateTime AsDateTime() => Convert.ToDateTime(RawValue, Culture);
        public DateTime AsDateTime(string keyPath) => GetNode(keyPath).AsDateTime();
        public DateTime AsDateTimeOrDefault(DateTime defaultValue) => TryConvert(RawValue, defaultValue, v => Convert.ToDateTime(v, Culture));
        public DateTime AsDateTimeOrDefault(string keyPath, DateTime defaultValue) => GetNode(keyPath).AsDateTimeOrDefault(defaultValue);

        public bool AsBool() => ParseBool(RawValue);
        public bool AsBool(string keyPath) => GetNode(keyPath).AsBool();
        public bool AsBoolOrDefault(bool defaultValue) => TryConvert(RawValue, defaultValue, ParseBool);
        public bool AsBoolOrDefault(string keyPath, bool defaultValue) => GetNode(keyPath).AsBoolOrDefault(defaultValue);
    }
}