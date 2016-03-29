using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.Services;
using SAHL.UI.Halo.Shared;
using SAHL.Core.Extensions;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Halo;

namespace SAHL.Services.Halo.Server.CommandHandlers
{
    public class TileEditorUpdateCommandHandler : IServiceCommandHandler<TileEditorUpdateCommand>
    {
        private readonly ITileDataRepository tileDataRepository;

        public TileEditorUpdateCommandHandler(ITileDataRepository tileDataRepository)
        {
            if (tileDataRepository == null) { throw new ArgumentNullException("tileDataRepository"); }
            this.tileDataRepository = tileDataRepository;
        }

        public ISystemMessageCollection HandleCommand(TileEditorUpdateCommand command, IServiceRequestMetadata metadata)
        {
            if (command == null) { throw new ArgumentNullException("command"); }
            var messageCollection = new SystemMessageCollection();

            try
            {
                var editorDataProvider = this.tileDataRepository.FindTileEditorDataProvider(command.TileConfiguration);
                if (editorDataProvider == null)
                {
                    throw new Exception("Editor Data Provider not found");
                }

                editorDataProvider.SaveData(command.BusinessContext, command.TileDataModel);
            }
            catch (Exception runtimeException)
            {
                var errorMessage = runtimeException.BuildExceptionErrorMessage();
                messageCollection.AddMessage(new SystemMessage(errorMessage, SystemMessageSeverityEnum.Exception));
            }

            return messageCollection;
        }
    }
}
