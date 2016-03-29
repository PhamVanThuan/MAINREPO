describe("client capture specifications ->", function(){
	
	var capitec = require('../../capitec.js');
	var AsyncSpec = require('jasmine-async')(jasmine);
	var landingPage = capitec.login.LoginAsRandomTestUser();
	var applyPage = landingPage.goToApplyPage();
	var applicationPreCheckPage = applyPage.goToSwitchApplication();
	var switchPage = applicationPreCheckPage.proceedForSwitchApplication(1);
	var switchResultsPage = switchPage.fillForm();
	var clientCapturePage = switchResultsPage.apply();
	var addressCapturePage;
	var employmentCapturePage;
	var declarationsPage;
	var nextAvailableIDNumber;

	describe("when the client capture screen is loaded ->", function(){
		it("should navigate the correct url", function(){
			expect(browser.getCurrentUrl()).toContain(clientCapturePage.url);
		});
		it("should show the identity number field", function(){
			expect(clientCapturePage.identityNumber.isDisplayed()).toBeTruthy();		
		});
		it("should show the surname field", function(){
			expect(clientCapturePage.surname.isDisplayed()).toBeTruthy();				
		});
		it("should show the first name field", function(){
			expect(clientCapturePage.firstname.isDisplayed()).toBeTruthy();		
		});
		it("should show the work phone number field", function(){
			expect(clientCapturePage.workPhoneNumber.isDisplayed()).toBeTruthy();		
		});
		it("should show the cell phone number field", function(){
			expect(clientCapturePage.cellPhoneNumber.isDisplayed()).toBeTruthy();			
		});
		it("should show the salutation field", function(){
			expect(clientCapturePage.salutationSpan.isDisplayed()).toBeTruthy();		
		});
		it("should show the email address field", function(){
			expect(clientCapturePage.emailAddress.isDisplayed()).toBeTruthy();		
		});
		it("should show the date of birth picker", function(){
			for(datepart in clientCapturePage.dateOfBirth){
				expect(clientCapturePage.dateOfBirth[datepart].isDisplayed).toBeTruthy();
			};
		});
		it("should display the correct page header", function(){
			expect(clientCapturePage.header()).toContain(clientCapturePage.headerText);
		});
		it("should display the correct page title", function(){
			expect(clientCapturePage.title()).toContain(clientCapturePage.titleText);
		});
		it("should contain the correct list of salutations", function(){
			var expectedSalutations = [ "Mr", "Mrs", "Prof", "Dr", "Past", "Capt", "Sir", "Miss", "Ms", "Lord", "Rev", "Advocate"];
			capitec.common.checkAllSelectOptionsExist(clientCapturePage.salutations, expectedSalutations);	
		});
	});

	describe("when clicking the next button without capturing any details ->", function(){
		it("should display identity number mandatory validation messages to the user", function(){
			clientCapturePage.goForward();
			expect(capitec.notifications.validationMessages()).toContain("Please provide a valid South African ID Number");
		});
		it("should display first name mandatory validation messages to the user", function(){
			expect(capitec.notifications.validationMessages()).toContain("First Name is required");
		});
		it("should display surname mandatory validation messages to the user", function(){
			expect(capitec.notifications.validationMessages()).toContain("Surname is required");
		});
		it("should not display the email address mandatory validation messages to the user", function(){
			expect(capitec.notifications.validationMessages()).not.toContain("Please provide a valid email address");
		});
		it("should display celphone number mandatory validation messages to the user", function(){
			expect(capitec.notifications.validationMessages()).toContain("Please provide a valid cellphone number.");
		});
		it("should display title mandatory validation messages to the user", function(){
			expect(capitec.notifications.validationMessages()).toContain("A Title is required");
		});
		it("should display date of birth mandatory validation messages to the user", function(){
			expect(capitec.notifications.validationMessages()).toContain("Please provide a valid date of Birth");
		});
		it("should stay on the client capture page", function(){
			expect(browser.getCurrentUrl()).toContain(clientCapturePage.url);
		});
	});

	describe("when capturing partial date of birth and clicking next", function() {		
		it("should display date of birth mandatory validation messages", function(){
			capitec.common.selectOption(clientCapturePage.dateOfBirth.day, '01');
			clientCapturePage.goForward();
			expect(capitec.notifications.validationMessages()).toContain("Please provide a valid date of Birth");
		});
	});

	describe("when validating the work phone number field", function(){
		it("should not allow a number less than 10 digits", function() {
			clientCapturePage.setWorkPhoneNumberTo('031123');
			clientCapturePage.goForward();
			expect(capitec.notifications.validationMessages()).toContain("Please provide a valid work phone number.");		  
		});
		it("should not allow any alphabetic characters", function() {
			clientCapturePage.setWorkPhoneNumberTo('031aaabbbccc1');
			clientCapturePage.goForward();
			expect(capitec.notifications.validationMessages()).toContain("Please provide a valid work phone number.");				  
		});
		it("should allow a number 10 digits in length", function() {
			clientCapturePage.setWorkPhoneNumberTo('0315653650');
			clientCapturePage.goForward();
			expect(capitec.notifications.validationMessages()).not.toContain("Please provide a valid work phone number.");				  
		});
		it("should allow the user to leave the field empty", function() {
			clientCapturePage.setWorkPhoneNumberTo('');
			clientCapturePage.goForward();
			expect(capitec.notifications.validationMessages()).not.toContain("Please provide a valid work phone number.");				  
		});		
		it("should not count spaces when determining the length of the work phone number", function() {
			  clientCapturePage.setWorkPhoneNumberTo('031 565365');
			  clientCapturePage.goForward();
			  expect(capitec.notifications.validationMessages()).toContain("Please provide a valid work phone number.");	
		});
		it("should not allow a 10 digit number that includes spaces", function() {
			  clientCapturePage.setWorkPhoneNumberTo('031 5653650');
			  clientCapturePage.goForward();
			  expect(capitec.notifications.validationMessages()).toContain("Please provide a valid work phone number.");	
		});
	});

	describe("when validating the cell phone number field", function(){
		it("should not allow a number less than 10 digits", function() {
			clientCapturePage.setCellPhoneNumberTo('082123');
			clientCapturePage.goForward();
			expect(capitec.notifications.validationMessages()).toContain('Please provide a valid cellphone number.');		  
		});
		it("should not allow any alphabetic characters", function() {
			clientCapturePage.setCellPhoneNumberTo('082aaabbbccc1');
			clientCapturePage.goForward();
			expect(capitec.notifications.validationMessages()).toContain('Please provide a valid cellphone number.');				  
		});
		it("should allow a number 10 digits in length", function() {
			clientCapturePage.setCellPhoneNumberTo('0827702444');
			clientCapturePage.goForward();
			expect(capitec.notifications.validationMessages()).not.toContain('Please provide a valid cellphone number.');					  
		});
		it("should not allow the user to leave the field empty", function() {
			clientCapturePage.setCellPhoneNumberTo('');
			clientCapturePage.goForward();
			expect(capitec.notifications.validationMessages()).toContain('Please provide a valid cellphone number.');				  
		});
	});
	describe("when capturing a valid ID number ->", function(){
		it("should auto populate the day dropdown of the date of birth picker", function(){
			clientCapturePage.setIdentityNumber("8001075001032");
			protractor.getInstance().sleep(1000);
			expect(clientCapturePage.dateOfBirth.selectedDay.getText()).toBe("07");
		});
		it("should auto populate the month dropdown of the date of birth picker", function(){
			expect(clientCapturePage.dateOfBirth.selectedMonth.getText()).toBe("01");
		});
		it("should auto populate the year dropdown of the date of birth picker", function(){
			expect(clientCapturePage.dateOfBirth.selectedYear.getText()).toBe("1980");
		});
		it("should repopulate the day dropdown of the date of birth picker when the id number changes", function(){
			clientCapturePage.setIdentityNumber("6505065001002");
			protractor.getInstance().sleep(1000);
			expect(clientCapturePage.dateOfBirth.selectedDay.getText()).toBe("06");
		});
		it("should repopulate the month dropdown of the date of birth picker", function(){
			expect(clientCapturePage.dateOfBirth.selectedMonth.getText()).toBe("05");
		});
		it("should repopulate the year dropdown of the date of birth picker", function(){
			expect(clientCapturePage.dateOfBirth.selectedYear.getText()).toBe("1965");
		});
	});
	
	describe("when capturing an ID number less than 13 digits ->", function(){
		it("should display a validation message when the user tries to go forward", function(){
			clientCapturePage.setIdentityNumber("1234");
			clientCapturePage.goForward();
			expect(capitec.notifications.validationMessages()).toContain("Please provide a valid South African ID Number");
		});
	});
		
	describe("when validating the email address ->", function(){
		it("should not allow an email address without an @ sign", function(){
			clientCapturePage.setEmailAddress("gmail.com");
			clientCapturePage.goForward();
			expect(capitec.notifications.validationMessages()).toContain("Please provide a valid email address");
		});
		it("should not allow an email address without a period", function(){
			clientCapturePage.setEmailAddress("clint@gmail");
			clientCapturePage.goForward();
			expect(capitec.notifications.validationMessages()).toContain("Please provide a valid email address");
		});
		it("should not allow an email address without strings on each side of the @ sign", function(){
			clientCapturePage.setEmailAddress("@gmail.com");
			clientCapturePage.goForward();
			expect(capitec.notifications.validationMessages()).toContain("Please provide a valid email address");
		});
		it("should allow the user to leave the field empty", function(){
			clientCapturePage.setEmailAddress('');
			clientCapturePage.goForward();
			expect(capitec.notifications.validationMessages()).not.toContain("Please provide a valid email address");
		});
	});

	describe("when the client information has been populated and the user navigates away ->", function(){
		var async = new AsyncSpec(this);
		async.beforeEach(function(done){
			var sql = capitec.queries.getNextIDNumber.toString();
			capitec.queryDB(sql, [], function(err, results){
				nextAvailableIDNumber = results[0].IDNumber;
				done();
			});
		})
		it("should maintain the field values on the client information page when returning to the page",function(){
			var validClientDetails =	{ 
				idNumber: nextAvailableIDNumber, 
				surname: "Speed", 
				firstName: "Clinton", 
				dateOfBirth: "1980-01-01",  
				contactDetails: { 
					emailAddress: "test@sahomeloans.com",
					workPhoneNumber: "0315713036",
					cellPhoneNumber: "0827702666"
				},
				salutation: "Mr"
			};
			addressCapturePage = clientCapturePage.fillForm(nextAvailableIDNumber);
			addressCapturePage.goBack();
			expect(clientCapturePage.firstname.getAttribute('value')).toEqual(validClientDetails.firstName);
			expect(clientCapturePage.surname.getAttribute('value')).toEqual(validClientDetails.surname);
			expect(clientCapturePage.identityNumber.getAttribute('value')).toEqual(validClientDetails.idNumber.toString());
		});
	});
});