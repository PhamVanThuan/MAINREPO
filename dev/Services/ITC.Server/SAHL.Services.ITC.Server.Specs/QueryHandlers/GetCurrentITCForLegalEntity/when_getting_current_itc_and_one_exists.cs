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

namespace SAHL.Services.ITC.Server.Specs.QueryHandlers.GetCurrentITCForLegalEntity
{
    public class when_getting_current_itc_and_one_exists : WithFakes
    {
        private static IItcManager itcManager;
        private static GetCurrentITCForLegalEntityQueryHandler handler;
        private static GetCurrentITCForLegalEntityQuery query;
        private static ISystemMessageCollection messages;

        private static string identityNumber;
        private static ItcProfile profile;

        private Establish context = () =>
        {
            itcManager = An<IItcManager>();
            handler = new GetCurrentITCForLegalEntityQueryHandler(itcManager);

            identityNumber = "99546846846846";
            query = new GetCurrentITCForLegalEntityQuery(identityNumber);

            profile = new ItcProfile(131, new List<ItcJudgement>(), new List<ItcDefault>(), new List<ItcPaymentProfile>(),
               new List<ItcNotice>(), new ItcDebtCounselling(new DateTime(2014, 12, 22), "AD"), true);
            itcManager.WhenToldTo(x => x.GetCurrentItcProfileForLegalEntity(identityNumber)).Return(profile);
        };

        private Because of = () =>
        {
            messages = handler.HandleQuery(query);
        };

        private It should_get_the_current_itc_for_the_legal_entity = () =>
        {
            itcManager.WasToldTo(x => x.GetCurrentItcProfileForLegalEntity(identityNumber));
        };

        private It should_add_the_itc_profile_to_the_query = () =>
        {
            query.Result.Results.First().ItcProfile.ShouldMatch(m =>
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