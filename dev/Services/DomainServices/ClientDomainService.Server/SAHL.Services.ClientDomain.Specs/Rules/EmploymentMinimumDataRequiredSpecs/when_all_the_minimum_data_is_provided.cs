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

namespace SAHL.Services.ClientDomain.Specs.Rules.EmploymentMinimumDataRequiredSpecs
{
    public class when_all_the_minimum_data_is_provided : WithFakes
    {
        private static EmploymentMinimumDataRequiredRule<SalaryDeductionEmploymentModel> rule;
        static IEmploymentDataManager dataManager;
        private static SalaryDeductionEmploymentModel model;
        private static ISystemMessageCollection messages;
        private static EmployerModel employer;

        private Establish context = () =>
        {
            dataManager = An<IEmploymentDataManager>();
            rule = new EmploymentMinimumDataRequiredRule<SalaryDeductionEmploymentModel>(dataManager);
            messages = SystemMessageCollection.Empty();
            employer = new EmployerModel(1111, "ABSA", "031", "1234567", "Max", "max@absa.co.za", EmployerBusinessType.Company, EmploymentSector.FinancialServices);
            model = new SalaryDeductionEmploymentModel(10000, 5000, 25, employer, DateTime.MinValue, SalaryDeductionRemunerationType.Salaried, EmploymentStatus.Current, null);
            var employerDataModel = new EmployerDataModel(employer.EmployerName, employer.TelephoneNumber, employer.TelephoneCode, employer.ContactPerson,
                employer.ContactEmail, "", "", "", "", "", (int)employer.EmployerBusinessType, "", null, (int)employer.EmploymentSector);
            dataManager.WhenToldTo(x => x.FindExistingEmployer(employer)).Return(new List<EmployerDataModel> { employerDataModel });
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_add_no_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}