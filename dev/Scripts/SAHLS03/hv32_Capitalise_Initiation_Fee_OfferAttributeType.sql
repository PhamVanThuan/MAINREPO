USE [2AM]
GO

BEGIN TRY

	BEGIN TRAN

	DECLARE @OfferAttributeTypeKey INT, @OfferAttributeTypeDescription VARCHAR(50), @OfferAttributeTypeGroupKey INT

	SET @OfferAttributeTypeKey = 35
	SET @OfferAttributeTypeDescription = 'Capitalise Initiation Fee'

	SET @OfferAttributeTypeGroupKey = (SELECT [OfferAttributeTypeGroupKey] FROM [dbo].[OfferAttributeTypeGroup] WHERE [Description] = 'Alpha Housing')

	IF EXISTS(SELECT 1 FROM [2AM].[dbo].[OfferAttributeType] WHERE OfferAttributeTypeKey = @OfferAttributeTypeKey AND [Description] != @OfferAttributeTypeDescription)
	BEGIN
		RAISERROR('OfferAttributeTypeKey = 35 ALREADY EXISTS',16,1)
	END

	IF NOT EXISTS(SELECT 1 FROM [2AM].[dbo].[OfferAttributeType] WHERE OfferAttributeTypeKey = @OfferAttributeTypeKey AND [Description] = @OfferAttributeTypeDescription)
	BEGIN
		INSERT INTO [dbo].[OfferAttributeType]
				   ([OfferAttributeTypeKey]
				   ,[Description]
				   ,[ISGeneric]
				   ,[OfferAttributeTypeGroupKey]
				   ,[UserEditable])
		 VALUES
			   (@OfferAttributeTypeKey
			   ,@OfferAttributeTypeDescription
			   ,0
			   ,@OfferAttributeTypeGroupKey
			   ,0)
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