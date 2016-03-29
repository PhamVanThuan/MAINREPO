using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ClientDomain.Specs.Managers;

using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.Services.ClientDomain.Specs.Managers.EmploymentDataManagerSpecs
{
    internal class when_saving_salary_deduction_employment : WithFakes
    {
        private static EmploymentDataManager dataManager;
        private static int clientKey;
        private static SalaryDeductionEmploymentModel model;
        private static FakeDbFactory dbFactory;
        private static EmployerModel employer;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            employer = new EmployerModel(1111, "ABSA", "031", "1234567", "Max", "max@absa.co.za", EmployerBusinessType.Company, EmploymentSector.FinancialServices);
            model = new SalaryDeductionEmploymentModel(10000, 1500, 25, employer, DateTime.MinValue, SalaryDeductionRemunerationType.Salaried, EmploymentStatus.Current, null);
            clientKey = 2;
            dataManager = new EmploymentDataManager(dbFactory);            
        };

        private Because of = () =>
        {
            dataManager.SaveSalaryDeductionEmployment(clientKey, model);
        };

        private It should_insert_a_new_employment_record = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<EmploymentDataModel>(Arg.Is<EmploymentDataModel>(
                    y => y.BasicIncome == model.BasicIncome + model.HousingAllowance
                        && y.MonthlyIncome == model.BasicIncome + model.HousingAllowance
                        && y.SalaryPaymentDay == model.SalaryPaymentDay
                        && y.EmployerKey == model.Employer.EmployerKey
                        && y.EmploymentStartDate == model.StartDate
                        && y.RemunerationTypeKey == (int)model.RemunerationType
                        && y.EmploymentStatusKey == (int)model.EmploymentStatus
                        && y.EmploymentTypeKey == (int)model.EmploymentType
                        && y.LegalEntityKey == clientKey
                )));
        };
    }
}