using System;
using System.Collections.Generic;
using System.Text;
using SAHL.X2.Common;
using SAHL.X2.Common.DataAccess;

namespace X2DomainService.Interface.RCS
{
    public interface IRCS
    {
        IX2ReturnData OnCreate(int AccountKey, out string IncomeConfirmed, out string ValidDisbursement, out string ValuationReceived, out string Name);
        IX2ReturnData GetOfferExceptionForAccountKey(int AccountKey, string ExceptionGroupType, out string Name);
        IX2ReturnData GetRequiresQuickCashForAccountAndOfferKey(int AccountKey, int ApplicationKey, out string Name);
        IX2ReturnData GetOfferEndDate(int AccountKey, out DateTime Date);
        IX2ReturnData GetDocumentsReceivedStatus(int AccountKey, out string Result);
        IX2ReturnData GetClientName(int AccountKey, out string Name);
        IX2ReturnData CheckQuickCashDisbursement(int AccountKey, out string Result);
        IX2ReturnData CheckRegistrationDisbursement(int AccountKey, out string Result);
        IX2ReturnData CloseOffer(int AccountKey, out string Result);
        IX2ReturnData GetAccountLegalName(int AccountKey, out string Result);
    }
}
