using SAHL.Core.Data;

namespace SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment.Statements
{
    public class GetHocInstalmentForFurtherLendingApplicationStatement : ISqlStatement<double?>
    {
        public GetHocInstalmentForFurtherLendingApplicationStatement(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }

        public string GetStatement()
        {
            return @"select h.HOCMonthlyPremium
                        from [2AM].[dbo].[Offer] o
                        join [2AM].[dbo].[FinancialService] fs on fs.AccountKey = o.AccountKey
							and fs.AccountStatusKey = 1
							and fs.FinancialServiceTypeKey = 1
						join [2AM].[dbo].[FinancialService] fs_hoc on fs_hoc.ParentFinancialServiceKey = fs.FinancialServiceKey
							and fs_hoc.AccountStatusKey = 1
							and fs_hoc.FinancialServiceTypeKey = 4
                        join [2AM].[dbo].[HOC] h on h.FinancialServiceKey = fs_hoc.FinancialServiceKey
							and h.HOCInsurerKey = 2
                        where o.OfferKey = @ApplicationKey
						and o.OfferTypeKey in (2, 3, 4)";
        }
    }
}