USE [2AM];
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO
IF EXISTS(SELECT table_name, column_name, data_type, character_maximum_length    
  FROM information_schema.columns  
  WHERE table_name = 'ThirdPartyPaymentBankAccount' and column_name = 'BeneficiaryBankCode' and character_maximum_length = 5)
BEGIN
ALTER TABLE ThirdPartyPaymentBankAccount ALTER COLUMN BeneficiaryBankCode VARCHAR(20)
END
GO


	Declare @attorneys_bank table
(
 TradingName nvarchar(50),
 Bank nvarchar(50),
 AccountNumber nvarchar(50),
 BranchCode nvarchar(50),
 Email varchar(50),
 ThirdPartyId uniqueidentifier,
 ThirdPartyKey int,
 BankAccountKey int,
 BeneficiaryBankCode varchar(20),
 AttorneyKey int
)

BEGIN TRANSACTION;
INSERT INTO @attorneys_bank([TradingName], [Bank], [AccountNumber], [BranchCode], [Email], [ThirdPartyKey], [ThirdPartyId], [BankAccountKey], [BeneficiaryBankCode], [AttorneyKey])
SELECT N'Herold Gie', N'NEDBANK LIMITED', N'1009014145', N'100909', N'gnackerdien@heroldgie.co.za', null, null, null, 'HEROLD', 27 UNION ALL
SELECT N'Klagsbrun Edelstein Bosman De Vries Inc (KEBD) PTA', N'First National Bank', N'62451715571', N'222026', N'marisa@kebd.co.za', null, null, null, 'KEBD', 39 UNION ALL
SELECT N'Klagsbrun Edelstein Bosman De Vries Inc (KEBD) PTA', N'First National Bank', N'62451715571', N'222026', N'marisa@kebd.co.za', null, null, null, 'KEBD', 3 UNION ALL
SELECT N'Mcintyre & Van Der Post', N'STANDARD BANK', N'041073355', N'055034', N'leanda@mcintyre.co.za', null, null, null, 'MCINTYRE', 46 UNION ALL
SELECT N'Moodie And Robertson', N'NEDBANK LIMITED', N'1950418553', N'195005', N'LindaL@riv.moodrob.co.za', null, null, null, 'MOODIE', 2 UNION ALL
SELECT N'Poswa Inc', N'ABSA', N'4074961884', N'632005', N'Ntompela@poswainc.co.za', null, null, null, 'POSWA', 80 UNION ALL
SELECT N'Poswa Inc', N'ABSA', N'4074961884', N'632005', N'Ntompela@poswainc.co.za', null, null, null, 'POSWA', 74 UNION ALL
SELECT N'Poswa Inc', N'ABSA', N'4074961884', N'632005', N'Ntompela@poswainc.co.za', null, null, null, 'POSWA', 59 UNION ALL
SELECT N'Randles Inc', N'First National Bank', N'50930027327', N'257355', N'nomusa@randles.co.za', null, null, null, 'RANDLES', 1 UNION ALL
SELECT N'Shepstone & Wylie (Durban)', N'STANDARD BANK', N'050281690', N'040026', N'amos@wylie.co.za', null, null, null, 'SHEPSTONE', 57 UNION ALL
SELECT N'Shepstone & Wylie (Durban)', N'STANDARD BANK', N'050281690', N'040026', N'amos@wylie.co.za', null, null, null, 'SHEPSTONE', 49 UNION ALL
SELECT N'Shepstone & Wylie (Durban)', N'STANDARD BANK', N'050281690', N'040026', N'amos@wylie.co.za', null, null, null, 'SHEPSTONE', 31 UNION ALL
SELECT N'Strauss Daly', N'STANDARD BANK', N'050220748', N'040026', N'SChetty@straussdaly.co.za', null, null, null, 'DALY ', 28 UNION ALL
SELECT N'Strauss Daly (Johannesburg Branch)', N'ABSA', N'4053763681', N'331926', N'lacker@straussdaly.co.za', null, null, null, 'DALYJHB', 33 UNION ALL
SELECT N'Strauss Daly (Western Cape) Inc.', N'STANDARD BANK', N'070404356', N'020009', N'lynda@bsdi.co.za', null, null, null, 'BALSILLIES', 50 UNION ALL
SELECT N'Strauss Daly (Western Cape) Inc.', N'STANDARD BANK', N'070404356', N'020009', N'lynda@bsdi.co.za', null, null, null, 'BALSILLIES', 47 UNION ALL
SELECT N'Strauss Daly (Western Cape) Inc.', N'STANDARD BANK', N'070404356', N'020009', N'lynda@bsdi.co.za', null, null, null, 'BALSILLIES', 43 UNION ALL
SELECT N'Velile Tinto & Ass (Pretoria)', N'ABSA', N'4057366261', N'632005', N'melanie@tintolaw.co.za', null, null, null, 'VELILE', 22 UNION ALL
SELECT N'Velile Tinto & Ass (Pretoria)', N'ABSA', N'4057366261', N'632005', N'melanie@tintolaw.co.za', null, null, null, 'VELILE', 63 UNION ALL
SELECT N'Velile Tinto & Ass (Pretoria)', N'ABSA', N'4057366261', N'632005', N'melanie@tintolaw.co.za', null, null, null, 'VELILE', 58 UNION ALL
SELECT N'Velile Tinto & Ass (Pretoria)', N'ABSA', N'4057366261', N'632005', N'melanie@tintolaw.co.za', null, null, null, 'VELILE', 42 UNION ALL
SELECT N'Velile Tinto & Ass (Pretoria)', N'ABSA', N'4057366261', N'632005', N'melanie@tintolaw.co.za', null, null, null, 'VELILE', 36 UNION ALL
SELECT N'Cliffe Dekker Hofmeyr Inc.', N'NEDBANK LIMITED', N'070268754', N'020009', N'Samantha.Dharmudas@dlacdh.com', null, null, null, 'CDH', 21
COMMIT;

