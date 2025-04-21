using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Make
{
    internal class ProgramStarter
    {
        internal static void Start(string command)
        {
            var startinfo = new ProcessStartInfo("/bin/bash", $"-c \"{command}\"")
            {
                CreateNoWindow = false,
                UseShellExecute = false
            };
            using var p = Process.Start(startinfo);
            p?.WaitForExit();
        }
    }
}