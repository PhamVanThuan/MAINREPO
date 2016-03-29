using SAHL.Core.Data;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Managers.Applicant.Statements
{
    public class GetClientDateOfBirthStatement : ISqlStatement<DateTime?>
    {
        public int ClientKey { get; protected set; }

        public GetClientDateOfBirthStatement(int clientKey)
        {
            this.ClientKey = clientKey;
        }

        public string GetStatement()
        {
            string query = @"select isnull(dateOfBirth,0) from [2am].dbo.LegalEntity where LegalEntityKey = @ClientKey";
            return query;
        }
    }
}