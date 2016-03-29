USE [2AM]
go

delete from test.TestVariable

declare @groupname varchar(max)
declare @state varchar(max)
set @groupname = 'Valuations'

set @state = 'Valuation Review Request'
insert into test.TestVariable 
select distinct top 01 'TestValuationReviewRequests',@groupname,offerkey from dbo.offer
	inner join x2.x2data.valuations with (nolock)
		on valuations.applicationkey = offer.offerkey
	inner join x2.x2.instance with (nolock)
		on valuations.instanceid = instance.id
	inner join x2.x2.state with (nolock)
		on instance.stateid= state.id
where offer.offerstatuskey = 1 and state.name = @state and offer.offerkey not in (select offerkey from test.TestVariable) 

set @state = 'Further Valuation Request'
insert into test.TestVariable
select distinct  top 01 'TestFurtherValuationRequests',@groupname,offerkey from dbo.offer
	inner join x2.x2data.valuations with (nolock)
		on valuations.applicationkey = offer.offerkey
	inner join x2.x2.instance with (nolock)
		on valuations.instanceid = instance.id
	inner join x2.x2.state with (nolock)
		on instance.stateid= state.id
where offer.offerstatuskey = 1 and state.name = @state and offer.offerkey not in (select offerkey from test.TestVariable) 

set @state = 'Schedule Valuation Assessment'
insert into test.TestVariable
select distinct  top 01 'TestInstructLightstoneValuations',@groupname,offerkey from dbo.offer
	inner join x2.x2data.valuations with (nolock)
		on valuations.applicationkey = offer.offerkey
	inner join x2.x2.instance with (nolock)
		on valuations.instanceid = instance.id
	inner join x2.x2.state with (nolock)
		on instance.stateid= state.id
where offer.offerstatuskey = 1 and state.name = @state and offer.offerkey not in (select offerkey from test.TestVariable) 

set @state = 'Schedule Valuation Assessment'
insert into test.TestVariable
select distinct  top 01 'TestInstructLightstoneValuations',@groupname,offerkey from dbo.offer
	inner join x2.x2data.valuations with (nolock)
		on valuations.applicationkey = offer.offerkey
	inner join x2.x2.instance with (nolock)
		on valuations.instanceid = instance.id
	inner join x2.x2.state with (nolock)
		on instance.stateid= state.id
where offer.offerstatuskey = 1 and state.name = @state and offer.offerkey not in (select offerkey from test.TestVariable) 

set @state = 'Valuation Assessment Pending'
insert into test.TestVariable
select distinct  top 01 'TestLightstoneValuationRejected',@groupname,offerkey from dbo.offer
	inner join x2.x2data.valuations with (nolock)
		on valuations.applicationkey = offer.offerkey
	inner join x2.x2.instance with (nolock)
		on valuations.instanceid = instance.id
	inner join x2.x2.state with (nolock)
		on instance.stateid= state.id
where offer.offerstatuskey = 1 and state.name = @state and offer.offerkey not in (select offerkey from test.TestVariable) 

set @state = 'Schedule Valuation Assessment'
insert into test.TestVariable
select distinct  top 01 'TestInstructAdcheckValuations',@groupname,offerkey from dbo.offer
	inner join x2.x2data.valuations with (nolock)
		on valuations.applicationkey = offer.offerkey
	inner join x2.x2.instance with (nolock)
		on valuations.instanceid = instance.id
	inner join x2.x2.state with (nolock)
		on instance.stateid= state.id
where offer.offerstatuskey = 1 and state.name = @state and offer.offerkey not in (select offerkey from test.TestVariable) 

set @state = 'Schedule Valuation Assessment'
insert into test.TestVariable
select distinct  top 01 'TestManualValuations',@groupname,offerkey from dbo.offer
	inner join x2.x2data.valuations with (nolock)
		on valuations.applicationkey = offer.offerkey
	inner join x2.x2.instance with (nolock)
		on valuations.instanceid = instance.id
	inner join x2.x2.state with (nolock)
		on instance.stateid= state.id
where offer.offerstatuskey = 1 and state.name = @state and offer.offerkey not in (select offerkey from test.TestVariable) 

