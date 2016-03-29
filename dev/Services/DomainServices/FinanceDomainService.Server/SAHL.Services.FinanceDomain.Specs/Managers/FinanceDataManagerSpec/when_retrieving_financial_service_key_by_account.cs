using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.FinanceDomain.Managers.Statements;
using SAHL.Services.Interfaces.FinanceDomain.Enum;

namespace SAHL.Services.FinanceDomain.Specs.Managers.FinanceDataManagerSpec
{
    public class when_retrieving_financial_service_key_by_account : WithFakes
    {
        private static IFinanceDataManager financeDataManager;
        private static FakeDbFactory dbFactory;
        private static int accountNumber;
        private static int? result, expectedFinancialServiceKey;

        private Establish context = () =>
         {
             dbFactory = new FakeDbFactory();
             financeDataManager = new FinanceDataManager(dbFactory);
             accountNumber = 10008;
             expectedFinancialServiceKey = 1408;
             dbFactory.NewDb().InReadOnlyAppContext().WhenToldTo(x =>
                 x.SelectOne(Param<GetFinancialServiceKeyByServiceTypeStatement>
                     .Matches(q => q.AccountNumber == accountNumber && q.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan))
                 ).Return(expectedFinancialServiceKey);
         };

        private Because of = () =>
         {
             result = financeDataManager.GetVariableLoanFinancialServiceKeyByAccount(accountNumber);
         };

        private It should_retrive_the_financial_service_key_from_the_db = () =>
         {
             dbFactory.NewDb().InReadOnlyAppContext().WasToldTo(y => y.SelectOne(Param<GetFinancialServiceKeyByServiceTypeStatement>
                 .Matches(q => q.AccountNumber == accountNumber && q.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan)));
         };

        private It should_return_the_financial_service_key = () =>
         {
             result.ShouldEqual(expectedFinancialServiceKey);
         };
    }
}