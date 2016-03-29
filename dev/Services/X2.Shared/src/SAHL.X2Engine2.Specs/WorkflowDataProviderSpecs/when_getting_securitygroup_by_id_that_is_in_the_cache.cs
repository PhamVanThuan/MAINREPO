using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_securitygroup_by_id_that_is_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static SecurityGroupDataModel securityGroupDataModel;
        private static SecurityGroupDataModel returnedSecurityGroupDataModel;
        private static int securityGroupId;

        Establish context = () =>
        {
            securityGroupDataModel = new SecurityGroupDataModel(10, false, "Name", "desc", 1, 1);
            securityGroupId = securityGroupDataModel.ID;

            automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<IKeyedCacheKey, SecurityGroupDataModel>(Param.IsAny<IKeyedCacheKey>())).Return("Key");
            automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
            automocker.Get<ICache>().WhenToldTo(x => x.GetItem<SecurityGroupDataModel>("Key")).Return(securityGroupDataModel);
        };

        Because of = () =>
        {
            returnedSecurityGroupDataModel = automocker.ClassUnderTest.GetSecurityGroup(securityGroupId);
        };

        It should_get_it_from_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.GetItem<SecurityGroupDataModel>("Key"));
        };

        It should_return_the_correct_security_group_data_model = () =>
        {
            returnedSecurityGroupDataModel.ShouldBeTheSameAs(securityGroupDataModel);
        };
    }
}