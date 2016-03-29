using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class RemoveAttorneyCommand : ServiceCommand, IFrontEndTestCommand
    {
        public int AttorneyKey { get; protected set; }

        public RemoveAttorneyCommand(int attorneyKey)
        {
            this.AttorneyKey = attorneyKey;
        }
    }
}