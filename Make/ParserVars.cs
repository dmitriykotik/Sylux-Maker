using System.Text.RegularExpressions;
using MultiAPI;

namespace Make
{
    internal class ParserVars
    {
        private static readonly Regex varRegex = new(@"%([^%]+)%", RegexOptions.Compiled);
        internal static readonly string[] sectionsPriority = { "Vars", "Output", "Compiler", "Debug" };

        internal static string Parse(INI ini, string input)
        {
            if (string.IsNullOrEmpty(input)) return "";

            return varRegex.Replace(input, match =>
            {
                var key = match.Groups[1].Value;

                foreach (var section in sectionsPriority)
                {
                    var value = ini.GetValue(section, key);
                    if (!string.IsNullOrEmpty(value))
                        return value;
                }

                return match.Value;
            });
        }
    }
}