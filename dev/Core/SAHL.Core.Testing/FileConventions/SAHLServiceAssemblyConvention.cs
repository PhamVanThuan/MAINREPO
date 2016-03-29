using System.Linq;
using System.Collections.Generic;
using System.IO;
using SAHL.Core.Testing.FileConventions;
using System.Reflection;
namespace SAHL.Core.Testing.FileConventions
{
    public class SAHLServiceAssemblyConvention : SAHLFileConvention, IFileConvention
    {
        public bool Process(FileInfo file)
        {
            return base.Run(file, () =>
            {
                if (IsFileExcludedFromProcessing(file))
                {
                    return false;
                }

                var serviceConfigFiles = GetConfigurationFiles(file);
                if (!AreThereFilesToProcess(serviceConfigFiles))
                {
                    return false;
                }

                var productName = GetProductName(serviceConfigFiles);

                if (IsServiceConfigFile(file) || IsServiceInterfaceFile(file, productName) || IsServiceFile(file, productName))
                {
                    return true;
                }

                return IsDataModel(file, productName);

            });
        }

        private bool IsServiceConfigFile(FileInfo file)
        {
            return file.Name.StartsWith("SAHL.Config.Services.") && file.Name.EndsWith("Server.dll");
        }

        private bool IsDataModel(FileInfo file, string productName)
        {
            return file.Name.Contains("SAHL." + productName + ".Models") || file.Name.Contains("SAHL.Core.Data.Models");
        }

        private bool IsServiceFile(FileInfo file, string productName)
        {
            return file.Name.Contains("SAHL.Services." + productName + ".dll");
        }

        private bool IsServiceInterfaceFile(FileInfo file, string productName)
        {
            return file.Name.Contains("SAHL.Services.Interfaces." + productName + ".dll");
        }

        private bool IsFileExcludedFromProcessing(FileInfo file)
        {
            return (file.Name.Contains("Test") || file.Name.Contains("Specs")) && !file.Name.Contains("FrontEndTest");
        }

        private bool AreThereFilesToProcess(List<FileInfo> serviceConfigFiles)
        {
            return serviceConfigFiles.Count != 0;
        }

        private List<FileInfo> GetConfigurationFiles(FileInfo file)
        {
            return file.Directory
                .EnumerateFiles()
                .Where(x => x.Name.StartsWith("SAHL.Config.Services.") && x.Name.EndsWith("Server.dll"))
                .ToList();
        }
        
        private string GetProductName(List<FileInfo> serviceConfigFiles)
        {
            return serviceConfigFiles[0].Name.Replace("SAHL.Config.Services.", "").Replace(".Server.dll", "");
        }

    }
}
