use [FeTest]
go

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'PopulateEmptyThirdPartyInvoices')
	DROP PROCEDURE dbo.PopulateEmptyThirdPartyInvoices
GO

CREATE PROCEDURE dbo.PopulateEmptyThirdPartyInvoices

AS


IF(EXISTS(SELECT 1 FROM FETest.dbo.EmptyThirdPartyInvoices))
	BEGIN
		truncate table FETest.dbo.EmptyThirdPartyInvoices
	END

			INSERT INTO FETest.dbo.EmptyThirdPartyInvoices 
			(ThirdPartyInvoiceKey)
			SELECT  t.ThirdPartyInvoiceKey FROM [2am].dbo.ThirdPartyInvoice t
			left join [2am].dbo.InvoiceLineItem l on t.ThirdPartyInvoiceKey=l.ThirdPartyInvoiceKey
			where 
			l.ThirdPartyInvoiceKey is null
			and invoiceDate is null
			and ThirdPartyId is null
			and InvoiceStatusKey = 1
			and InvoiceNumber is null
			and SahlReference like 'SAHL-IntegrationTest%'

			if((select count(*) from FETest.dbo.EmptyThirdPartyInvoices) < 100)
			begin

				insert into [2am].dbo.ThirdPartyInvoice
				(
				[SahlReference],
				[InvoiceStatusKey], 
				[AccountKey], 
				[ThirdPartyId], 
				[ReceivedFromEmailAddress],
				[InvoiceNumber],	
				[InvoiceDate],
				[ReceivedDate], 
				[CapitaliseInvoice], 
				[AmountExcludingVAT], 
				[VATAmount] , 
				[TotalAmountIncludingVAT]
				)
				select top 1000 
				'SAHL-IntegrationTest'+ CAST(ROW_NUMBER() OVER (ORDER BY a.AccountKey) as varchar(5)), 
				1,  
				a.AccountKey, null,
				'halouser@sahomeloans.com',
				null,
				null,
				getdate(),
				0,
				0,
				0, 
				0
				from [2am].dbo.Account a 
				left join [2am].dbo.ThirdPartyInvoice t on a.AccountKey = t.AccountKey
				where t.accountkey is null
				and a.AccountStatusKey =1
				and a.RRR_ProductKey = 9
				order by a.AccountKey asc

				truncate table FETest.dbo.EmptyThirdPartyInvoices 

				INSERT INTO FETest.dbo.EmptyThirdPartyInvoices 
				(ThirdPartyInvoiceKey)
				SELECT  t.ThirdPartyInvoiceKey FROM [2am].dbo.ThirdPartyInvoice t
				left join [2am].dbo.InvoiceLineItem l on t.ThirdPartyInvoiceKey=l.ThirdPartyInvoiceKey
				where 
				l.ThirdPartyInvoiceKey is null
				and invoiceDate is null
				and ThirdPartyId is null
				and InvoiceNumber is null
				and SahlReference like 'SAHL-IntegrationTest%'

			end
				
