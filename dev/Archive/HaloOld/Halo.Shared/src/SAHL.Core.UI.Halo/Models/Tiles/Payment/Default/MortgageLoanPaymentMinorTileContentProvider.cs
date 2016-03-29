using SAHL.Core.UI.Providers.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.UI.Halo.Tiles.Payment.Default
{
    public class MortgageLoanPaymentMinorTileContentProvider : AbstractSqlTileContentProvider<MortgageLoanPaymentMinorTileModel>
    {
        public override string GetStatement(BusinessModel.BusinessKey businessKey)
        {
            return string.Format(@"select top 1 fsba.ChangeDate as [EffectiveDate],
		                                    fspt.Description as [PaymentMethod],
		                                    fsba.DebitOrderDay as [PaymentDay],
		                                    fsba.UserID as [UserName],
		                                    ba.AccountName as [AccountHolderName],
		                                    acbt.ACBTypeDescription as [AccountType],
		                                    ba.AccountNumber as [AccountNumber],
		                                    b.ACBBankDescription as [BankingInstitution],
                                            ba.ACBBranchCode as [BranchCode],
		                                    fixedFS.Payment as [FixedDebitOrderAmount],
		                                    s.StopOrderAmount as [SubsidyStopOrderAmount],
		                                    le.RegisteredName as [SubsidyProvider],
		                                    case when fdcd.FutureDatedChangeDetailKey is null then 0 else 1 end as [FutureDatedChangeExists]
                                    from [2AM].dbo.FinancialService fs
                                    join [2AM].dbo.FinancialServiceBankAccount fsba on fsba.FinancialServiceKey = fs.FinancialServiceKey
                                    join [2AM].dbo.FinancialServicePaymentType fspt on fsba.FinancialServicePaymentTypeKey = fspt.FinancialServicePaymentTypeKey

                                    left join [2AM].dbo.BankAccount ba on fsba.BankAccountKey = ba.BankAccountKey
                                    left join [2AM].dbo.ACBBranch br on ba.ACBBranchCode = br.ACBBranchCode
                                    left join [2AM].dbo.ACBBank b on br.ACBBankCode = b.ACBBankCode
                                    left join [2AM].dbo.ACBType acbt on ba.ACBTypeNumber = acbt.ACBTypeNumber

                                    left join [2AM].dbo.FinancialService fixedFS on fixedFS.AccountKey = fs.AccountKey and fixedFS.FinancialServiceTypeKey = 2

                                    left join [2AM].dbo.AccountSubsidy accs on accs.AccountKey = fs.AccountKey
                                    left join [2AM].dbo.Subsidy s on accs.SubsidyKey = s.SubsidyKey
                                    left join [2AM].dbo.SubsidyProvider sp on s.SubsidyProviderKey = sp.SubsidyProviderKey
                                    left join [2AM].dbo.LegalEntity le on sp.LegalEntityKey = le.LegalEntityKey

									left join [2AM].dbo.FutureDatedChangeDetail fdcd on ReferenceKey = fsba.FinancialServiceBankAccountKey

                                    where	fs.AccountKey = {0}
	                                    and fs.FinancialServiceTypeKey in (1, 10)
	                                    and isnull(s.GeneralStatusKey, 1) = 1", 
                                          businessKey.Key);
        }
    }
}
