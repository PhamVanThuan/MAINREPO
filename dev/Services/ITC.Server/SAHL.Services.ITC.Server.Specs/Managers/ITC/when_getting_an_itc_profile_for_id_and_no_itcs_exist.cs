using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ITC.Models;
using System;

namespace SAHL.Services.ITC.Server.Specs.Managers.ITC
{
    public class when_getting_an_itc_profile_for_id_and_no_itcs_exist : WithITCManager
    {
        private static ItcProfile result;
        private static Guid itcID;

        private Establish context = () =>
        {
            itcID = Guid.Empty;
        };

        private Because of = () =>
        {
            result = itcManager.GetITCProfile(itcID);
        };

        private It should_get_an_itc_profile_for_the_identitynumber = () =>
        {
            dataManager.WasToldTo(x => x.GetITCByID(itcID));
        };

        private It should_return_null = () =>
        {
            result.ShouldBeNull();
        };
    }
}