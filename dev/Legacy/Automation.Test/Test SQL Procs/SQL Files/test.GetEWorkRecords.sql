USE [2AM]
GO

/****** Object:  StoredProcedure [test].[GetEWorkRecords]    Script Date: 09/22/2011 17:40:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.GetEWorkRecords') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.GetEWorkRecords 
	Print 'Dropped procedure test.GetEWorkRecords'
End
Go

CREATE PROCEDURE [test].[GetEWorkRecords]
	@eMapName varchar(max),
	@eStageName varchar(max),
	@accountkey int = 0
AS

IF OBJECT_ID('tempdb.test.#efolderrecords') IS NOT NULL
BEGIN
	DROP TABLE test.#efolderrecords
END

BEGIN
	create table #efolderrecords
	(
		efolderid varchar(max),
		accountkey int,
		StageName varchar(max),
		backtostage varchar(max),
		IDNumber varchar(max),
		AssignedUser varchar(max),
		IsSubsidised int
	)
	declare @efolderrecordHolder table
	(
		efolderid varchar(max),
		accountkey int,
		StageName varchar(max),
		backtostage varchar(max),
		IDNumber varchar(max),
		AssignedUser varchar(max),
		IsSubsidised int
	)
	insert into #efolderrecords
	select
		max(efolder.efolderid),
		case when isnumeric(efolder.efoldername)= 1
		 then efolder.efoldername else 0 end as accountkey,
		max(efolder.estagename) as estagename,
		max(losscontrol.backtostagename) as backtostage,
		max(legalentity.idnumber) as idnumber,
		max(losscontrol.usertodo) as AssignedUser,
		case when max(Subsidised) = 0
		then 0 else 1 end as IsSubsidised
	from (select * from [e-work].dbo.efolder  with (nolock) 
			where efolder.estagename in 
			(
				select estagename from [e-work].dbo.estage
				where emapname = @eMapName
					and estage.estagename = @eStageName
			)
			and isnumeric(efolderid) = 1) as efolder
		inner join [e-work].dbo.losscontrol with (nolock) on efolder.efolderid = losscontrol.efolderid
			and efolder.eArchived = 0
			and backToStageName is not null
		inner join dbo.legalentity with (nolock) on losscontrol.clientidnumber = legalentity.idnumber
			and legalentity.legalentitytypekey = 2
	group by efolder.efoldername, efolder.efolderid
	order by efolder.efoldername, efolder.efolderid	desc
			 
	if (@accountkey > 0)
		begin
			insert into @efolderrecordHolder
			select #efolderrecords.* from #efolderrecords
			where #efolderrecords.accountkey = @accountkey
			delete from #efolderrecords
			insert into #efolderrecords
			select * from @efolderrecordHolder
		end
	else
		begin
			delete from #efolderrecords
			where accountkey in
			(
				select debtcounselling.accountkey from debtcounselling.debtcounselling 
				where debtcounselling.debtcounsellingstatuskey in (1,3,4)
			)			
			insert into @efolderrecordHolder
			select #efolderrecords.* from #efolderrecords 
			delete from #efolderrecords
			insert into #efolderrecords
			select * from @efolderrecordHolder
		end
	
	select
		max(#efolderrecords.accountkey) as accountkey,
		max(#efolderrecords.backtostage) as BackToStage,
		max(@eMapName) as eMapName,
		max(#efolderrecords.StageName) as StageName,
		max(account.rrr_OriginationSourceKey) as OriginationSourceKey,
		max(#efolderrecords.idnumber) as IDNumber,
		max(loanstatistics.MonthsInArrears) as MonthsInArrears,
		case when max(#efolderrecords.issubsidised) = 0
		then convert(bit,0) else convert(bit,1) end as IsSubsidised,
		max(account.rrr_productKey) as ProductKey,
		max(#efolderrecords.AssignedUser) as AssignedUser,
		max(#efolderrecords.efolderid) as efolderid,
		max(#efolderrecords.accountkey) as accountkey
	from #efolderrecords
		inner join [2am].dbo.account
			on #efolderrecords.accountkey = account.accountkey
				and account.accountstatuskey=1
		inner join [sahldb].dbo.loanstatistics 
			on account.accountkey = loanstatistics.loannumber
			--and loanstatistics.monthsinarrears > 0.00000
	group by #efolderrecords.accountkey, #efolderrecords.efolderid
	order by newid()
	
END
GO