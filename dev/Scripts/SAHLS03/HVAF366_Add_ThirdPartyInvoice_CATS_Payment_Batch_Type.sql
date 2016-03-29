USE [2AM]
GO
if not exists( select * from [dbo].[CATSPaymentBatchType] where  [Description] = 'ThirdPartyInvoice')
begin
INSERT INTO [dbo].[CATSPaymentBatchType]
           ([Description]
           ,[CATSProfile]
		   ,[CATSFileNamePrefix]
           ,[CATSEnvironment]
		   ,[NextCATSFileSequenceNo])
     VALUES
           ('ThirdPartyInvoice'
           ,'SHL04'
           ,'SHL04_Disbursement'
		   ,1
		   ,0)
end
GO