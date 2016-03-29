using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.CommandHandlers;
using SAHL.Services.Capitec.Managers.Geo;

using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using System.Collections.Generic;
using SAHL.Core.Services;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.ChangeCityDetailsCommandHandlerSpecs
{
    public class when_told_to_change_city_details : WithFakes
    {
        static ChangeCityDetailsCommand command;
        static ChangeCityDetailsCommandHandler handler;
        static IGeoManager geoManager;
        static string cityName;
        static int sahlKey;
        static Guid id, provinceId;
        static ISystemMessageCollection messages;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            cityName = "New City";
            sahlKey = 1;
            id = Guid.NewGuid();
            provinceId = Guid.NewGuid();
            geoManager = An<IGeoManager>();
            command = new ChangeCityDetailsCommand(id, cityName, sahlKey, provinceId);
            handler = new ChangeCityDetailsCommandHandler(geoManager);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command,metadata);
        };

        It should_change_the_city_details_using_the_geo_manager = () =>
        {
            geoManager.WasToldTo(x => x.ChangeCityDetails(Param.Is(id), Param.Is(cityName), Param.Is(sahlKey), Param.Is(provinceId)));
        };

        It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}
