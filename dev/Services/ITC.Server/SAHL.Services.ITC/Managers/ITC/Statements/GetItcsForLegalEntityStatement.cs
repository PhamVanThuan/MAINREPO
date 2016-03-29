using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ITC.Managers.Itc.Statements
{
    public class GetItcsForLegalEntityStatement : ISqlStatement<ITCDataModel>
    {
        public string IdentityNumber { get; protected set; }

        public GetItcsForLegalEntityStatement(string identityNumber)
        {
            this.IdentityNumber = identityNumber;
        }

        public string GetStatement()
        {
            return @"SELECT itc.* FROM [2AM].dbo.ITC
                    JOIN[2AM].dbo.LegalEntity le ON itc.LegalEntityKey = le.LegalEntityKey
                    WHERE le.IDNumber = @IdentityNumber";
        }
    }
}