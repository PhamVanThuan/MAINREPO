describe("Address Capture Specifications ->", function() {
  
	var capitec = require('../../capitec.js');
	var AsyncSpec = require('jasmine-async')(jasmine);
	var landingPage = capitec.login.LoginAsRandomTestUser();
	var applyPage = landingPage.goToApplyPage();
	var applicationPreCheckPage = applyPage.goToSwitchApplication();
	var switchPage = applicationPreCheckPage.proceedForSwitchApplication(1);
	var switchResultsPage;
	var clientCapturePage;
	var addressCapturePage;
	var nextAvailableIDNumber;
	var employmentPage;

	var async = new AsyncSpec(this);
	async.beforeEach(function(done){
			if (!nextAvailableIDNumber){
				var sql = capitec.queries.getNextIDNumber.toString();
				capitec.queryDB(sql, [], function(err, results){
					nextAvailableIDNumber = results[0].IDNumber;
					done();
				});		
			} else {
				done();
			}
	});
	describe("when navigating to the address capture screen ->", function() {
		it("should contain the address capture fields", function() {
			switchResultsPage = switchPage.fillForm();
			clientCapturePage = switchResultsPage.apply();
	  		addressCapturePage = clientCapturePage.fillForm(nextAvailableIDNumber);
			expect(addressCapturePage.unitNumber.isDisplayed()).toBeTruthy();
			expect(addressCapturePage.buildingNumber.isDisplayed()).toBeTruthy();
			expect(addressCapturePage.buildingName.isDisplayed()).toBeTruthy();
			expect(addressCapturePage.streetName.isDisplayed()).toBeTruthy();
			expect(addressCapturePage.streetNumber.isDisplayed()).toBeTruthy();
			expect(addressCapturePage.suburb.isDisplayed()).toBeTruthy();
			expect(addressCapturePage.province.isDisplayed()).toBeTruthy();
			expect(addressCapturePage.postalCode.isDisplayed()).toBeTruthy();		  		
		});
		it("should have the correct title", function(){
			expect(addressCapturePage.title()).toContain(addressCapturePage.titleText);   
		});
		it("should have the correct header", function() {
			expect(addressCapturePage.header()).toContain(addressCapturePage.headerText);   
		});
		it("should navigate to the correct URL", function() {
			expect(browser.getCurrentUrl()).toContain(addressCapturePage.url);	 	    
		});
		it("should have the city field set to readonly", function() {
			expect(addressCapturePage.city.getAttribute('readonly')).toBeTruthy();
		});
		it("should have the province field set to readonly", function() {
			expect(addressCapturePage.province.getAttribute('readonly')).toBeTruthy();
		});
		it("should have the postal code field set to readonly", function() {
			expect(addressCapturePage.postalCode.getAttribute('readonly')).toBeTruthy();
		});
	});

	describe("when no address values are captured ->", function() {
		it("should prompt the user to capture a street number", function() {
			addressCapturePage.goForward();
		  expect(capitec.notifications.validationMessages()).toContain("Street Number is required");
		});
		it("should prompt the user to capture a street name", function() {
			addressCapturePage.goForward();
  		  expect(capitec.notifications.validationMessages()).toContain("Street Name is required");
		});
		it("should prompt the user to capture a suburb", function() {
			addressCapturePage.goForward();
  		  expect(capitec.notifications.validationMessages()).toContain("Suburb is required");
		});
	});

	describe("when an invalid suburb is captured", function(){
		it("should prompt the user to capture a suburb", function(){
			addressCapturePage.reset();
			var address = {
				streetName: "Street",
				streetNumber: "99",
				suburb: "Rubish Suburb"
			};
			addressCapturePage.captureAddress(address, true);
			expect(capitec.notifications.validationMessages()).toContain("Please enter a valid Suburb");
		});
	});

	describe("when a valid suburb is selected from the auto complete list ->", function() {
		it("should auto populate the province field", function() {
			addressCapturePage.reset();
			var address = {
				streetName: "Street",
				streetNumber: "99",
				suburb: "Durban North",
				province: "Kwazulu-natal"
			};
			addressCapturePage.captureAddress(address, false);
			expect(addressCapturePage.province.getAttribute('value')).toEqual("Kwazulu-natal");		  
		});
		it("should auto populate the city field", function() {
			expect(addressCapturePage.city.getAttribute('value')).toEqual("Durban (Kwazulu-natal)");
		});
		it("should auto populate the postal code field", function() {
			expect(addressCapturePage.postalCode.getAttribute('value')).toEqual("4051");
		});
	});

	describe("when an invalid suburb is captured after selecting a valid suburb", function(){
		it("should prompt the user to capture a suburb", function(){
			addressCapturePage.selectInvalidSuburb();
			expect(capitec.notifications.validationMessages()).toContain("Please enter a valid Suburb");
		});
	});

	describe("when inputing a building name greater then 50 characters", function() {
		it("should only alow input of 50 characters", function(){
			var address = {
				buildingName: "Believe me this input string is 51 characters long!",
				streetName: "Rock Road",
				streetNumber: "99",
				suburb: "Hillcrest",
				city: "Hillcrest",
				province:"Kwazulu-natal"
			};
			addressCapturePage.captureAddress(address, true);
			expect(capitec.notifications.validationMessages()).toContain("Building Name cannot be longer than 50 characters.");
		});
	});

	describe("when inputing a street name greater then 50 characters", function() {
		it("should only alow input of 50 characters", function(){
			addressCapturePage.reset();
			var address = {
				streetName: "Believe me this input string is 51 characters long!",
				streetNumber: "99",
				suburb: "Hillcrest",
				city: "Hillcrest",
				province:"Kwazulu-natal"
			};
			addressCapturePage.captureAddress(address, true);
			expect(capitec.notifications.validationMessages()).toContain("Street Name cannot be longer than 50 characters.");
		});
	});

	describe("when a valid address is supplied", function() {
	    it("should allow the user to proceed to the employment page when clicking the next button", function() {
	    	addressCapturePage.reset();
	    	var address = {
	    		buildingName: "This input string is only just 50 characters long!",
	    		buildingNumber: "55",
				streetName: "This input string is only just 50 characters long!",
				streetNumber: "99",
				suburb: "Hillcrest",
				city: "Hillcrest",
				province:"Kwazulu-natal"
			};			
    		employmentPage = addressCapturePage.captureAddress(address, true);
    		expect(browser.getCurrentUrl()).toContain(employmentPage.url);
	    });	
	});

	describe("when naigating back to address capture screen", function(){		
	    it("should maintain the address details", function() {
	    	employmentPage.goBack();
	      	expect(addressCapturePage.buildingNumber.getAttribute('value')).toEqual("55");
	      	expect(addressCapturePage.buildingName.getAttribute('value')).toEqual("This input string is only just 50 characters long!");
	      	expect(addressCapturePage.streetNumber.getAttribute('value')).toEqual("99");
	      	expect(addressCapturePage.streetName.getAttribute('value')).toEqual("This input string is only just 50 characters long!");
	      	expect(addressCapturePage.suburb.getAttribute('value')).toEqual("Hillcrest (Kwazulu-natal)");
	    });
	});

});