var wizardPage = require('./client-capture-wizard-base-page.js');
var capitec = require('../../capitec');
var declarationsPage = new wizardPage();

declarationsPage.url = '/client-capture/declarations';
declarationsPage.titleText = 'declarations for current applicant';
declarationsPage.headerText = 'apply';
//radio buttons
declarationsPage.isIncomeContributor = { yes: element(by.id('incomeContributor_0')), no: element(by.id('incomeContributor_1')) };
declarationsPage.isHappyWeDoITC = { yes: element(by.id('allowCreditBureauCheck_0')), no: element(by.id('allowCreditBureauCheck_1')) };
declarationsPage.isSalaryPaidToCapitec = { yes: element(by.id('hasCapitecTransactionAccount_0')), no: element(by.id('hasCapitecTransactionAccount_1')) };
declarationsPage.isMarriedInCOP = { yes: element(by.id('marriedInCommunityOfProperty_0')), no: element(by.id('marriedInCommunityOfProperty_1')) };
//labels
declarationsPage.incomeContributorQuestionText = element(by.id('lblIncomeContributor'));
declarationsPage.permissionForITCQuestionText = element(by.id('lblCreditBureauCheck'));
declarationsPage.salaryPaidToCapitecQuestionText = element(by.id('lblCapitecAccount'));
declarationsPage.marriedInCOPQuestionText = element(by.id('lblMarriedCOP'));
declarationsPage.btnPrintConsentForm = element(by.id('printConsentButton'));

declarationsPage.setDeclaration = function(declaration, answer){
	answer = answer.toLowerCase();
	declarationsPage[declaration][answer].click();
};

declarationsPage.answerDeclarations = function(answers, isFirstApplicant){	
	for (var key in answers) {
			declarationsPage.setDeclaration(key, answers[key]);
	};
};

declarationsPage.fillForm = function(isFirstApplicant){
	var answers = { 
		isIncomeContributor: 'Yes', 
		isHappyWeDoITC: 'Yes', 
		isSalaryPaidToCapitec: 'Yes', 
		isMarriedInCOP: 'No', 
	};
	declarationsPage.answerDeclarations(answers, isFirstApplicant);
	return declarationsPage.printConsentForm();
};

declarationsPage.printConsentForm = function(){
	var applicationSubmitPage = require('./application-submit-page.js');
	capitec.notifications.closeAll();
	declarationsPage.btnPrintConsentForm.click();
	return applicationSubmitPage;
};

module.exports = declarationsPage;