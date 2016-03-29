using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Ioc;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Managers.Statements;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using StructureMap;

namespace SAHL.Services.FinancialDomain.Specs.ManagerSpecs.FinancialData
{
    public class when_saving_application_fees : WithCoreFakes
    {
        private static IDbConfigurationProvider _dbConfigurationProvider;
        private static IDbConnectionProviderFactory _dbConnectionProviderFactory;
        private static IDbConnectionProvider _dbConnectionProvider;

        private static FinancialDataManager financialDataManager;
        private static FakeDbFactory dbFactory;

        private static int applicationNumber;
        private static OriginationFeesModel fees;
        private static decimal cancellationFee = 2;
        private static decimal initiationFee = 3;
        private static decimal registrationFee = 4;

        private Establish context = () =>
        {
            TestingIoc.Initialise();

            _dbConfigurationProvider = ObjectFactory.GetInstance<IDbConfigurationProvider>();
            _dbConnectionProvider = new DefaultDbConnectionProvider(_dbConfigurationProvider);
            _dbConnectionProviderFactory = An<IDbConnectionProviderFactory>();

            _dbConnectionProviderFactory.WhenToldTo(x => x.GetNewConnectionProvider()).Return(_dbConnectionProvider);
            DbContextConfiguration.Instance.DbConnectionProviderFactory = _dbConnectionProviderFactory;

            dbFactory = new FakeDbFactory();
            financialDataManager = new FinancialDataManager(dbFactory);

            fees = new OriginationFeesModel(0, cancellationFee, initiationFee, 0, registrationFee, 0, false, false);
            applicationNumber = 1;
        };

        private Because of = () =>
        {
            financialDataManager.SetApplicationExpenses(applicationNumber, fees);
        };

        private It should_remove_all_existing_offer_expenses_from_the_application = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Delete<int>(Arg.Is<RemoveAllApplicationFeesStatement>(y => y.ApplicationNumber == applicationNumber)));
        };

        private It should_save_the_cancellation_fee_against_the_application = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<OfferExpenseDataModel>(Arg.Is<OfferExpenseDataModel>(y => y.OfferKey == applicationNumber
                                                                                        && y.ExpenseTypeKey == (int)ExpenseType.CancellationFee
                                                                                        && y.TotalOutstandingAmount == (double)cancellationFee)));
        };

        private It should_save_the_initiation_fee_against_the_application = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<OfferExpenseDataModel>(Arg.Is<OfferExpenseDataModel>(y => y.OfferKey == applicationNumber
                                                                                        && y.ExpenseTypeKey == (int)ExpenseType.InitiationFee_BondPreparationFee
                                                                                        && y.TotalOutstandingAmount == (double)initiationFee)));
        };

        private It should_save_the_registration_fee_against_the_application = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<OfferExpenseDataModel>(Arg.Is<OfferExpenseDataModel>(y => y.OfferKey == applicationNumber
                                                                                        && y.ExpenseTypeKey == (int)ExpenseType.RegistrationFee
                                                                                        && y.TotalOutstandingAmount == (double)registrationFee)));
        };
    }
}