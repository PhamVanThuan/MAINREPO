describe("single application cat 0 pricing for risk ->", function(){

	var capitec = require('../../capitec.js');
	var AsyncSpec = require('jasmine-async')(jasmine);
	var landingPage;
	var applyPage;
	var switchPage;
	var switchPageResultsPage;
	var applicationSubmitPage;
	var applicationPreCheckPage;
	var clientCapturePage;
	var addressCapturePage;
	var employmentCapturePage;
	var declarationsCapturePage;
	var applyResultsPage;
	var doNotNavigate;
	var nextAvailableIDNumber;
	var applicationNumber;
	var personId;
	var applicantId;
	var applicationId;
	var clientDetails;
	var applicationLoanDetailId;
	var applicantDeclarations;
	var testCaseDetails = { 	
								EstimatedPropertyValue : 800000, 
								CashRequired: 200000, 
								CurrentBalance: 200000, 
								Income: 15000, 
								OccupancyType: "Owner Occupied" , 
								EmploymentType: "Salaried" 
							};

	beforeEach(function() {
		if(doNotNavigate)
			return;
		landingPage = capitec.login.LoginAsRandomTestUser();
		applyPage = landingPage.goToApplyPage();		
	});

	describe("when submitting a switch application with pricing for risk empirica values", function(){
		var async = new AsyncSpec(this);
		async.beforeEach(function(done){
			if (!doNotNavigate){	
				var sql = capitec.queries.getPrefixedIDNumber.toString();
				capitec.queryDB(sql, [{name: 'IdPrefix', value: '840116'}], function(err, results){
					nextAvailableIDNumber = results[0].IDNumber;
					doNotNavigate = true;
					clientDetails = {
						idNumber: nextAvailableIDNumber,
						salutation: 'Mr',
						firstName: capitec.common.randomString(10),
						surname: 'Empirica_00594',
						contactDetails: { 
							emailAddress: capitec.common.randomString(10) + '@test.com',
							cellPhoneNumber: '052' + capitec.common.randomString(7, '1234567890'),
							workPhoneNumber: '031' + capitec.common.randomString(7, '1234567890'),
							},
						mainContact: true
					};
					applicationPreCheckPage = applyPage.goToSwitchApplication();
					switchPage = applicationPreCheckPage.proceedForSwitchApplication(1);
					switchPageResultsPage = switchPage.submitPopulatedForm(testCaseDetails.EstimatedPropertyValue, testCaseDetails.CashRequired, testCaseDetails.CurrentBalance, 
						testCaseDetails.Income, testCaseDetails.OccupancyType, testCaseDetails.EmploymentType);
					clientCapturePage = switchPageResultsPage.apply();
					addressCapturePage = clientCapturePage.addClientDetails(clientDetails, true);
					employmentCapturePage = addressCapturePage.fillForm();
					declarationsCapturePage = employmentCapturePage.captureEmployment(testCaseDetails.EmploymentType, testCaseDetails.Income);
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
		it("should allow the user to complete the process", function(){
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
		it("should have created a switch application linked to the application number", function(){
			capitec.queryDB(capitec.queries.getApplication, [{ 
				name: 'ApplicationNumber', value: applicationNumber 
			}], function(err, application){
				expect(application[0].ApplicationNumber.toString()).toEqual(applicationNumber);
				expect(application[0].ApplicationType).toEqual("Switch");
				applicationId = application[0].Id;		
			});
		});		
	});
});