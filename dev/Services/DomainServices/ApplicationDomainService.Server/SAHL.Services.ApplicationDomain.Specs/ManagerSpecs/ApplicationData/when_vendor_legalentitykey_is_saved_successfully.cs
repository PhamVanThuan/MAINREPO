using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using System;

namespace SAHL.Services.ApplicationDomain.Specs.Managers.ApplicationData
{
    public class when_vendor_legalentitykey_is_saved_successfully : WithCoreFakes
    {
        private static ApplicationDataManager applicationDataManager;
        private static OfferRoleDataModel offerRoleDataModel;
        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            applicationDataManager = new ApplicationDataManager(dbFactory);
            offerRoleDataModel = new OfferRoleDataModel(956482, 1498174, (int)OfferRoleType.ExternalVendor, 1, new DateTime(2014, 1, 1));
        };

        private Because of = () =>
        {
            applicationDataManager.SaveExternalVendorOfferRole(offerRoleDataModel);
        };

        private It should_insert_the_offer_role_data_model_into_offerrole = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<OfferRoleDataModel>(Arg.Is<OfferRoleDataModel>(
                        y => y.LegalEntityKey == offerRoleDataModel.LegalEntityKey
                        && y.OfferKey == offerRoleDataModel.OfferKey
                        && y.OfferRoleTypeKey == (int)OfferRoleType.ExternalVendor
                        && y.GeneralStatusKey == offerRoleDataModel.GeneralStatusKey
                        && y.StatusChangeDate == new DateTime(2014, 1, 1)
                )));
        };
    }
}