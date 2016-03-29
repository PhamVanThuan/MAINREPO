using System.Security.Principal;

using SAHL.Core;
using SAHL.Core.UI.ApplicationState.Managers;
using SAHL.Core.UI.Configuration;

using SAHL.Core.UI.Models;
using StructureMap;


namespace ConfigConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IPrincipal user = new WindowsPrincipal(WindowsIdentity.GetCurrent());

            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                    scan.WithDefaultConventions();
                    scan.LookForRegistries();
                });
            });
        }
    }
}