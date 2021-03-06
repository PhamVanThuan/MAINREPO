USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[ReassignX2CasesForDebtCounselling]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[ReassignX2CasesForDebtCounselling]
	Print 'Dropped Proc [test].[ReassignX2CasesForDebtCounselling]'
End
Go
--usage exec test.[ReassignX2CasesForDebtCounselling] 'SAHL\DCCUser', 'Debt Counselling Consultant D', 5
CREATE PROCEDURE [test].[ReassignX2CasesForDebtCounselling]

 @TestUser varchar(max), 
 @WorkflowRoleType varchar(max),
 @CasesPerState int
 
 
AS
begin


declare @states table (stateID int)
declare @stateID int
declare @LegalEntityKey int
declare @WorkflowID int
declare @AdUserKey int
declare @WorkflowRoleTypeKey int
declare @stateName varchar(50)

set @WorkflowRoleTypeKey = (select workflowRoleTypeKey from [2am].dbo.WorkflowRoleType where description = @WorkflowRoleType)
set @LegalEntityKey = (select legalEntityKey from ADUser where ADUserName = @TestUser)
set @WorkflowID = (select max(id) from x2.x2.workflow where name = 'Debt Counselling')
select @LegalEntityKey = LegalEntityKey, @ADUserKey = ADUserKey from [2am].dbo.Aduser where adusername = @TestUser

if object_id('tempdb..#Consultants') IS NOT NULL
	begin
		drop table #Consultants
	end

select a.aduserkey, a.adusername
into #Consultants 
from userOrganisationstructure u
join aduser a on u.aduserkey=a.aduserkey
where generickey=@WorkflowRoleTypeKey and genericKeyTypeKey=34
and u.generalStatusKey = 2

declare assignmentCursor cursor local
for 
select StateID, s.Name
from x2.x2.stateWorklist w
join x2.x2.securityGroup sg on w.securityGroupID = sg.ID
join x2.x2.state s on w.stateID=s.id
where sg.workflowid=@WorkflowID and sg.name = @WorkflowRoleType
union all
select s.ID, s.Name 
from x2.x2.state s
where s.workflowid=@WorkflowID
and s.name = 'Debt Review Approved'

open assignmentCursor;
fetch next from assignmentCursor into
@stateID, @stateName

while (@@fetch_status = 0)

begin

if object_id('tempdb..#casesToUse') IS NOT NULL
	begin
		drop table #casesToUse
	end
	
create table #casesToUse
(
workflowRoleAssignmentID int,
WorklistID int,
InstanceID bigint,
adusername varchar(max),
NewConsultant varchar(max),
NewConsultantADUser int,
NewConsultantLEKey int,
debtCounsellingKey int
)

if (@stateName <> 'Debt Review Approved')

begin
	insert into #casesToUse
	select top (@CasesPerState) w.ID as workflowRoleAssignmentID, 
	wl.ID as WorklistID, 
	w.InstanceID, 
	wl.adusername, 
	@TestUser as NewConsultant,
	@ADUserKey as NewConsultantADUser,
	@LegalEntityKey as NewConsultantLEKey,
	debtCounsellingKey
	from x2.x2.workflowRoleAssignment w
	join #consultants c on w.aduserkey=c.aduserkey
		and w.generalStatusKey=1
	join x2.x2.worklist wl on wl.adusername = c.adusername
		and w.instanceid = wl.instanceid
	join x2.x2.instance i on w.instanceid=i.id
		and i.stateid = @stateID 
	join x2.x2data.debt_counselling dc on i.id=dc.instanceid
	
end

else

begin
	insert into #casesToUse
	select top (@CasesPerState) w.ID as workflowRoleAssignmentID, 0 as WorklistID, w.InstanceID, a.adusername, 
	@TestUser as NewConsultant,
	@ADUserKey as NewConsultantADUser,
	@LegalEntityKey as NewConsultantLEKey,
	debtCounsellingKey
	from x2.x2.workflowRoleAssignment w
	join #consultants c on w.aduserkey=c.aduserkey
		and w.generalStatusKey=1
	join x2.x2.instance i on w.instanceid=i.id
		and i.stateid = @stateID 
	join x2.x2data.debt_counselling dc on i.id=dc.instanceid
	join [2am].dbo.AdUser a on w.aduserKey = a.adUserKey

end


select *, @stateName from #casesToUse

--update worklist
update w
set adusername = NewConsultant
from x2.x2.worklist w 
join #casesToUse c on w.id=c.WorklistID
where c.WorklistID <> 0 --handles Debt Review Approved
--update workflowRoleAssignment
update wr
set aduserKey = NewConsultantADUser
from x2.x2.workflowRoleAssignment wr
join #casesToUse c on wr.id=c.workflowRoleAssignmentID
--update workflow Role
update wfr
set LegalEntityKey = NewConsultantLEKey, statusChangeDate = getdate()
from [2am].dbo.WorkflowRole wfr
join #casesToUse c on wfr.genericKey = c.DebtCounsellingKey
where wfr.WorkflowRoleTypeKey = 2
	and wfr.generalStatusKey = 1
--update security
update ias
set adusername = NewConsultant
from x2.x2.instanceActivitySecurity ias
join #casesToUse c on ias.instanceid = c.instanceid
	and ias.aduserName = c.adusername

fetch next from assignmentCursor into
@stateID, @stateName

end

close assignmentCursor;
deallocate assignmentCursor;

end

SET ANSI_NULLS OFF
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO


--call it now for our test users
exec test.[ReassignX2CasesForDebtCounselling] 'SAHL\DCCUser', 'Debt Counselling Consultant D', 50
exec test.[ReassignX2CasesForDebtCounselling] 'SAHL\DCCCUser', 'Debt Counselling Court Consultant D', 50
exec test.[ReassignX2CasesForDebtCounselling] 'SAHL\DCSUser', 'Debt Counselling Supervisor D', 50
exec test.[ReassignX2CasesForDebtCounselling] 'SAHL\DCAUser', 'Debt Counselling Admin D', 50

