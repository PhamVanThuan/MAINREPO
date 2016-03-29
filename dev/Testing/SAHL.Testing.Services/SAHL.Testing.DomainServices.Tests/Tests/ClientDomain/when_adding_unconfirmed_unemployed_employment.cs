using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ClientDomain
{
    [TestFixture]
    public class when_adding_unconfirmed_unemployed_employment : ServiceTestBase<IClientDomainServiceClient>
    {
        private EmployerModel _employer;
        private UnemployedEmploymentModel _unemployedModel;
        private int _clientKey;

        [SetUp]
        public void onTestSetup()
        {
            var getEmployerQuery = new GetEmployerQuery();
            base.PerformQuery(getEmployerQuery);
            var activeEmployer = getEmployerQuery.Result.Results.First();
            _employer = new EmployerModel(activeEmployer.EmployerKey, activeEmployer.Name, activeEmployer.TelephoneCode, activeEmployer.TelephoneNumber, string.Empty,
                string.Empty, EmployerBusinessType.Company, EmploymentSector.FinancialServices);
            GetNaturalPersonClientQuery query = new GetNaturalPersonClientQuery(true);
            base.PerformQuery(query);
            _clientKey = query.Result.Results.First().LegalEntityKey;
        }

        [TearDown]
        public void onTestTeardown()
        {
            _employer = null;
            _clientKey = 0;
            _unemployedModel = null;
        }

        [Test]
        public void when_successful()
        {
            int basicIncome = this.randomizer.Next(0, 100000);
            _unemployedModel = new UnemployedEmploymentModel(basicIncome, 25, _employer, DateTime.Now.AddYears(-2), UnemployedRemunerationType.Pension, EmploymentStatus.Current);
            var command = new AddUnconfirmedUnemployedEmploymentToClientCommand(_unemployedModel, _clientKey, Core.BusinessModel.Enums.OriginationSource.SAHomeLoans);
            base.Execute<AddUnconfirmedUnemployedEmploymentToClientCommand>(command);
            var clientEmployment = TestApiClient.Get<EmploymentDataModel>(new { legalentitykey = _clientKey });
            Assert.That(clientEmployment.Where(x => x.EmployerKey == _employer.EmployerKey
                && x.EmploymentTypeKey == (int)EmploymentType.Unemployed
                && x.BasicIncome == _unemployedModel.BasicIncome
                ).First() != null);
        }

        [Test]
        public void when_unsuccessful()
        {
            //set start date to the future
            int basicIncome = this.randomizer.Next(0, 100000);
            _unemployedModel = new UnemployedEmploymentModel(basicIncome, 25, _employer, DateTime.Now.AddYears(+2), UnemployedRemunerationType.Pension, EmploymentStatus.Current);
            var command = new AddUnconfirmedUnemployedEmploymentToClientCommand(_unemployedModel, _clientKey, Core.BusinessModel.Enums.OriginationSource.SAHomeLoans);
            base.Execute<AddUnconfirmedUnemployedEmploymentToClientCommand>(command)
                .AndExpectThatErrorMessagesContain("The start date of a current employment record cannot be in the future.");
        }
    }
}