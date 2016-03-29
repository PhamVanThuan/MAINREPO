using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_inserting_a_valuation : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static IUnitOfWorkFactory UOWFactory;
        private static ILinkedKeyManager linkedKeyManager;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static ValuationDataModel model;
        private static InsertValuationCommand command;
        private static InsertValuationCommandHandler commandHandler;

        private Establish context = () =>
        {
            testDataManager = An<ITestDataManager>();
            UOWFactory = An<IUnitOfWorkFactory>();
            linkedKeyManager = An<ILinkedKeyManager>();
            metadata = An<IServiceRequestMetadata>();
            model = new ValuationDataModel(1,1,DateTime.Now,10,100,100,"",1,0,0,0,DateTime.Now,1,1,1,"",1,false,1);
            command = new InsertValuationCommand(model, Guid.NewGuid());
            commandHandler = new InsertValuationCommandHandler(testDataManager, linkedKeyManager, UOWFactory);
        };

        private Because OF = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_return_messages = () =>
        {
            messages.ShouldNotBeNull();
        };

        private It should_not_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        private It should_insert_the_valuation = () =>
        {
            testDataManager.WasToldTo(x => x.InsertValuation(model));
        };

        private It should_link_the_guid_to_the_key = () =>
        {
            linkedKeyManager.WasToldTo(x => x.LinkKeyToGuid(command.Valuation.ValuationKey, command.ValuationId));
        };
    }
}
