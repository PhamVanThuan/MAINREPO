using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_updating_a_property : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static UpdatePropertyCommand command;
        private static PropertyDataModel model;
        private static UpdatePropertyCommandHandler commandHandler;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
         {
             testDataManager = An<ITestDataManager>();
             model = new PropertyDataModel((int)PropertyType.Maisonette, (int)TitleType.Freehold, 1, (int)EmploymentStatus.Current, 123,
                                           "propertyDescription1", "propertyDescription2", "propertyDescription3", 1, DateTime.Now, "erfNumber",
                                           "erfPortionNumber", "sectionalSchemeName", "sectionalUnitNumber", (int)DeedsPropertyType.Erf,
                                           "erfSuburbDescription", "erfMetroDescription", (int)DataProvider.SAHL);
             command = new UpdatePropertyCommand(model);
             commandHandler = new UpdatePropertyCommandHandler(testDataManager);
             metadata = An<IServiceRequestMetadata>();
         };

        private Because of = () =>
         {
             messages = commandHandler.HandleCommand(command, metadata);
         };

        private It should_Update_the_correct_property = () =>
         {
             testDataManager.WasToldTo(x => x.UpdateProperty(model));
         };

        private It should_return_messages = () =>
         {
             messages.ShouldNotBeNull();
         };

        private It should_not_return_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}