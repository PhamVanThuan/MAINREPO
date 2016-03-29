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
    [InstalledProductRegistration("#120", "#122", "1.0", IconResourceID = 420)]
    //[DefaultRegistryRoot("Software\\Microsoft\\VisualStudio\\12.0")]
    [ProvideAutoLoad(Microsoft.VisualStudio.Shell.Interop.UIContextGuids.NoSolution)]
    [ProvideAutoLoad(Microsoft.VisualStudio.Shell.Interop.UIContextGuids.SolutionExists)]
    [Guid(GuidList.guidSAHL_VS_WebNGPkgString)]
    [ProvideObject(typeof(GeneralPropertyPage))]
    [ProvideProjectFactory(typeof(WebNGProjectFactory), "AngularJS", "AngularJS Project Files (*.webngproj);*.webngproj", "webngproj", "webngproj", @"ProjectTemplates", LanguageVsTemplate = "WebNG", NewProjectRequireNewFolderVsTemplate = false)]
    [ProvideProjectItem(typeof(WebNGProjectFactory), "SAHL AngularJS Items", @"Templates", 500)]

    public sealed class WebNGPackage : ProjectPackage, IVsInstalledProduct
    {
        #region

        public override string ProductUserContext
        {
            get { return "WebNGProj"; }
        }

        #endregion

        #region Package Members

        protected override void Initialize()
        {
            base.Initialize();
            this.RegisterProjectFactory(new WebNGProjectFactory(this));
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
            pbstrName = "WebNG";
            return 0;
        }

        public int ProductDetails(out string pbstrProductDetails)
        {
            pbstrProductDetails = "WebNG";
            return 0;
        }

        public int ProductID(out string pbstrPID)
        {
            pbstrPID = base.GetType().Assembly.GetName().Version.ToString();
            return 0;
        }
    }
}
