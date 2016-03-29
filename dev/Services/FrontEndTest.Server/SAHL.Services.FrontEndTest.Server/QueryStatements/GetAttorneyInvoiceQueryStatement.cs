using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetAttorneyInvoiceQueryStatement : IServiceQuerySqlStatement<GetAttorneyInvoiceQuery, GetAttorneyInvoiceQueryResult>
    {
        public string GetStatement()
        {
            return @"select top 1 ID, STOR, GUID, Extension, Key1 as LoanNumber, Key2 as ThirdPartyInvoiceKey, Key3 as EmailSubject,
                        Key4 as FromEmailAddress, Key5 as InvoiceFileName, Key6 as Category, Key7 as DateRecieved, Key8 as DateProcess
                     from [ImageIndex].[dbo].[Data]
                     where Key3 = @EmailSubject";
        }
    }
}