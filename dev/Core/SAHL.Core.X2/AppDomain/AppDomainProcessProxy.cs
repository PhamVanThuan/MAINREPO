using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Transactions;
using SAHL.Core.X2.Shared;
using StructureMap;

namespace SAHL.Core.X2.AppDomain
{
    public class AppDomainProcessProxy : MarshalByRefObject, IAppDomainProcessProxy
    {
        private static readonly ThreadLocal<Dictionary<string, object>> threadLocalStore = new ThreadLocal<Dictionary<string, object>>(() => new Dictionary<string, object>());
        private static readonly object startablesLock = new object();
        private readonly List<Assembly> assemblyCache;

        private readonly System.Collections.Concurrent.ConcurrentDictionary<string, object> loadedProcesses;
        private readonly List<Startable> startables;

        public AppDomainProcessProxy()
        {
            assemblyCache = new List<Assembly>();
            startables = new List<Startable>();
            loadedProcesses = new System.Collections.Concurrent.ConcurrentDictionary<string, object>();
        }

        public void LoadAssembly(string assemblyPath)
        {
            Assembly asmToLoad = Assembly.LoadFrom(assemblyPath);
            this.assemblyCache.Add(asmToLoad);
            var types = asmToLoad.GetTypes();

            foreach (var type in types)
            {
                if (type.GetInterface("IDomainServiceStartable", false) == null)
                {
                    continue;
                }
                Startable startable = new Startable() {Assembly = asmToLoad, Type = type};
                startables.Add(startable);
            }
        }

        public MarshalByRefObject CreateInstance(string typeName, System.Reflection.BindingFlags bindingFlags, object[] constructorParams)
        {
            Assembly owningAssembly = null;
            foreach (Assembly assembly in assemblyCache)
            {
                if (assembly.GetType(typeName) != null)
                {
                    owningAssembly = assembly;
                }
            }
            if (owningAssembly == null)
            {
                throw new InvalidOperationException("Could not find owning assembly for type " + typeName);
            }
            MarshalByRefObject createdInstance = null;
            Exception remoteException = null;
            try
            {
                createdInstance = owningAssembly.CreateInstance(typeName, false, bindingFlags, null,
                    constructorParams, null, null) as MarshalByRefObject;
            }
            catch (Exception ex)
            {
                remoteException = ex;
            }
            if (createdInstance != null)
            {
                return createdInstance;
            }
            Console.Write("Trying to create: {0}, ex:{1}", typeName, remoteException == null ? "" : remoteException.ToString());
            if (remoteException == null)
            {
                throw new ArgumentException("typeName must specify a Type that derives from MarshalByRefObject");
            }
            throw remoteException;
        }

        public IX2Process GetProcess(string processName)
        {
            CreateStartables(processName);
            MarshalByRefObject result = this.CreateInstance(string.Format("{0}.Process", processName), BindingFlags.CreateInstance, null);
            IX2Process process = result as IX2Process;
            return process;
        }

        public void ClearThreadLocalStore()
        {
            if (threadLocalStore != null)
            {
                threadLocalStore.Value.Clear();
            }
        }

        public void SetThreadLocalStore(IDictionary<string, object> store, Type participantType)
        {
            object createdInstance = null;
            try
            {
                var type = participantType.GetInterfaces().FirstOrDefault(x => x != typeof (IParticipatesInThreadLocalStorage));
                if (type != null)
                {
                    createdInstance = ObjectFactory.GetInstance(type);
                }
                IParticipatesInThreadLocalStorage participatesInThreadLocalStorage = createdInstance as IParticipatesInThreadLocalStorage;
                participatesInThreadLocalStorage.SetThreadLocalStore(store);

                foreach (KeyValuePair<string, object> kvp in store)
                {
                    if (!threadLocalStore.Value.ContainsKey(kvp.Key))
                    {
                        threadLocalStore.Value.Add(kvp.Key, kvp.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                var exceptionMessage = String.Empty;
                try
                {
                    var serializedStore = Newtonsoft.Json.JsonConvert.SerializeObject(store);
                    exceptionMessage = String.Format("participant type: {0}, created instance: {1}", participantType.FullName, serializedStore);
                }
                catch (Exception)
                {
                    // Do nothing
                }
                throw new Exception(String.Format("Failed to set the Thread Local Store. Details include : {0}", exceptionMessage), ex);
            }
        }

        public IDictionary<string, object> GetTLSContents(Type participantType)
        {
            Assembly containingAssembly = assemblyCache.FirstOrDefault(assembly => assembly.GetType(participantType.FullName) != null);
            object createdInstance;
            try
            {
                createdInstance = containingAssembly.CreateInstance(participantType.FullName, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to GET TLS: {0}", ex.ToString());
                throw;
            }
            IParticipatesInThreadLocalStorage participatesInThreadLocalStorage = createdInstance as IParticipatesInThreadLocalStorage;
            return participatesInThreadLocalStorage.GetThreadLocalStore();
        }

        public object GetThreadLocalStore()
        {
            return threadLocalStore;
        }

        public System.AppDomain LoaderAppDomain
        {
            get { return System.AppDomain.CurrentDomain; }
        }

        public void CreateStartables(string processName)
        {
            string reFormattedProcessName = processName.Replace(" ", "_");

            if (this.loadedProcesses.ContainsKey(reFormattedProcessName))
            {
                return;
            }

            lock (startablesLock)
            {
                if (!this.loadedProcesses.ContainsKey(reFormattedProcessName))
                {
                    foreach (var startable in startables)
                    {
                        try
                        {
                            object o = startable.Assembly.CreateInstance(startable.Type.FullName);
                            if (o is IDomainServiceStartable)
                            {
                                ((IDomainServiceStartable) o).Start(reFormattedProcessName);
                            }
                        }
                        catch (Exception)
                        {
                            // Do nothing
                        }
                    }
                }
            }

            this.loadedProcesses.TryAdd(reFormattedProcessName, new object());
        }

        public void SetTransaction(Transaction currentTransaction)
        {
            Transaction.Current = currentTransaction;
        }

        public void InitialiseAppDomain()
        {
            const string typeName = "DomainService2.IOC.DomainServiceLoader";
            Assembly owningAssembly = null;
            foreach (Assembly assembly in assemblyCache)
            {
                if (assembly.GetType(typeName) != null)
                {
                    owningAssembly = assembly;
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

        public override object InitializeLifetimeService()
        {
            return null;
        }

        internal class Startable
        {
            internal Assembly Assembly { get; set; }

            internal Type Type { get; set; }
        }
    }
}