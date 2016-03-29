USE [2AM]
GO

BEGIN TRY

	DECLARE @RuleItemKey INT

	SET @RuleItemKey = 51585

	BEGIN TRAN

	IF EXISTS(SELECT 1 FROM [2AM].[dbo].[RuleItem] WHERE RuleItemKey = @RuleItemKey AND Name = 'ActiveSubsidyAndSalaryStopOrderConditionExists')
	BEGIN
		DELETE FROM [2AM].[dbo].[RuleItem] WHERE RuleItemKey = @RuleItemKey AND Name = 'ActiveSubsidyAndSalaryStopOrderConditionExists'
	END

	COMMIT

	BEGIN TRAN

	IF EXISTS(SELECT 1 FROM [2AM].[dbo].[RuleItem] WHERE RuleItemKey = @RuleItemKey AND Name != 'ActiveSubsidyAndSalaryStopOrderConditionExistsError')
	BEGIN
		RAISERROR('RuleItemKey = 51585 ALREADY EXISTS', 16, 1)
	END

	IF NOT EXISTS (SELECT 1 FROM [2AM].[dbo].[RuleItem] WHERE RuleItemKey = @RuleItemKey AND Name = 'ActiveSubsidyAndSalaryStopOrderConditionExistsError')
	BEGIN
		INSERT INTO RuleItem
		(RuleItemKey, Name, [Description], AssemblyName, TypeName, EnforceRule, GeneralStatusKey)
		VALUES (@RuleItemKey, 'ActiveSubsidyAndSalaryStopOrderConditionExistsError', 'Check if an active subsidy record exists for the account and then check for the existence of either the 222 or 223 condition linked to the accounts accepted offers.', 
		'SAHL.Rules.DLL', 'SAHL.Common.BusinessModel.Rules.Account.ActiveSubsidyAndSalaryStopOrderConditionExistsError', 1, 1)
	END
	ELSE
	BEGIN
		UPDATE RuleItem
		SET [Description] = 'Check if an active subsidy record exists for the account and then check for the existence of either the 222 or 223 condition linked to the accounts accepted offers.',
			AssemblyName = 'SAHL.Rules.DLL',
			TypeName = 'SAHL.Common.BusinessModel.Rules.Account.ActiveSubsidyAndSalaryStopOrderConditionExistsError',
			EnforceRule = 1,
			GeneralStatusKey = 1
		WHERE RuleItemKey = @RuleItemKey
		AND Name = 'ActiveSubsidyAndSalaryStopOrderConditionExistsError'
		
	END

	COMMIT

	SET @RuleItemKey = 51587

	BEGIN TRAN

	IF EXISTS(SELECT 1 FROM [2AM].[dbo].[RuleItem] WHERE RuleItemKey = @RuleItemKey AND Name != 'ActiveSubsidyAndSalaryStopOrderConditionExistsWarning')
	BEGIN
		RAISERROR('RuleItemKey = 51587 ALREADY EXISTS', 16, 1)
	END

	IF NOT EXISTS (SELECT 1 FROM [2AM].[dbo].[RuleItem] WHERE RuleItemKey = @RuleItemKey AND Name = 'ActiveSubsidyAndSalaryStopOrderConditionExistsWarning')
	BEGIN
		INSERT INTO RuleItem
		(RuleItemKey, Name, [Description], AssemblyName, TypeName, EnforceRule, GeneralStatusKey)
		VALUES (@RuleItemKey, 'ActiveSubsidyAndSalaryStopOrderConditionExistsWarning', 'Check if an active subsidy record exists for the account and then check for the existence of either the 222 or 223 condition linked to the accounts accepted offers.', 
		'SAHL.Rules.DLL', 'SAHL.Common.BusinessModel.Rules.Account.ActiveSubsidyAndSalaryStopOrderConditionExistsWarning', 0, 1)
	END
	ELSE
	BEGIN
		UPDATE RuleItem
		SET [Description] = 'Check if an active subsidy record exists for the account and then check for the existence of either the 222 or 223 condition linked to the accounts accepted offers.',
			AssemblyName = 'SAHL.Rules.DLL',
			TypeName = 'SAHL.Common.BusinessModel.Rules.Account.ActiveSubsidyAndSalaryStopOrderConditionExistsWarning',
			EnforceRule = 0,
			GeneralStatusKey = 1
		WHERE RuleItemKey = @RuleItemKey
		AND Name = 'ActiveSubsidyAndSalaryStopOrderConditionExistsWarning'
		
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
