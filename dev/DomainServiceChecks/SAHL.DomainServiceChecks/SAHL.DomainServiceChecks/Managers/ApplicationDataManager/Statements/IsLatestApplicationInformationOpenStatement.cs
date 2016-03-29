using SAHL.Core.Data;

namespace SAHL.DomainServiceChecks.Managers.ApplicationDataManager.Statements
{
    public class IsLatestApplicationInformationOpenStatement : ISqlStatement<int>
    {
        public IsLatestApplicationInformationOpenStatement(int applicationNumber)
        {
            this.ApplicationNumber = applicationNumber;
        }

        public int ApplicationNumber { get; protected set; }

        public string GetStatement()
        {
            var sqlQuery = @"
select case
when isnull(oi.OfferInformationTypeKey, 3) = 3 then 0
  else 1
end 
from [2am].dbo.OfferInformation oi
where oi.OfferInformationKey =
  (
    select max(OfferInformationKey) 
    from [2am].dbo.OfferInformation where OfferKey = @applicationNumber
  )";

            return sqlQuery;
        }
    }
}
