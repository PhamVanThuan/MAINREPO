var page = require('../../page.js');

var calculateResultsPage = new page();
calculateResultsPage.url = "/capitec/#/still-looking/calculation-result";	
calculateResultsPage.titleText = "calculation results - subject to full credit assessment";
calculateResultsPage.headerText = 'calculate';
calculateResultsPage.btnBack = element(by.id('submit2'));
calculateResultsPage.loanAmount = element(by.binding('calculationResult.amountQualifiedFor'));
calculateResultsPage.propertyPrice = element(by.binding('calculationResult.propertyPriceQualifiedFor'));
calculateResultsPage.interestRate = element(by.binding('calculationResult.interestRate'));
calculateResultsPage.instalment = element(by.binding('calculationResult.instalment'));
calculateResultsPage.loanTerm = element(by.binding('calculationResult.termInMonths'));
calculateResultsPage.serviceFee = element(by.binding('calculationResult.monthlyServiceFee'));

calculateResultsPage.back = function() {
	var calculatePage = require('./calculate-page.js');
	calculateResultsPage.btnBack.click();
	return calculatePage;
};

module.exports = calculateResultsPage;