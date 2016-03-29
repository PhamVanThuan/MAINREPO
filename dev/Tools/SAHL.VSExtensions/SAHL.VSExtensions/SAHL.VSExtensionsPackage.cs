using Microsoft.VisualStudio.Shell;
using SAHL.VSExtensions.Interfaces;
using SAHomeloans.SAHL_VSExtensions.Mappings;
using StructureMap;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SAHomeloans.SAHL_VSExtensions
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidSAHL_VSExtensionsPkgString)]
    [ProvideAutoLoad("ADFC4E64-0397-11D1-9F4E-00A0C911004F")]
    public sealed class SAHL_VSExtensionsPackage : Package
    {
        #region constructor

        public SAHL_VSExtensionsPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }

        #endregion constructor

        #region init

        protected override void Initialize()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            IOCConfig.Register(this);
            IVSMenuManager menuManager = ObjectFactory.GetInstance<IVSMenuManager>();
            menuManager.Initilize();
        }

        #endregion init
    }
}