using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.CommandHandlers;
using SAHL.Services.Capitec.Managers.Geo;

using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using System.Collections.Generic;
using SAHL.Core.Services;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.ChangeProvinceDetailsCommandHandlerSpecs
{
    public class when_told_to_change_province_details : WithFakes
    {
        static ChangeProvinceDetailsCommand command;
        static ChangeProvinceDetailsCommandHandler handler;
        static IGeoManager geoManager;
        static string provinceName;
        static int sahlKey;
        static Guid id, countryId;
        static ISystemMessageCollection messages;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            provinceName = "New Province";
            sahlKey = 1;
            id = Guid.NewGuid();
            countryId = Guid.NewGuid();
            geoManager = An<IGeoManager>();
            command = new ChangeProvinceDetailsCommand(id, provinceName, sahlKey, countryId);
            handler = new ChangeProvinceDetailsCommandHandler(geoManager);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command,metadata);
        };

        It should_change_the_province_details_using_the_geo_manager = () =>
        {
            geoManager.WasToldTo(x => x.ChangeProvinceDetails(Param.Is(id), Param.Is(provinceName), Param.Is(sahlKey), Param.Is(countryId)));
        };

        It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}
