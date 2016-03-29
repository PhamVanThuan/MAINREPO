'use strict';

angular.module('SAHL.Services.Interfaces.DecisionTree.queries', []).
	factory('$decisionTreeQueries', [function () {
		var shared = (function () {
						function CapitecAffordabilityInterestRate_1Query(householdIncome, deposit, calcRate, interestRateQuery, globalsVersion) {
				this.householdIncome = householdIncome;
				this.deposit = deposit;
				this.calcRate = calcRate;
				this.interestRateQuery = interestRateQuery;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecAffordabilityInterestRate_1Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecAffordabilityInterestRate_Query(householdIncome, deposit, calcRate, interestRateQuery, globalsVersion) {
				this.householdIncome = householdIncome;
				this.deposit = deposit;
				this.calcRate = calcRate;
				this.interestRateQuery = interestRateQuery;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecAffordabilityInterestRate_Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecAlphaCreditPricing_1Query(applicationType, propertyOccupancyType, householdIncomeType, householdIncome, propertyPurchasePrice, depositAmount, cashAmountRequired, currentMortgageLoanBalance, estimatedMarketValueofProperty, termInMonth, applicationEmpirica, fees, interimInterest, capitaliseFees, globalsVersion) {
				this.applicationType = applicationType || '';
				this.propertyOccupancyType = propertyOccupancyType || '';
				this.householdIncomeType = householdIncomeType || '';
				this.householdIncome = householdIncome;
				this.propertyPurchasePrice = propertyPurchasePrice;
				this.depositAmount = depositAmount;
				this.cashAmountRequired = cashAmountRequired;
				this.currentMortgageLoanBalance = currentMortgageLoanBalance;
				this.estimatedMarketValueofProperty = estimatedMarketValueofProperty;
				this.termInMonth = termInMonth;
				this.applicationEmpirica = applicationEmpirica;
				this.fees = fees;
				this.interimInterest = interimInterest;
				this.capitaliseFees = capitaliseFees;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecAlphaCreditPricing_1Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecAlphaCreditPricing_Query(applicationType, propertyOccupancyType, householdIncomeType, householdIncome, propertyPurchasePrice, depositAmount, cashAmountRequired, currentMortgageLoanBalance, estimatedMarketValueofProperty, termInMonth, applicationEmpirica, fees, interimInterest, capitaliseFees, globalsVersion) {
				this.applicationType = applicationType || '';
				this.propertyOccupancyType = propertyOccupancyType || '';
				this.householdIncomeType = householdIncomeType || '';
				this.householdIncome = householdIncome;
				this.propertyPurchasePrice = propertyPurchasePrice;
				this.depositAmount = depositAmount;
				this.cashAmountRequired = cashAmountRequired;
				this.currentMortgageLoanBalance = currentMortgageLoanBalance;
				this.estimatedMarketValueofProperty = estimatedMarketValueofProperty;
				this.termInMonth = termInMonth;
				this.applicationEmpirica = applicationEmpirica;
				this.fees = fees;
				this.interimInterest = interimInterest;
				this.capitaliseFees = capitaliseFees;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecAlphaCreditPricing_Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecApplicationCreditPolicy_1Query(firstIncomeContributorApplicantEmpirica, firstIncomeContributorApplicantIncome, secondIncomeContributorApplicantEmpirica, secondIncomeContributorApplicantIncome, householdIncome, youngestApplicantAgeinYears, eldestApplicantAgeinYears, globalsVersion) {
				this.firstIncomeContributorApplicantEmpirica = firstIncomeContributorApplicantEmpirica;
				this.firstIncomeContributorApplicantIncome = firstIncomeContributorApplicantIncome;
				this.secondIncomeContributorApplicantEmpirica = secondIncomeContributorApplicantEmpirica;
				this.secondIncomeContributorApplicantIncome = secondIncomeContributorApplicantIncome;
				this.householdIncome = householdIncome;
				this.youngestApplicantAgeinYears = youngestApplicantAgeinYears;
				this.eldestApplicantAgeinYears = eldestApplicantAgeinYears;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecApplicationCreditPolicy_1Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecApplicationCreditPolicy_2Query(firstIncomeContributorApplicantEmpirica, firstIncomeContributorApplicantIncome, secondIncomeContributorApplicantEmpirica, secondIncomeContributorApplicantIncome, householdIncome, youngestApplicantAgeinYears, eldestApplicantAgeinYears, globalsVersion) {
				this.firstIncomeContributorApplicantEmpirica = firstIncomeContributorApplicantEmpirica;
				this.firstIncomeContributorApplicantIncome = firstIncomeContributorApplicantIncome;
				this.secondIncomeContributorApplicantEmpirica = secondIncomeContributorApplicantEmpirica;
				this.secondIncomeContributorApplicantIncome = secondIncomeContributorApplicantIncome;
				this.householdIncome = householdIncome;
				this.youngestApplicantAgeinYears = youngestApplicantAgeinYears;
				this.eldestApplicantAgeinYears = eldestApplicantAgeinYears;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecApplicationCreditPolicy_2Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecApplicationCreditPolicy_Query(firstIncomeContributorApplicantEmpirica, firstIncomeContributorApplicantIncome, secondIncomeContributorApplicantEmpirica, secondIncomeContributorApplicantIncome, householdIncome, youngestApplicantAgeinYears, eldestApplicantAgeinYears, globalsVersion) {
				this.firstIncomeContributorApplicantEmpirica = firstIncomeContributorApplicantEmpirica;
				this.firstIncomeContributorApplicantIncome = firstIncomeContributorApplicantIncome;
				this.secondIncomeContributorApplicantEmpirica = secondIncomeContributorApplicantEmpirica;
				this.secondIncomeContributorApplicantIncome = secondIncomeContributorApplicantIncome;
				this.householdIncome = householdIncome;
				this.youngestApplicantAgeinYears = youngestApplicantAgeinYears;
				this.eldestApplicantAgeinYears = eldestApplicantAgeinYears;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecApplicationCreditPolicy_Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecClientCreditBureauAssessment_1Query(applicantEmpirica, numberofJudgmentswithinLast3Years, aggregatedJudgmentValuewithinLast3Years, nonSettledAggregatedJudgmentValuewithinLast3Years, numberofUnsettledDefaultswithinLast2Years, sequestrationNotice, administrationOrderNotice, debtCounsellingNotice, debtReviewNotice, consumerDeceasedNotification, creditCardRevoked, consumerAbsconded, paidOutonDeceasedClaim, creditBureauMatch, globalsVersion) {
				this.applicantEmpirica = applicantEmpirica;
				this.numberofJudgmentswithinLast3Years = numberofJudgmentswithinLast3Years;
				this.aggregatedJudgmentValuewithinLast3Years = aggregatedJudgmentValuewithinLast3Years;
				this.nonSettledAggregatedJudgmentValuewithinLast3Years = nonSettledAggregatedJudgmentValuewithinLast3Years;
				this.numberofUnsettledDefaultswithinLast2Years = numberofUnsettledDefaultswithinLast2Years;
				this.sequestrationNotice = sequestrationNotice;
				this.administrationOrderNotice = administrationOrderNotice;
				this.debtCounsellingNotice = debtCounsellingNotice;
				this.debtReviewNotice = debtReviewNotice;
				this.consumerDeceasedNotification = consumerDeceasedNotification;
				this.creditCardRevoked = creditCardRevoked;
				this.consumerAbsconded = consumerAbsconded;
				this.paidOutonDeceasedClaim = paidOutonDeceasedClaim;
				this.creditBureauMatch = creditBureauMatch;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecClientCreditBureauAssessment_1Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecClientCreditBureauAssessment_2Query(applicantEmpirica, numberofJudgmentswithinLast3Years, aggregatedJudgmentValuewithinLast3Years, nonSettledAggregatedJudgmentValuewithinLast3Years, numberofUnsettledDefaultswithinLast2Years, sequestrationNotice, administrationOrderNotice, debtCounsellingNotice, debtReviewNotice, consumerDeceasedNotification, creditCardRevoked, consumerAbsconded, paidOutonDeceasedClaim, creditBureauMatch, globalsVersion) {
				this.applicantEmpirica = applicantEmpirica;
				this.numberofJudgmentswithinLast3Years = numberofJudgmentswithinLast3Years;
				this.aggregatedJudgmentValuewithinLast3Years = aggregatedJudgmentValuewithinLast3Years;
				this.nonSettledAggregatedJudgmentValuewithinLast3Years = nonSettledAggregatedJudgmentValuewithinLast3Years;
				this.numberofUnsettledDefaultswithinLast2Years = numberofUnsettledDefaultswithinLast2Years;
				this.sequestrationNotice = sequestrationNotice;
				this.administrationOrderNotice = administrationOrderNotice;
				this.debtCounsellingNotice = debtCounsellingNotice;
				this.debtReviewNotice = debtReviewNotice;
				this.consumerDeceasedNotification = consumerDeceasedNotification;
				this.creditCardRevoked = creditCardRevoked;
				this.consumerAbsconded = consumerAbsconded;
				this.paidOutonDeceasedClaim = paidOutonDeceasedClaim;
				this.creditBureauMatch = creditBureauMatch;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecClientCreditBureauAssessment_2Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecClientCreditBureauAssessment_Query(applicantEmpirica, numberofJudgmentswithinLast3Years, aggregatedJudgmentValuewithinLast3Years, nonSettledAggregatedJudgmentValuewithinLast3Years, numberofUnsettledDefaultswithinLast2Years, sequestrationNotice, administrationOrderNotice, debtCounsellingNotice, debtReviewNotice, consumerDeceasedNotification, creditCardRevoked, consumerAbsconded, paidOutonDeceasedClaim, creditBureauMatch, globalsVersion) {
				this.applicantEmpirica = applicantEmpirica;
				this.numberofJudgmentswithinLast3Years = numberofJudgmentswithinLast3Years;
				this.aggregatedJudgmentValuewithinLast3Years = aggregatedJudgmentValuewithinLast3Years;
				this.nonSettledAggregatedJudgmentValuewithinLast3Years = nonSettledAggregatedJudgmentValuewithinLast3Years;
				this.numberofUnsettledDefaultswithinLast2Years = numberofUnsettledDefaultswithinLast2Years;
				this.sequestrationNotice = sequestrationNotice;
				this.administrationOrderNotice = administrationOrderNotice;
				this.debtCounsellingNotice = debtCounsellingNotice;
				this.debtReviewNotice = debtReviewNotice;
				this.consumerDeceasedNotification = consumerDeceasedNotification;
				this.creditCardRevoked = creditCardRevoked;
				this.consumerAbsconded = consumerAbsconded;
				this.paidOutonDeceasedClaim = paidOutonDeceasedClaim;
				this.creditBureauMatch = creditBureauMatch;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecClientCreditBureauAssessment_Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecCreditPricing_1Query(applicationType, propertyOccupancyType, householdIncomeType, householdIncome, propertyPurchasePrice, depositAmount, cashAmountRequired, currentMortgageLoanBalance, estimatedMarketValueofProperty, eldestApplicantAge, youngestApplicantAge, termInMonth, firstIncomeContributorApplicantEmpirica, firstIncomeContributorApplicantIncome, secondIncomeContributorApplicantEmpirica, secondIncomeContributorApplicantIncome, globalsVersion) {
				this.applicationType = applicationType || '';
				this.propertyOccupancyType = propertyOccupancyType || '';
				this.householdIncomeType = householdIncomeType || '';
				this.householdIncome = householdIncome;
				this.propertyPurchasePrice = propertyPurchasePrice;
				this.depositAmount = depositAmount;
				this.cashAmountRequired = cashAmountRequired;
				this.currentMortgageLoanBalance = currentMortgageLoanBalance;
				this.estimatedMarketValueofProperty = estimatedMarketValueofProperty;
				this.eldestApplicantAge = eldestApplicantAge;
				this.youngestApplicantAge = youngestApplicantAge;
				this.termInMonth = termInMonth;
				this.firstIncomeContributorApplicantEmpirica = firstIncomeContributorApplicantEmpirica;
				this.firstIncomeContributorApplicantIncome = firstIncomeContributorApplicantIncome;
				this.secondIncomeContributorApplicantEmpirica = secondIncomeContributorApplicantEmpirica;
				this.secondIncomeContributorApplicantIncome = secondIncomeContributorApplicantIncome;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecCreditPricing_1Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecCreditPricing_Query(applicationType, propertyOccupancyType, householdIncomeType, householdIncome, propertyPurchasePrice, depositAmount, cashAmountRequired, currentMortgageLoanBalance, estimatedMarketValueofProperty, eldestApplicantAge, youngestApplicantAge, termInMonth, firstIncomeContributorApplicantEmpirica, firstIncomeContributorApplicantIncome, secondIncomeContributorApplicantEmpirica, secondIncomeContributorApplicantIncome, globalsVersion) {
				this.applicationType = applicationType || '';
				this.propertyOccupancyType = propertyOccupancyType || '';
				this.householdIncomeType = householdIncomeType || '';
				this.householdIncome = householdIncome;
				this.propertyPurchasePrice = propertyPurchasePrice;
				this.depositAmount = depositAmount;
				this.cashAmountRequired = cashAmountRequired;
				this.currentMortgageLoanBalance = currentMortgageLoanBalance;
				this.estimatedMarketValueofProperty = estimatedMarketValueofProperty;
				this.eldestApplicantAge = eldestApplicantAge;
				this.youngestApplicantAge = youngestApplicantAge;
				this.termInMonth = termInMonth;
				this.firstIncomeContributorApplicantEmpirica = firstIncomeContributorApplicantEmpirica;
				this.firstIncomeContributorApplicantIncome = firstIncomeContributorApplicantIncome;
				this.secondIncomeContributorApplicantEmpirica = secondIncomeContributorApplicantEmpirica;
				this.secondIncomeContributorApplicantIncome = secondIncomeContributorApplicantIncome;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecCreditPricing_Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecOriginationCreditPricing_1Query(applicationType, propertyOccupancyType, householdIncomeType, householdIncome, propertyPurchasePrice, depositAmount, cashAmountRequired, currentMortgageLoanBalance, estimatedMarketValueofProperty, eldestApplicantAge, youngestApplicantAge, termInMonth, firstIncomeContributorApplicantEmpirica, firstIncomeContributorApplicantIncome, secondIncomeContributorApplicantEmpirica, secondIncomeContributorApplicantIncome, eligibleBorrower, fees, interimInterest, capitaliseFees, globalsVersion) {
				this.applicationType = applicationType || '';
				this.propertyOccupancyType = propertyOccupancyType || '';
				this.householdIncomeType = householdIncomeType || '';
				this.householdIncome = householdIncome;
				this.propertyPurchasePrice = propertyPurchasePrice;
				this.depositAmount = depositAmount;
				this.cashAmountRequired = cashAmountRequired;
				this.currentMortgageLoanBalance = currentMortgageLoanBalance;
				this.estimatedMarketValueofProperty = estimatedMarketValueofProperty;
				this.eldestApplicantAge = eldestApplicantAge;
				this.youngestApplicantAge = youngestApplicantAge;
				this.termInMonth = termInMonth;
				this.firstIncomeContributorApplicantEmpirica = firstIncomeContributorApplicantEmpirica;
				this.firstIncomeContributorApplicantIncome = firstIncomeContributorApplicantIncome;
				this.secondIncomeContributorApplicantEmpirica = secondIncomeContributorApplicantEmpirica;
				this.secondIncomeContributorApplicantIncome = secondIncomeContributorApplicantIncome;
				this.eligibleBorrower = eligibleBorrower;
				this.fees = fees;
				this.interimInterest = interimInterest;
				this.capitaliseFees = capitaliseFees;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecOriginationCreditPricing_1Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecOriginationCreditPricing_Query(applicationType, propertyOccupancyType, householdIncomeType, householdIncome, propertyPurchasePrice, depositAmount, cashAmountRequired, currentMortgageLoanBalance, estimatedMarketValueofProperty, eldestApplicantAge, youngestApplicantAge, termInMonth, firstIncomeContributorApplicantEmpirica, firstIncomeContributorApplicantIncome, secondIncomeContributorApplicantEmpirica, secondIncomeContributorApplicantIncome, eligibleBorrower, fees, interimInterest, capitaliseFees, globalsVersion) {
				this.applicationType = applicationType || '';
				this.propertyOccupancyType = propertyOccupancyType || '';
				this.householdIncomeType = householdIncomeType || '';
				this.householdIncome = householdIncome;
				this.propertyPurchasePrice = propertyPurchasePrice;
				this.depositAmount = depositAmount;
				this.cashAmountRequired = cashAmountRequired;
				this.currentMortgageLoanBalance = currentMortgageLoanBalance;
				this.estimatedMarketValueofProperty = estimatedMarketValueofProperty;
				this.eldestApplicantAge = eldestApplicantAge;
				this.youngestApplicantAge = youngestApplicantAge;
				this.termInMonth = termInMonth;
				this.firstIncomeContributorApplicantEmpirica = firstIncomeContributorApplicantEmpirica;
				this.firstIncomeContributorApplicantIncome = firstIncomeContributorApplicantIncome;
				this.secondIncomeContributorApplicantEmpirica = secondIncomeContributorApplicantEmpirica;
				this.secondIncomeContributorApplicantIncome = secondIncomeContributorApplicantIncome;
				this.eligibleBorrower = eligibleBorrower;
				this.fees = fees;
				this.interimInterest = interimInterest;
				this.capitaliseFees = capitaliseFees;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecOriginationCreditPricing_Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecOriginationPricingforRisk_1Query(applicationEmpirica, creditMatrixCategory, householdIncomeType, globalsVersion) {
				this.applicationEmpirica = applicationEmpirica;
				this.creditMatrixCategory = creditMatrixCategory || '';
				this.householdIncomeType = householdIncomeType || '';
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecOriginationPricingforRisk_1Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecOriginationPricingforRisk_2Query(applicationEmpirica, creditMatrixCategory, householdIncomeType, globalsVersion) {
				this.applicationEmpirica = applicationEmpirica;
				this.creditMatrixCategory = creditMatrixCategory || '';
				this.householdIncomeType = householdIncomeType || '';
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecOriginationPricingforRisk_2Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecOriginationPricingforRisk_Query(applicationEmpirica, creditMatrixCategory, householdIncomeType, globalsVersion) {
				this.applicationEmpirica = applicationEmpirica;
				this.creditMatrixCategory = creditMatrixCategory || '';
				this.householdIncomeType = householdIncomeType || '';
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecOriginationPricingforRisk_Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecSalariedCreditPricing_1Query(applicationType, propertyOccupancyType, householdIncomeType, householdIncome, propertyPurchasePrice, depositAmount, cashAmountRequired, currentMortgageLoanBalance, estimatedMarketValueofProperty, termInMonth, applicationEmpirica, fees, interimInterest, capitaliseFees, globalsVersion) {
				this.applicationType = applicationType || '';
				this.propertyOccupancyType = propertyOccupancyType || '';
				this.householdIncomeType = householdIncomeType || '';
				this.householdIncome = householdIncome;
				this.propertyPurchasePrice = propertyPurchasePrice;
				this.depositAmount = depositAmount;
				this.cashAmountRequired = cashAmountRequired;
				this.currentMortgageLoanBalance = currentMortgageLoanBalance;
				this.estimatedMarketValueofProperty = estimatedMarketValueofProperty;
				this.termInMonth = termInMonth;
				this.applicationEmpirica = applicationEmpirica;
				this.fees = fees;
				this.interimInterest = interimInterest;
				this.capitaliseFees = capitaliseFees;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecSalariedCreditPricing_1Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecSalariedCreditPricing_Query(applicationType, propertyOccupancyType, householdIncomeType, householdIncome, propertyPurchasePrice, depositAmount, cashAmountRequired, currentMortgageLoanBalance, estimatedMarketValueofProperty, termInMonth, applicationEmpirica, fees, interimInterest, capitaliseFees, globalsVersion) {
				this.applicationType = applicationType || '';
				this.propertyOccupancyType = propertyOccupancyType || '';
				this.householdIncomeType = householdIncomeType || '';
				this.householdIncome = householdIncome;
				this.propertyPurchasePrice = propertyPurchasePrice;
				this.depositAmount = depositAmount;
				this.cashAmountRequired = cashAmountRequired;
				this.currentMortgageLoanBalance = currentMortgageLoanBalance;
				this.estimatedMarketValueofProperty = estimatedMarketValueofProperty;
				this.termInMonth = termInMonth;
				this.applicationEmpirica = applicationEmpirica;
				this.fees = fees;
				this.interimInterest = interimInterest;
				this.capitaliseFees = capitaliseFees;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecSalariedCreditPricing_Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecSalariedwithDeductionCreditPricing_1Query(applicationType, propertyOccupancyType, householdIncomeType, householdIncome, propertyPurchasePrice, depositAmount, cashAmountRequired, currentMortgageLoanBalance, estimatedMarketValueofProperty, termInMonth, applicationEmpirica, fees, interimInterest, capitaliseFees, globalsVersion) {
				this.applicationType = applicationType || '';
				this.propertyOccupancyType = propertyOccupancyType || '';
				this.householdIncomeType = householdIncomeType || '';
				this.householdIncome = householdIncome;
				this.propertyPurchasePrice = propertyPurchasePrice;
				this.depositAmount = depositAmount;
				this.cashAmountRequired = cashAmountRequired;
				this.currentMortgageLoanBalance = currentMortgageLoanBalance;
				this.estimatedMarketValueofProperty = estimatedMarketValueofProperty;
				this.termInMonth = termInMonth;
				this.applicationEmpirica = applicationEmpirica;
				this.fees = fees;
				this.interimInterest = interimInterest;
				this.capitaliseFees = capitaliseFees;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecSalariedwithDeductionCreditPricing_1Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecSalariedwithDeductionCreditPricing_Query(applicationType, propertyOccupancyType, householdIncomeType, householdIncome, propertyPurchasePrice, depositAmount, cashAmountRequired, currentMortgageLoanBalance, estimatedMarketValueofProperty, termInMonth, applicationEmpirica, fees, interimInterest, capitaliseFees, globalsVersion) {
				this.applicationType = applicationType || '';
				this.propertyOccupancyType = propertyOccupancyType || '';
				this.householdIncomeType = householdIncomeType || '';
				this.householdIncome = householdIncome;
				this.propertyPurchasePrice = propertyPurchasePrice;
				this.depositAmount = depositAmount;
				this.cashAmountRequired = cashAmountRequired;
				this.currentMortgageLoanBalance = currentMortgageLoanBalance;
				this.estimatedMarketValueofProperty = estimatedMarketValueofProperty;
				this.termInMonth = termInMonth;
				this.applicationEmpirica = applicationEmpirica;
				this.fees = fees;
				this.interimInterest = interimInterest;
				this.capitaliseFees = capitaliseFees;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecSalariedwithDeductionCreditPricing_Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecSelfEmployedCreditPricing_1Query(applicationType, propertyOccupancyType, householdIncomeType, householdIncome, propertyPurchasePrice, depositAmount, cashAmountRequired, currentMortgageLoanBalance, estimatedMarketValueofProperty, termInMonth, applicationEmpirica, fees, interimInterest, capitaliseFees, globalsVersion) {
				this.applicationType = applicationType || '';
				this.propertyOccupancyType = propertyOccupancyType || '';
				this.householdIncomeType = householdIncomeType || '';
				this.householdIncome = householdIncome;
				this.propertyPurchasePrice = propertyPurchasePrice;
				this.depositAmount = depositAmount;
				this.cashAmountRequired = cashAmountRequired;
				this.currentMortgageLoanBalance = currentMortgageLoanBalance;
				this.estimatedMarketValueofProperty = estimatedMarketValueofProperty;
				this.termInMonth = termInMonth;
				this.applicationEmpirica = applicationEmpirica;
				this.fees = fees;
				this.interimInterest = interimInterest;
				this.capitaliseFees = capitaliseFees;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecSelfEmployedCreditPricing_1Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function CapitecSelfEmployedCreditPricing_Query(applicationType, propertyOccupancyType, householdIncomeType, householdIncome, propertyPurchasePrice, depositAmount, cashAmountRequired, currentMortgageLoanBalance, estimatedMarketValueofProperty, termInMonth, applicationEmpirica, fees, interimInterest, capitaliseFees, globalsVersion) {
				this.applicationType = applicationType || '';
				this.propertyOccupancyType = propertyOccupancyType || '';
				this.householdIncomeType = householdIncomeType || '';
				this.householdIncome = householdIncome;
				this.propertyPurchasePrice = propertyPurchasePrice;
				this.depositAmount = depositAmount;
				this.cashAmountRequired = cashAmountRequired;
				this.currentMortgageLoanBalance = currentMortgageLoanBalance;
				this.estimatedMarketValueofProperty = estimatedMarketValueofProperty;
				this.termInMonth = termInMonth;
				this.applicationEmpirica = applicationEmpirica;
				this.fees = fees;
				this.interimInterest = interimInterest;
				this.capitaliseFees = capitaliseFees;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.CapitecSelfEmployedCreditPricing_Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function EdTest2_1Query(dummyInput, globalsVersion) {
				this.dummyInput = dummyInput;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.EdTest2_1Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function EdTest2_2Query(dummyInput, globalsVersion) {
				this.dummyInput = dummyInput;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.EdTest2_2Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function EdTest2_Query(dummyInput, globalsVersion) {
				this.dummyInput = dummyInput;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.EdTest2_Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function EdTest_1Query(input, globalsVersion) {
				this.input = input;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.EdTest_1Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function EdTest_Query(input, globalsVersion) {
				this.input = input;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.EdTest_Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function Jarrodtesttree_1Query(x, y, globalsVersion) {
				this.x = x;
				this.y = y;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.Jarrodtesttree_1Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function Jarrodtesttree_Query(x, y, globalsVersion) {
				this.x = x;
				this.y = y;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.Jarrodtesttree_Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function NewDecisionTree_1Query(parentString, parentInt, parentEnum, globalsVersion) {
				this.parentString = parentString || '';
				this.parentInt = parentInt;
				this.parentEnum = parentEnum || '';
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.NewDecisionTree_1Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function NewDecisionTree_Query(parentString, parentInt, parentEnum, globalsVersion) {
				this.parentString = parentString || '';
				this.parentInt = parentInt;
				this.parentEnum = parentEnum || '';
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.NewDecisionTree_Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function OriginationNewBusinessSPVDetermination_1Query(isCapitec, isAlpha, isDeveloper, lTV, isBlueBanner, globalsVersion) {
				this.isCapitec = isCapitec;
				this.isAlpha = isAlpha;
				this.isDeveloper = isDeveloper;
				this.lTV = lTV;
				this.isBlueBanner = isBlueBanner;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.OriginationNewBusinessSPVDetermination_1Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function OriginationNewBusinessSPVDetermination_Query(isCapitec, isAlpha, isDeveloper, lTV, isBlueBanner, globalsVersion) {
				this.isCapitec = isCapitec;
				this.isAlpha = isAlpha;
				this.isDeveloper = isDeveloper;
				this.lTV = lTV;
				this.isBlueBanner = isBlueBanner;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.OriginationNewBusinessSPVDetermination_Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function SAHomeLoansBCCScorecard_1Query(aT001, aT039, aT040, aT162, aT163, gE010, gE022, gE030, gE032, gE044, gE048, gE051, gE052, gE053, gE054, gE056, mL001, mL039, mL040, mL162, mL163, dM001AL, dM003AL, eQ001AL, eQ001FC, eQ001FL, eQ001GA, eQ001GR, eQ001HW, eQ001JW, eQ001OT, eQ003FC, eQ003FL, eQ004GA, eQ004GR, eQ004HW, eQ004JW, eQ004OT, nG001AL, nG008AL, nG011AL, pP001AL, pP001CA, pP003AL, pP005AL, pP011AL, pP046MB, pP046VF, pP047FC, pP047FL, pP047NL, pP047PL, pP054AL, pP068CE, pP071AL, pP074AL, pP075CC, pP076CC, pP077CC, pP082CC, pP083CC, pP087CE, pP087NL, pP087PL, pP104VF, globalsVersion) {
				this.aT001 = aT001;
				this.aT039 = aT039;
				this.aT040 = aT040;
				this.aT162 = aT162;
				this.aT163 = aT163;
				this.gE010 = gE010;
				this.gE022 = gE022;
				this.gE030 = gE030;
				this.gE032 = gE032;
				this.gE044 = gE044;
				this.gE048 = gE048;
				this.gE051 = gE051;
				this.gE052 = gE052;
				this.gE053 = gE053;
				this.gE054 = gE054;
				this.gE056 = gE056;
				this.mL001 = mL001;
				this.mL039 = mL039;
				this.mL040 = mL040;
				this.mL162 = mL162;
				this.mL163 = mL163;
				this.dM001AL = dM001AL;
				this.dM003AL = dM003AL || '';
				this.eQ001AL = eQ001AL;
				this.eQ001FC = eQ001FC;
				this.eQ001FL = eQ001FL;
				this.eQ001GA = eQ001GA;
				this.eQ001GR = eQ001GR;
				this.eQ001HW = eQ001HW;
				this.eQ001JW = eQ001JW;
				this.eQ001OT = eQ001OT;
				this.eQ003FC = eQ003FC;
				this.eQ003FL = eQ003FL;
				this.eQ004GA = eQ004GA;
				this.eQ004GR = eQ004GR;
				this.eQ004HW = eQ004HW;
				this.eQ004JW = eQ004JW;
				this.eQ004OT = eQ004OT;
				this.nG001AL = nG001AL;
				this.nG008AL = nG008AL;
				this.nG011AL = nG011AL;
				this.pP001AL = pP001AL;
				this.pP001CA = pP001CA;
				this.pP003AL = pP003AL;
				this.pP005AL = pP005AL;
				this.pP011AL = pP011AL;
				this.pP046MB = pP046MB;
				this.pP046VF = pP046VF;
				this.pP047FC = pP047FC;
				this.pP047FL = pP047FL;
				this.pP047NL = pP047NL;
				this.pP047PL = pP047PL;
				this.pP054AL = pP054AL;
				this.pP068CE = pP068CE;
				this.pP071AL = pP071AL;
				this.pP074AL = pP074AL;
				this.pP075CC = pP075CC;
				this.pP076CC = pP076CC;
				this.pP077CC = pP077CC;
				this.pP082CC = pP082CC;
				this.pP083CC = pP083CC;
				this.pP087CE = pP087CE;
				this.pP087NL = pP087NL;
				this.pP087PL = pP087PL;
				this.pP104VF = pP104VF;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.SAHomeLoansBCCScorecard_1Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function SAHomeLoansBCCScorecard_Query(aT001, aT039, aT040, aT162, aT163, gE010, gE022, gE030, gE032, gE044, gE048, gE051, gE052, gE053, gE054, gE056, mL001, mL039, mL040, mL162, mL163, dM001AL, dM003AL, eQ001AL, eQ001FC, eQ001FL, eQ001GA, eQ001GR, eQ001HW, eQ001JW, eQ001OT, eQ003FC, eQ003FL, eQ004GA, eQ004GR, eQ004HW, eQ004JW, eQ004OT, nG001AL, nG008AL, nG011AL, pP001AL, pP001CA, pP003AL, pP005AL, pP011AL, pP046MB, pP046VF, pP047FC, pP047FL, pP047NL, pP047PL, pP054AL, pP068CE, pP071AL, pP074AL, pP075CC, pP076CC, pP077CC, pP082CC, pP083CC, pP087CE, pP087NL, pP087PL, pP104VF, globalsVersion) {
				this.aT001 = aT001;
				this.aT039 = aT039;
				this.aT040 = aT040;
				this.aT162 = aT162;
				this.aT163 = aT163;
				this.gE010 = gE010;
				this.gE022 = gE022;
				this.gE030 = gE030;
				this.gE032 = gE032;
				this.gE044 = gE044;
				this.gE048 = gE048;
				this.gE051 = gE051;
				this.gE052 = gE052;
				this.gE053 = gE053;
				this.gE054 = gE054;
				this.gE056 = gE056;
				this.mL001 = mL001;
				this.mL039 = mL039;
				this.mL040 = mL040;
				this.mL162 = mL162;
				this.mL163 = mL163;
				this.dM001AL = dM001AL;
				this.dM003AL = dM003AL || '';
				this.eQ001AL = eQ001AL;
				this.eQ001FC = eQ001FC;
				this.eQ001FL = eQ001FL;
				this.eQ001GA = eQ001GA;
				this.eQ001GR = eQ001GR;
				this.eQ001HW = eQ001HW;
				this.eQ001JW = eQ001JW;
				this.eQ001OT = eQ001OT;
				this.eQ003FC = eQ003FC;
				this.eQ003FL = eQ003FL;
				this.eQ004GA = eQ004GA;
				this.eQ004GR = eQ004GR;
				this.eQ004HW = eQ004HW;
				this.eQ004JW = eQ004JW;
				this.eQ004OT = eQ004OT;
				this.nG001AL = nG001AL;
				this.nG008AL = nG008AL;
				this.nG011AL = nG011AL;
				this.pP001AL = pP001AL;
				this.pP001CA = pP001CA;
				this.pP003AL = pP003AL;
				this.pP005AL = pP005AL;
				this.pP011AL = pP011AL;
				this.pP046MB = pP046MB;
				this.pP046VF = pP046VF;
				this.pP047FC = pP047FC;
				this.pP047FL = pP047FL;
				this.pP047NL = pP047NL;
				this.pP047PL = pP047PL;
				this.pP054AL = pP054AL;
				this.pP068CE = pP068CE;
				this.pP071AL = pP071AL;
				this.pP074AL = pP074AL;
				this.pP075CC = pP075CC;
				this.pP076CC = pP076CC;
				this.pP077CC = pP077CC;
				this.pP082CC = pP082CC;
				this.pP083CC = pP083CC;
				this.pP087CE = pP087CE;
				this.pP087NL = pP087NL;
				this.pP087PL = pP087PL;
				this.pP104VF = pP104VF;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.SAHomeLoansBCCScorecard_Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function SubtreeTest_1Query(empirica, creditMatrix, incomeType, newva56, newvar, globalsVersion) {
				this.empirica = empirica;
				this.creditMatrix = creditMatrix || '';
				this.incomeType = incomeType || '';
				this.newva56 = newva56 || '';
				this.newvar = newvar || '';
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.SubtreeTest_1Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function SubtreeTest_Query(empirica, creditMatrix, incomeType, newva56, newvar, globalsVersion) {
				this.empirica = empirica;
				this.creditMatrix = creditMatrix || '';
				this.incomeType = incomeType || '';
				this.newva56 = newva56 || '';
				this.newvar = newvar || '';
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.SubtreeTest_Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function TestProcess_1Query(newvarstr1, globalsVersion) {
				this.newvarstr1 = newvarstr1 || '';
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.TestProcess_1Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function TestProcess_Query(newvarstr1, globalsVersion) {
				this.newvarstr1 = newvarstr1 || '';
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.TestProcess_Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function TestSubtree_1Query(newvar, globalsVersion) {
				this.newvar = newvar || '';
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.TestSubtree_1Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function TestSubtree_Query(newvar, globalsVersion) {
				this.newvar = newvar || '';
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.TestSubtree_Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function Test_1Query(check, globalsVersion) {
				this.check = check;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.Test_1Query,SAHL.Services.Interfaces.DecisionTree';
				}
			function Test_Query(check, globalsVersion) {
				this.check = check;
				this.globalsVersion = globalsVersion;
				this._name = 'SAHL.Services.Interfaces.DecisionTree.Queries.Test_Query,SAHL.Services.Interfaces.DecisionTree';
				}

			return {
				CapitecAffordabilityInterestRate_1Query: CapitecAffordabilityInterestRate_1Query,
				CapitecAffordabilityInterestRate_Query: CapitecAffordabilityInterestRate_Query,
				CapitecAlphaCreditPricing_1Query: CapitecAlphaCreditPricing_1Query,
				CapitecAlphaCreditPricing_Query: CapitecAlphaCreditPricing_Query,
				CapitecApplicationCreditPolicy_1Query: CapitecApplicationCreditPolicy_1Query,
				CapitecApplicationCreditPolicy_2Query: CapitecApplicationCreditPolicy_2Query,
				CapitecApplicationCreditPolicy_Query: CapitecApplicationCreditPolicy_Query,
				CapitecClientCreditBureauAssessment_1Query: CapitecClientCreditBureauAssessment_1Query,
				CapitecClientCreditBureauAssessment_2Query: CapitecClientCreditBureauAssessment_2Query,
				CapitecClientCreditBureauAssessment_Query: CapitecClientCreditBureauAssessment_Query,
				CapitecCreditPricing_1Query: CapitecCreditPricing_1Query,
				CapitecCreditPricing_Query: CapitecCreditPricing_Query,
				CapitecOriginationCreditPricing_1Query: CapitecOriginationCreditPricing_1Query,
				CapitecOriginationCreditPricing_Query: CapitecOriginationCreditPricing_Query,
				CapitecOriginationPricingforRisk_1Query: CapitecOriginationPricingforRisk_1Query,
				CapitecOriginationPricingforRisk_2Query: CapitecOriginationPricingforRisk_2Query,
				CapitecOriginationPricingforRisk_Query: CapitecOriginationPricingforRisk_Query,
				CapitecSalariedCreditPricing_1Query: CapitecSalariedCreditPricing_1Query,
				CapitecSalariedCreditPricing_Query: CapitecSalariedCreditPricing_Query,
				CapitecSalariedwithDeductionCreditPricing_1Query: CapitecSalariedwithDeductionCreditPricing_1Query,
				CapitecSalariedwithDeductionCreditPricing_Query: CapitecSalariedwithDeductionCreditPricing_Query,
				CapitecSelfEmployedCreditPricing_1Query: CapitecSelfEmployedCreditPricing_1Query,
				CapitecSelfEmployedCreditPricing_Query: CapitecSelfEmployedCreditPricing_Query,
				EdTest2_1Query: EdTest2_1Query,
				EdTest2_2Query: EdTest2_2Query,
				EdTest2_Query: EdTest2_Query,
				EdTest_1Query: EdTest_1Query,
				EdTest_Query: EdTest_Query,
				Jarrodtesttree_1Query: Jarrodtesttree_1Query,
				Jarrodtesttree_Query: Jarrodtesttree_Query,
				NewDecisionTree_1Query: NewDecisionTree_1Query,
				NewDecisionTree_Query: NewDecisionTree_Query,
				OriginationNewBusinessSPVDetermination_1Query: OriginationNewBusinessSPVDetermination_1Query,
				OriginationNewBusinessSPVDetermination_Query: OriginationNewBusinessSPVDetermination_Query,
				SAHomeLoansBCCScorecard_1Query: SAHomeLoansBCCScorecard_1Query,
				SAHomeLoansBCCScorecard_Query: SAHomeLoansBCCScorecard_Query,
				SubtreeTest_1Query: SubtreeTest_1Query,
				SubtreeTest_Query: SubtreeTest_Query,
				TestProcess_1Query: TestProcess_1Query,
				TestProcess_Query: TestProcess_Query,
				TestSubtree_1Query: TestSubtree_1Query,
				TestSubtree_Query: TestSubtree_Query,
				Test_1Query: Test_1Query,
				Test_Query: Test_Query
			};
		}());
		return shared;
	}]);