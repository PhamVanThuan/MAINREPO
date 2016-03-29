use [2am]

go

if not exists(select * from sys.columns where Name = N'InvoiceNumber' and Object_ID = Object_ID(N'[2AM].[dbo].[ThirdPartyInvoice]'))
begin
    -- Column Exists
	ALTER TABLE [2AM].[dbo].[ThirdPartyInvoice]
	ADD InvoiceNumber Varchar(50)
end

if not exists(select * from sys.columns  where Name = N'InvoiceDate' and Object_ID = Object_ID(N'[2AM].[dbo].[ThirdPartyInvoice]'))
begin
	ALTER TABLE [2AM].[dbo].[ThirdPartyInvoice]
	ADD InvoiceDate DateTime
end