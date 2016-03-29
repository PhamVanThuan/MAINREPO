use [2AM]
go

delete
	cc
from 
	CreditMatrix cm
join 
	CreditCriteria cc
on
	cm.CreditMatrixKey = cc.CreditMatrixKey
where
	cm.CreditMatrixKey = 54
	and cc.CategoryKey = 2
	and cc.EmploymentTypeKey = 2

