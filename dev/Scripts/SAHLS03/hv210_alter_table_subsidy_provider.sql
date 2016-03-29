use [2am]
go

if not exists (select * from sys.columns WHERE Name = N'GEPFAffiliate' and Object_ID = Object_ID(N'SubsidyProvider'))
begin
	alter table [2am].[dbo].SubsidyProvider add GEPFAffiliate BIT NOT NULL DEFAULT 0;
end

