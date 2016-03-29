describe("Client Capture Specifications ->", function() {
	var capitec = require('../../capitec.js');
	var AsyncSpec = require('jasmine-async')(jasmine);

	var landingPage;
	var applyPage;
	var applicationPreCheckPage;
	var newPurchasePage;
	var newPurchaseResultsPage;
	var clientCapturePage;
	var existingIDNumber;
	var newIDNumber;
	var ApplicationNumber
	var declarationsPage;
	var navigate = true;
	var addressCapturePage;
	var employmentCapturePage;

	var async = new AsyncSpec(this);
	async.beforeEach(function(done){
		if (!newIDNumber){
			var sql1 = capitec.queries.getNextIDNumber.toString();
			capitec.queryDB(sql1, [], function(err1, results1){				
				newIDNumber = results1[0].IDNumber;
				var sql2 = capitec.queries.getExistingIdNumberWithAppNumber.toString();
				capitec.queryDB(sql2, [], function(err2, results2){
					existingIDNumber = results2[0].IdentityNumber;
					ApplicationNumber = results2[0].ApplicationNumber;
					done();
				});	
			});		
		} else {
			done();
		}
	});
	beforeEach(function() {
		if (navigate) {
		  	landingPage = capitec.login.LoginAsRandomTestUser();
			applyPage = landingPage.goToApplyPage();
			applicationPreCheckPage = applyPage.goToNewHomeApplication();
			newPurchasePage = applicationPreCheckPage.proceedForNewPurchaseApplication(1);
			newPurchaseResultsPage = newPurchasePage.submitPopulatedForm(500000, 150000, 25000, "Owner Occupied", "Self Employed");
			clientCapturePage = newPurchaseResultsPage.apply();
			clientCapturePage.setIdentityNumber(existingIDNumber);
			navigate = false;
		};
	});
	describe("when capturing the ID number for a client that already has an existing application", function() {
		it("should provide the user with a warning indicating that the user already has an application", function() {
		    capitec.notifications.checkIfValidationMessageExists("Application ", ApplicationNumber, " exists for applicant with identity number ", existingIDNumber);
	    });
	    it("should not allow the user to proceed with the application if they try and submit it", function() {
			addressCapturePage = clientCapturePage.fillForm(existingIDNumber);
			employmentCapturePage = addressCapturePage.fillForm();
			declarationsPage = employmentCapturePage.captureEmployment("Salaried", 15000);	
	   		expect(declarationsPage.btnPrintConsentForm.getAttribute("disabled")).toContain("true");
	    });	   
	    it("should not allow the user to proceed with the application if they try and submit a multi-applicant application", function() {
	    	declarationsPage.addAdditionalApplicant();
	    	clientCapturePage = declarationsPage.goToApplicant('2');
			clientCapturePage.fillForm(newIDNumber);
			employmentCapturePage = addressCapturePage.fillForm();
			declarationsPage = employmentCapturePage.captureEmployment("Salaried", 15000);	
	   		expect(declarationsPage.btnPrintConsentForm.getAttribute("disabled")).toContain("true");
	    });	  
  	});  
});