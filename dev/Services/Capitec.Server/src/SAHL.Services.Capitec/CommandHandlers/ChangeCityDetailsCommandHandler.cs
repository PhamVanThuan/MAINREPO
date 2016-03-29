using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Geo;
using SAHL.Services.Interfaces.Capitec.Commands;

namespace SAHL.Services.Capitec.CommandHandlers
{
    public class ChangeCityDetailsCommandHandler : IServiceCommandHandler<ChangeCityDetailsCommand>
    {
        private IGeoManager geoManager;
        static ServiceRequestMetadata metadata;

        public ChangeCityDetailsCommandHandler(IGeoManager geoManager)
        {
            this.geoManager = geoManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(ChangeCityDetailsCommand command, IServiceRequestMetadata metadatad)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            geoManager.ChangeCityDetails(command.Id, command.CityName, command.SahlCityKey, command.ProvinceId);
            return messages;
        }
    }
}