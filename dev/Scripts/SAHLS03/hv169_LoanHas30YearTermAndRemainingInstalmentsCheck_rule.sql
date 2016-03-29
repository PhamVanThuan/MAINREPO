USE [2AM]
GO

BEGIN TRY

	BEGIN TRAN

		DECLARE @RuleItemKey INT, @RuleItemName varchar(255)

		SET @RuleItemKey = 51590
		SET @RuleItemName = 'LoanHas30YearTermAndRemainingInstalmentsCheck'

		IF NOT EXISTS(SELECT 1 FROM [2AM].[dbo].[RuleItem] WHERE RuleItemKey = @RuleItemKey)
		BEGIN
		INSERT INTO [2AM].[dbo].[RuleItem]
				([RuleItemKey]
				,[Name]
				,[Description]
				,[AssemblyName]
				,[TypeName]
				,[EnforceRule]
				,[GeneralStatusKey]
				,[GeneralStatusReasonDescription])
		 VALUES
			   (@RuleItemKey
			   ,@RuleItemName
			   ,'Caution: loan approved on 30 year term.'
			   ,'SAHL.Rules.DLL'
			   ,'SAHL.Common.BusinessModel.Rules.Application.FurtherLending.LoanHas30YearTermAndRemainingInstalmentsCheck'
			   ,0
			   ,1
			   ,NULL)
		END
		ELSE
		BEGIN
			UPDATE [2AM].[dbo].[RuleItem]
			SET [Name] = @RuleItemName,
				[Description] = 'Caution: loan approved on 30 year term.',
				[AssemblyName] = 'SAHL.Rules.DLL',
				[TypeName] = 'SAHL.Common.BusinessModel.Rules.Application.FurtherLending.LoanHas30YearTermAndRemainingInstalmentsCheck',
				[EnforceRule] = 0,
				[GeneralStatusKey] = 1,
				[GeneralStatusReasonDescription] = NULL
			WHERE RuleItemKey = @RuleItemKey 
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
