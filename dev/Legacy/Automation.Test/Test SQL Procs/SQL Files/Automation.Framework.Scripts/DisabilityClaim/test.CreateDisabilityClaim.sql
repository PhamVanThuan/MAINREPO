use [2AM]
go

set ansi_nulls on
go
set quoted_identifier on
go

if exists (select * from sys.objects where object_id = object_id(N'[2AM].[test].[CreateDisabilityClaim]') and type in (N'P',N'PC'))
drop procedure test.CreateDisabilityClaim
go

create procedure test.CreateDisabilityClaim

@lifeAccountKey int,
@legalEntityKey int

as
begin
	
	declare @disabilityClaimKey int

	insert into [2AM].dbo.DisabilityClaim (AccountKey, LegalEntityKey, DateClaimReceived, DisabilityClaimStatusKey)
	values (@lifeAccountKey, @legalEntityKey, getdate(), 1)

	set @disabilityClaimKey = scope_identity()

	update [2AM].test.DisabilityClaimAutomationCases
	set DisabilityClaimKey = @disabilityClaimKey
	where LifeAccountKey = @lifeAccountKey
	and LegalEntityKey = @legalEntityKey

end
go