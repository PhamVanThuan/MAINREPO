using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class UpdateThirdPartyCommand : ServiceCommand, IFrontEndTestCommand
    {
        public LegalEntityDataModel thirdparty { get; protected set; }

        public UpdateThirdPartyCommand(LegalEntityDataModel thirdparty)
        {
            this.thirdparty = thirdparty;
        }
    }
}
