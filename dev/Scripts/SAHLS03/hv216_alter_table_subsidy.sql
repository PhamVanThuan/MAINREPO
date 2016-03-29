use [2am]
go

if not exists (select * from sys.columns WHERE Name = N'GEPFMember' and Object_ID = Object_ID(N'Subsidy'))
begin
	alter table [2am].[dbo].Subsidy add GEPFMember BIT NOT NULL DEFAULT 0;
end
go

if not exists (select * from sys.columns WHERE Name = N'GEPFMember' and Object_ID = Object_ID(N'AuditSubsidy'))
begin
	alter table [2am].[dbo].AuditSubsidy add GEPFMember BIT NOT NULL DEFAULT 0;
end
go


ALTER TRIGGER [dbo].[td_Subsidy] ON [dbo].[Subsidy]
FOR DELETE 
AS

SET NOCOUNT ON 


BEGIN
INSERT INTO dbo.AuditSubsidy (
	AuditAddUpdateDelete,
	SubsidyKey,
	SubsidyProviderKey,
	EmploymentKey,
	LegalEntityKey,
	SalaryNumber,
	Paypoint,
	Notch,
	Rank,
	GeneralStatusKey,
	StopOrderAmount,
	GEPFMember
)
SELECT
	'D' as AuditAddUpdateDelete,
	SubsidyKey,
	SubsidyProviderKey,
	EmploymentKey,
	LegalEntityKey,
	SalaryNumber,
	Paypoint,
	Notch,
	Rank,
	GeneralStatusKey,
	StopOrderAmount,
	GEPFMember
FROM
	DELETED;

SET NOCOUNT OFF 

END;
GO

ALTER TRIGGER [dbo].[ti_Subsidy] ON [dbo].[Subsidy]
FOR INSERT 
AS

SET NOCOUNT ON 


BEGIN
INSERT INTO dbo.AuditSubsidy (
	AuditAddUpdateDelete,
	SubsidyKey,
	SubsidyProviderKey,
	EmploymentKey,
	LegalEntityKey,
	SalaryNumber,
	Paypoint,
	Notch,
	Rank,
	GeneralStatusKey,
	StopOrderAmount,
	GEPFMember
)
SELECT
	'I' as AuditAddUpdateDelete,
	SubsidyKey,
	SubsidyProviderKey,
	EmploymentKey,
	LegalEntityKey,
	SalaryNumber,
	Paypoint,
	Notch,
	Rank,
	GeneralStatusKey,
	StopOrderAmount,
	GEPFMember
FROM
	INSERTED;

SET NOCOUNT OFF 

END;
GO

ALTER TRIGGER [dbo].[tu_Subsidy] ON [dbo].[Subsidy]
FOR UPDATE 
AS

SET NOCOUNT ON 


BEGIN
INSERT INTO dbo.AuditSubsidy (
	AuditAddUpdateDelete,
	SubsidyKey,
	SubsidyProviderKey,
	EmploymentKey,
	LegalEntityKey,
	SalaryNumber,
	Paypoint,
	Notch,
	Rank,
	GeneralStatusKey,
	StopOrderAmount,
	GEPFMember
)
SELECT
	'U' as AuditAddUpdateDelete,
	SubsidyKey,
	SubsidyProviderKey,
	EmploymentKey,
	LegalEntityKey,
	SalaryNumber,
	Paypoint,
	Notch,
	Rank,
	GeneralStatusKey,
	StopOrderAmount,
	GEPFMember
FROM
	INSERTED;

SET NOCOUNT OFF 

END;
GO
