USE [2AM]
GO
begin transaction 

if exists (select * from INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'test' and TABLE_NAME = 'OffersAtInternetApplication')
begin
drop table test.OffersAtInternetApplication
End

create table test.OffersAtInternetApplication
(
TestIdentifier varchar(255) Not Null,
TestGroup varchar(255),
Username varchar(255),
Password varchar(255),
LoanType varchar(255),
Product varchar(255),
MarketValue varchar(10),
ExistingLoan varchar(10),
CashDeposit varchar(10),
CashOut varchar(10),
EmploymentType varchar(255),
Term varchar(255),
PercentageToFix varchar(3),
CapitaliseFees varchar(5) default 'false',
InterestOnly varchar(5) default 'false',
HouseHoldIncome varchar(10),
LegalEntityType varchar(255),
LegalEntityRole varchar(255),
Firstname varchar(255),
Surname varchar(255),
CompanyName varchar(255),
EstateAgency varchar(255),
OfferKey varchar(10),
AtQAFlag bit,
AtManageApplicationFlag bit,
ApplicationManagementTestID varchar(255),
AtCreditFlag bit,
CreditTestID varchar(255),
AtRegistrationPipelineFlag bit,
RegistrationPipelineTestID varchar(255),
Primary Key (TestIdentifier)
)

