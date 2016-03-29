use [2AM]

--Create new matrix
set identity_insert [2am].dbo.CreditMatrix on

if not exists (select 1 from [2am].dbo.CreditMatrix where CreditMatrixKey=54)
	insert into [2am].dbo.CreditMatrix (CreditMatrixKey, NewBusinessIndicator, ImplementationDate)
		select 54 CreditMatrixKey, 'N' NewBusinessIndicator, getdate() ImplementationDate

set identity_insert [2am].dbo.CreditMatrix off
