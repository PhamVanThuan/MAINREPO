using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.ITC.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ITC.Server.Specs.Managers.ITC
{
    internal class when_getting_an_itc_profile_for_legal_entity_and_none_are_valid : WithITCManager
    {
        private static string identityNumber;
        private static int legalEntityKey;
        private static ItcProfile result;
        private static int daysItcIsValid = 31;

        private Establish context = () =>
        {
            identityNumber = "445781468468549";
            legalEntityKey = 49;

            var currentItcDataModel = new ITCDataModel(legalEntityKey, null, DateTime.Now.AddDays((-1 * daysItcIsValid) - 12),
                "<response>This one is also old</response>", "SUCCESS", "Barry", "<request></request>");
            var oldItcDataModel = new ITCDataModel(legalEntityKey, null, DateTime.Now.AddDays((-1 * daysItcIsValid) - 15),
                "<response>This one is old</response>", "SUCCESS", "Barry", "<request></request>");

            dataManager.WhenToldTo(x => x.GetItcsForLegalEntity(identityNumber)).Return(new List<ITCDataModel>
            {
                oldItcDataModel, currentItcDataModel
            });
        };

        private Because of = () =>
        {
            result = itcManager.GetCurrentItcProfileForLegalEntity(identityNumber);
        };

        private It should_get_the_itcs_for_the_legal_entity = () =>
        {
            dataManager.WasToldTo(x => x.GetItcsForLegalEntity(identityNumber));
        };

        private It return_nothing = () =>
        {
            result.ShouldBeNull();
        };
    }
}