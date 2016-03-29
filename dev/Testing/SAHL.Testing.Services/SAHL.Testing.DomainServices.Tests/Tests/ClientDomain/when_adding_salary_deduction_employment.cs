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
    public class when_adding_salary_deduction_employment : ServiceTestBase<IClientDomainServiceClient>
    {
        private SalaryDeductionEmploymentModel _employmentModel;
        private int _clientKey;
        private EmployerModel _employer;
        private List<EmployeeDeductionModel> _deductions;

        [SetUp]
        public void OnSetup()
        {
            base.OnTestSetup();
            var getEmployerQuery = new GetEmployerQuery();
            base.PerformQuery(getEmployerQuery);
            var activeEmployer = getEmployerQuery.Result.Results.First();
            _employer = new EmployerModel(activeEmployer.EmployerKey, activeEmployer.Name, activeEmployer.TelephoneCode, activeEmployer.TelephoneNumber, string.Empty,
                string.Empty, EmployerBusinessType.Company, EmploymentSector.FinancialServices);
            GetNaturalPersonClientQuery query = new GetNaturalPersonClientQuery(true);
            base.PerformQuery(query);
            _clientKey = query.Result.Results.First().LegalEntityKey;
            _deductions = new List<EmployeeDeductionModel>(){
                new EmployeeDeductionModel(EmployeeDeductionTypeEnum.MedicalAid, 555),
                new EmployeeDeductionModel(EmployeeDeductionTypeEnum.PayAsYouEarnTax, 666),
                new EmployeeDeductionModel(EmployeeDeductionTypeEnum.PensionOrProvidendFund, 777),
                new EmployeeDeductionModel(EmployeeDeductionTypeEnum.UnemploymentInsurance, 888),
            };
        }

        [TearDown]
        public void OnTeardown()
        {
            _employmentModel = null;
            _clientKey = 0;
            _employer = null;
            _deductions = null;
        }

        [Test]
        public void when_successful()
        {
            _employmentModel = new SalaryDeductionEmploymentModel(this.randomizer.Next(0, 100000), this.randomizer.Next(0, 100000),
                25, _employer, DateTime.Now.AddYears(-1), SalaryDeductionRemunerationType.Salaried, EmploymentStatus.Current, _deductions);
            var command = new AddUnconfirmedSalaryDeductionEmploymentToClientCommand(_employmentModel, _clientKey, OriginationSource.SAHomeLoans);
            base.Execute<AddUnconfirmedSalaryDeductionEmploymentToClientCommand>(command);
            var clientEmployment = TestApiClient.Get<EmploymentDataModel>(new { legalentitykey = _clientKey });
            Assert.That(clientEmployment.Where(x => x.BasicIncome == _employmentModel.BasicIncome + _employmentModel.HousingAllowance
                && x.EmployerKey == _employer.EmployerKey && x.MedicalAid == 555 & x.PAYE == 666 & x.PensionProvident == 777 & x.UIF == 888
                && x.EmploymentTypeKey == (int)EmploymentType.SalariedwithDeduction)
                .First() != null);
        }

        [Test]
        public void when_unsuccessful()
        {
            _employmentModel = new SalaryDeductionEmploymentModel(this.randomizer.Next(0, 100000), this.randomizer.Next(0, 100000),
                25, _employer, DateTime.Now.AddYears(+1), SalaryDeductionRemunerationType.Salaried, EmploymentStatus.Current, _deductions);
            var command = new AddUnconfirmedSalaryDeductionEmploymentToClientCommand(_employmentModel, _clientKey, OriginationSource.SAHomeLoans);
            base.Execute<AddUnconfirmedSalaryDeductionEmploymentToClientCommand>(command)
                .AndExpectThatErrorMessagesContain("The start date of a current employment record cannot be in the future.");
        }
    }
}