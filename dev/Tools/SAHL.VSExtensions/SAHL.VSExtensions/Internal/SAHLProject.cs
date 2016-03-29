using SAHL.VSExtensions.Interfaces;
using SAHL.VSExtensions.Interfaces.Configuration;

namespace SAHomeloans.SAHL_VSExtensions.Internal
{
    public class SAHLProject : ISAHLProject
    {
        private IVSServices services;
        private EnvDTE.Project currentProject;

        public SAHLProject(IVSServices services, EnvDTE.Project currentProject)
        {
            this.services = services;
            this.currentProject = currentProject;
        }

        public bool Build()
        {
            EnvDTE.DTE dte = services.GetService<EnvDTE.DTE>();
            dte.Solution.SolutionBuild.Build(true);
            return dte.Solution.SolutionBuild.LastBuildInfo == 0;
        }

        public string GetLatestBuildLocation()
        {
            System.IO.FileInfo file = new System.IO.FileInfo(this.currentProject.FullName);
            return System.IO.Path.Combine(file.Directory.FullName, currentProject.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value.ToString());
        }
    }
}