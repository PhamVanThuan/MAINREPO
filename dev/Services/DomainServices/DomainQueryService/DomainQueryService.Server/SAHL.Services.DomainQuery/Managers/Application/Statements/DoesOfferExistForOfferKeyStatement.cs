using SAHL.Core.Data;

namespace SAHL.Services.DomainQuery.Managers.Application.Statements
{
    public class DoesOfferExistForOfferKeyStatement : ISqlStatement<int>
    {
        public int OfferKey { get; private set; }

        public DoesOfferExistForOfferKeyStatement(int offerKey)
        {
            this.OfferKey = offerKey;
        }

        public string GetStatement()
        {
            return "SELECT COUNT(1) FROM[2AM].dbo.Offer WHERE OfferKey = @OfferKey";
        }
    }
}