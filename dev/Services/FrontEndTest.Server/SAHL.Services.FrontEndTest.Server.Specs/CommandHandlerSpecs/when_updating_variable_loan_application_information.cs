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
    public class when_updating_variable_loan_application_information : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static UpdateVariableLoanApplicationInformationCommand command;
        private static OfferInformationVariableLoanDataModel model;
        private static int offerInformationKey;
        private static UpdateVariableLoanApplicationInformationCommandHandler commandHandler;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
         {
             offerInformationKey = 12345;
             model = new OfferInformationVariableLoanDataModel(offerInformationKey, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                                              (int)EmploymentType.Salaried, (int)RateAdjustmentGroup.CounterRate,
                                                              (int)CreditCriteriaAttributeType.NewBusiness,
                                                              (int)CreditCriteriaAttributeType.FurtherLending_AlphaHousing, 6);
             command = new UpdateVariableLoanApplicationInformationCommand(model);
             testDataManager = An<ITestDataManager>();
             commandHandler = new UpdateVariableLoanApplicationInformationCommandHandler(testDataManager);
             metadata = An<IServiceRequestMetadata>();
         };

        private Because of = () =>
         {
             messages = commandHandler.HandleCommand(command, metadata);
         };

        private It should_update_the_variableLoanInfo = () =>
         {
             testDataManager.WasToldTo(x => x.UpdateVariableLoanApplicationInformationStatement(model));
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