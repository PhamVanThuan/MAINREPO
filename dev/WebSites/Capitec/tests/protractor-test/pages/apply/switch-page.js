var page = require('../../page');
var switchResults = require('./switch-calculation-results-page.js')
var switchFailResults = require('./switch-calculation-results-page-fail.js')

var switchPage = new page()
var capitec = require('../../capitec');

switchPage.url = "/switch/application-precheck/for-switch";
switchPage.headerText = "apply";
switchPage.titleText = "financial info for switching a home loan";

switchPage.selectedOccupancyType =element(by.id('occupancyTypeSpan'));
switchPage.selectedIncomeType = element(by.id('incomeTypeSpan'));
switchPage.occupancyType = element(by.model('occupancyType'));
switchPage.incomeType = element(by.model('incomeType'));
switchPage.occupancyTypes = switchPage.occupancyType.element.all(by.tagName('option'));
switchPage.incomeTypes = switchPage.incomeType.element.all(by.tagName('option'));
switchPage.backButton = element(by.id('submit2'));
switchPage.submitButton = element(by.id('submitButton'));
switchPage.switchForm = element(by.id('switchForm'));
switchPage.marketValue = element(by.id('estimatedMarketValueOfTheHome'));
switchPage.cashRequired = element(by.id('cashRequired'));
switchPage.currentBalance = element(by.id('currentBalance'));
switchPage.grossIncome = element(by.id('householdIncome'));

switchPage.submit = function(expectToFail) {
	capitec = require('../../capitec');
	capitec.notifications.closeAll();
	switchPage.submitButton.click();
	if(expectToFail)
	{
		return switchFailResults;
	}
	return switchResults;	
};

switchPage.back = function() {
	var precheckPage = require('./application-precheck-page')
	switchPage.backButton.click();
	return precheckPage;
};

switchPage.setOccupancyTypeTo = function(occupancyType){
	switchPage.selectOption(switchPage.occupancyType, occupancyType);
};

switchPage.setIncomeTypeTo = function(incomeType){
	switchPage.selectOption(switchPage.incomeType, incomeType);
};

switchPage.populateForm = function(estMarketVal, cashReq, currBalance, income) {
	switchPage.marketValue.clear()
	switchPage.marketValue.sendKeys(estMarketVal);
	switchPage.cashRequired.clear()
	switchPage.cashRequired.sendKeys(cashReq);
	switchPage.currentBalance.clear()
	switchPage.currentBalance.sendKeys(currBalance);
	switchPage.grossIncome.clear()
	switchPage.grossIncome.sendKeys(income);
	};

switchPage.fillForm = function(){
	switchPage.populateForm("500000", "100000", "100000", "25000");
	switchPage.setOccupancyTypeTo("Owner Occupied");
	switchPage.setIncomeTypeTo("Salaried");
	return switchPage.submit(false);
};	

switchPage.submitPopulatedForm = function(estMarketVal, cashReq, currBalance, income, occupancyType, incomeType, expectToFail) {
	switchPage.populateForm(estMarketVal, cashReq, currBalance, income);
	switchPage.setOccupancyTypeTo(occupancyType);
	switchPage.setIncomeTypeTo(incomeType);
	return switchPage.submit(expectToFail);
};

module.exports = switchPage;