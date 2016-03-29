using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Application;
using System;

namespace SAHL.Services.Capitec.Specs.ApplicationDataServiceSpecs
{
    public class when_adding_application_loan_details : WithFakes
    {
        static ApplicationDataManager service;
        static Guid applicationLoanDetailId, applicationId, employmentTypeId, occupancyType;
        static decimal householdIncome, instalment, interestRate, loanAmount, ltv, pti, fees;
        static int term;
        static bool capitaliseFees;
        private static FakeDbFactory dbFactory;

        Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            service = new ApplicationDataManager(dbFactory);
            applicationId = Guid.NewGuid();
            applicationLoanDetailId = Guid.NewGuid();
            employmentTypeId = Guid.NewGuid();
            occupancyType = Guid.NewGuid();
            householdIncome = 15000M;
            instalment = 2000M;
            interestRate = 0.028M;
            loanAmount = 300000M;
            ltv = 0.75M;
            pti = 0.3M;
            fees = 8500M;
            term = 240;
            capitaliseFees = true;
        };

        Because of = () =>
        {
            service.AddApplicationLoanDetail(applicationId, applicationLoanDetailId, employmentTypeId, occupancyType, householdIncome, instalment, interestRate, loanAmount,
                ltv, pti, fees, term, capitaliseFees);
        };

        It should_insert_application_loan_details_with_the_details_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<ApplicationLoanDetailDataModel>(
                y => y.ApplicationId == applicationId
                && y.EmploymentTypeID == employmentTypeId
                && y.Id == applicationLoanDetailId
                && y.OccupancyTypeEnumID == occupancyType
                && y.HouseholdIncome == householdIncome
                && y.Instalment == instalment
                && y.InterestRate == interestRate
                && y.LoanAmount == loanAmount
            )));
        };
    }
}
