USE [FETest]
GO

IF(EXISTS(SELECT 1 FROM SYS.sysobjects where name = 'OpenThirdPartyInvoices' and type = 'V'))
	BEGIN
		DROP VIEW [dbo].[OpenThirdPartyInvoices]
	end

go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE VIEW [dbo].OpenThirdPartyInvoices

AS

select t.ThirdPartyInvoiceKey, d.InstanceID, s.Name as WorkflowState, replace(ad.ADUserName, 'SAHL\', '') as AssignedUser, t.InvoiceStatusKey
from [2am].dbo.ThirdPartyInvoice t
join x2.x2data.third_party_invoices d on t.ThirdPartyInvoiceKey=d.ThirdPartyInvoiceKey
join x2.x2.instance i on d.InstanceID = i.ID
join x2.x2.state s on i.stateId=s.Id
join x2.x2.StateWorklist sw on s.Id = sw.StateID
join x2.x2.SecurityGroup sg on sw.SecurityGroupID=sg.ID
	and sg.IsDynamic = 1
join x2.x2.assignment a on i.Id=a.InstanceId
join [2am].dbo.UserOrganisationStructure uos on a.UserOrganisationStructureKey=uos.UserOrganisationStructureKey
join [2am].dbo.ADUser ad on uos.aduserkey=ad.ADUserKey
where s.Type <> 5 and s.Name <> 'Other Invoice Received'
union all
--static assignment won't have assignment records
select t.ThirdPartyInvoiceKey, d.InstanceID, s.Name as WorkflowState, replace(w.ADUserName, 'SAHL\', '')as AssignedUser, t.InvoiceStatusKey
from [2am].dbo.ThirdPartyInvoice t
join x2.x2data.third_party_invoices d on t.ThirdPartyInvoiceKey=d.ThirdPartyInvoiceKey
join x2.x2.instance i on d.InstanceID = i.ID
join x2.x2.state s on i.stateId=s.Id
join x2.x2.StateWorklist sw on s.Id = sw.StateID
join x2.x2.SecurityGroup sg on sw.SecurityGroupID=sg.ID
	and sg.IsDynamic = 0
join x2.x2.worklist w on i.Id = w.InstanceID
where s.Type <> 5 and s.name <> 'Other Invoice Received'











