using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.EmployeeDeductionsCanOnlyContainOneOfEachTypeSpecs
{
    public class when_the_employee_deductions_are_empty : WithFakes
    {
        private static EmployeeDeductionsCanOnlyContainOneOfEachTypeRule rule;
        private static SalaryDeductionEmploymentModel salaryDeductionEmployment;
        private static IEnumerable<EmployeeDeductionModel> deductions;
        private static EmployerModel employer;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            employer = new EmployerModel(1, "Google", "031", "571500", "Bob", "test@google.co.za",
                                         EmployerBusinessType.Company, EmploymentSector.ITandElectronics);
            deductions = Enumerable.Empty<EmployeeDeductionModel>();
            salaryDeductionEmployment = new SalaryDeductionEmploymentModel(15000, 5000, 1, employer, DateTime.Now.AddYears(-1), 
                                                                           SalaryDeductionRemunerationType.Salaried, 
                                                                           EmploymentStatus.Current, deductions);
            rule = new EmployeeDeductionsCanOnlyContainOneOfEachTypeRule();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, salaryDeductionEmployment);
        };

        private It should_not_return_a_message = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}