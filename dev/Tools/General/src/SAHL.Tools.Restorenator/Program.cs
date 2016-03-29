using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using SAHL.Tools.Scriptenator;

namespace SAHL.Tools.Restorenator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var commandLineParser = new Parser();
            var commandLineArguments = new RestorenatorCommandLineArguments();
            if (commandLineParser.ParseArguments(args, commandLineArguments))
            {
                try
                {
                    Console.WriteLine("Start create dynamic restore script..." + DateTime.Now);
                    var compiledConnectionString = new SqlConnectionStringBuilder(commandLineArguments.ConnectionString);
                    RestoreDatabase.GenerateRestoreScript(compiledConnectionString.UserID, commandLineArguments.Databases, commandLineArguments.RestoreFromPath, commandLineArguments.ScriptDirectory);
                    Console.WriteLine("Completed create dynamic restore script..." + DateTime.Now);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Console.WriteLine("An error occured, script processing aborted!");
                    Environment.ExitCode = -1;
                }
            }
        }
    }
}