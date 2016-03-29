using SAHL.Core.Data;

namespace SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment.Statements
{
    public class GetBondInstalmentForNewBusinessApplicationStatement : ISqlStatement<double?>
    {
        public GetBondInstalmentForNewBusinessApplicationStatement(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }

        public string GetStatement()
        {
            return @"select oivl.MonthlyInstalment
                        from [2AM].[dbo].[Offer] o
                        join (select max(OfferInformationKey) OfferInformationKey, OfferKey
                                from [2AM].[dbo].[OfferInformation]
                                group by OfferKey
                            ) maxoi on maxoi.OfferKey = o.OfferKey
                        join [2AM].[dbo].[OfferInformationVariableLoan] oivl on oivl.OfferInformationKey = maxoi.OfferInformationKey
                        where o.OfferKey = @ApplicationKey
                        and o.OfferTypeKey in (6, 7, 8)";
        }
    }
}