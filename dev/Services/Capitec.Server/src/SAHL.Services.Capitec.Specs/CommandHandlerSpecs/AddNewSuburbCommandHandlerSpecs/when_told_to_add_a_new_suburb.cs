using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.CommandHandlers;
using SAHL.Services.Capitec.Managers.Geo;

using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using System.Collections.Generic;
using SAHL.Core.Services;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.AddNewCityCommandHandlerSpecs
{
    public class when_told_to_add_a_new_suburb : WithFakes
    {
        static AddNewSuburbCommand command;
        static AddNewSuburbCommandHandler handler;
        static IGeoManager geoManager;
        static string suburbName;
        static int sahlKey;
        static Guid cityId;
        static ISystemMessageCollection messages;
        static string postalCode;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            suburbName = "New Suburb";
            sahlKey = 1;
            cityId = Guid.NewGuid();
            postalCode = "9999";
            geoManager = An<IGeoManager>();
            command = new AddNewSuburbCommand(suburbName, sahlKey, postalCode , cityId);
            handler = new AddNewSuburbCommandHandler(geoManager);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command,metadata);
        };

        It should_add_the_new_suburb_using_the_geo_manager = () =>
        {
            geoManager.WasToldTo(x => x.AddSuburb(Param.Is(suburbName), Param.Is(sahlKey), Param.Is(postalCode), Param.Is(cityId)));
        };

        It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}
