describe('Capitec Calculate Page Specifications -->', function(){
	
  	var capitec = require('../../capitec.js');
	var calculatePage;
	var doNotNavigate = false;	
	var jibar = 6.10;
	var cat4LinkRate = 3.90; 
	var cat7LinkRate = 4.30;
	beforeEach(function(){
		if (doNotNavigate)
			return;
		landingPage = capitec.login.LoginAsRandomTestUser();
		calculatePage = landingPage.goToCalculatePage();
	});

	describe("when navigating to the calculate page via the landing page", function(){
		it("should take the user to the correct page", function(){
			doNotNavigate = true;
			expect(browser.getCurrentUrl()).toContain(calculatePage.url);
		});
		it("should have the correct page header", function(){
			expect(calculatePage.header()).toContain(calculatePage.headerText);
		});
		it("should have the correct page title", function(){
			expect(calculatePage.title()).toContain(calculatePage.titleText);
		});
		it("should see the Household Income field", function(){
			expect(calculatePage.householdIncome.isDisplayed()).toBeTruthy();
			expect(calculatePage.householdIncome.getAttribute('value')).toEqual("R ");
		});
		it("should see the Cash Deposit field", function(){
			expect(calculatePage.cashDeposit.isDisplayed()).toBeTruthy();
			expect(calculatePage.cashDeposit.getAttribute('value')).toEqual("R ");
		});
		it("should see the Interest Rate field", function(){
			expect(calculatePage.interestRate.isDisplayed()).toBeTruthy();
		});	
		it("should default the Interest Rate value to the Alpha Housing rate", function() {
		  	expect(calculatePage.interestRate.getAttribute('value')).toEqual((jibar+cat7LinkRate).toFixed(2));
		});
		it("should have a calculate button", function(){
			doNotNavigate = false;
			expect(calculatePage.btnCalculate.isDisplayed()).toBeTruthy();
		});;
	});

	describe("when calculating what you can afford", function(){

		it('should be required to capture the household income and interest rate', function() {
			var calculateCriteria = { householdIncome : "", cashDeposit : "", interestRate: "" };
			calculateResultsPage = calculatePage.calculate(calculateCriteria);
			capitec.notifications.checkIfValidationMessageExists('Income must be greater than 0');
			capitec.notifications.checkIfValidationMessageExists('Interest Rate must be between 1 and 100 percent');
		});	

		it('should be notified if Income is less than minimum', function() {
			var calculateCriteria = { householdIncome : "5999", cashDeposit : "" };
			calculateResultsPage = calculatePage.calculate(calculateCriteria);
			capitec.notifications.checkIfValidationMessageExists('The minimum Household Income is :R 6000.0');
		});	

		it('should be notified if Income is greater than maximum', function() {
			var calculateCriteria = { householdIncome : "250001", cashDeposit : "" };
			calculateResultsPage = calculatePage.calculate(calculateCriteria);
			capitec.notifications.checkIfValidationMessageExists('The maximum Household Income is :R 250000.0');
		});	
	});	

	describe("when the default interest rate value is changed", function() {
		it("should warn the user that the default rate is determined by SAHL", function(){
			var calculateCriteria = {householdIncome : "15000", cashDeposit : "200000", interestRate : "6.25"};
			calculatePage.populateFields(calculateCriteria);
			capitec.informationToolTip.checkToolTipMessageIsDisplayed(
				"Changing this rate will impact calculation results. The default rate is determined by SA Home Loans and should be used as a base reference."
			);
		});  
	});

	describe("when the income is less than R 20,000", function(){
		it("should return the rate from alpha housing category 7", function() {
			var calculateCriteria = {householdIncome : "19999", cashDeposit : "200000"};
			calculatePage.populateFields(calculateCriteria);
			expect(calculatePage.interestRate.getAttribute('value')).toEqual((jibar+cat7LinkRate).toFixed(2))	  
		});
	});

	describe("when the income is greater than or equal to R 20,000", function() {
		it("should return the rate from category 4", function() {
		    var calculateCriteria = {householdIncome : "20000", cashDeposit : "200000"};
		    calculatePage.populateFields(calculateCriteria);
		    expect(calculatePage.interestRate.getAttribute('value')).toEqual((jibar+cat4LinkRate).toFixed(2))		
	  	});  
	});

	describe("when calculating what you can afford", function(){
		it("should return the calculate results page if all the input values are supplied", function(){
			var calculateCriteria = { householdIncome : "50000", cashDeposit : "200000", interestRate : "9.20"};
			calculateResultsPage = calculatePage.calculate(calculateCriteria);
			expect(browser.getCurrentUrl()).toContain(calculateResultsPage.url);
		});
	});	

});