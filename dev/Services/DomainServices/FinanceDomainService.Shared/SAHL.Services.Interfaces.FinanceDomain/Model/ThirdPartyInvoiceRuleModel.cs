using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinanceDomain.Model
{
    public class ThirdPartyInvoiceRuleModel : IThirdPartyInvoiceRuleModel
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "A ThirdPartyInvoiceKey must be provided.")]
        public int ThirdPartyInvoiceKey { get; set; }

        public ThirdPartyInvoiceRuleModel(int thirdPartyInvoiceKey)
        {
            ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }
    }
}