IF NOT EXISTS (SELECT TOP 1 *  FROM [2AM].[dbo].[BankAccount]  where [AccountNumber] = '041073355')
BEGIN
	INSERT INTO [dbo].[BankAccount]
           ([ACBBranchCode]
           ,[AccountNumber]
           ,[ACBTypeNumber]
           ,[AccountName]
           ,[UserID]
           ,[ChangeDate])
     VALUES
           ('055034'
           ,'041073355'
           ,1
           ,'Mcintyre & Van Der Post'
           ,null
           ,null)
END


--update thirdParty Id and Key
update tb
set 
	tb.ThirdPartyId = t.Id,
	tb.ThirdPartyKey = t.ThirdPartyKey
from 
	@attorneys_bank tb
join 
	[2AM].[dbo].LegalEntity l on l.TradingName = tb.TradingName
join 
	[2AM].[dbo].[ThirdParty] t on t.GenericKey = tb.AttorneyKey and t.GenericKeyTypeKey = 35

--update bankAccountKey
update b
set 
	b.BankAccountKey = bk.BankAccountKey
from 
	@attorneys_bank b
join 
	[2AM].[dbo].BankAccount bk on bk.AccountNumber = b.AccountNumber
	and 
	bk.ACBBranchCode = b.BranchCode

--insert into ThirdPartyPaymentBankAccount
Declare 
	@ThirdPartyId uniqueidentifier, @ThirdPartyKey int, @BankAccountKey int, @Email varchar(50), @BeneficiaryBankCode varchar(20)

Declare 
	insert_cursor Cursor For 
	select ThirdPartyId, ThirdPartyKey, BankAccountKey, Email, BeneficiaryBankCode from @attorneys_bank where BankAccountKey is not null
 
Open insert_cursor
	Fetch Next From insert_cursor into @ThirdPartyId, @ThirdPartyKey, @BankAccountKey, @Email, @BeneficiaryBankCode
 
While @@FETCH_STATUS=0
Begin
	
	if not exists (select * from ThirdPartyPaymentBankAccount where Id = @ThirdPartyId)
	begin
		Insert into ThirdPartyPaymentBankAccount(Id, BankAccountKey, ThirdPartyKey, GeneralStatusKey, BeneficiaryBankCode, EmailAddress)
		Select @ThirdPartyId,  @BankAccountKey,@ThirdPartyKey,1,@BeneficiaryBankCode, @Email
	end
	
	Fetch Next From insert_cursor into @ThirdPartyId, @ThirdPartyKey, @BankAccountKey, @Email, @BeneficiaryBankCode
END
close insert_cursor
Deallocate insert_cursor



