describe('Capitec Calculate Results Page Specifications -->', function(){
	
	var capitec = require('../../capitec.js');  	
	var calculatePage;	
	var calculateResultsPage;
	var doNotNavigate = false;

	beforeEach(function(){
		if (doNotNavigate)
			return;
		landingPage = capitec.login.LoginAsRandomTestUser();
		calculatePage = landingPage.goToCalculatePage();
	});

	describe("when calculate results page is displayed", function(){
		it("should display the correct page title", function(){
			doNotNavigate = true;
			var calculateCriteria = {householdIncome : "50000",cashDeposit : "200000", interestRate : "9.40"};
			calculateResultsPage = calculatePage.calculate(calculateCriteria);
			expect(calculateResultsPage.title()).toContain(calculateResultsPage.titleText);
		});

		it("should display the Loan Amount you can apply for", function(){
			expect(calculateResultsPage.loanAmount.getText()).toEqual("R 1,620,551.00");
		});

		it("should display the Property price you can afford", function(){
			expect(calculateResultsPage.propertyPrice.getText()).toEqual("R 1,820,551.00");
		});

		it("should display the Interest rate", function(){
			expect(calculateResultsPage.interestRate.getText()).toEqual("9.40%");
		});

		it("should display the monthly instalment", function(){
			expect(calculateResultsPage.instalment.getText()).toEqual("R 15,050.00");
		});

		it("should display the Loan term", function(){
			expect(calculateResultsPage.loanTerm.getText()).toEqual("240 months");
		});

		it("should display the service fee field", function(){
			expect(calculateResultsPage.serviceFee.getText()).toEqual("R 50.00");
		});

		it("should have a back button", function(){
			expect(calculateResultsPage.btnBack.isDisplayed()).toBeTruthy();
		});

		it("should be returned to the calculate page when the back button is clicked", function(){
			doNotNavigate = false;
			calculateResultsPage = calculateResultsPage.back();
			expect(browser.getCurrentUrl()).toContain(calculatePage.url);
		});
	});	
});