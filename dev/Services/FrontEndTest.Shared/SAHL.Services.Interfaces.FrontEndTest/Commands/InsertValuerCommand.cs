using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class InsertValuerCommand : ServiceCommand, IFrontEndTestCommand
    {
        public ValuatorDataModel Valuer { get; protected set; }
        public Guid ValuerId { get; protected set; }

        public InsertValuerCommand(ValuatorDataModel valuer, Guid valuerId)
        {
            this.Valuer = valuer;
            this.ValuerId = valuerId;
        }
    }
}