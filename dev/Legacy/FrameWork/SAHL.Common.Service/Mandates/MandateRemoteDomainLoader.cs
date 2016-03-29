using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Base;
using SAHL.Common.Service.Interfaces;
using System.Reflection;

namespace SAHL.Common.Service.Mandates
{
    public class MandateRemoteDomainLoader : RemotePluginDomainLoader
    {
        Dictionary<string, IMandate> TypeCache = new Dictionary<string, IMandate>();

        internal override bool CheckAssembly(Assembly asm)
        {
            object[] Attrs = asm.GetCustomAttributes(typeof(MandateAssemblyTag), false);
            return (0 != Attrs.Length);
        }

        internal IMandate GetRemotePlugin(string MandateTypeName)
        {
            lock (this)
            {
                if (TypeCache.ContainsKey(MandateTypeName))
                {
                    return TypeCache[MandateTypeName];
                }
                try
                {
                    Type[] exportedTypes = pluginAsm.GetExportedTypes();
                    // Loop through the types and look for our custom attribute (FilterInfo)
                    for (int i = 0; i < exportedTypes.Length; i++)
                    {
                        // Look for our attribute. (PluginInfo)
                        object[] Attrs = exportedTypes[i].GetCustomAttributes(typeof(MandateInfo), false);
                        if (0 != Attrs.Length)
                        {
                            // Get a reference to the current type in the assembly.
                            Type type = exportedTypes[i];
                            if (type.FullName == MandateTypeName)
                            {
                                IMandate ibr = (IMandate)pluginAsm.CreateInstance(type.FullName);
                                TypeCache.Add(MandateTypeName, ibr);
                                return ibr;
                            }
                        }
                    }
                }
                catch 
                {
                    throw;
                }
            }
            return null;
        }

        public IMandate GetIPluginProxy(string RuleName)
        {
            MandateProxy pp = new MandateProxy(this, RuleName);
            return (pp);
        }
    }
}
