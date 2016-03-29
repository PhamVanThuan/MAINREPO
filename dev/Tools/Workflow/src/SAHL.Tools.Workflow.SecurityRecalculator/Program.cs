using CommandLine;
using SAHL.Tools.Workflow.SecurityRecalculator.CommandLine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using recalculator = SAHL.Tools.Workflow.Common.Database.SecurityRecalculating;

namespace SAHL.Tools.Workflow.SecurityRecalculator
{
    public class Processes
    {
        public string ProcessName { get; set; }

        public int ProcessID { get; set; }

        public int OldProcessID { get; set; }

        public Processes(string processName, int processID, int oldProcessID)
        {
            this.ProcessName = processName;
            this.ProcessID = processID;
            this.OldProcessID = oldProcessID;
        }
    }

    public class Program
    {
        private static void Main(string[] args)
        {
            CommandLineArguments arguments = new CommandLineArguments();
            Parser parser = new Parser();
            bool parserResult = parser.ParseArguments(args, arguments);

            if (parserResult == true)
            {
                int noOFinstancesRecalculated = 0;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Starting Security Recalculation");
                
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Nuget Server : {0}", ConfigurationManager.AppSettings["NugetServerLocations"].ToString());

                string connectionString = string.Format("Data Source={0};Initial Catalog=X2;User ID={1};Password={2};Timeout=300", arguments.DatabaseName, arguments.UserName, arguments.Password);

                IList<Processes> processesToRecalculate = new List<Processes>();

                string processName = String.Empty;
                int oldProcessID = 0;
                int processID = 0;

                recalculator.SecurityRecalculator security = new recalculator.SecurityRecalculator();

                if (!String.IsNullOrEmpty(arguments.X2WorkflowMaps)) // we have been passed a list of maps so lets use them
                {
                    string[] maps = arguments.X2WorkflowMaps.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var map in maps)
                    {
                        processName = map.Trim();
                        processesToRecalculate.Add(new Processes(processName, processID, oldProcessID));
                    }
                }
                else if (arguments.ProcessID > 0 && arguments.OldProcessID > 0) // we have been passed specific process ids so lets use them
                {
                    processesToRecalculate.Add(new Processes(processName, arguments.ProcessID, arguments.OldProcessID));
                }

                foreach (var process in processesToRecalculate)
                {
                    // call security recalculator
                    noOFinstancesRecalculated = security.Recalculate(process.ProcessName, process.ProcessID, process.OldProcessID, connectionString);
                    Console.WriteLine("Process: {0}  - {1} instances have had their security recalculated.", process.ProcessName, noOFinstancesRecalculated);
                }

                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine("Cleaning up");
                var directoryToRemove = "dotnetX2Processes";
                try
                {
                    if (Directory.Exists(directoryToRemove))
                    {
                        Directory.Delete(directoryToRemove, true);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to remove " + directoryToRemove);
                    Console.WriteLine("Reason : " + ex.Message);
                }

                Console.WriteLine("Security Recalculation Completed");
                Environment.Exit(0);
            }
            else
            {
                throw new Exception("Error parsing command line arguments");
            }
        }
    }
}