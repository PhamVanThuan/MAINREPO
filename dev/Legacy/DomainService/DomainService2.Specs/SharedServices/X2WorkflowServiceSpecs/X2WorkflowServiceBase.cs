using DomainService2.SharedServices;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.SharedServices.X2WorkflowServiceSpecs
{
    public class X2WorkflowServiceBase : WithFakes
    {
        protected static IX2WorkflowService X2Service;
        protected static IX2Repository X2Repository;
        protected static ICastleTransactionsService castleTransactionsService;
        protected static IDomainMessageCollection messages;
    }
}