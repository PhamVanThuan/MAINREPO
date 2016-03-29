USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[AddAccountUnderDebtCounselling]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].AddAccountUnderDebtCounselling
	Print 'Dropped procedure [test].[AddAccountUnderDebtCounselling]'
End
Go

CREATE PROCEDURE test.AddAccountUnderDebtCounselling 
@AccountKey int

AS
BEGIN

	DECLARE @debtCounsellingGroupKey INT
	DECLARE @debtCounsellingKey INT
	
	IF NOT EXISTS (SELECT 1 FROM debtCounselling.debtCounselling WHERE accountKey=@accountKey
	AND debtCounsellingStatusKey=1
	)
	
	BEGIN

		--we need a debtCounsellingGroupKey
		INSERT INTO debtCounselling.debtCounsellingGroup
		(CreatedDate)
		VALUES
		(GETDATE())

		SET @debtCounsellingGroupKey = scope_identity()

		--insert into DebtCounselling
		INSERT INTO debtCounselling.debtCounselling
		(debtCounsellingGroupKey, AccountKey, DebtCounsellingStatusKey)
		VALUES
		(@debtCounsellingGroupKey, @AccountKey, 1)

		SET @debtCounsellingKey = scope_identity() 

		--insert into ExternalRole
		INSERT INTO [2am].dbo.ExternalRole
		(GenericKey, GenericKeyTypeKey, LegalEntityKey, ExternalRoleTypeKey, GeneralStatusKey, ChangeDate)
		SELECT 
		@debtCounsellingKey, 27, r.legalEntityKey, 1, 1, GETDATE()
		FROM [2am].dbo.Account a
		JOIN [2am].dbo.Role r ON a.AccountKey=r.AccountKey
		join [2am].dbo.LegalEntity le on r.LegalEntityKey=le.legalEntityKey
		WHERE a.accountKey = @AccountKey
		AND r.generalStatusKey=1 and le.legalentitytypekey=2
		
		SELECT @debtCounsellingKey as DebtCounsellingKey, @debtCounsellingGroupKey as DebtCounsellingGroupKey
	END
	
	ELSE
		BEGIN
			SELECT 0, 0
		END
END
