'use strict';

angular.module('SAHL.Services.Interfaces.Capitec.queries', []).
	factory('$capitecQueries', [function () {
		var shared = (function () {
						function GetAffordabilityInterestRateQuery(householdIncome, deposit, calcRate, interestRateQuery) {
				this.householdIncome = householdIncome;
				this.deposit = deposit;
				this.calcRate = calcRate;
				this.interestRateQuery = interestRateQuery;
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetAffordabilityInterestRateQuery,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.calcRate!==0 && !this.calcRate) {
							results.push({'calcRate': 'Calc Rate is required'});
						}
					if(this.calcRate && isNaN(this.calcRate)) {
								results.push({'calcRate':'Calc Rate must be a number.'});
						}
						if(!(typeof(this.calcRate) === 'undefined' || this.calcRate === null) && !isNaN(this.calcRate) && (this.calcRate < 0.01 || this.calcRate > 1 )) {
							results.push({'calcRate': 'Interest Rate must be between 1 and 100 percent'});
						}

					if(this.householdIncome && isNaN(this.householdIncome)) {
								results.push({'householdIncome':'Household Income must be a number.'});
						}
						if(!(typeof(this.householdIncome) === 'undefined' || this.householdIncome === null) && !isNaN(this.householdIncome) && (this.householdIncome < 0.1 || this.householdIncome > 1.79769313486232E+308 )) {
							results.push({'householdIncome': 'Income must be greater than 0'});
						}

					if(this.householdIncome!==0 && !this.householdIncome) {
							results.push({'householdIncome': 'Household Income is required'});
						}
					return results;
			};
		}
			function GetApplicationPurposeQuery() {
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetApplicationPurposeQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetApplicationResultQuery(applicationID) {
				this.applicationID = applicationID;
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetApplicationResultQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetApplicationByIdentityNumberQuery(identityNumber, statusTypeName) {
				this.identityNumber = identityNumber || '';
				this.statusTypeName = statusTypeName || '';
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetApplicationByIdentityNumberQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetNewGuidQuery() {
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetNewGuidQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetOccupancyTypesQuery() {
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetOccupancyTypesQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetDeclarationTypesQuery() {
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetDeclarationTypesQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetEmploymentTypesQuery() {
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetEmploymentTypesQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetRolesFromUserQuery(userId) {
				this.userId = userId;
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetRolesFromUserQuery,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.userId!==0 && !this.userId) {
							results.push({'userId': 'User Id is required'});
						}
					return results;
			};
		}
			function GetSalutationsQuery() {
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetSalutationsQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetApplicationByUserQuery(userName) {
				this.userName = userName || '';
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetApplicationByUserQuery,SAHL.Services.Interfaces.Capitec';
				}
			function CalculateApplicationScenarioNewPurchaseQuery(loanDetails) {
				this.loanDetails = loanDetails;
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.CalculateApplicationScenarioNewPurchaseQuery,SAHL.Services.Interfaces.Capitec';
				}
			function CalculateApplicationScenarioSwitchQuery(loanDetails) {
				this.loanDetails = loanDetails;
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.CalculateApplicationScenarioSwitchQuery,SAHL.Services.Interfaces.Capitec';
				}
			function FilterBranchesByNameQuery(branchName) {
				this.branchName = branchName || '';
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.FilterBranchesByNameQuery,SAHL.Services.Interfaces.Capitec';
				}
			function FilterCitiesByNameQuery(cityNameFilter) {
				this.cityNameFilter = cityNameFilter || '';
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.FilterCitiesByNameQuery,SAHL.Services.Interfaces.Capitec';
				}
			function FilterSuburbsByNameQuery(suburbNameFilter) {
				this.suburbNameFilter = suburbNameFilter || '';
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.FilterSuburbsByNameQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetCalculatorFeeQuery(offerType, loanAmount, cashOut) {
				this.offerType = offerType;
				this.loanAmount = loanAmount;
				this.cashOut = cashOut;
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetCalculatorFeeQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetCitiesQuery() {
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetCitiesQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetCityQuery(id) {
				this.id = id;
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetCityQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetCountriesQuery() {
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetCountriesQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetProvinceQuery(id) {
				this.id = id;
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetProvinceQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetBranchQuery(id) {
				this.id = id;
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetBranchQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetProvincesQuery() {
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetProvincesQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetBranchesQuery() {
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetBranchesQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetControlValueQuery(controlDescription) {
				this.controlDescription = controlDescription || '';
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetControlValueQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetRoleTypesQuery() {
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetRoleTypesQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetSuburbProvinceQuery(id) {
				this.id = id;
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetSuburbProvinceQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetSuburbQuery(id) {
				this.id = id;
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetSuburbQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetSuburbsQuery() {
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetSuburbsQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetUserFromAuthTokenQuery(authenticationToken) {
				this.authenticationToken = authenticationToken;
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetUserFromAuthTokenQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetUserQuery(id) {
				this.id = id;
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetUserQuery,SAHL.Services.Interfaces.Capitec';
				}
			function GetUsersQuery() {
				this._name = 'SAHL.Services.Interfaces.Capitec.Queries.GetUsersQuery,SAHL.Services.Interfaces.Capitec';
				}

			return {
				GetAffordabilityInterestRateQuery: GetAffordabilityInterestRateQuery,
				GetApplicationPurposeQuery: GetApplicationPurposeQuery,
				GetApplicationResultQuery: GetApplicationResultQuery,
				GetApplicationByIdentityNumberQuery: GetApplicationByIdentityNumberQuery,
				GetNewGuidQuery: GetNewGuidQuery,
				GetOccupancyTypesQuery: GetOccupancyTypesQuery,
				GetDeclarationTypesQuery: GetDeclarationTypesQuery,
				GetEmploymentTypesQuery: GetEmploymentTypesQuery,
				GetRolesFromUserQuery: GetRolesFromUserQuery,
				GetSalutationsQuery: GetSalutationsQuery,
				GetApplicationByUserQuery: GetApplicationByUserQuery,
				CalculateApplicationScenarioNewPurchaseQuery: CalculateApplicationScenarioNewPurchaseQuery,
				CalculateApplicationScenarioSwitchQuery: CalculateApplicationScenarioSwitchQuery,
				FilterBranchesByNameQuery: FilterBranchesByNameQuery,
				FilterCitiesByNameQuery: FilterCitiesByNameQuery,
				FilterSuburbsByNameQuery: FilterSuburbsByNameQuery,
				GetCalculatorFeeQuery: GetCalculatorFeeQuery,
				GetCitiesQuery: GetCitiesQuery,
				GetCityQuery: GetCityQuery,
				GetCountriesQuery: GetCountriesQuery,
				GetProvinceQuery: GetProvinceQuery,
				GetBranchQuery: GetBranchQuery,
				GetProvincesQuery: GetProvincesQuery,
				GetBranchesQuery: GetBranchesQuery,
				GetControlValueQuery: GetControlValueQuery,
				GetRoleTypesQuery: GetRoleTypesQuery,
				GetSuburbProvinceQuery: GetSuburbProvinceQuery,
				GetSuburbQuery: GetSuburbQuery,
				GetSuburbsQuery: GetSuburbsQuery,
				GetUserFromAuthTokenQuery: GetUserFromAuthTokenQuery,
				GetUserQuery: GetUserQuery,
				GetUsersQuery: GetUsersQuery
			};
		}());
		return shared;
	}]);