describe("Client Capture Process Specs ->", function(){
	var capitec = require('../../capitec.js');
	var AsyncSpec = require('jasmine-async')(jasmine);
	var landingPage;
	var applyPage;
	var switchPage;
	var switchPageResultsPage;
	var applicationPreCheckPage;
	var clientCapturePage;
	var addressCapturePage;
	var employmentCapturePage;
	var declarationsCapturePage;
	var applicationSubmitPage;
	var doNotNavigate;
	var nextAvailableIDNumbers;

	beforeEach(function() {
		if(doNotNavigate)
			return;
		landingPage = capitec.login.LoginAsRandomTestUser();
		applyPage = landingPage.goToApplyPage();		
	});

	describe("when submitting an application with application details that are outside of the SAHL credit criteria", function(){
		var async = new AsyncSpec(this);
		async.beforeEach(function(done){
			if (!nextAvailableIDNumbers){
				var sql = capitec.queries.getNextIDNumber.toString();
				capitec.queryDB(sql, [], function(err, results){
					nextAvailableIDNumbers = results;
					done();
				});		
			} else {
				done();
			}
		});
		it("should warn the user that the application is now falls outside of SAHL credit criteria", function(){
			doNotNavigate = true;
			applicationPreCheckPage = applyPage.goToSwitchApplication();
			switchPage = applicationPreCheckPage.proceedForSwitchApplication(1);
			switchPageResultsPage = switchPage.submitPopulatedForm(500000, 200000, 200000, 15000, "Owner Occupied", "Salaried");
			clientCapturePage = switchPageResultsPage.apply();
			var clientDetails =	{ 
				idNumber: nextAvailableIDNumbers[0].IDNumber, 
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
			addressCapturePage = clientCapturePage.addClientDetails(clientDetails, true);
			employmentCapturePage = addressCapturePage.fillForm();
			//now we add an applicant with income that doesnt qualify
			declarationsCapturePage = employmentCapturePage.captureEmployment("Salaried", 8500);
			applicationSubmitPage = declarationsCapturePage.fillForm(true);
			usersIdnumbers = [ nextAvailableIDNumbers[0].IDNumber ];
			applicationSubmitPage.selectAllCheckBoxes(usersIdnumbers);
			applyResultsPage = applicationSubmitPage.submitApplication();
			expect(browser.getCurrentUrl()).toContain(applyResultsPage.url);
			applyResultsPage.applicationResultMessages(function(messages){
				expect(messages).toContain('Your repayment as a percentage of household income (PTI) would be 46.9% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R290666 or alternatively additional income so that total income is at least R12084.');
			}); 
		});
	});
});