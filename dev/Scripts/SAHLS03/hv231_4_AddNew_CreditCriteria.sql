use [2am]

declare @CurrentCreditMatrixKey int
declare @NewCreditMatrixKey int
set @CurrentCreditMatrixKey = 53
set @NewCreditMatrixKey = 54

set identity_insert [2am].dbo.CreditCriteria on

if not exists (select 1 from [2am].dbo.CreditCriteria where CreditMatrixKey=@NewCreditMatrixKey)
begin
		insert into [2am].dbo.CreditCriteria (CreditCriteriaKey, CreditMatrixKey,MarginKey,CategoryKey,EmploymentTypeKey,MortgageLoanPurposeKey,MinLoanAmount,MaxLoanAmount,MinPropertyValue,MaxPropertyValue,LTV,PTI,MinIncomeAmount,ExceptionCriteria,MaxIncomeAmount,MinEmpiricaScore)
				select * from (
						select
						            -- reorder so we insert in simialr representation as spreadheet so creditcriteria keys flow neatly.
									(ROW_NUMBER() OVER(ORDER BY ncc.employmenttypekey, ncc.categorykey, ncc.maxloanamount, ncc.mortgageloanpurposekey) + (select max(CreditCriteriaKey) from [2am].dbo.CreditCriteria)) AS CreditCriteriaKey,
									@NewCreditMatrixKey as CreditMatrixKey,
									ncc.MarginKey,
									ncc.CategoryKey,
									ncc.EmploymentTypeKey,
									ncc.MortgageLoanPurposeKey,
									ncc.MinLoanAmount,
									ncc.MaxLoanAmount,
									ncc.MinPropertyValue,
									ncc.MaxPropertyValue,
									ncc.LTV,
									ncc.PTI,
									ncc.MinIncomeAmount,
									ncc.ExceptionCriteria,
									ncc.MaxIncomeAmount,
									ncc.MinEmpiricaScore
						from 
							(
								select distinct
											@NewCreditMatrixKey as CreditMatrixKey,
											MarginKey,
											CategoryKey,
											EmploymentTypeKey,
											MortgageLoanPurposeKey,
											MinLoanAmount,
											MaxLoanAmount,
											MinPropertyValue,
											MaxPropertyValue,
											LTV,
											PTI,
											MinIncomeAmount,
											ExceptionCriteria,
											MaxIncomeAmount,
											MinEmpiricaScore
								from (
												--- Copy of all current categories
												select 
													cc.CreditCriteriaKey,
												    @NewCreditMatrixKey as CreditMatrixKey,
													cc.MarginKey,
													cc.CategoryKey,
													cc.EmploymentTypeKey,
													cc.MortgageLoanPurposeKey,
													cc.MinLoanAmount,
													cc.MaxLoanAmount,
													cc.MinPropertyValue,
													cc.MaxPropertyValue,
													cc.LTV,
													cc.PTI,
													cc.MinIncomeAmount,
													cc.ExceptionCriteria,
													cc.MaxIncomeAmount,
													cc.MinEmpiricaScore
												from 
													creditcriteria cc 
												where 
													cc.creditmatrixkey=@CurrentCreditMatrixKey
												and
													creditcriteriakey not in (	select cc.CreditCriteriaKey
																				from creditcriteria cc 
																				where cc.creditmatrixkey=@CurrentCreditMatrixKey
																				and categorykey=2 
																				and maxloanamount>1800000.00 )  --- exclude category 2 high value loans from new matrix
												and
													creditcriteriakey not in (	select cc.CreditCriteriaKey
																				from creditcriteria cc 
																				where cc.creditmatrixkey=@CurrentCreditMatrixKey
																				and employmenttypekey = 3 --SWD
																				and categorykey in (5,8,9,21,22)
																				and mortgageloanpurposekey in (2,4) )  --- exclude switch and refinance for swd categories 5,8,9,21 and 22 from new matrix																					
												
											union
												
												--- Add copy of current SWD as new GEPF SWD with a slight renumber of new categories to keep Alpha GEPF together.
												select 
													CreditCriteriaKey,
												    @NewCreditMatrixKey as CreditMatrixKey,
													cc.MarginKey,
													CategoryKey =	case
																		when cc.CategoryKey=0 then 12
																		when cc.CategoryKey=1 then 13
																		when cc.CategoryKey=3 then 14
																		when cc.CategoryKey=4 then 15
																		when cc.CategoryKey=5 then 16
																		when cc.CategoryKey=10 then 17
																		when cc.CategoryKey=2 then 18
																		when cc.CategoryKey=6 then 19
																		when cc.CategoryKey=7 then 20
																		when cc.CategoryKey=8 then 21
																		when cc.CategoryKey=9 then 22
																		when cc.CategoryKey=99 then 99
																	end,
													cc.EmploymentTypeKey,
													cc.MortgageLoanPurposeKey,
													cc.MinLoanAmount,
													cc.MaxLoanAmount,
													cc.MinPropertyValue,
													cc.MaxPropertyValue,
													cc.LTV,
													cc.PTI,
													cc.MinIncomeAmount,
													cc.ExceptionCriteria,
													cc.MaxIncomeAmount,
													cc.MinEmpiricaScore
												from 
													creditcriteria cc 
												where 
													cc.creditmatrixkey=@CurrentCreditMatrixKey 
												and 
													cc.employmenttypekey=3
												and
													creditcriteriakey not in (	select cc.CreditCriteriaKey
																				from creditcriteria cc 
																				where cc.creditmatrixkey=@CurrentCreditMatrixKey
																				and categorykey=2 
																				and maxloanamount>1800000.00 )  --- exclude category 2 high value loans from new matrix
												and
													creditcriteriakey not in (	select cc.CreditCriteriaKey
																				from creditcriteria cc 
																				where cc.creditmatrixkey=@CurrentCreditMatrixKey
																				and employmenttypekey = 3 --SWD
																				and categorykey in (5,8,9,21,22)
																				and mortgageloanpurposekey in (2,4)  )  --- exclude switch and refinance for swd categories 5,8,9,21 and 22 from new matrix																				
											) new
										) ncc
				) newcreditcriteria

		set identity_insert [2am].dbo.CreditCriteria off
end



