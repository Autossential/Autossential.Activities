namespace Autossential.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsMatch(this string s, string p)
        {
            int i = 0, j = 0, star = -1, offset = -1;

            bool except = false;
            if (p.Length > 0 && p[0] == '!')
            {
                except = true;
                j++;
            }

            while (i < s.Length)
            {
                if (j < p.Length && (p[j] == '?' || s[i] == p[j]))
                {
                    i++;
                    j++;
                    continue;
                }

                if (j < p.Length && p[j] == '*')
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

            while (j < p.Length && p[j] == '*')
                j++;

            return except ? j < p.Length : j == p.Length;
        }
    }
}
