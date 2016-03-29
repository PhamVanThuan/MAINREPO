USE [2AM]
GO

SET IDENTITY_INSERT [OrgStruct].[Capability] ON

GO

declare @capabilityKey int
set @capabilityKey = 1

If Not Exists (Select [CapabilityKey] From [OrgStruct].[Capability] Where [CapabilityKey] = @capabilityKey)
Begin
	INSERT INTO [OrgStruct].[Capability] ([CapabilityKey],[Description]) Values (@capabilityKey, 'Invoice Processor')
End

set @capabilityKey = 2

If Not Exists (Select [CapabilityKey] From [OrgStruct].[Capability] Where [CapabilityKey] = @capabilityKey)
Begin
	INSERT INTO [OrgStruct].[Capability] ([CapabilityKey],[Description]) Values (@capabilityKey, 'Loss Control Fee Invoice Approver Under R15000')
End

set @capabilityKey = 3

If Not Exists (Select [CapabilityKey] From [OrgStruct].[Capability] Where [CapabilityKey] = @capabilityKey)
Begin
	INSERT INTO [OrgStruct].[Capability] ([CapabilityKey],[Description]) Values (@capabilityKey, 'Loss Control Fee Invoice Approver Under R30000')
End

set @capabilityKey = 4

If Not Exists (Select [CapabilityKey] From [OrgStruct].[Capability] Where [CapabilityKey] = @capabilityKey)
Begin
	INSERT INTO [OrgStruct].[Capability] ([CapabilityKey],[Description]) Values (@capabilityKey, 'Loss Control Fee Invoice Approver Up to R60000')
End

set @capabilityKey = 5

If Not Exists (Select [CapabilityKey] From [OrgStruct].[Capability] Where [CapabilityKey] = @capabilityKey)
Begin
	INSERT INTO [OrgStruct].[Capability] ([CapabilityKey],[Description]) Values (@capabilityKey, 'Invoice Approver Over R60000')
End

set @capabilityKey = 6

If Not Exists (Select [CapabilityKey] From [OrgStruct].[Capability] Where [CapabilityKey] = @capabilityKey)
Begin
	INSERT INTO [OrgStruct].[Capability] ([CapabilityKey],[Description]) Values (@capabilityKey, 'Invoice Payment Processor')
End

set @capabilityKey = 7

If Not Exists (Select [CapabilityKey] From [OrgStruct].[Capability] Where [CapabilityKey] = @capabilityKey)
Begin
	INSERT INTO [OrgStruct].[Capability] ([CapabilityKey],[Description]) Values (@capabilityKey, 'Invoice Approver')
End

set @capabilityKey = 8

If Not Exists (Select [CapabilityKey] From [OrgStruct].[Capability] Where [CapabilityKey] = @capabilityKey)
Begin
	INSERT INTO [OrgStruct].[Capability] ([CapabilityKey],[Description]) Values (@capabilityKey, 'Loss Control Fee Consultant')
End

SET IDENTITY_INSERT [OrgStruct].[Capability] OFF