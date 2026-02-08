namespace Autossential.Activities.Extensions
{
    public static class StringExtensions
    {
        public static bool IsMatch(this string str, string pattern)
        {
            int i = 0, j = 0, star = -1, offset = -1;

            bool except = false;
            if (pattern.Length > 0 && pattern[0] == '!')
            {
                except = true;
                j++;
            }

            while (i < str.Length)
            {
                if (j < pattern.Length && (pattern[j] == '?' || str[i] == pattern[j]))
                {
                    i++;
                    j++;
                    continue;
                }

                if (j < pattern.Length && pattern[j] == '*')
                {
                    star = j++;
                    offset = i;
                    continue;
                }

                if (star > -1)
                {
                    j = star + 1;
                    i = ++offset;
                    continue;
                }

                return except;
            }

            while (j < pattern.Length && pattern[j] == '*')
                j++;

            return except ? j < pattern.Length : j == pattern.Length;
        }
    }
}
