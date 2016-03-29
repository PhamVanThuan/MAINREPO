use [2AM]
GO
set ansi_nulls on
go
set quoted_identifier on
go
if exists (select * from dbo.sysobjects where id = object_id(N'[test].[ReAssignLifeLeads]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
begin
	drop procedure [test].[ReAssignLifeLeads]
	print 'Dropped Proc [test].[ReAssignLifeLeads]'
end
go
create procedure [test].[ReAssignLifeLeads]
	@adusername varchar(max),
	@loanNumber int
as
begin
	declare @LCUserLEKey int
	declare @offerkeys table (offerkey int)
	declare @instanceId table (instanceid int)
	declare @aduserkey int

	insert into @offerkeys
	select  o.offerkey from [2am].dbo.offerrole as ofr
		inner join dbo.offer as o
			on ofr.offerkey=o.offerkey
		inner join x2.x2data.lifeorigination lo
			on ofr.offerkey=o.offerkey
		inner join x2.x2.instance i 
			on lo.instanceid=i.id
		inner join x2.x2.state s 
			on i.stateid=s.id
		inner join dbo.LifeOfferAssignment loa
			on o.offerkey=loa.offerkey
		inner join x2.x2.worklist wl
			on i.id=wl.instanceid
	where offertypekey = 5 and o.offerstatuskey = 1 and ofr.generalstatuskey = 1 and s.type = 1 and lo.LoanNumber = @loanNumber
	order by o.offerkey desc

	select @LCUserLEKey=legalentitykey 
	from [2am].dbo.legalentity
	where firstnames = @adusername
	
	select @aduserkey=aduserkey from dbo.aduser
	where legalentitykey =@LCUserLEKey

	insert into @instanceId
	select instanceid from x2.x2data.lifeorigination (nolock)
	where offerkey in (select offerkey from @offerkeys)

	update dbo.LifeOfferAssignment
	set adusername = @adusername
	where offerkey in (select offerkey from @offerkeys)

	update dbo.offerrole
	set legalentitykey = @LCUserLEKey
	where offerkey in (select offerkey from @offerkeys) and offerroletypekey = 1

	update x2.x2data.lifeorigination 
	set assignto = @adusername
	where offerkey in (select offerkey from @offerkeys)

	update dbo.offerlife
	set consultant = @adusername
	where offerkey in (select offerkey from @offerkeys)

	update x2.x2.worklist
	set adusername = @adusername
	where instanceid in (select instanceid from @instanceId)

	update x2.X2.WorkflowAssignment
	set aduserkey=@aduserkey
	where instanceid in (select instanceid from @instanceId)

	update x2.x2.InstanceActivitySecurity
	set adusername = @adusername
	where instanceid in (select instanceid from @instanceId) and adusername like '%SAHL\%'
end
				



