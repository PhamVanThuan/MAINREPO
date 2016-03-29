using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;

using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.RefreshWorklistCommandHandlerSpecs
{
    public class when_refreshing_worklist_with_static_role : WithFakes
    {
        static AutoMocker<RefreshWorklistCommandHandler> automocker = new NSubstituteAutoMocker<RefreshWorklistCommandHandler>();
        static RefreshWorklistCommand command;
        static IReadWriteSqlRepository readWriteSqlRepository;
        static IX2ContextualDataProvider contextualDataProvider;
        static StateWorkListDataModel stateWorkListDataModel;
        static SecurityGroupDataModel securityGroupDataModel;
        static InstanceDataModel instance;
        static IX2Map map;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
            {
                map = An<IX2Map>();
                contextualDataProvider = An<IX2ContextualDataProvider>();
                instance = new InstanceDataModel(9, 1, null, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
                stateWorkListDataModel = new StateWorkListDataModel(1, 2);
                securityGroupDataModel = new SecurityGroupDataModel(false, "RoleName", "desc", null, null);
                automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetStateWorkList(1)).Return(new List<StateWorkListDataModel> { stateWorkListDataModel });
                automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetSecurityGroup(2)).Return(securityGroupDataModel);
                map.WhenToldTo(x => x.GetDynamicRole(instance, Param.IsAny<IX2ContextualDataProvider>(), "RoleName", Arg.Any<IX2Params>(), Param.IsAny<ISystemMessageCollection>())).Return("ADUSERNAME");
                command = new RefreshWorklistCommand(instance, contextualDataProvider, "activityMessage", map);
                readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
            };

        Because of = () =>
        {
            automocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        It should_remove_existing_worklist_entries_for_this_instance = () =>
            {
                readWriteSqlRepository.WasToldTo(x => x.DeleteWhere<WorkListDataModel>(Param.IsAny<string>(), Param.IsAny<object>()));
            };

        It should_get_stateworklistrows_for_this_state = () =>
            {
                automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetStateWorkList((int)instance.StateID));
            };

        It should_get_a_security_group_id_for_each_stateworklistrow = () =>
            {
                automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetSecurityGroup(stateWorkListDataModel.SecurityGroupID));
            };

        It should_not_resolve_the_dynamic_role = () =>
            {
                map.WasNotToldTo(x => x.GetDynamicRole(instance, contextualDataProvider, "RoleName", Arg.Any<IX2Params>(), Param.IsAny<ISystemMessageCollection>()));
            };

        It should_update_the_worklist = () =>
            {
                readWriteSqlRepository.WasToldTo(x => x.Insert<WorkListDataModel>(Arg.Any<IEnumerable<WorkListDataModel>>()));
            };

        It should_update_the_worklist_log = () =>
            {
                //readWriteSqlRepository.WasToldTo(x => x.Insert<WorkListLogDataModel>(Arg.Any<IEnumerable<WorkListLogDataModel>>()));
            };
    }
}