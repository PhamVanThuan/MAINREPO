using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SAHomeloans.SAHL_VS_WebJS.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace SAHomeloans.SAHL_VS_WebJS
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    //[DefaultRegistryRoot("Software\\Microsoft\\VisualStudio\\12.0")]
    [ProvideAutoLoad(Microsoft.VisualStudio.Shell.Interop.UIContextGuids.NoSolution)]
    [ProvideAutoLoad(Microsoft.VisualStudio.Shell.Interop.UIContextGuids.SolutionExists)]
    [Guid(GuidList.guidSAHL_VS_WebJSPkgString)]
    [ProvideObject(typeof(GeneralPropertyPage))]
    [ProvideProjectFactory(typeof(WebJSProjectFactory), "Javascript", "WebJS Project Files (*.webjsproj);*.webjsproj", "webjsproj", "webjsproj", @"ProjectTemplates", LanguageVsTemplate = "WebJS", NewProjectRequireNewFolderVsTemplate = false)]
    [ProvideProjectItem(typeof(WebJSProjectFactory), "SAHL WebJS Items", @"Templates", 500)]
    public sealed class WebJSPackage : ProjectPackage, IVsInstalledProduct
    {
        #region

        public override string ProductUserContext
        {
            get { return "WebJSProj"; }
        }

        #endregion

        #region Package Members

        protected override void Initialize()
        {
            base.Initialize();
            DependancyInstaller dependencyInstaller = new DependancyInstaller(this);
            this.RegisterProjectFactory(new WebJSProjectFactory(this));
            string version = "";
            this.ProductID(out version);
            
            dependencyInstaller.Dependancy("targetsInstalled", version, () =>
            {
                string _assemblyString = Path.GetDirectoryName(typeof(WebJSPackage).Assembly.Location);
                string msBuildPath = Environment.GetEnvironmentVariable("ProgramFiles")+"\\MSBuild";
                string sahlMsbuildPath = msBuildPath+"\\SAHomeloans";

                if (Directory.Exists(sahlMsbuildPath))
                {
                    Directory.Delete(sahlMsbuildPath,true);
                }
                Directory.CreateDirectory(sahlMsbuildPath);
                Directory.CreateDirectory(sahlMsbuildPath+"\\bin");

                File.Copy(_assemblyString + "\\SAHL.VS.Build.dll", sahlMsbuildPath + "\\bin\\SAHL.VS.Build.dll");
                File.Copy(_assemblyString + "\\MSBuild\\webJs\\SAHL.VS.WebJS.targets", sahlMsbuildPath + "\\SAHL.VS.WebJS.targets");
                File.Copy(_assemblyString + "\\MSBuild\\webJs\\SAHL.VS.WebNG.targets", sahlMsbuildPath + "\\SAHL.VS.WebNG.targets");
            });
        }

        #endregion

        public int IdBmpSplash(out uint pIdBmp)
        {
            pIdBmp = 0u;
            return 0;
        }

        public int IdIcoLogoForAboutbox(out uint pIdIco)
        {
            pIdIco = 0u;
            return 0;
        }

        public int OfficialName(out string pbstrName)
        {
            pbstrName = "WebJS";
            return 0;
        }

        public int ProductDetails(out string pbstrProductDetails)
        {
            pbstrProductDetails = "WebJS";
            return 0;
        }

        public int ProductID(out string pbstrPID)
        {
            pbstrPID = base.GetType().Assembly.GetName().Version.ToString();
            return 0;
        }
    }
};