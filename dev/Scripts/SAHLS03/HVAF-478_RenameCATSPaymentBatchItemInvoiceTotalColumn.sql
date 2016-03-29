 USE [2AM]
 GO
 IF EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'CATSPaymentBatchItem'
                 AND COLUMN_NAME = 'InvoiceTotal')
BEGIN
	IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'CATSPaymentBatchItem'
                 AND COLUMN_NAME = 'Amount')
	BEGIN
		EXEC sp_RENAME 'CATSPaymentBatchItem.InvoiceTotal' , 'Amount', 'COLUMN';
	END
END

