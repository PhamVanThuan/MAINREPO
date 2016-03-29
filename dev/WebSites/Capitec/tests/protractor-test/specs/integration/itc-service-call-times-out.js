describe("Single Applicant Qualifying New Purchase Application Spec ->", function(){

	var capitec = require('../../capitec.js');
	var AsyncSpec = require('jasmine-async')(jasmine);
	var landingPage, applyPage, newPurchasePage, newPurchaseResultsPage, applicationPreCheckPage;
	var applicationSubmitPage, clientCapturePage, addressCapturePage, employmentCapturePage;
	var declarationsCapturePage, applyResultsPage, doNotNavigate, nextAvailableIDNumber;
	var applicationNumber, personId, applicantId, applicationId, applicationLoanDetailId;
	var applicantDeclarations, clientAddress, applicantApplicationId, clientDetails;

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
					clientDetails = {
						idNumber: nextAvailableIDNumber,
						dateOfBirth: '19' + nextAvailableIDNumber.substring(0,2) + '-' + nextAvailableIDNumber.substring(2,2) + '-' + nextAvailableIDNumber.substring(4,2),
						salutation: 'Mr',
						firstName: capitec.common.randomString(10),
						surname: 'Sleep_120000',
						contactDetails: { 
							emailAddress: capitec.common.randomString(10) + '@test.com',
							cellPhoneNumber: '052' + capitec.common.randomString(7, '1234567890'),
							workPhoneNumber: '031' + capitec.common.randomString(7, '1234567890'),
							},
						mainContact: true
						};	
					applicationPreCheckPage = applyPage.goToNewHomeApplication();
					newPurchasePage = applicationPreCheckPage.proceedForNewPurchaseApplication(1);
					newPurchaseResultsPage = newPurchasePage.submitPopulatedForm(400000, 15000, 18500, "Owner Occupied", "Salaried with Housing Allowance");
					clientCapturePage = newPurchaseResultsPage.apply();	
					addressCapturePage = clientCapturePage.addClientDetails(clientDetails, true);
					employmentCapturePage = addressCapturePage.captureAddress(clientAddress, true);
					declarationsCapturePage = employmentCapturePage.captureEmployment("Salaried with Housing Allowance", 17500, 1000);
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
		it("should display warning messages for applicant", function() {
			applyResultsPage.firstApplicantMessages(function(messages){
				expect(messages).toEqual(["No credit bureau check was performed."]);
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