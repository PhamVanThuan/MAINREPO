using System.Collections.Generic;
using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement
{
    [Subject(typeof(ActivateNTUFromWatchdogTimeCommandHandler))]
    public class When_activating_NTU_from_watchdog_time : WithFakes
    {
        protected static ActivateNTUFromWatchdogTimeCommand command;
        protected static ActivateNTUFromWatchdogTimeCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static IX2Repository x2Repository;

        // Arrange
        Establish context = () =>
        {
            messages = An<IDomainMessageCollection>();
            x2Repository = An<IX2Repository>();
            IInstance instance = An<IInstance>();
            IState state = An<IState>();
            List<IInstance> instanceList = new List<IInstance>();

            state.WhenToldTo(x => x.Name).Return(Constants.WorkFlowExternalActivity.EXTWatchdog);
            instance.WhenToldTo(x => x.State).Return(state);
            instanceList.Add(instance);

            IEventList<IInstance> instances = new EventList<IInstance>(instanceList);
            x2Repository.WhenToldTo(x => x.GetChildInstances(Param<long>.IsAnything)).Return(instances);
            x2Repository.WhenToldTo(x => x.CreateAndSaveActiveExternalActivity(Param<string>.IsAnything, Param<long>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything));
            // mortgageLoanRepository.WhenToldTo(x => x.TermChange(Param<int>.IsAnything, Param<int>.IsAnything, Param<string>.IsAnything)).Throw(new Exception());
            command = new ActivateNTUFromWatchdogTimeCommand(Param<long>.IsAnything);
            handler = new ActivateNTUFromWatchdogTimeCommandHandler(x2Repository);
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_return_exception_for_invalid_instance = () =>
        {
            x2Repository.WasToldTo(x => x.CreateAndSaveActiveExternalActivity(Param<string>.IsAnything, Param<long>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything));
        };
    }
}