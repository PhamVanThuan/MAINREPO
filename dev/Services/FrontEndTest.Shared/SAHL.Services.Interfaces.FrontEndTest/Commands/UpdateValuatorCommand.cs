using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class UpdateValuatorCommand : ServiceCommand, IFrontEndTestCommand
    {
        public ValuatorDataModel Valuator { get; protected set; }

        public UpdateValuatorCommand(ValuatorDataModel valuator)
        {
            this.Valuator = valuator;
        }
    }
}