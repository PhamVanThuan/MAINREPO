using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ClientDomain.Managers.Client.Statements
{
    public class FindClientByIdNumberStatement : ISqlStatement<LegalEntityDataModel>
    {
        public string IdentityNumber { get; protected set; }

        public FindClientByIdNumberStatement(string identityNumber)
        {
            this.IdentityNumber = identityNumber;
        }

        public string GetStatement()
        {
            string query = "select top 1 * from [2am].dbo.LegalEntity where IdNumber = @IdentityNumber";
            return query;
        }
    }
}