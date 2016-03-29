using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ITC.Models;

namespace SAHL.Services.ITC.Server.Specs.Managers.ITC
{
    internal class when_getting_an_itc_profile_for_legal_entity_and_none_exist : WithITCManager
    {
        private static string identityNumber;
        private static ItcProfile result;

        private Establish context = () =>
        {
            identityNumber = "49";
        };

        private Because of = () =>
        {
            result = itcManager.GetCurrentItcProfileForLegalEntity(identityNumber);
        };

        private It should_get_the_itcs_for_the_legal_entity = () =>
        {
            dataManager.WasToldTo(x => x.GetItcsForLegalEntity(identityNumber));
        };

        private It should_return_nothing = () =>
        {
            result.ShouldBeNull();
        };
    }
}