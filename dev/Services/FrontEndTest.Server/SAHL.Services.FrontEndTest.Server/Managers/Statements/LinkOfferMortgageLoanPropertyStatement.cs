using SAHL.Core.Data;
using System;

namespace SAHL.Services.FrontEndTest.Managers.Statements
{
    public class LinkOfferMortgageLoanPropertyStatement : ISqlStatement<int>
    {
        public int OfferKey { get; protected set; }

        public int PropertyKey { get; protected set; }

        public LinkOfferMortgageLoanPropertyStatement(int offerKey, int propertyKey)
        {
            this.OfferKey = offerKey;
            this.PropertyKey = propertyKey;
        }

        public string GetStatement()
        {
            return @"Update [2am].dbo.OfferMortgageLoan set PropertyKey = @PropertyKey where OfferKey = @OfferKey";
        }
    }
}