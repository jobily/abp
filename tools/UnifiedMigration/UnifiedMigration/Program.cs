using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace UnifiedMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Please enter the execution directory, the default is the current directory:");
            var directory = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(directory))
            {
                directory = Directory.GetCurrentDirectory();
            }

            Console.WriteLine($"Directory: [{directory}]");
            directory = @"D:\Github\volo\abp3";

            string newMigrationName;
            do
            {
                Console.WriteLine("Please enter a migration name(Do not include spaces):");
                newMigrationName = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(newMigrationName));

            Console.WriteLine($"Migration Name: [{newMigrationName}]");

            /*
            Console.WriteLine("Please enter a config file name, the default is the projects.json:");

            var configFile = Console.ReadLine();

            if (configFile.IsNullOrWhiteSpace())
            {
                configFile = "projects.json";
            }
            if (!File.Exists(configFile))
            {
                throw new ArgumentException($"config file {configFile} not exist!");
            }
            Console.WriteLine($"Config file: {configFile}");
            */

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();

            const string configFile = "projects.json";
            var projects = JsonConvert.DeserializeObject<List<ProjectModel>>(File.ReadAllText(configFile));

            foreach (var project in projects)
            {
                var workingDirectory = Path.Combine(directory, project.Path);
                Console.WriteLine("Path: " + project.Path);

                if (project.Recreate)
                {
                    CmdHelper.RunCmdAndGetOutput("dotnet ef migrations remove -f --prefix-output", workingDirectory, Console.WriteLine, out var errorCode);

                    if (errorCode == CmdHelper.SuccessfulExitCode)
                    {
                        CmdHelper.RunCmdAndGetOutput($"dotnet ef migrations add {project.MigrationName} --prefix-output", workingDirectory, Console.WriteLine, out errorCode);
                    }
                }
                else
                {
                    CmdHelper.RunCmdAndGetOutput($"dotnet ef migrations add {newMigrationName} --prefix-output", workingDirectory, Console.WriteLine, out var errorCode);
                }
            }
        }
    }
}
