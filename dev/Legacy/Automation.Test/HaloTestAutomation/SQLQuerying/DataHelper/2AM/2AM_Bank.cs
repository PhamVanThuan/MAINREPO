using Common.Enums;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        ///   Retrieves a list of bank accounts for the legal entities playing a role on the account as well as the SAHL valuation
        ///   bank account details
        /// </summary>
        /// <param name = "offerKey">Offer.OfferKey</param>
        /// <returns>Bank Account String</returns>
        public QueryResults GetDisbursementBankAccountsByOfferKey(int offerKey)
        {
            SQLStoredProcedure proc = new SQLStoredProcedure { Name = "test.GetDisbursementBankAccountsByOfferKey" };
            proc.AddParameter(new SqlParameter("@OfferKey", offerKey.ToString()));
            return dataContext.ExecuteStoredProcedureWithResults(proc);
        }

        /// <summary>
        /// Gets the bank account record when provided with the banking details
        /// </summary>
        /// <param name="bank">Bank</param>
        /// <param name="branchCode">Branch Code</param>
        /// <param name="accountType">Account Type</param>
        /// <param name="accountNumber">Account Number</param>
        /// <returns></returns>
        public QueryResults GetBankAccount(string bank, string branchCode, string accountType, string accountNumber)
        {
            string query =
                string.Format(@"select bankaccountkey from bankaccount b (nolock)
                                    join acbType at (nolock) on b.acbtypenumber=at.acbtypenumber
                                    join acbBranch ac (nolock) on b.acbBranchCode=ac.acbBranchCode
                                    join acbBank ab (nolock) on ac.acbbankcode=ab.acbbankcode
                                    where accountNumber='{0}'
                                    and at.acbTypedescription='{1}'
                                    and ab.acbBankDescription='{2}'
                                    and ac.acbBranchCode='{3}'", accountNumber, accountType, bank, branchCode);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets a LegalEntityBankAccount record by its LegalEntityKey, BankAccountKey and General Status
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <param name="bankAccountKey"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public QueryResults GetLegalEntityBankAccountByStatus(int legalEntityKey, int bankAccountKey, GeneralStatusEnum status)
        {
            string query =
                string.Format(@"select * from legalEntityBankAccount (nolock) where legalEntityKey={0}
                                and bankAccountKey={1} and generalStatusKey={2}", legalEntityKey, bankAccountKey, (int)status);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Removes a bank account and its legal entity bank account records. this is used in this test to ensure that the
        /// test case being used for the CDV check failing does not already exist in the database.
        /// </summary>
        /// <param name="_CDVaccountNumber"></param>
        public void RemoveBankAccount(string _CDVaccountNumber)
        {
            string sql = string.Format(@"   delete from legalentitybankaccount where bankaccountkey in
                                                (select bankaccountKey from bankAccount where accountNumber = '{0}')
                                                delete from bankaccount where accountnumber = '{1}'", _CDVaccountNumber, _CDVaccountNumber);
            var statement = new SQLStatement { StatementString = sql };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Removes a bank account from the offer debit order table
        /// </summary>
        /// <param name="_bankAccountKey"></param>
        public void DeleteOfferDebitOrderByBankAccountKey(int _bankAccountKey)
        {
            string sql =
                 string.Format(@"Delete from [2am].dbo.OfferDebitOrder where bankAccountKey = {0}", _bankAccountKey);
            var statement = new SQLStatement { StatementString = sql };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Inserts Settlement Banking details
        /// </summary>
        /// <param name="offerKey"></param>
        public void InsertSettlementBanking(int offerKey)
        {
            SQLStoredProcedure proc = new SQLStoredProcedure { Name = "test.InsertSettlementBanking" };
            proc.AddParameter(new SqlParameter("@offerKey", offerKey.ToString()));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Returns the bank account model
        /// </summary>
        /// <param name="bankAccountKey">Pass through key to get a single bank account, otherwise it will return the first 1000</param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.BankAccount> GetBankAccount(int bankAccountKey = 0)
        {
            string token = bankAccountKey == 0
                               ? string.Empty
                               : string.Format(@"where bankAccountKey = {0}", bankAccountKey);

            return dataContext.Query<Automation.DataModels.BankAccount>(string.Format(@"select * from [2am].dbo.BankAccount ba join
                    [2am].dbo.ACBBranch br on ba.ACBBranchCode = br.ACBBranchCode join
                    [2am].dbo.ACBBank b on br.ACBBankCode = b.ACBBankCode
                    [token]").Replace("[token]", token));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.LegalEntityBankAccount> GetLegalEntityBankAccounts(int legalEntityKey)
        {
            var sql = string.Format(@"
                select leba.*, le.*, ba.*, br.*, b.*
                from [2am].dbo.legalentity le
                join [2am].dbo.legalentitybankaccount leba on le.legalentitykey = leba.legalentitykey
                join [2am].dbo.bankaccount ba on leba.bankaccountkey = ba.bankaccountkey
                join [2am].dbo.ACBBranch br
                on ba.ACBBranchCode=br.ACBBranchCode
	                join [2am].dbo.ACBBank b
                on br.ACBBankCode=b.ACBBankCode
                    where le.legalentitykey = {0}", legalEntityKey);
            return dataContext.Query<Automation.DataModels.LegalEntityBankAccount, Automation.DataModels.LegalEntity, Automation.DataModels.BankAccount, Automation.DataModels.LegalEntityBankAccount>(sql,
                (leba, le, ba) => { leba.LegalEntity = le; leba.BankAccount = ba; return leba; }, splitOn: "LegalEntityKey,BankAccountKey", commandtype: System.Data.CommandType.Text).DefaultIfEmpty();
        }

        public IEnumerable<Automation.DataModels.LegalEntityBankAccount> PopulateMultipleNestedModelsTest()
        {
            var sql = @"select leba.*, ba.*, br.*, b.*
                    from legalentitybankaccount leba
                    join bankaccount ba on leba.bankaccountkey = ba.bankaccountkey
                    join acbbranch br on ba.acbbranchcode = br.acbbranchcode
                    join acbbank b on br.acbbankcode = b.acbbankcode
                    where br.acbbranchdescription is not null and b.acbbankdescription is not null";

            return dataContext.Query<Automation.DataModels.LegalEntityBankAccount, Automation.DataModels.BankAccount, Automation.DataModels.Branch, Automation.DataModels.Bank, Automation.DataModels.LegalEntityBankAccount>(sql,
                (leba, ba, br, b) => { leba.BankAccount = ba; ba.Branch = br; br.Bank = b; return leba; }, splitOn: "BankAccountKey,ACBBranchCode,ACBBankCode", commandtype: System.Data.CommandType.Text).DefaultIfEmpty();
        }
    }
}