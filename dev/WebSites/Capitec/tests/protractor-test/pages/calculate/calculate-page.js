var calculateResultsPage = require('./calculate-results-page.js');

var page = require('../../page.js');

var calculatePage = new page();
calculatePage.householdIncome = element(by.id("householdIncome"));
calculatePage.cashDeposit = element(by.id("cashDeposit"));
calculatePage.interestRate = element(by.id("interestRate"));
calculatePage.url = "/capitec/#/still-looking";
calculatePage.btnCalculate = element(by.id("submitButton"));
calculatePage.titleText = "see how much you could qualify for";
calculatePage.headerText = 'calculate';

calculatePage.calculate = function(calculateCriteria){
	calculatePage.populateFields(calculateCriteria);
	calculatePage.btnCalculate.click();
	return calculateResultsPage;
};

calculatePage.populateFields = function(calculateCriteria){
	calculatePage.clearInputAndPopulate(calculatePage.householdIncome, calculateCriteria.householdIncome);			
	calculatePage.clearInputAndPopulate(calculatePage.cashDeposit, calculateCriteria.cashDeposit);
	calculatePage.clearInputAndPopulate(calculatePage.interestRate, calculateCriteria.interestRate);
};

module.exports = calculatePage;