using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_updating_an_attorney : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static AttorneyDataModel attorney;
        private static UpdateAttorneyCommand command;
        private static UpdateAttorneyCommandHandler commandHandler;
        private static ISystemMessageCollection actualResult;
        private static IServiceRequestMetadata metadata;

        private Establish context = () =>
         {
             testDataManager = An<ITestDataManager>();
             metadata = An<IServiceRequestMetadata>();
             attorney = new AttorneyDataModel((int)DeedsPropertyType.Unit, "VishavP", 5, 123456, 6, 7, true, 1235888, true, (int)GeneralStatus.Active);
             command = new UpdateAttorneyCommand(attorney);
             commandHandler = new UpdateAttorneyCommandHandler(testDataManager);
         };

        private Because of = () =>
         {
             actualResult = commandHandler.HandleCommand(command, metadata);
         };

        private It should_update_the_attorney = () =>
        {
            testDataManager.
             WasToldTo
              (x => x.UpdateAttorney
               (Arg.Is<AttorneyDataModel>(y => y.AttorneyContact == attorney.AttorneyContact
                 && y.DeedsOfficeKey == attorney.DeedsOfficeKey && y.GeneralStatusKey == attorney.GeneralStatusKey)));
        };

        private It should_return_messages = () =>
         {
             actualResult.ShouldNotBeNull();
         };
    }
}