'use strict';

angular.module('SAHL.Services.Interfaces.Capitec.sharedmodels', []).
	factory('$capitecSharedModels', [function () {
		var shared = (function () {
					function NewPurchaseApplication(newPurchaseLoanDetails, applicants, userId) {
				this.newPurchaseLoanDetails = newPurchaseLoanDetails;
				this.applicants = applicants;
				this.userId = userId;
				this._name = 'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.NewPurchaseApplication,SAHL.Services.Interfaces.Capitec';
				}
		function NewPurchaseLoanDetails(occupancyType, incomeType, householdIncome, purchasePrice, deposit, fees, termInMonths, capitaliseFees) {
				this.occupancyType = occupancyType;
				this.incomeType = incomeType;
				this.householdIncome = householdIncome;
				this.purchasePrice = purchasePrice;
				this.deposit = deposit;
				this.fees = fees;
				this.termInMonths = termInMonths;
				this.capitaliseFees = capitaliseFees;
				this._name = 'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.NewPurchaseLoanDetails,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.purchasePrice && isNaN(this.purchasePrice)) {
								results.push({'purchasePrice':'Purchase Price must be a number.'});
						}
						if(!(typeof(this.purchasePrice) === 'undefined' || this.purchasePrice === null) && !isNaN(this.purchasePrice) && (this.purchasePrice < 1 || this.purchasePrice > 2147483647 )) {
							results.push({'purchasePrice': 'Purchase Price must be greater than R 0'});
						}

					if(this.purchasePrice!==0 && !this.purchasePrice) {
							results.push({'purchasePrice': 'Purchase Price is required'});
						}
					if(this.deposit!==0 && !this.deposit) {
							results.push({'deposit': 'Cash Deposit is required'});
						}
					if(this.deposit && isNaN(this.deposit)) {
								results.push({'deposit':'Deposit must be a number.'});
						}
						if(!(typeof(this.deposit) === 'undefined' || this.deposit === null) && !isNaN(this.deposit) && (this.deposit < 0 || this.deposit > 2147483647 )) {
							results.push({'deposit': 'Cash Deposit must be a number'});
						}

					if(this.occupancyType!==0 && !this.occupancyType) {
							results.push({'occupancyType': 'Occupancy Type is required'});
						}
					if(this.incomeType!==0 && !this.incomeType) {
							results.push({'incomeType': 'Majority Income Type is required'});
						}
					if(this.householdIncome && isNaN(this.householdIncome)) {
								results.push({'householdIncome':'Household Income must be a number.'});
						}
						if(!(typeof(this.householdIncome) === 'undefined' || this.householdIncome === null) && !isNaN(this.householdIncome) && (this.householdIncome < 1 || this.householdIncome > 1.79769313486232E+308 )) {
							results.push({'householdIncome': 'Total Gross Income of all Applicants must be greater than R 0'});
						}

					if(this.householdIncome!==0 && !this.householdIncome) {
							results.push({'householdIncome': 'Total Gross Income of all Applicants is required'});
						}
					return results;
			};
		}
		function Applicant(information, residentialAddress, employmentDetails, declarations) {
				this.information = information;
				this.residentialAddress = residentialAddress;
				this.employmentDetails = employmentDetails;
				this.declarations = declarations;
				this._name = 'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.Applicant,SAHL.Services.Interfaces.Capitec';
				}
		function ApplicantInformation(identityNumber, firstName, surname, salutationEnumId, homePhoneNumber, workPhoneNumber, cellPhoneNumber, emailAddress, dateOfBirth, title, mainContact) {
				this.identityNumber = identityNumber || '';
				this.firstName = firstName || '';
				this.surname = surname || '';
				this.salutationEnumId = salutationEnumId;
				this.homePhoneNumber = homePhoneNumber || '';
				this.workPhoneNumber = workPhoneNumber || '';
				this.cellPhoneNumber = cellPhoneNumber || '';
				this.emailAddress = emailAddress || '';
				this.dateOfBirth = dateOfBirth;
				this.title = title || '';
				this.mainContact = mainContact;
				this._name = 'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.ApplicantInformation,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(!(typeof(this.identityNumber) === 'undefined' || this.identityNumber === null)){
						var identityNumberFound = this.identityNumber.match(/\d{13}/);
						if(!identityNumberFound || identityNumberFound.length <= 0) {
							results.push({'identityNumber': 'Please provide a valid South African ID Number'});
							}
						}
					if(this.firstName!==0 && !this.firstName) {
							results.push({'firstName': 'First Name is required'});
						}
					if(this.surname!==0 && !this.surname) {
							results.push({'surname': 'Surname is required'});
						}
					if(this.salutationEnumId!==0 && !this.salutationEnumId) {
							results.push({'salutationEnumId': 'A Title is required'});
						}
					if(!(typeof(this.homePhoneNumber) === 'undefined' || this.homePhoneNumber === null)){
						var homePhoneNumberFound = this.homePhoneNumber.match(/^$|0[0-8]\d{8}/);
						if(!homePhoneNumberFound || homePhoneNumberFound.length <= 0) {
							results.push({'homePhoneNumber': 'Home Phone Number is invalid'});
							}
						}
					if(!(typeof(this.workPhoneNumber) === 'undefined' || this.workPhoneNumber === null)){
						var workPhoneNumberFound = this.workPhoneNumber.match(/^$|0[0-8]\d{8}/);
						if(!workPhoneNumberFound || workPhoneNumberFound.length <= 0) {
							results.push({'workPhoneNumber': 'Work Phone Number is invalid'});
							}
						}
					if(!(typeof(this.cellPhoneNumber) === 'undefined' || this.cellPhoneNumber === null)){
						var cellPhoneNumberFound = this.cellPhoneNumber.match(/0[0-8]\d{8}/);
						if(!cellPhoneNumberFound || cellPhoneNumberFound.length <= 0) {
							results.push({'cellPhoneNumber': 'Cell Phone Number is invalid'});
							}
						}
					if(!(typeof(this.emailAddress) === 'undefined' || this.emailAddress === null)){
						var emailAddressFound = this.emailAddress.match(/([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})/);
						if(!emailAddressFound || emailAddressFound.length <= 0) {
							results.push({'emailAddress': 'Please provide a valid email address'});
							}
						}
					if(!(typeof(this.dateOfBirth) === 'undefined' || this.dateOfBirth === null)){
						var dateOfBirthFound = this.dateOfBirth.match(/^\d{4}-\d{1,2}-\d{1,2}$/);
						if(!dateOfBirthFound || dateOfBirthFound.length <= 0) {
							results.push({'dateOfBirth': 'Please provide a valid date of birth'});
							}
						}
					return results;
			};
		}
		function ApplicantResidentialAddress(unitNumber, buildingNumber, buildingName, streetNumber, streetName, suburb, province, city, postalCode, suburbId) {
				this.unitNumber = unitNumber || '';
				this.buildingNumber = buildingNumber || '';
				this.buildingName = buildingName || '';
				this.streetNumber = streetNumber || '';
				this.streetName = streetName || '';
				this.suburb = suburb || '';
				this.province = province || '';
				this.city = city || '';
				this.postalCode = postalCode || '';
				this.suburbId = suburbId;
				this._name = 'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.ApplicantResidentialAddress,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.streetNumber!==0 && !this.streetNumber) {
							results.push({'streetNumber': 'Street Number is required'});
						}
					if(this.streetName!==0 && !this.streetName) {
							results.push({'streetName': 'Street Name is required'});
						}
					if(this.suburb!==0 && !this.suburb) {
							results.push({'suburb': 'Suburb is required'});
						}
					return results;
			};
		}
		function ApplicantEmploymentDetails(employmentTypeEnumId, salariedDetails, salariedWithCommissionDetails, salariedWithHousingAllowanceDetails, selfEmployedDetails, unEmployedDetails) {
				this.employmentTypeEnumId = employmentTypeEnumId;
				this.salariedDetails = salariedDetails;
				this.salariedWithCommissionDetails = salariedWithCommissionDetails;
				this.salariedWithHousingAllowanceDetails = salariedWithHousingAllowanceDetails;
				this.selfEmployedDetails = selfEmployedDetails;
				this.unEmployedDetails = unEmployedDetails;
				this._name = 'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.ApplicantEmploymentDetails,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.employmentTypeEnumId!==0 && !this.employmentTypeEnumId) {
							results.push({'employmentTypeEnumId': 'An employment type is required'});
						}
					return results;
			};
		}
		function SalariedDetails(grossMonthlyIncome) {
				this.grossMonthlyIncome = grossMonthlyIncome;
				this._name = 'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.SalariedDetails,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.grossMonthlyIncome && isNaN(this.grossMonthlyIncome)) {
								results.push({'grossMonthlyIncome':'Gross Monthly Income must be a number.'});
						}
						if(!(typeof(this.grossMonthlyIncome) === 'undefined' || this.grossMonthlyIncome === null) && !isNaN(this.grossMonthlyIncome) && (this.grossMonthlyIncome < 1 || this.grossMonthlyIncome > 1.79769313486232E+308 )) {
							results.push({'grossMonthlyIncome': 'Gross Monthly Individual Income must be a number.'});
						}

					if(this.grossMonthlyIncome!==0 && !this.grossMonthlyIncome) {
							results.push({'grossMonthlyIncome': 'Gross Monthly Individual Income is required.'});
						}
					return results;
			};
		}
		function SalariedWithCommissionDetails(grossMonthlyIncome, threeMonthAverageCommission) {
				this.grossMonthlyIncome = grossMonthlyIncome;
				this.threeMonthAverageCommission = threeMonthAverageCommission;
				this._name = 'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.SalariedWithCommissionDetails,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.grossMonthlyIncome!==0 && !this.grossMonthlyIncome) {
							results.push({'grossMonthlyIncome': 'Gross Monthly Individual Income is required.'});
						}
					if(this.grossMonthlyIncome && isNaN(this.grossMonthlyIncome)) {
								results.push({'grossMonthlyIncome':'Gross Monthly Income must be a number.'});
						}
						if(!(typeof(this.grossMonthlyIncome) === 'undefined' || this.grossMonthlyIncome === null) && !isNaN(this.grossMonthlyIncome) && (this.grossMonthlyIncome < 1 || this.grossMonthlyIncome > 1.79769313486232E+308 )) {
							results.push({'grossMonthlyIncome': 'Gross Monthly Individual Income must be a number.'});
						}

					if(this.threeMonthAverageCommission && isNaN(this.threeMonthAverageCommission)) {
								results.push({'threeMonthAverageCommission':'Three Month Average Commission must be a number.'});
						}
						if(!(typeof(this.threeMonthAverageCommission) === 'undefined' || this.threeMonthAverageCommission === null) && !isNaN(this.threeMonthAverageCommission) && (this.threeMonthAverageCommission < 1 || this.threeMonthAverageCommission > 1.79769313486232E+308 )) {
							results.push({'threeMonthAverageCommission': '3 Month Average Commission must be a number.'});
						}

					if(this.threeMonthAverageCommission!==0 && !this.threeMonthAverageCommission) {
							results.push({'threeMonthAverageCommission': '3 Month Average Commission is required.'});
						}
					return results;
			};
		}
		function SalariedWithHousingAllowanceDetails(grossMonthlyIncome, housingAllowance) {
				this.grossMonthlyIncome = grossMonthlyIncome;
				this.housingAllowance = housingAllowance;
				this._name = 'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.SalariedWithHousingAllowanceDetails,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.grossMonthlyIncome && isNaN(this.grossMonthlyIncome)) {
								results.push({'grossMonthlyIncome':'Gross Monthly Income must be a number.'});
						}
						if(!(typeof(this.grossMonthlyIncome) === 'undefined' || this.grossMonthlyIncome === null) && !isNaN(this.grossMonthlyIncome) && (this.grossMonthlyIncome < 1 || this.grossMonthlyIncome > 2147483647 )) {
							results.push({'grossMonthlyIncome': 'Gross Monthly Individual Income must be a number.'});
						}

					if(this.grossMonthlyIncome!==0 && !this.grossMonthlyIncome) {
							results.push({'grossMonthlyIncome': 'Gross Monthly Individual Income is required.'});
						}
					if(this.housingAllowance!==0 && !this.housingAllowance) {
							results.push({'housingAllowance': 'Housing Allowance is required for the selected employment type.'});
						}
					if(this.housingAllowance && isNaN(this.housingAllowance)) {
								results.push({'housingAllowance':'Housing Allowance must be a number.'});
						}
						if(!(typeof(this.housingAllowance) === 'undefined' || this.housingAllowance === null) && !isNaN(this.housingAllowance) && (this.housingAllowance < 1 || this.housingAllowance > 2147483647 )) {
							results.push({'housingAllowance': 'Housing Allowance must be a number.'});
						}

					return results;
			};
		}
		function SelfEmployedDetails(grossMonthlyIncome) {
				this.grossMonthlyIncome = grossMonthlyIncome;
				this._name = 'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.SelfEmployedDetails,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.grossMonthlyIncome!==0 && !this.grossMonthlyIncome) {
							results.push({'grossMonthlyIncome': 'Gross Monthly Individual Income is required.'});
						}
					if(this.grossMonthlyIncome && isNaN(this.grossMonthlyIncome)) {
								results.push({'grossMonthlyIncome':'Gross Monthly Income must be a number.'});
						}
						if(!(typeof(this.grossMonthlyIncome) === 'undefined' || this.grossMonthlyIncome === null) && !isNaN(this.grossMonthlyIncome) && (this.grossMonthlyIncome < 1 || this.grossMonthlyIncome > 1.79769313486232E+308 )) {
							results.push({'grossMonthlyIncome': 'Gross Monthly Individual Income must be a number.'});
						}

					return results;
			};
		}
		function UnEmployedDetails(grossMonthlyIncome) {
				this.grossMonthlyIncome = grossMonthlyIncome;
				this._name = 'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.UnEmployedDetails,SAHL.Services.Interfaces.Capitec';
				}
		function ApplicantDeclarations(incomeContributor, allowCreditBureauCheck, hasCapitecTransactionAccount, marriedInCommunityOfProperty) {
				this.incomeContributor = incomeContributor;
				this.allowCreditBureauCheck = allowCreditBureauCheck;
				this.hasCapitecTransactionAccount = hasCapitecTransactionAccount;
				this.marriedInCommunityOfProperty = marriedInCommunityOfProperty;
				this._name = 'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.ApplicantDeclarations,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.incomeContributor!==0 && !this.incomeContributor) {
							results.push({'incomeContributor': 'Please indicate if the client is an income contributor'});
						}
					if(this.allowCreditBureauCheck!==0 && !this.allowCreditBureauCheck) {
							results.push({'allowCreditBureauCheck': 'Please indicate if the client will allow a credit bureau check to be conducted on them'});
						}
					if(this.hasCapitecTransactionAccount!==0 && !this.hasCapitecTransactionAccount) {
							results.push({'hasCapitecTransactionAccount': 'Please indicate if the client has a Capitec transactional account'});
						}
					if(this.marriedInCommunityOfProperty!==0 && !this.marriedInCommunityOfProperty) {
							results.push({'marriedInCommunityOfProperty': 'Please indicate if the client is married in community of property'});
						}
					return results;
			};
		}
		function SwitchLoanApplication(switchLoanDetails, applicants, userId) {
				this.switchLoanDetails = switchLoanDetails;
				this.applicants = applicants;
				this.userId = userId;
				this._name = 'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.SwitchLoanApplication,SAHL.Services.Interfaces.Capitec';
				}
		function SwitchLoanDetails(occupancyType, incomeType, householdIncome, estimatedMarketValueOfTheHome, cashRequired, currentBalance, fees, interimInterest, termInMonths, capitaliseFees) {
				this.occupancyType = occupancyType;
				this.incomeType = incomeType;
				this.householdIncome = householdIncome;
				this.estimatedMarketValueOfTheHome = estimatedMarketValueOfTheHome;
				this.cashRequired = cashRequired;
				this.currentBalance = currentBalance;
				this.fees = fees;
				this.interimInterest = interimInterest;
				this.termInMonths = termInMonths;
				this.capitaliseFees = capitaliseFees;
				this._name = 'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.SwitchLoanDetails,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.estimatedMarketValueOfTheHome && isNaN(this.estimatedMarketValueOfTheHome)) {
								results.push({'estimatedMarketValueOfTheHome':'Estimated Market Value Of The Home must be a number.'});
						}
						if(!(typeof(this.estimatedMarketValueOfTheHome) === 'undefined' || this.estimatedMarketValueOfTheHome === null) && !isNaN(this.estimatedMarketValueOfTheHome) && (this.estimatedMarketValueOfTheHome < 1 || this.estimatedMarketValueOfTheHome > 1.79769313486232E+308 )) {
							results.push({'estimatedMarketValueOfTheHome': 'Estimated Market Value of the Home must be greater than R 0'});
						}

					if(this.estimatedMarketValueOfTheHome!==0 && !this.estimatedMarketValueOfTheHome) {
							results.push({'estimatedMarketValueOfTheHome': 'Estimated Market Value of the Home is required'});
						}
					if(this.cashRequired && isNaN(this.cashRequired)) {
								results.push({'cashRequired':'Cash Required must be a number.'});
						}
						if(!(typeof(this.cashRequired) === 'undefined' || this.cashRequired === null) && !isNaN(this.cashRequired) && (this.cashRequired < 0 || this.cashRequired > 1.79769313486232E+308 )) {
							results.push({'cashRequired': 'Cash Required must be a number'});
						}

					if(this.cashRequired!==0 && !this.cashRequired) {
							results.push({'cashRequired': 'Cash Required is required'});
						}
					if(this.currentBalance!==0 && !this.currentBalance) {
							results.push({'currentBalance': 'Current Outstanding Home Loan Balance is required'});
						}
					if(this.currentBalance && isNaN(this.currentBalance)) {
								results.push({'currentBalance':'Current Balance must be a number.'});
						}
						if(!(typeof(this.currentBalance) === 'undefined' || this.currentBalance === null) && !isNaN(this.currentBalance) && (this.currentBalance < 1 || this.currentBalance > 1.79769313486232E+308 )) {
							results.push({'currentBalance': 'Current Outstanding Home Loan Balance must be greater than R 0'});
						}

					if(this.occupancyType!==0 && !this.occupancyType) {
							results.push({'occupancyType': 'Occupancy Type is required'});
						}
					if(this.incomeType!==0 && !this.incomeType) {
							results.push({'incomeType': 'Majority Income Type is required'});
						}
					if(this.householdIncome!==0 && !this.householdIncome) {
							results.push({'householdIncome': 'Total Gross Income of all Applicants is required'});
						}
					if(this.householdIncome && isNaN(this.householdIncome)) {
								results.push({'householdIncome':'Household Income must be a number.'});
						}
						if(!(typeof(this.householdIncome) === 'undefined' || this.householdIncome === null) && !isNaN(this.householdIncome) && (this.householdIncome < 1 || this.householdIncome > 1.79769313486232E+308 )) {
							results.push({'householdIncome': 'Total Gross Income of all Applicants must be greater than R 0'});
						}

					return results;
			};
		}

			return {
				NewPurchaseApplication: NewPurchaseApplication,
				NewPurchaseLoanDetails: NewPurchaseLoanDetails,
				Applicant: Applicant,
				ApplicantInformation: ApplicantInformation,
				ApplicantResidentialAddress: ApplicantResidentialAddress,
				ApplicantEmploymentDetails: ApplicantEmploymentDetails,
				SalariedDetails: SalariedDetails,
				SalariedWithCommissionDetails: SalariedWithCommissionDetails,
				SalariedWithHousingAllowanceDetails: SalariedWithHousingAllowanceDetails,
				SelfEmployedDetails: SelfEmployedDetails,
				UnEmployedDetails: UnEmployedDetails,
				ApplicantDeclarations: ApplicantDeclarations,
				SwitchLoanApplication: SwitchLoanApplication,
				SwitchLoanDetails: SwitchLoanDetails
			};
		}());
		return shared;
	}]).
	factory('$capitecSharedModelsContainer', ['$capitecSharedModels',function ($capitecSharedModels) {
		var shared = (function () {
			var Container = {
				'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.NewPurchaseApplication,SAHL.Services.Interfaces.Capitec': $capitecSharedModels.NewPurchaseApplication,
				'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.NewPurchaseLoanDetails,SAHL.Services.Interfaces.Capitec': $capitecSharedModels.NewPurchaseLoanDetails,
				'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.Applicant,SAHL.Services.Interfaces.Capitec': $capitecSharedModels.Applicant,
				'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.ApplicantInformation,SAHL.Services.Interfaces.Capitec': $capitecSharedModels.ApplicantInformation,
				'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.ApplicantResidentialAddress,SAHL.Services.Interfaces.Capitec': $capitecSharedModels.ApplicantResidentialAddress,
				'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.ApplicantEmploymentDetails,SAHL.Services.Interfaces.Capitec': $capitecSharedModels.ApplicantEmploymentDetails,
				'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.SalariedDetails,SAHL.Services.Interfaces.Capitec': $capitecSharedModels.SalariedDetails,
				'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.SalariedWithCommissionDetails,SAHL.Services.Interfaces.Capitec': $capitecSharedModels.SalariedWithCommissionDetails,
				'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.SalariedWithHousingAllowanceDetails,SAHL.Services.Interfaces.Capitec': $capitecSharedModels.SalariedWithHousingAllowanceDetails,
				'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.SelfEmployedDetails,SAHL.Services.Interfaces.Capitec': $capitecSharedModels.SelfEmployedDetails,
				'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.UnEmployedDetails,SAHL.Services.Interfaces.Capitec': $capitecSharedModels.UnEmployedDetails,
				'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.ApplicantDeclarations,SAHL.Services.Interfaces.Capitec': $capitecSharedModels.ApplicantDeclarations,
				'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.SwitchLoanApplication,SAHL.Services.Interfaces.Capitec': $capitecSharedModels.SwitchLoanApplication,
				'SAHL.Services.Interfaces.Capitec.ViewModels.Apply.SwitchLoanDetails,SAHL.Services.Interfaces.Capitec': $capitecSharedModels.SwitchLoanDetails
			}
			return {
				Container : Container
			};
		}());
		return shared;
	}]);