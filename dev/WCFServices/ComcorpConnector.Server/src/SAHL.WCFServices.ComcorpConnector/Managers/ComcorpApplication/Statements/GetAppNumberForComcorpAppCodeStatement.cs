using SAHL.Core.Data;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ComcorpApplication.Statements
{
    public class GetAppNumberForComcorpAppCodeStatement : ISqlStatement<int?>
    {
        public string ComcorpApplicationCode { get; protected set; }

        public GetAppNumberForComcorpAppCodeStatement(long comcorpApplicationCode)
        {
            this.ComcorpApplicationCode = comcorpApplicationCode.ToString();
        }

        public string GetStatement()
        {
            return @"SELECT TOP 10 * FROM [2AM].dbo.Offer o
                    JOIN OfferAttribute oa ON oa.OfferKey = o.OfferKey
                    WHERE oa.OfferAttributeTypeKey = 31
                    AND o.Reference = @ComcorpApplicationCode";
        }
    }
}