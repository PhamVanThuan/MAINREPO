using System;
using System.Text;
using System.Linq;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Wizards.ThirdParty.ConditionalActions
{
    public class ThirdPartyWorkflowApproveInvoiceConditionalProvider : HaloWorkflowActionConditionalProviderBase
    {
        public ThirdPartyWorkflowApproveInvoiceConditionalProvider(IDbFactory dbFactory)
            : base(dbFactory, "Third Party Invoices", "Third Party Invoices", "Approve for Payment")
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext, params object[] additionalParameters)
        {
            var capabilities = this.ConcatenateStrings(additionalParameters);

            var sqlString = string.Format(@"
SELECT      TPI.[TotalAmountIncludingVAT], C.[Description], CM.[StartRange], CM.[EndRange]
FROM        [2AM].[dbo].[ThirdPartyInvoice] TPI, [2AM].[OrgStruct].[Capability] C
LEFT JOIN   [2AM].[OrgStruct].[CapabilityMandate] CM ON CM.CapabilityKey = C.CapabilityKey
WHERE       [ThirdPartyInvoiceKey] = {0}
            AND C.[Description] IN ({1}) 
            AND CM.[EndRange] >= TPI.[TotalAmountIncludingVAT]", businessContext.BusinessKey.Key, capabilities);

            return sqlString;
        }

        public override bool Execute(BusinessContext businessContext, params string[] additionalParameters)
        {
            var invoiceCapability = this.ExecuteSqlStatement(businessContext, additionalParameters);
            return invoiceCapability != null;
        }

        private string ConcatenateStrings(object[] additionalParameters)
        {
            if (!additionalParameters.Any()) { return string.Empty; }

            var capabilities = new StringBuilder();
            foreach (var additionalParameter in additionalParameters)
            {
                capabilities.AppendFormat("'{0}',", additionalParameter);
            }

            return capabilities.ToString().Substring(0, capabilities.Length - 1);
        }
    }
}
