use [2am]

update [2am].dbo.RateAdjustmentElement set GeneralStatusKey=2 where RateAdjustmentElementKey in (select RateAdjustmentElementKey from [2am].dbo.RateAdjustmentElement where GeneralStatusKey=1 and RateAdjustmentGroupKey=1 and Description not like 'CM54 %')

if not exists (select 1 from [2am].dbo.RateAdjustmentElement where GeneralStatusKey=1 and RateAdjustmentGroupKey=1 and Description like 'CM54 %')
begin
	insert into [2am].dbo.RateAdjustmentElement
	 select * from (select	0	ElementMinValue,595	ElementMaxValue,'' ElementText,0.004 RateAdjustmentValue, getdate() EffectiveDate, 'CM54 Category 0 Salaried [Emp 575-595]' Description,1 RateAdjustmentGroupKey,48	RateAdjustmentElementTypeKey,2	GenericKeyTypeKey,1	GeneralStatusKey,51391	RuleItemKey,14	FinancialAdjustmentTypeSourceKey union
					select	2750000	,10000000		,	''	,	0.005	,	getdate()	,	'CM54 Category 0 Salaried [LAA > R2,750,000]'	,	1	,	49	,	2	,	1	,	51391	,	14	union
					select	0	,	600	,	''	,	0.004	,	getdate()	,	'CM54 Category 0 Salaried [Emp 575-600 Capitec]'	,	1	,	50	,	2	,	1	,	51391	,	14	union
					select	0	,	595	,	''	,	0.004	,	getdate()	,	'CM54 Category 1 Salaried [Emp 575-595]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	600	,	''	,	0.004	,	getdate()	,	'CM54 Category 1 Salaried [Emp 575-600 Capitec]'	,	1	,	50	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.007	,	getdate()	,	'CM54 Category 2 Salaried [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	595	,	609	,	''	,	0.004	,	getdate()	,	'CM54 Category 2 Salaried [Emp 595-609]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	610	,	629	,	''	,	0.002	,	getdate()	,	'CM54 Category 2 Salaried [Emp 610-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	629	,	''	,	0.002	,	getdate()	,	'CM54 Category 3 Salaried [Emp 595-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	629	,	''	,	0.003	,	getdate()	,	'CM54 Category 4 Salaried [Emp 595-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.009	,	getdate()	,	'CM54 Category 6 Salaried [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	595	,	609	,	''	,	0.006	,	getdate()	,	'CM54 Category 6 Salaried [Emp 595-609]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	610	,	629	,	''	,	0.004	,	getdate()	,	'CM54 Category 6 Salaried [Emp 610-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	630	,	649	,	''	,	0.002	,	getdate()	,	'CM54 Category 6 Salaried [Emp 630-649]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.01	,	getdate()	,	'CM54 Category 7 Salaried [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	595	,	609	,	''	,	0.007	,	getdate()	,	'CM54 Category 7 Salaried [Emp 595-609]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	610	,	629	,	''	,	0.004	,	getdate()	,	'CM54 Category 7 Salaried [Emp 610-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	630	,	649	,	''	,	0.002	,	getdate()	,	'CM54 Category 7 Salaried [Emp 630-649]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.013	,	getdate()	,	'CM54 Category 8 Salaried [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	595	,	609	,	''	,	0.008	,	getdate()	,	'CM54 Category 8 Salaried [Emp 595-609]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	610	,	629	,	''	,	0.005	,	getdate()	,	'CM54 Category 8 Salaried [Emp 610-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	630	,	649	,	''	,	0.002	,	getdate()	,	'CM54 Category 8 Salaried [Emp 630-649]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.013	,	getdate()	,	'CM54 Category 9 Salaried [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	595	,	609	,	''	,	0.008	,	getdate()	,	'CM54 Category 9 Salaried [Emp 595-609]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	610	,	629	,	''	,	0.005	,	getdate()	,	'CM54 Category 9 Salaried [Emp 610-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	630	,	649	,	''	,	0.002	,	getdate()	,	'CM54 Category 9 Salaried [Emp 630-649]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	640	,	649	,	''	,	0.003	,	getdate()	,	'CM54 Category 11 Salaried [Emp 640-649]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	1800001	,	10000000	,	''	,	0.003	,	getdate()	,	'CM54 Category 0 Self Employed [LAA > R1,800,000]'	,	1	,	49	,	2	,	1	,	51391	,	14	union
					select	1800001	,	10000000	,	''	,	0.003	,	getdate()	,	'CM54 Category 1 Self Employed [LAA > R1,800,000]'	,	1	,	49	,	2	,	1	,	51391	,	14	union
					select	0	,	595	,	''	,	0.004	,	getdate()	,	'CM54 Category 0 SWD [Emp 575-595]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	600	,	''	,	0.004	,	getdate()	,	'CM54 Category 1 SWD [Emp 575-600]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.007	,	getdate()	,	'CM54 Category 2 SWD [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	595	,	609	,	''	,	0.004	,	getdate()	,	'CM54 Category 2 SWD [Emp 595-609]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	609	,	629	,	''	,	0.002	,	getdate()	,	'CM54 Category 2 SWD [Emp 610-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	600	,	''	,	0.003	,	getdate()	,	'CM54 Category 3 SWD [Emp 575-600]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	600	,	''	,	0.003	,	getdate()	,	'CM54 Category 4 SWD [Emp 575-600]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	600	,	''	,	0.003	,	getdate()	,	'CM54 Category 5 SWD [Emp 575-600]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.009	,	getdate()	,	'CM54 Category 6 SWD [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	595	,	609	,	''	,	0.006	,	getdate()	,	'CM54 Category 6 SWD [Emp 595-609]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	610	,	629	,	''	,	0.004	,	getdate()	,	'CM54 Category 6 SWD [Emp 610-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	630	,	649	,	''	,	0.002	,	getdate()	,	'CM54 Category 6 SWD [Emp 630-649]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.01	,	getdate()	,	'CM54 Category 7 SWD [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	595	,	609	,	''	,	0.007	,	getdate()	,	'CM54 Category 7 SWD [Emp 595-609]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	610	,	629	,	''	,	0.004	,	getdate()	,	'CM54 Category 7 SWD [Emp 610-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	630	,	649	,	''	,	0.002	,	getdate()	,	'CM54 Category 7 SWD [Emp 630-649]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.013	,	getdate()	,	'CM54 Category 8 SWD [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	595	,	609	,	''	,	0.008	,	getdate()	,	'CM54 Category 8 SWD [Emp 595-609]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	610	,	629	,	''	,	0.005	,	getdate()	,	'CM54 Category 8 SWD [Emp 610-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	630	,	649	,	''	,	0.002	,	getdate()	,	'CM54 Category 8 SWD [Emp 630-649]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.013	,	getdate()	,	'CM54 Category 9 SWD [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	595	,	609	,	''	,	0.008	,	getdate()	,	'CM54 Category 9 SWD [Emp 595-609]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	610	,	629	,	''	,	0.005	,	getdate()	,	'CM54 Category 9 SWD [Emp 610-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	630	,	649	,	''	,	0.002	,	getdate()	,	'CM54 Category 9 SWD [Emp 630-649]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	629	,	''	,	0.003	,	getdate()	,	'CM54 Category 10 SWD [Emp 595-600]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.002	,	getdate()	,	'CM54 Category 12 SWD [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.004	,	getdate()	,	'CM54 Category 13 SWD [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	595	,	629	,	''	,	0.002	,	getdate()	,	'CM54 Category 13 SWD [Emp 595-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.004	,	getdate()	,	'CM54 Category 14 SWD [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	595	,	609	,	''	,	0.003	,	getdate()	,	'CM54 Category 14 SWD [Emp 595-609]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	610	,	629	,	''	,	0.002	,	getdate()	,	'CM54 Category 14 SWD [Emp 610-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	630	,	649	,	''	,	0.001	,	getdate()	,	'CM54 Category 14 SWD [Emp 630-649]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.004	,	getdate()	,	'CM54 Category 15 SWD [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	595	,	609	,	''	,	0.003	,	getdate()	,	'CM54 Category 15 SWD [Emp 595-609]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	610	,	629	,	''	,	0.002	,	getdate()	,	'CM54 Category 15 SWD [Emp 610-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	630	,	649	,	''	,	0.001	,	getdate()	,	'CM54 Category 15 SWD [Emp 630-649]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.005	,	getdate()	,	'CM54 Category 16 SWD [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	595	,	609	,	''	,	0.003	,	getdate()	,	'CM54 Category 16 SWD [Emp 595-609]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	610	,	629	,	''	,	0.002	,	getdate()	,	'CM54 Category 16 SWD [Emp 610-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	630	,	649	,	''	,	0.001	,	getdate()	,	'CM54 Category 16 SWD [Emp 630-649]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.009	,	getdate()	,	'CM54 Category 17 SWD [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	595	,	609	,	''	,	0.004	,	getdate()	,	'CM54 Category 17 SWD [Emp 595-609]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	610	,	629	,	''	,	0.002	,	getdate()	,	'CM54 Category 17 SWD [Emp 610-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	630	,	649	,	''	,	0.001	,	getdate()	,	'CM54 Category 17 SWD [Emp 630-649]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.004	,	getdate()	,	'CM54 Category 18 SWD [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	595	,	629	,	''	,	0.002	,	getdate()	,	'CM54 Category 18 SWD [Emp 595-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.004	,	getdate()	,	'CM54 Category 19 SWD [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	595	,	609	,	''	,	0.003	,	getdate()	,	'CM54 Category 19 SWD [Emp 595-609]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	610	,	629	,	''	,	0.002	,	getdate()	,	'CM54 Category 19 SWD [Emp 610-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	630	,	649	,	''	,	0.001	,	getdate()	,	'CM54 Category 19 SWD [Emp 630-649]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.005	,	getdate()	,	'CM54 Category 20 SWD [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	595	,	609	,	''	,	0.004	,	getdate()	,	'CM54 Category 20 SWD [Emp 595-609]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	610	,	629	,	''	,	0.003	,	getdate()	,	'CM54 Category 20 SWD [Emp 610-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	630	,	649	,	''	,	0.001	,	getdate()	,	'CM54 Category 20 SWD [Emp 630-649]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.006	,	getdate()	,	'CM54 Category 21 SWD [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	595	,	609	,	''	,	0.004	,	getdate()	,	'CM54 Category 21 SWD [Emp 595-609]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	610	,	629	,	''	,	0.003	,	getdate()	,	'CM54 Category 21 SWD [Emp 610-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	630	,	649	,	''	,	0.001	,	getdate()	,	'CM54 Category 21 SWD [Emp 630-649]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	0	,	594	,	''	,	0.011	,	getdate()	,	'CM54 Category 22 SWD [Emp 575-594]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	595	,	609	,	''	,	0.006	,	getdate()	,	'CM54 Category 22 SWD [Emp 595-609]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	610	,	629	,	''	,	0.003	,	getdate()	,	'CM54 Category 22 SWD [Emp 610-629]'	,	1	,	48	,	2	,	1	,	51391	,	14	union
					select	630	,	649	,	''	,	0.001	,	getdate()	,	'CM54 Category 22 SWD [Emp 630-649]'	,	1	,	48	,	2	,	1	,	51391	,	14	
					) newrae
end