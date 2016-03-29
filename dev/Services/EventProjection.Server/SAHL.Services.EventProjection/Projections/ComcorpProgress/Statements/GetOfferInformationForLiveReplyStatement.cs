using SAHL.Core.Data;
using SAHL.Services.EventProjection.Projections.ComcorpProgress.Model;

namespace SAHL.Services.EventProjection.Projections.ComcorpProgress.Statements
{
    public class GetOfferInformationForLiveReplyStatement : ISqlStatement<ComcorpApplicationInformationDataModel>
    {
        public int OfferKey
        {
            get;
            protected set;
        }

        public GetOfferInformationForLiveReplyStatement(int offerKey)
        {
            this.OfferKey = offerKey;
        }

        public string GetStatement()
        {
            return @"SELECT o.ReservedAccountKey,o.Reference,
	                   (
		                SELECT
			                CASE WHEN o.OfferTypeKey <> 6
				                THEN LoanAmountNoFees
				                ELSE LoanAgreementAmount
			                END
		                FROM [2am].[dbo].[OfferInformationVariableLoan]
		                WHERE OfferInformationKey = MIN(oi.OfferInformationKey)
	                   ) as RequestedAmount,
	                   (
		                SELECT
			                CASE WHEN o.OfferTypeKey <> 6
				                THEN LoanAmountNoFees
				                ELSE LoanAgreementAmount
			                END
		                FROM [2am].[dbo].[OfferInformationVariableLoan]
		                WHERE OfferInformationKey = MAX(oi.OfferInformationKey)
	                   ) as offeredAmount,
	                   (
		                SELECT BondToRegister FROM [2am].[dbo].[OfferInformationVariableLoan]
		                WHERE OfferInformationKey = MAX(oi.OfferInformationKey)
	                   ) as RegisteredAmount
                FROM [2am].[dbo].[Offer] o
                LEFT OUTER JOIN [2am].[dbo].[OfferInformation] oi ON oi.OfferKey = o.OfferKey
                WHERE o.OfferKey = @OfferKey
                GROUP BY o.ReservedAccountKey,o.Reference,o.OfferTypeKey";
        }
    }
}