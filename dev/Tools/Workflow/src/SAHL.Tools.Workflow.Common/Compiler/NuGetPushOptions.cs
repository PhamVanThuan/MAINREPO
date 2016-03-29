using System.Collections.Generic;

namespace SAHL.Tools.Workflow.Common.Compiler
{
    public class NuGetPushOptions
    {
        public NuGetPushOptions(string sahlNuGetInstallRepo, string sahlNuGetPushApi, string sahlNuGetApiKey)
        {
            this.SAHLNuGetInstallRepo = sahlNuGetInstallRepo;
            this.SAHLNuGetPushApi = sahlNuGetPushApi;
            this.SAHLNuGetApiKey = sahlNuGetApiKey;
        }
        public string SAHLNuGetInstallRepo { get; protected set; }
        public string SAHLNuGetPushApi { get; protected set; }
        public string SAHLNuGetApiKey { get; protected set; }
    }
}