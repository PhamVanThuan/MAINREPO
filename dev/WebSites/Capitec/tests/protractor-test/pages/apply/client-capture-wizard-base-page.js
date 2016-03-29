var page = require('../../page');
var capitec = require('../../capitec');
var wizardBase = page;

wizardBase.clientInfoImage = element(by.id("clientInfo")); 
wizardBase.addressImage = element(by.id("clientAddress"));
wizardBase.employmentImage = element(by.id("clientFinancials"));
wizardBase.declarationsImage = element(by.id("clientDeclarations"));
wizardBase.btnBack = element(by.id("backButton"));
wizardBase.btnNext = element(by.id("NextButton"));
wizardBase.addApplicantIcon = element(by.id("imgAddApplicant"));
wizardBase.applicantIcons = element.all(by.repeater("applicant in application.applicants"));
wizardBase.removeClient = element(by.id("removeClient"));
wizardBase.btnCancel = element(by.css("button[ng-click~='onClickCancel();']"));
wizardBase.btnRemove = element(by.css("button[ng-click~='onClickRemoveClient();']"));

wizardBase.prototype.goToClientPage = function(){
	var clientCapturePage = require('./client-capture-page');
	capitec.notifications.closeAll();
	wizardBase.btnNext.click();
	return clientCapturePage;
};

wizardBase.prototype.goToAddressPage = function(){
	var addressPage = require('./address-capture-page');
	capitec.notifications.closeAll();
	wizardBase.btnNext.click();
	return addressPage;
};

wizardBase.prototype.goToEmploymentPage = function() {
	var employmentPage = require('./employment-page');
	capitec.notifications.closeAll();
	wizardBase.btnNext.click();
	return employmentPage;
};

wizardBase.prototype.goToDeclarationsPage = function() {
	var declarationsPage = require('./declarations-page');
	capitec.notifications.closeAll();
	wizardBase.btnNext.click();
	return declarationsPage;
};

wizardBase.prototype.goBack = function() {
	wizardBase.btnBack.click();
};

wizardBase.prototype.goForward = function() {
	capitec.notifications.closeAll();
	wizardBase.btnNext.click();
};

wizardBase.prototype.addAdditionalApplicant = function() {
	wizardBase.addApplicantIcon.click();
};

wizardBase.prototype.checkNumberOfApplicantsEquals = function(applicantCount){
	wizardBase.applicantIcons.then(function(arr){
		expect(arr.length).toEqual(applicantCount);
	});
};

wizardBase.prototype.checkAddApplicantIconIsDisplayed = function(){
	expect(wizardBase.addApplicantIcon.isDisplayed()).toBeTruthy();
};

wizardBase.prototype.checkAddApplicantIconIsNoLongerDisplayed = function(){
	expect(capitec.common.checkElementExists(protractor.By.id('imgAddApplicant'))).toBeFalsy();
};

wizardBase.prototype.goToApplicant = function(number){
	var clientCapturePage = require('./client-capture-page');
	wizardBase.applicantIcons.then(function(applicantIcons){
		applicantIcons.some(function(applicantIcon){
			applicantIcon.getText().then(function(text){
				if (text === number) {
					applicantIcon.click();
				};
			})
		})
	});
	return clientCapturePage;
};

wizardBase.prototype.removeApplicant = function(confirm) {
	var clientCapturePage = require('./client-capture-page');
	wizardBase.removeClient.click();
	if(confirm)
		wizardBase.btnRemove.click();
	else
		wizardBase.btnCancel.click();
	return clientCapturePage;
};

module.exports = wizardBase;