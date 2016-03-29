using DomainService2.SharedServices;
using DomainService2.Specs.DomainObjects;
using DomainService2.Workflow.Origination.FurtherLending;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.FurtherLending.ArchiveFLRelatedCasesCommandHandlerSpecs
{
    [Subject(typeof(ArchiveFLRelatedCasesCommandHandler))]
    public class When_the_related_case_workflow_state_is_valuation_hold : ArchiveFLRelatedCasesCommandHandlerBase
    {
        static long instanceID = 123456;
        static int applicationKey = 1111;
        static IEventList<IInstance> list;

        static IInstance valuationHoldInstance;
        static IState valuationHoldState;

        Establish context = () =>
            {
                x2Repository = An<IX2Repository>();
                x2WorkflowService = An<IX2WorkflowService>();

                messages = new DomainMessageCollection();
                command = new ArchiveFLRelatedCasesCommand(applicationKey, "aUser", instanceID);
                handler = new ArchiveFLRelatedCasesCommandHandler(x2Repository, x2WorkflowService);

                IInstance instance = An<IInstance>();
                x2Repository.WhenToldTo(x => x.GetInstanceByKey(Param.IsAny<long>())).
                    Return(instance);

                instance.WhenToldTo(x => x.ID).
                    Return(instanceID);

                valuationHoldInstance = An<IInstance>();
                valuationHoldState = An<IState>();
                valuationHoldState.WhenToldTo(x => x.Name).
                    Return("VALUATION HOLD");

                valuationHoldInstance.WhenToldTo(x => x.ID)
                    .Return(instanceID + 1);

                valuationHoldInstance.WhenToldTo(x => x.State).
                    Return(valuationHoldState);

                list = new StubEventList<IInstance>(new IInstance[] { valuationHoldInstance });

                x2Repository.WhenToldTo(x => x.GetChildInstances(Param.IsAny<int>())).
                    Return(list);
            };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_retrieve_all_child_related_cases = () =>
        {
            x2Repository.WasToldTo(x => x.GetChildInstances(command.InstanceID));
        };

        It should_archive_valuations_for_the_instance = () =>
        {
            x2WorkflowService.WasToldTo(x => x.ArchiveValuationsFromSourceInstanceID(valuationHoldInstance.ID, command.ADUser, command.ApplicationKey));
        };

        It should_not_archive_quickcash = () =>
        {
            x2WorkflowService.WasNotToldTo(x => x.ArchiveQuickCashFromSourceInstanceID(Param.IsAny<long>(), Param.IsAny<string>(), Param.IsAny<int>()));
        };

        It should_not_archive_other_case_types = () =>
        {
            x2Repository.WasNotToldTo(x => x.CreateAndSaveActiveExternalActivity(Param.Is<string>("EXTArchiveMainCase"), Param.IsAny<long>(), Param.Is<string>(SAHL.Common.Constants.WorkFlowName.ApplicationManagement), Param.Is<string>(SAHL.Common.Constants.WorkFlowProcessName.Origination), Param.IsAny<string>()));
        };

        It should_createandsave_an_external_activity_to_perform_the_archive = () =>
        {
            x2Repository.WasToldTo(x => x.CreateAndSaveActiveExternalActivity("EXTArchiveValuation", valuationHoldInstance.ID, SAHL.Common.Constants.WorkFlowName.ApplicationManagement, SAHL.Common.Constants.WorkFlowProcessName.Origination, null));
        };
    }
}