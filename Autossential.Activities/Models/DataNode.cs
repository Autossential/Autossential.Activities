using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;

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
    public sealed partial class DataNode
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
        /// Determines whether a node exists at the specified key path within the current data structure.
        /// </summary>
        /// <remarks>This method returns false if the path is invalid, the node does not exist, or an
        /// error occurs during traversal. The key path can reference nested dictionary keys or list indices as
        /// supported by the underlying data structure.</remarks>
        /// <param name="keyPath">The path to the node to check for existence. The path should use the appropriate format for addressing
        /// nested elements or indices.</param>
        /// <returns>true if a node exists at the specified key path; otherwise, false.</returns>
        public bool HasNode(string keyPath)
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
                            map[seg.Key] = new Dictionary<string, object>(StringComparer.Ordinal);
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

        private T TryConvert<T>(Func<T> converter, T defaultValue)
        {
            if (RawValue is null)
                return defaultValue;

            try { return converter(); }
            catch { return defaultValue; }
        }

        public string AsString() => RawValue?.ToString();
        public string AsStringOrDefault(string defaultValue) => RawValue is null ? defaultValue : AsString();
        public int AsInt() => Convert.ToInt32(RawValue, Culture);
        public int AsIntOrDefault(int defaultValue) => TryConvert(AsInt, defaultValue);
        public long AsLong() => Convert.ToInt64(RawValue, Culture);
        public long AsLongOrDefault(long defaultValue) => TryConvert(AsLong, defaultValue);
        public float AsFloat() => Convert.ToSingle(RawValue, Culture);
        public float AsFloatOrDefault(float defaultValue) => TryConvert(AsFloat, defaultValue);
        public double AsDouble() => Convert.ToDouble(RawValue, Culture);
        public double AsDoubleOrDefault(double defaultValue) => TryConvert(AsDouble, defaultValue);
        public decimal AsDecimal() => Convert.ToDecimal(RawValue, Culture);
        public decimal AsDecimalOrDefault(decimal defaultValue) => TryConvert(AsDecimal, defaultValue);
        public bool AsBool() => ParseBool(RawValue);
        public bool AsBoolOrDefault(bool defaultValue) => TryConvert(AsBool, defaultValue);
        public DateTime AsDateTime() => Convert.ToDateTime(RawValue, Culture);
        public DateTime AsDateTimeOrDefault(DateTime defaultValue) => TryConvert(AsDateTime, defaultValue);
        public Regex AsRegex(RegexOptions options) => new(RawValue.ToString(), options);
        public Regex AsRegexOrDefault(Regex defaultValue, RegexOptions options) => TryConvert(() => AsRegex(options), defaultValue);
        public Regex AsRegex() => AsRegex(RegexOptions.None);
        public Regex AsRegexOrDefault(Regex defaultValue) => TryConvert(() => AsRegex(), defaultValue);
        private IEnumerable<T> EnumerableAsType<T>(IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                if (item is T expected)
                    yield return expected;

                else if (typeof(T) == typeof(DataNode))
                    yield return (T)(object)new DataNode(item, Culture);

                else if (item is IConvertible)
                    yield return (T)Convert.ChangeType(item, typeof(T));

                else
                    throw new InvalidOperationException($"Unexpected type: {item?.GetType() ?? null} can't be converted to {typeof(T)}");
            }
        }

        public List<T> AsSequence<T>()
        {
            EnsureType(NodeType.Sequence);
            if (RawValue is IEnumerable value)
                return [.. EnumerableAsType<T>(value)];

            return (List<T>)RawValue;
        }
        public List<T> AsSequenceOrDefault<T>(List<T> defaultValue)
            => TryConvert(() => AsSequence<T>(), defaultValue);
    }
}