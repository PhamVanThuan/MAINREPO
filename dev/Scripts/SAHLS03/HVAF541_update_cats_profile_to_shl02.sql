use [2AM]
go
IF OBJECT_ID(N'dbo.CATSPaymentBatchType', N'U') IS NOT NULL
BEGIN
    update [2AM].[dbo].[CATSPaymentBatchType] 
	set CATSProfile = 'SHL02', CATSFileNamePrefix = 'SHL02_Disbursement' where CATSPaymentBatchTypeKey = 1 
END
go