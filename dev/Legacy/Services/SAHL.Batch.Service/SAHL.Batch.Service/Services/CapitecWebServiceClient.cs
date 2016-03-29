using SAHL.Batch.Common.ServiceContracts;
using SAHL.Batch.Service.CapitecService;
using SAHL.Services.Capitec.Models.Shared;

namespace SAHL.Batch.Service.Services
{
    public class CapitecWebServiceClient : ICapitecClientService
    {
        private ICapitec capitecServiceClient;

        public CapitecWebServiceClient(ICapitec capitecServiceClient)
        {
            this.capitecServiceClient = capitecServiceClient;
        }

        public bool CreateApplication(CapitecApplication capitecApplication, int messageId)
        {
            var status = false;
            if ((capitecApplication as NewPurchaseApplication) != null)
            {
                var newPurchaseApplication = capitecApplication as NewPurchaseApplication;
                status = capitecServiceClient.CreateCapitecNewPurchaseApplication(newPurchaseApplication, messageId);
            }
            else if ((capitecApplication as SwitchLoanApplication) != null)
            {
                var switchLoanApplication = capitecApplication as SwitchLoanApplication;
                status = capitecServiceClient.CreateCapitecSwitchLoanApplication(switchLoanApplication, messageId);
            }
            return status;
        }
    }
}