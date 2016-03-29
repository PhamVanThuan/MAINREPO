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
					newPurchaseResultsPage = newPurchasePage.submitPopulatedForm(500000, 150000, 25000, "Owner Occupied", "Self Employed");
					clientCapturePage = newPurchaseResultsPage.apply();	
					addressCapturePage = clientCapturePage.fillForm(nextAvailableIDNumber);
					employmentCapturePage = addressCapturePage.captureAddress(clientAddress, true);
					declarationsCapturePage = employmentCapturePage.captureEmployment("Self Employed", 25000);
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
		it("should have created a new purchase application linked to the application number", function(){
			capitec.queryDB(capitec.queries.getApplication, [{ 
				name: 'ApplicationNumber', value: applicationNumber 
			}], function(err, application){
				expect(application[0].ApplicationNumber.toString()).toEqual(applicationNumber);
				expect(application[0].ApplicationType).toEqual("New Purchase");
				applicationId = application[0].Id;			
			});
		});
		it("should have linked the applicant to the application", function() {
			capitec.queryDB(capitec.queries.getApplicantApplication, [
					{ name: 'ApplicationID', value: applicationId },
					{ name: 'ApplicantID', value: applicantId
				}], function(err, results){
				applicantApplicationId = results[0].Id;		
				expect(applicantApplicationId.length).toBeGreaterThan(0);
			});		  
		});
		it("should create an application with the correct application details", function(){
			capitec.queryDB(capitec.queries.getApplicationLoanDetail, [
					{ name: 'ApplicationID', value: applicationId }
				], function(err, applicationDetails){
				expect(applicationDetails[0].EmploymentType).toEqual("Self Employed");
				expect(applicationDetails[0].OccupancyType).toEqual("Owner Occupied");
				expect(applicationDetails[0].HouseholdIncome).toEqual(25000);
				expect(applicationDetails[0].LoanAmount).toEqual(350000);
				expect(applicationDetails[0].LTV).toEqual(0.7);			
				applicationLoanDetailId = applicationDetails[0].id;
			});				
		});
		it("should have stored the details specific to the new purchase application", function() {
			  capitec.queryDB(capitec.queries.getNewPurchaseApplicationLoanDetail, [
			  	{ name: 'ApplicationLoanDetailID', value: applicationLoanDetailId 
			  }], function(err, newPurchaseLoanDetails){
			  	expect(newPurchaseLoanDetails[0].Deposit).toEqual(150000);
			  	expect(newPurchaseLoanDetails[0].PurchasePrice).toEqual(500000);				  				  			
			  });
		});	
		it("should create an employment record for the applicant", function(){
			capitec.queryDB(capitec.queries.getApplicantEmployment, [{ 
				name: 'ApplicantID', value: applicantId 
			}], function(err, applicantEmployment){
				expect(applicantEmployment[0].Name).toEqual("Self Employed");
				expect(applicantEmployment[0].BasicIncome).toEqual(25000);	
			});		
		});	

		it("should save the address supplied for the applicant", function() {
			capitec.queryDB(capitec.queries.getApplicantAddress, [{ 
				name: 'ApplicantID', value: applicantId 
			}], function(err, address){
				expect(address[0].StreetNumber).toEqual(clientAddress.streetNumber);
				expect(address[0].StreetName).toEqual(clientAddress.streetName);
				expect(address[0].SuburbName).toEqual(clientAddress.suburb);
				expect(address[0].CityName).toEqual(clientAddress.city);
				expect(address[0].ProvinceName).toEqual(clientAddress.province);
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
	});
});