using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CommandLine;
using SAHL.Tools.Workflow.MapLegacyUpdater.CommandLine;

namespace SAHL.Tools.Workflow.MapLegacyUpdater
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CommandLineArguments arguments = new CommandLineArguments();
            Parser parser = new Parser();
            bool parserResult = parser.ParseArguments(args, arguments);
            if (parserResult)
            {
                // check that the the workflow x2p file exists
                if (File.Exists(arguments.X2WorkflowMap))
                {
                    // open xml file
                    XDocument xmlFile = XDocument.Load(arguments.X2WorkflowMap);

                    // update version number using linq to xml
                    xmlFile.Descendants("ProcessName").Single().SetAttributeValue("Legacy", arguments.Legacy);

                    // save xml file
                    xmlFile.Save(arguments.X2WorkflowMap, SaveOptions.None);
                }
            }
        }
    }
}