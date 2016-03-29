USE [2AM]
GO
-- Find an existing index named IX_fin_FinancialTransaction_Description_IsRolledBack and delete it if found. 
IF EXISTS (SELECT name FROM sys.indexes
            WHERE name = N'IX_fin_FinancialTransaction_Description_IsRolledBack') 
    DROP INDEX IX_fin_FinancialTransaction_Description_IsRolledBack ON fin.FinancialTransaction; 
GO
-- Create a nonclustered index called IX_fin_FinancialTransaction_Description_IsRolledBack 
-- on the fin.FinancialTransaction table using the Reference & IsRolledBack columns. 
CREATE NONCLUSTERED INDEX IX_fin_FinancialTransaction_Description_IsRolledBack 
    ON fin.FinancialTransaction(Reference ASC, IsRolledBack ASC); 
GO