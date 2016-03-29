'use strict';

angular.module('SAHL.DecisionTree.Shared.globals', []).
	factory('$sharedGlobals', [function () {
		var shared = (function () {
				var Enumerations = {
					sAHomeLoans : {
						credit : {
							creditMatrixCategory : {
								SalariedCategory0 : "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedCategory0",
								SalariedCategory1 : "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedCategory1",
								SalariedCategory3 : "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedCategory3",
								SalariedCategory4 : "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedCategory4",
								SalariedCategory5 : "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedCategory5",
								SalariedwithDeductionCategory0 : "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedwithDeductionCategory0",
								SalariedwithDeductionCategory1 : "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedwithDeductionCategory1",
								SalariedwithDeductionCategory3 : "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedwithDeductionCategory3",
								SalariedwithDeductionCategory4 : "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedwithDeductionCategory4",
								SalariedwithDeductionCategory5 : "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedwithDeductionCategory5",
								SalariedwithDeductionCategory10 : "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedwithDeductionCategory10",
								SelfEmployedCategory0 : "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SelfEmployedCategory0",
								SelfEmployedCategory1 : "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SelfEmployedCategory1",
								SelfEmployedCategory3 : "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SelfEmployedCategory3",
								AlphaCategory6 : "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.AlphaCategory6",
								AlphaCategory7 : "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.AlphaCategory7",
								AlphaCategory8 : "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.AlphaCategory8",
								AlphaCategory9 : "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.AlphaCategory9"
							}
						},
						propertyOccupancyType : {
							OwnerOccupied : "Globals.Enumerations.sAHomeLoans.propertyOccupancyType.OwnerOccupied",
							InvestmentProperty : "Globals.Enumerations.sAHomeLoans.propertyOccupancyType.InvestmentProperty"
						},
						householdIncomeType : {
							Salaried : "Globals.Enumerations.sAHomeLoans.householdIncomeType.Salaried",
							SelfEmployed : "Globals.Enumerations.sAHomeLoans.householdIncomeType.SelfEmployed",
							SalariedwithDeduction : "Globals.Enumerations.sAHomeLoans.householdIncomeType.SalariedwithDeduction",
							Unemployed : "Globals.Enumerations.sAHomeLoans.householdIncomeType.Unemployed"
						},
						mortgageLoanApplicationType : {
							Switch : "Globals.Enumerations.sAHomeLoans.mortgageLoanApplicationType.Switch",
							NewPurchase : "Globals.Enumerations.sAHomeLoans.mortgageLoanApplicationType.NewPurchase"
						},
						defaultEnumValue : {
							Unknown : "Globals.Enumerations.sAHomeLoans.defaultEnumValue.Unknown"
						}
					}
			}
	var Messages = {
					
					capitec : {
						Insufficientinformation : "The correct information is not available to continue.",
						credit : {
							ApplicantMinimumEmpirica : "The Credit Bureau score is below required minimum.",
							ApplicantMaximumJudgementsinLast3Years : "There is record of multiple recent unpaid judgements in the last 3 years.",
							MaximumAggregateJudgementValuewith3JudgementsinLast3Years : "There is record of unpaid judgements with a material aggregated rand value.",
							MaximumAggregatedJudgementValueUnsettledForBetween13And36Months : "There is record of an outstanding aggregated unpaid judgement of material value.",
							MaximumNumberOfUnsettledDefaultsWithinPast2Years : "There is record of numerous unsettled defaults within the past 2 years.",
							NoticeOfSequestration : "There is a record of Sequestration.",
							NoticeOfAdministrationOrder : "There is a record of an Administration Order.",
							NoticeOfDebtCounselling : "There is a record of Debt Counselling.",
							NoticeOfDebtReview : "There is a record of Debt Review.",
							NoticeOfConsumerIsDeceased : "There is record that the consumer is deceased.",
							NoticeOfCreditCardRevoked : "There is record of a revoked credit card.",
							NoticeOfAbsconded : "There is record that the applicant has absconded.",
							NoticeOfPaidOutOnDeceasedClaim : "There is record that a deceased claim has been paid out.",
							NoCreditBureauMatchFound : "No credit bureau match found.",
							LoantoValueAboveCreditMaximum : "Insufficient property value for loan amount requested.",
							NewPurchaseInvestmentPropertyLoanToValueAboveMaximum : "Your requested loan amount as a percentage of property value (LTV) for an investment property would be #{Variables::outputs.LoantoValueasPercent} which equals or exceeds the maximum of 80%. In order to reduce your LTV, the loan amount would need to be lowered by increasing the deposit, by R#{requiredamounttolowerloanamountby} or more.",
							NewPurchaseLTVaboveMaximum : "Your loan amount as a percentage of purchase price (LTV) would be #{Variables::outputs.LoantoValueasPercent}, which is greater than or equal to the maximum of #{maximumloantovalue}%. In order to reduce your LTV, the loan amount would need to be lowered by increasing the deposit by R#{requiredamounttolowerloanamountby} or more.",
							SwitchLTVaboveMaximum : "Your loan amount as a percentage of estimated property value (LTV) would be #{Variables::outputs.LoantoValueasPercent}, which is greater than or equal to the maximum of #{maximumloantovalue}%. In order to reduce your LTV, the loan amount would need to be lowered by decreasing the cash required by R#{requiredamounttolowerloanamountby} or more.",
							PTIaboveMaximum : "Your repayment as a percentage of household income (PTI) would be #{Variables::outputs.PaymenttoIncomeasPercent} and is greater than or equal to the maximum of #{maximumpti}%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R#{maximumloanamount} or alternatively additional income so that total income is at least R#{requiredhouseholdincome}.",
							HouseholdIncomeTypeIsUnemployed : "Your household income type may not be Unemployed.",
							SwitchInvestmentPropertyLoanToValueAboveMaximum : "Your requested loan amount as a percentage of property value (LTV) for an investment property would be #{Variables::outputs.LoantoValueasPercent} which equals or exceeds the maximum of 80%. In order to reduce your LTV, the loan amount would need to be lowered by decreasing the cash required by R#{requiredamounttolowerloanamountby} or more.",
							ApplicantAgeLimit : "All applicants must be between the ages of 18 and 65 years.",
							salaried : {
								HouseholdIncomeBelowMinimum : "Total income is below the required minimum for salaried applicants.",
								LoanAmountBelowMinimum : "Loan amount requested is below the product minimum.",
								LoanAmountAboveMaximum : "Loan amount requested is above the product maximum.",
								LTVAboveMaximum : "The application LTV is above the maximum allowed for salaried applicants.",
								CreditScoreBelowMinimum : "The best Credit Bureau score applicable for this application is below the minimum requirement for salaried applicants.",
								PTIAboveMaximum : "Affordability: Loan repayment to income ratio (PTI) is above the maximum limit. Reducing the loan amount may make the loan more affordable."
							},
							salariedwithDeduction : {
								HouseholdIncomeBelowMinimum : "Total income is below the required minimum for salaried with deduction applicants.",
								LoanAmountBelowMinimum : "Loan amount requested is below the product minimum.",
								LoanAmountAboveMaximum : "Loan amount requested is above the product maximum.",
								LTVAboveMaximum : "The application LTV is above the maximum allowed for salaried with deduction applicants.",
								CreditScoreBelowMinimum : "The best Credit Bureau score applicable for this application is below the minimum requirement for salaried with deduction applicants.",
								PTIAboveMaximum : "Affordability: Loan repayment to income ratio (PTI) is above the maximum limit. Reducing the loan amount may make the loan more affordable."
							},
							selfEmployed : {
								HouseholdIncomeBelowMinimum : "Total income is below the required minimum for self-employed applicants.",
								LoanAmountBelowMinimum : "Loan amount requested is below the product minimum.",
								LoanAmountAboveMaximum : "Loan amount requested is above the product maximum.",
								LTVAboveMaximum : "The application LTV is above the maximum allowed for self-employed applicants.",
								CreditScoreBelowMinimum : "The best Credit Bureau score applicable for this application is below the minimum requirement for self-employed applicants.",
								PTIAboveMaximum : "Affordability: Loan repayment to income ratio (PTI) is above the maximum limit. Reducing the loan amount may make the loan more affordable."
							},
							alpha : {
								HouseholdIncomeBelowMinimum : "Total income is below the required minimum for the product.",
								HouseholdIncomeAboveMaximum : "Total income is above the maximum for the product.",
								LoanAmountBelowMinimum : "Loan amount requested is below the product minimum.",
								LoanAmountAboveMaximum : "Loan amount requested is above the product maximum.",
								LTVAboveMaximum : "The application LTV is above the maximum allowed for the product.",
								CreditScoreBelowMinimum : "The best Credit Bureau score applicable for this application is below the minimum requirement for the product.",
								PTIAboveMaximum : "Affordability: Loan repayment to income ratio (PTI) is above the maximum limit. Reducing the loan amount may make the loan more affordable.",
								PropertyValueBelowMinimum : "The property value is below the minimum for the product."
							}
						}
					}
			}
	var Variables = {
					
					
					capitec : {
						AffordabilityMaximumPTI : 0.3,
						AffordabilityMaximumLTV : 1,
						credit : {
							MinimumApplicationEmpirica : 575,
							MinimumApplicantAge : 18,
							MaximumApplicantAge : 65,
							PercentVarianceonPaymentToIncomeRatio : 0.1,
							PercentVarianceonCategoryEmpirica : 0.015,
							InvestmentPropertyMaximumLTV : 0.8,
							salaried : {
								MinimumLoanAmount : 150000,
								MinimumHouseholdIncome : 13000,
								MaximumLoanAmountLoanSizeRange1 : 1800000,
								MaximumLoanAmountLoanSizeRange2 : 2750000,
								MaximumLoanAmountLoanSizeRange3 : 5000000,
								MaximumLoanAmount : 5000000,
								category0 : {
									MaximumLoanToValue : 0.7,
									MinimumApplicationEmpirica : 575,
									CategoryLinkRate : 0.028,
									MinimumHouseholdIncome : 13000,
									loanSizeRange1 : {
										MaximumPaymentToIncome : 0.3
									},
									loanSizeRange2 : {
										MaximumPaymentToIncome : 0.3
									},
									loanSizeRange3 : {
										MaximumPaymentToIncome : 0.25
									}
								},
								category1 : {
									MaximumLoanToValue : 0.8,
									MinimumApplicationEmpirica : 600,
									CategoryLinkRate : 0.032,
									MinimumHouseholdIncome : 13000,
									loanSizeRange1 : {
										MaximumPaymentToIncome : 0.3
									},
									loanSizeRange2 : {
										MaximumPaymentToIncome : 0.25
									}
								},
								category3 : {
									MaximumLoanToValue : 0.85,
									MinimumApplicationEmpirica : 610,
									CategoryLinkRate : 0.037,
									MinimumHouseholdIncome : 18600,
									loanSizeRange1 : {
										MaximumPaymentToIncome : 0.3
									},
									loanSizeRange2 : {
										MaximumPaymentToIncome : 0.25
									}
								},
								category4 : {
									MaximumLoanToValue : 0.9,
									MinimumApplicationEmpirica : 625,
									CategoryLinkRate : 0.039,
									MinimumHouseholdIncome : 18600,
									loanSizeRange1 : {
										MaximumPaymentToIncome : 0.3
									},
									loanSizeRange2 : {
										MaximumPaymentToIncome : 0.25
									}
								},
								category5 : {
									MaximumLoanToValue : 0.95,
									MinimumApplicationEmpirica : 640,
									CategoryLinkRate : 0.039,
									MinimumHouseholdIncome : 18600,
									loanSizeRange1 : {
										MaximumPaymentToIncome : 0.25
									}
								}
							},
							salariedwithDeduction : {
								MinimumLoanAmount : 150000,
								MinimumHouseholdIncome : 13000,
								MaximumLoanAmountLoanSizeRange1 : 1800000,
								MaximumLoanAmountLoanSizeRange2 : 2750000,
								MaximumLoanAmountLoanSizeRange3 : 5000000,
								MinimumApplicationEmpirica : 575,
								MaximumLoanAmount : 5000000,
								category0 : {
									MaximumLoanToValue : 0.7,
									CategoryLinkRate : 0.028,
									MinimumHouseholdIncome : 13000,
									MinimumApplicationEmpirica : 575,
									loanSizeRange1 : {
										MaximumPaymentToIncome : 0.3
									},
									loanSizeRange2 : {
										MaximumPaymentToIncome : 0.3
									},
									loanSizeRange3 : {
										MaximumPaymentToIncome : 0.25
									}
								},
								category1 : {
									MaximumLoanToValue : 0.8,
									CategoryLinkRate : 0.032,
									MinimumHouseholdIncome : 13000,
									MinimumApplicationEmpirica : 575,
									loanSizeRange1 : {
										MaximumPaymentToIncome : 0.3
									},
									loanSizeRange2 : {
										MaximumPaymentToIncome : 0.25
									}
								},
								category3 : {
									MaximumLoanToValue : 0.85,
									CategoryLinkRate : 0.037,
									MinimumHouseholdIncome : 18600,
									MinimumApplicationEmpirica : 600,
									loanSizeRange1 : {
										MaximumPaymentToIncome : 0.3
									},
									loanSizeRange2 : {
										MaximumPaymentToIncome : 0.25
									}
								},
								category4 : {
									MaximumLoanToValue : 0.9,
									CategoryLinkRate : 0.039,
									MinimumHouseholdIncome : 18600,
									MinimumApplicationEmpirica : 610,
									loanSizeRange1 : {
										MaximumPaymentToIncome : 0.3
									},
									loanSizeRange2 : {
										MaximumPaymentToIncome : 0.25
									}
								},
								category5 : {
									MaximumLoanToValue : 0.95,
									CategoryLinkRate : 0.039,
									MinimumHouseholdIncome : 18600,
									MinimumApplicationEmpirica : 620,
									loanSizeRange1 : {
										MaximumPaymentToIncome : 0.25
									}
								},
								category10 : {
									CategoryLinkRate : 0.042,
									MaximumLoanToValue : 1,
									MinimumHouseholdIncome : 18600,
									MinimumApplicationEmpirica : 620,
									newPurchase : {
										MaximumLoanToValue : 1,
										loanSizeRange1 : {
											MaximumPaymentToIncome : 0.25
										}
									}
								}
							},
							selfEmployed : {
								MinimumLoanAmount : 150000,
								MinimumHouseholdIncome : 13000,
								MaximumLoanAmountLoanSizeRange1 : 1800000,
								MaximumLoanAmountLoanSizeRange2 : 2750000,
								MaximumLoanAmountLoanSizeRange3 : 3500000,
								MaximumLoanAmount : 3500000,
								category0 : {
									MaximumLoanToValue : 0.7,
									MinimumApplicationEmpirica : 610,
									CategoryLinkRate : 0.032,
									MaximumPaymentToIncome : 0.25,
									MinimumHouseholdIncome : 13000
								},
								category1 : {
									MaximumLoanToValue : 0.8,
									MinimumApplicationEmpirica : 610,
									CategoryLinkRate : 0.036,
									MinimumHouseholdIncome : 13000,
									loanSizeRange1 : {
										MaximumPaymentToIncome : 0.25
									},
									loanSizeRange2 : {
										MaximumPaymentToIncome : 0.2
									}
								},
								category3 : {
									MaximumLoanToValue : 0.85,
									MinimumApplicationEmpirica : 610,
									CategoryLinkRate : 0.037,
									MinimumHouseholdIncome : 18600,
									loanSizeRange1 : {
										MaximumPaymentToIncome : 0.25
									},
									loanSizeRange2 : {
										MaximumPaymentToIncome : 0.2
									}
								}
							},
							alpha : {
								MinimumLoanAmount : 100000,
								MinimumHouseholdIncome : 8000,
								MaximumHouseholdIncome : 18599,
								MinimumPropertyValue : 200000,
								category6 : {
									MaximumLoanToValue : 0.85,
									MaximumPaymentToIncome : 0.3,
									MinimumApplicationEmpirica : 600,
									CategoryLinkRate : 0.039
								},
								category7 : {
									MaximumLoanToValue : 0.92,
									MaximumPaymentToIncome : 0.3,
									MinimumApplicationEmpirica : 600,
									CategoryLinkRate : 0.044
								},
								category8 : {
									MaximumLoanToValue : 0.96,
									MaximumPaymentToIncome : 0.3,
									MinimumApplicationEmpirica : 610,
									CategoryLinkRate : 0.048
								},
								category9 : {
									MaximumLoanToValue : 1,
									MaximumPaymentToIncome : 0.3,
									MinimumApplicationEmpirica : 610,
									CategoryLinkRate : 0.051,
									MinimumHouseholdIncome : 10000
								}
							}
						}
					},
					sAHomeLoans : {
						newBusiness : {
							credit : {
							}
						},
						rates : {
							JIBAR3MonthRounded : 0.058
						}
					}
			}

			return {
				Enumerations: Enumerations,
				Messages: Messages,
				Variables: Variables
			};
		}());
		return shared;
	}]);