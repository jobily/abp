using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace UnifiedMigration
{
    public static class CmdHelper
    {
        public static int SuccessfulExitCode = 0;

        public static void RunCmdAndGetOutput(string command, string workingDirectory, Action<string> outputAction, out int exitCode)
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo(CmdHelper.GetFileName())
                {
                    Arguments = CmdHelper.GetArguments(command),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WorkingDirectory = workingDirectory
                };

                process.Start();

                using (var standardOutput = process.StandardOutput)
                {
                    using (var standardError = process.StandardError)
                    {
                        var standardOutputText = standardOutput.ReadToEnd();
                        outputAction?.Invoke(standardOutputText);

                        var standardErrorText = standardError.ReadToEnd();
                        outputAction?.Invoke(standardErrorText);
                    }
                }

                process.WaitForExit();

                exitCode = process.ExitCode;
            }
        }

        public static string GetArguments(string command)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "-c \"" + command + "\"";
            }

            //Windows default.
            return "/C \"" + command + "\"";
        }

        public static string GetFileName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                //TODO: Test this. it should work for both operation systems.
                return "/bin/bash";
            }

            //Windows default.
            return "cmd.exe";
        }
    }
}
