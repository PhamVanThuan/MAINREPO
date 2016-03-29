using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class UpdateAttorneyCommand : ServiceCommand, IFrontEndTestCommand
    {
        public AttorneyDataModel Attorney { get; protected set; }

        public UpdateAttorneyCommand(AttorneyDataModel attorney)
        {
            this.Attorney = attorney;
        }
    }
}