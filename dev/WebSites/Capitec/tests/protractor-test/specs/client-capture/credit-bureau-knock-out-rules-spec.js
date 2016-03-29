describe("Credit Bureau Knock Out Rules ->", function() {
  
	var capitec = require('../../capitec.js');
	var AsyncSpec = require('jasmine-async')(jasmine);
	var landingPage, applyPage, newPurchasePage, newPurchaseResultsPage, applicationPreCheckPage;
	var applicationSubmitPage, clientCapturePage, addressCapturePage, employmentCapturePage;
	var declarationsCapturePage, applyResultsPage, doNotNavigate, nextAvailableIDNumber;
	var applicantDeclarations, clientAddress, applicantApplicationId;
    var firstApplicant, secondApplicant;	
    var applicationNumber, applicationId, firstPersonId, secondPersonId, firstApplicantId, firstApplicantId, applicationLoanDetailId, actualFirstApplicantDeclarations, actualSecondApplicantDeclarations;
    var doNotNavigate = false;
    var applicantAddress = {
			streetName: "Rock Road",
			streetNumber: "99",
			suburb: "Hillcrest",
			city: "Hillcrest",
			province:"Kwazulu-natal"
		};
	var Cleanup = function(){		
			doNotNavigate = false;
			firstApplicantIdNumber = null;
			secondApplicantIdNumber = null;
			firstApplicantSurname = null;
			secondApplicantSurname = null
	}

	landingPage = capitec.login.LoginAsRandomTestUser();

	var Navigate = function(firstApplicantIdNumber, secondApplicantIdNumber, firstApplicantSurname, secondApplicantSurname){
		var idnumbers;
		firstApplicant = {
			idNumber: firstApplicantIdNumber,
			salutation: 'Mr',
			firstName: 'firstApplicant ' + capitec.common.randomString(5),
			surname: firstApplicantSurname,
			contactDetails: { 
				emailAddress: capitec.common.randomString(10) + '@test.com',
				cellPhoneNumber: '031' + capitec.common.randomString(7, '1234567890'),
				workPhoneNumber: '072' + capitec.common.randomString(7, '1234567890'),
				},
			mainContact: true
		};		
		applyPage = landingPage.goToApplyPage();	
		applicationPreCheckPage = applyPage.goToNewHomeApplication();
		if(secondApplicantIdNumber)
			newPurchasePage = applicationPreCheckPage.proceedForNewPurchaseApplication(2);
		else
			newPurchasePage = applicationPreCheckPage.proceedForNewPurchaseApplication(1);
		newPurchaseResultsPage = newPurchasePage.submitPopulatedForm(500000, 150000, 25000, "Owner Occupied", "Self Employed");
		clientCapturePage = newPurchaseResultsPage.apply();	
		clientCapturePage.addClientDetails(firstApplicant, false);
		addressCapturePage = clientCapturePage.Next();
		employmentCapturePage = addressCapturePage.fillForm();
		declarationsCapturePage = employmentCapturePage.captureEmployment("Self Employed", 25000);
		applicantDeclarations = { 
			isIncomeContributor: 'Yes', 
			isHappyWeDoITC: 'Yes', 
			isSalaryPaidToCapitec: 'Yes', 
			isMarriedInCOP: 'No'
		};
		declarationsCapturePage.answerDeclarations(applicantDeclarations);
		if(secondApplicantIdNumber) {
			secondApplicant = {
			idNumber: secondApplicantIdNumber,
			salutation: 'Mr',
			firstName: 'secondApplicant ' + capitec.common.randomString(5),
			surname: secondApplicantSurname,
			contactDetails: { 
				emailAddress: capitec.common.randomString(10) + '@test.com',
				cellPhoneNumber: '031' + capitec.common.randomString(7, '1234567890'),
				workPhoneNumber: '072' + capitec.common.randomString(7, '1234567890'),
				},
			mainContact: false
			};		
			declarationsCapturePage.printConsentForm();
			clientCapturePage.addClientDetails(secondApplicant, false);
			addressCapturePage = clientCapturePage.Next();
			employmentCapturePage = addressCapturePage.selectPreviousAddress();
			declarationsCapturePage = employmentCapturePage.captureEmployment("Self Employed", 25000);
			applicationSubmitPage = declarationsCapturePage.fillForm(true);
			idnumbers = [firstApplicantIdNumber, secondApplicantIdNumber];
			applicationSubmitPage.selectAllCheckBoxes(idnumbers);
			applyResultsPage = applicationSubmitPage.submitApplication()
		}
		else {
			applicationSubmitPage = declarationsCapturePage.printConsentForm()
			idnumbers = [firstApplicantIdNumber];
			applicationSubmitPage.selectAllCheckBoxes(idnumbers);
			applyResultsPage = applicationSubmitPage.submitApplication()
		}
	};

	var GetId = function(resultIndex, callback){
		var sql = capitec.queries.getNextIDNumber.toString();
		capitec.queryDB(sql, [], function(err, results){
			callback(results[resultIndex].IDNumber);
		});	
	};

	var GetIdsAndNavigate = function(firstSurname, secondSurname, callback){
		if(!doNotNavigate) {
			doNotNavigate = true;	
			GetId(0, function(firstID) {
				if(secondSurname) {
					GetId(1, function(secondID){	
						Navigate(firstID, secondID, firstSurname, secondSurname);
						callback();
					});
				}
				else {
					Navigate(firstID, undefined, firstSurname, undefined);
					callback();
				};				
			});
		}
		else {
			callback();
		};
	};

	xdescribe("application fails ->", function() {
		var async = new AsyncSpec(this);
		async.beforeEach(function(done){
			GetIdsAndNavigate("Empirica_575", undefined, function(){
				done();
			});
		});
		it("should inform the applicant that the application does not qualify", function() {	
			expect(applyResultsPage.title()).toContain(applyResultsPage.titleText.rejected);
		});
		it("should display application declined message", function(){
			applyResultsPage.assertAppDeclinedMessageIsDisplayed();
		});
		it("should display application result messages", function(){
			applyResultsPage.applicationResultMessages(function(messages){
				expect(messages).toContain("The best Credit Bureau score applicable for this application is below the minimum requirement for self-employed applicants.");
			});
		});
		it("should not display warning that first applicant failed itc check", function() {
			applyResultsPage.isApplicantITCCheckFailedDisplayed(0, function(result){				
				expect(result).toBeFalsy();
			});
		});
	});

	xdescribe("single applicant fails itc check ->", function() {
		var async = new AsyncSpec(this);
		async.beforeEach(function(done){
			GetIdsAndNavigate("DebtCounselling", undefined, function(){
				done();
			});
		});
		it("should inform the applicant that the application does not qualify", function() {	
			expect(applyResultsPage.title()).toContain(applyResultsPage.titleText.rejected);
		});
		it("should display application declined message", function(){
			applyResultsPage.assertAppDeclinedMessageIsDisplayed();
		});
		it("should display warning that first applicant failed itc check", function() {
			applyResultsPage.isApplicantITCCheckFailedDisplayed(0, function(result){				
				expect(result).toBeTruthy();
			});
		});
		it("should display warning messages header", function(){			
			applyResultsPage.firstApplicantMessagesHeader(function(element){
				expect(element.getText()).toContain("Credit Bureau Decline Reasons for " + firstApplicant.firstName + " " + firstApplicant.surname)
			});
		});
		it("should display warning messages", function() {
			applyResultsPage.firstApplicantMessages(function(messages){
				expect(messages).toContain('There is a record of Debt Counselling.');
			});
			Cleanup();
		});
	});

	xdescribe("first applicant fails itc check and second applicant has warnings ->", function() {
		var async = new AsyncSpec(this);
		async.beforeEach(function(done){
			GetIdsAndNavigate("DebtCounselling", "Defaults5In2Yrs", function(){
				done();
			});
		});
		it("should inform the applicant that the application does not qualify", function() {	
			expect(applyResultsPage.title()).toContain(applyResultsPage.titleText.rejected);
		});
		it("should display application declined message", function(){
			applyResultsPage.assertAppDeclinedMessageIsDisplayed();
		});
		it("should display first applicant warning messages header", function(){			
			applyResultsPage.firstApplicantMessagesHeader(function(element){
				expect(element.getText()).toContain("Credit Bureau Decline Reasons for " + firstApplicant.firstName + " " + firstApplicant.surname)
			});
		});
		it("should display warning that first applicant failed itc check", function() {
			applyResultsPage.isApplicantITCCheckFailedDisplayed(0, function(result){				
				expect(result).toBeTruthy();
			});
		});
		it("should display warning messages for first applicant", function() {
			applyResultsPage.firstApplicantMessages(function(messages){
				expect(messages).toContain('There is a record of Debt Counselling.');
			});
		});
		it("should display second applicant warning messages header", function(){			
			applyResultsPage.secondApplicantMessagesHeader(function(element){
				expect(element.getText()).toContain("Credit Bureau Warnings for " + secondApplicant.firstName + " " + secondApplicant.surname)
			});
		});
		it("should not display warning that second applicant failed itc check", function() {
			applyResultsPage.isApplicantITCCheckFailedDisplayed(1, function(result){				
				expect(result).toBeFalsy();
			});
		});
		it("should display warning messages for second applicant", function() {
			applyResultsPage.secondApplicantMessages(function(messages){
				expect(messages).toContain('There is record of numerous unsettled defaults within the past 2 years.');
			});
			Cleanup();
		});
	});

	xdescribe("first applicant passed itc check and second applicant failed itc check ->", function() {
		var async = new AsyncSpec(this);
		async.beforeEach(function(done){
			GetIdsAndNavigate("Empirica_601", "DebtCounselling", function(){
				done();
			});
		});
		it("should inform the applicant that the application does not qualify", function() {	
			expect(applyResultsPage.title()).toContain(applyResultsPage.titleText.rejected);
		});
		it("should display application declined message", function(){
			applyResultsPage.assertAppDeclinedMessageIsDisplayed();
		});
		it("should not display warning that first applicant failed itc check", function() {
			applyResultsPage.isApplicantITCCheckFailedDisplayed(0, function(result){				
				expect(result).toBeFalsy();
			});
		});
		it("should not display warning messages for first applicant", function() {
			applyResultsPage.firstApplicantMessages(function(messages){
				expect(messages).toEqual([]);
			});
		});
		it("should display second applicant warning messages header", function(){			
			applyResultsPage.secondApplicantMessagesHeader(function(element){
				expect(element.getText()).toContain("Credit Bureau Decline Reasons for " + secondApplicant.firstName + " " + secondApplicant.surname)
			});
		});
		it("should display warning that second applicant failed itc check", function() {
			applyResultsPage.isApplicantITCCheckFailedDisplayed(1, function(result){				
				expect(result).toBeTruthy();
			});
		});
		it("should display warning messages for second applicant", function() {
			applyResultsPage.secondApplicantMessages(function(messages){
				expect(messages).toContain('There is a record of Debt Counselling.');
			});
			Cleanup();
		});
	});

	xdescribe("first applicant has warnings and second applicant failed itc check ->", function() {
		var async = new AsyncSpec(this);
		async.beforeEach(function(done){
			GetIdsAndNavigate("Judgement5_Random", "DebtCounselling", function(){
				done();
			});
		});
		it("should inform the applicant that the application does not qualify", function() {	
			expect(applyResultsPage.title()).toContain(applyResultsPage.titleText.rejected);
		});
		it("should display application declined message", function(){
			applyResultsPage.assertAppDeclinedMessageIsDisplayed();
		});
		it("should display first applicant warning messages header", function(){			
			applyResultsPage.firstApplicantMessagesHeader(function(element){
				expect(element.getText()).toContain("Credit Bureau Warnings for " + firstApplicant.firstName + " " + firstApplicant.surname)
			});
		});
		it("should not display warning that first applicant failed itc check", function() {
			applyResultsPage.isApplicantITCCheckFailedDisplayed(0, function(result){				
				expect(result).toBeFalsy();
			});
		});
		it("should display warning messages for first applicant", function() {
			applyResultsPage.firstApplicantMessages(function(messages){
				expect(messages).toContain('There is record of multiple recent unpaid judgements in the last 3 years.');
				expect(messages).toContain('There is record of unpaid judgements with a material aggregated rand value.');
				expect(messages).toContain('There is record of an outstanding aggregated unpaid judgement of material value.');
			});
		});
		it("should display second applicant warning messages header", function(){			
			applyResultsPage.secondApplicantMessagesHeader(function(element){
				expect(element.getText()).toContain("Credit Bureau Decline Reasons for " + secondApplicant.firstName + " " + secondApplicant.surname)
			});
		});
		it("should display warning that second applicant failed itc check", function() {
			applyResultsPage.isApplicantITCCheckFailedDisplayed(1, function(result){				
				expect(result).toBeTruthy();
			});
		});
		it("should display warning messages for second applicant", function() {
			applyResultsPage.secondApplicantMessages(function(messages){
				expect(messages).toContain('There is a record of Debt Counselling.');
			});
			Cleanup();
		});
	});

	xdescribe("first applicant has warnings and second applicant has warnings ->", function() {
		var async = new AsyncSpec(this);
		async.beforeEach(function(done){
			GetIdsAndNavigate("Judgement5_Random", "Defaults5In2Yrs", function(){
				done();
			});
		});
		it("should inform the applicant that the application has been submitted", function() {	
			expect(applyResultsPage.title()).toContain(applyResultsPage.titleText.accepted);
		});
		it("should display application qualifies message", function(){
			applyResultsPage.assertAppQualifiesMessageIsDisplayed();
		});
		it("should display first applicant warning messages header", function(){			
			applyResultsPage.firstApplicantMessagesHeader(function(element){
				expect(element.getText()).toContain("Credit Bureau Warnings for " + firstApplicant.firstName + " " + firstApplicant.surname)
			});
		});
		it("should not display warning that first applicant failed itc check", function() {
			applyResultsPage.isApplicantITCCheckFailedDisplayed(0, function(result){				
				expect(result).toBeFalsy();
			});
		});
		it("should display warning messages for first applicant", function() {
			applyResultsPage.firstApplicantMessages(function(messages){
				expect(messages).toContain('There is record of multiple recent unpaid judgements in the last 3 years.');
				expect(messages).toContain('There is record of unpaid judgements with a material aggregated rand value.');
				expect(messages).toContain('There is record of an outstanding aggregated unpaid judgement of material value.');
			});
		});
		it("should display second applicant warning messages header", function(){			
			applyResultsPage.secondApplicantMessagesHeader(function(element){
				expect(element.getText()).toContain("Credit Bureau Warnings for " + secondApplicant.firstName + " " + secondApplicant.surname)
			});
		});
		it("should not display warning that second applicant failed itc check", function() {
			applyResultsPage.isApplicantITCCheckFailedDisplayed(0, function(result){				
				expect(result).toBeFalsy();
			});
		});
		it("should display warning messages for second applicant", function() {
			applyResultsPage.secondApplicantMessages(function(messages){
				expect(messages).toContain('There is record of numerous unsettled defaults within the past 2 years.');
			});
			Cleanup();
		});
	});

	describe("first applicant fails itc check and second applicant passes itc check ->", function() {
		var async = new AsyncSpec(this);
		async.beforeEach(function(done){
			GetIdsAndNavigate("DebtCounselling", "Empirica_601", function(){
				done();
			});
		});
		it("should inform the applicant that the application does not qualify", function() {	
			expect(applyResultsPage.title()).toContain(applyResultsPage.titleText.rejected);
		});
		it("should display application declined message", function(){
			applyResultsPage.assertAppDeclinedMessageIsDisplayed();
		});
		it("should display first applicant warning messages header", function(){			
			applyResultsPage.firstApplicantMessagesHeader(function(element){
				expect(element.getText()).toContain("Credit Bureau Decline Reasons for " + firstApplicant.firstName + " " + firstApplicant.surname)
			});
		});
		it("should display warning that first applicant failed itc check", function() {
			applyResultsPage.isApplicantITCCheckFailedDisplayed(0, function(result){				
				expect(result).toBeTruthy();
			});
		});
		it("should display warning messages", function() {
			applyResultsPage.firstApplicantMessages(function(messages){
				expect(messages).toContain('There is a record of Debt Counselling.');
			});
		});
		it("should not display warning that second applicant failed itc check", function() {
			applyResultsPage.isApplicantITCCheckFailedDisplayed(1, function(result){				
				expect(result).toBeFalsy();
			});
		});
		it("should not display warning messages for second applicant", function() {
			applyResultsPage.secondApplicantMessages(function(messages){
				expect(messages).toEqual([]);
			});
		});
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
				name: 'IdentityNumber', value: firstApplicant.idNumber
			}], function(err, results){
				expect(results[0].IdentityNumber).toEqual(firstApplicant.idNumber);
				firstPersonId = results[0].Id;
			});	  
		});		
		it("should have created a person record for second applicant", function() {
			capitec.queryDB(capitec.queries.getPerson, [{
				name: 'IdentityNumber', value: secondApplicant.idNumber
			}], function(err, results){
				expect(results[0].IdentityNumber).toEqual(secondApplicant.idNumber);
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
				expect(applicationDetails[0].EmploymentType).toEqual("Self Employed");
				expect(applicationDetails[0].OccupancyType).toEqual("Owner Occupied");
				expect(applicationDetails[0].HouseholdIncome).toEqual(50000);
				expect(applicationDetails[0].LoanAmount).toEqual(350000);
				expect(applicationDetails[0].LTV).toEqual(0.7);			
				applicationLoanDetailId = applicationDetails[0].id;
			});				
		});

		it("should create an employment record for the first applicant", function(){
			capitec.queryDB(capitec.queries.getApplicantEmployment, [{ 
				name: 'ApplicantID', value: firstApplicantId
			}], function(err, applicantEmployment){
				expect(applicantEmployment.length).toEqual(1);
			});		
		});	
		it("should create an employment record for the second applicant", function(){
			capitec.queryDB(capitec.queries.getApplicantEmployment, [{ 
				name: 'ApplicantID', value: secondApplicantId
			}], function(err, applicantEmployment){
				expect(applicantEmployment.length).toEqual(1);
			});		
		});	
		it("should save the address supplied for the first applicant", function() {
			capitec.queryDB(capitec.queries.getApplicantAddress, [{ 
				name: 'ApplicantID', value: firstApplicantId
			}], function(err, address){
				expect(address[0].StreetNumber).toEqual(applicantAddress.streetNumber);
				expect(address[0].StreetName).toEqual(applicantAddress.streetName);
				expect(address[0].SuburbName).toEqual(applicantAddress.suburb);
				expect(address[0].CityName).toEqual(applicantAddress.city);
				expect(address[0].ProvinceName).toEqual(applicantAddress.province);
				expect(address[0].CountryName).toEqual("South Africa");						
			});												  
		});
		it("should save the address supplied for the second applicant", function() {
			capitec.queryDB(capitec.queries.getApplicantAddress, [{ 
				name: 'ApplicantID', value: secondApplicantId
			}], function(err, address){
				expect(address[0].StreetNumber).toEqual(applicantAddress.streetNumber);
				expect(address[0].StreetName).toEqual(applicantAddress.streetName);
				expect(address[0].SuburbName).toEqual(applicantAddress.suburb);
				expect(address[0].CityName).toEqual(applicantAddress.city);
				expect(address[0].ProvinceName).toEqual(applicantAddress.province);
				expect(address[0].CountryName).toEqual("South Africa");						
			});												  
		});
		it("should save the email address for the first applicant", function() {
			capitec.queryDB(capitec.queries.getEmailAddressContactDetail, [
				{ name: 'ApplicantID', value: firstApplicantId
			}], function(err, contactDetails){
				expect(contactDetails[0].emailAddress).toEqual(firstApplicant.contactDetails.emailAddress);
			});				  
		});
		it("should save the email address for the second applicant", function() {
			capitec.queryDB(capitec.queries.getEmailAddressContactDetail, [
				{ name: 'ApplicantID', value: secondApplicantId
			}], function(err, contactDetails){
				expect(contactDetails[0].emailAddress).toEqual(secondApplicant.contactDetails.emailAddress);
			});				  
		});
		it("should save the cellphone number for the first applicant", function() {
			capitec.queryDB(capitec.queries.getPhoneContactDetailForApplicant, [{ 
				name: 'ApplicantID', value: firstApplicantId },
				{ name: 'ContactDetailType', value: 'Mobile'
			}], function(err, contactDetails){
				expect(contactDetails[0].PhoneNumber).toEqual(firstApplicant.contactDetails.cellPhoneNumber);
			});				  
		}); 
		it("should save the cellphone number for the second applicant", function() {
			capitec.queryDB(capitec.queries.getPhoneContactDetailForApplicant, [{ 
				name: 'ApplicantID', value: secondApplicantId },
				{ name: 'ContactDetailType', value: 'Mobile'
			}], function(err, contactDetails){
				expect(contactDetails[0].PhoneNumber).toEqual(secondApplicant.contactDetails.cellPhoneNumber);
			});				  
		}); 

		it("should save the workphone number for the first applicant", function() {
			capitec.queryDB(capitec.queries.getPhoneContactDetailForApplicant, [{ 
				name: 'ApplicantID', value: firstApplicantId },
				{ name: 'ContactDetailType', value: 'Work'
			}], function(err, contactDetails){
				expect(contactDetails[0].PhoneNumber).toEqual(firstApplicant.contactDetails.workPhoneNumber);
			});				  
		});

		it("should save the workphone number for the second applicant", function() {
			capitec.queryDB(capitec.queries.getPhoneContactDetailForApplicant, [{ 
				name: 'ApplicantID', value: secondApplicantId },
				{ name: 'ContactDetailType', value: 'Work'
			}], function(err, contactDetails){
				expect(contactDetails[0].PhoneNumber).toEqual(secondApplicant.contactDetails.workPhoneNumber);
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
				expect(incomeContributor.Answer).toEqual(applicantDeclarations.isIncomeContributor);
			});							  
		});

		it("should have recorded whether the first applicant is married in community of property", function() {
			var marriedInCOP = actualFirstApplicantDeclarations.filterBy({
				name: 'DeclarationText',
				value: 'Married In Community Of Property'
			});
			expect(marriedInCOP.Answer).toEqual(applicantDeclarations.isMarriedInCOP);		  
		});

		it("should have recorded whether the first applicant's salary is paid into a Capitec transactional account", function() {
			var salaryPaidToCapitec = actualFirstApplicantDeclarations.filterBy({
				name: 'DeclarationText',
				value: 'Has Capitec Transaction Account'
			});
			expect(salaryPaidToCapitec.Answer).toEqual(applicantDeclarations.isSalaryPaidToCapitec);					  
		});

		it("should have recorded whether the first applicant granted permission for the Credit Bureau Check", function() {
			var allowedToDoCreditBureauCheck = actualFirstApplicantDeclarations.filterBy({
				name: 'DeclarationText',
				value: 'Allow Credit Bureau Check'
			});
			expect(allowedToDoCreditBureauCheck.Answer).toEqual(applicantDeclarations.isHappyWeDoITC);					  
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
				expect(incomeContributor.Answer).toEqual(applicantDeclarations.isIncomeContributor);
			});							  
		});

		it("should have recorded whether the second applicant is married in community of property", function() {
			var marriedInCOP = actualSecondApplicantDeclarations.filterBy({
				name: 'DeclarationText',
				value: 'Married In Community Of Property'
			});
			expect(marriedInCOP.Answer).toEqual(applicantDeclarations.isMarriedInCOP);		  
		});

		it("should have recorded whether the second applicant's salary is paid into a Capitec transactional account", function() {
			var salaryPaidToCapitec = actualSecondApplicantDeclarations.filterBy({
				name: 'DeclarationText',
				value: 'Has Capitec Transaction Account'
			});
			expect(salaryPaidToCapitec.Answer).toEqual(applicantDeclarations.isSalaryPaidToCapitec);					  
		});

		it("should have recorded whether the second applicant granted permission for the Credit Bureau Check", function() {
			var allowedToDoCreditBureauCheck = actualSecondApplicantDeclarations.filterBy({
				name: 'DeclarationText',
				value: 'Allow Credit Bureau Check'
			});
			expect(allowedToDoCreditBureauCheck.Answer).toEqual(applicantDeclarations.isHappyWeDoITC);					  
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