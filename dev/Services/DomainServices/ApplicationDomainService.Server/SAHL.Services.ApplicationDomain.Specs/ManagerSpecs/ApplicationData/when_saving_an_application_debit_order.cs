using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_saving_an_application_debit_order : WithCoreFakes
    {
        private static ApplicationDataManager applicationDataManager;
        private static FakeDbFactory dbFactory;
        private static int bankAccountKey;
        private static ApplicationDebitOrderModel applicationDebitOrderModel;
        private static int validDebitOrderDay = 28;
        private static int applicationNumber = 1234567;
        private static int clientBankAccountKey = 789546244;
        private static int offerDebitOrderKey = 111111;
        private static int result = 0;

        private Establish context = () =>
        {
            bankAccountKey = 100;
            dbFactory = new FakeDbFactory();
            applicationDataManager = new ApplicationDataManager(dbFactory);
            applicationDebitOrderModel = new ApplicationDebitOrderModel(applicationNumber, validDebitOrderDay, clientBankAccountKey);
            dbFactory.FakedDb.DbContext.WhenToldTo(x => x.Insert<OfferDebitOrderDataModel>(Param.IsAny<OfferDebitOrderDataModel>())).Callback<OfferDebitOrderDataModel>(
                y=>y.OfferDebitOrderKey = offerDebitOrderKey);
        };

        private Because of = () =>
        {
            result = applicationDataManager.SaveApplicationDebitOrder(applicationDebitOrderModel, bankAccountKey);
        };

        private It should_insert_a_new_offer_debit_order_record = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<OfferDebitOrderDataModel>(Arg.Is<OfferDebitOrderDataModel>(y => y.OfferKey == applicationDebitOrderModel.ApplicationNumber)));
        };

        private It should_return_the_offer_debit_order_key = () =>
        {
            result.ShouldEqual(offerDebitOrderKey);
        };
    }
}