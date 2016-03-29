using System.Collections.Generic;
using DomainService2.Workflow.Origination.Credit;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Credit.PerformCreditMandateCheckCommandHandlerSpecs
{
    [Subject(typeof(PerformCreditMandateCheckCommandHandler))]
    public class When_performed : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static PerformCreditMandateCheckCommand command;
        protected static PerformCreditMandateCheckCommandHandler handler;
        protected static IWorkflowAssignmentRepository workflowAssignmentRepository;

        protected static int applicationKey = 1;
        protected static long instanceID = 1;
        protected static List<string> loadBalanceStates = new List<string> { "one", "two", "Three" };
        protected static bool loadBalanceIncludeStates = true;
        protected static bool loadBalance1stPass = true;
        protected static bool loadBalance2ndPass = true;

        // Arrange
        Establish context = () =>
        {
            workflowAssignmentRepository = An<IWorkflowAssignmentRepository>();

            command = new PerformCreditMandateCheckCommand(applicationKey, instanceID, loadBalanceStates, loadBalanceIncludeStates, loadBalance1stPass, loadBalance2ndPass);
            handler = new PerformCreditMandateCheckCommandHandler(workflowAssignmentRepository);
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_execute_PerformCreditMandateCheck = () =>
        {
            workflowAssignmentRepository.WasToldTo(x => x.PerformCreditMandateCheck(Param.IsAny<int>(),
                Param.IsAny<int>(),
                Param.IsAny<List<string>>(),
                Param.IsAny<bool>(),
                Param.IsAny<bool>(),
                Param.IsAny<bool>()));
        };

        //Assert
        It should_set_command_ApplicationKey = () =>
        {
            command.ApplicationKey.Equals(applicationKey);
        };

        //Assert
        It should_set_command_InstanceID = () =>
        {
            command.InstanceID.Equals(instanceID);
        };

        //Assert
        It should_set_command_LoadBalanceStates = () =>
        {
            command.LoadBalanceStates.Equals(loadBalanceStates);
        };

        //Assert
        It should_set_command_LoadBalanceIncludeStates = () =>
        {
            command.LoadBalanceIncludeStates.Equals(loadBalanceIncludeStates);
        };

        //Assert
        It should_set_command_LoadBalance1stPass = () =>
        {
            command.LoadBalance1stPass.Equals(loadBalance1stPass);
        };

        //Assert
        It should_set_command_LoadBalance2ndPass = () =>
        {
            command.LoadBalance2ndPass.Equals(loadBalance2ndPass);
        };
    }
}