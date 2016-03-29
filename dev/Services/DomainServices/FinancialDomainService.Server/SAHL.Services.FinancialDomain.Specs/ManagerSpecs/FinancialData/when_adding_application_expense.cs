using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinancialDomain.Managers;

namespace SAHL.Services.FinancialDomain.Specs.ManagerSpecs.FinancialData
{
    public class when_adding_application_expense : WithCoreFakes
    {
        private static FinancialDataManager financialDataManager;
        private static FakeDbFactory dbFactory;

        private static int ApplicationNumber;
        private static ExpenseType expenseType;
        private static decimal amount;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            financialDataManager = new FinancialDataManager(dbFactory);

            ApplicationNumber = 1;
            expenseType = ExpenseType.CancellationFee;
            amount = 123m;
        };

        private Because of = () =>
        {
            financialDataManager.AddApplicationExpense(ApplicationNumber, expenseType, amount);
        };

        private It should_insert_an_offer_expense_against_the_application = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<OfferExpenseDataModel>(Arg.Is<OfferExpenseDataModel>(y => y.OfferKey == ApplicationNumber
                                                                                           && y.ExpenseTypeKey == (int)ExpenseType.CancellationFee
                                                                                           && y.TotalOutstandingAmount == (double)amount)));
        };
    }
}