--truncate table test.OffersAtInternetApplication
insert into [2am].test.OffersAtInternetApplication (TestIdentifier,TestGroup,Username,Password,LoanType,Product,MarketValue,ExistingLoan,CashDeposit,CashOut,EmploymentType,Term,PercentageToFix,CapitaliseFees,InterestOnly,HouseHoldIncome,LegalEntityType,LegalEntityRole,Firstname,Surname,CompanyName,EstateAgency,OfferKey,AtQAFlag,AtManageApplicationFlag,ApplicationManagementTestID,AtCreditFlag,CreditTestID,AtRegistrationPipelineFlag,RegistrationPipelineTestID) Values ('CreateNetApplicationNewPurchaseVariable','CreateNetApplication','sahl\telcuser','Natal1','New Purchase','Variable','1000000','','','','Salaried','240','','FALSE','FALSE','100000','','','NetApplicationNewPurchase','Variable','','','','0','0','','0','','0','')
insert into [2am].test.OffersAtInternetApplication (TestIdentifier,TestGroup,Username,Password,LoanType,Product,MarketValue,ExistingLoan,CashDeposit,CashOut,EmploymentType,Term,PercentageToFix,CapitaliseFees,InterestOnly,HouseHoldIncome,LegalEntityType,LegalEntityRole,Firstname,Surname,CompanyName,EstateAgency,OfferKey,AtQAFlag,AtManageApplicationFlag,ApplicationManagementTestID,AtCreditFlag,CreditTestID,AtRegistrationPipelineFlag,RegistrationPipelineTestID) Values ('CreateNetApplicationNewPurchaseVarifix','CreateNetApplication','sahl\telcuser','Natal1','New Purchase','Varifix','1000000','','','','Self Employed','240','60','FALSE','FALSE','100000','','','NetApplicationNewPurchase','Varifix','','','','0','0','','0','','0','')
insert into [2am].test.OffersAtInternetApplication (TestIdentifier,TestGroup,Username,Password,LoanType,Product,MarketValue,ExistingLoan,CashDeposit,CashOut,EmploymentType,Term,PercentageToFix,CapitaliseFees,InterestOnly,HouseHoldIncome,LegalEntityType,LegalEntityRole,Firstname,Surname,CompanyName,EstateAgency,OfferKey,AtQAFlag,AtManageApplicationFlag,ApplicationManagementTestID,AtCreditFlag,CreditTestID,AtRegistrationPipelineFlag,RegistrationPipelineTestID) Values ('CreateNetApplicationNewPurchaseVariableInterestOnly','CreateNetApplication','sahl\telcuser','Natal1','New Purchase','Variable','1000000','','','','Self Employed','240','','FALSE','TRUE','100000','','','NetApplicationNewPurchase','VariableInterestOnly','','','','0','0','','0','','0','')
insert into [2am].test.OffersAtInternetApplication (TestIdentifier,TestGroup,Username,Password,LoanType,Product,MarketValue,ExistingLoan,CashDeposit,CashOut,EmploymentType,Term,PercentageToFix,CapitaliseFees,InterestOnly,HouseHoldIncome,LegalEntityType,LegalEntityRole,Firstname,Surname,CompanyName,EstateAgency,OfferKey,AtQAFlag,AtManageApplicationFlag,ApplicationManagementTestID,AtCreditFlag,CreditTestID,AtRegistrationPipelineFlag,RegistrationPipelineTestID) Values ('CreateNetApplicationNewPurchaseVarifixInterestOnly','CreateNetApplication','sahl\telcuser','Natal1','New Purchase','Varifix','1000000','','','','Salaried','240','60','FALSE','TRUE','100000','','','NetApplicationNewPurchase','VarifixInterestOnly','','','','0','0','','0','','0','')
insert into [2am].test.OffersAtInternetApplication (TestIdentifier,TestGroup,Username,Password,LoanType,Product,MarketValue,ExistingLoan,CashDeposit,CashOut,EmploymentType,Term,PercentageToFix,CapitaliseFees,InterestOnly,HouseHoldIncome,LegalEntityType,LegalEntityRole,Firstname,Surname,CompanyName,EstateAgency,OfferKey,AtQAFlag,AtManageApplicationFlag,ApplicationManagementTestID,AtCreditFlag,CreditTestID,AtRegistrationPipelineFlag,RegistrationPipelineTestID) Values ('CreateNetApplicationSwitchVariable','CreateNetApplication','sahl\telcuser','Natal1','Switch','Variable','1000000','50000','','100000','Salaried','240','','FALSE','FALSE','100000','','','NetapplicationSwitch','Variable','','','','0','0','','0','','0','')
insert into [2am].test.OffersAtInternetApplication (TestIdentifier,TestGroup,Username,Password,LoanType,Product,MarketValue,ExistingLoan,CashDeposit,CashOut,EmploymentType,Term,PercentageToFix,CapitaliseFees,InterestOnly,HouseHoldIncome,LegalEntityType,LegalEntityRole,Firstname,Surname,CompanyName,EstateAgency,OfferKey,AtQAFlag,AtManageApplicationFlag,ApplicationManagementTestID,AtCreditFlag,CreditTestID,AtRegistrationPipelineFlag,RegistrationPipelineTestID) Values ('CreateNetApplicationSwitchVarifix','CreateNetApplication','sahl\telcuser','Natal1','Switch','Varifix','1000000','50000','','100000','Self Employed','240','60','FALSE','FALSE','100000','','','NetapplicationSwitch','Varifix','','','','0','0','','0','','0','')
insert into [2am].test.OffersAtInternetApplication (TestIdentifier,TestGroup,Username,Password,LoanType,Product,MarketValue,ExistingLoan,CashDeposit,CashOut,EmploymentType,Term,PercentageToFix,CapitaliseFees,InterestOnly,HouseHoldIncome,LegalEntityType,LegalEntityRole,Firstname,Surname,CompanyName,EstateAgency,OfferKey,AtQAFlag,AtManageApplicationFlag,ApplicationManagementTestID,AtCreditFlag,CreditTestID,AtRegistrationPipelineFlag,RegistrationPipelineTestID) Values ('CreateNetApplicationSwitchVariableCapitaliseFees','CreateNetApplication','sahl\telcuser','Natal1','Switch','Variable','1000000','50000','','100000','Self Employed','240','','TRUE','TRUE','100000','','','NetapplicationSwitch','VariableCapitaliseFees','','','','0','0','','0','','0','')
insert into [2am].test.OffersAtInternetApplication (TestIdentifier,TestGroup,Username,Password,LoanType,Product,MarketValue,ExistingLoan,CashDeposit,CashOut,EmploymentType,Term,PercentageToFix,CapitaliseFees,InterestOnly,HouseHoldIncome,LegalEntityType,LegalEntityRole,Firstname,Surname,CompanyName,EstateAgency,OfferKey,AtQAFlag,AtManageApplicationFlag,ApplicationManagementTestID,AtCreditFlag,CreditTestID,AtRegistrationPipelineFlag,RegistrationPipelineTestID) Values ('CreateNetApplicationSwitchVarifixCapitaliseFees','CreateNetApplication','sahl\telcuser','Natal1','Switch','Varifix','1000000','50000','','100000','Salaried','240','60','TRUE','TRUE','100000','','','NetapplicationSwitch','VarifixCapitalisefees','','','','0','0','','0','','0','')

commit
--rollback

--select * from test.OffersAtInternetApplication