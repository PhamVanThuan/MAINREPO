using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;

namespace SAHL.Services.Capitec.Managers.ITC.Statements
{
    public class GetItcsForIdentityNumberStatement : ISqlStatement<ITCDataModel>
    {
        public string IdentityNumber { get; protected set; }

        public GetItcsForIdentityNumberStatement(string identityNumber)
        {
            this.IdentityNumber = identityNumber;
        }

        public string GetStatement()
        {
            return @"SELECT TOP 1 itc.*
                        FROM [Capitec].[dbo].[ITC] itc
                        Join [Capitec].[dbo].PersonITC pitc on pitc.CurrentITCId = itc.ID
                        Join [Capitec].[dbo].Person p on p.Id = pitc.Id
                        Where p.IdentityNumber = @IdentityNumber
                        Order by itc.ITCDate desc";
        }
    }
}