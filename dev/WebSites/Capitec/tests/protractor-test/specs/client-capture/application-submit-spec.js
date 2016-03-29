describe("Application submit Spec ->", function(){

	var capitec = require('../../capitec.js');
	var AsyncSpec = require('jasmine-async')(jasmine);
	var landingPage, applyPage, switchPage, switchPageResultsPage, applicationPreCheckPage, clientCapturePage;
	var addressCapturePage, employmentCapturePage, declarationsCapturePage, applicationSubmitPage, doNotNavigate;
	var idNumber1, idNumber2, applicantDeclarations, applyResultsPage, firstApplicant, secondApplicant;
	doNotNavigate = false;

	var async = new AsyncSpec(this);
		async.beforeEach(function(done){
		if (!idNumber1){
			var sql = capitec.queries.getNextIDNumber.toString();
			capitec.queryDB(sql, [], function(err, results){
				idNumber1 = results[0].IDNumber;
				idNumber2 = results[1].IDNumber;
				done();
			});		
		} else {
			done();
		}
	});

	beforeEach(function() {
		if(doNotNavigate)
			return;
		landingPage = capitec.login.LoginAsRandomTestUser();
		applyPage = landingPage.goToApplyPage();
		doNotNavigate = true;	
	});

	describe("when the application submit page is loaded with multiple applicants ->", function(){

		it("should allow the user to complete the process", function(){
			doNotNavigate = true;	
			firstApplicant = {
				idNumber: idNumber1,
				dateOfBirth: capitec.common.getDateOfBirthFromID(idNumber1),
				salutation: 'Mr',
				firstName: capitec.common.randomString(10),
				surname: 'Client1',
				contactDetails: { 
					emailAddress: capitec.common.randomString(10) + '@test.com',
					cellPhoneNumber: '031' + capitec.common.randomString(7, '1234567890'),
					workPhoneNumber: '072' + capitec.common.randomString(7, '1234567890'),
					},
				mainContact: true,
				address: {
					streetName: 'Address2 Street',
					streetNumber: capitec.common.randomString(5, '1234567890'),
					suburb: "Hillcrest",
					city: "Hillcrest",
					province:"Kwazulu-natal"
				},
				declarations: {
					isIncomeContributor: "Yes", 
					isHappyWeDoITC: "Yes", 
					isSalaryPaidToCapitec: "Yes", 
					isMarriedInCOP: "No"
				},
				employment: {
					type: "Salaried",
					grossIncome: 20000
				}
			};		
			secondApplicant = {
				idNumber: idNumber2,
				dateOfBirth: capitec.common.getDateOfBirthFromID(idNumber2),
				salutation: 'Mr',
				firstName: capitec.common.randomString(10),
				surname: 'Client2',
				contactDetails: { 
					emailAddress: capitec.common.randomString(10) + '@test.com',
					cellPhoneNumber: '031' + capitec.common.randomString(7, '1234567890'),
					workPhoneNumber: '072' + capitec.common.randomString(7, '1234567890')
				},
				mainContact: false,
				address: {
					streetName: 'Address1 Street',
					streetNumber: capitec.common.randomString(5, '1234567890'),
					suburb: "Hillcrest",
					city: "Hillcrest",
					province:"Kwazulu-natal"
				},
				declarations: { 
					isIncomeContributor: "Yes", 
					isHappyWeDoITC: "Yes", 
					isSalaryPaidToCapitec: "Yes", 
					isMarriedInCOP: "No"
				},
				employment: {
					type: "Self Employed",
					grossIncome: 10000
				}
			};
			applicationPreCheckPage = applyPage.goToNewHomeApplication();
			newPurchasePage = applicationPreCheckPage.proceedForNewPurchaseApplication(2);
			newPurchaseResultsPage = newPurchasePage.submitPopulatedForm(500000, 150000, 25000, "Owner Occupied", "Self Employed");
			clientCapturePage = newPurchaseResultsPage.apply();
			clientCapturePage.addClientDetails(firstApplicant);
			addressCapturePage = clientCapturePage.goToAddressPage();
			addressCapturePage.captureAddress(firstApplicant.address);
			employmentCapturePage = addressCapturePage.goToEmploymentPage();
			declarationsCapturePage = employmentCapturePage.captureEmployment(firstApplicant.employment.type, firstApplicant.employment.grossIncome);
			declarationsCapturePage.answerDeclarations(firstApplicant.declarations, true);
			clientCapturePage = declarationsCapturePage.goToApplicant('2');
			clientCapturePage.addClientDetails(secondApplicant);
			addressCapturePage = clientCapturePage.goToAddressPage();
			addressCapturePage.captureAddress(secondApplicant.address);
			employmentCapturePage = addressCapturePage.goToEmploymentPage();
			declarationsCapturePage = employmentCapturePage.captureEmployment(secondApplicant.employment.type, secondApplicant.employment.grossIncome);
			declarationsCapturePage.answerDeclarations(secondApplicant.declarations, false);
			applicationSubmitPage = declarationsCapturePage.printConsentForm();
		});

		it("should have the correct title", function(){
			expect(applicationSubmitPage.title()).toContain(applicationSubmitPage.titleText);		
		});

		it("should have the correct header", function(){
			expect(applicationSubmitPage.header()).toContain(applicationSubmitPage.headerText);			
		});

		it("should have a signed checkbox for the first applicant", function(){
			var checkbox = applicationSubmitPage.findCheckboxByIdNumber(firstApplicant.idNumber);
			expect(checkbox.isDisplayed()).toBeTruthy();		
		});
		it("should have a reprint button for the second applicant", function(){
			var reprintBtn = applicationSubmitPage.findReprintButtonByIdNumber(firstApplicant.idNumber);
			expect(reprintBtn.isDisplayed()).toBeTruthy();			
		});

		it("should have a signed checkbox for the first applicant", function(){
			var checkbox = applicationSubmitPage.findCheckboxByIdNumber(secondApplicant.idNumber);
			expect(checkbox.isDisplayed()).toBeTruthy();		
		});

		it("should have a reprint button for the second applicant", function(){
			var reprintBtn = applicationSubmitPage.findReprintButtonByIdNumber(secondApplicant.idNumber);
			expect(reprintBtn.isDisplayed()).toBeTruthy();			
		});

		it("should have an applicant description for the first applicant", function() {
		  	var applicantDescription = applicationSubmitPage.findApplicantDescriptionByIdNumber(firstApplicant.idNumber);
		  	expect(applicantDescription.getText()).toContain(firstApplicant.firstName);
		  	expect(applicantDescription.getText()).toContain(firstApplicant.surname);
		  	expect(applicantDescription.getText()).toContain(firstApplicant.idNumber);
		});

		it("should have an applicant description for the second applicant", function() {
		  	var applicantDescription = applicationSubmitPage.findApplicantDescriptionByIdNumber(secondApplicant.idNumber);
		  	expect(applicantDescription.getText()).toContain(secondApplicant.firstName);
		  	expect(applicantDescription.getText()).toContain(secondApplicant.surname);
		  	expect(applicantDescription.getText()).toContain(secondApplicant.idNumber);
		});

		it("should have back button", function(){
			expect(applicationSubmitPage.btnBack.isDisplayed()).toBeTruthy();		
		});

		it("should have a submit button", function(){
			expect(applicationSubmitPage.btnSubmit.isDisplayed()).toBeTruthy();
		});
		it("should have a disable submit button until the checkboxes have been selected", function() {
		  	expect(applicationSubmitPage.btnSubmit.isEnabled()).toBeFalsy();
		});

		it("should allow the user to complete the process", function() {
			usersIdnumbers = [ firstApplicant.idNumber, secondApplicant.idNumber ];
			applicationSubmitPage.selectAllCheckBoxes(usersIdnumbers);
			applyResultsPage = applicationSubmitPage.submitApplication()
			expect(browser.getCurrentUrl()).toContain(applyResultsPage.url);			  
		});
	});
});