describe("Single Applicant Qualifying New Purchase Application Spec ->", function(){

	var capitec = require('../../capitec.js');
	var AsyncSpec = require('jasmine-async')(jasmine);
	var landingPage, applyPage, newPurchasePage, newPurchaseResultsPage, applicationPreCheckPage;
	var applicationSubmitPage, clientCapturePage, addressCapturePage, employmentCapturePage;
	var declarationsCapturePage, applyResultsPage, doNotNavigate, nextAvailableIDNumber;
	var applicationNumber, personId, applicantId, applicationId, applicationLoanDetailId;
	var applicantDeclarations, clientAddress, applicantApplicationId;

	beforeEach(function() {
		if(doNotNavigate)
			return;
		landingPage = capitec.login.LoginAsRandomTestUser();
		applyPage = landingPage.goToApplyPage();	
	});

	describe("when submitting a qualifying new purchase application with a single applicant", function(){
		var async = new AsyncSpec(this);
		async.beforeEach(function(done){
			if (!doNotNavigate){						
				doNotNavigate = true;
				var sql = capitec.queries.getPrefixedIDNumber.toString();
				capitec.queryDB(sql, [{name: 'IdPrefix', value: '840116'}], function(err, results){
					nextAvailableIDNumber = results[0].IDNumber;
					clientAddress = {
						streetName: capitec.common.randomString(6) + ' ' + capitec.common.randomString(6),
						streetNumber: capitec.common.randomString(5, '1234567890'),
						suburb: "Hillcrest",
						city: "Hillcrest",
						province:"Kwazulu-natal"
					};
					applicationPreCheckPage = applyPage.goToNewHomeApplication();
					newPurchasePage = applicationPreCheckPage.proceedForNewPurchaseApplication(1);
					newPurchaseResultsPage = newPurchasePage.submitPopulatedForm(500000, 22000, 25000, "Owner Occupied", "Salaried with Housing Allowance");
					clientCapturePage = newPurchaseResultsPage.apply();	
					addressCapturePage = clientCapturePage.fillForm(nextAvailableIDNumber);
					employmentCapturePage = addressCapturePage.captureAddress(clientAddress, true);
					declarationsCapturePage = employmentCapturePage.captureEmployment("Salaried with Housing Allowance", 25000, 6500);
					applicationSubmitPage = declarationsCapturePage.fillForm(true);
					idnumbers = [nextAvailableIDNumber];
					applicationSubmitPage.selectAllCheckBoxes(idnumbers);
					applyResultsPage = applicationSubmitPage.submitApplication();
					done();
				});		
			} else {
				done();
			}
		});
		it("should inform the applicant that the application has been submitted", function() {	
			expect(applyResultsPage.title()).toContain(applyResultsPage.titleText.accepted);
		});
		it("should not display warnings that applicant failed itc check", function() {
			applyResultsPage.isApplicantITCCheckFailedDisplayed(0, function(result){				
				expect(result).toBeFalsy();
			});
		});
		it("should not display warning messages for applicant", function() {
			applyResultsPage.firstApplicantMessages(function(messages){
				expect(messages).toEqual([]);
			});
		});
		async.it("should create the application and display the application number to the user", function(done){
			browser.sleep(5000);
			expect(applyResultsPage.applicationNumber.isDisplayed()).toBeTruthy();
			applyResultsPage.applicationNumber.getText().then(function setApplicationNumber(text){
				applicationNumber = text;	
				done();
			});
		});
		
		it("should have created a new purchase application linked to the application number", function(){
			capitec.queryDB(capitec.queries.getApplication, [{ 
				name: 'ApplicationNumber', value: applicationNumber 
			}], function(err, application){
				expect(application[0].ApplicationNumber.toString()).toEqual(applicationNumber);
				expect(application[0].ApplicationType).toEqual("New Purchase");
				applicationId = application[0].Id;			
			});
		});
	});
});