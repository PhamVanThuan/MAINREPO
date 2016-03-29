using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Specs.Repositories.Life.GetMonthlyPremium
{
    internal class when_get_monthly_premium : LifeRepositoryWithFakesBase
    {
        Establish context = () =>
        {
            lifeRepository.WhenToldTo(x => x.GetMonthlyPremium(Param.IsAny<int>())).Return(Param.IsAny<int>());
        };

        Because of = () =>
        {
            lifeRepository.GetMonthlyPremium(Param.IsAny<int>());
        };

        It should_get_life_monthly_premium = () =>
        {
            lifeRepository.WasToldTo(x => x.GetMonthlyPremium(Param.IsAny<int>()));
        };
    }
}
