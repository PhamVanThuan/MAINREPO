USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.CreateValuationTestCases') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.CreateValuationTestCases
	Print 'Dropped Proc test.CreateValuationTestCases'
End
Go
Create Procedure test.CreateValuationTestCases
AS

declare @FurtherValuationRequest int
declare @ValuationReviewRequest int
declare @ManualValuation int
declare @AdcheckValuation int
declare @ValuationManager int

select top 01 @FurtherValuationRequest = valuations.applicationkey from x2.x2data.valuations (nolock)
	inner join x2.x2.instance (nolock)
		on valuations.instanceid = instance.id
	inner join x2.x2.state (nolock)
		on state.id = instance.stateid
	inner join dbo.offer (nolock)
		on valuations.applicationkey = offer.offerkey
	left join test.ValuationTestCases
		on offer.offerkey = ValuationTestCases.applicationkey 
		and ValuationTestCases.applicationkey != valuations.applicationkey  
where state.name like '%Further Valuation Request%' and ValuationTestCases.applicationkey is null

select top 01 @ValuationReviewRequest = valuations.applicationkey  from x2.x2data.valuations (nolock)
	inner join x2.x2.instance (nolock)
		on valuations.instanceid = instance.id
	inner join x2.x2.state (nolock)
		on state.id = instance.stateid
	inner join dbo.offer (nolock)
		on valuations.applicationkey = offer.offerkey
			left join test.ValuationTestCases
		on offer.offerkey = ValuationTestCases.applicationkey 
		and ValuationTestCases.applicationkey != valuations.applicationkey  
where state.name like '%Valuation Review Request%' and ValuationTestCases.applicationkey is null

select top 01 @ManualValuation = valuations.applicationkey  from x2.x2data.valuations (nolock)
	inner join x2.x2.instance (nolock)
		on valuations.instanceid = instance.id
	inner join x2.x2.state (nolock)
		on state.id = instance.stateid
	inner join dbo.offer (nolock)
		on valuations.applicationkey = offer.offerkey
	left join test.ValuationTestCases
		on offer.offerkey = ValuationTestCases.applicationkey 
		and ValuationTestCases.applicationkey != valuations.applicationkey  
where state.name like '%Schedule Valuation Assessment%' 

select top 01 @AdcheckValuation = valuations.applicationkey  from x2.x2data.valuations (nolock)
	inner join x2.x2.instance (nolock)
		on valuations.instanceid = instance.id
	inner join x2.x2.state (nolock)
		on state.id = instance.stateid
	inner join dbo.offer (nolock)
		on valuations.applicationkey = offer.offerkey
	left join test.ValuationTestCases
		on offer.offerkey = ValuationTestCases.applicationkey 
		and ValuationTestCases.applicationkey != valuations.applicationkey  
where state.name like '%Schedule Valuation Assessment%' and ValuationTestCases.applicationkey is null

select top 01 @ValuationManager = valuations.applicationkey  from x2.x2data.valuations (nolock)
	inner join x2.x2.instance (nolock)
		on valuations.instanceid = instance.id
	inner join x2.x2.state (nolock)
		on state.id = instance.stateid
	inner join dbo.offer (nolock)
		on valuations.applicationkey = offer.offerkey
	left join test.ValuationTestCases
		on offer.offerkey = ValuationTestCases.applicationkey 
		and ValuationTestCases.applicationkey != 
where state.name like '%Schedule Valuation Assessment%' and ValuationTestCases.applicationkey is null


update test.ValuationTestCases
set ValuationTestCases.ApplicationKey = @ValuationReviewRequest
where ValuationTestCases.TestGroup = 'ValuationReview'

update test.ValuationTestCases
set ValuationTestCases.ApplicationKey = @FurtherValuationRequest
where ValuationTestCases.TestGroup = 'FurtherValuation'

update test.ValuationTestCases
set ValuationTestCases.ApplicationKey = @ManualValuation
where ValuationTestCases.TestGroup = 'ManualValuation'

update test.ValuationTestCases
set ValuationTestCases.ApplicationKey = @AdcheckValuation
where ValuationTestCases.TestGroup = 'AdcheckValuation'


update test.ValuationTestCases
set ValuationTestCases.ApplicationKey = @ValuationManager
where ValuationTestCases.TestGroup = 'ValuationManager'