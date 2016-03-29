using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ApplicationDomain.Managers.Application.Statements
{
    public class LinkPropertyToApplicationStatement : ISqlStatement<OfferMortgageLoanDataModel>
    {
        public int ApplicationNumber { get; protected set; }
        public int PropertyKey { get; protected set; }

        public LinkPropertyToApplicationStatement(int ApplicationNumber, int PropertyKey)
        {
            this.ApplicationNumber = ApplicationNumber;
            this.PropertyKey = PropertyKey;
        }

        public string GetStatement()
        {
            var query = @"UPDATE [2AM].[dbo].[OfferMortgageLoan] SET [PropertyKey] = @PropertyKey WHERE [OfferKey] = @ApplicationNumber";
            return query;
        }
    }
}
