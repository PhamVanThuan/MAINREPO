using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CommandLine;
using SAHL.Tools.Workflow.MapPackageUpdater.CommandLine;

namespace SAHL.Tools.Workflow.MapPackageUpdater
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
                    XDocument map = null;
                    using (var fileStream = File.OpenRead(arguments.X2WorkflowMap))
                    {
                        map = XDocument.Load(fileStream);
                    }
                    map.Elements("ProcessName").Elements("NugetPackages").Elements("NugetPackage").Remove();
                    var nugetPackageNode = new XElement("NugetPackage",
                        new XAttribute("PackageName", arguments.PackageName),
                        new XAttribute("Version", arguments.Version));

                    if (map.Elements("ProcessName").Elements("NugetPackages").FirstOrDefault() == null)
                    {
                        map.Elements("ProcessName").First().Add(new XElement("NugetPackages"));
                    }

                    map.Elements("ProcessName").Elements("NugetPackages").First().Add(nugetPackageNode);
                    
                    map.Save(arguments.X2WorkflowMap);
                }
            }
        }
    }
}