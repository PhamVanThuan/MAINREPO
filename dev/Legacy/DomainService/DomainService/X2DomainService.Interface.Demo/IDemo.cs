using System;
using SAHL.Common.Collections.Interfaces;
using SAHL.X2.Common;

namespace X2DomainService.Interface.Demo
{
    public interface IDemo
    {
        bool HasSomeConditionBeenMet(IDomainMessageCollection messages, Int64 instanceID, int GenericKey);
        void DoATransactionalSave(IDomainMessageCollection messages, Int64 instanceID, int genericKey, string userPerformingAction);
        void CreateEWorkCase(IDomainMessageCollection messages, int genericKey, out string efolderID);
        void PerformEWorkAction(IDomainMessageCollection messages, string efolderID, string actionToPerform);
    }
}
