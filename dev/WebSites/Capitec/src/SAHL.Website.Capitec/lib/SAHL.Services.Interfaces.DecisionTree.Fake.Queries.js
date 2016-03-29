var decisionTreequeryFakes = function(){
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
	this.CapitecAffordabilityInterestRate_1QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecAffordabilityInterestRate_1QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecAffordabilityInterestRate_1QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,interestRate,amountQualifiedFor,propertyPriceQualifiedFor,paymentToIncome,termInMonths,instalment){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"InterestRate": interestRate,
					"AmountQualifiedFor": amountQualifiedFor,
					"PropertyPriceQualifiedFor": propertyPriceQualifiedFor,
					"PaymentToIncome": paymentToIncome,
					"TermInMonths": termInMonths,
					"Instalment": instalment
					}
				)
			}
					}
	}
	this.CapitecAffordabilityInterestRate_QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecAffordabilityInterestRate_QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecAffordabilityInterestRate_QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,interestRate,amountQualifiedFor,propertyPriceQualifiedFor,paymentToIncome,termInMonths,instalment){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"InterestRate": interestRate,
					"AmountQualifiedFor": amountQualifiedFor,
					"PropertyPriceQualifiedFor": propertyPriceQualifiedFor,
					"PaymentToIncome": paymentToIncome,
					"TermInMonths": termInMonths,
					"Instalment": instalment
					}
				)
			}
					}
	}
	this.CapitecAlphaCreditPricing_1QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecAlphaCreditPricing_1QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecAlphaCreditPricing_1QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,interestRate,loantoValue,loanAmount,paymenttoIncome,creditMatrixCategory,alpha,propertyValue,instalment,eligibleApplication,interestRateasPercent,loantoValueasPercent,paymenttoIncomeasPercent,installmentinRands,linkRate){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"InterestRate": interestRate,
					"LoantoValue": loantoValue,
					"LoanAmount": loanAmount,
					"PaymenttoIncome": paymenttoIncome,
					"CreditMatrixCategory": creditMatrixCategory,
					"Alpha": alpha,
					"PropertyValue": propertyValue,
					"Instalment": instalment,
					"EligibleApplication": eligibleApplication,
					"InterestRateasPercent": interestRateasPercent,
					"LoantoValueasPercent": loantoValueasPercent,
					"PaymenttoIncomeasPercent": paymenttoIncomeasPercent,
					"InstallmentinRands": installmentinRands,
					"LinkRate": linkRate
					}
				)
			}
					}
	}
	this.CapitecAlphaCreditPricing_QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecAlphaCreditPricing_QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecAlphaCreditPricing_QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,interestRate,loantoValue,loanAmount,paymenttoIncome,creditMatrixCategory,alpha,propertyValue,instalment,eligibleApplication,interestRateasPercent,loantoValueasPercent,paymenttoIncomeasPercent,installmentinRands,linkRate){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"InterestRate": interestRate,
					"LoantoValue": loantoValue,
					"LoanAmount": loanAmount,
					"PaymenttoIncome": paymenttoIncome,
					"CreditMatrixCategory": creditMatrixCategory,
					"Alpha": alpha,
					"PropertyValue": propertyValue,
					"Instalment": instalment,
					"EligibleApplication": eligibleApplication,
					"InterestRateasPercent": interestRateasPercent,
					"LoantoValueasPercent": loantoValueasPercent,
					"PaymenttoIncomeasPercent": paymenttoIncomeasPercent,
					"InstallmentinRands": installmentinRands,
					"LinkRate": linkRate
					}
				)
			}
					}
	}
	this.CapitecApplicationCreditPolicy_1QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecApplicationCreditPolicy_1QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecApplicationCreditPolicy_1QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,applicationEmpirica,eligibleBorrowerAge,eligibleEmpirica){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"ApplicationEmpirica": applicationEmpirica,
					"EligibleBorrowerAge": eligibleBorrowerAge,
					"EligibleEmpirica": eligibleEmpirica
					}
				)
			}
					}
	}
	this.CapitecApplicationCreditPolicy_2QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecApplicationCreditPolicy_2QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecApplicationCreditPolicy_2QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,applicationEmpirica,eligibleBorrowerAge,eligibleEmpirica){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"ApplicationEmpirica": applicationEmpirica,
					"EligibleBorrowerAge": eligibleBorrowerAge,
					"EligibleEmpirica": eligibleEmpirica
					}
				)
			}
					}
	}
	this.CapitecApplicationCreditPolicy_QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecApplicationCreditPolicy_QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecApplicationCreditPolicy_QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,applicationEmpirica,eligibleBorrowerAge,eligibleEmpirica){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"ApplicationEmpirica": applicationEmpirica,
					"EligibleBorrowerAge": eligibleBorrowerAge,
					"EligibleEmpirica": eligibleEmpirica
					}
				)
			}
					}
	}
	this.CapitecClientCreditBureauAssessment_1QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecClientCreditBureauAssessment_1QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecClientCreditBureauAssessment_1QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,eligibleBorrower){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"EligibleBorrower": eligibleBorrower
					}
				)
			}
					}
	}
	this.CapitecClientCreditBureauAssessment_2QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecClientCreditBureauAssessment_2QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecClientCreditBureauAssessment_2QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,eligibleBorrower){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"EligibleBorrower": eligibleBorrower
					}
				)
			}
					}
	}
	this.CapitecClientCreditBureauAssessment_QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecClientCreditBureauAssessment_QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecClientCreditBureauAssessment_QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,eligibleBorrower){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"EligibleBorrower": eligibleBorrower
					}
				)
			}
					}
	}
	this.CapitecCreditPricing_1QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecCreditPricing_1QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecCreditPricing_1QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,interestRate,loantoValue,loanAmount,paymenttoIncome,creditMatrixCategory,alpha,propertyValue,instalment,applicationEmpirica,eligibleApplication,eligibleBorrowerAge,eligibleEmpirica,eligibleBorrower,interestRateasPercent,loantoValueasPercent,paymenttoIncomeasPercent,installmentinRands){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"InterestRate": interestRate,
					"LoantoValue": loantoValue,
					"LoanAmount": loanAmount,
					"PaymenttoIncome": paymenttoIncome,
					"CreditMatrixCategory": creditMatrixCategory,
					"Alpha": alpha,
					"PropertyValue": propertyValue,
					"Instalment": instalment,
					"ApplicationEmpirica": applicationEmpirica,
					"EligibleApplication": eligibleApplication,
					"EligibleBorrowerAge": eligibleBorrowerAge,
					"EligibleEmpirica": eligibleEmpirica,
					"EligibleBorrower": eligibleBorrower,
					"InterestRateasPercent": interestRateasPercent,
					"LoantoValueasPercent": loantoValueasPercent,
					"PaymenttoIncomeasPercent": paymenttoIncomeasPercent,
					"InstallmentinRands": installmentinRands
					}
				)
			}
					}
	}
	this.CapitecCreditPricing_QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecCreditPricing_QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecCreditPricing_QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,interestRate,loantoValue,loanAmount,paymenttoIncome,creditMatrixCategory,alpha,propertyValue,instalment,applicationEmpirica,eligibleApplication,eligibleBorrowerAge,eligibleEmpirica,eligibleBorrower,interestRateasPercent,loantoValueasPercent,paymenttoIncomeasPercent,installmentinRands){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"InterestRate": interestRate,
					"LoantoValue": loantoValue,
					"LoanAmount": loanAmount,
					"PaymenttoIncome": paymenttoIncome,
					"CreditMatrixCategory": creditMatrixCategory,
					"Alpha": alpha,
					"PropertyValue": propertyValue,
					"Instalment": instalment,
					"ApplicationEmpirica": applicationEmpirica,
					"EligibleApplication": eligibleApplication,
					"EligibleBorrowerAge": eligibleBorrowerAge,
					"EligibleEmpirica": eligibleEmpirica,
					"EligibleBorrower": eligibleBorrower,
					"InterestRateasPercent": interestRateasPercent,
					"LoantoValueasPercent": loantoValueasPercent,
					"PaymenttoIncomeasPercent": paymenttoIncomeasPercent,
					"InstallmentinRands": installmentinRands
					}
				)
			}
					}
	}
	this.CapitecOriginationCreditPricing_1QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecOriginationCreditPricing_1QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecOriginationCreditPricing_1QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,interestRate,loantoValue,loanAmount,paymenttoIncome,creditMatrixCategory,alpha,propertyValue,instalment,applicationEmpirica,eligibleApplication,eligibleBorrowerAge,eligibleEmpirica,interestRateasPercent,loantoValueasPercent,paymenttoIncomeasPercent,installmentinRands,linkRate,linkRateAdjustment){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"InterestRate": interestRate,
					"LoantoValue": loantoValue,
					"LoanAmount": loanAmount,
					"PaymenttoIncome": paymenttoIncome,
					"CreditMatrixCategory": creditMatrixCategory,
					"Alpha": alpha,
					"PropertyValue": propertyValue,
					"Instalment": instalment,
					"ApplicationEmpirica": applicationEmpirica,
					"EligibleApplication": eligibleApplication,
					"EligibleBorrowerAge": eligibleBorrowerAge,
					"EligibleEmpirica": eligibleEmpirica,
					"InterestRateasPercent": interestRateasPercent,
					"LoantoValueasPercent": loantoValueasPercent,
					"PaymenttoIncomeasPercent": paymenttoIncomeasPercent,
					"InstallmentinRands": installmentinRands,
					"LinkRate": linkRate,
					"LinkRateAdjustment": linkRateAdjustment
					}
				)
			}
					}
	}
	this.CapitecOriginationCreditPricing_QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecOriginationCreditPricing_QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecOriginationCreditPricing_QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,interestRate,loantoValue,loanAmount,paymenttoIncome,creditMatrixCategory,alpha,propertyValue,instalment,applicationEmpirica,eligibleApplication,eligibleBorrowerAge,eligibleEmpirica,interestRateasPercent,loantoValueasPercent,paymenttoIncomeasPercent,installmentinRands,linkRate,linkRateAdjustment){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"InterestRate": interestRate,
					"LoantoValue": loantoValue,
					"LoanAmount": loanAmount,
					"PaymenttoIncome": paymenttoIncome,
					"CreditMatrixCategory": creditMatrixCategory,
					"Alpha": alpha,
					"PropertyValue": propertyValue,
					"Instalment": instalment,
					"ApplicationEmpirica": applicationEmpirica,
					"EligibleApplication": eligibleApplication,
					"EligibleBorrowerAge": eligibleBorrowerAge,
					"EligibleEmpirica": eligibleEmpirica,
					"InterestRateasPercent": interestRateasPercent,
					"LoantoValueasPercent": loantoValueasPercent,
					"PaymenttoIncomeasPercent": paymenttoIncomeasPercent,
					"InstallmentinRands": installmentinRands,
					"LinkRate": linkRate,
					"LinkRateAdjustment": linkRateAdjustment
					}
				)
			}
					}
	}
	this.CapitecOriginationPricingforRisk_1QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecOriginationPricingforRisk_1QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecOriginationPricingforRisk_1QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,linkRateAdjustment){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"LinkRateAdjustment": linkRateAdjustment
					}
				)
			}
					}
	}
	this.CapitecOriginationPricingforRisk_2QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecOriginationPricingforRisk_2QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecOriginationPricingforRisk_2QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,linkRateAdjustment){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"LinkRateAdjustment": linkRateAdjustment
					}
				)
			}
					}
	}
	this.CapitecOriginationPricingforRisk_QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecOriginationPricingforRisk_QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecOriginationPricingforRisk_QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,linkRateAdjustment){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"LinkRateAdjustment": linkRateAdjustment
					}
				)
			}
					}
	}
	this.CapitecSalariedCreditPricing_1QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecSalariedCreditPricing_1QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecSalariedCreditPricing_1QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,interestRate,loantoValue,loanAmount,paymenttoIncome,creditMatrixCategory,alpha,propertyValue,instalment,eligibleApplication,interestRateasPercent,loantoValueasPercent,paymenttoIncomeasPercent,installmentinRands,linkRate){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"InterestRate": interestRate,
					"LoantoValue": loantoValue,
					"LoanAmount": loanAmount,
					"PaymenttoIncome": paymenttoIncome,
					"CreditMatrixCategory": creditMatrixCategory,
					"Alpha": alpha,
					"PropertyValue": propertyValue,
					"Instalment": instalment,
					"EligibleApplication": eligibleApplication,
					"InterestRateasPercent": interestRateasPercent,
					"LoantoValueasPercent": loantoValueasPercent,
					"PaymenttoIncomeasPercent": paymenttoIncomeasPercent,
					"InstallmentinRands": installmentinRands,
					"LinkRate": linkRate
					}
				)
			}
					}
	}
	this.CapitecSalariedCreditPricing_QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecSalariedCreditPricing_QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecSalariedCreditPricing_QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,interestRate,loantoValue,loanAmount,paymenttoIncome,creditMatrixCategory,alpha,propertyValue,instalment,eligibleApplication,interestRateasPercent,loantoValueasPercent,paymenttoIncomeasPercent,installmentinRands,linkRate){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"InterestRate": interestRate,
					"LoantoValue": loantoValue,
					"LoanAmount": loanAmount,
					"PaymenttoIncome": paymenttoIncome,
					"CreditMatrixCategory": creditMatrixCategory,
					"Alpha": alpha,
					"PropertyValue": propertyValue,
					"Instalment": instalment,
					"EligibleApplication": eligibleApplication,
					"InterestRateasPercent": interestRateasPercent,
					"LoantoValueasPercent": loantoValueasPercent,
					"PaymenttoIncomeasPercent": paymenttoIncomeasPercent,
					"InstallmentinRands": installmentinRands,
					"LinkRate": linkRate
					}
				)
			}
					}
	}
	this.CapitecSalariedwithDeductionCreditPricing_1QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecSalariedwithDeductionCreditPricing_1QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecSalariedwithDeductionCreditPricing_1QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,interestRate,loantoValue,loanAmount,paymenttoIncome,creditMatrixCategory,alpha,propertyValue,instalment,eligibleApplication,interestRateasPercent,loantoValueasPercent,paymenttoIncomeasPercent,installmentinRands,linkRate){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"InterestRate": interestRate,
					"LoantoValue": loantoValue,
					"LoanAmount": loanAmount,
					"PaymenttoIncome": paymenttoIncome,
					"CreditMatrixCategory": creditMatrixCategory,
					"Alpha": alpha,
					"PropertyValue": propertyValue,
					"Instalment": instalment,
					"EligibleApplication": eligibleApplication,
					"InterestRateasPercent": interestRateasPercent,
					"LoantoValueasPercent": loantoValueasPercent,
					"PaymenttoIncomeasPercent": paymenttoIncomeasPercent,
					"InstallmentinRands": installmentinRands,
					"LinkRate": linkRate
					}
				)
			}
					}
	}
	this.CapitecSalariedwithDeductionCreditPricing_QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecSalariedwithDeductionCreditPricing_QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecSalariedwithDeductionCreditPricing_QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,interestRate,loantoValue,loanAmount,paymenttoIncome,creditMatrixCategory,alpha,propertyValue,instalment,eligibleApplication,interestRateasPercent,loantoValueasPercent,paymenttoIncomeasPercent,installmentinRands,linkRate){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"InterestRate": interestRate,
					"LoantoValue": loantoValue,
					"LoanAmount": loanAmount,
					"PaymenttoIncome": paymenttoIncome,
					"CreditMatrixCategory": creditMatrixCategory,
					"Alpha": alpha,
					"PropertyValue": propertyValue,
					"Instalment": instalment,
					"EligibleApplication": eligibleApplication,
					"InterestRateasPercent": interestRateasPercent,
					"LoantoValueasPercent": loantoValueasPercent,
					"PaymenttoIncomeasPercent": paymenttoIncomeasPercent,
					"InstallmentinRands": installmentinRands,
					"LinkRate": linkRate
					}
				)
			}
					}
	}
	this.CapitecSelfEmployedCreditPricing_1QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecSelfEmployedCreditPricing_1QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecSelfEmployedCreditPricing_1QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,interestRate,loantoValue,loanAmount,paymenttoIncome,creditMatrixCategory,alpha,propertyValue,instalment,eligibleApplication,interestRateasPercent,loantoValueasPercent,paymenttoIncomeasPercent,installmentinRands,linkRate){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"InterestRate": interestRate,
					"LoantoValue": loantoValue,
					"LoanAmount": loanAmount,
					"PaymenttoIncome": paymenttoIncome,
					"CreditMatrixCategory": creditMatrixCategory,
					"Alpha": alpha,
					"PropertyValue": propertyValue,
					"Instalment": instalment,
					"EligibleApplication": eligibleApplication,
					"InterestRateasPercent": interestRateasPercent,
					"LoantoValueasPercent": loantoValueasPercent,
					"PaymenttoIncomeasPercent": paymenttoIncomeasPercent,
					"InstallmentinRands": installmentinRands,
					"LinkRate": linkRate
					}
				)
			}
					}
	}
	this.CapitecSelfEmployedCreditPricing_QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecSelfEmployedCreditPricing_QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.CapitecSelfEmployedCreditPricing_QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,interestRate,loantoValue,loanAmount,paymenttoIncome,creditMatrixCategory,alpha,propertyValue,instalment,eligibleApplication,interestRateasPercent,loantoValueasPercent,paymenttoIncomeasPercent,installmentinRands,linkRate){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"InterestRate": interestRate,
					"LoantoValue": loantoValue,
					"LoanAmount": loanAmount,
					"PaymenttoIncome": paymenttoIncome,
					"CreditMatrixCategory": creditMatrixCategory,
					"Alpha": alpha,
					"PropertyValue": propertyValue,
					"Instalment": instalment,
					"EligibleApplication": eligibleApplication,
					"InterestRateasPercent": interestRateasPercent,
					"LoantoValueasPercent": loantoValueasPercent,
					"PaymenttoIncomeasPercent": paymenttoIncomeasPercent,
					"InstallmentinRands": installmentinRands,
					"LinkRate": linkRate
					}
				)
			}
					}
	}
	this.EdTest2_1QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.EdTest2_1QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.EdTest2_1QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,scaledBCCScore,tobescored){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"ScaledBCCScore": scaledBCCScore,
					"Tobescored": tobescored
					}
				)
			}
					}
	}
	this.EdTest2_2QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.EdTest2_2QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.EdTest2_2QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,scaledBCCScore,tobescored){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"ScaledBCCScore": scaledBCCScore,
					"Tobescored": tobescored
					}
				)
			}
					}
	}
	this.EdTest2_QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.EdTest2_QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.EdTest2_QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,scaledBCCScore,tobescored){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"ScaledBCCScore": scaledBCCScore,
					"Tobescored": tobescored
					}
				)
			}
					}
	}
	this.EdTest_1QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.EdTest_1QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.EdTest_1QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,otherTruth,output1,newvar2,Z){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"OtherTruth": otherTruth,
					"Output1": output1,
					"Newvar2": newvar2,
					"Z": Z
					}
				)
			}
					}
	}
	this.EdTest_QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.EdTest_QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.EdTest_QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,otherTruth,output1,newvar2,Z){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"OtherTruth": otherTruth,
					"Output1": output1,
					"Newvar2": newvar2,
					"Z": Z
					}
				)
			}
					}
	}
	this.Jarrodtesttree_1QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.Jarrodtesttree_1QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.Jarrodtesttree_1QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,Z,X){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"Z": Z,
					"X": X
					}
				)
			}
					}
	}
	this.Jarrodtesttree_QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.Jarrodtesttree_QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.Jarrodtesttree_QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,Z,X){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"Z": Z,
					"X": X
					}
				)
			}
					}
	}
	this.NewDecisionTree_1QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.NewDecisionTree_1QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.NewDecisionTree_1QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,outString,outInt,parentInt){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"OutString": outString,
					"OutInt": outInt,
					"ParentInt": parentInt
					}
				)
			}
					}
	}
	this.NewDecisionTree_QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.NewDecisionTree_QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.NewDecisionTree_QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,outString,outInt,parentInt){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"OutString": outString,
					"OutInt": outInt,
					"ParentInt": parentInt
					}
				)
			}
					}
	}
	this.OriginationNewBusinessSPVDetermination_1QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.OriginationNewBusinessSPVDetermination_1QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.OriginationNewBusinessSPVDetermination_1QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,blueBannerAlpha,oldMutualAlpha,mainStreet65,oldMutualDeveloper,calibre,blueBanner){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"BlueBannerAlpha": blueBannerAlpha,
					"OldMutualAlpha": oldMutualAlpha,
					"MainStreet65": mainStreet65,
					"OldMutualDeveloper": oldMutualDeveloper,
					"Calibre": calibre,
					"BlueBanner": blueBanner
					}
				)
			}
					}
	}
	this.OriginationNewBusinessSPVDetermination_QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.OriginationNewBusinessSPVDetermination_QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.OriginationNewBusinessSPVDetermination_QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,blueBannerAlpha,oldMutualAlpha,mainStreet65,oldMutualDeveloper,calibre,blueBanner){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"BlueBannerAlpha": blueBannerAlpha,
					"OldMutualAlpha": oldMutualAlpha,
					"MainStreet65": mainStreet65,
					"OldMutualDeveloper": oldMutualDeveloper,
					"Calibre": calibre,
					"BlueBanner": blueBanner
					}
				)
			}
					}
	}
	this.SAHomeLoansBCCScorecard_1QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.SAHomeLoansBCCScorecard_1QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.SAHomeLoansBCCScorecard_1QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,scaledBCCScore,toBeScored){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"ScaledBCCScore": scaledBCCScore,
					"ToBeScored": toBeScored
					}
				)
			}
					}
	}
	this.SAHomeLoansBCCScorecard_QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.SAHomeLoansBCCScorecard_QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.SAHomeLoansBCCScorecard_QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,scaledBCCScore,toBeScored){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"ScaledBCCScore": scaledBCCScore,
					"ToBeScored": toBeScored
					}
				)
			}
					}
	}
	this.SubtreeTest_1QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.SubtreeTest_1QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.SubtreeTest_1QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,rateAdjustment,newva123,newvar3,passed){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"RateAdjustment": rateAdjustment,
					"Newva123": newva123,
					"Newvar3": newvar3,
					"Passed": passed
					}
				)
			}
					}
	}
	this.SubtreeTest_QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.SubtreeTest_QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.SubtreeTest_QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,rateAdjustment,newva123,newvar3,passed){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"RateAdjustment": rateAdjustment,
					"Newva123": newva123,
					"Newvar3": newvar3,
					"Passed": passed
					}
				)
			}
					}
	}
	this.TestProcess_1QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.TestProcess_1QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.TestProcess_1QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,newvar5){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"Newvar5": newvar5
					}
				)
			}
					}
	}
	this.TestProcess_QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.TestProcess_QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.TestProcess_QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,newvar5){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"Newvar5": newvar5
					}
				)
			}
					}
	}
	this.TestSubtree_1QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.TestSubtree_1QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.TestSubtree_1QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,newvar){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"Newvar": newvar
					}
				)
			}
					}
	}
	this.TestSubtree_QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.TestSubtree_QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.TestSubtree_QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,newvar){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"Newvar": newvar
					}
				)
			}
					}
	}
	this.Test_1QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.Test_1QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.Test_1QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,newvar){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"Newvar": newvar
					}
				)
			}
					}
	}
	this.Test_QueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTree.Models.Test_QueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTree.Models.Test_QueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(globalsVersionResultIsBasedOn,newvar){
				this.ReturnData.Results.$values.push(
					{
						"GlobalsVersionResultIsBasedOn": globalsVersionResultIsBasedOn,
					"Newvar": newvar
					}
				)
			}
					}
	}
	return {
		SystemMessages : SystemMessages,
		CapitecAffordabilityInterestRate_1QueryResult: CapitecAffordabilityInterestRate_1QueryResult,
		CapitecAffordabilityInterestRate_QueryResult: CapitecAffordabilityInterestRate_QueryResult,
		CapitecAlphaCreditPricing_1QueryResult: CapitecAlphaCreditPricing_1QueryResult,
		CapitecAlphaCreditPricing_QueryResult: CapitecAlphaCreditPricing_QueryResult,
		CapitecApplicationCreditPolicy_1QueryResult: CapitecApplicationCreditPolicy_1QueryResult,
		CapitecApplicationCreditPolicy_2QueryResult: CapitecApplicationCreditPolicy_2QueryResult,
		CapitecApplicationCreditPolicy_QueryResult: CapitecApplicationCreditPolicy_QueryResult,
		CapitecClientCreditBureauAssessment_1QueryResult: CapitecClientCreditBureauAssessment_1QueryResult,
		CapitecClientCreditBureauAssessment_2QueryResult: CapitecClientCreditBureauAssessment_2QueryResult,
		CapitecClientCreditBureauAssessment_QueryResult: CapitecClientCreditBureauAssessment_QueryResult,
		CapitecCreditPricing_1QueryResult: CapitecCreditPricing_1QueryResult,
		CapitecCreditPricing_QueryResult: CapitecCreditPricing_QueryResult,
		CapitecOriginationCreditPricing_1QueryResult: CapitecOriginationCreditPricing_1QueryResult,
		CapitecOriginationCreditPricing_QueryResult: CapitecOriginationCreditPricing_QueryResult,
		CapitecOriginationPricingforRisk_1QueryResult: CapitecOriginationPricingforRisk_1QueryResult,
		CapitecOriginationPricingforRisk_2QueryResult: CapitecOriginationPricingforRisk_2QueryResult,
		CapitecOriginationPricingforRisk_QueryResult: CapitecOriginationPricingforRisk_QueryResult,
		CapitecSalariedCreditPricing_1QueryResult: CapitecSalariedCreditPricing_1QueryResult,
		CapitecSalariedCreditPricing_QueryResult: CapitecSalariedCreditPricing_QueryResult,
		CapitecSalariedwithDeductionCreditPricing_1QueryResult: CapitecSalariedwithDeductionCreditPricing_1QueryResult,
		CapitecSalariedwithDeductionCreditPricing_QueryResult: CapitecSalariedwithDeductionCreditPricing_QueryResult,
		CapitecSelfEmployedCreditPricing_1QueryResult: CapitecSelfEmployedCreditPricing_1QueryResult,
		CapitecSelfEmployedCreditPricing_QueryResult: CapitecSelfEmployedCreditPricing_QueryResult,
		EdTest2_1QueryResult: EdTest2_1QueryResult,
		EdTest2_2QueryResult: EdTest2_2QueryResult,
		EdTest2_QueryResult: EdTest2_QueryResult,
		EdTest_1QueryResult: EdTest_1QueryResult,
		EdTest_QueryResult: EdTest_QueryResult,
		Jarrodtesttree_1QueryResult: Jarrodtesttree_1QueryResult,
		Jarrodtesttree_QueryResult: Jarrodtesttree_QueryResult,
		NewDecisionTree_1QueryResult: NewDecisionTree_1QueryResult,
		NewDecisionTree_QueryResult: NewDecisionTree_QueryResult,
		OriginationNewBusinessSPVDetermination_1QueryResult: OriginationNewBusinessSPVDetermination_1QueryResult,
		OriginationNewBusinessSPVDetermination_QueryResult: OriginationNewBusinessSPVDetermination_QueryResult,
		SAHomeLoansBCCScorecard_1QueryResult: SAHomeLoansBCCScorecard_1QueryResult,
		SAHomeLoansBCCScorecard_QueryResult: SAHomeLoansBCCScorecard_QueryResult,
		SubtreeTest_1QueryResult: SubtreeTest_1QueryResult,
		SubtreeTest_QueryResult: SubtreeTest_QueryResult,
		TestProcess_1QueryResult: TestProcess_1QueryResult,
		TestProcess_QueryResult: TestProcess_QueryResult,
		TestSubtree_1QueryResult: TestSubtree_1QueryResult,
		TestSubtree_QueryResult: TestSubtree_QueryResult,
		Test_1QueryResult: Test_1QueryResult,
		Test_QueryResult: Test_QueryResult
	};
}();