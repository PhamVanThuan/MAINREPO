using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.Interfaces.FinancialDomain.Models;

namespace SAHL.Services.FinancialDomain.Specs.ManagerSpecs.FinancialData
{
    public class when_getting_variableloan_application_information : WithCoreFakes
    {
        private static FinancialDataManager financialDataManager;
        private static int ApplicationInformationKey;

        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            financialDataManager = new FinancialDataManager(dbFactory);
            ApplicationInformationKey = 1;
        };

        private Because of = () =>
        {
            financialDataManager.GetApplicationInformationMortgageLoan(ApplicationInformationKey);
        };

        private It should_query_for_the_applications_information = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<MortgageLoanApplicationInformationModel>(Param.IsAny<ISqlStatement<MortgageLoanApplicationInformationModel>>()));
        };
    }
}