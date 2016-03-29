using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Managers.Client.Statements
{
    public class FindClientByPassportNumberStatement : ISqlStatement<LegalEntityDataModel>
    {
        public string PassportNumber { get; protected set; }

        public FindClientByPassportNumberStatement(string passportNumber)
        {
            this.PassportNumber = passportNumber;
        }

        public string GetStatement()
        {
            string query = "select top 1 * from [2am].dbo.LegalEntity where PassportNumber = @PassportNumber";
            return query;
        }
    }
}