USE [2AM]
GO

BEGIN TRY

	DECLARE @RuleExclusionSetKey INT

	SET @RuleExclusionSetKey = 60

	BEGIN TRAN

	IF EXISTS(SELECT 1 FROM [2AM].[dbo].[RuleExclusionSet] WHERE [RuleExclusionSetKey] = @RuleExclusionSetKey AND [Description] != 'AffordabilityAssessmentRelatedLegalEntity')
	BEGIN
		RAISERROR('RuleExclusionSetKey = 60 ALREADY EXISTS', 16, 1)
	END

	IF NOT EXISTS (SELECT 1 FROM [2AM].[dbo].[RuleExclusionSet] WHERE [RuleExclusionSetKey] = @RuleExclusionSetKey AND [Description] = 'AffordabilityAssessmentRelatedLegalEntity')
	BEGIN
		INSERT INTO [2AM].[dbo].[RuleExclusionSet]
           ([RuleExclusionSetKey]
           ,[Description]
           ,[Comment])
		VALUES (@RuleExclusionSetKey, 'AffordabilityAssessmentRelatedLegalEntity', 'Rules that must be excluded when saving related legal entities for an affordability assessment.')
	END
	ELSE
	BEGIN
		UPDATE [2AM].[dbo].[RuleExclusionSet]
		SET [Description] = 'AffordabilityAssessmentRelatedLegalEntity',
			[Comment] = 'Rules that must be excluded when saving related legal entities for an affordability assessment.'
		WHERE [RuleExclusionSetKey] = @RuleExclusionSetKey
	END

	DELETE FROM [2AM].[dbo].[RuleExclusion] WHERE [RuleExclusionSetKey] = @RuleExclusionSetKey 

	INSERT INTO [dbo].[RuleExclusion]
           ([RuleExclusionSetKey]
           ,[RuleItemKey])
	SELECT @RuleExclusionSetKey, 49763 -- LegalEntityNaturalPersonMandatoryGender
	UNION
	SELECT @RuleExclusionSetKey, 49764 -- LegalEntityNaturalPersonMandatoryMaritalStatus
	UNION
	SELECT @RuleExclusionSetKey, 49765 -- LegalEntityNaturalPersonMandatoryPopulationGroup
	UNION
	SELECT @RuleExclusionSetKey, 49766 -- LegalEntityNaturalPersonMandatoryEducation
	UNION
	SELECT @RuleExclusionSetKey, 49767 -- LegalEntityNaturalPersonMandatoryCitizenType
	UNION
	SELECT @RuleExclusionSetKey, 49768 -- LegalEntityNaturalPersonMandatoryIDNumber
	UNION
	SELECT @RuleExclusionSetKey, 49771 -- LegalEntityNaturalPersonMandatoryPassportNumber
	UNION
	SELECT @RuleExclusionSetKey, 49774 -- LegalEntityNaturalPersonMandatoryDateOfBirth
	UNION
	SELECT @RuleExclusionSetKey, 49775 -- LegalEntityNaturalPersonMandatoryHomeLanguage
	UNION
	SELECT @RuleExclusionSetKey, 49776 -- LegalEntityNaturalPersonMandatoryDocumentLanguage
	UNION
	SELECT @RuleExclusionSetKey, 49777 -- LegalEntityNaturalPersonMandatoryLegalEntityStatus

	COMMIT

END TRY

BEGIN CATCH

	IF @@TRANCOUNT > 0
		ROLLBACK TRAN

	--Raise an error with the details of the exception
	DECLARE @ErrMsg NVARCHAR(4000), @ErrSeverity INT

	SELECT @ErrMsg = ERROR_MESSAGE(), @ErrSeverity = ERROR_SEVERITY()

	RAISERROR(@ErrMsg, @ErrSeverity, 1)

END CATCH
GO
