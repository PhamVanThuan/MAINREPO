using System;

using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.EventProjectionStreamer;

namespace SAHL.Services.EventProjectionStreamer.CommandHandlers
{
    public class ProjectionUpdatedCommandHandler : IServiceCommandHandler<ProjectionUpdatedCommand>
    {
        public ISystemMessageCollection HandleCommand(ProjectionUpdatedCommand command, IServiceRequestMetadata metadata)
        {
            var messageCollection = new SystemMessageCollection();
            return messageCollection;
        }
    }
}
