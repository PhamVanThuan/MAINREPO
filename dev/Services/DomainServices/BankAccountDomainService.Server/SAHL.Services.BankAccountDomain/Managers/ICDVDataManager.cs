using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;
namespace SAHL.Services.BankAccountDomain.Managers
{
    public interface ICDVDataManager
    {
        IEnumerable<AccountIndicationDataModel> GetAccountIndications();
        IEnumerable<AccountTypeRecognitionDataModel> GetAccountTypeRecognitions(int acbBankCode, int acbTypeNumber);
        IEnumerable<ACBBankDataModel> GetBankForACBBranch(string branchCode);
        IEnumerable<CDVExceptionsDataModel> GetCDVExceptions(int bankCode, string cdvExceptionCode, int accountType);
        IEnumerable<CDVDataModel> GetCDVs(int bankNumber, string branchCode, int accountType);
    }
}
