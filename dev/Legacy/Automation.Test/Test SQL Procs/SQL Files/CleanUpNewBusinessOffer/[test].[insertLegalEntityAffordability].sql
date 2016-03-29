USE [2AM]
GO
/****** 
Object:  StoredProcedure [test].[insertLegalEntityAffordability]    
Script Date: 10/19/2010 16:48:58 
******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[insertLegalEntityAffordability]') AND type in (N'P', N'PC'))
DROP PROCEDURE [test].insertLegalEntityAffordability

GO

create procedure test.insertLegalEntityAffordability

@offerKey int

as

declare @offerTypeKey int

select @offerTypeKey = offerTypeKey
from [2am].dbo.Offer 
where offerKey = @offerKey

if @offerTypeKey = 11

begin

	--remove existing records
	delete from [2am].dbo.LegalEntityAffordability
	where offerKey = @offerKey
	
	--one income record
	insert into [2am].dbo.LegalEntityAffordability
	select legalEntityKey, 1, 50000, NULL, offerKey 
	from [2am].dbo.offer o
	join [2am].dbo.externalRole er on o.offerKey = er.genericKey
	and er.externalRoleTypeKey = 1
	and er.genericKeyTypeKey = 2
	where o.offerkey = @offerKey
	
	--one expense record
	insert into [2am].dbo.LegalEntityAffordability
	select legalEntityKey, 7, 10000, NULL, offerKey 
	from [2am].dbo.offer o
	join [2am].dbo.externalRole er on o.offerKey = er.genericKey
	and er.externalRoleTypeKey = 1
	and er.genericKeyTypeKey = 2
	where o.offerkey = @offerKey

end

