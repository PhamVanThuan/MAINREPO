using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.TransUnion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace SAHL.Services.ITC.Server.Specs.Managers.ITC
{
    public class when_getting_an_itc_profile_for_an_id_and_one_exists : WithITCManager
    {
        private static Guid itcID;
        private static ItcProfile result;
        private static ItcProfile profile;

        private Establish context = () =>
        {
            itcID = Guid.Parse("{57A90A70-48B4-4EB0-AF9D-06F23921CCF7}");

            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Managers/ITC/TestITCs", "itc_with_debt_counselling.xml");
            XDocument itcData = XDocument.Load(path);
            var itcDataModel = new ITCRequestDataModel(itcID, DateTime.Now, itcData.ToString());
            var reader = new ItcReader(itcData);
            profile = reader.GetITCProfile;

            dataManager.WhenToldTo(x => x.GetITCByID(itcID)).Return(itcDataModel);
        };

        private Because of = () =>
        {
            result = itcManager.GetITCProfile(itcID);
        };

        private It should_get_an_itc_profile_for_the_identitynumber = () =>
        {
            dataManager.WasToldTo(x => x.GetITCByID(itcID));
        };

        private It should_return_an_itc_profile_for_the_identitynumber = () =>
        {
            result.ShouldMatch(m =>
                m.CreditBureauMatch == profile.CreditBureauMatch &&
                m.DebtCounselling.Date == profile.DebtCounselling.Date &&
                m.DebtCounselling.Code == profile.DebtCounselling.Code &&
                m.EmpericaScore == profile.EmpericaScore);
        };
    }
}