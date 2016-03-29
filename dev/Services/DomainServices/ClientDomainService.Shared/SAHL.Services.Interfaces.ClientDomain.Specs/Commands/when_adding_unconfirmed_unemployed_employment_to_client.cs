using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Commands
{
    public class when_adding_unconfirmed_unemployed_employment_to_client : WithFakes
    {
        private static AddUnconfirmedUnemployedEmploymentToClientCommand command;
        private static UnemployedEmploymentModel employment;
        private static int clientKey;
        private static OriginationSource originationSource;

        private Establish context = () =>
        {
            clientKey = 5;
            originationSource = OriginationSource.SAHomeLoans;
            employment = new UnemployedEmploymentModel(10000, 25, new EmployerModel(1111, "Investments", "031", "1234567", "Max", "max@absa.co.za", 
                EmployerBusinessType.Company, EmploymentSector.FinancialServices), DateTime.MinValue, UnemployedRemunerationType.InvestmentIncome, EmploymentStatus.Current);
        };

        private Because of = () =>
        {
            command = new AddUnconfirmedUnemployedEmploymentToClientCommand(employment, clientKey, originationSource);
        };

        private It should_implement_the_requires_client_domain_service_check = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresClient));
        };
    }
}