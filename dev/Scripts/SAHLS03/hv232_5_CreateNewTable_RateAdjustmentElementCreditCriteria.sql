use [2am]

if not exists (select 1 from INFORMATION_SCHEMA.TABLES where TABLE_TYPE='BASE TABLE' and TABLE_NAME='RateAdjustmentElementCreditCriteria')
begin
	create table dbo.RateAdjustmentElementCreditCriteria (
		RateAdjustmentElementKey int not null,
		CreditCriteriaKey int not null,
		constraint FK_RateAdjustmentElementCreditCriteria_CreditCriteriaKey foreign key (CreditCriteriaKey) references [2AM].dbo.CreditCriteria(CreditCriteriaKey),
		constraint FK_RateAdjustmentElementCreditCriteria_RateAdjustmentElementKey foreign key (RateAdjustmentElementKey) references [2AM].dbo.RateAdjustmentElement(RateAdjustmentElementKey)
	);
	
	create index FK_RateAdjustmentElementCreditCriteria_CreditCriteriaKey on [2AM].dbo.RateAdjustmentElementCreditCriteria (CreditCriteriaKey);
	
	create index FK_RateAdjustmentElementCreditCriteria_RateAdjustmentElementKey on [2AM].dbo.RateAdjustmentElementCreditCriteria (RateAdjustmentElementKey);
end


USE [2AM]
GO

ALTER AUTHORIZATION ON [dbo].[RateAdjustmentElementCreditCriteria] TO  SCHEMA OWNER 
GO
GRANT DELETE ON [dbo].[RateAdjustmentElementCreditCriteria] TO [AppRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[RateAdjustmentElementCreditCriteria] TO [AppRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[RateAdjustmentElementCreditCriteria] TO [AppRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[RateAdjustmentElementCreditCriteria] TO [AppRole] AS [dbo]
GO
GRANT DELETE ON [dbo].[RateAdjustmentElementCreditCriteria] TO [ITDeveloper] AS [dbo]
GO
GRANT INSERT ON [dbo].[RateAdjustmentElementCreditCriteria] TO [ITDeveloper] AS [dbo]
GO
GRANT REFERENCES ON [dbo].[RateAdjustmentElementCreditCriteria] TO [ITDeveloper] AS [dbo]
GO
GRANT SELECT ON [dbo].[RateAdjustmentElementCreditCriteria] TO [ITDeveloper] AS [dbo]
GO
GRANT UPDATE ON [dbo].[RateAdjustmentElementCreditCriteria] TO [ITDeveloper] AS [dbo]
GO
GRANT VIEW DEFINITION ON [dbo].[RateAdjustmentElementCreditCriteria] TO [ITDeveloper] AS [dbo]
GO
GRANT DELETE ON [dbo].[RateAdjustmentElementCreditCriteria] TO [ProcessRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[RateAdjustmentElementCreditCriteria] TO [ProcessRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[RateAdjustmentElementCreditCriteria] TO [ProcessRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[RateAdjustmentElementCreditCriteria] TO [ProcessRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[RateAdjustmentElementCreditCriteria] TO [public] AS [dbo]
GO
GRANT SELECT ON [dbo].[RateAdjustmentElementCreditCriteria] TO [sahlwebuser] AS [dbo]
GO
GRANT DELETE ON [dbo].[RateAdjustmentElementCreditCriteria] TO [user] AS [dbo]
GO
GRANT INSERT ON [dbo].[RateAdjustmentElementCreditCriteria] TO [user] AS [dbo]
GO
GRANT REFERENCES ON [dbo].[RateAdjustmentElementCreditCriteria] TO [user] AS [dbo]
GO
GRANT SELECT ON [dbo].[RateAdjustmentElementCreditCriteria] TO [user] AS [dbo]
GO
GRANT UPDATE ON [dbo].[RateAdjustmentElementCreditCriteria] TO [user] AS [dbo]
GO
GRANT VIEW DEFINITION ON [dbo].[RateAdjustmentElementCreditCriteria] TO [user] AS [dbo]
GO
