using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicationData
{
    public class when_saving_application_mailing_address : WithFakes
    {
        private static ApplicationDataManager applicationDataManager;
        private static FakeDbFactory dbFactory;
        private static int addressKey;
        private static int result;
        private static int clientKey;
        private static int expectedOfferMailingAddressKey;
        private static ApplicationMailingAddressModel applicationMailingAddress;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            applicationDataManager = new ApplicationDataManager(dbFactory);
            addressKey = 121;
            clientKey = 321;
            expectedOfferMailingAddressKey = 454;
            applicationMailingAddress = new ApplicationMailingAddressModel(1001, 1010, CorrespondenceLanguage.English, OnlineStatementFormat.HTML, CorrespondenceMedium.SMS, 1122, false);
            dbFactory.FakedDb.DbContext.WhenToldTo<IReadWriteSqlRepository>(x => x.Insert<OfferMailingAddressDataModel>(Param.IsAny<OfferMailingAddressDataModel>()))
                .Callback<OfferMailingAddressDataModel>(y => { y.OfferMailingAddressKey = expectedOfferMailingAddressKey; });
        };

        private Because of = () =>
        {
            result = applicationDataManager.SaveApplicationMailingAddress(applicationMailingAddress, clientKey, addressKey);
        };

        private It should_insert_application_mailing_address_with_provided_parameters = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<OfferMailingAddressDataModel>(Arg.Is<OfferMailingAddressDataModel>(omf => omf.AddressKey == addressKey
                && omf.OfferKey == applicationMailingAddress.ApplicationNumber
                && omf.OnlineStatement == applicationMailingAddress.OnlineStatementRequired
                && omf.OnlineStatementFormatKey == (int)applicationMailingAddress.OnlineStatementFormat
                && omf.LanguageKey == (int)applicationMailingAddress.CorrespondenceLanguage
                && omf.LegalEntityKey == clientKey
                )));
        };

        private It should_return_a_system_generated_application_mailing_address_key = () =>
        {
            result.ShouldEqual(expectedOfferMailingAddressKey);
        };
    }
}