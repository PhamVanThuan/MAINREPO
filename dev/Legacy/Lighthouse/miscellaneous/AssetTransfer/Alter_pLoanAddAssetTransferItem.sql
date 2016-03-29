SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



/*******************************************************************************************************************************************************************************  
 Description:   
  Adds an account to the AssetTransfer table and calculates fields  
 History:    
  19/08/2005 Jason Ball Created  
  09/11/2005 Jason Ball Removed join to Bond for 2am loan  
  09/12/2005 Jason Ball Changed to look for product instead on database to determine if VariFix or not  
  03/05/2006 Gary Daniell Added return result MaxReAdvance when SPVKey = 24, DetailTypeNumber = 417 for: Help Desk Request Number 11763
  
*******************************************************************************************************************************************************************************/  
  
ALTER               Procedure [dbo].[pLoanAddAssetTransferItem]  
@AccountKey int,  
@InclUnderCancelYN char(1),  
@UserName varchar(64)  
As  
Begin  
  
  
-- Date to which accrued interest is calculated interest  
  
Declare @TotalAccruedInt float,  
  @AccruedInt float,  
  @FinancialServiceKey int,  
  @ToDate smalldatetime,  
  @FromDate smalldatetime,  
  @ToDateSAHLDB varchar(10),  
  @FromDateSAHLDB varchar(10),  
  @Ret int,  
  @RetMessage varchar(256),  
  @ATProduct int,  
  @AddProduct int

Select @RetMessage = ''  
  
 -- Check if loan already exists  
 If Exists (Select * From AssetTransfer Where AccountKey = @Accountkey)  
  Begin   
   Select @RetMessage = 'Loan already added by ' + UserName  
   From AssetTransfer   
   Where AccountKey = @Accountkey  
   Raiserror(@RetMessage,11,1)  
   Return 1  
  End  
  
 -- Check for valid loan number  
 If Not Exists (Select * From FinancialService fs Join MortgageLoan ml On ml.FinancialServiceKey = fs.FinancialServiceKey Where fs.AccountKey = @AccountKey)  
  If Not Exists (Select * From SAHLDB..Loan Where LoanNumber = @AccountKey)  
   Begin   
    Raiserror('Invalid loan number!',11,1)  
    Return 1  
   End  
  
 -- Added by RA Pitout 13/10/2005  
 -- Check to make sure VariFix and non-VariFix loans are grouped and not mixed.  
  
 Select Top 1 @ATProduct = 
	Case 
		When ProductKey is Null Then 1 
		Else ProductKey 
	End   
 From AssetTransfer att  
  Left Join Account a  
   on att.AccountKey = a.AccountKey  
  Left Join OriginationSourceProduct osp  
   On osp.OriginationSourceProductKey = a.OriginationSourceProductKey  
 Where UserName = @UserName  
  
  
 Select @AddProduct = Case When ProductKey is Null  
   Then 1 Else ProductKey End   
 From SAHLDB..vw_AllOpenLoanAccounts l  
  Left Join Account a  
   on a.AccountKey = l.LoanNumber  
  Left Join OriginationSourceProduct osp  
   On osp.OriginationSourceProductKey = a.OriginationSourceProductKey  
 Where l.LoanNumber = @AccountKey  
  
 select @AddProduct, @ATProduct  
  
 If @AddProduct <> @ATProduct  
  Begin  
   If (@ATProduct = 2)   
    Begin  
     set @RetMessage = 'There are already VariFix loans added, and loan ' + cast(@AccountKey as varchar(20)) + ' is a Standard Variable loan.'  
    End   
   Else if (@ATProduct = 1)  
    Begin  
     set @RetMessage = 'There are already Standard Variable loans added, and loan ' + cast(@AccountKey as varchar(20)) + ' is a VariFix loan.'  
    End  
   Else if (@ATProduct = 5)  
    Begin  
     set @RetMessage = 'There are already Super Rate loans added, and loan ' + cast(@AccountKey as varchar(20)) + ' is a Super Rate loan.'  
    End  
  
   Set @RetMessage = @RetMessage + ' You can only transfer loans of the same type.'  
  
   Raiserror(@RetMessage, 11, 1)  
   Return 1  
  End  
  
 -- Check for loan under cancellation  
 If @InclUnderCancelYN = 'N'  
  If Exists (Select * From SAHLDB..Detail Where detailtypenumber = 11 And LoanNumber = @AccountKey)  
   Begin   
    Raiserror('Loan under cancellation!',11,1)  
    Return 1  
   End  
  
 -- Check if 2am open loan  
 If dbo.fWhichDB(@AccountKey) = '[2am]'  
  Begin   
   If Exists (Select * From vOpenMortgageLoan Where AccountKey = @AccountKey)  
    Begin  
       
     -- Save the details of the account to be transfered  
     Insert Into [2am]..AssetTransfer  
     Select AccountKey,  
       ClientSurname,  
       SPVKey,  
       LoanTotalBondAmount,  
       Sum(CurrentBalance) as CurrentBalance,  
       @UserName,  
       'N'  
     From MortgageLoan ml  
      Join vOpenMortgageLoan vml  
       On vml.FinancialServiceKey = ml.FinancialServiceKey  
      Join SAHLDB..Loan l  
       On l.LoanNumber = vml.AccountKey  
      Join SAHLDB..Client c  
       On c.ClientNumber = l.ClientNumber  
     Where vml.AccountKey = @AccountKey  
     Group By AccountKey, ClientSurname, SPVKey, LoanTotalBondAmount  

    End  -- open mortgage loan  
   Else  
    Raiserror('Not an open mortgage loan!',11,1)  
  
  End  
  
 -- Check if SAHLDB open loan  
 Else If dbo.fWhichDB(@AccountKey) = 'SAHLDB'  
  Begin   
  
   If Exists (Select * From SAHLDB..vw_OpenLoans Where LoanNumber = @AccountKey)  
    Begin  
      
     -- Save the details of the account to be transfered  
     Insert Into AssetTransfer  
     Select  l.LoanNumber,  
       ClientSurname,    
       SPVNumber,  
       LoanTotalBondAmount,  
       LoanCurrentBalance,  
       @UserName,  
       'N'  
     From SAHLDB..Loan l    
      Join SAHLDB..vw_OpenLoans ol  
       On ol.LoanNumber = l.LoanNumber    
      Join SAHLDB..Client c  
       On c.ClientNumber = l.ClientNumber  
     Where l.LoanNumber = @AccountKey  

    End -- open mortgage loan  
   Else  
    Raiserror('Not an open mortgage loan!',11,1)  
  End  
 Else  
  Raiserror('Unable to determine database. Contact administrator',11,1)  
  
 If @RetMessage <> ''  
  Begin   
   Set @RetMessage = 'Unable to calculate accrued interest for: ' + @RetMessage  
   Raiserror(@RetMessage,11,1)   
  End
 Else
  Begin
   --Get a MaxReadvance = 95% LTV - LoanCurrentBalance
    select ((PropertyLatestValuation * 0.95) - LoanCurrentBalance) As MaxReAdvance From SAHLDB..vw_AllOpenLoansBasic vOL 
     Left Join SAHLDB..Detail d On vOL.Loannumber = d.LoanNumber
     Where vOL.Loannumber = @AccountKey And 
     vOL.spvnumber = 24 And d.DetailTypeNumber = 417
     --Select * From AssetTransfer a Inner Join SAHLDB..Detail d On a.AccountKey = d.LoanNumber Where a.AccountKey = @AccountKey And SPVKey = 24 And d.Detail = 417
  End
End  
  
  
  
  
  
  
  
  


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

