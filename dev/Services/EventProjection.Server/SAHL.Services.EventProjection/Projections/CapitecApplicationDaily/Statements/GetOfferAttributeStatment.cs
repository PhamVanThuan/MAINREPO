using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.EventProjection.Projections.CapitecApplicationDaily.Statements
{
    public class GetOfferAttributeStatment : ISqlStatement<OfferAttributeDataModel>
    {
        public int OfferKey { get; protected set; }

        public int OfferAttributeTypeKey { get; protected set; }

        public GetOfferAttributeStatment(int offerKey, OfferAttributeType offerAttributeTypeKey)
        {
            this.OfferKey = offerKey;
            this.OfferAttributeTypeKey = (int)offerAttributeTypeKey;
        }

        public string GetStatement()
        {
            return @"SELECT * FROM [2AM].dbo.OfferAttribute
  WHERE OfferAttributeTypeKey = @OfferAttributeTypeKey
  AND OfferKey = @OfferKey";
        }
    }
}