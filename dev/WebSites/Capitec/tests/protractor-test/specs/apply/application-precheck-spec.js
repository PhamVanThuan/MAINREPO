describe("Application Pre-check Capture Specifications ->", function() {
	var doNotNavigate = false;
	var applicationPreCheckPage;
	var capitec;
	var landingPage;
	var applyPage;

	beforeEach(function() {
		if (doNotNavigate)
			return;
		capitec = require('../../capitec.js');	
		landingPage = capitec.login.LoginAsRandomTestUser();
		applyPage = landingPage.goToApplyPage();
	});

	describe("when navigating to the application pre-check page for a switch application", function() {
		it("should contain the correct URL", function() {
			doNotNavigate = true;
			applicationPreCheckPage = applyPage.goToSwitchApplication();
			expect(browser.getCurrentUrl()).toContain(applicationPreCheckPage.SwitchUrl);
		});
		it("should contain the correct page title", function() {
	  	  	expect(applicationPreCheckPage.title()).toContain(applicationPreCheckPage.titleText);
		});
		it("should contain the correct page header", function() {
			expect(applicationPreCheckPage.header()).toContain(applicationPreCheckPage.headerText);
	  	});
	  	it("should ask the user for the number of applicants required on the application", function() {
	  		expect(applicationPreCheckPage.NumberOfApplicantsEqualsOne.isDisplayed()).toBeTruthy();
	  		expect(applicationPreCheckPage.NumberOfApplicantsEqualsTwo.isDisplayed()).toBeTruthy();  
	  		expect(applicationPreCheckPage.NumberOfApplicantsEqualsOne.getAttribute('checked')).toBeFalsy();  
	  		expect(applicationPreCheckPage.NumberOfApplicantsEqualsTwo.getAttribute('checked')).toBeFalsy();
	  		applicationPreCheckPage.selectNumberOfApplicants(1);    
	  	});
	  	it("should display the applicants present question and answer fields", function() {
 			expect(applicationPreCheckPage.YesToAllApplicantsArePresent.isDisplayed()).toBeTruthy();
 			expect(applicationPreCheckPage.NoToAllApplicantsArePresent.isDisplayed()).toBeTruthy();
 			expect(applicationPreCheckPage.YesToAllApplicantsArePresent.getAttribute('checked')).toBeFalsy();
 			expect(applicationPreCheckPage.NoToAllApplicantsArePresent.getAttribute('checked')).toBeFalsy();

 			applicationPreCheckPage.answerAllApplicantsPresentQuestion("No");
 			capitec.informationToolTip.checkToolTipMessageIsDisplayed("All applicants need to be present in the branch in order to apply for a home loan.");			
  	  	});
  	  	it("should display the debt counselling question and answer fields", function() {
  	  		applicationPreCheckPage.answerAllApplicantsPresentQuestion("Yes");

 			expect(applicationPreCheckPage.YesToUnderDebtCounselling.isDisplayed()).toBeTruthy();
 			expect(applicationPreCheckPage.NoToUnderDebtCounselling.isDisplayed()).toBeTruthy();
 			expect(applicationPreCheckPage.YesToUnderDebtCounselling.getAttribute('checked')).toBeFalsy();
 			expect(applicationPreCheckPage.NoToUnderDebtCounselling.getAttribute('checked')).toBeFalsy();

 			applicationPreCheckPage.answerDebtCounsellingQuestion("Yes");
 			capitec.informationToolTip.checkToolTipMessageIsDisplayed("No applicants are allowed to be under debt counselling when applying for a home loan.");
  	  	});
	  	it("should display the age question and answer fields", function() {
	  		applicationPreCheckPage.answerDebtCounsellingQuestion("No");

	  		expect(applicationPreCheckPage.YesToWithinSpecifiedAge.isDisplayed()).toBeTruthy();
 			expect(applicationPreCheckPage.NoToWithinSpecifiedAge.isDisplayed()).toBeTruthy();
 			expect(applicationPreCheckPage.YesToWithinSpecifiedAge.getAttribute('checked')).toBeFalsy();
 			expect(applicationPreCheckPage.NoToWithinSpecifiedAge.getAttribute('checked')).toBeFalsy();

 			applicationPreCheckPage.answerAgeQuestion("No");
 			capitec.informationToolTip.checkToolTipMessageIsDisplayed("All applicants need to be between the ages of 18 and 60 years old in order to apply for a home loan."); 
  	  	});
  	  	it("should display the title deed question and answer fields", function(){
  	  		applicationPreCheckPage.answerAgeQuestion("Yes");

  	  		expect(applicationPreCheckPage.NoToPropertyHasTitleDeed.isDisplayed()).toBeTruthy();
  	  		expect(applicationPreCheckPage.YesToPropertyHasTitleDeed.isDisplayed()).toBeTruthy();
  	  		expect(applicationPreCheckPage.NoToPropertyHasTitleDeed.getAttribute('checked')).toBeFalsy();
  	  		expect(applicationPreCheckPage.YesToPropertyHasTitleDeed.getAttribute('checked')).toBeFalsy();

  	  		applicationPreCheckPage.answerTitleDeedQuestion("No");
  	  		capitec.informationToolTip.checkToolTipMessageIsDisplayed("A title deed is a requirement for applying for a home loan."); 

  	  	});  
  	  	it("should display the proceed button if all questions were answered appropriately", function(){
  	  		applicationPreCheckPage.answerTitleDeedQuestion("Yes");
  	  		expect(applicationPreCheckPage.btnProceed.isDisplayed()).toBeTruthy();
  	  		doNotNavigate = false;
  	  	});
    	    	
  	});

	describe("when navigating to the application pre-check page for a new purchase application", function() {
		it("should contain the correct URL", function() {
			doNotNavigate = true;
			applicationPreCheckPage = applyPage.goToNewHomeApplication();
			expect(browser.getCurrentUrl()).toContain(applicationPreCheckPage.NewPurchaseUrl);
		});
		it("should contain the correct page title", function() {
	  	  	expect(applicationPreCheckPage.title()).toContain(applicationPreCheckPage.titleText);
		});
		it("should contain the correct page header", function() {
			expect(applicationPreCheckPage.header()).toContain(applicationPreCheckPage.headerText);
	  	});
	  	it("should ask the user for the number of applicants required on the application", function() {
	  		expect(applicationPreCheckPage.NumberOfApplicantsEqualsOne.isDisplayed()).toBeTruthy();
	  		expect(applicationPreCheckPage.NumberOfApplicantsEqualsTwo.isDisplayed()).toBeTruthy();  
	  		expect(applicationPreCheckPage.NumberOfApplicantsEqualsOne.getAttribute('checked')).toBeFalsy();  
	  		expect(applicationPreCheckPage.NumberOfApplicantsEqualsTwo.getAttribute('checked')).toBeFalsy();
	  		applicationPreCheckPage.selectNumberOfApplicants(1);    
	  	});
	  	it("should display the applicants present question and answer fields", function() {
 			expect(applicationPreCheckPage.YesToAllApplicantsArePresent.isDisplayed()).toBeTruthy();
 			expect(applicationPreCheckPage.NoToAllApplicantsArePresent.isDisplayed()).toBeTruthy();
 			expect(applicationPreCheckPage.YesToAllApplicantsArePresent.getAttribute('checked')).toBeFalsy();
 			expect(applicationPreCheckPage.NoToAllApplicantsArePresent.getAttribute('checked')).toBeFalsy();

 			applicationPreCheckPage.answerAllApplicantsPresentQuestion("No");
 			capitec.informationToolTip.checkToolTipMessageIsDisplayed("All applicants need to be present in the branch in order to apply for a home loan.");			
  	  	});
  	  	it("should display the debt counselling question and answer fields", function() {
  	  		applicationPreCheckPage.answerAllApplicantsPresentQuestion("Yes");

 			expect(applicationPreCheckPage.YesToUnderDebtCounselling.isDisplayed()).toBeTruthy();
 			expect(applicationPreCheckPage.NoToUnderDebtCounselling.isDisplayed()).toBeTruthy();
 			expect(applicationPreCheckPage.YesToUnderDebtCounselling.getAttribute('checked')).toBeFalsy();
 			expect(applicationPreCheckPage.NoToUnderDebtCounselling.getAttribute('checked')).toBeFalsy();

 			applicationPreCheckPage.answerDebtCounsellingQuestion("Yes");
 			capitec.informationToolTip.checkToolTipMessageIsDisplayed("No applicants are allowed to be under debt counselling when applying for a home loan.");
  	  	});
  	  	it("should display the age question and answer fields", function() {
	  		applicationPreCheckPage.answerDebtCounsellingQuestion("No");

	  		expect(applicationPreCheckPage.YesToWithinSpecifiedAge.isDisplayed()).toBeTruthy();
 			expect(applicationPreCheckPage.NoToWithinSpecifiedAge.isDisplayed()).toBeTruthy();
 			expect(applicationPreCheckPage.YesToWithinSpecifiedAge.getAttribute('checked')).toBeFalsy();
 			expect(applicationPreCheckPage.NoToWithinSpecifiedAge.getAttribute('checked')).toBeFalsy();

 			applicationPreCheckPage.answerAgeQuestion("No");
 			capitec.informationToolTip.checkToolTipMessageIsDisplayed("All applicants need to be between the ages of 18 and 60 years old in order to apply for a home loan."); 
  	  	});  

  	  	it("should display the title deed question and answer fields", function() {
	  		applicationPreCheckPage.answerAgeQuestion("Yes");

	  		expect(applicationPreCheckPage.YesToPropertyHasTitleDeed.isDisplayed()).toBeTruthy();
 			expect(applicationPreCheckPage.NoToPropertyHasTitleDeed.isDisplayed()).toBeTruthy();
 			expect(applicationPreCheckPage.YesToPropertyHasTitleDeed.getAttribute('checked')).toBeFalsy();
 			expect(applicationPreCheckPage.NoToPropertyHasTitleDeed.getAttribute('checked')).toBeFalsy();

 			applicationPreCheckPage.answerTitleDeedQuestion("No");
 			capitec.informationToolTip.checkToolTipMessageIsDisplayed("A title deed is a requirement for applying for a home loan."); 
  	  	});
  	  	
  	  	it("should display the offer to purchase question and answer fields", function(){
 	  		applicationPreCheckPage.answerTitleDeedQuestion("Yes");

	  		expect(applicationPreCheckPage.YesToOfferToPurchase.isDisplayed()).toBeTruthy();
 			expect(applicationPreCheckPage.NoToOfferToPurchase.isDisplayed()).toBeTruthy();
 			expect(applicationPreCheckPage.YesToOfferToPurchase.getAttribute('checked')).toBeFalsy();
 			expect(applicationPreCheckPage.NoToOfferToPurchase.getAttribute('checked')).toBeFalsy();

 			applicationPreCheckPage.answerLinkedToPropertyQuestion("No");
 			capitec.informationToolTip.checkToolTipMessageIsDisplayed("SA Home Loans cannot provide a general 'pre-approval' - we can only process a bond application based on a specific offer to purchase, where the purchase price is known.");	  		
  	  	});
  	  	it("should display the proceed button if all questions were answered appropriately", function(){	
  	  		applicationPreCheckPage.answerLinkedToPropertyQuestion("Yes");
  	  		expect(applicationPreCheckPage.btnProceed.isDisplayed()).toBeTruthy();
  	  		doNotNavigate = false;
  	  	});  	      	    	
  	});
});