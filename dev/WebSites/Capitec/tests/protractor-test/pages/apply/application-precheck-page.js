var page = require('../../page');

var applicationPreCheckPage = new page();

applicationPreCheckPage.SwitchUrl = "/switch/application-precheck";
applicationPreCheckPage.NewPurchaseUrl = "/apply/new-home/application-precheck";
applicationPreCheckPage.titleText = "please answer all questions";
applicationPreCheckPage.headerText = "apply";
applicationPreCheckPage.YesToAllApplicantsArePresent = element(by.id('allApplicantsArePresent_0'));
applicationPreCheckPage.NoToAllApplicantsArePresent = element(by.id('allApplicantsArePresent_1'));
applicationPreCheckPage.YesToUnderDebtCounselling = element(by.id('underDebtCounselling_0'));
applicationPreCheckPage.NoToUnderDebtCounselling = element(by.id('underDebtCounselling_1'));
applicationPreCheckPage.YesToWithinSpecifiedAge = element(by.id('specifiedAge_0'));
applicationPreCheckPage.NoToWithinSpecifiedAge = element(by.id('specifiedAge_1'));
applicationPreCheckPage.YesToPropertyHasTitleDeed = element(by.id('titleDeed_0'));
applicationPreCheckPage.NoToPropertyHasTitleDeed = element(by.id('titleDeed_1'));
applicationPreCheckPage.YesToOfferToPurchase = element(by.id('linkedToSale_0'));
applicationPreCheckPage.NoToOfferToPurchase = element(by.id('linkedToSale_1'));
applicationPreCheckPage.btnProceed = element(by.id("proceedButton"));
applicationPreCheckPage.NumberOfApplicantsEqualsOne = element(by.id('numberOfApplicants_0'));
applicationPreCheckPage.NumberOfApplicantsEqualsTwo = element(by.id('numberOfApplicants_1'));

applicationPreCheckPage.answerAllApplicantsPresentQuestion = function(answer){
	if (answer === 'Yes'){
		applicationPreCheckPage.YesToAllApplicantsArePresent.click(); 
	}
	else {
		applicationPreCheckPage.NoToAllApplicantsArePresent.click(); 
	}
};

applicationPreCheckPage.answerDebtCounsellingQuestion = function(answer){
	if (answer === 'Yes'){
		applicationPreCheckPage.YesToUnderDebtCounselling.click(); 
	}
	else {
		applicationPreCheckPage.NoToUnderDebtCounselling.click(); 
	}
};

applicationPreCheckPage.answerTitleDeedQuestion = function(answer){
	if (answer === 'Yes'){
		applicationPreCheckPage.YesToPropertyHasTitleDeed.click(); 
	}
	else {
		applicationPreCheckPage.NoToPropertyHasTitleDeed.click(); 
	}
};

applicationPreCheckPage.answerAgeQuestion = function(answer){
	if (answer === 'Yes'){
		applicationPreCheckPage.YesToWithinSpecifiedAge.click(); 
	}
	else {
		applicationPreCheckPage.NoToWithinSpecifiedAge.click(); 	
	}
};

applicationPreCheckPage.answerLinkedToPropertyQuestion = function(answer){
	if (answer === 'Yes'){
		applicationPreCheckPage.YesToOfferToPurchase.click(); 
	}
	else {
		applicationPreCheckPage.NoToOfferToPurchase.click(); 			
	}
};

applicationPreCheckPage.proceedForSwitchApplication = function(noOfApplicants){
	var switchPage = require('./switch-page');
	applicationPreCheckPage.selectNumberOfApplicants(noOfApplicants)
	applicationPreCheckPage.answerAllApplicantsPresentQuestion("Yes");
	applicationPreCheckPage.answerDebtCounsellingQuestion("No");
	applicationPreCheckPage.answerAgeQuestion("Yes");
	applicationPreCheckPage.answerTitleDeedQuestion("Yes");
	applicationPreCheckPage.btnProceed.click();
	return switchPage;
};

applicationPreCheckPage.proceedForNewPurchaseApplication = function(noOfApplicants){
	var newPurchasePage = require('./new-purchase-page');
	applicationPreCheckPage.selectNumberOfApplicants(noOfApplicants)
	applicationPreCheckPage.answerAllApplicantsPresentQuestion("Yes");
	applicationPreCheckPage.answerDebtCounsellingQuestion("No");
	applicationPreCheckPage.answerAgeQuestion("Yes");
	applicationPreCheckPage.answerTitleDeedQuestion("Yes");
	applicationPreCheckPage.answerLinkedToPropertyQuestion("Yes");
	applicationPreCheckPage.btnProceed.click();
	return newPurchasePage;
};

applicationPreCheckPage.selectNumberOfApplicants = function(noOfApplicants){
		if (noOfApplicants == 1){
			applicationPreCheckPage.NumberOfApplicantsEqualsOne.click(); 
		}
		else {
			applicationPreCheckPage.NumberOfApplicantsEqualsTwo.click(); 			
		}
};

module.exports = applicationPreCheckPage;
