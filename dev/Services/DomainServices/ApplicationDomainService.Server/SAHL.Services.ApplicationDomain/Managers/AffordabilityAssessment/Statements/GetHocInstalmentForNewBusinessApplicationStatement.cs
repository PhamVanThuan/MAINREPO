using SAHL.Core.Data;

namespace SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment.Statements
{
    public class GetHocInstalmentForNewBusinessApplicationStatement : ISqlStatement<double?>
    {
        public GetHocInstalmentForNewBusinessApplicationStatement(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }

        public string GetStatement()
        {
            return @"select h.HOCMonthlyPremium
                        from [2AM].[dbo].[Offer] o
                        join [2AM].[dbo].[OfferAccountRelationship] oar on oar.OfferKey = o.OfferKey
                        join [2AM].[dbo].[FinancialService] fs on fs.AccountKey = oar.AccountKey
	                        and fs.AccountStatusKey = 3
	                        and fs.FinancialServiceTypeKey = 4
                        join [2AM].[dbo].[HOC] h on h.FinancialServiceKey = fs.FinancialServiceKey
	                        and h.HOCInsurerKey = 2
                        where o.OfferKey = @ApplicationKey
                        and o.OfferTypeKey in (6, 7, 8)";
        }
    }
}