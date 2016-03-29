using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using SAHL.Tools.DecisionTreeTestFromJsonGenerator.Commands;
using SAHL.Tools.DecisionTreeTestFromJsonGenerator.Lib;
using SAHL.Tools.ObjectModelGenerator.Lib;
using System.Diagnostics;

namespace SAHL.Tools.DecisionTreeTestFromJsonGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineArguments commands = new CommandLineArguments();
            Parser parser = new Parser();
            bool parserResult = parser.ParseArguments(args, commands);

            string connectionString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User Id={2};Password={3};Connect Timeout=300;",
                                                      commands.Server, commands.Database, commands.UserName, commands.Password);
            List<string> exclusions = new List<string>();


            IncludeFilter filter = new IncludeFilter(commands.Include);
            exclusions.AddRange(filter.Excluded);
            bool publishedOnly = commands.BuildMode == "Release";
            DecisionTreeTestGenerator testGenerator = new DecisionTreeTestGenerator();

            IList<string> trees = testGenerator.GetAllTreesNames(connectionString);
            
            testGenerator.GenerateDecisonTreeTests(connectionString, publishedOnly, exclusions, commands.OutputPath, commands.NameSpace);
        }
    }
}
