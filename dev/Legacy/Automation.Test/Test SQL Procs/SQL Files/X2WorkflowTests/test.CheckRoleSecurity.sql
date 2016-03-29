USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.CheckRoleSecurity') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.CheckRoleSecurity  
	Print 'Dropped Proc test.CheckRoleSecurity'
End
Go

CREATE PROCEDURE test.CheckRoleSecurity
	@activityName varchar(max) ='',
	@stateName varchar(max) ='',
	@roleName varchar(max),
	@workflowName varchar(max)
AS
BEGIN

declare @exists bit
declare @workflowId int

declare @securitygroup_static table (id int, name varchar(max))
declare @securitygroup_dynamic table (id int, name varchar(max))

declare @staticRoleCount bit
declare @dynamicRoleCount bit
declare @rowcount bit

select @workflowId = max(id) from x2.x2.workflow where name = @workflowName

-- Get the securitygroup for all static roles
insert into @securitygroup_static
select id, name from x2.X2.SecurityGroup with (nolock)
where isdynamic = 0 and workflowid is null and name = @roleName

-- Get the securitygroup for all dynamic roles
insert into @securitygroup_dynamic
select id,name from x2.X2.SecurityGroup with (nolock)
where isdynamic = 1 and  workflowid is not null and name = @roleName

select @staticRoleCount = count(*) from @securitygroup_static
select @dynamicRoleCount = count(*) from @securitygroup_dynamic

set @rowcount = 0

if (@staticRoleCount = 1)
begin
	select @rowcount = count(*) from x2.x2.activitysecurity as acs with (nolock)
		inner join x2.x2.activity as a
			on acs.activityid = a.id
		inner join x2.x2.state as s
			on a.stateid = s.id 
	where securitygroupid in (select id	from @securitygroup_static )
	and a.workflowid = @workflowId and s.workflowid =@workflowId
	and s.name = @stateName and a.name = @activityName
end
if (@dynamicRoleCount = 1)
begin
	select @rowcount = count(*)  from x2.x2.activitysecurity as acs with (nolock)
		inner join x2.x2.activity as a
			on acs.activityid = a.id
		inner join x2.x2.state as s
			on a.stateid = s.id 
	where securitygroupid in (select id	from @securitygroup_dynamic )
	and a.workflowid = @workflowId and s.workflowid =@workflowId
	and s.name = @stateName and a.name = @activityName
end

if (@rowcount > 0)
begin
select convert(bit,1)
end
else
begin
select convert(bit,0)
end


END