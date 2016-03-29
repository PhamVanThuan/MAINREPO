var page = require('../../page');
var switchPage = require('./switch-page');
var applyPage = new page();
var applicationPreCheckPage = require('./application-precheck-page');

applyPage.url = "/apply";
applyPage.titleText = "choose the type of home loan you need";
applyPage.headerText = "apply";

applyPage.switchApp = element(by.id('track'));
applyPage.newHomeApp = element(by.id('calculate'));

applyPage.goToNewHomeApplication = function() {
	applyPage.newHomeApp.click();
	return applicationPreCheckPage;
};

applyPage.goToSwitchApplication = function() {
	applyPage.switchApp.click();
	return applicationPreCheckPage;
};

module.exports = applyPage;