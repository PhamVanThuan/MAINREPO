var page = require('../../page.js');
var newPurchaseCalculationResultPage = require('./newpurchase-calculation-result-page')
var newPurchaseCalculationResultFailPage = require('./newpurchase-calculation-result-page-fail')
var capitec = require('../../capitec');
var newPurchasePage = new page();

newPurchasePage.url = "/new-home/application-precheck/for-new-home";
newPurchasePage.titleText = "financial info for purchasing a new home";
newPurchasePage.headerText = "apply";

newPurchasePage.selectedOccupancyType =element(by.id('occupancyTypeSpan'));
newPurchasePage.selectedIncomeType = element(by.id('incomeTypeSpan'));

newPurchasePage.occupancyType = element(by.model('occupancyType'));
newPurchasePage.incomeType = element(by.model('incomeType'));
newPurchasePage.occupancyTypes = newPurchasePage.occupancyType.element.all(by.tagName('option'));
newPurchasePage.incomeTypes = newPurchasePage.incomeType.element.all(by.tagName('option'));

newPurchasePage.newPurchaseForm = element(by.id('newPurchaseForm'));
newPurchasePage.purchasePrice = element(by.id('purchasePrice'));
newPurchasePage.cashDeposit = element(by.id('deposit'));
newPurchasePage.grossIncome = element(by.id('householdIncome'));
newPurchasePage.backButton = element(by.id('submit2'));
newPurchasePage.submitButton = element(by.id('submitButton'));
newPurchasePage.divContainingInputElements = element(by.id('pageContent'));

newPurchasePage.backButtons = element.all(by.id('submit2'));

newPurchasePage.submit = function(expectToFail) {
	capitec.notifications.closeAll();
	newPurchasePage.submitButton.click();

	if(expectToFail)
	{
		return newPurchaseCalculationResultFailPage;
	}
	return newPurchaseCalculationResultPage;
	
};

newPurchasePage.back = function() {
	var precheckPage = require('./application-precheck-page')
	newPurchasePage.backButton.click();
	return precheckPage;
};

newPurchasePage.setOccupancyTypeTo = function(occupancyType){
	newPurchasePage.selectOption(newPurchasePage.occupancyType, occupancyType);
};

newPurchasePage.setIncomeTypeTo = function(incomeType){
	newPurchasePage.selectOption(newPurchasePage.incomeType, incomeType);
};

newPurchasePage.populateForm = function(purchasePriceValue, cashDepositValue, grossIncomeValue) {
	newPurchasePage.purchasePrice.clear()
	newPurchasePage.purchasePrice.sendKeys(purchasePriceValue);
	newPurchasePage.cashDeposit.clear()
	newPurchasePage.cashDeposit.sendKeys(cashDepositValue);
	newPurchasePage.grossIncome.clear()
	newPurchasePage.grossIncome.sendKeys(grossIncomeValue);
};

newPurchasePage.submitPopulatedForm = function(purchasePriceValue, cashDepositValue, grossIncomeValue, occupancyType, incomeType, expectToFail) {
	newPurchasePage.populateForm(purchasePriceValue,cashDepositValue,grossIncomeValue);
	newPurchasePage.setOccupancyTypeTo(occupancyType);
	newPurchasePage.setIncomeTypeTo(incomeType);
	return newPurchasePage.submit(expectToFail);
};

newPurchasePage.reset = function(){
	newPurchasePage.clearInputs(newPurchasePage.divContainingInputElements);
	newPurchasePage.setOccupancyTypeTo("Please Select");
	newPurchasePage.setIncomeTypeTo("Please Select");
};

module.exports = newPurchasePage;