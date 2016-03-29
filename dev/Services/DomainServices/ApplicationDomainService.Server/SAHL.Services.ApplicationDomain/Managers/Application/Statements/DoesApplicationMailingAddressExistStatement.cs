using SAHL.Core.Data;

namespace SAHL.Services.ApplicationDomain.Managers.Application.Statements
{
    public class DoesApplicationMailingAddressExistStatement : ISqlStatement<int>
    {
        public int ApplicationNumber { get; protected set; }

        public DoesApplicationMailingAddressExistStatement(int applicationNumber)
        {
            ApplicationNumber = applicationNumber;
        }

        public string GetStatement()
        {
            return "SELECT COUNT(*) AS Total FROM [2am].[dbo].[OfferMailingAddress] WHERE [OfferKey] = @ApplicationNumber";
        }
    }
}