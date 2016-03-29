using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using SAHL.Common.Service.Interfaces;


namespace SAHL.Common.Service.Base
{
    public abstract class RemotePluginDomainLoader : MarshalByRefObject
    {

        public override Object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// Used to keep track of whether proxies are in use. Multiple "GET" calls can be made to the 
        /// mutex. However the same number of "RELEASE" calls MUST be made to fully release the mutex
        /// 
        /// SO: 3x Proxy classes are created 2 call release. The PluginDomain loader want to unload
        /// the appDomain at this point. It is forced to wait until the 3rd proxy has "DISPOSED" until
        /// it can OWN th mutex. This is how we ensure that all workers in the remote domain are
        /// complete before unloading the remote domain.
        /// </summary>
        protected Assembly pluginAsm = null;

        internal abstract bool CheckAssembly(Assembly asm);

        internal bool LoadAssemblies(PluginManager pluginManager)
        {
            try
            {
                FileStream stream = File.OpenRead(pluginManager.FullPath);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
                stream.Close();
                Assembly asm = null;

                //this wont show in VS.NET, but symbols IS in fact loaded
                string pdbName = pluginManager.DebugFullPath;
                if (!String.IsNullOrEmpty(pdbName))
                {
                    stream = File.OpenRead(pdbName);
                    byte[] debugbuffer = new byte[stream.Length];
                    stream.Read(debugbuffer, 0, (int)stream.Length);
                    stream.Close();
                    asm = Assembly.Load(buffer, debugbuffer);
                }
                else
                {
                    asm = Assembly.Load(buffer);
                }
                if (CheckAssembly(asm))
                {
                    pluginAsm = asm;
                }
                Type[] types = asm.GetExportedTypes();
                return true;
            }
            catch 
            {
                throw;
            }
        }

    }
}
