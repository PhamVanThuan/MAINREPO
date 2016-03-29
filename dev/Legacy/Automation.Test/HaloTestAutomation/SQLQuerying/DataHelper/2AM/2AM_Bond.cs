using System;
using System.Collections.Generic;
using System.Linq;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        ///   Returns the latest bond record details when provided with an offer key
        /// </summary>
        /// <param name = "offerKey">Application Number</param>
        /// <returns>Models.Bond</returns>
        public Automation.DataModels.Bond GetLatestBondRecordByOfferkey(int offerKey)
        {
            var bonds = dataContext.Query<Automation.DataModels.Bond>(String.Format(
                @"select b.BondKey, BondRegistrationNumber, BondRegistrationDate, BondRegistrationAmount,
                    BondLoanAgreementAmount, b.bondRegistrationAmount - bondLoanAgreementAmount as Diff
                    from [2am].dbo.bond b (nolock)
                    where bondkey = (select max(bondkey)
                    from [2am].dbo.offer o (nolock)
                    join [2am].dbo.financialservice fs (nolock) on o.accountkey=fs.accountkey
                    join [2am].dbo.bondMortgageLoan bml (nolock) on fs.financialServiceKey=bml.financialServiceKey
                    where offerkey={0})",
                offerKey));
            return (from b in bonds select b).FirstOrDefault();
        }

        /// <summary>
        /// Gets all of the bond records against an account.
        /// </summary>
        /// <param name="accountKey">account number</param>
        /// <returns>Models.Bond</returns>
        public IEnumerable<Automation.DataModels.Bond> GetAccountBonds(int accountKey)
        {
            var bonds = dataContext.Query<Automation.DataModels.Bond>(
                string.Format(@"select fs.AccountKey,
                    b.BondKey, do.Description as DeedsOffice, le.RegisteredName as Attorney, BondRegistrationNumber, BondRegistrationDate,
                    BondRegistrationAmount, BondLoanAgreementAmount, b.bondRegistrationAmount - bondLoanAgreementAmount as Diff
                    from
                    [2am].[dbo].[FinancialService] fs
                    join [2am].[dbo].[BondMortgageLoan] bml on fs.[FinancialServiceKey]=bml.[FinancialServiceKey]
                    join [2am].[dbo].[Bond] b on bml.[BondKey] = b.[BondKey]
                    join [2am].[dbo].[Attorney] a on b.[AttorneyKey] = a.[AttorneyKey]
                    join [2am].[dbo].[LegalEntity] le on a.[LegalEntityKey] = le.[LegalEntityKey]
                    join [2am].[dbo].[DeedsOffice] do on b.[DeedsOfficeKey] = do.[DeedsOfficeKey]
                    where fs.[AccountKey] = {0}", accountKey));
            return bonds;
        }

        /// <summary>
        /// Gets a list of accounts with more than one bond record.
        /// </summary>
        /// <returns></returns>
        public List<int> GetAccountsWithMoreThanOneBond()
        {
            var db = new _2AMDataHelper();
            var query = string.Format(@"select fs.[AccountKey]
                from [2am].dbo.Account a
                join [2am].[dbo].[FinancialService] fs on a.AccountKey = fs.AccountKey
                join [2am].[dbo].[BondMortgageLoan] bml on fs.[FinancialServiceKey]=bml.[FinancialServiceKey]
                join [2am].[dbo].[Bond] b on bml.[BondKey] = b.[BondKey]
                where fs.AccountStatusKey = 1 and a.AccountStatusKey = 1
                group by fs.[AccountKey]
                having count(fs.[AccountKey]) > 1");
            SQLStatement statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            return (from r in results select r.Column(0).GetValueAs<int>()).ToList();
        }
    }
}