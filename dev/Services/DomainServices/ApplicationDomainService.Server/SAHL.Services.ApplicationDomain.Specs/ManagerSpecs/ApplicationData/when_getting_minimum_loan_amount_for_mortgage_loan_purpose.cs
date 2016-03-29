using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Managers.Application.Statements;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_getting_minimum_loan_amount_for_mortgage_loan_purpose : WithFakes
    {
        private static IApplicationDataManager applicationDataManager;
        private static FakeDbFactory dbFactory;
        private static decimal result;
        private static MortgageLoanPurpose mortgageLoanPurpose;

        private Establish context = () =>
        {
            mortgageLoanPurpose = MortgageLoanPurpose.Newpurchase;
            dbFactory = new FakeDbFactory();
            applicationDataManager = new ApplicationDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne<decimal>(Param.IsAny<GetMinimumLoanAmountForApplicationStatement>())).Return(100000);
        };

        private Because of = () =>
        {
            result = applicationDataManager.GetMinimumLoanAmountForMortgageLoanPurpose(mortgageLoanPurpose);
        };

        private It should_query_for_the_MinLoanAmount_using_the_MortgageLoanPurpose = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<decimal>(Arg.Is<GetMinimumLoanAmountForApplicationStatement>(
                y => y.MortgageLoanPurposeKey == (int)mortgageLoanPurpose)));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeGreaterThan(0);
        };
    }
}