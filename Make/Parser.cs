using MultiAPI;

#pragma warning disable CS8604,CS8602

namespace Make
{
    internal class Parser
    {
        internal static bool Execute(INI ini, string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            var data = ParserSplit.Split2Args(input);

            switch (data.firstArg)
            {
                case "echo":
                    Console.WriteLine("Make: " + ParserVars.Parse(ini, data.secondArg));
                    return true;

                case "space":
                    Console.WriteLine();
                    return true;

                case "foreach":
                    var _data = ParserSplit.Split3Args(input);
                    if (_data.secondArg == "gcc")
                    {
                        var excludePath = Path.GetFullPath(ParserVars.Parse(ini, "%i686-elf-tools%")).Replace("\\", "/");

                        var cFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.c", SearchOption.AllDirectories)
                            .Where(f => !Path.GetFullPath(f).Replace("\\", "/").StartsWith(excludePath));

                        foreach (var file in cFiles)
                        {
                            var noExt = Path.Combine(Path.GetDirectoryName(file)!, Path.GetFileNameWithoutExtension(file)).Replace("\\", "/");
                            var command = _data.thirdArg.Replace("%*%", noExt);
                            command = ParserVars.Parse(ini, command);

                            Console.WriteLine("Make: Foreach: " + command);
                            ProgramStarter.Start(command);
                        }
                        return true;
                    }
                    else if (_data.secondArg == "ld")
                    {
                        var excludePath = Path.GetFullPath(ParserVars.Parse(ini, "%i686-elf-tools%")).Replace("\\", "/");

                        var oFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.o", SearchOption.AllDirectories)
                            .Where(f => !Path.GetFullPath(f).Replace("\\", "/").StartsWith(excludePath))
                            .Select(f => f.Replace("\\", "/"));

                        var filesCombined = string.Join(" ", oFiles);
                        var command = _data.thirdArg.Replace("%*%", filesCombined);
                        command = ParserVars.Parse(ini, command);

                        Console.WriteLine("Make: Foreach: " + command);
                        ProgramStarter.Start(command);
                        return true;
                    }
                    else if (_data.secondArg == "output")
                    {
                        var excludePath = Path.GetFullPath(ParserVars.Parse(ini, "%i686-elf-tools%")).Replace("\\", "/");

                        var cFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.o", SearchOption.AllDirectories)
                            .Where(f => !Path.GetFullPath(f).Replace("\\", "/").StartsWith(excludePath));

                        foreach (var file in cFiles)
                        {
                            var noExt = Path.Combine(Path.GetDirectoryName(file)!, Path.GetFileNameWithoutExtension(file)).Replace("\\", "/");
                            var command = _data.thirdArg.Replace("%*%", noExt);
                            command = ParserVars.Parse(ini, command);

                            Console.WriteLine("Make: Foreach: " + command);
                            ProgramStarter.Start(command);
                        }
                        return true;
                    }
                    else return false;

                default:
                    return false;
            }
        }
    }
}