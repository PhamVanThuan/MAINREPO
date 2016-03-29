use [2AM]
GO
set ansi_nulls on
go
set quoted_identifier on
go
if exists (select * from dbo.sysobjects where id = object_id(N'[test].[GetLifeLeads]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
begin
	drop procedure [test].[GetLifeLeads]
	print 'Dropped Proc [test].[GetLifeLeads]'
end

go
create procedure [test].[GetLifeLeads]
	@stateName varchar(max),
	@cloneTimerName varchar(max),
	@policyTypeKey varchar(max),
	@loanNumber int
as
begin

	create table #results (
		InstanceId int,
		OfferKey int,
		LifeAccountKey int,
		AccountKey int,
		ProductSwitchReason varchar(max),
		ConfirmationRequired bit,
		StateName varchar(max),
		CreationDate datetime,
		AssignedConsultant varchar(max)
	)
	if (@stateName is not null)
	begin
		select i.id,i.ParentInstanceID,i.CreationDate,stateid
		into #instances
		from x2.x2.Instance i (nolock)
			join x2.x2.State s (nolock)
				on i.stateid=s.ID
		where s.Name = @stateName
		order by i.CreationDate desc

		insert into #instances
		select i.id,i.ParentInstanceID,i.CreationDate,stateid
		from x2.x2.Instance i (nolock)
			join x2.x2.State s (nolock)
				on i.stateid=s.ID
		where s.Name = @cloneTimerName and i.ParentInstanceID in (select id from #instances)
		order by i.CreationDate desc

		insert into #results
		select
			i.ID,
			lo.OfferKey,
			o.accountkey as LifeAccountKey,
			lo.LoanNumber as AccountKey,
			null as ProductSwitchReason,
			ConfirmationRequired,
			s.Name as StateName,
			i.CreationDate,
			wl.ADUserName as AssignedConsultant
		from x2.x2data.LifeOrigination lo (nolock)
			join #instances i (nolock)
				on lo.instanceid = i.id
			join x2.x2.state s (nolock)
				on i.stateid=s.ID
			left join x2.x2.WorkList  wl (nolock)
				on i.ID=wl.InstanceID
			left join [2am].dbo.offerlife ol  (nolock)
				on lo.offerkey=ol.offerkey
			left join [2am].dbo.offer o  (nolock)
				on ol.offerkey=o.offerkey
		where ol.LifePolicyTypeKey=@policyTypeKey
		drop table #instances
	end
	if (@loanNumber > 0 and @loanNumber is not null)
	begin
		insert into #results
		select
			i.ID,
			lo.OfferKey,
			o.accountkey as LifeAccountKey,
			lo.LoanNumber as AccountKey,
			null as ProductSwitchReason,
			ConfirmationRequired,
			s.Name as StateName,
			i.CreationDate,
			wl.ADUserName as AssignedConsultant
		from x2.x2data.LifeOrigination lo (nolock)
			join X2.X2.Instance i (nolock) 
				on lo.instanceid = i.id
			join x2.x2.state s (nolock)
				on i.stateid=s.ID
			left join x2.x2.WorkList  wl (nolock)
				on i.ID=wl.InstanceID
			left join [2am].dbo.offerlife ol (nolock)
				on lo.offerkey=ol.offerkey
			left join [2am].dbo.offer o (nolock)
				on ol.offerkey=o.offerkey
		where lo.loannumber = @loanNumber
		delete from #results
		where accountkey != @loanNumber
	end
	if (@loanNumber > 0 and @loanNumber is not null and @stateName is not null)
	begin
		delete from #results
		where StateName != @stateName 
	end

	select distinct * from #results
	order by CreationDate desc
	drop table #results

end
				



