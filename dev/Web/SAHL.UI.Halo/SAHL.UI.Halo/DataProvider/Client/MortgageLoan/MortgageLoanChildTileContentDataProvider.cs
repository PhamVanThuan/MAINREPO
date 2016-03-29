using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.Client.MortgageLoan;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.ContentProvider.Client.MortgageLoan
{
    public class MortgageLoanChildTileContentDataProvider : HaloTileBaseContentDataProvider<MortgageLoanChildModel>
    {
        public MortgageLoanChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"SELECT {0} as AccountNumber,
                [2am].dbo.fGetFormattedAddress(prop.AddressKey) as PropertyAddress,
                a.OpenDate as OpenDate,
                sum(ISNULL(lb.InitialBalance,0)) as LoanAgreementAmount,
                sum(cur_bal.Amount) as LoanCurrentBalance,
                sum(arr_bal.Amount) as LoanArrearBalance,
                fsb.DebitOrderDay,
                sum(fs.Payment) as InstalmentAmount,
                lb.RemainingInstalments,
                a.AccountStatusKey as AccountStatus,
                osp.OriginationSourceKey as [OriginationSource]
                    FROM [2am].dbo.Property prop (NOLOCK)
                    JOIN [2am].fin.MortgageLoan ml (NOLOCK) on ml.PropertyKey = prop.PropertyKey
                    JOIN [2am].dbo.FinancialService fs (NOLOCK) on ml.FinancialServiceKey = fs.FinancialServiceKey
                    JOIN [2am].dbo.Account a (NOLOCK) on a.AccountKey = fs.AccountKey
                    JOIN [2AM].dbo.OriginationSourceProduct osp (NOLOCK) on a.OriginationSourceProductKey = osp.OriginationSourceProductKey
                    JOIN [2am].fin.LoanBalance lb (NOLOCK) on lb.FinancialServiceKey = fs.FinancialServiceKey
                    JOIN [2am].fin.Balance cur_bal (NOLOCK) on cur_bal.FinancialServiceKey = fs.FinancialServiceKey and cur_bal.BalanceTypeKey = 1
                    JOIN [2am].dbo.FinancialService afs (NOLOCK) on afs.ParentFinancialServiceKey = fs.FinancialServiceKey
                    JOIN [2am].fin.Balance arr_bal (NOLOCK) on arr_bal.FinancialServiceKey = afs.FinancialServiceKey and arr_bal.BalanceTypeKey = 4
                    LEFT OUTER JOIN [2am].dbo.FinancialServiceBankAccount fsb (NOLOCK) on fs.financialservicekey = fsb.financialservicekey and fsb.GeneralStatusKey = 1
                    LEFT OUTER JOIN dw.DWWAREHOUSEPRE.Securitisation.FactAccountAttribute fa (NOLOCK) on fa.AccountKey = fs.AccountKey
                WHERE fs.AccountKey = {0}
                GROUP BY fs.AccountKey,
                prop.AddressKey,
                a.OpenDate,
                fsb.DebitOrderDay,
                lb.RemainingInstalments,
                a.AccountStatusKey,
                osp.OriginationSourceKey", businessContext.BusinessKey.Key);
        }
    }
}