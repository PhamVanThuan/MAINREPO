USE [X2]
GO

-- update the new colum with correct value
update [x2].[x2].Process set ViewableOnUserInterfaceVersion = '2' where name not in ('Third Party Invoices') 
update [x2].[x2].Process set ViewableOnUserInterfaceVersion = '3' where name in ('Third Party Invoices') 