using System.Text.RegularExpressions;

namespace Autossential.Activities.Models
{
    public sealed partial class DataNode
    {
        public string AsString(string keyPath) => GetNode(keyPath).AsString();
        public string AsStringOrDefault(string keyPath, string defaultValue) => GetNode(keyPath).AsStringOrDefault(defaultValue);
        public int AsInt(string keyPath) => GetNode(keyPath).AsInt();
        public int AsIntOrDefault(string keyPath, int defaultValue) => GetNode(keyPath).AsIntOrDefault(defaultValue);
        public long AsLong(string keyPath) => GetNode(keyPath).AsLong();
        public long AsLongOrDefault(string keyPath, int defaultValue) => GetNode(keyPath).AsLongOrDefault(defaultValue);
        public float AsFloat(string keyPath) => GetNode(keyPath).AsFloat();
        public float AsFloatOrDefault(string keyPath, float defaultValue) => GetNode(keyPath).AsFloatOrDefault(defaultValue);
        public double AsDouble(string keyPath) => GetNode(keyPath).AsDouble();
        public double AsDoubleOrDefault(string keyPath, double defaultValue) => GetNode(keyPath).AsDoubleOrDefault(defaultValue);
        public decimal AsDecimal(string keyPath) => GetNode(keyPath).AsDecimal();
        public decimal AsDecimalOrDefault(string keyPath, decimal defaultValue) => GetNode(keyPath).AsDecimalOrDefault(defaultValue);
        public bool AsBool(string keyPath) => GetNode(keyPath).AsBool();
        public bool AsBoolOrDefault(string keyPath, bool defaultValue) => GetNode(keyPath).AsBoolOrDefault(defaultValue);
        public DateTime AsDateTime(string keyPath) => GetNode(keyPath).AsDateTime();
        public DateTime AsDateTimeOrDefault(string keyPath, DateTime defaultValue) => GetNode(keyPath).AsDateTimeOrDefault(defaultValue);
        public Regex AsRegex(string keyPath) => AsRegex(keyPath, RegexOptions.None);
        public Regex AsRegex(string keyPath, RegexOptions options) => GetNode(keyPath).AsRegex(options);
        public Regex AsRegexOrDefault(string keyPath, Regex defaultValue, RegexOptions options) => TryConvert(() => AsRegex(keyPath, options), defaultValue);
        public Regex AsRegexOrDefault(string keyPath, Regex defaultValue) => AsRegexOrDefault(keyPath, defaultValue, RegexOptions.None);
        public List<T> AsSequence<T>(string keyPath) => GetNode(keyPath).AsSequence<T>();
        public List<T> AsSequenceOrDefault<T>(string keyPath, List<T> defaultValue) => TryConvert(() => AsSequence<T>(keyPath), defaultValue);
    }
}
