using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.DetermineApplicationHouseholdIncome
{
    public class when_determining_HouseholdIncome_given_confirmed_income : WithCoreFakes
    {
        private static DetermineApplicationHouseholdIncomeCommand command;
        private static DetermineApplicationHouseholdIncomeCommandHandler handler;

        private static IApplicationDataManager applicationDataManager;
        private static IApplicantDataManager applicantDataManager;

        private static int applicationNumber;
        private static int applicationInformationNumber;

        private static OfferInformationDataModel offerInformationDataModel;
        private static OfferInformationVariableLoanDataModel offerInformationVariableLoanDataModel;
        private static IEnumerable<EmploymentDataModel> incomeContributorEmployments;

        private static double initialHouseholdIncome;
        private static double confirmedIncome = 7000;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            applicantDataManager = An<IApplicantDataManager>();
            eventRaiser = An<IEventRaiser>();
            applicationNumber = 1;
            applicationInformationNumber = 123;
            var domainRuleManager = An<IDomainRuleManager<OfferInformationDataModel>>();

            initialHouseholdIncome = 0;

            offerInformationDataModel = new OfferInformationDataModel(applicationInformationNumber, DateTime.Now, applicationNumber, (int)OfferInformationType.OriginalOffer, "", DateTime.Now, 
                (int)Product.NewVariableLoan);
            applicationDataManager.WhenToldTo(x => x.GetLatestApplicationOfferInformation(applicationNumber)).Return(offerInformationDataModel);
            offerInformationVariableLoanDataModel = new OfferInformationVariableLoanDataModel(applicationInformationNumber, null, null, null, null, null, initialHouseholdIncome, null, null, null, 
                null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            applicationDataManager.WhenToldTo(x => x.GetApplicationInformationVariableLoan(offerInformationDataModel.OfferInformationKey)).Return(offerInformationVariableLoanDataModel);

            incomeContributorEmployments = new List<EmploymentDataModel>()
            {
                new EmploymentDataModel(0, null, (int)EmploymentType.Salaried, (int)RemunerationType.Salaried, (int)EmploymentStatus.Current, 1, null, null, "", "", "", "", null, "", null, "", 3, 
                    null, null, null, null, null, null, null, null, null, 0, null, null, null, null, null, null, null, null, null, "", 0, confirmedIncome, null, null, null, null, null)
            };
            applicantDataManager.WhenToldTo(x => x.GetIncomeContributorApplicantsCurrentEmployment(applicationNumber)).Return(incomeContributorEmployments);

            command = new DetermineApplicationHouseholdIncomeCommand(applicationNumber);
            handler = new DetermineApplicationHouseholdIncomeCommandHandler(applicationDataManager, unitOfWorkFactory, applicantDataManager, eventRaiser, domainRuleManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_set_the_application_information_variable_loan_household_income_using_confirmed_income = () =>
        {
            offerInformationVariableLoanDataModel.HouseholdIncome.Value.ShouldEqual(confirmedIncome);
        };

        private It should_update_the_application_information_variable_loan = () =>
        {
            applicationDataManager.WasToldTo(x => x.UpdateApplicationInformationVariableLoan(offerInformationVariableLoanDataModel));
        };
    }
}
