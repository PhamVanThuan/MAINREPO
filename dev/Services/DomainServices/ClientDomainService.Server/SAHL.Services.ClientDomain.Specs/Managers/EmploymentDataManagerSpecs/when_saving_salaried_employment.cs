using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.ClientDomain.Specs.Managers.EmploymentDataManagerSpecs
{
    internal class when_saving_salaried_employment : WithFakes
    {
        static EmploymentDataManager dataManager;
        static int clientKey;
        static SalariedEmploymentModel model;
        private static FakeDbFactory dbFactory;
        static EmployerModel employer;
        Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            employer = new EmployerModel(1111, "ABSA", "031", "1234567", "Max", "max@absa.co.za", EmployerBusinessType.Company, EmploymentSector.FinancialServices);
            model = new SalariedEmploymentModel(10000, 25, employer, DateTime.MinValue, SalariedRemunerationType.Salaried, EmploymentStatus.Current, null);
            clientKey = 2;
            dataManager = new EmploymentDataManager(dbFactory);
        };

        Because of = () =>
        {
            dataManager.SaveSalariedEmployment(clientKey, model);
        };

        It should_insert_a_new_employment_record = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<EmploymentDataModel>(Arg.Is<EmploymentDataModel>(
                    y => y.BasicIncome == model.BasicIncome
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