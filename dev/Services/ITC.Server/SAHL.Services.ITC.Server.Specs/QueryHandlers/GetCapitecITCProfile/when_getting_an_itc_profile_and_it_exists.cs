using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.Interfaces.ITC.Queries;
using SAHL.Services.ITC.Managers.Itc;
using SAHL.Services.ITC.QueryHandlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ITC.Server.Specs.QueryHandlers.GetCapitecITCProfile
{
    public class when_getting_an_itc_profile_and_it_exists : WithFakes
    {
        private static GetCapitecITCProfileQuery query;
        private static GetCapitecITCProfileQueryHandler queryHandler;
        private static IItcManager itcManager;
        private static ISystemMessageCollection messages;
        private static ItcProfile profile;
        private static Guid itcID;

        private Establish context = () =>
        {
            itcManager = An<IItcManager>();
            itcID = Guid.Parse("{4770C905-C60B-4710-9762-037C52D0B31F}");
            query = new GetCapitecITCProfileQuery(itcID);
            queryHandler = new GetCapitecITCProfileQueryHandler(itcManager);
            profile = new ItcProfile(131, new List<ItcJudgement>(), new List<ItcDefault>(), new List<ItcPaymentProfile>(),
                new List<ItcNotice>(), new ItcDebtCounselling(new DateTime(2014, 12, 22), "AD"), true);
            itcManager.WhenToldTo(x => x.GetITCProfile(itcID)).Return(profile);
        };

        private Because of = () =>
        {
            messages = queryHandler.HandleQuery(query);
        };

        private It should_get_an_itc_using_the_itc_id = () =>
        {
            itcManager.WasToldTo(x => x.GetITCProfile(itcID));
        };

        private It should_set_the_itc_profile_on_the_query_result = () =>
        {
            query.Result.Results.First().ITCProfile.ShouldMatch(m =>
                m.CreditBureauMatch == profile.CreditBureauMatch &&
                m.DebtCounselling == profile.DebtCounselling &&
                m.Defaults == profile.Defaults &&
                m.EmpericaScore == profile.EmpericaScore &&
                m.Judgments == profile.Judgments &&
                m.Notices == profile.Notices &&
                m.PaymentProfiles == profile.PaymentProfiles);
        };

        private It should_not_return_any_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}