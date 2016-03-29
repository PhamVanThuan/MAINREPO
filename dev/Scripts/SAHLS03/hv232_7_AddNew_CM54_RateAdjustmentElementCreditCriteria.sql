use [2am]

insert into [2am].dbo.RateAdjustmentElementCreditCriteria
select rae.RateAdjustmentElementKey,cc.CreditCriteriaKey
from 
	creditcriteria cc
join 
	category c 
on 
	c.CategoryKey=cc.CategoryKey
join 
	employmenttype et 
on 
	et.EmploymentTypeKey=cc.EmploymentTypeKey
join 
	rateadjustmentelement rae 
on 
	rae.Description like 'CM'+cast(cc.CreditMatrixKey as varchar)+ ' '+c.Description+ ' '+ (case when et.Description = 'Salaried with Deduction' then 'SWD' else et.Description end)+'%'
left join
	RateAdjustmentElementCreditCriteria raecc
on
	raecc.RateAdjustmentElementKey = rae.RateAdjustmentElementKey and cc.CreditCriteriaKey = raecc.CreditCriteriaKey
where 
	cc.creditmatrixkey=54
	and raecc.RateAdjustmentElementKey is null
