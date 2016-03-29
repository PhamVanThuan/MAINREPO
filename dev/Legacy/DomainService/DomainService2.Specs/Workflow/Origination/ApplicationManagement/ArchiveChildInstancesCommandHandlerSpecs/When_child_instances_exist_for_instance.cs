using System.Collections.Generic;
using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.ArchiveChildInstancesCommandHandlerSpecs
{
    [Subject(typeof(ArchiveChildInstancesCommandHandler))]
    public class When_child_instances_exist_for_instance : WithFakes
    {
        protected static ArchiveChildInstancesCommand command;
        protected static ArchiveChildInstancesCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static IX2Repository x2Repository;
        protected static long parentInstanceID = 1;
        protected static long appManValHoldchildInstanceID = 2;
        protected static long valuationschildInstanceID = 3;
        // Arrange
        Establish context = () =>
            {
                messages = An<IDomainMessageCollection>();
                x2Repository = An<IX2Repository>();
                IInstance appManValHoldInstance = An<IInstance>();
                IInstance valuationsChildInstance = An<IInstance>();
                IState state = An<IState>();
                IWorkFlow workflow = An<IWorkFlow>();
                List<IInstance> appManValHoldinstanceList = new List<IInstance>();
                List<IInstance> sourceInstanceList = new List<IInstance>();

                workflow.WhenToldTo(x => x.Name).Return(SAHL.Common.Constants.WorkFlowName.Valuations);
                valuationsChildInstance.WhenToldTo(x => x.WorkFlow).Return(workflow);
                valuationsChildInstance.WhenToldTo(x=>x.ID).Return(valuationschildInstanceID);

                // Test ValuationHold
                state.WhenToldTo(x => x.Name).Return(SAHL.Common.WorkflowState.ValuationHold);
                appManValHoldInstance.WhenToldTo(x => x.State).Return(state);
                appManValHoldinstanceList.Add(appManValHoldInstance);
                appManValHoldInstance.WhenToldTo(x => x.ID).Return(appManValHoldchildInstanceID);

                IEventList<IInstance> appManValHoldinstances = new EventList<IInstance>(appManValHoldinstanceList);
                x2Repository.WhenToldTo(x => x.GetChildInstances(Param<long>.IsAnything)).Return(appManValHoldinstances);
                x2Repository.WhenToldTo(x => x.CreateAndSaveActiveExternalActivity(Param<string>.IsAnything, Param<long>.Matches(y => y == appManValHoldchildInstanceID), Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything));

                sourceInstanceList.Add(valuationsChildInstance);
                IEventList<IInstance> sourceInstances = new EventList<IInstance>(sourceInstanceList);
                x2Repository.WhenToldTo(x => x.GetInstanceForSourceInstanceID(Param<long>.Matches(y => y == appManValHoldchildInstanceID))).Return(sourceInstances);
                x2Repository.WhenToldTo(x => x.CreateAndSaveActiveExternalActivity(Param<string>.IsAnything, Param<long>.Matches(y => y == valuationschildInstanceID), Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything));

                command = new ArchiveChildInstancesCommand(parentInstanceID, Param<string>.IsAnything);
                handler = new ArchiveChildInstancesCommandHandler(x2Repository);
            };

        // Act
        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        // Assert
        It should_raise_a_flag_to_archive_the_applicationmanagement_child_instances = () =>
            {
                x2Repository.WasToldTo(x => x.CreateAndSaveActiveExternalActivity(Param<string>.IsAnything, Param<long>.Matches(y=>y == appManValHoldchildInstanceID), Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything));
            };

        It should_raise_a_flag_to_cleanup_the_valuations_child_instances = () =>
            {
                x2Repository.WasToldTo(x => x.CreateAndSaveActiveExternalActivity(Param<string>.IsAnything, Param<long>.Matches(y => y == valuationschildInstanceID), Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything));
            };
    }
}