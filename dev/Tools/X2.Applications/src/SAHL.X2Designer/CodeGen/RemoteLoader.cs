using System;

namespace SAHL.X2Designer.CodeGen
{
    public class RemoteDomainLoader : MarshalByRefObject
    {
        IRemoteCompile Compiler = new RemoteCompile();

        public override Object InitializeLifetimeService()
        {
            return null;
        }

        public RemoteDomainLoader()
        {
        }

        static object syncObj = new object();

        public IRemoteCompile GetCompiler()
        {
            Console.WriteLine("In Appdomain: {0}", AppDomain.CurrentDomain.FriendlyName);
            return Compiler;
        }

        public IRemoteCompile GetProxyCompiler()
        {
            ProxyCompiler pp = new ProxyCompiler(this);
            return (pp);
        }
    }
}