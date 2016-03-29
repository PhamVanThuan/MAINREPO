using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Commands
{
    internal class when_adding_unconfirmed_salary_deduction_employment_to_client
    {
        private static AddUnconfirmedSalaryDeductionEmploymentToClientCommand command;
        private static OriginationSource originationSource;
        private static SalaryDeductionEmploymentModel salaryDeductionEmploymentModel;

        private Establish context = () =>
        {
            originationSource = OriginationSource.SAHomeLoans;
            salaryDeductionEmploymentModel =
                new SalaryDeductionEmploymentModel(10000, 0, 25,
                    new EmployerModel(1111, "ABSA", "031", "1234567", "Max", "max@absa.co.za", EmployerBusinessType.Company, EmploymentSector.FinancialServices),
                    DateTime.MinValue, SalaryDeductionRemunerationType.Salaried, EmploymentStatus.Current, Enumerable.Empty<EmployeeDeductionModel>());
        };

        private Because of = () =>
        {
            command = new AddUnconfirmedSalaryDeductionEmploymentToClientCommand(salaryDeductionEmploymentModel, 1, originationSource);
        };

        private It should_implement_the_requires_client_domain_service_check = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresClient));
        };
    }
}