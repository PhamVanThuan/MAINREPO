IF NOT EXISTS (SELECT * FROM [2AM].[dbo].[GenericKeyType] WHERE [GenericKeyTypeKey] = 60)
BEGIN
INSERT INTO [2AM].[dbo].[GenericKeyType]
           ([GenericKeyTypeKey]
           ,[Description]
           ,[TableName]
           ,[PrimaryKeyColumn])
     VALUES
           (60
           ,'CATSPaymentBatch'
           ,'[2AM].[dbo].[CATSPaymentBatch]'
           ,'CATSPaymentBatchKey')
end
GO


