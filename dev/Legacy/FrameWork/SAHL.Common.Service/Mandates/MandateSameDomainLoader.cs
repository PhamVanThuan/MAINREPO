using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Base;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.Service.Mandates
{
    internal class MandateSameDomainLoader : SameDomainLoader
    {
        protected static RemotePluginDomainLoader loader;
        protected static PluginManager pluginManager;

        static  MandateSameDomainLoader()
        {
            if (null == pluginManager)
                pluginManager = MandatePluginManager.Access();
            if (null == loader)
            {
                loader = new MandateRemoteDomainLoader();
                loader.LoadAssemblies(pluginManager);
            }
        }
        protected override bool CreateRemoteLoader()
        {
            try
            {
                if (loader.LoadAssemblies(pluginManager))
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create Plugin Domain.", ex);
            }
        }
        public static IMandate GetPlugin(string MandateTypeName)
        {
            try
            {
                MandateRemoteDomainLoader ml = (MandateRemoteDomainLoader)loader;
                IMandate i = ml.GetIPluginProxy(MandateTypeName);
                return i;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Unable to get plugin {0}", MandateTypeName), ex);
            }
        }
    }
}
