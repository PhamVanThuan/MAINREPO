using SAHL.Tools.DomainServiceDocumenter.Lib.Helpers;
using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using SAHL.Tools.DomainServiceDocumenter.Lib.Templates;
using SAHL.Tools.DomainServiceDocumenter.Lib.TypeScanners;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SAHL.Tools.DomainServiceDocumenter.Lib
{
    public class Documenter
    {
        public void GenerateObjects(string serviceName, string inputPath, string outputPath)
        {
            
            ScanVariables variables = new ScanVariables(serviceName, inputPath, outputPath);
            ScanDirectoryUtility scanDirectoryUtility = new ScanDirectoryUtility(variables);
            scanDirectoryUtility.PrepareWorkingDirectories();

            ServiceScanner serviceScanner = new ServiceScanner(variables);
            ScanService(serviceScanner);
            serviceScanner.ScanAssociations();

            WriteOutput(variables.ServiceName, outputPath, scanDirectoryUtility, serviceScanner);

            // generate the queries
            
        }

        private void ScanService(ServiceScanner serviceScanner)
        {
            if (File.Exists(serviceScanner.SharedName))
            {
                serviceScanner.ScanEnumerators();
                serviceScanner.ScanModels();
                serviceScanner.ScanDomainChecks();
                serviceScanner.ScanEvents();
                serviceScanner.ScanRules();
                serviceScanner.ScanCommands();
                serviceScanner.ScanCommandHandlers();
            }
            else
            {
            }

            if (File.Exists(serviceScanner.ServerName))
            {
                // now scan for the command handlers
            }
            else
            {
            }

        }

        private void WriteOutput(string serviceName, string outputPath, ScanDirectoryUtility scanDirectoryUtility, ServiceScanner serviceScanner)
        {
            HtmlOutputGenerator htmlOutputGenerator = new HtmlOutputGenerator(serviceName, outputPath, scanDirectoryUtility, serviceScanner);
            htmlOutputGenerator.WriteCSSFile();
            htmlOutputGenerator.WriteIndex();
            htmlOutputGenerator.WriteCommands();
            htmlOutputGenerator.WriteQueries();
            htmlOutputGenerator.WriteEvents();
            htmlOutputGenerator.WriteModels();
            htmlOutputGenerator.WriteRules();
            htmlOutputGenerator.WriteDomainChecks();
            htmlOutputGenerator.WriteEmerators();
        }

    }
}