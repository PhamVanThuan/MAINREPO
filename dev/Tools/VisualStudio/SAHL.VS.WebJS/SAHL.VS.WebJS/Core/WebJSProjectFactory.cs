using Microsoft.VisualStudio.Project;
using System;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace SAHomeloans.SAHL_VS_WebJS.Core
{
    [System.Runtime.InteropServices.Guid(GuidList.WebJSProjectFactory)]
    public class WebJSProjectFactory : ProjectFactory
    {
        private WebJSPackage package;

        public WebJSProjectFactory(WebJSPackage package)
            : base(package)
        {
            this.package = package;
        }

        protected override ProjectNode CreateProject()
        {
            WebJSProjectNode project = new WebJSProjectNode(this.package);
            project.SetSite((IOleServiceProvider)((IServiceProvider)this.package).GetService(typeof(IOleServiceProvider)));
            return project;
        }
    }
}