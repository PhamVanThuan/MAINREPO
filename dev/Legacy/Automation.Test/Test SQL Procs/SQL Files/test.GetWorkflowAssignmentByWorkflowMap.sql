USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[GetWorkflowAssignmentByWorkflowMap]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[GetWorkflowAssignmentByWorkflowMap]
	Print 'Dropped procedure [test].[GetWorkflowAssignmentByWorkflowMap]'
End
Go

/*
This procedure will retrieve the workflow assignment records for an instance in a workflow
when provided with the OfferKey and a Worflow. It also fetches the relevant OfferRole records
for the application.
e.g. EXEC test.GetWorkflowAssignmentByWorkflowMap 852431, 'Application Capture'
*/

CREATE PROCEDURE [test].[GetWorkflowAssignmentByWorkflowMap]

	@OfferKey INT,
	@WorkflowName VARCHAR(100)

AS
BEGIN

declare @var_offerKey varchar(max)
set @var_offerKey = cast(@OfferKey as varchar(max))
 
if (@WorkflowName <> 'Debt Counselling')
begin
	SELECT 
	row_number() over (order by w.id asc) as [Order], a.adusername as [User], ort.description as RoleType, insertdate as InsertDate, 
	gs.description as [Status]
	FROM 
	[x2].[x2].instance i join 
	[x2].[x2].[WorkflowAssignment] w on i.id=w.instanceid
	JOIN [2am].[dbo].[OfferRoleTypeOrganisationStructureMapping] o
	ON w.[OfferRoleTypeOrganisationStructureMappingKey]=o.[OfferRoleTypeOrganisationStructureMappingKey]
	JOIN [2am].[dbo].ADUser a ON w.ADUserKey=a.ADUserKey
	join x2.x2.state s on i.stateid=s.id
	join x2.x2.workflow wf on s.workflowid=wf.id
	join [2am]..offerroletype ort on o.offerroletypekey=ort.offerroletypekey
	join [2am]..generalstatus gs on w.generalstatuskey=gs.generalstatuskey
	WHERE i.name = @var_offerKey and isnumeric(i.name)=1
	and wf.name= @WorkflowName
	ORDER BY w.[InsertDate] ASC
end

if (@WorkflowName = 'Debt Counselling')
begin
	SELECT 
	row_number() over (order by w.id asc) as [Order], a.adusername as [User], wrt.description as RoleType, insertdate as InsertDate, 
	gs.description as [Status]
	FROM x2.x2data.debt_counselling d
	join x2.x2.instance i 
	on d.instanceid=i.id
	join x2.x2.WorkflowRoleAssignment w 
	on i.id=w.instanceid
	join [2am]..WorkflowRoleTypeOrganisationStructureMapping map
	on w.WorkflowRoleTypeOrganisationStructureMappingKey=map.WorkflowRoleTypeOrganisationStructureMappingKey
	join [2am]..aduser a on w.aduserkey=a.aduserkey
	join x2.x2.state s on i.stateid=s.id
	join x2.x2.workflow wf on s.workflowid=wf.id
	join [2am]..workflowRoleType wrt on map.workflowRoleTypeKey=wrt.workflowRoleTypeKey
	join [2am]..generalstatus gs on w.generalStatusKey=gs.generalStatusKey
	where d.debtcounsellingkey=@OfferKey
end
	END
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

