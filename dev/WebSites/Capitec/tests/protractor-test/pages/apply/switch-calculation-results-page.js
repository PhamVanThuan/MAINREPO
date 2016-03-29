var page = require('../../page');

var switchResultsPage = new page();

switchResultsPage.url = "/for-switch/calculation-result";
switchResultsPage.headerText = "apply";
switchResultsPage.titleText = "you may qualify for a home loan - subject to full credit assessment";
switchResultsPage.btnBack = element(by.id('calcResultBackButton'));
switchResultsPage.btnApply = element(by.id('submit1'));

switchResultsPage.apply = function(){
	var clientCapturePage = require('./client-capture-page');
	switchResultsPage.btnApply.click();
	return clientCapturePage;
};

switchResultsPage.back = function(){
	var switchPage = require('./switch-page');
	switchResultsPage.btnBack.click();
	return switchPage;
}

module.exports = switchResultsPage;
