USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.CleanUpOfferDebitOrder') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.CleanUpOfferDebitOrder
	Print 'Dropped Proc test.CleanUpOfferDebitOrder'
End
Go

/****** Object:  StoredProcedure [test].[CleanUpOfferDebitOrder]  Script Date: 10/20/2010 07:23:42 ******/

CREATE PROCEDURE [test].[CleanUpOfferDebitOrder]

@offerKey int

AS

--application debit order

declare @offerTypeKey int

select @offerTypeKey = offerTypeKey from [2am]..Offer
where offerKey = @offerKey


if @offerTypeKey <> 11

begin

if not exists (select 1 from [2am].dbo.offerDebitOrder odo
where odo.offerKey=@offerKey)
begin
      declare @bankAccountKey int
      declare @legalEntityKey_ba int

      if not exists (
      select 1 from 
      [2am].dbo.offer o
      join [2am].dbo.offerRole ofr on o.offerKey=ofr.offerKey AND offerroletypekey IN (8,10,12,11)  
      join [2am].dbo.legalentitybankaccount leba on ofr.legalEntityKey=leba.legalEntityKey
      where o.offerKey=@offerKey)
            begin
                  --we do not have a legalentity bank account to use so we need insert one
                  insert into [2am]..BankAccount
                  (AcbBranchCode,AccountNumber, ACBTypeNumber, AccountName, UserID, ChangeDate)
                  select top 1 test.ACBBranchCode,test.AccountNumber, test.ACBTypeNumber,'Test','System',getdate() 
                  from test.bankaccountdetails test
                  left join bankaccount ba on test.accountnumber=ba.accountnumber
                  where ba.bankaccountkey is null
                  
                  select @bankAccountKey = scope_identity()
                  select top 1 @legalEntityKey_ba = ofr.legalEntityKey
                  from [2am].dbo.offer o
                  join [2am].dbo.offerRole ofr on o.offerKey=ofr.offerKey AND offerroletypekey IN (8,10,12,11) 
                  where o.offerKey=@offerKey
                  
                  --we can now insert into the record
                  insert into [2am].dbo.legalEntityBankAccount
                  (BankAccountKey, LegalEntityKey, GeneralStatusKey, UserID, ChangeDate)
                  values
                  (
                  @bankAccountKey, @legalEntityKey_ba, 1, 'System', GetDate()
                  )
            end
      else
            begin
                  --we do have a legal entity bank account to use
                  select top 1 @bankAccountKey = leba.bankAccountKey
                  from [2am].dbo.offer o
                  join [2am].dbo.offerRole ofr on o.offerKey=ofr.offerKey AND offerroletypekey IN (8,10,12,11)  
                  join [2am].dbo.legalentitybankaccount leba on ofr.legalEntityKey=leba.legalEntityKey
                  where o.offerKey=@offerKey    
            end
            
            insert into [2am].dbo.OfferDebitOrder
            (OfferKey, BankAccountKey, Percentage, DebitOrderDay, FinancialServicePaymentTypeKey)
            values
            (
            @offerKey, @bankAccountKey, 0, '26', 1
            )
end         

end

if @OfferTypeKey = 11
begin

if not exists (select 1 from [2am].dbo.offerDebitOrder odo
where odo.offerKey=@offerKey)
begin

      if not exists (
      select 1 from 
      [2am].dbo.offer o
      join [2am].dbo.externalRole er on o.offerKey=er.genericKey 
      AND externalRoletypekey IN (1)  
      AND genericKeyTypeKey = 2
      join [2am].dbo.legalentitybankaccount leba 
      on er.legalEntityKey=leba.legalEntityKey
      where o.offerKey=@offerKey)
            begin
                  --we do not have a legalentity bank account to use so we need insert one
                  insert into [2am]..BankAccount
                  (AcbBranchCode,AccountNumber, ACBTypeNumber, AccountName, UserID, ChangeDate)
                  select top 1 test.ACBBranchCode,test.AccountNumber, test.ACBTypeNumber,'Test','System',getdate() 
                  from test.bankaccountdetails test
                  left join bankaccount ba on test.accountnumber=ba.accountnumber
                  where ba.bankaccountkey is null
                  
                  select @bankAccountKey = scope_identity()
                  select top 1 @legalEntityKey_ba = er.legalEntityKey
                  from [2am].dbo.offer o
				  join [2am].dbo.externalRole er on o.offerKey=er.genericKey 
				  AND externalRoletypekey IN (1)  
				  AND genericKeyTypeKey = 2
                  where o.offerKey=@offerKey
                  
                  --we can now insert into the record
                  insert into [2am].dbo.legalEntityBankAccount
                  (BankAccountKey, LegalEntityKey, GeneralStatusKey, UserID, ChangeDate)
                  values
                  (
                  @bankAccountKey, @legalEntityKey_ba, 1, 'System', GetDate()
                  )
            end
      else
            begin
                  --we do have a legal entity bank account to use
                  select top 1 @bankAccountKey = leba.bankAccountKey
                  from [2am].dbo.offer o
				  join [2am].dbo.externalRole er on o.offerKey=er.genericKey 
				  AND externalRoletypekey IN (1)  
				  AND genericKeyTypeKey = 2
                  join [2am].dbo.legalentitybankaccount leba on er.legalEntityKey=leba.legalEntityKey
                  where o.offerKey=@offerKey    
            end
            
            insert into [2am].dbo.OfferDebitOrder
            (OfferKey, BankAccountKey, Percentage, DebitOrderDay, FinancialServicePaymentTypeKey)
            values
            (
            @offerKey, @bankAccountKey, 0, '26', 1
            )
end 

end
