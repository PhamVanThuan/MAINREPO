var page = require('../../page');
var capitec = require('../../capitec');
var applicationSubmitPage = new page();

applicationSubmitPage.url = "/application-submit";
applicationSubmitPage.headerText = "apply";
applicationSubmitPage.titleText = "please sign the client consent form";

applicationSubmitPage.btnSubmit = element(by.id('submitAppButton'));
applicationSubmitPage.btnBack = element(by.id('backButton'));
applicationSubmitPage.applicantIdentityNumber = element(by.binding('applicant.identityNumber'));
applicationSubmitPage.applicantFirstName = element(by.binding('applicant.firstName'));
applicationSubmitPage.applicantSurname = element(by.binding('applicant.surname'));
applicationSubmitPage.btnReprintAll = element(by.id('reprintAllButton'));

applicationSubmitPage.findCheckboxByIdNumber = function(idNumber){
	var checkbox = element(by.id("chk_"+idNumber));
		return checkbox;
};

applicationSubmitPage.selectAllCheckBoxes = function(idNumbers){
	for (var i = 0; i < idNumbers.length; i++) {
		var checkboxes = element(by.id("chk_"+idNumbers[i]));
			checkboxes.click();
	};
};

applicationSubmitPage.findReprintButtonByIdNumber = function(idNumber){
	var reprintButton = element(by.id("print_"+idNumber));
	return reprintButton;
};

applicationSubmitPage.findApplicantDescriptionByIdNumber = function(idNumber){
	var label = element(by.id(idNumber+"_applicant"));
	return label;
};

applicationSubmitPage.submitApplication = function(){
	var applyResultsPage = require('./apply-results-page');
	capitec.notifications.closeAll();
	applicationSubmitPage.btnSubmit.click();
	return applyResultsPage;
};

module.exports = applicationSubmitPage;





