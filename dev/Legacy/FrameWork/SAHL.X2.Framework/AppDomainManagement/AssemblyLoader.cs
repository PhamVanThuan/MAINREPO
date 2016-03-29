using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SAHL.Common.CacheData;
using SAHL.Common.DataAccess;
using SAHL.Common.Logging;
using SAHL.X2.Framework.Common;

namespace SAHL.X2.Framework
{
    public class AssemblyLoader : MarshalByRefObject
    {
        private List<Assembly> assemblyCache;

        private Dictionary<string, object> threadLocalStore;

        public AssemblyLoader()
        {
            this.assemblyCache = new List<Assembly>();
            this.threadLocalStore = new Dictionary<string, object>();
        }

        public AppDomain LoaderAppDomain
        {
            get
            {
                return AppDomain.CurrentDomain;
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void LoadAssembly(string assemblyPath)
        {
            string fn = AppDomain.CurrentDomain.FriendlyName;
            Assembly asmToLoad = Assembly.LoadFrom(assemblyPath);//(Path.GetFileNameWithoutExtension(assemblyPath));//
            this.assemblyCache.Add(asmToLoad);
        }

        public MarshalByRefObject CreateInstance(string typeName, BindingFlags bindingFlags,
            object[] constructorParams)
        {
            Assembly owningAssembly = null;
            foreach (Assembly assembly in assemblyCache)
            {
                if (assembly.GetType(typeName) != null)
                {
                    owningAssembly = assembly;
                    continue;
                }
            }
            if (owningAssembly == null)
            {
                throw new InvalidOperationException("Could not find owning assembly for type " + typeName);
            }
            MarshalByRefObject createdInstance = null;
            try
            {
                createdInstance = owningAssembly.CreateInstance(typeName, false, bindingFlags, null,
                    constructorParams, null, null) as MarshalByRefObject;
            }
            catch (Exception)
            {
            }
            if (createdInstance == null)
            {
                throw new ArgumentException("typeName must specify a Type that derives from MarshalByRefObject");
            }
            return createdInstance;
        }

        public IX2Process GetProcess()
        {
            MarshalByRefObject result = this.CreateInstance("Process", BindingFlags.CreateInstance, null);
            IX2Process process = result as IX2Process;
            return process;
        }

        public void InitialiseAppDomain()
        {
            string typeName = "DomainService2.IOC.DomainServiceLoader";
            Assembly owningAssembly = null;
            foreach (Assembly assembly in assemblyCache)
            {
                if (assembly.GetType(typeName) != null)
                {
                    owningAssembly = assembly;
                    continue;
                }
            }
            if (owningAssembly == null)
            {
                throw new InvalidOperationException("Could not find owning assembly for type " + typeName);
            }

            // this part is a bit hairy
            // create an instance of the DomainServiceLoader
            var ioc = owningAssembly.CreateInstance(typeName);

            // get its type
            Type iocType = Type.GetType("DomainService2.IOC.DomainServiceLoader, DomainService2.IOC");

            // get the static Instance property via reflection
            PropertyInfo pi = iocType.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            // access the property to create the singleton and intialise activerecord.
            pi.GetValue(ioc, null);
        }

        public void ClearDSCacheForAppDomain()
        {
            WorkflowSecurityRepositoryCacheHelper.Instance.ClearCache();
        }

        public ITransactionManager GetTransactionManager()
        {
            MarshalByRefObject result = CreateInstance("SAHL.Common.DataAccess.ActiveRecordTransactionManager", BindingFlags.CreateInstance, null);
            ITransactionManager tranManager = result as ITransactionManager;
            return tranManager;
        }

        public void ClearThreadLocalStore()
        {
            this.threadLocalStore.Clear();
        }

        public void SetThreadLocalStore(IDictionary<string, object> store)
        {
            this.threadLocalStore = store as Dictionary<string, object>;
        }

        public object GetThreadLocalStore()
        {
            return this.threadLocalStore;
        }

        public void SetThreadLocalStore(IDictionary<string, object> metricsContext, IDictionary<string, object> loggingContext)
        {
            SAHL.Common.Logging.Metrics.ThreadContext.Clear();
            foreach (KeyValuePair<string, object> kvp in metricsContext)
            {
                SAHL.Common.Logging.Metrics.ThreadContext.Add(kvp.Key, kvp.Value);
            }

            SAHL.Common.Logging.Logger.ThreadContext.Clear();
            foreach (KeyValuePair<string, object> kvp in loggingContext)
            {
                SAHL.Common.Logging.Logger.ThreadContext.Add(kvp.Key, kvp.Value);
            }
        }
    }
}