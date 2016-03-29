
use [2AM]
go
if (object_id('test.CreditScoringTestApplicants') is not null)
	begin
		ALTER TABLE test.CreditScoringTestApplicants Drop constraint FK_TestIdentifier
		print 'Dropped FOREIGN KEY CONSTRAINT: FK_TestIdentifier'
		drop TABLE test.CreditScoringTestApplicants
		print 'Dropped TABLE: test.CreditScoringTestApplicants'
	end
go

use [2AM]
go
if (object_id('test.CreditScoringTests') is not null)
	begin
		drop TABLE test.CreditScoringTests
		print 'Dropped TABLE: test.CreditScoringTests'
	end
go

CREATE TABLE test.CreditScoringTests
(
TestIdentifier VARCHAR(250) Primary Key,
AggregateDecision VARCHAR(7) NOT NULL,
CallingProcess VARCHAR(50), 
CallingMethod VARCHAR(50)
)
Go

CREATE TABLE test.CreditScoringTestApplicants
(
ApplicantIdentifier int Identity(1,1),
TestIdentifier VARCHAR(250) NOT NULL,
ApplicantType Varchar(9) NOT NULL,
OfferRoleType varchar(50),
ApplicantDecision VARCHAR(50) NOT NULL,
ApplicantEmpiricaScore int NOT NULL,
ApplicantSBCScore int NOT NULL,
ApplicantRiskMatrixCellKey Varchar(2),
ApplicantRiskMatrixDescription varchar(50),
LegalEntityID VARCHAR(13),
Income FLOAT,
IncomeContributor BIT
Constraint FK_TestIdentifier Foreign Key (TestIdentifier) References test.CreditScoringTests(TestIdentifier),
Primary Key (ApplicantIdentifier)
)
Go

--Populate test.CreditScoringTests table
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('PrimMAAcceptSecSurDeclineHighLTV', 'Refer', '', '')
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('PrimMAReferLowLTV', 'Refer', '', '')
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('PrimMADeclineSecMAReferLowLTV', 'Decline', '', '')
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('TwoMABothIncomeContrib', 'Accept', '', '')
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('TwoMAOneIncomeContrib', 'Accept', '', '')
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('OneMAOneSuretyBothIncomeContrib', 'Refer', '', '')
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('OneMATwoSuretyMANotIncomeContrib', 'Refer', '', '')
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('OneMainAppCompanyNatPersonSurety', 'Decline', '', '')
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('IncContribCompanyMultipleNatPersons', 'Accept', '', '')
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('ThreeMAAllIncomeContrib', 'Refer', '', '')
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('ITCValidSBC', 'Refer', '', '')
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('ITCDisputeIndicated', 'Refer', '', '')
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('ITCValidEmpericaScore', 'Refer', '', '')
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('ITCAccountCustomerWEPPStatusDecline', 'Decline', '', '')
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('ITCAccountDefaultsIndicated', 'Refer', '', '')
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('ITCAccountLegalNoticesIndicated', 'Refer', '', '')
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('ITCAccountJudgementsIndicated', 'Refer', '', '')
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('ITCDebtReviewIndicated', 'Refer', '', '')
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('ITCValidEmpiricaScoreReasonCodes', 'Refer', '', '')
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('ITCAccountCustomerWEPPStatusRefer', 'Refer', '', '')
Insert into test.CreditScoringTests (TestIdentifier, AggregateDecision, CallingProcess, CallingMethod) Values ('ITCAccountCustomerWEPPStatusAlpha', 'Refer', '', '')


GO

