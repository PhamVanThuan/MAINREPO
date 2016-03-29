using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Geo;
using SAHL.Services.Interfaces.Capitec.Commands;

namespace SAHL.Services.Capitec.CommandHandlers
{
    public class ChangeSuburbDetailsCommandHandler : IServiceCommandHandler<ChangeSuburbDetailsCommand>
    {
        private IGeoManager geoManager;

        public ChangeSuburbDetailsCommandHandler(IGeoManager geoManager)
        {
            this.geoManager = geoManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(ChangeSuburbDetailsCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            geoManager.ChangeSuburbDetails(command.Id, command.SuburbName, command.SahlSuburbKey, command.PostalCode, command.CityId);
            return messages;
        }
    }
}