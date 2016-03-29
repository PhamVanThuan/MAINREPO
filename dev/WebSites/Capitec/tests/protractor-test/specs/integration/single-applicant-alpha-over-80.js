describe("Single Applicant Qualifying Switch Application Spec ->", function(){

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
	var applicationLoanDetailId;
	var applicantDeclarations;
	var testCaseDetails = { 	
								EstimatedPropertyValue : 500000, 
								CashRequired: 200000, 
								CurrentBalance: 225000, 
								Income: 16500, 
								OccupancyType: "Owner Occupied" , 
								EmploymentType: "Salaried" 
							};

	beforeEach(function() {
		if(doNotNavigate)
			return;
		landingPage = capitec.login.LoginAsRandomTestUser();
		applyPage = landingPage.goToApplyPage();		
	});

	describe("when submitting a qualifying switch application with a single applicant", function(){
		var async = new AsyncSpec(this);
		async.beforeEach(function(done){
			if (!doNotNavigate){	
				var sql = capitec.queries.getPrefixedIDNumber.toString();
				capitec.queryDB(sql, [{name: 'IdPrefix', value: '840116'}], function(err, results){
					nextAvailableIDNumber = results[0].IDNumber;
					doNotNavigate = true;
					applicationPreCheckPage = applyPage.goToSwitchApplication();
					switchPage = applicationPreCheckPage.proceedForSwitchApplication(1);
					switchPageResultsPage = switchPage.submitPopulatedForm(testCaseDetails.EstimatedPropertyValue, testCaseDetails.CashRequired, testCaseDetails.CurrentBalance, 
						testCaseDetails.Income, testCaseDetails.OccupancyType, testCaseDetails.EmploymentType);
					clientCapturePage = switchPageResultsPage.apply();
					addressCapturePage = clientCapturePage.fillForm(nextAvailableIDNumber);
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