describe("Client Capture Specifications -> ", function(){
  	var capitec = require('../../capitec');
	var AsyncSpec = require('jasmine-async')(jasmine);
  	var landingPage;
	var applyPage;
	var switchPage;
	var switchResultsPage;
	var applicationPreCheckPage;
	var clientCapturePage;
	var declarationsPage;
	var addressCapturePage;
	var employmentCapturePage;
	var nextAvailableIDNumber;
	var doNotNavigate = false;
	var async = new AsyncSpec(this);

	beforeEach(function() {
		if(doNotNavigate)
			return;
	  	landingPage = capitec.login.LoginAsRandomTestUser();
		applyPage = landingPage.goToApplyPage();
		applicationPreCheckPage = applyPage.goToSwitchApplication();
		switchPage = applicationPreCheckPage.proceedForSwitchApplication(1);
		switchResultsPage = switchPage.fillForm();
		clientCapturePage = switchResultsPage.apply();
	});

	describe("when submitting the application and the first applicant has indicated they are not an income contributor -> ", function() {
		var async = new AsyncSpec(this);
		async.beforeEach(function(done){
				if (!nextAvailableIDNumber){
					var sql = capitec.queries.getNextIDNumber.toString();
					capitec.queryDB(sql, [], function(err, results){
						nextAvailableIDNumber = results[0].IDNumber;
						done();
					});		
				} else {
					done();
				}
		});

		it("should change the description of the button to read Next Applicant", function() {
			doNotNavigate = true;
			addressCapturePage = clientCapturePage.fillForm(nextAvailableIDNumber);
			employmentCapturePage = addressCapturePage.fillForm();
			declarationsPage = employmentCapturePage.captureEmployment("Salaried", 15000);	
			var answers = { isIncomeContributor: "No", isHappyWeDoITC: "Yes", isSalaryPaidToCapitec: "Yes", isMarriedInCOP: "No" };
			declarationsPage.answerDeclarations(answers);
			expect(declarationsPage.btnPrintConsentForm.getText()).toEqual('Next Applicant'); 		  	  
	  	});
	  	
	  	it("should provide the user with the reason why they need to capture an additional applicant", function() {
			declarationsPage.printConsentForm();
			expect(capitec.notifications.infoMessages())
			.toContain("Please capture details for an applicant that is an income contributor.");  	  	  
	  	});

	  	it("should not submit the application and redirect the user to the client capture page", function() {
	  		expect(browser.getCurrentUrl()).toContain(clientCapturePage.url);  	  
	  	});

	  	it("should add the additional applicant icon to the screen", function() {
	  		clientCapturePage.checkNumberOfApplicantsEquals(2);  
	  	});

	  	it("should only allow the capture of 1 additional applicant", function(){
	  		clientCapturePage.checkAddApplicantIconIsNoLongerDisplayed();
		});
	});

});