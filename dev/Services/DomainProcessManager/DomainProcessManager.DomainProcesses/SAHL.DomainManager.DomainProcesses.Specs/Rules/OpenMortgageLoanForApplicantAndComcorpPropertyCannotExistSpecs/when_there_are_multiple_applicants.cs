using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.Rules;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Application;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Rules.OpenMortgageLoanForApplicantAndComcorpPropertyCannotExistSpecs
{
    public class when_there_are_multiple_applicants : WithFakes
    {
        private static ApplicantModel applicant1;
        private static ApplicantModel applicant2;
        private static List<AddressModel> addresses;
        private static ApplicationCreationModel model;
        private static ISystemMessageCollection messages;
        private static OpenMortgageLoanForApplicantAndComcorpPropertyCannotExistRule rule;
        private static IApplicationDataManager applicationDataManager;

        private Establish context = () =>
            {
                applicationDataManager = An<IApplicationDataManager>();
                applicationDataManager.WhenToldTo(x => x.DoesOpenApplicationExistForComcorpProperty(Param.IsAny<string>(), Param.IsAny<ComcorpApplicationPropertyDetailsModel>()))
                    .Return(false);
                addresses = ApplicationCreationTestHelper.PopulateAddresses();
                applicant1 = ApplicationCreationTestHelper.PopulateApplicantModel(addresses);
                applicant1.IDNumber = "8211045229080";
                applicant2 = ApplicationCreationTestHelper.PopulateApplicantModel(addresses);
                applicant2.IDNumber = "8401160057081";
                model = ApplicationCreationTestHelper.PopulateApplicationCreationModelWithApplicants(OfferType.RefinanceLoan, new List<ApplicantModel>() { applicant1, applicant2 });
                messages = SystemMessageCollection.Empty();
                rule = new OpenMortgageLoanForApplicantAndComcorpPropertyCannotExistRule(applicationDataManager);
            };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_check_for_an_existing_application_for_applicant_one = () =>
        {
            applicationDataManager.WasToldTo(x => x.DoesOpenApplicationExistForComcorpProperty(applicant1.IDNumber, model.ComcorpApplicationPropertyDetail));
        };

        private It should_check_for_an_existing_application_for_applicant_two = () =>
        {
            applicationDataManager.WasToldTo(x => x.DoesOpenApplicationExistForComcorpProperty(applicant2.IDNumber, model.ComcorpApplicationPropertyDetail));
        };
    }
}