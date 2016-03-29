using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Geo;
using SAHL.Services.Interfaces.Capitec.Commands;

namespace SAHL.Services.Capitec.CommandHandlers
{
    public class AddNewSuburbCommandHandler : IServiceCommandHandler<AddNewSuburbCommand>
    {
        private IGeoManager geoManager;

        public AddNewSuburbCommandHandler(IGeoManager geoManager)
        {
            this.geoManager = geoManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(AddNewSuburbCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            geoManager.AddSuburb(command.SuburbName, command.SahlSuburbKey, command.PostalCode, command.CityId);
            return messages;
        }
    }
}