--Populate test.CreditScoringTestApplicants table
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('PrimMAAcceptSecSurDeclineHighLTV','Primary','Lead - Main Applicant','Accept',635,609,'','','',50000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('PrimMAAcceptSecSurDeclineHighLTV','Secondary','Lead - Suretor','Decline',615,553,'','','',20000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('PrimMAReferLowLTV','Primary','Lead - Main Applicant','Refer',605,541,'','','',50000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('PrimMADeclineSecMAReferLowLTV','Primary','Lead - Main Applicant','Decline',600,553,'','','',30000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('PrimMADeclineSecMAReferLowLTV','Secondary','Lead - Main Applicant','Refer',600,572,'','','',20000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('TwoMABothIncomeContrib','Primary','Lead - Main Applicant','Accept',635,609,'','','',50000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('TwoMABothIncomeContrib','Secondary','Lead - Main Applicant','Accept',635,609,'','','',20000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('TwoMAOneIncomeContrib','Primary','Lead - Main Applicant','Accept',605,588,'','','',80000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('TwoMAOneIncomeContrib','None','Lead - Main Applicant','No Score - No ITC',0,0,'','','',85000,0)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('OneMAOneSuretyBothIncomeContrib','Primary','Lead - Main Applicant','Accept',600,609,'','','',50000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('OneMAOneSuretyBothIncomeContrib','Secondary','Lead - Suretor','Decline',600,553,'','','',55000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('OneMATwoSuretyMANotIncomeContrib','None','Lead - Main Applicant','No Score - No ITC',0,0,'','','',50000,0)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('OneMATwoSuretyMANotIncomeContrib','Primary','Lead - Suretor','Refer',635,562,'','','',30000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('OneMATwoSuretyMANotIncomeContrib','Secondary','Lead - Suretor','Refer',635,562,'','','',20000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('OneMainAppCompanyNatPersonSurety','None','Lead - Main Applicant','No Score - No ITC',0,0,'','','',10000,0)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('OneMainAppCompanyNatPersonSurety','Primary','Lead - Suretor','Decline',580,536,'','','',80000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('IncContribCompanyMultipleNatPersons','None','Lead - Main Applicant','No Score - No ITC',0,0,'','','',50000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('IncContribCompanyMultipleNatPersons','Primary','Lead - Suretor','Accept',605,588,'','','',35000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('IncContribCompanyMultipleNatPersons','None','Lead - Suretor','No Score - No ITC',0,0,'','','',15000,0)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('ThreeMAAllIncomeContrib','Primary','Lead - Main Applicant','Accept',600,609,'','','',50000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('ThreeMAAllIncomeContrib','Secondary','Lead - Main Applicant','Decline',600,553,'','','',30000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('ThreeMAAllIncomeContrib','None','Lead - Main Applicant','Not Scored - Accept',0,0,'','','',20000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('ITCValidSBC','Primary','Lead - Main Applicant','No Score - No ITC',0,0,'','','',40000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('ITCDisputeIndicated','Primary','Lead - Main Applicant','No Score - No ITC',0,0,'','','',40000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('ITCValidEmpericaScore','Primary','Lead - Main Applicant','No Score - No ITC',0,0,'','','',40000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('ITCAccountCustomerWEPPStatusDecline','Primary','Lead - Main Applicant','No Score - No ITC',0,0,'','','',40000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('ITCAccountDefaultsIndicated','Primary','Lead - Main Applicant','No Score - No ITC',0,0,'','','',40000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('ITCAccountLegalNoticesIndicated','Primary','Lead - Main Applicant','No Score - No ITC',0,0,'','','',40000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('ITCAccountJudgementsIndicated','Primary','Lead - Main Applicant','No Score - No ITC',0,0,'','','',40000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('ITCDebtReviewIndicated','Primary','Lead - Main Applicant','No Score - No ITC',0,0,'','','',40000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('ITCValidEmpiricaScoreReasonCodes','Primary','Lead - Main Applicant','No Score - No ITC',0,0,'','','',40000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('ITCAccountCustomerWEPPStatusRefer','Primary','Lead - Main Applicant','No Score - No ITC',0,0,'','','',40000,1)
Insert into test.CreditScoringTestApplicants (TestIdentifier, ApplicantType, OfferRoleType, ApplicantDecision, ApplicantEmpiricaScore, ApplicantSBCScore, ApplicantRiskMatrixCellKey, ApplicantRiskMatrixDescription, LegalEntityID,Income, IncomeContributor) Values ('ITCAccountCustomerWEPPStatusAlpha','Primary','Lead - Main Applicant','No Score - No ITC',0,0,'','','',40000,1)

Go
