using SAHL.Core.Services;
using System;
using System.IO;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.CommandHandlers
{
    public class RemoveFileByPathCommandhandler : IServiceCommandHandler<RemoveFileByPathCommand>
    {
        public ISystemMessageCollection HandleCommand(RemoveFileByPathCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            if (File.Exists(command.filePath))
            {
                File.Delete(command.filePath);
            }
            else
            {
                messages.AddMessage(new SystemMessage("File: " + command.filePath + " does not exist.", SystemMessageSeverityEnum.Error));
            }
            return messages;
        }
    }
}
