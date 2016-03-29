using SAHL.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.DomainProcessManagerProxy.Models
{
    public class ThirdPartyPaymentModel : ValidatableModel
    {
        [Required(ErrorMessage = "The ThirdPartyInvoiceKey must be provided.")]
        public int ThirdPartyInvoiceKey { get; set; }
        [Required(ErrorMessage = "The InstanceId must be provided.")]
        public long InstanceId { get; set; }
        [Required(ErrorMessage = "The Account Number must be provided.")]
        public int AccountNumber { get; set; }
        [Required(ErrorMessage = "The SAHL-Reference must be provided.")]
        public string SAHLReference { get; set; }

        public ThirdPartyPaymentModel(int thirdPartyInvoiceKey, long instanceId, int accountNumber, string sahlReference)
        {
            ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            InstanceId = instanceId;
            AccountNumber = accountNumber;
            SAHLReference = sahlReference;

            this.Validate();
        }
    }
}
