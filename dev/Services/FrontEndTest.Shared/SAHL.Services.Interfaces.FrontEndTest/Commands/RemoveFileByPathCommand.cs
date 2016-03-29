﻿using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class RemoveFileByPathCommand : ServiceCommand, IFrontEndTestCommand
    {
        public string filePath { get; protected set; }

        public RemoveFileByPathCommand(string filePath)
        {
            this.filePath = filePath;
        }
    }
}
