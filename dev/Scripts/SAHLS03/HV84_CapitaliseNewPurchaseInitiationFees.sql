USE [SAHLDB]
GO
/****** Object:  StoredProcedure [dbo].[f_GenerateDisbursementTxns]    Script Date: 2015-10-01 12:00:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO



ALTER procedure [dbo].[f_GenerateDisbursementTxns] 
@LoanNumber 		integer,
@FromDate 			DateTime,
@ToDate 			DateTime,
@UserID 			varchar(25),
@TxnCode1 			integer=0,               --Guarantee Amt
@TxnCodeAmt1 		decimal(22,10)=0.00,
@TxnCode2			integer=0,               --Amt paid to Client
@TxnCodeAmt2 		decimal(22,10)=0.00,
@TxnCode3 			integer=0,               --Client Deposit
@TxnCodeAmt3 		decimal(22,10)=0.00,
@TxnCode4 			integer=0,               -- Field1
@TxnCodeAmt4 		decimal(22,10)=0.00,
@TxnCode5 			integer=0,               -- Field2 
@TxnCodeAmt5 		decimal(22,10)=0.00,
@TxnCode6 			integer=0,               -- Field 3
@TxnCodeAmt6 		decimal(22,10)=0.00,
@TxnCode7 			integer=0,               -- Field 4
@TxnCodeAmt7 		decimal(22,10)=0.00,
@TxnCode8 			integer=0,                -- Field 5
@TxnCodeAmt8 		decimal(22,10)=0.00,
@TxnCode9 			integer=0,               -- Field 6
@TxnCodeAmt9 		decimal(22,10)=0.00,
@TxnCode10 			integer=0,
@TxnCodeAmt10 		decimal(22,10)=0.00,
@TxnCode11 			integer=0,
@TxnCodeAmt11 		decimal(22,10)=0.00,
@TxnCode12 			integer=0,
@TxnCodeAmt12 		decimal(22,10)=0.00,
@TxnCode14 			integer=0,
@TxnCodeAmt14 		decimal(22,10)=0.00,
@RegistrationAmt 		decimal(22,10)=0.00,
@DeedsOfficeNbr 		integer,
@AttorneyNbr 			integer ,
@LoanAgreementAmt		decimal(22,10)=0.00
WITH EXECUTE AS 'HaloProxy'
as
begin
/*

Author		Paul C	11-04-2012

Description: 
		#20725. Revamp of existing proc. Names and behaviour kept the same as its called from lighthouse. 

2012-12-13	Grantw	Ensure that the Admin fee from the monthly premiumn table is used.
2013-01-17	BryanP	Pass through HOC base premium to calc Prorata #22871
2013-05-18	CraigF	TRAC #23842 pProcessPendingDomiciliumAddress moved to lighthouse schema
2014-06-10	BryanP	FB2263 make sure child account SPVs are synced to parent's
2014-08-27	BryanP	FB2875 don't update HOC.OpenDate=getdate(), use @ToDate
2015-09-08	VirekR	Create new flags by inserting into AccountInformation table for GEPF and Stop Order Discount Eligible 
					https://sahomeloans.atlassian.net/browse/BEBD-55
*/

