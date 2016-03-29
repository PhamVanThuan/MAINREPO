USE [2AM]
GO

BEGIN TRY

	DECLARE @RuleItemKey INT

	SET @RuleItemKey = 51589

	BEGIN TRAN

	IF EXISTS(SELECT 1 FROM [2AM].[dbo].[RuleItem] WHERE RuleItemKey = @RuleItemKey AND Name != 'CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck')
	BEGIN
		RAISERROR('RuleItemKey = 51589 ALREADY EXISTS', 16, 1)
	END

	IF NOT EXISTS (SELECT 1 FROM [2AM].[dbo].[RuleItem] WHERE RuleItemKey = @RuleItemKey AND Name = 'CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck')
	BEGIN
		INSERT INTO RuleItem
		(RuleItemKey, Name, [Description], AssemblyName, TypeName, EnforceRule, GeneralStatusKey)
		VALUES (@RuleItemKey, 'CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck', 'This client and\or a related legalentity are linked to an affordability assessment.', 
		'SAHL.Rules.DLL', 'SAHL.Common.BusinessModel.Rules.AffordabilityAssessment.CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck', 1, 1)
	END
	ELSE
	BEGIN
		UPDATE RuleItem
		SET [Description] = 'This client and\or a related legalentity are linked to an affordability assessment.',
			AssemblyName = 'SAHL.Rules.DLL',
			TypeName = 'SAHL.Common.BusinessModel.Rules.AffordabilityAssessment.CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck',
			EnforceRule = 1,
			GeneralStatusKey = 1
		WHERE RuleItemKey = @RuleItemKey
		AND Name = 'CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck'
		
	END

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
