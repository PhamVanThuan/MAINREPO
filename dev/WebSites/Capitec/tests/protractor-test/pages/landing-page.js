var page = require('../page.js');
var capitec = require('../capitec.js');
var applyPage = require('../pages/apply/apply-page.js');
var trackingPage = require('../pages/track/tracking-page.js');
var calculatePage = require('../pages/calculate/calculate-page.js');

var landingPage = new page();

landingPage.splashBody = element(by.className("splashbody"));
landingPage.calculate = element(by.id("calculate"));
landingPage.apply = element(by.css('a[ui-sref="home.content.apply"]'));
landingPage.track = element(by.id("track"));
landingPage.url = "/capitec/#/";

landingPage.goToCalculatePage = function(){
	landingPage.calculate.click();
	return calculatePage;
};
landingPage.goToApplyPage = function(){
	landingPage.apply.click();
	return applyPage;
};
landingPage.goToTrackingPage = function(){
	landingPage.track.click();
	return trackingPage;
};
landingPage.goToAdministrationPage = function(){
	capitec.horizontalMenu.goToAdministration();
	return adminLandingPage;
};

module.exports = landingPage;