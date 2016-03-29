var capitecqueryFakes = function(){
	function SystemMessages(){
		return {
			"$type" : "SAHL.Core.SystemMessages.SystemMessageCollection, SAHL.Core",
			"HasErrors" : false,
			"HasWarnings" : false,
			"ErrorMessages" : function(){
				var retObj = this.AllMessages;
				retObj.$values = $.grep(this.AllMessages.$values,function(msg){
					return ((msg.Severity === 1)||(msg.Severity === 3));
				});
				return retObj;
			},
			"WarningMessages" : function(){
				var retObj = this.AllMessages;
				retObj.$values = $.grep(this.AllMessages.$values,function(msg){
					return msg.Severity === 0;
				});
				return retObj;
			},
			"AllMessages" : {
				"$id" : "99",
				"$type" : "System.Collections.Generic.List`1[[SAHL.Core.SystemMessages.ISystemMessage, SAHL.Core",
				"$values" : []
			},
			"AddError": function(message){
				var message = {"$type" : "SAHL.Core.SystemMessages.SystemMessage, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null","Severity" : 1,"Message" : message};
				this.AllMessages.$values.push(message);
				this.HasErrors = true;
			},
			"AddWarning": function(message){
				var message = {"$type" : "SAHL.Core.SystemMessages.SystemMessage, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null","Severity" : 0,"Message" : message};
				this.AllMessages.$values.push(message);
				this.HasWarnings = true;
			},
			"AddInfo": function(message){
				var message = {
					"$type" : "SAHL.Core.SystemMessages.SystemMessage, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
					"Severity" : 2,
					"Message" : message
				};
				this.AllMessages.$values.push(message);
			}
		}
	}
	this.GetAffordabilityInterestRateQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetAffordabilityInterestRateQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetAffordabilityInterestRateQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(amountQualifiedFor,instalment,interestRate,propertyPriceQualifiedFor,termInMonths,paymentToIncome){
				this.ReturnData.Results.$values.push(
					{
						"AmountQualifiedFor": amountQualifiedFor,
					"Instalment": instalment,
					"InterestRate": interestRate,
					"PropertyPriceQualifiedFor": propertyPriceQualifiedFor,
					"TermInMonths": termInMonths,
					"PaymentToIncome": paymentToIncome
					}
				)
			}
					}
	}
	this.GetApplicationPurposeQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetApplicationPurposeQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetApplicationPurposeQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,name){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"Name": name
					}
				)
			}
					}
	}
	this.GetApplicationResultQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetApplicationResultQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetApplicationResultQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(submitted,applicationNumber,applicationCalculationMessages,firstApplicantName,firstApplicantITCPassed,firstApplicantITCMessages,secondApplicantName,secondApplicantITCPassed,secondApplicantITCMessages){
				this.ReturnData.Results.$values.push(
					{
						"Submitted": submitted,
					"ApplicationNumber": applicationNumber,
					"ApplicationCalculationMessages": applicationCalculationMessages,
					"FirstApplicantName": firstApplicantName,
					"FirstApplicantITCPassed": firstApplicantITCPassed,
					"FirstApplicantITCMessages": firstApplicantITCMessages,
					"SecondApplicantName": secondApplicantName,
					"SecondApplicantITCPassed": secondApplicantITCPassed,
					"SecondApplicantITCMessages": secondApplicantITCMessages
					}
				)
			}
					}
	}
	this.GetApplicationByIdentityNumberQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetApplicationByIdentityNumberQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetApplicationByIdentityNumberQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(applicationNumber){
				this.ReturnData.Results.$values.push(
					{
						"ApplicationNumber": applicationNumber
					}
				)
			}
					}
	}
	this.GetNewGuidQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetNewGuidQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetNewGuidQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(newGuid){
				this.ReturnData.Results.$values.push(
					{
						"NewGuid": newGuid
					}
				)
			}
					}
	}
	this.GetOccupancyTypesResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetOccupancyTypesResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetOccupancyTypesResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,name){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"Name": name
					}
				)
			}
					}
	}
	this.GetDeclarationTypesResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetDeclarationTypesResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetDeclarationTypesResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,name){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"Name": name
					}
				)
			}
					}
	}
	this.GetEmploymentTypesResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetEmploymentTypesResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetEmploymentTypesResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,name){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"Name": name
					}
				)
			}
					}
	}
	this.GetRolesFromUserQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetRolesFromUserQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetRolesFromUserQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,name){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"Name": name
					}
				)
			}
					}
	}
	this.GetSalutationsQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetSalutationsQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetSalutationsQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,name){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"Name": name
					}
				)
			}
					}
	}
	this.GetApplicationQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetApplicationQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetApplicationQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,applicationDate,userId,applicationNumber){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"ApplicationDate": applicationDate,
					"UserId": userId,
					"ApplicationNumber": applicationNumber
					}
				)
			}
					}
	}
	this.CalculateApplicationScenarioQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.CalculateApplicationScenarioQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.CalculateApplicationScenarioQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(interestRate,lTV,pTI,instalment,loanAmount,termInMonths,lTVAsPercent,pTIAsPercent,interestRateAsPercent,eligibleApplication,decisionTreeMessages){
				this.ReturnData.Results.$values.push(
					{
						"InterestRate": interestRate,
					"LTV": lTV,
					"PTI": pTI,
					"Instalment": instalment,
					"LoanAmount": loanAmount,
					"TermInMonths": termInMonths,
					"LTVAsPercent": lTVAsPercent,
					"PTIAsPercent": pTIAsPercent,
					"InterestRateAsPercent": interestRateAsPercent,
					"EligibleApplication": eligibleApplication,
					"DecisionTreeMessages": decisionTreeMessages
					}
				)
			}
					}
	}
	this.FilterBranchesByNameQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.FilterBranchesByNameQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.FilterBranchesByNameQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,branchName){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"BranchName": branchName
					}
				)
			}
					}
	}
	this.FilterCitiesByNameQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.FilterCitiesByNameQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.FilterCitiesByNameQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,cityName){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"CityName": cityName
					}
				)
			}
					}
	}
	this.FilterSuburbsByNameQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.FilterSuburbsByNameQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.FilterSuburbsByNameQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,suburbName){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"SuburbName": suburbName
					}
				)
			}
					}
	}
	this.GetCalculatorFeeQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetCalculatorFeeQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetCalculatorFeeQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(cancellationFee,initiationFee,registrationFee,interimInterest,bondToRegister){
				this.ReturnData.Results.$values.push(
					{
						"CancellationFee": cancellationFee,
					"InitiationFee": initiationFee,
					"RegistrationFee": registrationFee,
					"InterimInterest": interimInterest,
					"BondToRegister": bondToRegister
					}
				)
			}
					}
	}
	this.GetCitiesQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetCitiesQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetCitiesQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,cityName,numberOfBranches){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"CityName": cityName,
					"NumberOfBranches": numberOfBranches
					}
				)
			}
					}
	}
	this.GetCityQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetCityQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetCityQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,cityName,sAHLCityKey,provinceId){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"CityName": cityName,
					"SAHLCityKey": sAHLCityKey,
					"ProvinceId": provinceId
					}
				)
			}
					}
	}
	this.GetCountriesQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetCountriesQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetCountriesQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,countryName){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"CountryName": countryName
					}
				)
			}
					}
	}
	this.GetProvinceQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetProvinceQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetProvinceQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,sAHLProvinceKey,provinceName,countryId){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"SAHLProvinceKey": sAHLProvinceKey,
					"ProvinceName": provinceName,
					"CountryId": countryId
					}
				)
			}
					}
	}
	this.GetBranchQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetBranchQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetBranchQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,branchName,branchCode,isActive,suburbId,suburbName){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"BranchName": branchName,
					"BranchCode": branchCode,
					"IsActive": isActive,
					"SuburbId": suburbId,
					"SuburbName": suburbName
					}
				)
			}
					}
	}
	this.GetProvincesQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetProvincesQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetProvincesQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,provinceName,numberOfBranches,countryName){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"ProvinceName": provinceName,
					"NumberOfBranches": numberOfBranches,
					"CountryName": countryName
					}
				)
			}
					}
	}
	this.GetBranchesQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetBranchesQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetBranchesQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,branchName,suburbName,provinceName,isActive,numberOfUsers){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"BranchName": branchName,
					"SuburbName": suburbName,
					"ProvinceName": provinceName,
					"IsActive": isActive,
					"NumberOfUsers": numberOfUsers
					}
				)
			}
					}
	}
	this.GetControlValueQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetControlValueQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetControlValueQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(controlNumeric,controlText){
				this.ReturnData.Results.$values.push(
					{
						"ControlNumeric": controlNumeric,
					"ControlText": controlText
					}
				)
			}
					}
	}
	this.GetRoleTypesQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetRoleTypesQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetRoleTypesQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,name){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"Name": name
					}
				)
			}
					}
	}
	this.GetSuburbProvinceQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetSuburbProvinceQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetSuburbProvinceQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,sAHLSuburbKey,suburbName,postalCode,cityId,cityName,provinceName){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"SAHLSuburbKey": sAHLSuburbKey,
					"SuburbName": suburbName,
					"PostalCode": postalCode,
					"CityId": cityId,
					"CityName": cityName,
					"ProvinceName": provinceName
					}
				)
			}
					}
	}
	this.GetSuburbQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetSuburbQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetSuburbQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,sAHLSuburbKey,suburbName,postalCode,cityId,cityName){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"SAHLSuburbKey": sAHLSuburbKey,
					"SuburbName": suburbName,
					"PostalCode": postalCode,
					"CityId": cityId,
					"CityName": cityName
					}
				)
			}
					}
	}
	this.GetSuburbsQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetSuburbsQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetSuburbsQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,suburbName,provinceName,numberOfBranches){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"SuburbName": suburbName,
					"ProvinceName": provinceName,
					"NumberOfBranches": numberOfBranches
					}
				)
			}
					}
	}
	this.GetUserFromAuthTokenQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetUserFromAuthTokenQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetUserFromAuthTokenQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,username,firstName,lastName,roles){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"Username": username,
					"FirstName": firstName,
					"LastName": lastName,
					"Roles": roles
					}
				)
			}
					}
	}
	this.GetUserQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetUserQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetUserQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,username,isActive,emailAddress,firstName,lastName,branchName,branchId,roles){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"Username": username,
					"IsActive": isActive,
					"EmailAddress": emailAddress,
					"FirstName": firstName,
					"LastName": lastName,
					"BranchName": branchName,
					"BranchId": branchId,
					"Roles": roles
					}
				)
			}
					}
	}
	this.GetUsersQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Capitec.Models.GetUsersQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Capitec.Models.GetUsersQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,name,emailAddress,isActive,branchName,lastActivity){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"Name": name,
					"EmailAddress": emailAddress,
					"IsActive": isActive,
					"BranchName": branchName,
					"LastActivity": lastActivity
					}
				)
			}
					}
	}
	return {
		SystemMessages : SystemMessages,
		GetAffordabilityInterestRateQueryResult: GetAffordabilityInterestRateQueryResult,
		GetApplicationPurposeQueryResult: GetApplicationPurposeQueryResult,
		GetApplicationResultQueryResult: GetApplicationResultQueryResult,
		GetApplicationByIdentityNumberQueryResult: GetApplicationByIdentityNumberQueryResult,
		GetNewGuidQueryResult: GetNewGuidQueryResult,
		GetOccupancyTypesResult: GetOccupancyTypesResult,
		GetDeclarationTypesResult: GetDeclarationTypesResult,
		GetEmploymentTypesResult: GetEmploymentTypesResult,
		GetRolesFromUserQueryResult: GetRolesFromUserQueryResult,
		GetSalutationsQueryResult: GetSalutationsQueryResult,
		GetApplicationQueryResult: GetApplicationQueryResult,
		CalculateApplicationScenarioQueryResult: CalculateApplicationScenarioQueryResult,
		FilterBranchesByNameQueryResult: FilterBranchesByNameQueryResult,
		FilterCitiesByNameQueryResult: FilterCitiesByNameQueryResult,
		FilterSuburbsByNameQueryResult: FilterSuburbsByNameQueryResult,
		GetCalculatorFeeQueryResult: GetCalculatorFeeQueryResult,
		GetCitiesQueryResult: GetCitiesQueryResult,
		GetCityQueryResult: GetCityQueryResult,
		GetCountriesQueryResult: GetCountriesQueryResult,
		GetProvinceQueryResult: GetProvinceQueryResult,
		GetBranchQueryResult: GetBranchQueryResult,
		GetProvincesQueryResult: GetProvincesQueryResult,
		GetBranchesQueryResult: GetBranchesQueryResult,
		GetControlValueQueryResult: GetControlValueQueryResult,
		GetRoleTypesQueryResult: GetRoleTypesQueryResult,
		GetSuburbProvinceQueryResult: GetSuburbProvinceQueryResult,
		GetSuburbQueryResult: GetSuburbQueryResult,
		GetSuburbsQueryResult: GetSuburbsQueryResult,
		GetUserFromAuthTokenQueryResult: GetUserFromAuthTokenQueryResult,
		GetUserQueryResult: GetUserQueryResult,
		GetUsersQueryResult: GetUsersQueryResult
	};
}();