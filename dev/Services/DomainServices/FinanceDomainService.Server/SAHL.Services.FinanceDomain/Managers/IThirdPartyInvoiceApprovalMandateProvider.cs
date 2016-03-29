using System;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Managers
{
    public interface IThirdPartyInvoiceApprovalMandateProvider
    {
        List<string> GetMandatedCapability(decimal invoiceAmount);

        Tuple<decimal, decimal> GetMandatedRange(string capability);

        string GetCapabilityWithHigherMandate(string[] approverUserCapabilities);
    }
}