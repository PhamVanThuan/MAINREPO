using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;

namespace SAHL.Services.ApplicationDomain.Managers.Application.Statements
{
    public class DoesOpenApplicationExistStatement : ISqlStatement<int>
    {
        public int ApplicationNumber { get; protected set; }

        public int ApplicationStatus { get; protected set; }

        public DoesOpenApplicationExistStatement(int applicationNumber)
        {
            ApplicationNumber = applicationNumber;
            ApplicationStatus = (int)OfferStatus.Open;
        }

        public string GetStatement()
        {
            return @"SELECT count(*) AS Total FROM [2AM].[dbo].[Offer] WHERE OfferKey = @ApplicationNumber AND OfferStatusKey = @ApplicationStatus";
        }
    }
}