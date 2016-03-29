using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ClientDomain
{
    [TestFixture]
    public class when_adding_unconfirmed_salaried_employment : ServiceTestBase<IClientDomainServiceClient>
    {
        private EmployerModel _employer;
        private SalariedEmploymentModel _salariedEmploymentModel;
        private int _clientKey;
        private List<EmployeeDeductionModel> _deductions;

        [SetUp]
        public void onTestSetup()
        {
            var getEmployerQuery = new GetEmployerQuery();
            base.PerformQuery(getEmployerQuery);
            var activeEmployer = getEmployerQuery.Result.Results.First();
            _employer = new EmployerModel(activeEmployer.EmployerKey, activeEmployer.Name, activeEmployer.TelephoneCode, activeEmployer.TelephoneNumber, string.Empty,
                string.Empty, EmployerBusinessType.Company, EmploymentSector.FinancialServices);
            GetNaturalPersonClientQuery getNaturalPersonClientQuery = new GetNaturalPersonClientQuery(true);
            base.PerformQuery(getNaturalPersonClientQuery);
            _clientKey = getNaturalPersonClientQuery.Result.Results.First().LegalEntityKey;
            _deductions = new List<EmployeeDeductionModel>(){
                new EmployeeDeductionModel(EmployeeDeductionTypeEnum.MedicalAid, 555),
                new EmployeeDeductionModel(EmployeeDeductionTypeEnum.PayAsYouEarnTax, 666),
                new EmployeeDeductionModel(EmployeeDeductionTypeEnum.PensionOrProvidendFund, 777),
                new EmployeeDeductionModel(EmployeeDeductionTypeEnum.UnemploymentInsurance, 888)
            };
        }

        [TearDown]
        public void onTestTeardown()
        {
            _employer = null;
            _clientKey = 0;
            _salariedEmploymentModel = null;
        }

        [Test]
        public void when_successful()
        {
            int basicIncome = this.randomizer.Next(0, 100000);
            _salariedEmploymentModel = new SalariedEmploymentModel(basicIncome, 25, _employer, DateTime.Now.AddYears(-2), SalariedRemunerationType.Salaried, EmploymentStatus.Current,
                _deductions);
            var command = new AddUnconfirmedSalariedEmploymentToClientCommand(_salariedEmploymentModel, _clientKey, Core.BusinessModel.Enums.OriginationSource.SAHomeLoans);
            base.Execute<AddUnconfirmedSalariedEmploymentToClientCommand>(command);
            var clientEmployment = TestApiClient.Get<EmploymentDataModel>(new { legalentitykey = _clientKey });
            Assert.That(clientEmployment.Where(x => x.EmployerKey == _employer.EmployerKey
                && x.EmploymentTypeKey == (int)EmploymentType.Salaried
                && x.BasicIncome == _salariedEmploymentModel.BasicIncome && x.MedicalAid == 555 & x.PAYE == 666 & x.PensionProvident == 777 & x.UIF == 888
                ).First() != null);
        }

        [Test]
        public void when_unsuccessful()
        {
            //set start date to the future
            _salariedEmploymentModel = new SalariedEmploymentModel(10000, 25, _employer, DateTime.Now.AddYears(+1), SalariedRemunerationType.Salaried, EmploymentStatus.Current,
                _deductions);
            var command = new AddUnconfirmedSalariedEmploymentToClientCommand(_salariedEmploymentModel, _clientKey, Core.BusinessModel.Enums.OriginationSource.SAHomeLoans);
            base.Execute<AddUnconfirmedSalariedEmploymentToClientCommand>(command)
                .AndExpectThatErrorMessagesContain("The start date of a current employment record cannot be in the future.");
        }
    }
}