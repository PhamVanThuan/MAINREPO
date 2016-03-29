using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Geo;
using SAHL.Services.Interfaces.Capitec.Commands;

namespace SAHL.Services.Capitec.CommandHandlers
{
    public class ChangeProvinceDetailsCommandHandler : IServiceCommandHandler<ChangeProvinceDetailsCommand>
    {
        private IGeoManager geoManager;

        public ChangeProvinceDetailsCommandHandler(IGeoManager geoManager)
        {
            this.geoManager = geoManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(ChangeProvinceDetailsCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            geoManager.ChangeProvinceDetails(command.Id, command.ProvinceName, command.SahlProvinceKey, command.CountryId);
            return messages;
        }
    }
}