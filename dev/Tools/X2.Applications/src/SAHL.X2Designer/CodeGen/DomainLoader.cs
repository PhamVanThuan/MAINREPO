using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading;

namespace SAHL.X2Designer.CodeGen
{
    public class DomainLoaderOldDoNotUse : MarshalByRefObject
    {
        protected static string AppDomainName = "";
        protected static FileSystemWatcher fsw = new FileSystemWatcher();
        internal static AppDomain Domain;
        static ManualResetEvent mre = new ManualResetEvent(false);
        protected static string BuildFolder = @"C:\Development\RnD\RemoteAppdomainCompile\RemoteAppdomainCompile\bin\Debug\Build";
        protected static string RemoteLoaderClassName = "";
        public static object syncObj = new object();

        static DomainLoaderOldDoNotUse()
        {
            if (!Start())
                throw new Exception("DomainLoader Failed to start.");
        }

        protected static bool Start()
        {
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name + ".exe";

            try
            {
                if (null != Domain)
                    AppDomain.Unload(Domain);
                AppDomainName = "X2.Compile";
                string Path = AppDomain.CurrentDomain.BaseDirectory;
                // copy *this* file to the build folder as the remoteloader class is contained in *this* file

                BuildFolder = Path + "Build";
                //Directory.Delete(BuildFolder, true);
                if (!Directory.Exists(BuildFolder))
                    Directory.CreateDirectory(BuildFolder);

                string s = string.Format("{0}\\Build\\{1}", Path, assemblyName);
                if (!File.Exists(s))
                    File.Copy(Path + "\\" + assemblyName, Path + "\\Build\\" + assemblyName, true);

                // Setup New AppDomain params
                AppDomainSetup appSetup = new AppDomainSetup();
                // Where to start looking for assemblies (eg the one that contains the remoteloader)
                appSetup.ApplicationBase = BuildFolder;
                ApplicationTrust at = new System.Security.Policy.ApplicationTrust();
                at.DefaultGrantSet = new PolicyStatement(new PermissionSet(PermissionState.Unrestricted));
                appSetup.ApplicationTrust = at;

                // Name of new AppDomain
                appSetup.ApplicationName = AppDomainName;
                // Create the plugins domain.
                Domain = AppDomain.CreateDomain(AppDomainName, null, appSetup);

#if DEBUG
                Debug.WriteLine("Appdomain created.");
#endif
                if (!CreateRemoteLoader())
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to Start DomainLoader for assembly " + assemblyName + " : {0}", ex.ToString());
            }
            return false;
        }

        private static RemoteDomainLoader remoteloader;

        public static RemoteDomainLoader RemoteLoader
        {
            get
            {
                return remoteloader;
            }
        }

        protected static bool CreateRemoteLoader()
        {
            string assemblyName = Assembly.GetExecutingAssembly().FullName;

            try
            {
                remoteloader = (RemoteDomainLoader)Domain.CreateInstanceAndUnwrap(assemblyName, "SAHL.X2Designer.CodeGen.RemoteDomainLoader");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to create Domain for assembly " + assemblyName + " : {0}", ex.ToString());
                return false;
            }
        }

        public static IRemoteCompile GetCompiler()
        {
            try
            {
                while (!mre.WaitOne(0, false))
                {
                    IRemoteCompile i = (IRemoteCompile)remoteloader.GetProxyCompiler();
                    i.BaseDomainPath = BuildFolder;
                    return i;
                }
                return null;
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
            return null;
        }

        public static void UnloadCompileDomain()
        {
            mre.Set();
            if (!Start())
                throw new Exception("DomainLoader Failed to restart.");
            mre.Reset();
        }
    }
}