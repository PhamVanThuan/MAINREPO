use [2AM]

if exists(select 1 from creditcriteria where creditmatrixkey=54) 
begin
	--Deactivate Old Credit matrix
	update [2am].dbo.CreditMatrix set NewBusinessIndicator='N' where CreditMatrixKey=53

	--Activate New Credit matrix
	update [2am].dbo.CreditMatrix set NewBusinessIndicator='Y' where CreditMatrixKey=54

	--Activate all originationsource products for new SAHL credit matrix
	update [2am].dbo.OriginationSourceProductCreditMatrix set CreditMatrixKey=54 where CreditMatrixKey=53
end
else
begin
	--Keep Old Credit matrix active
	update [2am].dbo.CreditMatrix set NewBusinessIndicator='Y' where CreditMatrixKey=53
end
