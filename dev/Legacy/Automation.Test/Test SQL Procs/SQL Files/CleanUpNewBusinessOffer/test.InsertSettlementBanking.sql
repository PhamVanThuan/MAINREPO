USE [2AM]
GO
/****** Object:  StoredProcedure [test].[GetCasesInWorklistByStateAndTypeAndLTV]    Script Date: 10/06/2010 12:51:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'test.InsertSettlementBanking') AND type in (N'P', N'PC'))
DROP PROCEDURE test.InsertSettlementBanking
go

USE [2AM]
GO
/****** Object:  StoredProcedure test.InsertSettlementBanking******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE test.InsertSettlementBanking

@offerKey int

AS

--add the settlement banking details
if (select offerTypeKey from offer (nolock) where offerKey=@offerKey) = 6
	begin
	
	if not exists (select 1 from offerExpense where offerKey = @offerKey and ExpenseTypeKey=8)
		begin			
			--we need a bank account to use so we need to insert one
			insert into [2am]..BankAccount
			(AcbBranchCode,AccountNumber, ACBTypeNumber, AccountName, UserID, ChangeDate)
			select top 1 test.ACBBranchCode,test.AccountNumber, test.ACBTypeNumber,'Test','System',getdate() 
			from test.bankaccountdetails test
			left join bankaccount ba on test.accountnumber=ba.accountnumber
			where ba.bankaccountkey is null
			declare @bankAccountKey int
			select @bankAccountKey = scope_identity()	
			declare @existingLoan float
			select @existingLoan = existingloan from [2am]..offer
			join
			(SELECT        
			o.offerkey as OfferKey, max(offerinformationkey) as OfferInformationKey 
			FROM  [2am]..[Offer] O
			JOIN [2am]..[OfferInformation] OI (NOLOCK) ON O.OfferKey = OI.OfferKey
			WHERE   o.offerKey = @OfferKey
			GROUP BY O.OfferKey) as maxoi on offer.offerkey=maxoi.offerkey
			join [2am]..offerinformationvariableloan oivl on maxoi.offerinformationkey=oivl.offerinformationkey

			--insert the offerexpense record
			insert into [2am]..offerExpense
			select @OfferKey, NULL, 8, NULL, NULL, 'Test', @existingLoan, 0, 0, 0

			--get the key
			declare @offerExpenseKey int
			select @offerExpenseKey = scope_identity()

			--insert the offerDebtSettlement
			insert into [2am].dbo.offerDebtSettlement
			select @offerExpenseKey, 0, getdate(), 2, @bankAccountKey, NULL,0,getdate(),@existingLoan,0,NULL
		end
end