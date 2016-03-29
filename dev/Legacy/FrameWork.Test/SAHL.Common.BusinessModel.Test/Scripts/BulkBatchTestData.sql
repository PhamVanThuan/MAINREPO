INSERT INTO [2AM].[dbo].[BulkBatch]
           ([BulkBatchStatusKey]
           ,[Description]
           ,[BulkBatchTypeKey]
           ,[IdentifierReferenceKey]
           ,[EffectiveDate]
           ,[StartDateTime]
           ,[CompletedDateTime]
           ,[FileName]
           ,[UserID]
           ,[ChangeDate])
     VALUES
           (1, 'test', 1, null, getdate(), null, null, null, null, null)

declare @BulkBatchKey int
set @BulkBatchKey = scope_identity()

INSERT INTO [2AM].[dbo].[BulkBatchLog]
           ([BulkBatchKey]
           ,[Description]
           ,[MessageTypeKey]
           ,[MessageReference]
           ,[MessageReferenceKey])
     VALUES
           (@BulkBatchKey, 'test', 1, null, null)

declare @BulkBatchLogKey int
set @BulkBatchLogKey = scope_identity()

INSERT INTO [2AM].[dbo].[BulkBatchParameter]
           ([BulkBatchKey]
           ,[ParameterName]
           ,[ParameterValue])
     VALUES
           (@BulkBatchKey, 'test', 'test')

declare @BulkBatchParameterKey int
set @BulkBatchParameterKey = scope_identity()

declare @AccountKey int
set @AccountKey = (select top 1 AccountKey from [2AM].[dbo].[Account])

INSERT INTO [2AM].[dbo].[BatchTransaction]
           ([BulkBatchKey]
           ,[AccountKey]
           ,[LegalEntityKey]
           ,[TransactionTypeNumber]
           ,[EffectiveDate]
           ,[Amount]
           ,[Reference]
           ,[UserID]
           ,[BatchTransactionStatusKey])
     VALUES
           (@BulkBatchKey
			, @AccountKey
           , null
           , 1
           , getdate()
           , 0
           , 'test'
           , null
           , 1)

declare @BatchTransactionKey int
set @BatchTransactionKey = scope_identity()

select @BulkBatchKey as BulkBatchKey
, @BulkBatchLogKey as BulkBatchLogKey
, @BulkBatchParameterKey as BulkBatchParameterKey
, @BatchTransactionKey as BatchTransactionKey