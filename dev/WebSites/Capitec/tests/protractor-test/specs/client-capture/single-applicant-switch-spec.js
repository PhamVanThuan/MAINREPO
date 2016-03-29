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
								EstimatedPropertyValue : 500000, CashRequired: 200000, CurrentBalance: 200000, 
								Income: 15000, OccupancyType: "Owner Occupied" , EmploymentType: "Salaried" 
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
				var sql = capitec.queries.getNextIDNumber.toString();
				capitec.queryDB(sql, [], function(err, results){
					nextAvailableIDNumber = results[0].IDNumber;
					doNotNavigate = true;
					applicationPreCheckPage = applyPage.goToSwitchApplication();
					switchPage = applicationPreCheckPage.proceedForSwitchApplication(1);
					switchPageResultsPage = switchPage.submitPopulatedForm(testCaseDetails.EstimatedPropertyValue, testCaseDetails.CashRequired, testCaseDetails.CurrentBalance, 
						testCaseDetails.Income, testCaseDetails.OccupancyType, testCaseDetails.EmploymentType);
					clientCapturePage = switchPageResultsPage.apply();
					var clientDetails =	{ 
						idNumber: nextAvailableIDNumber, 
						surname: "Empirica_601", 
						firstName: "FirstName1 " + capitec.common.randomString(5), 
						dateOfBirth: "1980-01-01",  
						contactDetails: { 
							emailAddress: "test@sahomeloans.com",
							workPhoneNumber: "0315713036",
							cellPhoneNumber: "0827702444"
						},
						salutation: "Mr",
						mainContact: true
					};
					addressCapturePage = clientCapturePage.addClientDetails(clientDetails, true);					employmentCapturePage = addressCapturePage.fillForm();
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
		it("should have created a person record for the applicant", function() {
			capitec.queryDB(capitec.queries.getPerson, [{
				name: 'IdentityNumber', value: nextAvailableIDNumber
			}], function(err, results){
				expect(results[0].IdentityNumber).toEqual(nextAvailableIDNumber);
				personId = results[0].Id;
			});	  
		});
		it("should have created an applicant record for the person", function() {
			capitec.queryDB(capitec.queries.getApplicant, [{
				name: 'PersonID', value: personId
			}], function(err, results){
				expect(results[0].PersonID).toEqual(personId);
				applicantId = results[0].Id;
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
		it("should have linked the applicant to the application", function() {
			capitec.queryDB(capitec.queries.getApplicantApplication, [
					{ name: 'ApplicationID', value: applicationId },
					{ name: 'ApplicantID', value: applicantId
				}], function(err, applicant){
				expect(applicant.length).toEqual(1);
			});		  
		});
		it("should create an application with the correct application details", function(){
			capitec.queryDB(capitec.queries.getApplicationLoanDetail, [
					{ name: 'ApplicationID', value: applicationId }
				], function(err, applicationDetails){
				var expectedLoanAmount = testCaseDetails.CurrentBalance+testCaseDetails.CashRequired+applicationDetails[0].Fees;	
				expect(applicationDetails[0].EmploymentType).toEqual("Salaried");
				expect(applicationDetails[0].OccupancyType).toEqual("Owner Occupied");
				expect(applicationDetails[0].HouseholdIncome).toEqual(testCaseDetails.Income);
				expect(applicationDetails[0].LoanAmount).toEqual(expectedLoanAmount);
				expect(applicationDetails[0].LTV).toEqual(0.826);
				expect(applicationDetails[0].Fees).toBeGreaterThan(0);		
				applicationLoanDetailId = applicationDetails[0].id;
			});				
		});
		it("should have stored the details specific to the switch application", function() {
			  capitec.queryDB(capitec.queries.getSwitchApplicationLoanDetail, [
			  	{ name: 'ApplicationLoanDetailID', value: applicationLoanDetailId 
			  }], function(err, switchLoanDetails){
			  	expect(switchLoanDetails[0].CashRequired).toEqual(testCaseDetails.CashRequired);
			  	expect(switchLoanDetails[0].CurrentBalance).toEqual(testCaseDetails.CurrentBalance);		
			  	expect(switchLoanDetails[0].EstimatedMarketValueOfTheHome).toEqual(testCaseDetails.EstimatedPropertyValue);
			  	expect(switchLoanDetails[0].InterimInterest).toEqual(0);					  				  			
			  });
		});	
		it("should create an employment record for the applicant", function(){
			capitec.queryDB(capitec.queries.getApplicantEmployment, [{ 
				name: 'ApplicantID', value: applicantId 
			}], function(err, applicantEmployment){
				expect(applicantEmployment[0].Name).toEqual(testCaseDetails.EmploymentType);
				expect(applicantEmployment[0].BasicIncome).toEqual(testCaseDetails.Income);	
			});		
		});	

		it("should save the address supplied for the applicant", function() {
			capitec.queryDB(capitec.queries.getApplicantAddress, [{ 
				name: 'ApplicantID', value: applicantId 
			}], function(err, address){
				expect(address[0].StreetNumber).toEqual("99");
				expect(address[0].StreetName).toEqual("Rock Road");
				expect(address[0].SuburbName).toEqual("Hillcrest");
				expect(address[0].CityName).toEqual("Hillcrest");
				expect(address[0].ProvinceName).toEqual("Kwazulu-natal");
				expect(address[0].CountryName).toEqual("South Africa");						
			});												  
		});

		it("should save the email address for the applicant", function() {
			capitec.queryDB(capitec.queries.getEmailAddressContactDetail, [
				{ name: 'ApplicantID', value: applicantId 
			}], function(err, contactDetails){
				expect(contactDetails[0].emailAddress).toEqual("test@sahomeloans.com");
			});				  
		});

		it("should save the cellphone number for the applicant", function() {
			capitec.queryDB(capitec.queries.getPhoneContactDetailForApplicant, [{ 
				name: 'ApplicantID', value: applicantId },
				{ name: 'ContactDetailType', value: 'Mobile'
			}], function(err, contactDetails){
				expect(contactDetails[0].PhoneNumber).toEqual("0827702444");
			});				  
		}); 

		it("should save the workphone number for the applicant", function() {
			capitec.queryDB(capitec.queries.getPhoneContactDetailForApplicant, [{ 
				name: 'ApplicantID', value: applicantId },
				{ name: 'ContactDetailType', value: 'Work'
			}], function(err, contactDetails){
				expect(contactDetails[0].PhoneNumber).toEqual("0315713036");
			});				  
		});

		it("should have recorded whether the applicant is an income contributor", function() {
			capitec.queryDB(capitec.queries.getApplicantDeclarations, [{ 
				name: 'ApplicantID', value: applicantId
			}], function(err, declarations){
				applicantDeclarations = declarations;
				var incomeContributor = applicantDeclarations.filterBy({
					name: 'DeclarationText',
					value: 'Income Contributor'
				});
				expect(incomeContributor.Answer).toEqual("Yes");
			});							  
		});

		it("should have recorded whether the applicant is married in community of property", function() {
			var marriedInCOP = applicantDeclarations.filterBy({
				name: 'DeclarationText',
				value: 'Married In Community Of Property'
			});
			expect(marriedInCOP.Answer).toEqual("No");		  
		});

		it("should have recorded whether the applicant's salary is paid into a Capitec transactional account", function() {
			var salaryPaidToCapitec = applicantDeclarations.filterBy({
				name: 'DeclarationText',
				value: 'Has Capitec Transaction Account'
			});
			expect(salaryPaidToCapitec.Answer).toEqual("Yes");					  
		});
		it("should have recorded whether the applicant granted permission for the Credit Bureau Check", function() {
			var allowedToDoCreditBureauCheck = applicantDeclarations.filterBy({
				name: 'DeclarationText',
				value: 'Allow Credit Bureau Check'
			});
			expect(allowedToDoCreditBureauCheck.Answer).toEqual("Yes");					  
		});
		it("should have recorded the applicant as the main contact for the application", function() {
			capitec.queryDB(capitec.queries.getApplicant, [{
				name: 'PersonID', value: personId
			}], function(err, results){
				expect(results[0].MainContact).toBeTruthy();
			});	  
		});				
	});
});