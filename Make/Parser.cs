using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiAPI;

namespace Make
{
    internal class Parser
    {
        internal static bool Execute(INI ini, string input)
        {
            if (input.Split(' ').Length < 1) return false;
            
            switch (getFirstArg(input).ToLower())
            {
                case "echo":
                    Console.WriteLine("Make: " + ParserVars.Parse(ini, getSecondArg(input)));
                    return true;
                case "space":
                    Console.WriteLine();
                    return true;
                default:
                    return false;
            }
        }

        private static string getFirstArg(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";

            var index = input.IndexOf(' ');
            if (index == -1)
                return "";

            var first = input.Substring(0, index);
            return first;
        }

        private static string getSecondArg(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";

            var index = input.IndexOf(' ');
            if (index == -1)
                return "";

            var second = input.Substring(index + 1).TrimStart();
            return second;
        }
    }
}