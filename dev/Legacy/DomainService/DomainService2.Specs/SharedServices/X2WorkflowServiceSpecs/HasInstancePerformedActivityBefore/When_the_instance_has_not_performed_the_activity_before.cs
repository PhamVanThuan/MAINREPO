using DomainService2.SharedServices;
using DomainService2.Specs.DomainObjects;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace DomainService2.Specs.SharedServices.X2WorkflowServiceSpecs.HasInstancePerformedActivity
{
    [Subject(typeof(X2WorkflowService))]
    public class When_the_instance_has_not_performed_the_activity_before : X2WorkflowServiceBase
    {
        static bool result;
        static long instanceID;
        const string activityName = SAHL.Common.Constants.WorkFlowActivityName.CreditResubmit;

        Establish context = () =>
        {
            X2Repository = An<IX2Repository>();
            castleTransactionsService = An<ICastleTransactionsService>();
            X2Service = new X2WorkflowService(X2Repository, castleTransactionsService);
            IEventList<IWorkFlowHistory> workflowHistoryList = new StubEventList<IWorkFlowHistory>();

            X2Repository.WhenToldTo(x => x.GetWorkflowHistoryForInstanceAndActivity(instanceID, activityName)).
                Return(workflowHistoryList);
        };

        Because of = () =>
        {
            instanceID = 0;
            result = X2Service.HasInstancePerformedActivity(instanceID, activityName);
        };

        It should_get_the_history = () =>
        {
            X2Repository.WasToldTo(x => x.GetWorkflowHistoryForInstanceAndActivity(instanceID, activityName));
        };

        It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}