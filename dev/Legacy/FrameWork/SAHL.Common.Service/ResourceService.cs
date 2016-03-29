using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using System.Resources;
using SAHL.Common.Attributes;
using System.Reflection;

namespace SAHL.Common.Service
{
    [FactoryType(typeof(IResourceService), LifeTime = FactoryTypeLifeTime.Singleton)]
    public class ResourceService : IResourceService, IDisposable
    {
        #region IResourceService Members

        ResourceManager _applicationResources = null;
        protected ResourceManager ApplicationResources
        {
            get
            {
                if (_applicationResources == null)
                {
                    Assembly[] Asms = AppDomain.CurrentDomain.GetAssemblies();
                    Assembly SAHLWeb = null;
                    for (int i = 0; i < Asms.Length; i++)
                    {
                        if (Asms[i].FullName.StartsWith("SAHL.Web,"))
                        {
                            SAHLWeb = Asms[i];
                            break;
                        }
                    }
                    _applicationResources = new ResourceManager("SAHL.Web.Resources", SAHLWeb);
                }
                return _applicationResources;
            }
        }

        ResourceManager _frameworkResources = null;
        protected ResourceManager FrameworkResources
        {
            get
            {
                if (_frameworkResources == null)
                {
                    Assembly[] Asms = AppDomain.CurrentDomain.GetAssemblies();
                    Assembly SAHLCommonGlobals = null;
                    for (int i = 0; i < Asms.Length; i++)
                    {
                        if (Asms[i].FullName.StartsWith("SAHL.Common.Globals,"))
                        {
                            SAHLCommonGlobals = Asms[i];
                            break;
                        }
                    }
                    _frameworkResources = new ResourceManager("SAHL.Common.Globals.Resources", SAHLCommonGlobals);
                }
                return _frameworkResources;
            }
        }

        public string GetString(string ResourceID)
        {
            // search application resources first
            string Result = ApplicationResources.GetString(ResourceID);
            if (Result == null)
                Result = FrameworkResources.GetString(ResourceID);

            return Result == null ? "" : Result;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if(_applicationResources != null)
                _applicationResources.ReleaseAllResources();
            if(_frameworkResources != null)
                _frameworkResources.ReleaseAllResources();
        }

        #endregion
    }
}
