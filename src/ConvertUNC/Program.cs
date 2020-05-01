using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConvertUNC
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var programName = Environment.GetCommandLineArgs().FirstOrDefault();
            try
            {
                programName = Path.GetFileName(Process.GetCurrentProcess().MainModule?.FileName) ?? Process.GetCurrentProcess().ProcessName;

                if (args.Length != 2)
                {
                    await Console.Error.WriteLineAsync($"Usage: {programName} EXECUTABLE PATH");
                    await Console.Error.WriteLineAsync($"I got: {programName} {string.Join(' ', args)}");
                    return 1;
                }

                var executable = args[0];
                var path = args[1];
                var newPath = path.Replace('/', '\\');

                await Console.Out.WriteLineAsync($"{programName}: converting path and executing {executable} {newPath}...");

                await Process.Start(executable, newPath).WaitForExitAsync();

                return 0;
            }
            catch (Exception e)
            {
                await Console.Error.WriteLineAsync(e.Message);
                await Console.Error.WriteLineAsync($"The call was: {programName} {string.Join(' ', args)}");
                return 2;
            }
        }
    }
}
