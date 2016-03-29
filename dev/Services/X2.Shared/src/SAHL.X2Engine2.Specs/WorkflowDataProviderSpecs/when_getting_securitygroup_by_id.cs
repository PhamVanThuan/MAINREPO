using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Caching;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.Providers;

using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_securitygroup_by_id : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static SecurityGroupDataModel securityGroupDataModel;
        private static SecurityGroupDataModel returnedSecurityGroupDataModel;
        private static string coreUIStatementToExecute;
        private static Dictionary<string, object> parameters = new Dictionary<string, object>();
        private static int securityGroupId;

        Establish context = () =>
        {
            coreUIStatementToExecute = UIStatements.securitygroupdatamodel_selectbykey;
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            securityGroupDataModel = new SecurityGroupDataModel(10, false, "Name", "desc", 1, 1);
            securityGroupId = securityGroupDataModel.ID;
            parameters.Add("PrimaryKey", securityGroupId);
            readOnlySqlRepository.WhenToldTo(x => x.SelectOne<SecurityGroupDataModel>(Param.IsAny<string>(), Param.IsAny<object>())).Return(securityGroupDataModel);
        };

        Because of = () =>
        {
            returnedSecurityGroupDataModel = automocker.ClassUnderTest.GetSecurityGroup(securityGroupId);
        };

        It should_return_the_correct_security_group_data_model = () =>
        {
            returnedSecurityGroupDataModel.ShouldBeTheSameAs(securityGroupDataModel);
        };

        It should_add_it_to_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.AddItem<SecurityGroupDataModel>(Param.IsAny<string>(), returnedSecurityGroupDataModel));
        };

        It should_use_the_securityGroupId_provided_when_running_core_UIStatement = () =>
        {
            readOnlySqlRepository.WasToldTo(x => x.SelectOne<SecurityGroupDataModel>(coreUIStatementToExecute, Arg.Is<object>(anon => anon.CheckValue(parameters))));
        };
    }
}