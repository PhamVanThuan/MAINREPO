using SAHL.Core.Services;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class SetApplicationSPVToZeroCommand : ServiceCommand, IFrontEndTestCommand
    {
        public SetApplicationSPVToZeroCommand(int applicationInformationKey)
        {
            this.ApplicationInformationKey = applicationInformationKey;
        }

        public int ApplicationInformationKey { get; protected set; }
    }
}