set @state = 'Valuation Assessment Pending'
insert into test.TestVariable
select distinct  top 01 'TestPendingValuationAssessmentOverdue',@groupname,offerkey from dbo.offer
	inner join x2.x2data.valuations with (nolock)
		on valuations.applicationkey = offer.offerkey
	inner join x2.x2.instance with (nolock)
		on valuations.instanceid = instance.id
	inner join x2.x2.state with (nolock)
		on instance.stateid= state.id
where offer.offerstatuskey = 1 and state.name = @state and offer.offerkey not in (select offerkey from test.TestVariable) 

set @state = 'Manager Review'
insert into test.TestVariable
select distinct  top 01 'TestManagerArchives',@groupname,offerkey from dbo.offer
	inner join x2.x2data.valuations with (nolock)
		on valuations.applicationkey = offer.offerkey
	inner join x2.x2.instance with (nolock)
		on valuations.instanceid = instance.id
	inner join x2.x2.state with (nolock)
		on instance.stateid= state.id
where offer.offerstatuskey = 1 and state.name = @state and offer.offerkey not in (select offerkey from test.TestVariable) 

set @state = 'Schedule Valuation Assessment'
insert into test.TestVariable
select distinct  top 01 'TestReAssignValuation',@groupname,offerkey from dbo.offer
	inner join x2.x2data.valuations with (nolock)
		on valuations.applicationkey = offer.offerkey
	inner join x2.x2.instance with (nolock)
		on valuations.instanceid = instance.id
	inner join x2.x2.state with (nolock)
		on instance.stateid= state.id
where offer.offerstatuskey = 1 and state.name = @state and offer.offerkey not in (select offerkey from test.TestVariable) 

set @state = 'Schedule Valuation Assessment'
insert into test.TestVariable
select distinct  top 01 'TestManagerEscalations',@groupname,offerkey from dbo.offer
	inner join x2.x2data.valuations with (nolock)
		on valuations.applicationkey = offer.offerkey
	inner join x2.x2.instance with (nolock)
		on valuations.instanceid = instance.id
	inner join x2.x2.state with (nolock)
		on instance.stateid= state.id
where offer.offerstatuskey = 1 and state.name = @state and offer.offerkey not in (select offerkey from test.TestVariable) 

set @state = 'Valuation Assessment Pending'
insert into test.TestVariable
select distinct  top 01 'TestValuationWithdrawals',@groupname,offerkey from dbo.offer
	inner join x2.x2data.valuations with (nolock)
		on valuations.applicationkey = offer.offerkey
	inner join x2.x2.instance with (nolock)
		on valuations.instanceid = instance.id
	inner join x2.x2.state with (nolock)
		on instance.stateid= state.id
where offer.offerstatuskey = 1 and state.name = @state and offer.offerkey not in (select offerkey from test.TestVariable) 

set @state = 'Withdraw Valuation Assessment'
insert into test.TestVariable
select distinct  top 01 'TestValuationCancelWithdrawals',@groupname,offerkey from dbo.offer
	inner join x2.x2data.valuations with (nolock)
		on valuations.applicationkey = offer.offerkey
	inner join x2.x2.instance with (nolock)
		on valuations.instanceid = instance.id
	inner join x2.x2.state with (nolock)
		on instance.stateid= state.id
where offer.offerstatuskey = 1 and state.name = @state and offer.offerkey not in (select offerkey from test.TestVariable) 

set @state = 'Valuation Complete'
insert into test.TestVariable
select distinct  top 01 'TestPropertyOnOpenAccountValuationInOrder',@groupname,offerkey from dbo.offer
	inner join x2.x2data.valuations with (nolock)
		on valuations.applicationkey = offer.offerkey
	inner join x2.x2.instance with (nolock)
		on valuations.instanceid = instance.id
	inner join x2.x2.state with (nolock)
		on instance.stateid= state.id
where offer.offerstatuskey = 1 and state.name = @state and offer.offerkey not in (select offerkey from test.TestVariable) 

set @state = 'Valuation Complete'
insert into test.TestVariable
select distinct  top 01 'TestPropertyNotOnOpenAccountValuationInOrder',@groupname,offerkey from dbo.offer
	inner join x2.x2data.valuations with (nolock)
		on valuations.applicationkey = offer.offerkey
	inner join x2.x2.instance with (nolock)
		on valuations.instanceid = instance.id
	inner join x2.x2.state with (nolock)
		on instance.stateid= state.id
where offer.offerstatuskey = 1 and state.name = @state and offer.offerkey not in (select offerkey from test.TestVariable)