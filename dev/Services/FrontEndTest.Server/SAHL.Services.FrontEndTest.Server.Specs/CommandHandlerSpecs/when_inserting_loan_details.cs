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
    public class when_inserting_loan_details : WithFakes 
    {
        private static ITestDataManager testDataManager;
        private static IUnitOfWorkFactory uowFactory;
        private static ILinkedKeyManager linkedKeyManager;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;
        private static InsertLoanDetailCommand command;
        private static DetailDataModel model;
        private static InsertLoanDetailCommandHandler commandHandler;

        private Establish context = () =>
            {
                testDataManager = An<ITestDataManager>();
                uowFactory = An<IUnitOfWorkFactory>();
                linkedKeyManager = An<ILinkedKeyManager>();
                metadata = An<IServiceRequestMetadata>();
                model = new DetailDataModel(1,1,DateTime.Now,9,"",1,"VishavP",DateTime.Now);
                command = new InsertLoanDetailCommand(model,Guid.NewGuid());
                commandHandler = new InsertLoanDetailCommandHandler(testDataManager, linkedKeyManager,uowFactory );
            };

        private Because of = () =>
           {
               messages = commandHandler.HandleCommand(command, metadata);
           };

        private It should_not_return_error_messages = () =>
            {
                messages.HasErrors.ShouldBeFalse();
            };

        private It should_insert_the_loan_detail = () =>
            {
                testDataManager.WasToldTo(x=>x.InsertLoanDetail(model));
            };
    }
}
