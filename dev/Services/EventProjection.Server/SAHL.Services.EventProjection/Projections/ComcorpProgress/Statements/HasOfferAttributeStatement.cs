using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.EventProjection.Projections.ComcorpProgress.Statements
{
    public class HasOfferAttributeStatement : ISqlStatement<OfferAttributeDataModel>
    {
        public int OfferKey
        {
            get;
            protected set;
        }

        public int OfferAttributeTypeKey
        {
            get;
            protected set;
        }

        public HasOfferAttributeStatement(int offerKey, SAHL.Core.BusinessModel.Enums.OfferAttributeType offerAttributeTypeKey)
        {
            this.OfferKey = offerKey;
            this.OfferAttributeTypeKey = (int)offerAttributeTypeKey;
        }

        public string GetStatement()
        {
            return @"SELECT TOP 1 OfferAttributeKey,OfferKey,OfferAttributeTypeKey
FROM [2am].dbo.OfferAttribute
WHERE OfferKey = @OfferKey AND OfferAttributeTypeKey = @OfferAttributeTypeKey";
        }
    }
}