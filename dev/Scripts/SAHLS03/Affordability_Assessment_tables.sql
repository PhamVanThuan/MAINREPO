use [2AM]
go

-- lookups
if not exists (select * from sysobjects where name='AffordabilityAssessmentStatus' and xtype='U')
begin

	CREATE TABLE [dbo].[AffordabilityAssessmentStatus](
		[AffordabilityAssessmentStatusKey] [int] NOT NULL,
		[Description] varchar(50) NOT NULL,
	 CONSTRAINT [PK_AffordabilityAssessmentStatus] PRIMARY KEY CLUSTERED 
	(
		[AffordabilityAssessmentStatusKey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	insert into [dbo].[AffordabilityAssessmentStatus]
	select 1, 'Unconfirmed'
	union
	select 2, 'Confirmed'
end
go


if not exists (select * from sysobjects where name='AffordabilityAssessmentStressFactor' and xtype='U')
begin

	CREATE TABLE [dbo].[AffordabilityAssessmentStressFactor](
		[AffordabilityAssessmentStressFactorKey] [int] NOT NULL,
		[StressFactorPercentage] varchar(5) NOT NULL,
		[PercentageIncreaseOnRepayments] decimal(4, 2) NOT NULL,
	 CONSTRAINT [PK_AffordabilityAssessmentStressFactorKey] PRIMARY KEY CLUSTERED 
	(
		[AffordabilityAssessmentStressFactorKey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	insert into [dbo].[AffordabilityAssessmentStressFactor]
	select 1, '0%', 0
	union
	select 2, '0.5%', 0.04
	union
	select 3, '1.0%', 0.07
	union
	select 4, '2.0%', 0.14
end
go


if not exists (select * from sysobjects where name='AffordabilityAssessmentItemCategory' and xtype='U')
begin

	CREATE TABLE [dbo].[AffordabilityAssessmentItemCategory](
		[AffordabilityAssessmentItemCategoryKey] [int] NOT NULL,
		[Description] varchar(50) NOT NULL,
	 CONSTRAINT [PK_AffordabilityAssessmentItemCategory] PRIMARY KEY CLUSTERED 
	(
		[AffordabilityAssessmentItemCategoryKey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	insert into [dbo].[AffordabilityAssessmentItemCategory]
	select 1, 'Income'
	union
	select 2, 'Income Deductions'
	union
	select 3, 'Necessary Expenses'
	union
	select 4, 'Payment Obligations'
	union
	select 5, 'SAHL Payment Obligations'
	union
	select 6, 'Other Expenses'
end
go


if not exists (select * from sysobjects where name='AffordabilityAssessmentItemType' and xtype='U')
begin

	CREATE TABLE [dbo].[AffordabilityAssessmentItemType](
		[AffordabilityAssessmentItemTypeKey] [int] NOT NULL,
		[AffordabilityAssessmentItemCategoryKey] int not null,
		[Description] varchar(50) NOT NULL,
		ApplyStressFactor bit default(0) NOT NULL,
		Consolidatable bit default(0) NOT NULL,
	 CONSTRAINT [PK_AffordabilityAssessmentItemType] PRIMARY KEY CLUSTERED 
	(
		[AffordabilityAssessmentItemTypeKey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE dbo.AffordabilityAssessmentItemType ADD CONSTRAINT
		FK_AffordabilityAssessmentItemType_AffordabilityAssessmentItemCategory FOREIGN KEY
		(
		AffordabilityAssessmentItemCategoryKey
		) REFERENCES dbo.AffordabilityAssessmentItemCategory
		(
		AffordabilityAssessmentItemCategoryKey
		) ON UPDATE  NO ACTION
		 ON DELETE  NO ACTION
		 NOT FOR REPLICATION

	insert into [dbo].[AffordabilityAssessmentItemType]
	select 1, 1, 'Basic Gross Salary/Drawings', 0, 0
	union
	select 2, 1, 'Commission/Overtime', 0, 0
	union
	select 3, 1, 'Net Rental', 0, 0
	union
	select 4, 1, 'Investments', 0, 0
	union
	select 5, 1, 'Other Income 1', 0, 0
	union
	select 6, 1, 'Other Income 2', 0, 0
	union
	select 7, 2, 'Payroll Deductions', 0, 0
	union
	select 8, 3, 'Accommodation/Rental', 0, 0
	union
	select 9, 3, 'Transport', 0, 0
	union
	select 10, 3, 'Food', 0, 0
	union
	select 11, 3, 'Education', 0, 0
	union
	select 12, 3, 'Medical', 0, 0
	union
	select 13, 3, 'Utilities', 0, 0
	union
	select 14, 3, 'Child Support', 0, 0
	union
	select 15, 4, 'Other Bond/s', 1, 1
	union
	select 16, 4, 'Vehicle', 0, 1
	union
	select 17, 4, 'Credit Card/s', 0, 1
	union
	select 18, 4, 'Personal Loan/s', 0, 1
	union
	select 19, 4, 'Retail Accounts', 0, 1
	union
	select 20, 4, 'Other Debt Expenses', 0, 1
	union
	select 21, 5, 'SAHL Bond', 1, 0
	union
	select 22, 5, 'HOC', 0, 0
	union
	select 23, 6, 'Domestic Salary', 0, 0
	union
	select 24, 6, 'Insurance Policies', 0, 0
	union
	select 25, 6, 'Committed Savings', 0, 1
	union
	select 26, 6, 'Security', 0, 0
	union
	select 27, 6, 'Telephone/T.V', 0, 0
	union
	select 28, 6, 'Other', 0, 0
end
go


-- data tables
if not exists (select * from sysobjects where id = OBJECT_ID(N'dbo.AffordabilityAssessment') and xtype='U')
begin

	CREATE TABLE [dbo].[AffordabilityAssessment](
		[AffordabilityAssessmentKey] [int] IDENTITY(1,1) NOT NULL,
		GenericKey int not null,
		GenericKeyTypeKey int not null,
		AffordabilityAssessmentStatusKey int not null,
		GeneralStatusKey int not null,
		AffordabilityAssessmentStressFactorKey int not null,
		ModifiedDate datetime not null,
		ModifiedByUserId int not null,		
		NumberOfContributingApplicants int not null,
		NumberOfHouseholdDependants int not null,
		MinimumMonthlyFixedExpenses int,
		ConfirmedDate datetime null,
		Notes nvarchar(max) null,
	 CONSTRAINT [PK_AffordabilityAssessment] PRIMARY KEY CLUSTERED 
	(
		[AffordabilityAssessmentKey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE dbo.AffordabilityAssessment ADD CONSTRAINT
		FK_AffordabilityAssessment_AffordabilityAssessmentStatus FOREIGN KEY
		(
		AffordabilityAssessmentStatusKey
		) REFERENCES dbo.AffordabilityAssessmentStatus
		(
		AffordabilityAssessmentStatusKey
		) ON UPDATE  NO ACTION
		 ON DELETE  NO ACTION
		 NOT FOR REPLICATION

	ALTER TABLE dbo.AffordabilityAssessment ADD CONSTRAINT
		FK_AffordabilityAssessment_GeneralStatus FOREIGN KEY
		(
		GeneralStatusKey
		) REFERENCES dbo.GeneralStatus
		(
		GeneralStatusKey
		) ON UPDATE  NO ACTION
		 ON DELETE  NO ACTION
		 NOT FOR REPLICATION

	ALTER TABLE dbo.AffordabilityAssessment ADD CONSTRAINT
		FK_AffordabilityAssessment_AffordabilityAssessmentStressFactor FOREIGN KEY
		(
		AffordabilityAssessmentStressFactorKey
		) REFERENCES dbo.AffordabilityAssessmentStressFactor
		(
		AffordabilityAssessmentStressFactorKey
		) ON UPDATE  NO ACTION
		 ON DELETE  NO ACTION
		 NOT FOR REPLICATION

	ALTER TABLE dbo.AffordabilityAssessment ADD CONSTRAINT
		FK_AffordabilityAssessment_ADUser FOREIGN KEY
		(
		ModifiedByUserId
		) REFERENCES dbo.ADUser
		(
		ADUserKey
		) ON UPDATE  NO ACTION
		 ON DELETE  NO ACTION
		 NOT FOR REPLICATION

	ALTER TABLE dbo.AffordabilityAssessment ADD CONSTRAINT
		FK_AffordabilityAssessment_GenericKeyType FOREIGN KEY
		(
		GenericKeyTypeKey
		) REFERENCES dbo.GenericKeyType
		(
		GenericKeyTypeKey
		) ON UPDATE  NO ACTION
		 ON DELETE  NO ACTION
		 NOT FOR REPLICATION

end
go


if not exists (select * from sysobjects where id = OBJECT_ID(N'dbo.AffordabilityAssessmentItem') and xtype='U')
begin

	CREATE TABLE [dbo].[AffordabilityAssessmentItem](
		[AffordabilityAssessmentItemKey] [int] IDENTITY(1,1) NOT NULL,
		AffordabilityAssessmentKey int not null,
		AffordabilityAssessmentItemTypeKey int not null,
		ModifiedDate datetime not null,
		ModifiedByUserId int not null,
		ClientValue int,
		CreditValue int,
		DebtToConsolidateValue int,
		ItemNotes nvarchar(max),
	 CONSTRAINT [PK_AffordabilityAssessmentItem] PRIMARY KEY CLUSTERED 
	(
		[AffordabilityAssessmentItemKey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE dbo.AffordabilityAssessmentItem ADD CONSTRAINT
		FK_AffordabilityAssessmentItem_AffordabilityAssessment FOREIGN KEY
		(
		AffordabilityAssessmentKey
		) REFERENCES dbo.AffordabilityAssessment
		(
		AffordabilityAssessmentKey
		) ON UPDATE  NO ACTION
		 ON DELETE  NO ACTION
		 NOT FOR REPLICATION

	ALTER TABLE dbo.AffordabilityAssessmentItem ADD CONSTRAINT
		FK_AffordabilityAssessmentItem_AffordabilityAssessmentItemType FOREIGN KEY
		(
		AffordabilityAssessmentItemTypeKey
		) REFERENCES dbo.AffordabilityAssessmentItemType
		(
		AffordabilityAssessmentItemTypeKey
		) ON UPDATE  NO ACTION
		 ON DELETE  NO ACTION
		 NOT FOR REPLICATION

	ALTER TABLE dbo.AffordabilityAssessmentItem ADD CONSTRAINT
		FK_AffordabilityAssessmentItem_ADUser FOREIGN KEY
		(
		ModifiedByUserId
		) REFERENCES dbo.ADUser
		(
		ADUserKey
		) ON UPDATE  NO ACTION
		 ON DELETE  NO ACTION
		 NOT FOR REPLICATION
end
go


if not exists (select * from sysobjects where id = OBJECT_ID(N'dbo.AffordabilityAssessmentLegalEntity') and xtype='U')
begin

	CREATE TABLE [dbo].[AffordabilityAssessmentLegalEntity](
		[AffordabilityAssessmentLegalEntityKey] [int] IDENTITY(1,1) NOT NULL,
		AffordabilityAssessmentKey int NOT NULL,
		LegalEntityKey int NOT NULL,
	 CONSTRAINT [PK_AffordabilityAssessmentLegalEntity] PRIMARY KEY CLUSTERED 
	(
		[AffordabilityAssessmentLegalEntityKey] ASC
	),
	CONSTRAINT [UQ_AffordabilityAssessment_LegalEntity] UNIQUE NONCLUSTERED
	(
		AffordabilityAssessmentKey, LegalEntityKey
	)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE dbo.AffordabilityAssessmentLegalEntity ADD CONSTRAINT
		FK_AffordabilityAssessmentLegalEntity_AffordabilityAssessment FOREIGN KEY
		(
		AffordabilityAssessmentKey
		) REFERENCES dbo.AffordabilityAssessment
		(
		AffordabilityAssessmentKey
		) ON UPDATE  NO ACTION
		 ON DELETE  NO ACTION
		 NOT FOR REPLICATION

	ALTER TABLE dbo.AffordabilityAssessmentLegalEntity ADD CONSTRAINT
		FK_AffordabilityAssessmentLegalEntity_LegalEntity FOREIGN KEY
		(
		LegalEntityKey
		) REFERENCES dbo.LegalEntity
		(
		LegalEntityKey
		) ON UPDATE  NO ACTION
		 ON DELETE  NO ACTION
		 NOT FOR REPLICATION

end
go


GRANT DELETE ON [dbo].[AffordabilityAssessmentStatus] TO [AppRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessmentStatus] TO [AppRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessmentStatus] TO [AppRole] AS [dbo]
GO
GRANT DELETE ON [dbo].[AffordabilityAssessmentStatus] TO [ProcessRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessmentStatus] TO [ProcessRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessmentStatus] TO [ProcessRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[AffordabilityAssessmentStatus] TO [public] AS [dbo]
GO
GRANT DELETE ON [dbo].[AffordabilityAssessmentStatus] TO [ServiceArchitect] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessmentStatus] TO [ServiceArchitect] AS [dbo]
GO
GRANT REFERENCES ON [dbo].[AffordabilityAssessmentStatus] TO [ServiceArchitect] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessmentStatus] TO [ServiceArchitect] AS [dbo]
GO


GRANT DELETE ON [dbo].[AffordabilityAssessmentItemCategory] TO [AppRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessmentItemCategory] TO [AppRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessmentItemCategory] TO [AppRole] AS [dbo]
GO
GRANT DELETE ON [dbo].[AffordabilityAssessmentItemCategory] TO [ProcessRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessmentItemCategory] TO [ProcessRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessmentItemCategory] TO [ProcessRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[AffordabilityAssessmentItemCategory] TO [public] AS [dbo]
GO
GRANT DELETE ON [dbo].[AffordabilityAssessmentItemCategory] TO [ServiceArchitect] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessmentItemCategory] TO [ServiceArchitect] AS [dbo]
GO
GRANT REFERENCES ON [dbo].[AffordabilityAssessmentItemCategory] TO [ServiceArchitect] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessmentItemCategory] TO [ServiceArchitect] AS [dbo]
GO


GRANT DELETE ON [dbo].[AffordabilityAssessmentItemType] TO [AppRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessmentItemType] TO [AppRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessmentItemType] TO [AppRole] AS [dbo]
GO
GRANT DELETE ON [dbo].[AffordabilityAssessmentItemType] TO [ProcessRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessmentItemType] TO [ProcessRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessmentItemType] TO [ProcessRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[AffordabilityAssessmentItemType] TO [public] AS [dbo]
GO
GRANT DELETE ON [dbo].[AffordabilityAssessmentItemType] TO [ServiceArchitect] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessmentItemType] TO [ServiceArchitect] AS [dbo]
GO
GRANT REFERENCES ON [dbo].[AffordabilityAssessmentItemType] TO [ServiceArchitect] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessmentItemType] TO [ServiceArchitect] AS [dbo]
GO


GRANT DELETE ON [dbo].[AffordabilityAssessmentStressFactor] TO [AppRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessmentStressFactor] TO [AppRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessmentStressFactor] TO [AppRole] AS [dbo]
GO
GRANT DELETE ON [dbo].[AffordabilityAssessmentStressFactor] TO [ProcessRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessmentStressFactor] TO [ProcessRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessmentStressFactor] TO [ProcessRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[AffordabilityAssessmentStressFactor] TO [public] AS [dbo]
GO
GRANT DELETE ON [dbo].[AffordabilityAssessmentStressFactor] TO [ServiceArchitect] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessmentStressFactor] TO [ServiceArchitect] AS [dbo]
GO
GRANT REFERENCES ON [dbo].[AffordabilityAssessmentStressFactor] TO [ServiceArchitect] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessmentStressFactor] TO [ServiceArchitect] AS [dbo]
GO


GRANT DELETE ON [dbo].[AffordabilityAssessment] TO [AppRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessment] TO [AppRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessment] TO [AppRole] AS [dbo]
GO
GRANT DELETE ON [dbo].[AffordabilityAssessment] TO [ProcessRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessment] TO [ProcessRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessment] TO [ProcessRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[AffordabilityAssessment] TO [public] AS [dbo]
GO
GRANT DELETE ON [dbo].[AffordabilityAssessment] TO [ServiceArchitect] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessment] TO [ServiceArchitect] AS [dbo]
GO
GRANT REFERENCES ON [dbo].[AffordabilityAssessment] TO [ServiceArchitect] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessment] TO [ServiceArchitect] AS [dbo]
GO


GRANT DELETE ON [dbo].[AffordabilityAssessmentLegalEntity] TO [AppRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessmentLegalEntity] TO [AppRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessmentLegalEntity] TO [AppRole] AS [dbo]
GO
GRANT DELETE ON [dbo].[AffordabilityAssessmentLegalEntity] TO [ProcessRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessmentLegalEntity] TO [ProcessRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessmentLegalEntity] TO [ProcessRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[AffordabilityAssessmentLegalEntity] TO [public] AS [dbo]
GO
GRANT DELETE ON [dbo].[AffordabilityAssessmentLegalEntity] TO [ServiceArchitect] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessmentLegalEntity] TO [ServiceArchitect] AS [dbo]
GO
GRANT REFERENCES ON [dbo].[AffordabilityAssessmentLegalEntity] TO [ServiceArchitect] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessmentLegalEntity] TO [ServiceArchitect] AS [dbo]
GO


GRANT DELETE ON [dbo].[AffordabilityAssessmentItem] TO [AppRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessmentItem] TO [AppRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessmentItem] TO [AppRole] AS [dbo]
GO
GRANT DELETE ON [dbo].[AffordabilityAssessmentItem] TO [ProcessRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessmentItem] TO [ProcessRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessmentItem] TO [ProcessRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[AffordabilityAssessmentItem] TO [public] AS [dbo]
GO
GRANT DELETE ON [dbo].[AffordabilityAssessmentItem] TO [ServiceArchitect] AS [dbo]
GO
GRANT INSERT ON [dbo].[AffordabilityAssessmentItem] TO [ServiceArchitect] AS [dbo]
GO
GRANT REFERENCES ON [dbo].[AffordabilityAssessmentItem] TO [ServiceArchitect] AS [dbo]
GO
GRANT UPDATE ON [dbo].[AffordabilityAssessmentItem] TO [ServiceArchitect] AS [dbo]
GO


-- audit tables
if not exists (select * from sysobjects where id = OBJECT_ID(N'dbo.AuditAffordabilityAssessment') and xtype='U')
begin

	CREATE TABLE dbo.AuditAffordabilityAssessment(
		[AuditNumber] [numeric](18, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[AuditLogin] [varchar](50) NOT NULL CONSTRAINT [DF_AuditAffordabilityAssessment1]  DEFAULT (user_name()),
		[AuditHostName] [varchar](50) NOT NULL CONSTRAINT [DF_AuditAffordabilityAssessment2]  DEFAULT (host_name()),
		[AuditProgramName] [varchar](100) NULL CONSTRAINT [DF_AuditAffordabilityAssessment3]  DEFAULT (app_name()),
		[AuditDate] [datetime] NOT NULL CONSTRAINT [DF_AuditAffordabilityAssessment4]  DEFAULT (getdate()),
		[AuditAddUpdateDelete] [char](1) NOT NULL,
		AffordabilityAssessmentKey int not null,
		GenericKey int not null,
		GenericKeyTypeKey int not null,
		AffordabilityAssessmentStatusKey int not null,
		GeneralStatusKey int not null,
		AffordabilityAssessmentStressFactorKey int not null,
		ModifiedDate datetime not null,
		ModifiedByUserId int not null,		
		NumberOfContributingApplicants int not null,
		NumberOfHouseholdDependants int not null,
		MinimumMonthlyFixedExpenses int,
		ConfirmedDate datetime null,
		Notes nvarchar(max) null,
 CONSTRAINT [PK_AuditAffordabilityAssessment] PRIMARY KEY CLUSTERED 
(
	[AuditNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

end
go


if not exists (select * from sysobjects where id = OBJECT_ID(N'dbo.AuditAffordabilityAssessmentItem') and xtype='U')
begin

	CREATE TABLE dbo.AuditAffordabilityAssessmentItem(
		[AuditNumber] [numeric](18, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[AuditLogin] [varchar](50) NOT NULL CONSTRAINT [DF_AuditAffordabilityAssessmentItem1]  DEFAULT (user_name()),
		[AuditHostName] [varchar](50) NOT NULL CONSTRAINT [DF_AuditAffordabilityAssessmentItem2]  DEFAULT (host_name()),
		[AuditProgramName] [varchar](100) NULL CONSTRAINT [DF_AuditAffordabilityAssessmentItem3]  DEFAULT (app_name()),
		[AuditDate] [datetime] NOT NULL CONSTRAINT [DF_AuditAffordabilityAssessmentItem4]  DEFAULT (getdate()),
		[AuditAddUpdateDelete] [char](1) NOT NULL,
		[AffordabilityAssessmentItemKey] [int] NOT NULL,
		AffordabilityAssessmentKey int not null,
		AffordabilityAssessmentItemTypeKey int not null,
		ModifiedDate datetime not null,
		ModifiedByUserId int not null,
		ClientValue int,
		CreditValue int,
		DebtToConsolidateValue int,
		ItemNotes nvarchar(max),
 CONSTRAINT [PK_AuditAffordabilityAssessmentItem] PRIMARY KEY CLUSTERED 
(
	[AuditNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

end
go


if not exists (select * from sysobjects where id = OBJECT_ID(N'dbo.AuditAffordabilityAssessmentLegalEntity') and xtype='U')
begin

	CREATE TABLE dbo.AuditAffordabilityAssessmentLegalEntity(
		[AuditNumber] [numeric](18, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[AuditLogin] [varchar](50) NOT NULL CONSTRAINT [DF_AuditAffordabilityAssessmentLegalEntity1]  DEFAULT (user_name()),
		[AuditHostName] [varchar](50) NOT NULL CONSTRAINT [DF_AuditAffordabilityAssessmentLegalEntity2]  DEFAULT (host_name()),
		[AuditProgramName] [varchar](100) NULL CONSTRAINT [DF_AuditAffordabilityAssessmentLegalEntity3]  DEFAULT (app_name()),
		[AuditDate] [datetime] NOT NULL CONSTRAINT [DF_AuditAffordabilityAssessmentLegalEntity4]  DEFAULT (getdate()),
		[AuditAddUpdateDelete] [char](1) NOT NULL,
		[AffordabilityAssessmentLegalEntityKey] [int] NOT NULL,
		AffordabilityAssessmentKey int NOT NULL,
		LegalEntityKey int NOT NULL,
 CONSTRAINT [PK_AuditAffordabilityAssessmentLegalEntity] PRIMARY KEY CLUSTERED 
(
	[AuditNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

end
go


-- audit triggers
if exists (select * from sysobjects where [type] = 'TR' AND [name] = 'tu_AffordabilityAssessment')
begin
	DROP TRIGGER [dbo].[tu_AffordabilityAssessment];
end
go

CREATE TRIGGER [dbo].[tu_AffordabilityAssessment] ON [dbo].[AffordabilityAssessment]
FOR UPDATE 
AS

SET NOCOUNT ON;

BEGIN

INSERT INTO [dbo].[AuditAffordabilityAssessment]
			([AuditAddUpdateDelete]
			,[AffordabilityAssessmentKey]
			,[GenericKey]
			,[GenericKeyTypeKey]
			,[AffordabilityAssessmentStatusKey]
			,[GeneralStatusKey]
			,[AffordabilityAssessmentStressFactorKey]
			,[ModifiedDate]
			,[ModifiedByUserId]
			,[NumberOfContributingApplicants]
			,[NumberOfHouseholdDependants]
			,[MinimumMonthlyFixedExpenses]
			,[ConfirmedDate]
			,[Notes])
SELECT
		'U' as AuditAddUpdateDelete
		,[AffordabilityAssessmentKey]
		,[GenericKey]
		,[GenericKeyTypeKey]
		,[AffordabilityAssessmentStatusKey]
		,[GeneralStatusKey]
		,[AffordabilityAssessmentStressFactorKey]
		,[ModifiedDate]
		,[ModifiedByUserId]
		,[NumberOfContributingApplicants]
		,[NumberOfHouseholdDependants]
		,[MinimumMonthlyFixedExpenses]
		,[ConfirmedDate]
		,[Notes]
FROM
	INSERTED;

SET NOCOUNT OFF;

END
GO

if exists (select * from sysobjects where [type] = 'TR' AND [name] = 'ti_AffordabilityAssessment')
begin
	DROP TRIGGER [dbo].[ti_AffordabilityAssessment];
end
go

CREATE TRIGGER [dbo].[ti_AffordabilityAssessment] ON [dbo].[AffordabilityAssessment]
FOR INSERT 
AS

SET NOCOUNT ON;

BEGIN

INSERT INTO [dbo].[AuditAffordabilityAssessment]
			([AuditAddUpdateDelete]
			,[AffordabilityAssessmentKey]
			,[GenericKey]
			,[GenericKeyTypeKey]
			,[AffordabilityAssessmentStatusKey]
			,[GeneralStatusKey]
			,[AffordabilityAssessmentStressFactorKey]
			,[ModifiedDate]
			,[ModifiedByUserId]
			,[NumberOfContributingApplicants]
			,[NumberOfHouseholdDependants]
			,[MinimumMonthlyFixedExpenses]
			,[ConfirmedDate]
			,[Notes])
SELECT
		'I' as AuditAddUpdateDelete
		,[AffordabilityAssessmentKey]
		,[GenericKey]
		,[GenericKeyTypeKey]
		,[AffordabilityAssessmentStatusKey]
		,[GeneralStatusKey]
		,[AffordabilityAssessmentStressFactorKey]
		,[ModifiedDate]
		,[ModifiedByUserId]
		,[NumberOfContributingApplicants]
		,[NumberOfHouseholdDependants]
		,[MinimumMonthlyFixedExpenses]
		,[ConfirmedDate]
		,[Notes]
FROM
	INSERTED;

SET NOCOUNT OFF;

END
GO

if exists (select * from sysobjects where [type] = 'TR' AND [name] = 'td_AffordabilityAssessment')
begin
	DROP TRIGGER [dbo].[td_AffordabilityAssessment];
end
go

CREATE TRIGGER [dbo].[td_AffordabilityAssessment] ON [dbo].[AffordabilityAssessment]
FOR DELETE 
AS

SET NOCOUNT ON;

BEGIN

INSERT INTO [dbo].[AuditAffordabilityAssessment]
			([AuditAddUpdateDelete]
			,[AffordabilityAssessmentKey]
			,[GenericKey]
			,[GenericKeyTypeKey]
			,[AffordabilityAssessmentStatusKey]
			,[GeneralStatusKey]
			,[AffordabilityAssessmentStressFactorKey]
			,[ModifiedDate]
			,[ModifiedByUserId]
			,[NumberOfContributingApplicants]
			,[NumberOfHouseholdDependants]
			,[MinimumMonthlyFixedExpenses]
			,[ConfirmedDate]
			,[Notes])
SELECT
		'D' as AuditAddUpdateDelete
		,[AffordabilityAssessmentKey]
		,[GenericKey]
		,[GenericKeyTypeKey]
		,[AffordabilityAssessmentStatusKey]
		,[GeneralStatusKey]
		,[AffordabilityAssessmentStressFactorKey]
		,[ModifiedDate]
		,[ModifiedByUserId]
		,[NumberOfContributingApplicants]
		,[NumberOfHouseholdDependants]
		,[MinimumMonthlyFixedExpenses]
		,[ConfirmedDate]
		,[Notes]
FROM
	DELETED;

SET NOCOUNT OFF;

END
GO


if exists (select * from sysobjects where [type] = 'TR' AND [name] = 'tu_AffordabilityAssessmentItem')
begin
	DROP TRIGGER [dbo].[tu_AffordabilityAssessmentItem];
end
go

CREATE TRIGGER [dbo].[tu_AffordabilityAssessmentItem] ON [dbo].[AffordabilityAssessmentItem]
FOR UPDATE 
AS

SET NOCOUNT ON;

BEGIN

INSERT INTO [dbo].[AuditAffordabilityAssessmentItem]
			([AuditAddUpdateDelete]
			,[AffordabilityAssessmentItemKey]
			,[AffordabilityAssessmentKey]
			,[AffordabilityAssessmentItemTypeKey]
			,[ModifiedDate]
			,[ModifiedByUserId]
			,[ClientValue]
			,[CreditValue]
			,[DebtToConsolidateValue]
			,[ItemNotes])
SELECT
		'U' as AuditAddUpdateDelete
		,[AffordabilityAssessmentItemKey]
		,[AffordabilityAssessmentKey]
		,[AffordabilityAssessmentItemTypeKey]
		,[ModifiedDate]
		,[ModifiedByUserId]
		,[ClientValue]
		,[CreditValue]
		,[DebtToConsolidateValue]
		,[ItemNotes]
FROM
	INSERTED;

SET NOCOUNT OFF;

END
GO

if exists (select * from sysobjects where [type] = 'TR' AND [name] = 'ti_AffordabilityAssessmentItem')
begin
	DROP TRIGGER [dbo].[ti_AffordabilityAssessmentItem];
end
go

CREATE TRIGGER [dbo].[ti_AffordabilityAssessmentItem] ON [dbo].[AffordabilityAssessmentItem]
FOR INSERT 
AS

SET NOCOUNT ON;

BEGIN

INSERT INTO [dbo].[AuditAffordabilityAssessmentItem]
			([AuditAddUpdateDelete]
			,[AffordabilityAssessmentItemKey]
			,[AffordabilityAssessmentKey]
			,[AffordabilityAssessmentItemTypeKey]
			,[ModifiedDate]
			,[ModifiedByUserId]
			,[ClientValue]
			,[CreditValue]
			,[DebtToConsolidateValue]
			,[ItemNotes])
SELECT
		'I' as AuditAddUpdateDelete
		,[AffordabilityAssessmentItemKey]
		,[AffordabilityAssessmentKey]
		,[AffordabilityAssessmentItemTypeKey]
		,[ModifiedDate]
		,[ModifiedByUserId]
		,[ClientValue]
		,[CreditValue]
		,[DebtToConsolidateValue]
		,[ItemNotes]
FROM
	INSERTED;

SET NOCOUNT OFF;

END
GO

if exists (select * from sysobjects where [type] = 'TR' AND [name] = 'td_AffordabilityAssessmentItem')
begin
	DROP TRIGGER [dbo].[td_AffordabilityAssessmentItem];
end
go

CREATE TRIGGER [dbo].[td_AffordabilityAssessmentItem] ON [dbo].[AffordabilityAssessmentItem]
FOR DELETE 
AS

SET NOCOUNT ON;

BEGIN

INSERT INTO [dbo].[AuditAffordabilityAssessmentItem]
			([AuditAddUpdateDelete]
			,[AffordabilityAssessmentItemKey]
			,[AffordabilityAssessmentKey]
			,[AffordabilityAssessmentItemTypeKey]
			,[ModifiedDate]
			,[ModifiedByUserId]
			,[ClientValue]
			,[CreditValue]
			,[DebtToConsolidateValue]
			,[ItemNotes])
	SELECT
		'D' as AuditAddUpdateDelete
		,[AffordabilityAssessmentItemKey]
		,[AffordabilityAssessmentKey]
		,[AffordabilityAssessmentItemTypeKey]
		,[ModifiedDate]
		,[ModifiedByUserId]
		,[ClientValue]
		,[CreditValue]
		,[DebtToConsolidateValue]
		,[ItemNotes]
FROM
	DELETED;

SET NOCOUNT OFF;

END
GO

if exists (select * from sysobjects where [type] = 'TR' AND [name] = 'tu_AffordabilityAssessmentLegalEntity')
begin
	DROP TRIGGER [dbo].[tu_AffordabilityAssessmentLegalEntity];
end
go

CREATE TRIGGER [dbo].[tu_AffordabilityAssessmentLegalEntity] ON [dbo].[AffordabilityAssessmentLegalEntity]
FOR UPDATE 
AS

SET NOCOUNT ON;

BEGIN

INSERT INTO [dbo].[AuditAffordabilityAssessmentLegalEntity]
			([AuditAddUpdateDelete]
			,[AffordabilityAssessmentLegalEntityKey]
			,[AffordabilityAssessmentKey]
			,[LegalEntityKey])
SELECT
		'U' as AuditAddUpdateDelete
		,[AffordabilityAssessmentLegalEntityKey]
		,[AffordabilityAssessmentKey]
		,[LegalEntityKey]
FROM
	INSERTED;

SET NOCOUNT OFF;

END
GO

if exists (select * from sysobjects where [type] = 'TR' AND [name] = 'ti_AffordabilityAssessmentLegalEntity')
begin
	DROP TRIGGER [dbo].[ti_AffordabilityAssessmentLegalEntity];
end
go

CREATE TRIGGER [dbo].[ti_AffordabilityAssessmentLegalEntity] ON [dbo].[AffordabilityAssessmentLegalEntity]
FOR INSERT 
AS

SET NOCOUNT ON;

BEGIN

INSERT INTO [dbo].[AuditAffordabilityAssessmentLegalEntity]
			([AuditAddUpdateDelete]
			,[AffordabilityAssessmentLegalEntityKey]
			,[AffordabilityAssessmentKey]
			,[LegalEntityKey])
SELECT
		'I' as AuditAddUpdateDelete
		,[AffordabilityAssessmentLegalEntityKey]
		,[AffordabilityAssessmentKey]
		,[LegalEntityKey]
FROM
	INSERTED;

SET NOCOUNT OFF;

END
GO

if exists (select * from sysobjects where [type] = 'TR' AND [name] = 'td_AffordabilityAssessmentLegalEntity')
begin
	DROP TRIGGER [dbo].[td_AffordabilityAssessmentLegalEntity];
end
go

CREATE TRIGGER [dbo].[td_AffordabilityAssessmentLegalEntity] ON [dbo].[AffordabilityAssessmentLegalEntity]
FOR DELETE 
AS

SET NOCOUNT ON;

BEGIN

INSERT INTO [dbo].[AuditAffordabilityAssessmentLegalEntity]
			([AuditAddUpdateDelete]
			,[AffordabilityAssessmentLegalEntityKey]
			,[AffordabilityAssessmentKey]
			,[LegalEntityKey])
SELECT
		'D' as AuditAddUpdateDelete
		,[AffordabilityAssessmentLegalEntityKey]
		,[AffordabilityAssessmentKey]
		,[LegalEntityKey]
FROM
	DELETED;

SET NOCOUNT OFF;

END
GO