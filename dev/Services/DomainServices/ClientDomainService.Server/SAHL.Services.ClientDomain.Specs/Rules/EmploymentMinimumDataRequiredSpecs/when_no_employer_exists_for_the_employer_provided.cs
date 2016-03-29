using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.EmploymentMinimumDataRequiredSpecs
{
    public class when_an_employer_exists_for_the_employer_provided : WithFakes
    {
        private static EmploymentMinimumDataRequiredRule<SalaryDeductionEmploymentModel> rule;
        private static IEmploymentDataManager dataManager;
        private static SalaryDeductionEmploymentModel model;
        private static ISystemMessageCollection messages;
        private static EmployerModel employer;
        private static IEnumerable<EmployerDataModel> existingEmployer;

        private Establish context = () =>
            {
                dataManager = An<IEmploymentDataManager>();
                existingEmployer = Enumerable.Empty<EmployerDataModel>();
                employer = new EmployerModel(0, "Test", "", "", "", "", EmployerBusinessType.Company, EmploymentSector.FinancialServices);
                model = new SalaryDeductionEmploymentModel(10000, 0, 25, employer, DateTime.MinValue, SalaryDeductionRemunerationType.Salaried, EmploymentStatus.Current, null);
                messages = SystemMessageCollection.Empty();
                rule = new EmploymentMinimumDataRequiredRule<SalaryDeductionEmploymentModel>(dataManager);
                dataManager.WhenToldTo(x => x.FindExistingEmployer(Param.IsAny<EmployerModel>())).Return(existingEmployer);
            };

        private Because of = () =>
            {
                rule.ExecuteRule(messages, model);
            };

        private It should_check_if_the_employer_exists = () =>
            {
                dataManager.WasToldTo(x => x.FindExistingEmployer(employer));
            };

        private It should_return_a_message = () =>
            {
                messages.ErrorMessages().First().Message.ShouldContain("An Employer is required when adding an employment record for a client.");
            };
    }
}