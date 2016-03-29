using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetEmployerQueryStatement : IServiceQuerySqlStatement<GetEmployerQuery, GetEmployerQueryResult>
    {
        public string GetStatement()
        {
            var query = @"select top 1 EmployerKey, Name, TelephoneCode, TelephoneNumber, ContactPerson, ContactEmail, EmployerBusinessTypeKey as EmployerBusinessType, 
                EmploymentSectorKey as EmploymentSector
                from [2am].dbo.Employer
                where len(name) > 5 and name <> 'Unknown'
                order by newid()";
            return query;
        }
    }
}