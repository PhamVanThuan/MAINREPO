using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Geo;
using SAHL.Services.Interfaces.Capitec.Commands;

namespace SAHL.Services.Capitec.CommandHandlers
{
    public class AddNewCityCommandHandler : IServiceCommandHandler<AddNewCityCommand>
    {
        private IGeoManager geoManager;

        public AddNewCityCommandHandler(IGeoManager geoManager)
        {
            this.geoManager = geoManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(AddNewCityCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            geoManager.AddCity(command.CityName, command.SahlCityKey, command.ProvinceId);
            return messages;
        }
    }
}