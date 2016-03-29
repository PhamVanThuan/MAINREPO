using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.BankAccountDomain.Managers.Statements;
using System.Collections.Generic;

namespace SAHL.Services.BankAccountDomain.Managers
{
    public class CDVDataManager : ICDVDataManager 
    {
        private IDbFactory dbFactory;
        public CDVDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public IEnumerable<CDVDataModel> GetCDVs(int bankNumber, string branchCode, int accountType)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var sql = new SelectCDVForACBBankStatement(bankNumber, branchCode, accountType);
                return db.Select<CDVDataModel>(sql);
            }
        }

        public IEnumerable<AccountIndicationDataModel> GetAccountIndications()
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var sql = new SelectAccountIndicationsStatement();
                return db.Select<AccountIndicationDataModel>(sql);
            }
        }

        public IEnumerable<AccountTypeRecognitionDataModel> GetAccountTypeRecognitions(int acbBankCode, int acbTypeNumber)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var sql = new SelectAccountTypeRecognitionForACBBankStatement(acbBankCode, acbTypeNumber);
                return db.Select<AccountTypeRecognitionDataModel>(sql);
            }
        }

        public IEnumerable<ACBBankDataModel> GetBankForACBBranch(string branchCode)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var sql = new SelectACBBankForACBBranchStatement(branchCode);
                return db.Select<ACBBankDataModel>(sql);
            }
        }

        public IEnumerable<CDVExceptionsDataModel> GetCDVExceptions(int bankCode, string cdvExceptionCode, int accountType)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var sql = new SelectCDVExceptionsForCDVStatement(bankCode, cdvExceptionCode, accountType);
                return db.Select<CDVExceptionsDataModel>(sql);
            }
        }
    }
}