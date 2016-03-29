using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ITC.Managers.Itc;
using SAHL.Services.ITC.Managers.Itc.Statements;

namespace SAHL.Services.ITC.Server.Specs.Managers.ITCData
{
    public class when_getting_the_itcs_for_a_legal_entity : WithCoreFakes
    {
        private static FakeDbFactory dbFactory;
        private static ItcDataManager dataManager;
        private static string identityNumber;
        private static ITCDataModel result;
        private static ITCDataModel itcData;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = new ItcDataManager(dbFactory);

            identityNumber = "9099";
            itcData = new ITCDataModel(9299, 44, new DateTime(2015, 01, 14), "<response></response>", "SUCCESS", "Hans", "<request></request>");
            dbFactory.FakedDb.InReadOnlyAppContext()
                .WhenToldTo(x => x.Select(Param<GetItcsForLegalEntityStatement>.Matches(m => m.IdentityNumber == identityNumber)))
                .Return(new List<ITCDataModel> { itcData });
        };

        private Because of = () =>
        {
            result = dataManager.GetItcsForLegalEntity(identityNumber).First();
        };

        private It should_get_the_itcs = () =>
        {
            dbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.Select(Param<GetItcsForLegalEntityStatement>.Matches(m =>
                m.IdentityNumber == identityNumber)));
        };

        private It should_have_a_matching_AccountKey = () =>
        {
            result.AccountKey.ShouldEqual(itcData.AccountKey);
        };

        private It should_have_a_matching_ChangeDate = () =>
        {
            result.ChangeDate.ShouldEqual(itcData.ChangeDate);
        };

        private It should_have_a_matching_ITCKey = () =>
        {
            result.ITCKey.ShouldEqual(itcData.ITCKey);
        };

        private It should_have_a_matching_LegalEntityKey = () =>
        {
            result.LegalEntityKey.ShouldEqual(itcData.LegalEntityKey);
        };

        private It should_have_a_matching_RequestXML = () =>
        {
            result.RequestXML.ShouldEqual(itcData.RequestXML);
        };

        private It should_have_a_matching_ResponseStatus = () =>
        {
            result.ResponseStatus.ShouldEqual(itcData.ResponseStatus);
        };

        private It should_have_a_matching_ResponseXML = () =>
        {
            result.ResponseXML.ShouldEqual(itcData.ResponseXML);
        };

        private It should_have_a_matching_UserID = () =>
        {
            result.UserID.ShouldEqual(itcData.UserID);
        };
    }
}
