using System.Text;

namespace Autossential.Activities.Core
{
    // ─────────────────────────────────────────────────────────────────────────
    //  Returns one of three native types:
    //    • string                        — scalar value (null for YAML null/~)
    //    • List<object>                  — sequence
    //    • Dictionary<string, object>    — mapping
    //
    //  Supported features:
    //    1. Scalars, Lists, Maps
    //    2. Block scalars  |  |-  |+  >  >-  >+
    //    3. Anchors (&) and Aliases (*)
    //    4. Merge keys (<<)  — including <<: [*a, *b]
    // ─────────────────────────────────────────────────────────────────────────
    public class YamlParser(string input)
    {
        // Each entry: (indentation, full-line content after comment strip, 1-based line number)
        private readonly List<(int indent, string content, int lineNo)> _lines = PreProcess(input);
        private readonly Dictionary<string, object> _anchors = [];
        private int _pos;

        /// <summary>Parse the YAML string and return the root value.</summary>
        public static object Parse(string input)
        {
            var p = new YamlParser(input);
            return p.ParseDocument();
        }

        // ── Pre-processor ────────────────────────────────────────────────────

        private static List<(int, string, int)> PreProcess(string input)
        {
            string[] raw = input.Replace("\r\n", "\n").Replace("\r", "\n").Split(['\n'], StringSplitOptions.RemoveEmptyEntries);
            var result = new List<(int, string, int)>(raw.Length);
            for (int i = 0; i < raw.Length; i++)
            {
                string stripped = StripInlineComment(raw[i]).TrimEnd();
                int indent = LeadingSpaces(stripped);
                result.Add((indent, stripped, i + 1));
            }
            return result;
        }

        private static int LeadingSpaces(string s)
        {
            int n = 0;
            while (n < s.Length && s[n] == ' ') n++;
            return n;
        }

