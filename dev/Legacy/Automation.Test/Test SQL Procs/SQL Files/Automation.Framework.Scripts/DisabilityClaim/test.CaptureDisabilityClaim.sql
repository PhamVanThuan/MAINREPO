use [2AM]
go

set ansi_nulls on
go
set quoted_identifier on
go

if exists (select * from sys.objects where object_id = object_id(N'[2AM].[test].[CaptureDisabilityClaim]') and type in (N'P',N'PC'))
drop procedure test.CaptureDisabilityClaim
go

create procedure test.CaptureDisabilityClaim

@disabilityClaimKey int

as
begin
	
	declare @date date = getdate()
	declare @dateOfDiagnosis datetime = (select dateadd(dd,((abs(CHECKSUM(newid())) % 10) * -1),@date))
	declare @lastDateWorked datetime = (select dateadd(dd,((abs(CHECKSUM(newid())) % 20) * -1),@date))
	declare @disabilityTypeKey int = (select top 1 DisabilityTypeKey from DisabilityType order by newid())	

	create table #occupation (Number int, Job varchar(50))
	insert into #occupation values (1, 'Boat builder')
	insert into #occupation values (2, 'Racing driver')
	insert into #occupation values (3, 'Fitness instructor')
	insert into #occupation values (4, 'Train driver')
	insert into #occupation values (5, 'Lifeguard')
	declare @claimantOccupation varchar(50) = (select top 1 Job from #occupation order by newid())
	drop table #occupation

	update [2AM].dbo.DisabilityClaim
	set DateOfDiagnosis = @dateOfDiagnosis,
	DisabilityTypeKey = @disabilityTypeKey,
	ClaimantOccupation = @claimantOccupation,
	LastDateWorked = @lastDateWorked	
	where DisabilityClaimKey = @disabilityClaimKey

end
go