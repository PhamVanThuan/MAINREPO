using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet;
using System.Linq;

namespace SAHL.Tools.MSBuildExtensions
{
    public class GetLatestVersion : Task
    {
        public string Project
        {
            get;
            set;
        }

        public string NugetUrl
        {
            get;
            set;
        }

        public string HgFolder
        {
            get;
            set;
        }

        [Output]
        public string VersionNumber
        {
            get;
            set;
        }

        public override bool Execute()
        {
            Mercurial.Repository repo = new Mercurial.Repository(HgFolder);
            string branchName = repo.Branch();
            branchName = GetParentofFirstRevisionForBranch(repo, branchName);
            this.VersionNumber = GetVersionNumberFromNuget(branchName);
            return true;
        }

        internal string GetVersionNumberFromNuget(string branchName)
        {
            var context = System.Security.Principal.WindowsIdentity.GetCurrent().Impersonate();
            
            string maxRevisionNumber = "0.1.";

            if (branchName != "default")
            {
                maxRevisionNumber = branchName.Replace("x", "");
            }

            PackageRepositoryFactory.Default.HttpClientFactory = uri =>
            {
                HttpClient client = new HttpClient(uri);
                client.SendingRequest += client_SendingRequest;
                return client;
            };
            IPackageRepository packageRepo = PackageRepositoryFactory.Default.CreateRepository(NugetUrl);

            var packages = packageRepo.GetPackages().Where(x => x.Id == Project).AsEnumerable();

            packages = packages.Where(x => x.Version.ToString().StartsWith(maxRevisionNumber));
            var max = packages.Max(x => x.Version);
            IPackage package = packages.SingleOrDefault(x => x.Version == max);
            context.Undo();
            if (package != null)
            {
                return package.Version.ToString();
            }

            return "0.0.0.0";
        }

        private void client_SendingRequest(object sender, WebRequestEventArgs e)
        {
            e.Request.UseDefaultCredentials = true;
            e.Request.Credentials = System.Net.CredentialCache.DefaultCredentials;
            e.Request.Proxy = System.Net.HttpWebRequest.DefaultWebProxy;
            e.Request.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
        }

        private string GetParentofFirstRevisionForBranch(Mercurial.Repository currentRepo, string branchName)
        {
            string parentBranchName = branchName;
            if (parentBranchName[0] == '#')
            {
                Mercurial.LogCommand command = new Mercurial.LogCommand();
                command.AdditionalArguments.Add(string.Format("-r \"parents(min(branch('{0}')))\"", branchName));
                var parent = currentRepo.Log(command).FirstOrDefault();
                parentBranchName = parent.Branch;
                return GetParentofFirstRevisionForBranch(currentRepo, parentBranchName);
            }
            return parentBranchName;
        }
    }
}