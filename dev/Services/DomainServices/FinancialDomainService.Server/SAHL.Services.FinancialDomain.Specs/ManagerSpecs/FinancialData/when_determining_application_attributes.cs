using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Managers.Models;
using SAHL.Services.FinancialDomain.Managers.Statements;

namespace SAHL.Services.FinancialDomain.Specs.ManagerSpecs.FinancialData
{
    public class when_determining_application_attributes : WithCoreFakes
    {
        private static FakeDbFactory dbFactory;
        private static IFinancialDataManager financialDataManager;
        private static int applicationNumber;

        private static decimal LTV;
        private static EmploymentType employmentType;
        private static decimal householdIncome;
        private static bool isStaffHomeLoan;
        private static bool isGEPF;

        private Establish context = () =>
        {
            LTV = 0.9m;
            employmentType = EmploymentType.Salaried;
            householdIncome = 12000;
            isStaffHomeLoan = false;
            isGEPF = false;

            dbFactory = new FakeDbFactory();
            financialDataManager = new FinancialDataManager(dbFactory);
            applicationNumber = 1;
        };

        private Because of = () =>
        {
            financialDataManager.DetermineApplicationAttributes(applicationNumber, LTV, employmentType, householdIncome, isStaffHomeLoan, isGEPF);
        };

        private It should_submit_query_to_determine_application_attributes = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select<GetOfferAttributesModel>(Arg.Is<DetermineApplicationAttributesStatement>(y => y.ApplicationNumber == applicationNumber
                                                                                                                                                   && y.EmploymentType == employmentType
                                                                                                                                                   && y.HouseHoldIncome == householdIncome
                                                                                                                                                   && y.IsStaffLoan == isStaffHomeLoan
                                                                                                                                                   && y.LTV == LTV)));
        };
    }
}