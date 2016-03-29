USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.CleanUpLegalEntityData') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.CleanUpLegalEntityData
	Print 'Dropped Proc test.CleanUpLegalEntityData'
End
Go

/****** Object:  StoredProcedure [test].[CleanUpOfferDebitOrder]  Script Date: 10/20/2010 07:23:42 ******/


CREATE PROCEDURE [test].[CleanUpLegalEntityData]

@offerKey int

AS

--collect legal entities for origination/personal loan offers

declare @table table (legalEntityKey int)

insert into @table
select ofr.LegalEntityKey
from dbo.Offer o
join dbo.OfferRole ofr on o.offerKey = ofr.offerKey
where ofr.offerroletypekey IN (8,10,12,11)
and o.offerKey = @offerKey
--PL offers
insert into @table
select e.legalentityKey 
from dbo.Offer o
join dbo.ExternalRole e on o.offerKey = e.GenericKey
where e.ExternalRoleTypeKey = 1
and o.offerKey = @offerKey
 
 
UPDATE le   
SET    maritalstatuskey = 2,   
       populationgroupkey = 1,
       educationKey = 2,
       homeLanguageKey=2,
       documentLanguageKey=2,
	   salutationkey=1,
	   genderkey=1    
FROM [2AM].dbo.legalentity le 
	JOIN @table t  ON t.legalentitykey = le.legalentitykey   
WHERE   
le.legalentitytypekey in (2)  

UPDATE le 
SET CitizenTypeKey = 1
FROM [2AM].dbo.legalentity le 
	JOIN @table t  ON t.legalentitykey = le.legalentitykey   
WHERE 
CitizenTypeKey is null
  
 
UPDATE le   
SET    initials='A'  
FROM [2AM].dbo.legalentity le 
	JOIN @table t  ON t.legalentitykey = le.legalentitykey  
WHERE   
legalentitytypekey in (2) and (initials is null or len(initials) = 0)
  
PRINT 'Update to ensure at least one contact detail for: ' + cast(@OfferKey as varchar(10))  
UPDATE le   
SET    CellPhonenumber = '0123456789',   
       EmailAddress = 'test@test.co.za'   
FROM [2AM].dbo.legalentity le 
	JOIN @table t  ON t.legalentitykey = le.legalentitykey 
WHERE   
(le.CellPhonenumber is null or len(le.CellPhonenumber)=0)   
AND (le.EmailAddress is null or len(le.EmailAddress) = 0)  
  
PRINT 'Update Trust/CC Details for: ' + cast(@OfferKey as varchar(10))  
UPDATE le   
SET    TradingName=RegisteredName,   
       WorkPhoneCode='031',  
    WorkPhoneNumber='1235467',
    RegistrationNumber = SUBSTRING(REPLACE(CONVERT(varchar(255), NEWID()),'-',''), 0, 20) 
FROM [2AM].dbo.legalentity le 
	JOIN @table t  ON t.legalentitykey = le.legalentitykey 
WHERE   
legalentitytypekey in (3,4,5)  

PRINT 'Update the passport id number for foreign legal entities ' + cast(@OfferKey as varchar(10))  

declare @RandomString varchar(20)

UPDATE le   
SET   
	passportnumber =  SUBSTRING(REPLACE(CONVERT(varchar(255), NEWID()),'-',''), 0, 15)

FROM [2AM].dbo.legalentity le 
	JOIN @table t  ON t.legalentitykey = le.legalentitykey 
WHERE   
legalentitytypekey in (2)  
and CitizenTypeKey in (2,4,5,6,7,8,9,10)

GO

