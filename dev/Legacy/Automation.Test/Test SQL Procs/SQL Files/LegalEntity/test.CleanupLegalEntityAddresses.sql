USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.CleanupLegalEntityAddresses') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.CleanupLegalEntityAddresses
	Print 'Dropped procedure test.CleanupLegalEntityAddresses'
End
Go

CREATE PROCEDURE test.CleanupLegalEntityAddresses
	@LegalEntityKey int,
	@GeneralStatusKey int,
	@Delete int

as
begin

if (@Delete = 1)
	begin
		select lea.legalEntityAddressKey
		into #AddressesToDelete 
		from [2am].dbo.LegalEntityAddress lea
		left join [2am].dbo.LegalEntityDomicilium led on lea.legalEntityAddressKey = led.legalEntityAddressKey
		left join [2am].dbo.MailingAddress ma on lea.AddressKey = ma.AddressKey
			and ma.legalEntityKey = @LegalEntityKey
		left join [2am].dbo.OfferMailingAddress oma on lea.AddressKey = oma.AddressKey
			and oma.legalEntityKey = @LegalEntityKey
		where lea.legalEntityKey = @LegalEntityKey
		and (led.legalEntityDomiciliumKey is null
		and ma.MailingAddressAccountKey is null
		and oma.OfferMailingAddressKey is null
		)
		
		delete from [2am].dbo.LegalEntityAddress where legalEntityAddressKey in 
		(select legalEntityAddressKey from #AddressesToDelete)
	end
else
	begin
		update [2am].dbo.LegalEntityAddress 
		set GeneralStatusKey = @GeneralStatusKey
		where legalEntityKey = @LegalEntityKey
	end
end


