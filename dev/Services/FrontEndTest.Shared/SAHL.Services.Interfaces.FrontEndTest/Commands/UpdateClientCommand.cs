using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class UpdateClientCommand : ServiceCommand, IFrontEndTestCommand
    {
        public LegalEntityDataModel legalEntity { get; protected set; }

        public UpdateClientCommand(LegalEntityDataModel legalEntity)
        {
            this.legalEntity = legalEntity;
        }

    }
}
