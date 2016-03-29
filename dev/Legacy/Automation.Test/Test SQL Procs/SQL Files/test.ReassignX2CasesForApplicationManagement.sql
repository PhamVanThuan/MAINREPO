USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[ReassignX2CasesForApplicationManagement]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[ReassignX2CasesForApplicationManagement]
	Print 'Dropped Proc [test].[ReassignX2CasesForApplicationManagement]'
End
Go
--usage exec test.[ReassignX2CasesForApplicationManagement] 'SAHL\DCCUser', 'Debt Counselling Consultant D', 5
CREATE PROCEDURE [test].[ReassignX2CasesForApplicationManagement]

 @TestUser varchar(max), 
 @OfferRoleType varchar(max),
 @CasesPerState int
 
 
AS
begin


declare @states table (stateID int)
declare @stateID int
declare @LegalEntityKey int
declare @WorkflowID int
declare @AdUserKey int
declare @OfferRoleTypeKey int

set @OfferRoleTypeKey = (select offerRoleTypeKey from [2am].dbo.OfferRoleType where description = @OfferRoleType)
--select @Offerroletypekey
set @LegalEntityKey = (select legalEntityKey from ADUser where ADUserName = @TestUser)
set @WorkflowID = (select max(id) from x2.x2.workflow where name = 'Application Management')
select @LegalEntityKey = LegalEntityKey, @ADUserKey = ADUserKey from [2am].dbo.Aduser where adusername = @TestUser

if object_id('tempdb..#Consultants') IS NOT NULL
	begin
		drop table #Consultants
	end

select a.aduserkey, a.adusername 
into #Consultants 
from [2am].dbo.offerroletypeorganisationstructuremapping ortosm 
join [2am].dbo.offerroletype ort on ortosm.offerroletypekey = ort.offerroletypekey 
join [2am].dbo.organisationstructure os on ortosm.organisationstructurekey = os.organisationstructurekey 
join [2am].dbo.userorganisationstructure uos on os.organisationstructurekey = uos.organisationstructurekey 
join [2am].dbo.aduser a on uos.aduserkey = a.aduserkey
where 
ortosm.offerroletypekey = @OfferRoleTypeKey
and a.generalstatuskey = 2

declare assignmentCursor cursor local
for 
select StateID
from x2.x2.stateWorklist w
join x2.x2.securityGroup sg on w.securityGroupID = sg.ID
join x2.x2.state s on w.stateID=s.id 
	and s.type <> 5
where sg.workflowid=@WorkflowID and sg.name = @OfferRoleType

open assignmentCursor;
fetch next from assignmentCursor into
@stateID

while (@@fetch_status = 0)

begin

--select * from x2.x2.state where id = @stateid

if object_id('tempdb..#casesToUse') IS NOT NULL
	begin
		drop table #casesToUse
	end

select top (@CasesPerState) w.ID as workflowAssignmentID, wl.ID as WorklistID, w.InstanceID, wl.adusername, 
@TestUser as NewConsultant,
@ADUserKey as NewConsultantADUser,
@LegalEntityKey as NewConsultantLEKey,
applicationkey
into #casesToUse
from x2.x2.workflowAssignment w
join #consultants c on w.aduserkey=c.aduserkey
	and w.generalStatusKey=1
join [2am].dbo.offerroletypeorganisationstructuremapping ortosm on w.offerroletypeorganisationstructuremappingkey = ortosm.offerroletypeorganisationstructuremappingkey
	and ortosm.offerroletypekey = @offerroletypekey
join x2.x2.worklist wl on wl.adusername = c.adusername
	and w.instanceid = wl.instanceid
join x2.x2.instance i on w.instanceid=i.id
	and i.stateid = @stateID 
join x2.x2data.application_management dc on i.id=dc.instanceid

select * from #casesToUse

--update worklist
update w
set adusername = NewConsultant
from x2.x2.worklist w 
join #casesToUse c on w.id=c.WorklistID

--update workflowRoleAssignment
update wr
set aduserKey = NewConsultantADUser
from x2.x2.workflowAssignment wr
join #casesToUse c on wr.id=c.workflowAssignmentID

--update workflow Role
update wfr
set LegalEntityKey = NewConsultantLEKey, statusChangeDate = getdate()
from [2am].dbo.OfferRole wfr
join #casesToUse c on wfr.offerkey = c.applicationkey
where wfr.OfferRoleTypeKey = @OfferRoleTypekey
	and wfr.generalStatusKey = 1

fetch next from assignmentCursor into
@stateID

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
exec test.[ReassignX2CasesForApplicationManagement] 'SAHL\NBPUser1', 'New Business Processor D', 100

exec test.[ReassignX2CasesForApplicationManagement] 'SAHL\FLAppProcUser3', 'FL Processor D', 100
