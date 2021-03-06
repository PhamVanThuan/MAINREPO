USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[InsertCAPBrokerOrAdmin]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[InsertCAPBrokerOrAdmin]
	Print 'Dropped procedure [test].[InsertCAPBrokerOrAdmin]'
End
Go

--usage example: EXEC test.InsertCAPBrokerOrAdmin 'CONSULTANT', 'SAHL\ClintonS'
CREATE PROCEDURE [test].[InsertCAPBrokerOrAdmin]

	@CapUserUype VARCHAR(50), --use 'CAP ADMIN' for a CAP Credit User or 'CONSULTANT' for a CAP Consultant
	@ADUserName VARCHAR(50)
	
AS
BEGIN

DECLARE  @ADUserKey INT 
DECLARE  @BrokerKey INT 
SET		 @BrokerKey = 0 

DECLARE  @legalentitykey INT 
DECLARE  @OrganisationStructureKey INT 
DECLARE  @UserName VARCHAR(50)

SELECT @OrganisationStructureKey = vos.organisationstructurekey 
FROM   [2am].dbo.vorganisationstructure vos 
       JOIN [2am].dbo.vorganisationstructure vos_parent 
         ON vos.organisationstructureparentkey = vos_parent.organisationstructurekey 
WHERE  vos.organisationstructure = @CapUserUype --either a CAP admin for the credit user OR a consultant for the CAP consultant user. 
       AND vos_parent.organisationstructure = 'CAP' 
       
SELECT @ADUserKey = aduserkey, 
       @UserName = firstnames + Space(1) + surname 
FROM   aduser ad 
       JOIN legalentity le 
         ON ad.legalentitykey = le.legalentitykey 
WHERE  adusername = @ADUserName 

SELECT @BrokerKey = brokerkey 
FROM   [2am].dbo.broker 
WHERE  adusername = @ADUserName 

IF @BrokerKey = 0 
  BEGIN 
    INSERT INTO [2am].dbo.broker 
               (adusername, 
                fullname, 
                initials, 
                generalstatuskey, 
                aduserkey) 
    VALUES     (@ADUserName, 
                @UserName, 
                Substring(@username,0,2), 
                1, 
                @ADUserKey) 
     
    SET @BrokerKey = Scope_identity() 
  END 

IF NOT EXISTS (SELECT * 
               FROM   [2am].dbo.userorganisationstructure 
               WHERE  aduserkey = @ADUserKey 
                      AND organisationstructurekey = @OrganisationStructureKey) 
  BEGIN 
    INSERT INTO [2am].dbo.userorganisationstructure
    (ADuserKey, OrganisationStructureKey) 
    SELECT @ADUserKey, 
           @OrganisationStructureKey 
  END 

IF @CapUserUype = 'CAP ADMIN'
  BEGIN 

	IF NOT EXISTS(SELECT * FROM CapCreditBrokerToken where BrokerKey=@BrokerKey)
		BEGIN
			INSERT INTO [2am].dbo.capcreditbrokertoken 
			SELECT @BrokerKey, 
				   0
		END
  END 
  
END
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

