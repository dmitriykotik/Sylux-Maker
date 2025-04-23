using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Make
{
    internal class ParserSplit
    {
        internal static SplitArguments Split2Args(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new SplitArguments() { firstArg = "", secondArg = "" };

            var index = input.IndexOf(' ');
            if (index == -1)
                return new SplitArguments() { firstArg = input, secondArg = "" };

            var first = input.Substring(0, index);
            var second = input.Substring(index + 1).TrimStart();
            return new SplitArguments() { firstArg = first, secondArg = second };
        }

        internal static SplitArgumentsM Split3Args(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new SplitArgumentsM() { firstArg = "", secondArg = "", thirdArg = "" };

            var firstSpace = input.IndexOf(' ');
            if (firstSpace == -1)
                return new SplitArgumentsM() { firstArg = input, secondArg = "", thirdArg = "" };

            var secondSpace = input.IndexOf(' ', firstSpace + 1);
            if (secondSpace == -1)
            {
                var first = input.Substring(0, firstSpace);
                var second = input.Substring(firstSpace + 1).Trim();
                return new SplitArgumentsM() { firstArg = first, secondArg = second, thirdArg = "" };
            }

            var firstArg = input.Substring(0, firstSpace);
            var secondArg = input.Substring(firstSpace + 1, secondSpace - firstSpace - 1);
            var thirdArg = input.Substring(secondSpace + 1).Trim();
            return new SplitArgumentsM() { firstArg = firstArg, secondArg = secondArg, thirdArg = thirdArg };
        }

        internal class SplitArguments
        {
            public string? firstArg;
            public string? secondArg;
        }

        internal class SplitArgumentsM
        {
            public string? firstArg;
            public string? secondArg;
            public string? thirdArg;
        }
    }
}