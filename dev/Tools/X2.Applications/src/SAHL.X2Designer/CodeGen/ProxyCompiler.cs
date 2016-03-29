using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using SAHL.X2Designer.Documents;

namespace SAHL.X2Designer.CodeGen
{
    public class ProxyCompiler : MarshalByRefObject, IRemoteCompile
    {
        protected RemoteDomainLoader RemoteLoader = null;

        public ProxyCompiler(RemoteDomainLoader rl)
        {
            this.RemoteLoader = rl;
        }

        public string BaseDomainPath
        {
            get
            {
                IRemoteCompile i = RemoteLoader.GetCompiler();
                return i.BaseDomainPath;
            }
            set
            {
                IRemoteCompile i = RemoteLoader.GetCompiler();
                i.BaseDomainPath = value;
            }
        }

        public List<string> UsingStatements
        {
            get
            {
                IRemoteCompile i = RemoteLoader.GetCompiler();
                return i.UsingStatements;
            }
            set
            {
                IRemoteCompile i = RemoteLoader.GetCompiler();
                i.UsingStatements = value;
            }
        }

        public void AddReferenceDLL(List<ReferenceItem> References)
        {
            IRemoteCompile i = RemoteLoader.GetCompiler();
            i.AddReferenceDLL(References);
        }

        public void AddSystemReference(List<string> SystemReferences)
        {
            IRemoteCompile i = RemoteLoader.GetCompiler();
            i.AddSystemReference(SystemReferences);
        }

        public CompilerResults Compile(string Code, string OutputFileName)
        {
            IRemoteCompile i = RemoteLoader.GetCompiler();
            return i.Compile(Code, OutputFileName);
        }

        public void ExecuteMethod(string MethodName, string TypeName)
        {
            IRemoteCompile i = RemoteLoader.GetCompiler();
            i.ExecuteMethod(MethodName, TypeName);
        }

        public List<ReferenceItem> LoadAssembly(string AssemblyName, string BinaryType, List<ReferenceItem> AlreadyLoaded)
        {
            IRemoteCompile i = RemoteLoader.GetCompiler();
            return i.LoadAssembly(AssemblyName, BinaryType, AlreadyLoaded);
        }

        //public List<ReferenceItem> LoadAssemblyFromZip(string AssemblyName, List<ReferenceItem> AlreadyLoaded)
        //{
        // IRemoteCompile i = RemoteLoader.GetCompiler();
        // return i.LoadAssemblyFromZip(AssemblyName, AlreadyLoaded);
        //}

        public ReferenceItem LoadAndCopyAssembly(string AssemblyPath)
        {
            IRemoteCompile i = RemoteLoader.GetCompiler();
            return i.LoadAndCopyAssembly(AssemblyPath);
        }

        public ReferenceItem LoadGlobalAssembly(string GlobalAssemblyName)
        {
            IRemoteCompile i = RemoteLoader.GetCompiler();
            return i.LoadGlobalAssembly(GlobalAssemblyName);
        }

        public List<Type> GetAllTypesInDomain(List<string> UsingStatements)
        {
            IRemoteCompile i = RemoteLoader.GetCompiler();
            return i.GetAllTypesInDomain(UsingStatements);
        }
    }
}