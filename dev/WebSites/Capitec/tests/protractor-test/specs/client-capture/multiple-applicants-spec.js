describe("Multiple Applicant Qualifying New Purchase Application Spec ->", function(){

	var capitec = require('../../capitec.js');
	var AsyncSpec = require('jasmine-async')(jasmine);
	var landingPage, applyPage, newPurchasePage, newPurchaseResultsPage, applicationPreCheckPage, clientCapturePage, addressCapturePage;
	var employmentCapturePage, declarationsCapturePage, applyResultsPage, FirstAvailableIDNumber, SecondAvailableIDNumber, applicationNumber;
	var firstPersonId, secondPersonId, firstApplicantId, secondApplicantId, applicationId, applicationLoanDetailId, actualFirstApplicantDeclarations;
	var actualSecondApplicantDeclarations,client1Address, client2Address, client1Details, client2Details, firstApplicantDeclarations, secondApplicantDeclarations;
	var navigate = true;
	var done = false;


	var async = new AsyncSpec(this);
	async.beforeEach(function(done){
		if (!FirstAvailableIDNumber){
			var sql = capitec.queries.getNextIDNumber.toString();
			capitec.queryDB(sql, [], function(err, results){
				FirstAvailableIDNumber = results[0].IDNumber;
				SecondAvailableIDNumber = results[1].IDNumber;
				done();
			});		
		} else {
			done();
		}
	});

	beforeEach(function(){
		if (navigate) {
			client1Details = {
				idNumber: FirstAvailableIDNumber,
				dateOfBirth: '19' + FirstAvailableIDNumber.substring(0,2) + '-' + FirstAvailableIDNumber.substring(2,2) + '-' + FirstAvailableIDNumber.substring(4,2),
				salutation: 'Mr',
				firstName: capitec.common.randomString(10),
				surname: 'Client1',
				contactDetails: { 
					emailAddress: capitec.common.randomString(10) + '@test.com',
					cellPhoneNumber: '072' + capitec.common.randomString(7, '1234567890'),
					workPhoneNumber: '031' + capitec.common.randomString(7, '1234567890'),
					},
				mainContact: true
				};		
			client2Details = {
				idNumber: SecondAvailableIDNumber,
				dateOfBirth: '19' + SecondAvailableIDNumber.substring(0,2) + '-' + SecondAvailableIDNumber.substring(2,2) + '-' + SecondAvailableIDNumber.substring(4,2),
				salutation: 'Mr',
				firstName: capitec.common.randomString(10),
				surname: 'Client2',
				contactDetails: { 
					emailAddress: capitec.common.randomString(10) + '@test.com',
					cellPhoneNumber: '072' + capitec.common.randomString(7, '1234567890'),
					workPhoneNumber: '031' + capitec.common.randomString(7, '1234567890')
				},
				mainContact: false
			};
			client1Address = {
				streetName: 'Address1 Street',
				streetNumber: capitec.common.randomString(5, '1234567890'),
				suburb: "Hillcrest",
				city: "Hillcrest",
				province:"Kwazulu-natal"
			};
			client2Address = {
				streetName: 'Address2 Street',
				streetNumber: capitec.common.randomString(5, '1234567890'),
				suburb: "Hillcrest",
				city: "Hillcrest",
				province:"Kwazulu-natal"
			};
			landingPage = capitec.login.LoginAsRandomTestUser();
			applyPage = landingPage.goToApplyPage();
			applicationPreCheckPage = applyPage.goToNewHomeApplication();
			newPurchasePage = applicationPreCheckPage.proceedForNewPurchaseApplication(2);
			newPurchaseResultsPage = newPurchasePage.submitPopulatedForm(500000, 150000, 25000, "Owner Occupied", "Self Employed");
			clientCapturePage = newPurchaseResultsPage.apply();
			addressCapturePage = clientCapturePage.addClientDetails(client1Details, true);
			employmentCapturePage = addressCapturePage.captureAddress(client1Address, true);
			declarationsCapturePage = employmentCapturePage.captureEmployment("Self Employed", 10000);
			firstApplicantDeclarations = { isIncomeContributor: "Yes", isHappyWeDoITC: "Yes", isSalaryPaidToCapitec: "Yes", isMarriedInCOP: "No" };
			declarationsCapturePage.answerDeclarations(firstApplicantDeclarations);
			clientCapturePage = declarationsCapturePage.goToApplicant('2');
			addressCapturePage = clientCapturePage.addClientDetails(client2Details, true);
			employmentCapturePage = addressCapturePage.captureAddress(client2Address, true);
			declarationsCapturePage = employmentCapturePage.captureEmployment("Salaried", 20000);
			secondApplicantDeclarations = { isIncomeContributor: "Yes", isHappyWeDoITC: "Yes", isSalaryPaidToCapitec: "Yes", isMarriedInCOP: "No" };
			declarationsCapturePage.answerDeclarations(secondApplicantDeclarations);
			applicationSubmitPage = declarationsCapturePage.printConsentForm();
			idnumbers = [FirstAvailableIDNumber, SecondAvailableIDNumber];
			applicationSubmitPage.selectAllCheckBoxes(idnumbers);
			applyResultsPage = applicationSubmitPage.submitApplication();
			navigate = false;			
		};
	});
	describe("when submitting a qualifying new purchase application with multiple applicants", function(){
		it("should create the application and display the application number to the user", function(){
			browser.sleep(5000);
			expect(browser.getCurrentUrl()).toContain(applyResultsPage.url);	
			expect(applyResultsPage.applicationNumber.isDisplayed()).toBeTruthy();
			applyResultsPage.applicationNumber.getText().then(function setApplicationNumber(text){
				applicationNumber = text;	
			});
		});
		it("should have created a person record for first applicant", function() {
			capitec.queryDB(capitec.queries.getPerson, [{
				name: 'IdentityNumber', value: FirstAvailableIDNumber
			}], function(err, results){
				expect(results[0].IdentityNumber).toEqual(FirstAvailableIDNumber);
				firstPersonId = results[0].Id;
			});	  
		});		
		it("should have created a person record for second applicant", function() {
			capitec.queryDB(capitec.queries.getPerson, [{
				name: 'IdentityNumber', value: SecondAvailableIDNumber
			}], function(err, results){
				expect(results[0].IdentityNumber).toEqual(SecondAvailableIDNumber);
				secondPersonId = results[0].Id;
			});	
		});
		it("should have created an applicant record for the first applicant", function() {
			capitec.queryDB(capitec.queries.getApplicant, [{
				name: 'PersonID', value: firstPersonId
			}], function(err, results){
				expect(results[0].PersonID).toEqual(firstPersonId);
				firstApplicantId = results[0].Id;
			});	 		  	
		});
		it("should have created an applicant record for the first applicant", function() {
			capitec.queryDB(capitec.queries.getApplicant, [{
				name: 'PersonID', value: secondPersonId
			}], function(err, results){
				expect(results[0].PersonID).toEqual(secondPersonId);
				secondApplicantId = results[0].Id;
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

		it("should have linked the first applicant to the application", function() {
			capitec.queryDB(capitec.queries.getApplicantApplication, [
					{ name: 'ApplicationID', value: applicationId },
					{ name: 'ApplicantID', value: firstApplicantId
				}], function(err, applicant){
				expect(applicant.length).toEqual(1);
			});		  
		});
		it("should have linked the second applicant to the application", function() {
			capitec.queryDB(capitec.queries.getApplicantApplication, [
					{ name: 'ApplicationID', value: applicationId },
					{ name: 'ApplicantID', value: secondApplicantId
				}], function(err, applicant){
				expect(applicant.length).toEqual(1);
			});		  
		});

		it("should create an application with the correct application details", function(){
			capitec.queryDB(capitec.queries.getApplicationLoanDetail, [
					{ name: 'ApplicationID', value: applicationId }
				], function(err, applicationDetails){
				expect(applicationDetails[0].EmploymentType).toEqual("Salaried");
				expect(applicationDetails[0].OccupancyType).toEqual("Owner Occupied");
				expect(applicationDetails[0].HouseholdIncome).toEqual(30000);
				expect(applicationDetails[0].LoanAmount).toEqual(350000);
				expect(applicationDetails[0].LTV).toEqual(0.7);			
				applicationLoanDetailId = applicationDetails[0].id;
			});				
		});

		it("should create an employment record for the first applicant", function(){
			capitec.queryDB(capitec.queries.getApplicantEmployment, [{ 
				name: 'ApplicantID', value: firstApplicantId
			}], function(err, applicantEmployment){
				expect(applicantEmployment[0].Name).toEqual("Self Employed");
				expect(applicantEmployment[0].BasicIncome).toEqual(10000);	
			});		
		});	
		it("should create an employment record for the second applicant", function(){
			capitec.queryDB(capitec.queries.getApplicantEmployment, [{ 
				name: 'ApplicantID', value: secondApplicantId
			}], function(err, applicantEmployment){
				expect(applicantEmployment[0].Name).toEqual("Salaried");
				expect(applicantEmployment[0].BasicIncome).toEqual(20000);	
			});		
		});	
		it("should save the address supplied for the first applicant", function() {
			capitec.queryDB(capitec.queries.getApplicantAddress, [{ 
				name: 'ApplicantID', value: firstApplicantId
			}], function(err, address){
				expect(address[0].StreetNumber).toEqual(client1Address.streetNumber);
				expect(address[0].StreetName).toEqual(client1Address.streetName);
				expect(address[0].SuburbName).toEqual(client1Address.suburb);
				expect(address[0].CityName).toEqual(client1Address.city);
				expect(address[0].ProvinceName).toEqual(client1Address.province);
				expect(address[0].CountryName).toEqual("South Africa");						
			});												  
		});
		it("should save the address supplied for the second applicant", function() {
			capitec.queryDB(capitec.queries.getApplicantAddress, [{ 
				name: 'ApplicantID', value: secondApplicantId
			}], function(err, address){
				expect(address[0].StreetNumber).toEqual(client2Address.streetNumber);
				expect(address[0].StreetName).toEqual(client2Address.streetName);
				expect(address[0].SuburbName).toEqual(client2Address.suburb);
				expect(address[0].CityName).toEqual(client2Address.city);
				expect(address[0].ProvinceName).toEqual(client2Address.province);
				expect(address[0].CountryName).toEqual("South Africa");						
			});												  
		});
		it("should save the email address for the first applicant", function() {
			capitec.queryDB(capitec.queries.getEmailAddressContactDetail, [
				{ name: 'ApplicantID', value: firstApplicantId
			}], function(err, contactDetails){
				expect(contactDetails[0].emailAddress).toEqual(client1Details.contactDetails.emailAddress);
			});				  
		});
		it("should save the email address for the second applicant", function() {
			capitec.queryDB(capitec.queries.getEmailAddressContactDetail, [
				{ name: 'ApplicantID', value: secondApplicantId
			}], function(err, contactDetails){
				expect(contactDetails[0].emailAddress).toEqual(client2Details.contactDetails.emailAddress);
			});				  
		});
		it("should save the cellphone number for the first applicant", function() {
			capitec.queryDB(capitec.queries.getPhoneContactDetailForApplicant, [{ 
				name: 'ApplicantID', value: firstApplicantId },
				{ name: 'ContactDetailType', value: 'Mobile'
			}], function(err, contactDetails){
				expect(contactDetails[0].PhoneNumber).toEqual(client1Details.contactDetails.cellPhoneNumber);
			});				  
		}); 
		it("should save the cellphone number for the second applicant", function() {
			capitec.queryDB(capitec.queries.getPhoneContactDetailForApplicant, [{ 
				name: 'ApplicantID', value: secondApplicantId },
				{ name: 'ContactDetailType', value: 'Mobile'
			}], function(err, contactDetails){
				expect(contactDetails[0].PhoneNumber).toEqual(client2Details.contactDetails.cellPhoneNumber);
			});				  
		}); 

		it("should save the workphone number for the first applicant", function() {
			capitec.queryDB(capitec.queries.getPhoneContactDetailForApplicant, [{ 
				name: 'ApplicantID', value: firstApplicantId },
				{ name: 'ContactDetailType', value: 'Work'
			}], function(err, contactDetails){
				expect(contactDetails[0].PhoneNumber).toEqual(client1Details.contactDetails.workPhoneNumber);
			});				  
		});

		it("should save the workphone number for the second applicant", function() {
			capitec.queryDB(capitec.queries.getPhoneContactDetailForApplicant, [{ 
				name: 'ApplicantID', value: secondApplicantId },
				{ name: 'ContactDetailType', value: 'Work'
			}], function(err, contactDetails){
				expect(contactDetails[0].PhoneNumber).toEqual(client2Details.contactDetails.workPhoneNumber);
			});				  
		});

		it("should have recorded whether the first applicant is an income contributor", function() {
			capitec.queryDB(capitec.queries.getApplicantDeclarations, [{ 
				name: 'ApplicantID', value: firstApplicantId
			}], function(err, declarations){
				actualFirstApplicantDeclarations = declarations;
				var incomeContributor = actualFirstApplicantDeclarations.filterBy({
					name: 'DeclarationText',
					value: 'Income Contributor'
				});
				expect(incomeContributor.Answer).toEqual(firstApplicantDeclarations.isIncomeContributor);
			});							  
		});	

		it("should have recorded whether the first applicant is married in community of property", function() {
			var marriedInCOP = actualFirstApplicantDeclarations.filterBy({
				name: 'DeclarationText',
				value: 'Married In Community Of Property'
			});
			expect(marriedInCOP.Answer).toEqual(firstApplicantDeclarations.isMarriedInCOP);		  
		});

		it("should have recorded whether the first applicant's salary is paid into a Capitec transactional account", function() {
			var salaryPaidToCapitec = actualFirstApplicantDeclarations.filterBy({
				name: 'DeclarationText',
				value: 'Has Capitec Transaction Account'
			});
			expect(salaryPaidToCapitec.Answer).toEqual(firstApplicantDeclarations.isSalaryPaidToCapitec);					  
		});

		it("should have recorded whether the first applicant granted permission for the Credit Bureau Check", function() {
			var allowedToDoCreditBureauCheck = actualFirstApplicantDeclarations.filterBy({
				name: 'DeclarationText',
				value: 'Allow Credit Bureau Check'
			});
			expect(allowedToDoCreditBureauCheck.Answer).toEqual(firstApplicantDeclarations.isHappyWeDoITC);					  
		});

		it("should have recorded whether the second applicant is an income contributor", function() {
			capitec.queryDB(capitec.queries.getApplicantDeclarations, [{ 
				name: 'ApplicantID', value: secondApplicantId
			}], function(err, declarations){
				actualSecondApplicantDeclarations = declarations;
				var incomeContributor = actualSecondApplicantDeclarations.filterBy({
					name: 'DeclarationText',
					value: 'Income Contributor'
				});
				console.log('applicantid:',secondApplicantId);
				expect(incomeContributor.Answer).toEqual(secondApplicantDeclarations.isIncomeContributor);
			});							  
		});

		it("should have recorded whether the second applicant is married in community of property", function() {
			var marriedInCOP = actualSecondApplicantDeclarations.filterBy({
				name: 'DeclarationText',
				value: 'Married In Community Of Property'
			});
			expect(marriedInCOP.Answer).toEqual(firstApplicantDeclarations.isMarriedInCOP);		  
		});

		it("should have recorded whether the second applicant's salary is paid into a Capitec transactional account", function() {
			var salaryPaidToCapitec = actualSecondApplicantDeclarations.filterBy({
				name: 'DeclarationText',
				value: 'Has Capitec Transaction Account'
			});
			expect(salaryPaidToCapitec.Answer).toEqual(secondApplicantDeclarations.isSalaryPaidToCapitec);					  
		});

		it("should have recorded whether the second applicant granted permission for the Credit Bureau Check", function() {
			var allowedToDoCreditBureauCheck = actualSecondApplicantDeclarations.filterBy({
				name: 'DeclarationText',
				value: 'Allow Credit Bureau Check'
			});
			expect(allowedToDoCreditBureauCheck.Answer).toEqual(secondApplicantDeclarations.isHappyWeDoITC);					  
		});			

		it("should have stored the details specific to the new purchase application", function() {
			  capitec.queryDB(capitec.queries.getNewPurchaseApplicationLoanDetail, [
			  	{ name: 'ApplicationLoanDetailID', value: applicationLoanDetailId 
			  }], function(err, newPurchaseLoanDetails){
			  	expect(newPurchaseLoanDetails[0].Deposit).toEqual(150000);
			  	expect(newPurchaseLoanDetails[0].PurchasePrice).toEqual(500000);				  				  			
			  });
		});

		it("should have recorded that first applicant was selected as the main contact for the application", function() {
			capitec.queryDB(capitec.queries.getApplicant, [{
				name: 'PersonID', value: firstPersonId
			}], function(err, results){
				expect(results[0].MainContact).toBeTruthy();
			});	 
		});	

		it("should have recorded that second applicant was not selected as the main contact for the application", function() {
			capitec.queryDB(capitec.queries.getApplicant, [{
				name: 'PersonID', value: secondPersonId
			}], function(err, results){
				expect(results[0].MainContact).toBeFalsy();
			});	 
		});	
	});
});