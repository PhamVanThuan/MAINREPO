using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.TransUnion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace SAHL.Services.ITC.Server.Specs.Managers.ITC
{
    internal class when_getting_an_itc_profile_for_legal_entity_and_one_exists : WithITCManager
    {
        private static string identityNumber;
        private static int legalEntityKey;
        private static ItcProfile result;
        private static ItcProfile profile;
        private static int daysItcIsValid = 31;

        private Establish context = () =>
        {
            legalEntityKey = 49;
            identityNumber = "889848498484984";

            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Managers/ITC/TestITCs", "itc_with_debt_counselling.xml");
            XDocument currentItcData = XDocument.Load(path);
            var reader = new ItcReader(currentItcData);
            profile = reader.GetITCProfile;

            var currentItcDataModel = new ITCDataModel(legalEntityKey, null, DateTime.Now.AddDays((-1 * daysItcIsValid) + 2),
                currentItcData.ToString(), "SUCCESS", "Barry", "<request></request>");
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

        private It should_return_the_most_recent_itc_that_is_not_older_than_the_valid_itc_days = () =>
        {
            result.ShouldMatch(m =>
                m.CreditBureauMatch == profile.CreditBureauMatch &&
                m.DebtCounselling.Date == profile.DebtCounselling.Date &&
                m.DebtCounselling.Code == profile.DebtCounselling.Code &&
                m.EmpericaScore == profile.EmpericaScore);
        };
    }
}