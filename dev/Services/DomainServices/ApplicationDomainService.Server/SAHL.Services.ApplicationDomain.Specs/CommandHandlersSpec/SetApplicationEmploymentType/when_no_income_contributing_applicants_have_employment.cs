using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.SetApplicationEmploymentType
{
    public class when_no_income_contributing_applicants_have_employment : WithCoreFakes
    {
        private static SetApplicationEmploymentTypeCommand command;
        private static SetApplicationEmploymentTypeCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;
        private static IApplicationManager applicationManager;
        private static IApplicantDataManager applicantDataManager;
        private static OfferInformationDataModel offerInformationDataModel;
        private static OfferInformationVariableLoanDataModel offerInformationVariableLoanDataModel;
        private static IEnumerable<EmploymentDataModel> employment;
        private static int applicationNumber;
        private static int applicationInformationNumber;
        private static IDomainRuleManager<OfferInformationDataModel> domainRuleManager;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            applicationManager = An<IApplicationManager>();
            applicantDataManager = An<IApplicantDataManager>();
            domainRuleManager = An<IDomainRuleManager<OfferInformationDataModel>>();
            applicationNumber = 1;
            applicationInformationNumber = 123;
            offerInformationDataModel = new OfferInformationDataModel(applicationInformationNumber, DateTime.Today.AddDays(-1), applicationNumber, (int)OfferInformationType.AcceptedOffer, 
                "bob", null, null);
            applicationDataManager.WhenToldTo(x => x.GetLatestApplicationOfferInformation(applicationNumber)).Return(offerInformationDataModel);

            offerInformationVariableLoanDataModel = new OfferInformationVariableLoanDataModel(applicationNumber, null, null, null, null, null, null, null, null, null, null, null, null, null, 
                null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            applicationDataManager.WhenToldTo(x => x.GetApplicationInformationVariableLoan(offerInformationDataModel.OfferInformationKey)).Return(offerInformationVariableLoanDataModel);
            employment = Enumerable.Empty<EmploymentDataModel>();
            applicantDataManager.WhenToldTo(x => x.GetIncomeContributorApplicantsCurrentEmployment(Param.IsAny<int>())).Return(employment);
            command = new SetApplicationEmploymentTypeCommand(applicationNumber);
            handler = new SetApplicationEmploymentTypeCommandHandler(unitOfWorkFactory, applicationDataManager, applicationManager, applicantDataManager, eventRaiser, domainRuleManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_not_determine_the_employment_type = () =>
        {
            applicationManager.WasNotToldTo(x => x.DetermineEmploymentTypeForApplication(Param.IsAny<IEnumerable<EmploymentDataModel>>()));
        };

        private It should_still_update_the_application = () =>
        {
            applicationDataManager.WasToldTo(x => x.UpdateApplicationInformationVariableLoan(Param.IsAny<OfferInformationVariableLoanDataModel>()));
        };

        private It should_still_raise_an_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<IEvent>(), Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}