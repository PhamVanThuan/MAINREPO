use [2am]

go

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'InsertCapability')

	BEGIN

		DROP PROCEDURE dbo.InsertCapability

	END

GO

CREATE PROC dbo.InsertCapability

@capabilityKey int,
@userOrganisationStructureKey int

AS

BEGIN

	IF NOT EXISTS (SELECT 1 FROM [2AM].OrgStruct.UserOrganisationStructureCapability where UserOrganisationStructureKey = @UserOrganisationStructureKey 
		and CapabilityKey = @capabilityKey)
			BEGIN
			insert into [2am].OrgStruct.UserOrganisationStructureCapability
				(CapabilityKey, UserOrganisationStructureKey)
				values 
				( @capabilityKey, @userOrganisationStructureKey)
			END

END


