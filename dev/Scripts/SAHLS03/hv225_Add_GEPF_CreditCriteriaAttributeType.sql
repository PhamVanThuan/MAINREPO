USE [2AM]
GO


SET IDENTITY_INSERT [dbo].[CreditCriteriaAttributeType] ON;

BEGIN TRY

	BEGIN TRAN

	DECLARE @CreditCriteriaAttributeTypeKey INT, @CreditCriteriaAttributeTypeDescription VARCHAR(50)

	SET @CreditCriteriaAttributeTypeKey = 4
	SET @CreditCriteriaAttributeTypeDescription = 'Government Employee Pension Fund'

	IF EXISTS(SELECT 1 FROM [2AM].[dbo].[CreditCriteriaAttributeType] WHERE CreditCriteriaAttributeTypeKey = @CreditCriteriaAttributeTypeKey AND [Description] != @CreditCriteriaAttributeTypeDescription)
	BEGIN
		RAISERROR('CreditCriteriaAttributeTypeKey = 4 ALREADY EXISTS',16,1)
	END

	IF NOT EXISTS(SELECT 1 FROM [2AM].[dbo].[CreditCriteriaAttributeType] WHERE CreditCriteriaAttributeTypeKey = @CreditCriteriaAttributeTypeKey AND [Description] = @CreditCriteriaAttributeTypeDescription)
	BEGIN
		INSERT INTO [dbo].[CreditCriteriaAttributeType]
				   (CreditCriteriaAttributeTypeKey
				   ,[Description])
		 VALUES
			   (@CreditCriteriaAttributeTypeKey
			   ,@CreditCriteriaAttributeTypeDescription)
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

SET IDENTITY_INSERT [dbo].[CreditCriteriaAttributeType] OFF;
