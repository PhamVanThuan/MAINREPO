using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.CommandHandlers;
using SAHL.Services.Capitec.Managers.Geo;

using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using SAHL.Core.Services;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.ChangeSuburbDetailsCommandHandlerSpecs
{
    public class when_told_to_change_suburb_details : WithFakes
    {
        static ChangeSuburbDetailsCommand command;
        static ChangeSuburbDetailsCommandHandler handler;
        static IGeoManager geoManager;
        static string suburbName;
        static int sahlKey;
        static Guid id, cityId;
        static ISystemMessageCollection messages;
        static string postalCode;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            suburbName = "New Suburb";
            sahlKey = 1;
            id = Guid.NewGuid();
            cityId = Guid.NewGuid();
            postalCode = "9999";
            geoManager = An<IGeoManager>();
            command = new ChangeSuburbDetailsCommand(id, suburbName, sahlKey, postalCode, cityId);
            handler = new ChangeSuburbDetailsCommandHandler(geoManager);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command,metadata);
        };

        It should_change_the_suburb_details_using_the_geo_manager = () =>
        {
            geoManager.WasToldTo(x => x.ChangeSuburbDetails(Param.Is(id), Param.Is(suburbName), Param.Is(sahlKey), Param.Is(postalCode), Param.Is(cityId)));
        };

        It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}
