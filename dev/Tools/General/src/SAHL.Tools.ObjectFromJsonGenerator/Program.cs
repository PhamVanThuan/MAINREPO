using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Tools.ObjectFromJsonGenerator.Commands;
using CommandLine;

namespace SAHL.Tools.ObjectFromJsonGenerator
{
    public class Program
    {
        private static void Main(string[] args)
        {
            CommandLineArguments commands = new CommandLineArguments();
            Parser parser = new Parser();
            bool parserResult = parser.ParseArguments(args, commands);
            if (parserResult == true)
            {
                if(commands.BuildMode==null)
                {
                    commands.BuildMode = "Debug";
                }
                SAHL.Tools.ObjectFromJsonGenerator.Lib.ObjectGenerator generator = new SAHL.Tools.ObjectFromJsonGenerator.Lib.ObjectGenerator();

                string connectionString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User Id={2};Password={3};Connect Timeout=300;",
                                        commands.Server, commands.Database, commands.UserName, commands.Password);
                try
                {
                    generator.GenerateObjects(connectionString, commands.OutputPath, commands.MsgSetVersion, commands.VarSetVersion, commands.EnumSetVersion, commands.Namespace, commands.BuildMode);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    Console.WriteLine("");
                    Console.WriteLine("stack: " + ex.StackTrace);
                    Environment.Exit(-1);
                }
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Error parsing command line arguments.");
            }
            Environment.Exit(0);
        }
    }
}
