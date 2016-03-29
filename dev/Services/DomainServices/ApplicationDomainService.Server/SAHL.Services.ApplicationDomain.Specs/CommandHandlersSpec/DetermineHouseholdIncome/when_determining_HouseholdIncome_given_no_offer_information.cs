using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using System;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.DetermineApplicationHouseholdIncome
{
    public class when_determining_HouseholdIncome_given_no_offer_information : WithCoreFakes
    {
        private static DetermineApplicationHouseholdIncomeCommand command;
        private static DetermineApplicationHouseholdIncomeCommandHandler handler;

        private static IApplicationDataManager applicationDataManager;
        private static IApplicantDataManager applicantDataManager;
        private static int applicationNumber;

        private static OfferInformationDataModel offerInformationDataModel;
        private static OfferInformationVariableLoanDataModel offerInformationVariableLoanDataModel;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            applicantDataManager = An<IApplicantDataManager>();
            applicationNumber = 1;
            var domainRuleManager = An<IDomainRuleManager<OfferInformationDataModel>>();

            offerInformationDataModel = new OfferInformationDataModel(DateTime.Now, applicationNumber, (int)OfferInformationType.OriginalOffer, "", DateTime.Now, (int)Product.NewVariableLoan);
            applicationDataManager.WhenToldTo(x => x.GetLatestApplicationOfferInformation(applicationNumber)).Return(offerInformationDataModel);
            offerInformationVariableLoanDataModel = null;
            applicationDataManager.WhenToldTo(x => x.GetApplicationInformationVariableLoan(Param.IsAny<int>())).Return(offerInformationVariableLoanDataModel);

            command = new DetermineApplicationHouseholdIncomeCommand(applicationNumber);
            handler = new DetermineApplicationHouseholdIncomeCommandHandler(applicationDataManager, unitOfWorkFactory, applicantDataManager, eventRaiser, domainRuleManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_not_raise_the_household_income_determined_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(
                 Param.IsAny<DateTime>()
               , Param.IsAny<ApplicationHouseholdIncomeDeterminedEvent>()
               , Param.IsAny<int>()
               , Param.IsAny<int>()
               , Param.IsAny<IServiceRequestMetadata>()
           ));
        };

        private It should_add_an_error_message_to_the_messages_collection = () =>
        {
            messages.ErrorMessages().ShouldContain(x => x.Message.Contains("No OfferInformationVariableLoan was found for application"));
        };
    }
}