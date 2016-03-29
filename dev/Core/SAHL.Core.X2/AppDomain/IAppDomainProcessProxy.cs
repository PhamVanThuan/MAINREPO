using System;
using System.Collections.Generic;
using System.Reflection;
using System.Transactions;

namespace SAHL.Core.X2.AppDomain
{
    public interface IAppDomainProcessProxy
    {
        System.AppDomain LoaderAppDomain { get; }

        void LoadAssembly(string assemblyPath);

        MarshalByRefObject CreateInstance(string typeName, BindingFlags bindingFlags, object[] constructorParams);

        IX2Process GetProcess(string processName);

        void CreateStartables(string processName);

        IDictionary<string, object> GetTLSContents(Type participantType);

        void SetTransaction(Transaction currentTransaction);

        void ClearThreadLocalStore();

        void SetThreadLocalStore(IDictionary<string, object> store, Type participantType);

        object GetThreadLocalStore();
    }
}