        // Strip trailing `# comment` — only when preceded by a space and not inside quotes
        private static string StripInlineComment(string s)
        {
            bool inSingle = false, inDouble = false;
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c == '\'' && !inDouble) inSingle = !inSingle;
                else if (c == '"' && !inSingle) inDouble = !inDouble;
                else if (c == '#' && !inSingle && !inDouble && i > 0 && s[i - 1] == ' ')
                    return s[0..i];
            }
            return s;
        }

        // ── Skip helpers ─────────────────────────────────────────────────────

        private void SkipBlanks()
        {
            while (_pos < _lines.Count)
            {
                string t = _lines[_pos].content.TrimStart();
                if (string.IsNullOrEmpty(t) || t.StartsWith('#')) _pos++;
                else break;
            }
        }

        // ── Entry point ───────────────────────────────────────────────────────

        private object ParseDocument()
        {
            _pos = 0;
            SkipBlanks();
            return _pos >= _lines.Count ? null : ParseValue();
        }

        // ── Value dispatcher ─────────────────────────────────────────────────

        private object ParseValue()
        {
            SkipBlanks();
            if (_pos >= _lines.Count) return null;

            var (indent, content, lineNo) = _lines[_pos];
            string t = content.TrimStart();

            // Alias on its own line: *name
            if (t.StartsWith('*'))
            {
                string name = t[1..].Trim().Split(' ')[0];
                _pos++;
                return ResolveAlias(name, lineNo);
            }

            if (t.StartsWith("- ") || t == "-")
                return ParseList(indent);

            if (IsMapLine(t))
                return ParseMap(indent);

            _pos++;
            return ParseScalar(t, lineNo);
        }

        // ── Map ───────────────────────────────────────────────────────────────

        private Dictionary<string, object> ParseMap(int baseIndent)
        {
            var map = new Dictionary<string, object>(StringComparer.Ordinal);

            while (_pos < _lines.Count)
            {
                SkipBlanks();
                if (_pos >= _lines.Count) break;

                var (indent, content, lineNo) = _lines[_pos];
                if (indent != baseIndent) break;

                string t = content.TrimStart();
                if (!IsMapLine(t)) break;

                var (key, inlineVal) = SplitKeyValue(t);

                // Anchor declared on the key side:  &name key: value
                string anchorName = null;
                if (key.StartsWith('&'))
                {
                    int sp = key.IndexOf(' ');
                    if (sp < 0) { anchorName = key[1..]; key = ""; }
                    else { anchorName = key[1..sp]; key = key[(sp + 1)..]; }
                }

                // ── Merge key (<<) ───────────────────────────────────────────
                if (key == "<<")
                {
                    _pos++;
                    object mergeSource = ResolveMergeValue(inlineVal.Trim(), baseIndent + 1, lineNo);
                    YamlParser.ApplyMerge(map, mergeSource, lineNo);
                    continue;
                }

                _pos++;

                // ── Parse the value ──────────────────────────────────────────
                object value = ParseMappingValue(inlineVal, baseIndent, lineNo);

                if (anchorName != null) _anchors[anchorName] = value;
                map[key] = value;
            }

            return map;
        }

        // Decide how to obtain the value for a map entry.
        private object ParseMappingValue(string inlineVal, int baseIndent, int lineNo)
        {
            string iv = inlineVal.Trim();

            // No inline value → value is on the following indented lines
            if (string.IsNullOrEmpty(iv))
            {
                SkipBlanks();
                if (_pos >= _lines.Count) return null;
                int nextIndent = _lines[_pos].indent;
                return nextIndent > baseIndent ? ParseValue() : null;
            }

            return ParseInlineValue(iv, baseIndent + 1, lineNo);
        }

        // ── List ──────────────────────────────────────────────────────────────

        private List<object> ParseList(int baseIndent)
        {
            var list = new List<object>();

            while (_pos < _lines.Count)
            {
                SkipBlanks();
                if (_pos >= _lines.Count) break;

                var (indent, content, lineNo) = _lines[_pos];
                if (indent != baseIndent) break;

                string t = content.TrimStart();
                if (!t.StartsWith("- ") && t != "-") break;

                string rest = t.Length > 2 ? t[2..].Trim() : "";
                _pos++;

                object item = ParseListItemValue(rest, baseIndent, lineNo);
                list.Add(item);
            }

            return list;
        }

        private object ParseListItemValue(string rest, int baseIndent, int lineNo)
        {
            // Empty rest → value on next indented lines
            if (string.IsNullOrEmpty(rest))
            {
                SkipBlanks();
                if (_pos >= _lines.Count) return null;
                int ni = _lines[_pos].indent;
                return ni > baseIndent ? ParseValue() : null;
            }

            // Inline map entry as list item:  - key: value
            if (IsMapLine(rest))
            {
                var inlineMap = new Dictionary<string, object>(StringComparer.Ordinal);
                CollectInlineMapEntry(inlineMap, rest, baseIndent + 2, lineNo);

                // Pick up any sibling map lines at baseIndent+2
                while (_pos < _lines.Count)
                {
                    SkipBlanks();
                    if (_pos >= _lines.Count) break;
                    var (ni, nc, nl) = _lines[_pos];
                    if (ni != baseIndent + 2) break;
                    string nt = nc.TrimStart();
                    if (!IsMapLine(nt)) break;
                    _pos++;
                    CollectInlineMapEntry(inlineMap, nt, baseIndent + 2, nl);
                }
                return inlineMap;
            }

            return ParseInlineValue(rest, baseIndent + 2, lineNo);
        }

        // Process one key: value line into an existing map (used for list-item maps)
        private void CollectInlineMapEntry(Dictionary<string, object> map, string t, int baseIndent, int lineNo)
        {
            var (key, inlineVal) = SplitKeyValue(t);

            if (key == "<<")
            {
                object src = ResolveMergeValue(inlineVal.Trim(), baseIndent + 1, lineNo);
                YamlParser.ApplyMerge(map, src, lineNo);
                return;
            }

            map[key] = ParseMappingValue(inlineVal, baseIndent, lineNo);
        }

        // ── Block scalars (| and >) ───────────────────────────────────────────

        private string ParseBlockScalar(string indicator, int childIndent, int lineNo)
        {
            bool folded = indicator.TrimStart().StartsWith('>');
            char chomp = indicator.Contains('-') ? '-' : indicator.Contains('+') ? '+' : ' ';

            int explicitIndent = -1;
            foreach (char c in indicator)
                if (char.IsDigit(c)) { explicitIndent = c - '0'; break; }

            var blockLines = new List<string>();
            int detectedIndent = explicitIndent > 0 ? explicitIndent : -1;

            while (_pos < _lines.Count)
            {
                var (ind, raw, _) = _lines[_pos];

                if (string.IsNullOrWhiteSpace(raw))
                {
                    blockLines.Add("");
                    _pos++;
                    continue;
                }

                if (detectedIndent < 0 && ind >= childIndent)
                    detectedIndent = ind;

                int effectiveIndent = detectedIndent > 0 ? detectedIndent : childIndent;
                if (ind < effectiveIndent) break;

                string stripped = raw.Length >= detectedIndent ? raw[detectedIndent..] : raw.TrimStart();
                blockLines.Add(stripped);
                _pos++;
            }

            return BuildBlockScalar(blockLines, folded, chomp);
        }

        private static string BuildBlockScalar(List<string> lines, bool folded, char chomp)
        {
            int last = lines.Count - 1;
            while (last >= 0 && string.IsNullOrEmpty(lines[last])) last--;

            var sb = new StringBuilder();
            if (folded)
            {
                bool prevBlank = false;
                for (int i = 0; i <= last; i++)
                {
                    string line = lines[i];
                    if (string.IsNullOrEmpty(line))
                    {
                        sb.Append('\n');
                        prevBlank = true;
                    }
                    else
                    {
                        if (i > 0 && !prevBlank) sb.Append(' ');
                        sb.Append(line);
                        prevBlank = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i <= last; i++)
                    sb.Append(lines[i]).Append('\n');
            }

            string result = sb.ToString();
            return chomp switch
            {
                '-' => result.TrimEnd('\n'),
                '+' => result,
                _ => result.TrimEnd('\n') + '\n',   // clip: exactly one trailing newline
            };
        }

        // ── Flow collections ──────────────────────────────────────────────────

        private List<object> ParseFlowList(string s, int lineNo)
        {
            s = s.Trim();
            if (s.StartsWith('[')) s = s[1..];
            if (s.EndsWith(']')) s = s[..^1];

            var list = new List<object>();
            foreach (string part in SplitFlowItems(s))
            {
                string p = part.Trim();
                list.Add(p.StartsWith('*')
                    ? ResolveAlias(p[1..].Trim().Split(' ')[0], lineNo)
                    : ParseScalar(p, lineNo));
            }
            return list;
        }

        private static Dictionary<string, object> ParseFlowMap(string s, int lineNo)
        {
            s = s.Trim();
            if (s.StartsWith('{')) s = s[1..];
            if (s.EndsWith('}')) s = s[..^1];

            var map = new Dictionary<string, object>(StringComparer.Ordinal);
            foreach (string part in SplitFlowItems(s))
            {
                string p = part.Trim();
                if (!IsMapLine(p)) continue;
                var (k, v) = SplitKeyValue(p);
                map[k] = ParseScalar(v.Trim(), lineNo);
            }
            return map;
        }

        // ── Inline value router ───────────────────────────────────────────────

        // Resolve a value that appears on the same line as its key (or after a dash).
        private object ParseInlineValue(string iv, int childIndent, int lineNo)
        {
            if (string.IsNullOrWhiteSpace(iv))
            {
                SkipBlanks();
                if (_pos >= _lines.Count) return null;
                int ni = _lines[_pos].indent;
                return ni >= childIndent ? ParseValue() : null;
            }

            // Block scalar indicators
            if (IsBlockIndicator(iv))
                return ParseBlockScalar(iv, childIndent, lineNo);

            // Anchor on value: &name <rest>
            if (iv.StartsWith('&'))
            {
                int sp = iv.IndexOf(' ');
                string aName = sp < 0 ? iv[1..] : iv[1..sp];
                string rest = sp < 0 ? "" : iv[(sp + 1)..].Trim();
                object v = ParseInlineValue(rest, childIndent, lineNo);
                _anchors[aName] = v;
                return v;
            }

            // Alias: *name
            if (iv.StartsWith('*'))
                return ResolveAlias(iv[1..].Trim().Split(' ')[0], lineNo);

            // Flow collections
            if (iv.StartsWith('[')) return ParseFlowList(iv, lineNo);
            if (iv.StartsWith('{')) return ParseFlowMap(iv, lineNo);

            return ParseScalar(iv, lineNo);
        }

        private static bool IsBlockIndicator(string iv) =>
            iv is "|" or "|+" or "|-" or ">" or ">+" or ">-"
            || iv.StartsWith("|2") || iv.StartsWith("| ")
            || iv.StartsWith(">2") || iv.StartsWith("> ");

        // ── Merge key helpers ─────────────────────────────────────────────────

        // Resolve the value of a `<<:` line — could be *alias, [*a,*b], or a block alias
        private object ResolveMergeValue(string iv, int childIndent, int lineNo)
        {
            if (!string.IsNullOrWhiteSpace(iv))
            {
                if (iv.StartsWith('*'))
                    return ResolveAlias(iv[1..].Trim().Split(' ')[0], lineNo);
                if (iv.StartsWith('['))
                    return ResolveFlowAliasList(iv, lineNo);
                return ParseScalar(iv, lineNo);
            }
            // No inline value → next indented value (uncommon but valid)
            return ParseValue();
        }

        // Resolve a flow list whose items are aliases: [*a, *b]
        private List<object> ResolveFlowAliasList(string s, int lineNo)
        {
            s = s.Trim();
            if (s.StartsWith('[')) s = s[1..];
            if (s.EndsWith(']')) s = s[..^1];

            var list = new List<object>();
            foreach (string part in SplitFlowItems(s))
            {
                string p = part.Trim();
                if (p.StartsWith('*'))
                    list.Add(ResolveAlias(p[1..].Trim().Split(' ')[0], lineNo));
                else if (!string.IsNullOrEmpty(p))
                    list.Add(ParseScalar(p, lineNo));
            }
            return list;
        }

        private static void ApplyMerge(Dictionary<string, object> target, object source, int lineNo)
        {
            if (source is Dictionary<string, object> srcMap)
            {
                foreach (var kv in srcMap)
                    if (!target.ContainsKey(kv.Key))
                        target[kv.Key] = kv.Value;
            }
            else if (source is List<object> srcList)
            {
                foreach (object item in srcList)
                    YamlParser.ApplyMerge(target, item, lineNo);
            }
            else
                throw new Exception($"Line {lineNo}: merge key (<<) requires a map or list of maps");
        }

        // ── Scalar ────────────────────────────────────────────────────────────

        private static object ParseScalar(string raw, int lineNo)
        {
            raw = raw.Trim();
            if (raw is "null" or "~" or "") return null;
            if (raw.StartsWith('"') && raw.EndsWith('"'))
                return UnescapeDoubleQuoted(raw[1..^1]);
            if (raw.StartsWith('\'') && raw.EndsWith('\''))
                return raw[1..^1].Replace("''", "'");
            return raw;
        }

        private static string UnescapeDoubleQuoted(string s)
        {
            var sb = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\\' && i + 1 < s.Length)
                {
                    sb.Append(++i switch
                    {
                        _ => s[i] switch
                        {
                            'n' => '\n',
                            't' => '\t',
                            'r' => '\r',
                            '"' => '"',
                            '\\' => '\\',
                            '0' => '\0',
                            var c => c
                        }
                    });
                }
                else sb.Append(s[i]);
            }
            return sb.ToString();
        }

        // ── Alias resolution ──────────────────────────────────────────────────

        private object ResolveAlias(string name, int lineNo) =>
            _anchors.TryGetValue(name, out object node)
                ? node
                : throw new Exception($"Line {lineNo}: undefined alias *{name}");

        // ── Structural helpers ────────────────────────────────────────────────

        private static bool IsMapLine(string t)
        {
            if (string.IsNullOrEmpty(t) || t.StartsWith("- ") || t == "-") return false;

            // Quoted key
            if (t.StartsWith('"') || t.StartsWith('\''))
            {
                char q = t[0];
                int end = t.IndexOf(q, 1);
                return end > 0 && end + 1 < t.Length && t[end + 1] == ':';
            }

            int colon = t.IndexOf(':');
            if (colon < 0) return false;
            if (colon == t.Length - 1) return true;         // "key:" with no value
            return t[colon + 1] == ' ' || t[colon + 1] == '\t';
        }

        private static (string key, string value) SplitKeyValue(string t)
        {
            // Quoted key
            if (t.StartsWith('"') || t.StartsWith('\''))
            {
                char q = t[0];
                int end = t.IndexOf(q, 1);
                if (end > 0)
                {
                    string k = t[1..end];
                    string rest = (end + 1 < t.Length ? t[(end + 1)..] : "").TrimStart();
                    if (rest.StartsWith(':')) rest = rest.Length > 1 ? rest[1..] : "";
                    return (k, rest);
                }
            }

            int colon = t.IndexOf(':');
            if (colon < 0) return (t, "");
            return (t[..colon].Trim(), colon + 1 < t.Length ? t[(colon + 1)..] : "");
        }

        private static List<string> SplitFlowItems(string s)
        {
            var items = new List<string>();
            int depth = 0;
            bool inSingle = false, inDouble = false;
            int start = 0;
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c == '\'' && !inDouble) inSingle = !inSingle;
                else if (c == '"' && !inSingle) inDouble = !inDouble;
                else if (!inSingle && !inDouble)
                {
                    if (c is '[' or '{') depth++;
                    else if (c is ']' or '}') depth--;
                    else if (c == ',' && depth == 0) { items.Add(s[start..i]); start = i + 1; }
                }
            }
            if (start < s.Length) items.Add(s[start..]);
            return items;
        }
    }
}