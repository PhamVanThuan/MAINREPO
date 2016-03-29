using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Service.Base;


namespace SAHL.Common.Rules.Service
{
    /// <summary>
    /// 
    /// </summary>


    public class RulesRemotePluginDomainLoader : RemotePluginDomainLoader
    {

        Dictionary<string, IBusinessRule> TypeCache = new Dictionary<string, IBusinessRule>();

        internal override bool CheckAssembly(Assembly asm)
        {
            object[] Attrs = asm.GetCustomAttributes(typeof(RuleAssemblyTag), false);
            return (0 != Attrs.Length);
        }

        internal IBusinessRule GetRemotePlugin(string RuleName)
        {
            lock (this)
            {
                if (TypeCache.ContainsKey(RuleName))
                {
                    return TypeCache[RuleName];
                }
                try
                {
                    Type[] exportedTypes = pluginAsm.GetExportedTypes();
                    // Loop through the types and look for our custom attribute (FilterInfo)
                    for (int i = 0; i < exportedTypes.Length; i++)
                    {
                        // Look for our attribute. (PluginInfo)
                        object[] Attrs = exportedTypes[i].GetCustomAttributes(typeof(RuleInfo), false);
                        if (0 != Attrs.Length)
                        {
                            // Get a reference to the current type in the assembly.
                            Type type = exportedTypes[i];
                            if (type.FullName == RuleName)
                            {
                                IBusinessRule ibr = (IBusinessRule)pluginAsm.CreateInstance(type.FullName);
                                TypeCache.Add(RuleName, ibr);
                                return ibr;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new RuleException(ex.Message, ex);
                }
            }
            return null;
        }

        public IBusinessRule GetIPluginProxy(string RuleName)
        {
            RulesProxyIPlugin pp = new RulesProxyIPlugin(this, RuleName);
            return (pp);
        }
    }
}