using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.Interfaces.WorkflowTask.Queries;
using SAHL.Services.WorkflowTask.Server.Managers;
using SAHL.Services.WorkflowTask.Server.QueryHandlers;

namespace SAHL.Services.WorkflowTask.Server.Specs.Tags
{
    public class when_fetching_all_tags_for_user_should_map_to_dictionary : WithFakes
    {
        private static GetAllTagsForUserQuery query;
        private static GetAllTagsForUserQueryHandler handler;
        private static WorkflowTaskDataManager dataManager;
        private static FakeDbFactory dbFactory;
        private static string adUsername;
        private static UserTagsDataModel userTagReturning;
        private static List<UserTagsDataModel> usertagsList;

        private Establish context = () =>
        {
            adUsername = "testUser";
            dbFactory = new FakeDbFactory();
            dataManager = An<WorkflowTaskDataManager>(dbFactory);
            query = new GetAllTagsForUserQuery(adUsername);

            userTagReturning = new UserTagsDataModel(Guid.NewGuid(), "", "", "", "", DateTime.Now);
            usertagsList = new List<UserTagsDataModel> { userTagReturning };

            dbFactory.WhenToldTo(x => x.FakedDb.InReadOnlyAppContext().SelectWhere<UserTagsDataModel>("Username = @Username", Arg.Any<object>()))
                .Return(usertagsList);
            handler = new GetAllTagsForUserQueryHandler(dataManager);
        };

        private Because of = () =>
        {
            handler.HandleQuery(query);
        };

        private It should_call_the_manager_to_fetch_items_from_database = () =>
        {
            dataManager.WasToldTo(x => x.GetAllTagsForUser(adUsername));
        };

        private It should_call_down_to_database_to_get_tags = () =>
        {
            dbFactory.FakedDb.InReadOnlyWorkflowContext()
                .WasToldTo(x => x.SelectWhere<UserTagsDataModel>("ADUsername = @Username", Arg.Any<object>()));
        };

        private It should_call_the_mapping_method_with_the_items = () =>
        {
            dataManager.WasToldTo(x => x.MapTags(usertagsList));
        };
    }
}
