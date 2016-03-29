using SAHL.Core.Data;

namespace SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment.Statements
{
    public class GetBondInstalmentForFurtherLendingApplicationStatement : ISqlStatement<double?>
    {
        public GetBondInstalmentForFurtherLendingApplicationStatement(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }

        public string GetStatement()
        {// calculate the new amortised instalment because it is not stored
            return @"select (((oivl.[MarketRate] + m.Value) / 12) + (((oivl.[MarketRate] + m.Value) / 12) / (POWER((1 + ((oivl.[MarketRate] + m.Value) / 12)), oivl.Term) - 1))) * (oivl.ExistingLoan + oivl.LoanAgreementAmount)
                        from [2AM].[dbo].[Offer] o
                        join (select max(OfferInformationKey) OfferInformationKey, OfferKey
		                        from [2AM].[dbo].[OfferInformation]
		                        group by OfferKey
	                        ) maxoi on maxoi.OfferKey = o.OfferKey
                        join [2AM].[dbo].[OfferInformationVariableLoan] oivl on oivl.OfferInformationKey = maxoi.OfferInformationKey
                        join [2AM].[dbo].[RateConfiguration] rc on rc.RateConfigurationKey = oivl.RateConfigurationKey
                        join [2AM].[dbo].[Margin] m on m.MarginKey = rc.MarginKey
                        where o.OfferKey = @ApplicationKey
                        and o.OfferTypeKey in (2, 3, 4)";
        }
    }
}