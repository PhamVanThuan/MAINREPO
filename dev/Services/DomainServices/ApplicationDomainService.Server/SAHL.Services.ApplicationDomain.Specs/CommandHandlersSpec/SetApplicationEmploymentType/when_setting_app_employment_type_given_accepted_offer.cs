using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using System;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.DetermineHouseholdIncome
{
    public class when_setting_app_employment_type_given_accepted_offer : WithCoreFakes
    {
        private static SetApplicationEmploymentTypeCommand command;
        private static SetApplicationEmploymentTypeCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;
        private static IApplicationManager applicationManager;
        private static IApplicantDataManager applicantDataManager;
        private static OfferInformationDataModel offerInformationDataModel;
        private static IDomainRuleManager<OfferInformationDataModel> domainRuleManager;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            applicationManager = An<IApplicationManager>();
            applicantDataManager = An<IApplicantDataManager>();
            domainRuleManager = new DomainRuleManager<OfferInformationDataModel>();

            offerInformationDataModel = new OfferInformationDataModel(DateTime.Today.AddDays(-1), 123, (int)OfferInformationType.AcceptedOffer, "bob", null, null);
            applicationDataManager.WhenToldTo(x => x.GetLatestApplicationOfferInformation(123)).Return(offerInformationDataModel);

            command = new SetApplicationEmploymentTypeCommand(123);
            handler = new SetApplicationEmploymentTypeCommandHandler(unitOfWorkFactory, applicationDataManager, applicationManager, applicantDataManager, eventRaiser, domainRuleManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_return_error_message = () =>
        {
            messages.HasErrors.ShouldBeTrue();
            messages.ErrorMessages().ShouldContain(x => x.Message.Contains("Application employment type cannot be set once the application has been accepted"));
        };
    }
}