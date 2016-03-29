USE [2AM]
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[GetValuationOffersAtState]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[GetValuationOffersAtState]
	Print 'Dropped procedure [test].[GetValuationOffersAtState]'
End
Go

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [test].[GetValuationOffersAtState]
	@AmendedValuations bit,
	@StateName varchar(max)
AS
BEGIN
	if object_id('tempdb..#valuationOffers') is not null
	begin
		drop table #valuationOffers
	end
	if object_id('tempdb..#valuations') is not null
	begin
		drop table #valuations
	end
	select o.*
	into #valuationOffers
	from x2.x2data.valuations v
		join x2.x2.instance i 
			on v.instanceid=i.id	
		join x2.x2.state s 
			on i.stateid=s.id
		join dbo.offer o
			on v.applicationkey =o.offerkey
	where s.name=@StateName and o.offerstatuskey = 1
	
	select v.*,x.xmldata.value('(//PropertyDetails/PreviousUniqueID)[1]','INT') as AmendedVal 
	into #valuations
	from #valuationOffers v
		join dbo.xmlhistory x
			on v.offerkey=x.generickey
			and generickeytypekey = 2
	
	if (@AmendedValuations = 1)
	begin
		select distinct * from dbo.offer
		where offerkey in (select offerkey from #valuations	where offerkey not in (select offerkey from #valuations where AmendedVal is null ))
	end
	else
	begin
		select distinct * from dbo.offer
		where offerkey in (select offerkey from #valuations	where offerkey not in (select offerkey from #valuations where AmendedVal is not null ))
	end
END




