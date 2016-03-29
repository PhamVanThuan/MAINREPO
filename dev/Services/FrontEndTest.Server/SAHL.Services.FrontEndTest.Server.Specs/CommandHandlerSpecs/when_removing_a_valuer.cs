using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_removing_a_valuer : WithFakes
    {
        private static ValuatorDataModel valuer;
        private static RemoveValuerCommandHandler commandHandler;
        private static RemoveValuerCommand command;
        private static int valuatorKey;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;
        private static ITestDataManager testDataManager;

        private Establish context = () =>
        {
            valuatorKey = 12092115;
            testDataManager = An<ITestDataManager>();
            metadata = An<IServiceRequestMetadata>();
            command = new RemoveValuerCommand(valuatorKey);
            commandHandler = new RemoveValuerCommandHandler(testDataManager);
            valuer = new ValuatorDataModel(valuatorKey,"Someone","Anyone",1,(int)GeneralStatus.Pending,123456789);
        };

        private Because of = () =>
         {
             messages = commandHandler.HandleCommand(command, metadata);
         };

        private It should_remove_the_valuer = () =>
         {
             testDataManager.WasToldTo(x=>x.RemoveValuer(valuatorKey));
         };

        private It should_return_messages = () =>
         {
             messages.ShouldNotBeNull();
         };

        private It should_not_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}