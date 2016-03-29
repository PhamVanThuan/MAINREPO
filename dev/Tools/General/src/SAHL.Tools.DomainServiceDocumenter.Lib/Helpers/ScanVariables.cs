using Microsoft.SqlServer.Server;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Helpers
{
    public class ScanVariables
    {

        public string ServiceName { get; private set; }
        public string InputPath { get; private set; }
        public string OutputPath { get; private set; }

        public ScanVariables(string serviceName, string inputPath, string outputPath)
        {
            ServiceName = PrepareServiceName(serviceName);
            InputPath = inputPath;
            OutputPath = outputPath;
        }

        private static string PrepareServiceName(string serviceName)
        {
            if (serviceName.StartsWith("SAHL.Services.Interfaces"))
            {
                serviceName = serviceName.Replace("SAHL.Services.Interfaces", "");
            }

            if (serviceName.StartsWith("SAHL.Services."))
            {
                serviceName = serviceName.Replace("SAHL.Services.", "");
            }

            if (serviceName.EndsWith("Domain"))
            {
                serviceName = serviceName.Replace("Domain", "");
            }
            return serviceName;
        }

        
    }
}