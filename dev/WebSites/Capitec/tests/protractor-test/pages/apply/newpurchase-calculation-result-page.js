var page = require('../../page.js');
var newPurchaseResultsPage = new page();

newPurchaseResultsPage.url = "/for-new-home/calculation-result";
newPurchaseResultsPage.headerText = "apply";
newPurchaseResultsPage.titleText = "you may qualify for a home loan - subject to full credit assessment";
newPurchaseResultsPage.btnBack = element(by.id('calcResultBackButton'));
newPurchaseResultsPage.btnApply = element(by.id('submit1'));

newPurchaseResultsPage.back = function() {
	var newPurchasePage = require('./new-purchase-page');
	newPurchaseResultsPage.btnBack.click();
	return newPurchasePage;
};

newPurchaseResultsPage.apply = function() {
	var clientCapturePage = require('./client-capture-page');
	newPurchaseResultsPage.btnApply.click();
	return clientCapturePage;
};

module.exports = newPurchaseResultsPage;