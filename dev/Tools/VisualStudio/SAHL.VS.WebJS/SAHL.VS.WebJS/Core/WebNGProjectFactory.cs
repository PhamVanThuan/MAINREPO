using Microsoft.VisualStudio.Project;
using SAHomeloans.SAHL_VS_WebJS;
using System;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace SAHomeloans.SAHL_VS_WebJS.Core
{
    [System.Runtime.InteropServices.Guid(GuidList.WebNGProjectFactory)]
    public class WebNGProjectFactory : ProjectFactory
    {
        private WebNGPackage package;

        public WebNGProjectFactory(WebNGPackage package)
            : base(package)
        {
            this.package = package;
        }

        protected override ProjectNode CreateProject()
        {
            WebNGProjectNode project = new WebNGProjectNode(this.package);
            project.SetSite((IOleServiceProvider)((IServiceProvider)this.package).GetService(typeof(IOleServiceProvider)));
            return project;
        }
    }
}