using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class LinkOfferMortgageLoanPropertyCommand : ServiceCommand, IFrontEndTestCommand
    {
        public int OfferKey { get; protected set; }

        public int PropertyKey { get; protected set; }

        public LinkOfferMortgageLoanPropertyCommand(int offerKey, int propertyKey)
        {
            this.OfferKey = offerKey;
            this.PropertyKey = propertyKey;
        }
    }
}