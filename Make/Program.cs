using Microsoft.ML.Data;
using MultiAPI;

namespace Make
{
    internal class Program
    {
        private static string cfgDefaultContent = @";
;  __  __       _             _____       _            
; |  \/  |     | |       _   / ____|     | |           
; | \  / | __ _| | _____(_) | (___  _   _| |_   ___  __
; | |\/| |/ _` | |/ / _ \    \___ \| | | | | | | \ \/ /
; | |  | | (_| |   <  __/_   ____) | |_| | | |_| |>  < 
; |_|  |_|\__,_|_|\_\___(_) |_____/ \__, |_|\__,_/_/\_\
;                                    __/ |             
;                                   |___/              
;

; Specify the path to the compiler. If you have not installed the compiler, you can install it, 
; as well as other necessary packages, using the ./Make install_i686 command
[Compiler]
i686-elf-tools=

; Make sure that the qemu folder you specify contains tools such as: qemu-system-x86_64.
; If the tools can be used without specifying the full path, leave this field blank
[Debug]
qemu=

; Specify the data for the output files 
[Output]
bin=
iso=

; Section for user variables.
; After entering the %i686-elf-tools% variable, do not enter a slash, it will be supplied if necessary. 
; I.e. specify the string as follows: %i686-elf-tools%i686-elf-gcc <...>
; Enter the identifier for your variable, and after through the equals sign, enter the value without the quotation marks. 
; You can then use this variable as follows: %your_variable%
[Vars]
gcc=i686-elf-gcc
as=i686-elf-as

; Assembly Section. 
; Specify the number of commands to be executed, and name the variables with these commands in numerical order (i.e. 1, 2, 3, etc.). The countdown starts at 1.
; While compiling with grub-mkrescue make sure you have the xorriso package installed, if not you can install it with ./Make install_i686
[Build]
Count=1
1=

; Launch Section.
; Specify the number of commands to be executed, and name the variables with these commands in numerical order (i.e. 1, 2, 3, etc.). The countdown starts at 1.
[Run]
Count=1
1=

; Clean Section.
; Specify the number of commands to be executed, and name the variables with these commands in numerical order (i.e. 1, 2, 3, etc.). The countdown starts at 1.
[Clean]
Count=1
1=
";

        static void Main(string[] args)
        {
            Console.WriteLine(@"   _____       _              _  __                    _  ");
            Console.WriteLine(@"  / ____|     | |            | |/ /                   | | ");
            Console.WriteLine(@" | (___  _   _| |_   ___  __ | ' / ___ _ __ _ __   ___| | ");
            Console.WriteLine(@"  \___ \| | | | | | | \ \/ / |  < / _ \ '__| '_ \ / _ \ | ");
            Console.WriteLine(@"  ____) | |_| | | |_| |>  <  | . \  __/ |  | | | |  __/ | ");
            Console.WriteLine(@" |_____/ \__, |_|\__,_/_/\_\ |_|\_\___|_|  |_| |_|\___|_| ");
            Console.WriteLine(@"          __/ |                                           ");
            Console.WriteLine(@"         |___/                                            ");
            Console.WriteLine();

            if (!File.Exists("Config.Make")) 
            {
                File.WriteAllText(@"Config.Make", cfgDefaultContent);
                Console.WriteLine("Make: The configuration file has been created. Before working with Make, please set up the build configuration.");
                Console.WriteLine("Make: Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(0);
            }

            INI cfg = new INI("Config.Make");

            if (args.Length <= 0)
            {
                Console.WriteLine(@"  ./Make build - Kernel assembly.
  ./Make run - Starting the kernel.
  ./Make clean - Cleaning.
  ./Make version - Product version.
  ./Make install_i686 - Installing the necessary packages to build the kernel.
  ./Make help - Help.");
                Environment.Exit(0);
            }

            switch (args[0].ToLower())
            {
                case "clean":
                    for (int i = 1; i <= int.Parse(cfg.GetValue("Clean", "Count")); i++)
                    {
                        string _string = cfg.GetValue("Clean", i.ToString());
                        if (string.IsNullOrEmpty(_string)) continue;
                        if (!Parser.Execute(cfg, _string)) ProgramStarter.Start(ParserVars.Parse(cfg, _string));
                    }
                    break;

                case "build":
                    for (int i = 1; i <= int.Parse(cfg.GetValue("Build", "Count")); i++)
                    {
                        string _string = cfg.GetValue("Build", i.ToString());
                        if (string.IsNullOrEmpty(_string)) continue;
                        if (!Parser.Execute(cfg, _string)) ProgramStarter.Start(ParserVars.Parse(cfg, _string));
                    }
                    break;

                case "run":
                    for (int i = 1; i <= int.Parse(cfg.GetValue("Run", "Count")); i++)
                    {
                        string _string = cfg.GetValue("Run", i.ToString());
                        if (string.IsNullOrEmpty(_string)) continue;
                        if (!Parser.Execute(cfg, _string)) ProgramStarter.Start(ParserVars.Parse(cfg, _string));
                    }
                    break;

                case "build_run":
                    for (int i = 1; i <= int.Parse(cfg.GetValue("Build", "Count")); i++)
                    {
                        string _string = cfg.GetValue("Build", i.ToString());
                        if (string.IsNullOrEmpty(_string)) return;
                        if (!Parser.Execute(cfg, _string)) ProgramStarter.Start(ParserVars.Parse(cfg, _string));
                    }
                    for (int i = 1; i <= int.Parse(cfg.GetValue("Run", "Count")); i++)
                    {
                        string _string = cfg.GetValue("Run", i.ToString());
                        if (string.IsNullOrEmpty(_string)) return;
                        if (!Parser.Execute(cfg, _string)) ProgramStarter.Start(ParserVars.Parse(cfg, _string));
                    }
                    break;
                case "help":
                    Console.WriteLine(@"  ./Make build - Kernel assembly.
  ./Make run - Starting the kernel.
  ./Make clean - Cleaning.
  ./Make version - Product version.
  ./Make install_i686 - Installing the necessary packages to build the kernel.
  ./Make help - Help.");
                    break;
                case "install_i686":
                    ProgramStarter.Start("apt-get update");
                    ProgramStarter.Start("apt-get install unzip");
                    ProgramStarter.Start("apt-get install xorriso");
                    ProgramStarter.Start("mkdir i686-elf-tools");
                    ProgramStarter.Start("wget https://github.com/lordmilko/i686-elf-tools/releases/download/13.2.0/i686-elf-tools-linux.zip");
                    ProgramStarter.Start("unzip i686-elf-tools-linux.zip -d i686-elf-tools");
                    ProgramStarter.Start("rm i686-elf-tools-linux.zip");
                    cfg.SetValue("Compiler", "i686-elf-tools", "i686-elf-tools");
                    break;
                case "version":
                    Console.WriteLine(@"Make for Sylux v0.1.1.61");
                    break;
            }
        }
    }
}
