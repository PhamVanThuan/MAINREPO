using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;
using System.Linq;

namespace SAHL.Services.DomainQuery.QueryStatements
{
    public class GetClientDetailsQueryStatement : IServiceQuerySqlStatement<GetClientDetailsQuery, GetClientDetailsQueryResult>
    {
        public string GetStatement()
        {
            string query = @"select top 1
              LegalEntityKey, FirstNames, Surname, DateOfBirth, IDNumber, HomePhoneCode,
              HomePhoneNumber as HomePhone, WorkPhoneCode, WorkPhoneNumber as WorkPhone, CellPhoneNumber as Cellphone, EmailAddress, LegalEntityTypeKey as LegalEntityType
              from [2am].dbo.legalEntity where legalEntityKey = @ClientKey";
            return query;
        }
    }
}