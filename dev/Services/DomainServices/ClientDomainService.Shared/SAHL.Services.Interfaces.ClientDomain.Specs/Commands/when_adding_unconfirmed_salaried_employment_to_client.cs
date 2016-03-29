using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Commands
{
    internal class when_adding_unconfirmed_salaried_employment_to_client
    {
        private static AddUnconfirmedSalariedEmploymentToClientCommand command;
        private static SalariedEmploymentModel salariedEmployment;
        private static int clientKey;
        private static OriginationSource originationSource;

        private Establish context = () =>
        {
            clientKey = 5;
            originationSource = OriginationSource.SAHomeLoans;
            salariedEmployment = new SalariedEmploymentModel(10000, 25, new EmployerModel(1111, "ABSA", "031", "1234567", "Max", "max@absa.co.za", EmployerBusinessType.Company, 
                EmploymentSector.FinancialServices), DateTime.MinValue, SalariedRemunerationType.Salaried, EmploymentStatus.Current, Enumerable.Empty<EmployeeDeductionModel>());
        };

        private Because of = () =>
        {
            command = new AddUnconfirmedSalariedEmploymentToClientCommand(salariedEmployment, clientKey, originationSource);
        };

        private It should_implement_the_requires_client_domain_service_check = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresClient));
        };
    }
}