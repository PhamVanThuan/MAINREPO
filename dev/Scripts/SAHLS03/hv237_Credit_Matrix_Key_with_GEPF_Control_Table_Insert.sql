USE [2AM]
GO

BEGIN TRY

	BEGIN TRAN

	DECLARE @ControlDescription VARCHAR(50), @ControlNumeric int

	SET @ControlDescription = 'Credit matrix key with GEPF introduced'
	SET @ControlNumeric = 54

	IF NOT EXISTS(SELECT 1 FROM [2am].[dbo].[Control] WHERE ControlDescription = @ControlDescription)
	BEGIN
		INSERT INTO [2am].[dbo].[Control]
			(ControlDescription, 
			ControlNumeric, 
			ControlText, 
			ControlGroupKey)
		 VALUES
			   (@ControlDescription
			   ,@ControlNumeric
			   ,null
			   ,3)
	END
	ELSE
	BEGIN
		UPDATE [2am].[dbo].[Control]
		SET ControlNumeric = @ControlNumeric,
			ControlText = null,
			ControlGroupKey = 3
		WHERE ControlDescription = @ControlDescription
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

