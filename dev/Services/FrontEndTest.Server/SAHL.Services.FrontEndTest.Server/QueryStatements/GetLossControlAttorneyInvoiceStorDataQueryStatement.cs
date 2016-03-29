using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetLossControlAttorneyInvoiceStorDataQueryStatement : IServiceQuerySqlStatement<GetLossControlAttorneyInvoiceStorDataQuery, GetLossControlAttorneyInvoiceStorDataQueryResult>
    {
        public int ThirdPartyInvoiceKey { get; protected set; }
        public string InvoiceNumber { get; protected set; }
        public GetLossControlAttorneyInvoiceStorDataQueryStatement(int thirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }
        public string GetStatement()
        {
            return @"select d.ID,
	                    d.archivedate,
	                    d.STOR,
	                    d.GUID,
	                    d.Extension,
	                    d.Key1 as AccountKey,
	                    d.Key2 as ThirdPartyInvoiceKey,
	                    d.Key3 as EmailSubject,
	                    d.Key4 as FromEmailAddress,
	                    d.Key5 as InvoiceFileName,
	                    d.Key6 as Category,
	                    d.Key7 as Datereceived,
	                    d.Key8 as DateProcessed,
	                    s.Folder + '\' + cast(datepart(YEAR,d.archiveDate) as varchar(4)) + '\' + 
		                    case 
			                    when datepart(Month,d.archiveDate) < 10 
			                    then '0'+cast(datepart(Month,d.archiveDate) as varchar(2)) 
			                    else cast(datepart(Month,d.archiveDate) as varchar(2)) 
		                    end + '\' + d.GUID as FilePath
                    from ImageIndex.dbo.Data d
	                    join ImageIndex.dbo.Stor s on d.STOR = s.ID
                    where stor = 44
	                    and d.Key2 = @ThirdPartyInvoiceKey";
        }
    }
}