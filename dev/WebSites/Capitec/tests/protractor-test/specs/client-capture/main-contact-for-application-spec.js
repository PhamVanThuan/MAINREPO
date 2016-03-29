describe("Main Contact for Application Specs ->", function() {

	var capitec = require('../../capitec.js');
	var AsyncSpec = require('jasmine-async')(jasmine);
	var landingPage;
	var applyPage;
	var newPurchasePage;
	var newPurchaseResultsPage;
	var applicationPreCheckPage;
	var clientCapturePage;
	var addressCapturePage;
	var employmentCapturePage;
	var declarationsCapturePage;
	var applyResultsPage;
	var FirstAvailableIDNumber;
	var SecondAvailableIDNumber;
	var applicationNumber;
	var firstPersonId;
	var secondPersonId;
	var firstApplicantId;
	var secondApplicantId
	var applicationId;
	var applicationLoanDetailId;
	var actualFirstApplicantDeclarations;
	var actualSecondApplicantDeclarations;
	var client1Address;
	var client2Address;
	var client1Details;
	var client2Details;
	var firstApplicantDeclarations;
	var secondApplicantDeclarations;
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
			landingPage = capitec.login.LoginAsRandomTestUser();
			applyPage = landingPage.goToApplyPage();
			applicationPreCheckPage = applyPage.goToNewHomeApplication();
			newPurchasePage = applicationPreCheckPage.proceedForNewPurchaseApplication(1);
			newPurchaseResultsPage = newPurchasePage.submitPopulatedForm(500000, 150000, 25000, "Owner Occupied", "Self Employed");
			clientCapturePage = newPurchaseResultsPage.apply();
			client1Details = {
				idNumber: FirstAvailableIDNumber,
				dateOfBirth: '19' + FirstAvailableIDNumber.substring(0,2) + '-' + FirstAvailableIDNumber.substring(2,2) + '-' + FirstAvailableIDNumber.substring(4,2),
				salutation: 'Mr',
				firstName: capitec.common.randomString(10),
				surname: 'Client1',
				contactDetails: { 
					emailAddress: capitec.common.randomString(10) + '@test.com',
					cellPhoneNumber: '031' + capitec.common.randomString(7, '1234567890'),
					workPhoneNumber: '072' + capitec.common.randomString(7, '1234567890'),
					},
				mainContact: false
				};		
			addressCapturePage = clientCapturePage.addClientDetails(client1Details, true);
			client1Address = {
				streetName: 'Address1 Street',
				streetNumber: capitec.common.randomString(5, '1234567890'),
				suburb: "Hillcrest",
				city: "Hillcrest",
				province:"Kwazulu-natal"
			};
			employmentCapturePage = addressCapturePage.captureAddress(client1Address, true);
			declarationsCapturePage = employmentCapturePage.captureEmployment("Self Employed", 10000);
			firstApplicantDeclarations = { isIncomeContributor: "Yes", isHappyWeDoITC: "Yes", isSalaryPaidToCapitec: "Yes", isMarriedInCOP: "No" };
			declarationsCapturePage.answerDeclarations(firstApplicantDeclarations);			
			navigate = false;			
		};
	});
  describe("when an application is submitted without a main contact", function() {
  	it("should inform the user that a main contact is required before the application can be submitted", function() {
  	  	declarationsCapturePage.printConsentForm();
  	  	capitec.notifications.checkIfValidationMessageExists("The application requires that one applicant must be selected as the main contact.");
  	});  	
  });
  describe("when trying to mark an applicant as a main applicant on an application with an existing main applicant", function() {
  	it("should not allow the user to check the main applicant checkbox", function() {
  		declarationsCapturePage.goBack();
  		employmentCapturePage.goBack();
  		addressCapturePage.goBack();
  		clientCapturePage.selectMainContact(true);
	    clientCapturePage.addAdditionalApplicant();
	    clientCapturePage = clientCapturePage.goToApplicant('2');
		client2Details = {
			idNumber: SecondAvailableIDNumber,
			dateOfBirth: '19' + SecondAvailableIDNumber.substring(0,2) + '-' + SecondAvailableIDNumber.substring(2,2) + '-' + SecondAvailableIDNumber.substring(4,2),
			salutation: 'Mr',
			firstName: capitec.common.randomString(10),
			surname: 'Client2',
			contactDetails: { 
				emailAddress: capitec.common.randomString(10) + '@test.com',
				cellPhoneNumber: '031' + capitec.common.randomString(7, '1234567890'),
				workPhoneNumber: '072' + capitec.common.randomString(7, '1234567890')
			},
			mainContact: true
		};
		clientCapturePage.addClientDetails(client2Details);
		expect(clientCapturePage.mainContact.isSelected()).toBeFalsy();
  	});
  	it("should display an information message to the user indicating that their is already a main contact for the application", function(){
		capitec.notifications.checkIfValidationMessageExists("Only one applicant can be selected as the main contact on this application.");
  	})	  
  });
  describe("when there is already a main contact an application", function() {
  	it("should allow the user to change the main contact to the second applicant by deselecting the first applicant as the main contact", function() {
  	  	clientCapturePage.goToApplicant("1");
  		clientCapturePage.selectMainContact(false);
  		clientCapturePage.goToApplicant("2");
  		clientCapturePage.selectMainContact(true);
  		clientCapturePage.goForward();
		client2Address = {
			streetName: 'Address2 Street',
			streetNumber: capitec.common.randomString(5, '1234567890'),
			suburb: "Hillcrest",
			city: "Hillcrest",
			province:"Kwazulu-natal"
		};
		employmentCapturePage = addressCapturePage.captureAddress(client2Address, true);
		declarationsCapturePage = employmentCapturePage.captureEmployment("Salaried", 20000);
		secondApplicantDeclarations = { isIncomeContributor: "Yes", isHappyWeDoITC: "Yes", isSalaryPaidToCapitec: "Yes", isMarriedInCOP: "No" };
		declarationsCapturePage.answerDeclarations(secondApplicantDeclarations);
		applicationSubmitPage = declarationsCapturePage.printConsentForm();
		idnumbers = [FirstAvailableIDNumber, SecondAvailableIDNumber];
		applicationSubmitPage.selectAllCheckBoxes(idnumbers);
		applyResultsPage = applicationSubmitPage.submitApplication();
		expect(browser.getCurrentUrl()).toContain(applyResultsPage.url);		
	});  	
  });
});