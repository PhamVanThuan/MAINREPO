
UPDATE [2am].[dbo].[InvoiceStatus]
   SET [InvoiceStatusKey] = 1
      ,[Description] = 'Received'
 WHERE  [InvoiceStatusKey] = 1
GO

UPDATE [2am].[dbo].[InvoiceStatus]
   SET [InvoiceStatusKey] = 2
      ,[Description] = 'Awaiting Approval'
 WHERE  [InvoiceStatusKey] = 2
GO

UPDATE [2am].[dbo].[InvoiceStatus]
   SET [InvoiceStatusKey] = 3
      ,[Description] = 'Approved'
 WHERE  [InvoiceStatusKey] = 3
GO

UPDATE [2am].[dbo].[InvoiceStatus]
   SET [InvoiceStatusKey] = 4
      ,[Description] = 'Processing Payment'
 WHERE  [InvoiceStatusKey] = 4
GO

UPDATE [2am].[dbo].[InvoiceStatus]
   SET [InvoiceStatusKey] = 5
      ,[Description] = 'Rejected'
 WHERE  [InvoiceStatusKey] = 5
GO

UPDATE [2am].[dbo].[InvoiceStatus]
   SET [InvoiceStatusKey] = 6
      ,[Description] = 'Paid'
 WHERE  [InvoiceStatusKey] = 6
GO