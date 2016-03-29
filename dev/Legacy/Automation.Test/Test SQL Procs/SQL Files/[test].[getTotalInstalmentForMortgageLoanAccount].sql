
USE [2AM]
GO

IF  EXISTS (SELECT * FROM sys.objects  WHERE object_id = OBJECT_ID(N'[test].[getTotalInstalmentForAccount]') 
            AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION test.GetTotalInstalmentForAccount
END
GO      
/****** Object:  UserDefinedFunction [test].[getTotalInstalmentForAccount]   Script Date: 01/28/2012 13:34:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION test.getTotalInstalmentForAccount(
	@AccountKey int
)
RETURNS @AccountInstalment table(AccountKey int, Instalment float)
AS

begin

	insert into @AccountInstalment
	select @AccountKey, round(isnull(dbo.fGetHOCMonthlyPremium(a_h.accountKey),0) + isnull([Process].[halo].[fLifeGetMonthlyPremium] (a_l.accountKey),0) + isnull(f.Amount,0) + isnull(loan_payment.pmt,0), 2) as Instalment
	 from [2am].dbo.account a
	 join [2am].dbo.financialservice fs_loan on a.accountkey=fs_loan.accountkey
		and fs_loan.financialservicetypekey in (1,10,11)
	 left join [2am].dbo.account a_h on a.AccountKey = a_h.parentAccountKey
		and a_h.rrr_productkey=3 
		and a_h.accountStatusKey = 1
	left join [2am].dbo.financialservice fs on a_h.accountkey=fs.accountkey
		and fs.financialservicetypekey = 4
	left join [2am].dbo.account a_l on a.accountKey = a_l.parentAccountKey
		and a_l.rrr_productkey=4 
		and a_l.accountStatusKey = 1
	left join dbo.financialservice fs_l on a_l.accountkey=fs_l.accountkey
		and fs_l.financialservicetypekey = 5
	left join [2am].fin.Fee f on fs_loan.financialservicekey = f.financialServiceKey
	join (select sum(fs.payment) as Pmt, fs.accountKey from [2am].dbo.financialservice fs 
	where fs.accountKey = @AccountKey
	group by fs.accountKey) as loan_payment on a.accountKey = loan_payment.accountKey
	where a.accountkey = @AccountKey order by a_h.opendate desc

	RETURN

end

GO






