IF NOT EXISTS(SELECT * FROM [2AM].sys.columns 
            WHERE name = 'Processed' and
			 Object_ID = Object_ID(N'[2AM].[dbo].CATSPaymentBatchItem')) 
begin
ALTER TABLE [2AM].[dbo].[CATSPaymentBatchItem]
ADD  Processed bit DEFAULT 1
end
