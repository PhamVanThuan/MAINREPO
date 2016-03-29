using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class RemoveValuerCommand : ServiceCommand, IFrontEndTestCommand
    {
        public int ValuatorKey { get; protected set; }

        public RemoveValuerCommand(int valuatorKey)
        {
            this.ValuatorKey = valuatorKey;
        }
    }
}