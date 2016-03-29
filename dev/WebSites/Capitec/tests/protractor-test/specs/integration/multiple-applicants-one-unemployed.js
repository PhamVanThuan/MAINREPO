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
			newPurchaseResultsPage = newPurchasePage.submitPopulatedForm(500000, 150000, 30000, "Owner Occupied", "Self Employed");
			clientCapturePage = newPurchaseResultsPage.apply();
			addressCapturePage = clientCapturePage.addClientDetails(client1Details, true);
			employmentCapturePage = addressCapturePage.captureAddress(client1Address, true);
			declarationsCapturePage = employmentCapturePage.captureEmployment("Self Employed", 30000);
			firstApplicantDeclarations = { isIncomeContributor: "Yes", isHappyWeDoITC: "Yes", isSalaryPaidToCapitec: "Yes", isMarriedInCOP: "Yes" };
			declarationsCapturePage.answerDeclarations(firstApplicantDeclarations);
			clientCapturePage = declarationsCapturePage.goToApplicant('2');
			addressCapturePage = clientCapturePage.addClientDetails(client2Details, true);
			employmentCapturePage = addressCapturePage.captureAddress(client2Address, true);
			declarationsCapturePage = employmentCapturePage.captureEmployment("Unemployed", 0);
			secondApplicantDeclarations = { isIncomeContributor: "No", isHappyWeDoITC: "Yes", isSalaryPaidToCapitec: "Yes", isMarriedInCOP: "No" };
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