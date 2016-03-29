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
    public class when_told_to_add_a_new_city : WithFakes
    {
        static AddNewCityCommand command;
        static AddNewCityCommandHandler handler;
        static IGeoManager geoManager;
        static string cityName;
        static int sahlKey;
        static Guid provinceId;
        static ISystemMessageCollection messages;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            cityName = "New City";
            sahlKey = 1;
            provinceId = Guid.NewGuid();
            geoManager = An<IGeoManager>();
            command = new AddNewCityCommand(cityName, sahlKey, provinceId);
            handler = new AddNewCityCommandHandler(geoManager);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command,metadata);
        };

        It should_add_the_city_using_the_geo_manager = () =>
        {
            geoManager.WasToldTo(x => x.AddCity(Param.Is(cityName), Param.Is(sahlKey), Param.Is(provinceId)));
        };

        It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}
