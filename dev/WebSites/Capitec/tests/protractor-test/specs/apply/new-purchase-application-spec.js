describe('Capitec Create New Purchase Application Specifications  -->', function(){
	var capitec = require('../../capitec.js');	
	var landingPage, applyPage, applicationPreCheckPage, newPurchasePage, newPurchaseResultsPage, newPurchaseResultsFailPage;
	var doNotNavigate = false;

	function navigate(){
		landingPage = capitec.login.LoginAsRandomTestUser();
		applyPage = landingPage.goToApplyPage();
		applicationPreCheckPage = applyPage.goToNewHomeApplication();
		newPurchasePage = applicationPreCheckPage.proceedForNewPurchaseApplication(1);
	}

	beforeEach(function() {
		if (doNotNavigate)
			return;
		capitec.common.ensureBrowserIsAtPage(newPurchasePage, navigate);
	});

	describe('when navigating to the new purchase application capture screen', function() {

		it("should navigate to the correct url", function() {
			doNotNavigate = true;
			expect(browser.getCurrentUrl()).toContain(newPurchasePage.url);
		});

		it("should contain the correct title", function() {
			expect(newPurchasePage.title()).toContain(newPurchasePage.titleText);
		});

		it("should contain the correct header", function() {
			expect(newPurchasePage.header()).toContain(newPurchasePage.headerText);
		});
		
		it('should see the new purchase calculator', function() {
			expect(newPurchasePage.newPurchaseForm.isDisplayed()).toBeTruthy();
		});

		it('should see the new purchase price field', function() {
			expect(newPurchasePage.purchasePrice.isDisplayed()).toBeTruthy();
		});

		it('should see the cash deposit field', function() {
			expect(newPurchasePage.cashDeposit.isDisplayed()).toBeTruthy();
		});

		it('should see the occupancy type field and should default to --Please select--', function() {
			expect(newPurchasePage.selectedOccupancyType.getText()).toEqual('Please select');
		});

		it('should see the income type field and should default to --Please select--', function() {
			expect(newPurchasePage.selectedIncomeType.getText()).toEqual('Please select');
		});

		it('should see the gross income field', function() {
			expect(newPurchasePage.grossIncome.isDisplayed()).toBeTruthy();
		});

		it('should see the back button', function() {
			expect(newPurchasePage.backButton.isDisplayed()).toBeTruthy();
		});

		it('should see the submit button', function() {
			expect(newPurchasePage.submitButton.isDisplayed()).toBeTruthy();
		});

		it("should be able to select from a list of occupancy types", function(){
			var expectedOccupancyTypes = ["Owner Occupied", "Investment Property"];
			capitec.common.checkAllSelectOptionsExist(newPurchasePage.occupancyTypes, expectedOccupancyTypes);	
		});

		it("should be able to select from a list of employment types", function(){
			var expectedEmploymentTypes = [ "Salaried with Housing Allowance", "Salaried with Commission", "Self Employed", "Salaried"];
			capitec.common.checkAllSelectOptionsExist(newPurchasePage.incomeTypes, expectedEmploymentTypes);	
		});

		it('should be returned to the apply page when the back button is clicked', function() {
			newPurchasePage.back();
			expect(browser.getCurrentUrl()).toContain(applicationPreCheckPage.NewPurchaseUrl);
			doNotNavigate = false;
		});
	});

	describe('when capturing a new purchase application the user', function() {
		
    	it('should be required to capture the purchase price, cash deposit and gross monthly household income', function() {
			newPurchasePage.submit();
			capitec.notifications.checkIfValidationMessageExists('Purchase Price is required');
			capitec.notifications.checkIfValidationMessageExists('Cash Deposit is required');
			capitec.notifications.checkIfValidationMessageExists('Total Gross Income of all Applicants is required');
			capitec.notifications.checkIfValidationMessageExists('Occupancy Type is required');
			capitec.notifications.checkIfValidationMessageExists('Majority Income Type is required');
		});	
		
		it('should not be able to apply for a loan where loan amount is < 150 000', function() {
			newPurchaseResultsFailPage = newPurchasePage.submitPopulatedForm('119999','10000','20000','Owner Occupied','Salaried',true);
			expect(browser.getCurrentUrl()).toContain('for-new-home/calculation-result-fail');
			newPurchaseResultsFailPage.checkReasonIsDisplayed('Loan amount requested is below the product minimum.');
			expect(newPurchaseResultsFailPage.title()).toContain('application declined');
		});

		it('should be able to apply for a loan where the loan amount is > 150 000', function() {
			newPurchaseResultsPage = newPurchasePage.submitPopulatedForm('160001','10000','20000','Owner Occupied','Salaried', false);
			expect(browser.getCurrentUrl()).toContain('for-new-home/calculation-result');
			expect(newPurchaseResultsPage.title()).toContain('you may qualify for a home loan - subject to full credit assessment');
		});
	});
	
	describe('when returning from the calulation results page', function() {

		it("should navigate to the correct url", function() {
			doNotNavigate = true;
			newPurchaseResultsPage = newPurchasePage.submitPopulatedForm('1000000', '500000', '50000', 'Owner Occupied', 'Salaried');
			newPurchaseResultsPage.back();
			expect(browser.getCurrentUrl()).toContain(newPurchasePage.url);
		});

		it("should retain the populated purchase price value", function() {
			expect(newPurchasePage.purchasePrice.getAttribute('value')).toEqual('R 1,000,000');
		});

		it("should retain the populated cash deposit value", function() {
			expect(newPurchasePage.cashDeposit.getAttribute('value')).toEqual('R 500,000');
		});

		it("should retain the populated household income value", function() {
			expect(newPurchasePage.grossIncome.getAttribute('value')).toEqual('R 50,000');
		});

		it("should retain the populated occupancy type value", function() {
			expect(newPurchasePage.selectedOccupancyType.getText()).toEqual('Owner Occupied');
		});

		it("should retain the populated salary type value", function() {
			doNotNavigate = false;
			expect(newPurchasePage.selectedIncomeType.getText()).toEqual('Salaried');
		});
	});
});