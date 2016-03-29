using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Geo;
using SAHL.Services.Interfaces.Capitec.Commands;

namespace SAHL.Services.Capitec.CommandHandlers
{
    public class AddNewProvinceCommandHandler : IServiceCommandHandler<AddNewProvinceCommand>
    {
        private IGeoManager geoManager;

        public AddNewProvinceCommandHandler(IGeoManager geoManager)
        {
            this.geoManager = geoManager;
        }

        public ISystemMessageCollection HandleCommand(AddNewProvinceCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            geoManager.AddProvince(command.ProvinceName, command.SahlProvinceKey, command.CountryId);
            return messages;
        }
    }
}