describe('Capitec Switch Application Specifications  -->', function(){
	var capitec = require('../../capitec.js');
	var landingPage;
	var applyPage;
	var switchPage;
	var switchResultsPage;
	var switchResultsFailPage;
	var applicationPreCheckPage;
	var doNotNavigate = false;

	function navigate(){
		landingPage = capitec.login.LoginAsRandomTestUser();
		applyPage = landingPage.goToApplyPage();
		applicationPreCheckPage = applyPage.goToSwitchApplication();
		switchPage = applicationPreCheckPage.proceedForSwitchApplication(1);
	}

	beforeEach(function() {
		if (doNotNavigate)
			return;
		capitec.common.ensureBrowserIsAtPage(switchPage, navigate);
	});

	describe('when navigating to the switch application capture screen', function() {
		
		it("should navigate to the correct url", function() {
			doNotNavigate = true;
			expect(browser.getCurrentUrl()).toContain(switchPage.url);
		});

		it("should contain the correct title", function() {
			expect(switchPage.title()).toContain(switchPage.titleText);
		});

		it("should contain the correct header", function() {
			expect(switchPage.header()).toContain(switchPage.headerText);
		});

		it('should see the estimated market value of the home field', function() {
			expect(switchPage.marketValue.isDisplayed()).toBeTruthy();
		});

		it('should see the cash required field', function() {
			expect(switchPage.cashRequired.isDisplayed()).toBeTruthy();
		});

		it('should have a default value of R 0 in the cash required field', function() {
			expect(switchPage.cashRequired.getAttribute("value")).toEqual('R 0');
		});

		it('should see the current balance field', function() {
			expect(switchPage.currentBalance.isDisplayed()).toBeTruthy();
		});

		it('should have a default value of R 0 in the current balance field', function() {
			expect(switchPage.currentBalance.getAttribute("value")).toEqual('R 0');
		});

		it('should see the gross monthly household income field', function() {
			expect(switchPage.grossIncome.isDisplayed()).toBeTruthy();
		});

		it('should see the occupancy type field and should default to --Please select--', function() {
			expect(switchPage.selectedOccupancyType.getText()).toEqual('Please select');
		});

		it('should see the income type field and should default to --Please select--', function() {
			expect(switchPage.selectedIncomeType.getText()).toEqual('Please select');
		});

		it('should see the back button', function() {
			expect(switchPage.backButton.isDisplayed()).toBeTruthy();
		});

		it('should see the submit button', function() {
			expect(switchPage.submitButton.isDisplayed()).toBeTruthy();
		});

		it("should be able to select from a list of occupancy types", function(){
			var expectedOccupancyTypes = ["Owner Occupied", "Investment Property"];
			capitec.common.checkAllSelectOptionsExist(switchPage.occupancyTypes, expectedOccupancyTypes);	
		});

		it("should be able to select from a list of employment types", function(){
			var expectedEmploymentTypes = [ "Salaried with Housing Allowance", "Salaried with Commission", "Self Employed", "Salaried"];
			capitec.common.checkAllSelectOptionsExist(switchPage.incomeTypes, expectedEmploymentTypes);	
		});

		it('should be returned to the apply page when the back button is clicked', function() {
			doNotNavigate = false;
			switchPage.back();
			expect(browser.getCurrentUrl()).toContain(applicationPreCheckPage.SwitchUrl);
		});
    });

	describe('when capturing a switch application the user', function() {

    	it('should be required to capture the estimated market value of the home', function() {
			switchPage.submit();
			capitec.notifications.checkIfValidationMessageExists('Estimated Market Value of the Home is required');
		});

    	it('should be required to capture the occupancy type', function() {
			capitec.notifications.checkIfValidationMessageExists('Occupancy Type is required');
		});

    	it('should be required to capture the income type', function() {
			capitec.notifications.checkIfValidationMessageExists('Majority Income Type is required');
		});

    	it('should be required to capture the household income', function() {
    		capitec.notifications.checkIfValidationMessageExists('Total Gross Income of all Applicants is required');
		});

		it('should warn the user and ask if they are sure they have no balance on their bond', function(){
			capitec.notifications.checkIfValidationMessageExists('Are you sure you have no balance on your bond account?')
		});

		it('should warn the user when tabing out of current loan amount field with zero input', function(){
			capitec.notifications.closeAll();
			switchResultsPage = switchPage.populateForm('500000','150000','0','30000');
			capitec.notifications.checkIfValidationMessageExists('Are you sure you have no balance on your bond account?')
		});

		it('should not be able to apply for a loan where loan amount is < 150 000', function() {
			switchResultsFailPage = switchPage.submitPopulatedForm('119999','10000','100000','20000','Owner Occupied','Salaried',true);
			expect(browser.getCurrentUrl()).toContain('/for-switch/calculation-result-fail');
			switchResultsFailPage.checkReasonIsDisplayed('Loan amount requested is below the product minimum.');
			expect(switchResultsFailPage.title()).toContain('application declined');
			
		});

		it('should be possible to apply for a loan where current loan amount is zero', function(){
			switchPage.populateForm('500000','150000','0','30000');
			switchPage.setOccupancyTypeTo('Owner Occupied');
			switchPage.setIncomeTypeTo('Salaried');
			capitec.notifications.closeAll();
			switchResultsPage = switchPage.submit(false)
			expect(browser.getCurrentUrl()).toContain('/for-switch/calculation-result');
			capitec.notifications.checkIfValidationMessageExists('Are you sure you have no balance on your bond account?')
			expect(switchResultsPage.title()).toContain('you may qualify for a home loan - subject to full credit assessment');
		});

		it('should be able to apply for a loan where the loan amount is > 150 000', function() {
			switchResultsPage = switchPage.submitPopulatedForm('500000','100000','90000','30000','Owner Occupied','Salaried', false);
			expect(browser.getCurrentUrl()).toContain('/for-switch/calculation-result');
			expect(switchResultsPage.title()).toContain('you may qualify for a home loan - subject to full credit assessment');
		});

		it('should be able to apply for a loan without any cash out', function() {
			switchResultsPage = switchPage.submitPopulatedForm('500000','0','190000','30000','Owner Occupied','Salaried', false);
			expect(browser.getCurrentUrl()).toContain('/for-switch/calculation-result');
			expect(switchResultsPage.title()).toContain('you may qualify for a home loan - subject to full credit assessment');
		});
	});
});