describe("Removing Applicants Specs ->", function() {
	var capitec = require('../../capitec.js');
	var clientCapturePage = require('../../pages/apply/client-capture-page.js');
	var landingPage;
	var applyPage;
	var applicationPreCheckPage;
	var newPurchasePage;
	var newPurchaseResultsPage;
	var navigate = true;	

	beforeEach(function(){
		if (navigate) {
			landingPage = capitec.login.LoginAsRandomTestUser();
			applyPage = landingPage.goToApplyPage();
			applicationPreCheckPage = applyPage.goToNewHomeApplication();
			newPurchasePage = applicationPreCheckPage.proceedForNewPurchaseApplication(1);
			newPurchaseResultsPage = newPurchasePage.submitPopulatedForm(500000, 150000, 25000, "Owner Occupied", "Self Employed");
			clientCapturePage = newPurchaseResultsPage.apply();
			clientCapturePage.setFirstNameTo("Applicant1");
			clientCapturePage.addAdditionalApplicant();
			clientCapturePage.goToApplicant('2');
			clientCapturePage.setFirstNameTo("Applicant2");
			navigate = false;
		};
	});

	describe("when removing the second applicant from an application with more than one applicant", function() {
		it("should allow the user to cancel their intention to remove the applicant", function() {
    		clientCapturePage.checkNumberOfApplicantsEquals(2);
    		clientCapturePage.removeApplicant(false);
    		clientCapturePage.checkNumberOfApplicantsEquals(2);
		});
		it("should allow the user to remove the applicant once they have confirmed their intention", function() {
    		clientCapturePage.checkNumberOfApplicantsEquals(2);
    		clientCapturePage.removeApplicant(true);
    		clientCapturePage.checkNumberOfApplicantsEquals(1);
		});
		it("should redirect the user back to the previous applicant's details", function(){
			expect(clientCapturePage.firstname.getAttribute('value')).toContain("Applicant1");
		});	  
	});	

	describe("when removing the first applicant from an application with more than one applicant", function() {  
		it("should allow the user to remove the applicant once they have confirmed their intention", function() {
    		clientCapturePage.addAdditionalApplicant();
    		clientCapturePage.goToApplicant('2');
			clientCapturePage.setFirstNameTo("Applicant3");
    		clientCapturePage.checkNumberOfApplicantsEquals(2);
			clientCapturePage.goToApplicant("1");
			clientCapturePage.removeApplicant(true);
    		clientCapturePage.checkNumberOfApplicantsEquals(1);
		});
		it("should redirect the user back to the previous applicant's details", function(){
			expect(clientCapturePage.firstname.getAttribute('value')).toContain("Applicant3");
		});	 
	});	
	
	describe("when removing the only applicant from an application", function() {
		it("should not allow the user to remove the applicant and display a validation message", function() {
    		clientCapturePage.removeApplicant(true);
			capitec.notifications.checkIfValidationMessageExists("The application requires at least one applicant.");
		});	  
	});	
});