begin try

	-- Check LoanNumber is valid
	if (select count(AccountKey) from [2am].dbo.Account where AccountKey=@LoanNumber) = 0
		raiserror('Account does not exist', 16, 1)

	begin tran

	
	
	DECLARE @Number 			int
	,@d_Loan 			decimal(22,10)
	,@d_PeriodRate 		decimal(22,10)
	,@BaseRate			decimal(22,10)
	,@d_LinkRate 		decimal(22,10)
	,@d_InstallmentAmount 	decimal(22,10)
	,@BondCount 			int
	,@d_Install 			decimal(22,10)
	,@l_RetCode 			int
	,@ResetConfigurationKey	int
	,@VarRateConfigKey	int
	,@s_RateDate 		char(10)
	,@dLoanOpenDate		datetime
	,@HOCProRataPremium  decimal(22,10)
	,@HOCPremium			decimal(22,10)
	,@HOCPolicyNumber	int
	,@FinancialServiceKey int
	,@ProspectNumber		int
	,@HOCHistoryKey		int
	,@HOCThatchAmount	decimal(22,10)
	,@HOCConventionalAmount decimal(22,10)
	,@HOCShingleAmount	decimal(22,10)
	,@HOCSASRIAAmount	decimal(22,10)
	,@HOCBasePremium		decimal(22,10)
	,@InsurerKey			int
	,@SubsidenceKey		int
	,@NextResetDate		datetime
	,@InitiationFee		decimal(22,10)
	,@QuickCashAmt		decimal(22,10)
	,@QuickCashFee		decimal(22,10)
	,@ProcessDate		datetime
	,@AccountOpenDate	datetime
	,@NCACompliant		varchar(5)
	,@GrantedDate		datetime
	,@HOCMonthlyFee		decimal(22,10)
	,@InstanceID		BigInt
	,@LoanFSKey			int
	,@HOCFSKey			int
	,@HocAccountKey		int
	,@LifeFSKey			int
	,@LifeAccountKey	int
	,@SPVKey			int
	,@MSg				varchar(1024)
	,@R					int
	,@BondKey			int

	SELECT @d_Loan = 0.00, @LoanFSKey=financialservicekey, @SPVKey=a.SPVKey
	from [2am].dbo.Account a 
	join [2am].dbo.financialservice fs on fs.AccountKey=a.AccountKey
	where fs.AccountKey=@LoanNumber and financialServiceTypeKey=1
	
	select @HOCFSKey=fs.FinancialServiceKey, @HocAccountKey=ha.AccountKey
	from [2am].dbo.Account ha
	join [2am].dbo.FinancialService fs on ha.AccountKey=fs.AccountKey
	where ha.ParentAccountKey=@LoanNumber and fs.FinancialServiceTypeKey=4

	select @LifeFSKey=fs.FinancialServiceKey, @LifeAccountKey=ha.AccountKey
	from [2am].dbo.Account ha
	join [2am].dbo.FinancialService fs on ha.AccountKey=fs.AccountKey
	where ha.ParentAccountKey=@LoanNumber and fs.FinancialServiceTypeKey=5
	
	SELECT @d_Loan = @TxnCodeAmt1 + @TxnCodeAmt2 - @TxnCodeAmt3  + @TxnCodeAmt5 + @TxnCodeAmt6 + @TxnCodeAmt7 + @TxnCodeAmt8 + @TxnCodeAmt9 

	Select @AccountOpenDate = convert(varchar(10),OpenDate,111)
	from [2am]..Account (nolock) 
	where AccountKey = @LoanNumber

	Select @ProspectNumber = max(OfferKey) 
	from [2am]..Offer (nolock)
	where AccountKey = @LoanNumber 
	and OfferTypeKey in (6,7,8)
	
	-- SS : Get Offer GrantedDate (#NewOrig)
	Select @GrantedDate = max(TransitionDate)
			from [2am]..StageTransitionComposite (nolock) 
			where GenericKey = @ProspectNumber and StageDefinitionStageDefinitionGroupKey = 2617

	Select @NCACompliant = 'No'

	if (@AccountOpenDate > '2007/06/01' and @GrantedDate < '2007/09/04')	-- Opened Post 1 June
		Select @NCACompliant = 'Yes'
	
	if  @TxnCodeAmt11 <= 0				-- No Val Fee being charged
		Select @NCACompliant = 'Yes'
		
	IF @d_Loan <= 0.00 
	begin
		raiserror('Total Disbursement cannot be be less than or equal to zero.', 16, 1)
	end

	-- Get the Market Rate
	SELECT @d_PeriodRate = 0.00
	/*	Ronnie: 22/06/2004
		Inserted this on management request, to default the rate to 9.5 between a set of dates, it will revert past the end date.*/
	IF getdate() >= convert(datetime,'21/06/2004',103) and getdate() <= convert(datetime,'21/08/2004',103)
	BEGIN
		SELECT @dLoanOpenDate = OpenDate
		FROM 	[2am]..Account with (nolock)
		WHERE Accountkey = @LoanNumber

		IF @dLoanOpenDate < convert(datetime,'21/06/2004',103)
		begin
			SELECT @d_PeriodRate = 0.074
		end
		ELSE
		begin
			select @d_PeriodRate = m.value from 
			[2am].dbo.financialservice fs with (nolock) 
			join [2am].fin.LoanBalance lb on fs.financialservicekey = lb.financialservicekey 
			join [2am].dbo.rateconfiguration rc with (nolock) on lb.RateConfigurationKey = rc.RateConfigurationKey
			join [2am].dbo.marketrate m with (nolock) on m.MarketRateKey = rc.MarketRateKey
			where fs.Accountkey = @LoanNumber 
				and AccountStatusKey = 3			
		end
	END
	ELSE
	BEGIN
			select @d_PeriodRate = m.value from 
			[2am].dbo.financialservice fs with (nolock) 
			join [2am].fin.LoanBalance lb on fs.financialservicekey = lb.financialservicekey 
			join [2am].dbo.rateconfiguration rc with (nolock) on lb.RateConfigurationKey = rc.RateConfigurationKey
			join [2am].dbo.marketrate m with (nolock) on m.MarketRateKey = rc.MarketRateKey
			where fs.Accountkey = @LoanNumber 
				and AccountStatusKey = 3
	END

	IF @d_PeriodRate <= 0.00 
	begin
		raiserror('Market Rate cannot be be less than or equal to zero.', 16, 1)
	end

	-- Link Rate Obtained from Loan Record as varies accroding to SPV
	Select @d_LinkRate = lb.InterestRate - lb.ActiveMarketRate , @ResetConfigurationKey = ResetConfigurationKey
	from [2am]..financialservice fs with (nolock)
	join [2am].fin.LoanBalance lb on fs.FinancialServiceKey=lb.FinancialServiceKey
	where Accountkey = @LoanNumber
	and AccountStatusKey = 3  -- Prospect (This will still get back to the correct link rate)

	IF @d_LinkRate <= 0.00 
	BEGIN 	
		raiserror('Link Rate cannot be be less than or equal to zero.', 16, 1);
	end
		
   	Select @BaseRate = @d_PeriodRate
	SELECT @d_PeriodRate = @d_PeriodRate + @d_LinkRate
	
   	--Update the loan record balance,installment,loanrate,linkrate,OpenDate(Registration Date)
	
	Update [2am].dbo.Account with (rowlock)
	set UserID     = @UserID, 
		ChangeDate = getdate()
	where Accountkey = @LoanNumber
	and AccountStatusKey = 3
	
	-- Sync child SPV to parent's
	Update [2am].dbo.Account with (rowlock)
	set SPVKey = @SPVKey
	where ParentAccountkey = @LoanNumber
	and AccountStatusKey <> 2
	and SPVKey <> @SPVKey
		
	exec @R = process.[fin].[pOpenAccount] @LoanNumber, @Msg output
	if @R <> 0
	begin 
		raiserror(@msg,16,1)	
	end
	
		
	select top 0 * into #Transactions from process.template.Transactions
	exec process.template.pAlterTempTable '#Transactions', 1, ''
	select top 0 * into #TransactionResults from process.template.TransactionResults
	select top 0 * into #Errors from process.template.errors
	SELECT TOP 0 * INTO #FinancialServicesToUpdate FROM Process.template.FinancialServicesToUpdate;
	EXEC Process.template.paltertemptable  '#FinancialServicesToUpdate',   1,   ''; 		
--TRAC 12243 the reset date must show when the reset will occur, ie: the action date, not reset date
	Select @NextResetDate = ActionDate 
	from [2am]..resetconfiguration R (nolock) 
	where ResetConfigurationkey = @ResetConfigurationKey  
	
	Update [2am]..Financialservice with (rowlock)
	set --AccountStatusKey = 1,
	NextResetDate = @NextResetDate
	where Accountkey = @LoanNumber
	--and AccountStatusKey = 3

	Update lb
	set lb.InterestRate = @d_PeriodRate,
		lb.ActiveMarketRate = @BaseRate
	from [2am].dbo.FinancialService fs
	join [2am].fin.LoanBalance lb on fs.FinancialServiceKey=lb.FinancialServiceKey
	where fs.Accountkey = @LoanNumber
	and AccountStatusKey = 1	
	
	update [2am].dbo.FinancialService set OpenDate = @ToDate where AccountKey=@LoanNumber
	-- Guarantee - Loan Amt txncodeamt1
	IF @TxnCodeAmt1 > 0
	BEGIN
		insert into #Transactions (ProcessStatusKey, ErrorMessage, FinancialServiceKey, TransactionTYpeKey, EffectiveDate,
								Amount, Reference, UserID, SPVKey, ReferenceKey)
		select 1, NULL, @LoanFSKey, @TxnCode1, @ToDate, @TxnCodeAmt1, 'Disbursement', @USerID, @SPVKey, @LoanFSKey
	END
	
	-- Paid to Client
	IF @TxnCodeAmt2 > 0
	BEGIN       
		insert into #Transactions (ProcessStatusKey, ErrorMessage, FinancialServiceKey, TransactionTYpeKey, EffectiveDate,
								Amount, Reference, UserID, SPVKey, ReferenceKey)
		select 1, NULL, @LoanFSKey, @TxnCode2, @ToDate, @TxnCodeAmt2, 'Disbursement', @USerID, @SPVKey, @LoanFSKey
	END
		
	--Client Deposit
	--- New Txn Type 399  27/11/2003 (Rob Poley)
	DECLARE @TxnCode399Amt		decimal(22,10)
	DECLARE @PurposeNumber		integer
	--		SS : Read ML purpose from MortgageLoan
	Select @PurposeNumber = ml.MortgageLoanPurposeKey 
	from [2am].dbo.FinancialService fs 
	join [2am].fin.MortgageLoan ML (nolock) on ML.FinancialServiceKey = FS.FinancialServiceKey
	where fs.accountkey = @LoanNumber 
	and fs.FinancialServiceTypeKey = 1

	SELECT  @TxnCode399Amt = 0

	IF @PurposeNumber = 3   --New Purchase
	BEGIN
		if exists (select 1 from [2am].dbo.OfferAttribute where OfferKey = @ProspectNumber and OfferAttributeTYpeKey = 35) -- Capitalise Initiation Fee
			BEGIN 
				-- Deeds Fee + Stamp Duty + Conveyancing Fee + Valuation Fees + QC Interest VAT
				SELECT @TxnCode399Amt =  @TxnCodeAmt9 + @TxnCodeAmt8 +  @TxnCodeAmt6  + @TxnCodeAmt11 + @TxnCodeAmt7 + @TxnCodeAmt12
			END
		ELSE
			BEGIN
				-- Deeds Fee + Stamp Duty + Conveyancing Fee + (Admin Fees - Initiation Fee Discount) + Valuation Fees + QC Interest VAT
				SELECT @TxnCode399Amt =  @TxnCodeAmt9 + @TxnCodeAmt8 +  @TxnCodeAmt6  +  (@TxnCodeAmt10 - @TxnCodeAmt14) + @TxnCodeAmt11 + @TxnCodeAmt7 + @TxnCodeAmt12
			END
	END

	SELECT @TxnCodeAmt3 =  @TxnCodeAmt3 - @TxnCode399Amt

	IF @TxnCodeAmt3 > 0
	BEGIN
		insert into #Transactions (ProcessStatusKey, ErrorMessage, FinancialServiceKey, TransactionTYpeKey, EffectiveDate,
								Amount, Reference, UserID, SPVKey, ReferenceKey)
		select 1, NULL, @LoanFSKey, @TxnCode3, @ToDate, @TxnCodeAmt3, 'Disbursement', @USerID, @SPVKey, @LoanFSKey
	END

	--Transfer Duty ---- Changed to Quick Cash Processing Fee ---- Deanh 09/03/2006
	-- SS : Only post this tran if Loan Opened before 1 June 2007 or is Not NCA Compliant
	if  (@NCACompliant = 'No' and @GrantedDate < '2007/09/04')
	BEGIN
		IF @TxnCodeAmt4 > 0
		BEGIN
			insert into #Transactions (ProcessStatusKey, ErrorMessage, FinancialServiceKey, TransactionTYpeKey, EffectiveDate,
									Amount, Reference, UserID, SPVKey, ReferenceKey)
			select 1, NULL, @LoanFSKey, @TxnCode4, @ToDate, @TxnCodeAmt4, 'Disbursement', @USerID, @SPVKey, @LoanFSKey
		END
	END
	
	IF @TxnCodeAmt5 > 0 
	BEGIN
		insert into #Transactions (ProcessStatusKey, ErrorMessage, FinancialServiceKey, TransactionTYpeKey, EffectiveDate,
								Amount, Reference, UserID, SPVKey, ReferenceKey)
		select 1, NULL, @LoanFSKey, @TxnCode5, @ToDate, @TxnCodeAmt5, 'Disbursement', @USerID, @SPVKey, @LoanFSKey
		
	END

	-- Conveyancing Fee
	IF @TxnCodeAmt6 > 0 
	BEGIN
		insert into #Transactions (ProcessStatusKey, ErrorMessage, FinancialServiceKey, TransactionTYpeKey, EffectiveDate,
								Amount, Reference, UserID, SPVKey, ReferenceKey)
		select 1, NULL, @LoanFSKey, @TxnCode6, @ToDate, @TxnCodeAmt6, 'Disbursement', @USerID, @SPVKey, @LoanFSKey

	END

	-- VAT
	IF @TxnCodeAmt7 > 0 
	BEGIN
		insert into #Transactions (ProcessStatusKey, ErrorMessage, FinancialServiceKey, TransactionTYpeKey, EffectiveDate,
								Amount, Reference, UserID, SPVKey, ReferenceKey)
		select 1, NULL, @LoanFSKey, @TxnCode7, @ToDate, @TxnCodeAmt7, 'Disbursement', @USerID, @SPVKey, @LoanFSKey

	END
	-- Stamp Duty
	IF @TxnCodeAmt8 > 0
	BEGIN
		insert into #Transactions (ProcessStatusKey, ErrorMessage, FinancialServiceKey, TransactionTYpeKey, EffectiveDate,
								Amount, Reference, UserID, SPVKey, ReferenceKey)
		select 1, NULL, @LoanFSKey, @TxnCode8, @ToDate, @TxnCodeAmt8, 'Disbursement', @USerID, @SPVKey, @LoanFSKey
	END   
	-- Deeds Fee  
	IF @TxnCodeAmt9 > 0 
	BEGIN
		insert into #Transactions (ProcessStatusKey, ErrorMessage, FinancialServiceKey, TransactionTYpeKey, EffectiveDate,
								Amount, Reference, UserID, SPVKey, ReferenceKey)
		select 1, NULL, @LoanFSKey, @TxnCode9, @ToDate, @TxnCodeAmt9, 'Disbursement', @USerID, @SPVKey, @LoanFSKey

	END

	-- Admin Fees
	IF @TxnCodeAmt10 > 0 
	BEGIN
		insert into #Transactions (ProcessStatusKey, ErrorMessage, FinancialServiceKey, TransactionTYpeKey, EffectiveDate,
								Amount, Reference, UserID, SPVKey, ReferenceKey)
		select 1, NULL, @LoanFSKey, @TxnCode10, @ToDate, @TxnCodeAmt10, 'Disbursement', @USerID, @SPVKey, @LoanFSKey

		--Initiation Fee Discount – Returning Client
		IF @TxnCodeAmt14 > 0    
		BEGIN
			insert into #Transactions (ProcessStatusKey, ErrorMessage, FinancialServiceKey, TransactionTYpeKey, EffectiveDate,
									Amount, Reference, UserID, SPVKey, ReferenceKey)
			select 1, NULL, @LoanFSKey, @TxnCode14, @ToDate, @TxnCodeAmt14, 'Disbursement', @USerID, @SPVKey, @LoanFSKey
		END
	END


	IF  (@GrantedDate < '2007/09/04') -- Not Applicable after GO LIVE Date
	BEGIN
		IF (@AccountOpenDate > '2007/06/01' or @NCACompliant = 'Yes') -- This only applies post 1 June 07 or NCA Compliant Loans
		BEGIN
			IF @TxnCodeAmt4 > 0 -- Post Memo for QC Proc fee
			BEGIN
				insert into #Transactions (ProcessStatusKey, ErrorMessage, FinancialServiceKey, TransactionTYpeKey, EffectiveDate,
										Amount, Reference, UserID, SPVKey, ReferenceKey)
				select 1, NULL, @LoanFSKey, 925, getdate(), 0, 'Disbursement-Memo', @USerID, @SPVKey, @LoanFSKey
			end
		END
	END -- END : IF  (@GrantedDate < '2007/08/15')	

	--Valuation Fees
	IF @TxnCodeAmt11 > 0    
	BEGIN
		insert into #Transactions (ProcessStatusKey, ErrorMessage, FinancialServiceKey, TransactionTYpeKey, EffectiveDate,
								Amount, Reference, UserID, SPVKey, ReferenceKey)
		select 1, NULL, @LoanFSKey, @TxnCode11, @ToDate, @TxnCodeAmt11, 'Disbursement', @USerID, @SPVKey, @LoanFSKey
		
	END

	--Interest On Quick Cash Advance -------- Deanh 03/09/2006
	IF @TxnCodeAmt12 > 0    
	BEGIN
		insert into #Transactions (ProcessStatusKey, ErrorMessage, FinancialServiceKey, TransactionTYpeKey, EffectiveDate,
								Amount, Reference, UserID, SPVKey, ReferenceKey)
		select 1, NULL, @LoanFSKey, @TxnCode12, @ToDate, @TxnCodeAmt12, 'Disbursement', @USerID, @SPVKey, @LoanFSKey
	END
	--- New Txn Type 399  27/11/2003 (Rob Poley)
	IF @PurposeNumber = 3   --New Purchase
	BEGIN
		IF @TxnCode399Amt > 0    
		BEGIN
			insert into #Transactions (ProcessStatusKey, ErrorMessage, FinancialServiceKey, TransactionTYpeKey, EffectiveDate,
									Amount, Reference, UserID, SPVKey, ReferenceKey)
			select 1, NULL, @LoanFSKey, 399, @ToDate, @TxnCode399Amt, 'Disbursement', @USerID, @SPVKey, @LoanFSKey			
		END
	END
	ELSE IF @PurposeNumber IN ( 2 , 4  )         -- Switch Loan , Refinance  NOTE : Further Loans Txn Type 399 is dealt with in f_GenerateFurtherLoanTxns
	BEGIN
		-- Get the Detail Type Number 339  if any .............NOTE : DETAILTYPENUMBER IS 339 NOT 399 ..... This detail type is deleted later  in this stored proc....
		SELECT  @TxnCode399Amt = Amount
		FROM [2am]..DETAIL with (nolock)
		WHERE 	Accountkey = @LoanNumber 
		AND DetailTypeKey 	= 	339	

		IF @TxnCode399Amt > 0    
		BEGIN
			insert into #Transactions (ProcessStatusKey, ErrorMessage, FinancialServiceKey, TransactionTYpeKey, EffectiveDate,
									Amount, Reference, UserID, SPVKey, ReferenceKey)
			select 1, NULL, @LoanFSKey, 399, @ToDate, @TxnCode399Amt, 'Disbursement', @USerID, @SPVKey, @LoanFSKey						
		END
	END
	
	exec @R = process.fin.pProcessTran @msg output
	if @R <> 0 or (select count(1) from #Transactions where ProcessStatusKey in (3,4)) > 0
	begin
		--select * from #Transactions
		raiserror('Error posting transactions', 16, 1)
	end
	
	-- Rate Change and Installment Amount
	delete from #FinancialServicesToUpdate

	INSERT INTO #FinancialServicesToUpdate (ProcessStatusKey, ErrorMessage, FinancialServiceKey,Reference ,UserID, InstalmentAmount) 
	select 1, 1, @LoanFSKey,'Disbursement',@UserID, 0

	EXEC @R = process.fin.pUpdateInterestRate 0,@Msg OUTPUT
	if @R <> 0 
	begin
		raiserror(@msg, 16, 1)
	end

	-- update initial balance
	update lb 
	set InitialBalance = b.Amount
	from [2am].fin.LoanBalance lb
	join [2am].fin.Balance b on lb.FinancialServiceKey=b.FinancialServiceKey
	where b.FinancialServiceKey=@LoanFSKey
	
	-- insert bond record
	INSERT INTO [2AM]..Bond(DeedsOfficeKey,AttorneyKey, BondRegistrationNumber,BondRegistrationDate, 
  								BondRegistrationAmount ,BondLoanAgreementAmount,UserID,ChangeDate,OfferKey)        
	VALUES(@DeedsOfficeNbr, @AttorneyNbr, '', @ToDate, @RegistrationAmt, @LoanAgreementAmt, @UserID, GetDate(), @Prospectnumber)
  	
	Set @BondKey = SCOPE_IDENTITY()

	INSERT INTO [2am]..BondMortgageLoan(FinancialServicekey, BondKey)
	VALUES(@LoanFSKey, @BondKey)

	INSERT  INTO [2AM]..LoanAgreement(AgreementDate, Amount, UserName, BondKey, ChangeDate) 
	Values(GetDate(), @LoanAgreementAmt, @UserID, @BondKey, GetDate()) 

	Update [2am]..Offer with (rowlock) 
	Set OfferEndDate = getdate()
	where OfferKey = @ProspectNumber

	/******************************************************************* CREATE FLAG FOR GEPF LOAN ***********************************************************************/	

	-- Only insert record when an attribute is created against GEPF and a SWD employment type and credit matrix key >= 51
	INSERT INTO [2AM].dbo.AccountInformation (AccountInformationTypeKey, AccountKey, EntryDate, Amount, Information)		
	SELECT	DISTINCT (SELECT AccountInformationTypeKey FROM [2am].dbo.AccountInformationType  WHERE Description LIKE 'Government Employee Pension Fund'), @LoanNumber, GETDATE(),NULL,NULL
	FROM	[2am].dbo.OfferAttribute oa (NOLOCK) 
	WHERE	oa.OfferAttributeTypeKey = 36 -- GEPF
	AND		oa.OfferKey = @ProspectNumber

	/******************************************************************* CREATE FLAG FOR STOP ORDER DISCOUNT ELIGIBLE ***********************************************************************/	

	-- Only insert record when an attribute is created against a SWD employment type and credit matrix key >= 51
	INSERT INTO [2AM].dbo.AccountInformation (AccountInformationTypeKey, AccountKey, EntryDate, Amount, Information)		
	SELECT	DISTINCT (SELECT AccountInformationTypeKey FROM [2am].dbo.AccountInformationType  WHERE Description LIKE 'Stop Order Discount Eligible'), @LoanNumber, GETDATE(),NULL,NULL
	FROM	[2am].dbo.OfferAttribute oa (NOLOCK) 
	WHERE	oa.OfferAttributeTypeKey = 37 --Stop Order Discount
	AND		oa.OfferKey = @ProspectNumber


	/*******************************************************************************************************************************************************************************/

	-------- SS : No Monthly fee is loan is a resub with detail type 574 ------------------
	if exists (Select * from detail(nolock) where LoanNumber = @LoanNumber and DetailTypeNumber in (574,237))
	Begin
		Delete from Detail where LoanNumber = @LoanNumber and DetailTypeNumber = 574
	End
	else
	Begin
		if @GrantedDate >= '2007/08/02'
		Begin
			exec process.fin.pCreateMonthlyFeeTran @LoanFSKey, @LifeAccountKey
		End	
	End		

	if @HOCFSKey is not null
	begin
		----------------- Begin Creation /Update of HOC Record ------------------------
		--Get required values off HOC table
		SELECT		@FinancialServiceKey =  FS.FinancialServicekey, 
			@HOCPolicyNumber = FS.AccountKey,
			@HOCThatchAmount = h.HOCThatchAmount,
			@HOCConventionalAmount = h.HOCConventionalAmount,
			@HOCShingleAmount = h.HOCShingleAmount,
			@InsurerKey = h.HOCInsurerKey,
			@SubsidenceKey = h.HOCSubsidenceKey
		from [2am]..account ACR (nolock)  
		inner join [2am]..financialservice FS (nolock) on ACR.AccountKey = FS.AccountKey  
		inner join [2am]..HOC h (nolock) on fs.FinancialServicekey = h.FinancialServicekey
		where FinancialServiceTypeKey = 4 
		and ACR.ParentAccountKey = @LoanNumber
		  
		if @InsurerKey = 2
		begin
			-- Calculate HOC premiums
			SELECT TOP 0 * INTO #HocMonthlyPremium FROM process.template.HocMonthlyPremium
			SELECT TOP 0 * INTO #HocProRataPremium FROM process.template.HocProRataPremium
			
			INSERT INTO #hocmonthlypremium 
				(
					financialservicekey, 
					InsurerKey, 
					SubsidenceKey, 
					thatchamount,
					conventionalamount
				)
			values 
				(
					@HOCFSKey, 
					@InsurerKey, 
					@SubsidenceKey, 
					@HOCThatchAmount, 
					@HOCConventionalAmount
				)					 
			
			EXEC @R = process.hoc.pMonthlyPremium @msg OUTPUT
			
			if @R <> 0
			begin
				raiserror(@msg,16,1)
			end

			Select	@HOCPremium = HOCMonthlyPremium,
					@HOCBasePremium = HOCBasePremium, 
					@HOCSASRIAAmount = SASRIAAmount,
					@HOCMonthlyFee = HOCAdminFee
			from #hocmonthlypremium --SELECT * FROM process.template.hocmonthlypremium
			
			-- Pass HOC base premium to calc prorata
			INSERT INTO #HocProRataPremium (FinancialServiceKey, InsurerKey, HOCMonthlyPremium)
			values (@HOCFSKey, @InsurerKey, @HOCBasePremium)

			EXEC @R = process.hoc.pProrataPremium @msg OUTPUT  
			if @R <> 0
			begin
				raiserror(@msg,16,1)
			end
			
			Select @HOCProRataPremium = HOCProrataPremium
			from #HocProRataPremium
			
			--Update HOC Record				
			Update [2am].dbo.HOC with (rowlock) Set
					HOCMonthlyPremium = isnull(@HOCPremium,0),
					HOCProRatapremium = isnull(@HOCProRataPremium,0),	
					CommencementDate = @ToDate,
					AnniversaryDate = DateAdd(year,1,DateAdd(month,1,replace(convert(char(10),dateadd(d,-datepart(d,@ToDate)+1, @ToDate),110),'-','/'))),
					HOCStatusKey = 1,	
					ChangeDate = getdate(),
					HOCAdministrationFee = abs(@HOCMonthlyFee)*-1,
					HOCBasePremium = @HOCBasePremium, 
					SASRIAAmount = @HOCSASRIAAmount
			Where FinancialServiceKey = @HOCFSKey 
				and HOCInsurerkey = 2
				
			Update [2am]..FinancialService with (rowlock) 
			Set Payment = 0, AccountStatusKey = 1
			where FinancialServiceKey = @HOCFSKey
				
		end
		else
		begin 
			Update [2am]..FinancialService with (rowlock) 
			Set Payment = 0, 
				AccountStatusKey = 1
			where FinancialServiceKey = @HOCFSKey
		end

		INSERT INTO [2am]..Role (LegalEntityKey, AccountKey,  RoleTypeKey , GeneralStatusKey, StatusChangeDate)
		select r.LegalEntityKey, h.AccountKey, r.RoleTypeKey, r.GeneralStatusKey, getdate()
		from [2am]..account h (nolock) 
		join [2am]..account l (nolock) on l.AccountKey=h.ParentAccountKey
		join [2am]..role r (nolock) on l.accountkey = r.accountkey and RoleTypeKey = 2
		where h.RRR_ProductKey = 3	
		and h.AccountKey not in (select AccountKey from [2am]..role (nolock))
		and H.AccountKey = @HOCPolicyNumber

		----------- Update History Records with Commencement Date -------------------		
		Select	@HOCHistoryKey		= isnull(HOCHistoryKey,0) ,
		@HOCThatchAmount	= HOCThatchAmount,
		@HOCConventionalAmount = HOCConventionalAmount,
		@HOCShingleAmount	=	HOCShingleAmount,
		@HOCBasePremium = HOCBasePremium, 
		@HOCSASRIAAmount = SASRIAAmount

		from [2am]..HOC (nolock) 
		where FinancialServiceKey = @FinancialServiceKey and HOCInsurerKey = 2

		IF isnull(@HOCHistoryKey,0) > 0
		Begin
			Update [2am]..HOCHistory with (rowlock) set CommencementDate = @Todate,ChangeDate = getdate()
			Where HOCHistoryKey = @HOCHistoryKey and HOCInsurerKey = 2

			Insert into [2am]..HOCHistoryDetail
							(
							HOCHistoryKey,
							EffectiveDate,
							UpdateType,
							HOCThatchAmount,
							HOCConventionalAmount,
							HOCShingleAmount,
							HOCProrataPremium,
							HOCMonthlyPremium,
							ChangeDate,
							UserID,
							HOCAdministrationFee,
							HOCBasePremium,
							SASRIAAmount
							)	
			values
							(
							@HOCHistoryKey,
							getdate(),
							'I',
							@HOCThatchAmount,
							@HOCConventionalAmount,
							@HOCShingleAmount,
							isnull(@HOCProRataPremium,0),
							isnull(@HOCPremium,0),
							getdate(),
							'Disbursements',
							abs(@HOCMonthlyFee)*-1,
							@HOCBasePremium, 
							@HOCSASRIAAmount
							)
		End

		Update [2am]..Account with (rowlock)
			set AccountStatusKey = 1,OpenDate = @ToDate
		where AccountKey = @HOCPolicyNumber
			and AccountStatusKey <> 1
		---------------------- End of HOC ---------------------------------------------
	end

	-- Ensure REGMAIL is set to Registration Received
	UPDATE REGMAIL with (rowlock) SET 	DetailTypeNumber= 6  ,RegMailDateTime= GETDATE()
	WHERE LoanNumber = @LoanNumber 
	AND ( DetailTypeNumber 	<= 	9  OR
		  DetailTypeNumber 	= 	343 		
		 )

	-- Update the  Loans Guarantee to Paid Away on Disbursment if a Guaranteee exists for the loan
	UPDATE [2am]..GUARANTEE with (rowlock) SET StatusNumber = 2 
	WHERE Accountkey = @LoanNumber

	--- Finally delete all registration type data from detail table for this loan 
	-- Change on 27/11/2003....... Delete Detail Type Number 339 is any,....
	DELETE FROM [2am]..DETAIL 
	WHERE 	Accountkey 		= 	@LoanNumber 	AND
	( DetailTypeKey 	<= 	9 		OR 
	DetailTypeKey 	= 	343 		OR 
	DetailTypeKey	=	339 )
--select '00 in these procs'
	IF @PurposeNumber IN ( 2, 3) 
	BEGIN
		-- Update the  Final Guarantee Values in the Disbursment Table if any ( i.e Disbursements with Guarnatee Interets Only)  (10/12/2003)
		Exec r_SetFinalGuaranteeFigures @LoanNumber
		Exec r_SetFinalCashGuaranteeFigures @LoanNumber
	END
--select '00 end in these procs'
	--		Update X2 to tell Workflow the Loan has been Disbursed
	select @InstanceID=(select top 1 InstanceID from x2.x2data.application_Management (nolock)
						where ApplicationKey=@ProspectNumber order by InstanceID desc)
	if (@InstanceID <> null)
	Begin
		exec [X2].[dbo].[pr_PipeLineDisburse] @InstanceID
	end
	  
	-- Added by Gary Daniell. Set origination source product to New variable if it is getting disbursed as VariFix.
	if ((select count(AccountKey) from [2am].dbo.Account where AccountKey = @LoanNumber and RRR_ProductKey = 2) = 1)
	BEGIN
		UPDATE [2am].dbo.Account set OriginationSourceProductKey = 17
		where AccountKey = @LoanNumber
	END

	-- Added by AdriaanP, Detail insert from OfferAttribute ( 'HOC - No HOC' and 'HOC - Cession Of Policy')
	INSERT INTO [2AM].[dbo].[Detail]([DetailTypeKey],[AccountKey],[DetailDate]           ,[Description],[ChangeDate])
	select top 1 12,@LoanNumber,getdate(),convert(varchar(150), oa.OfferKey),getdate()
	from [2AM]..OfferAttribute oa
	where oa.OfferKey = @ProspectNumber
	and oa.OfferAttributeTypeKey  = 23
	
	if exists (select * from [2am].dbo.OfferAttribute where OfferKey=@ProspectNumber and OfferAttributeTYpeKey=26)
	begin
		insert into [2am].dbo.Detail (DetailTypeKey, AccountKey, DetailDate, Description, ChangeDate)
		select 599, @LoanNumber, getdate(), 'Alpha Housing', getdate()
	end

	INSERT INTO [2AM].[dbo].[Detail]([DetailTypeKey],[AccountKey],[DetailDate]           ,[Description],[ChangeDate])
	select top 1 13,@LoanNumber,getdate(),convert(varchar(150), oa.OfferKey),getdate()
	from [2AM]..OfferAttribute oa
	where oa.OfferKey = @ProspectNumber
	and oa.OfferAttributeTypeKey  = 24	  
	
	exec process.lighthouse.pProcessPendingDomiciliumAddress @ProspectNumber, @msg OUTPUT
	
	-- Added by adriaanP ##11027. update offerstatus and set offerenddate.
	update [2AM]..Offer 
	set OfferStatusKey = 3, 
		OfferEndDate = getdate()
	where OfferKey = @ProspectNumber;
	
	
	
	commit
	
	select ''
end try

begin catch
	if @@TRANCOUNT > 0
		rollback
		
	select ISNULL(error_message(), ' Failed')
end catch
end



