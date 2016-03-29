describe("employment capture screen specs ->", function() {

	var capitec = require('../../capitec');
  	var AsyncSpec = require('jasmine-async')(jasmine);
  	var landingPage = capitec.login.LoginAsRandomTestUser();
	var applyPage = landingPage.goToApplyPage();
	var applicationPreCheckPage = applyPage.goToSwitchApplication();
	var switchPage = applicationPreCheckPage.proceedForSwitchApplication(1);
	var switchResultsPage = switchPage.fillForm();
	var clientCapturePage = switchResultsPage.apply();
	var declarationsPage;
	var nextAvailableIDNumber;
	var doNotNavigate = false;
	var clientAddress;

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

	beforeEach(function() {
		if (doNotNavigate)
			return;
		addressCapturePage = clientCapturePage.fillForm(nextAvailableIDNumber);
		employmentCapturePage = addressCapturePage.fillForm();
		doNotNavigate = true;
	});

	describe("when navigating to the employment capture screen ->", function() {
		it("should navigate to the correct url", function() {
			expect(browser.getCurrentUrl()).toContain(employmentCapturePage.url);
		});
		it("should contain the correct title", function() {
			expect(employmentCapturePage.title()).toContain(employmentCapturePage.titleText);
		});
		it("should contain the correct header", function() {
			expect(employmentCapturePage.header()).toContain(employmentCapturePage.headerText);
		});
		it("should display the employment type dropdown", function() {
			expect(employmentCapturePage.employmentTypeSpan.isDisplayed()).toBeTruthy();
		});
		it("should contain the correct list of employment types in the dropdown list", function() {
			var expectedEmploymentTypes = ["Salaried", "Self Employed", "Salaried with Housing Allowance", "Salaried with Commission", "Unemployed"];
			capitec.common.checkAllSelectOptionsExist(employmentCapturePage.employmentTypes, expectedEmploymentTypes);
		});
		it("should default the value of the employment type dropdown to 'Please Select'", function() {
			expect(employmentCapturePage.employmentTypeSpan.getText()).toEqual('Please select');	
		});
		it("should display no other income capture fields", function(){
			expect(capitec.common.checkElementExists(protractor.By.id('grossMonthlyIncome'))).toBeFalsy();
		});
	});

	describe("when clicking the next button without selecting any employment type from the dropdown->", function() {
		it("should prompt the user to select a type of employment", function() {
			employmentCapturePage.goForward();
			capitec.notifications.checkIfValidationMessageExists("An employment type is required");	    
	  	});
	});
	describe("when clicking the next button without providing employment details for a 'Salaried' applicant ->", function() {
	  	it("should prompt the user to enter a Gross Monthly Individual Income value", function() {
			employmentCapturePage.selectEmploymentType("Salaried");
			employmentCapturePage.goForward();
			capitec.notifications.checkIfValidationMessageExists("Gross Monthly Individual Income is required.");
  	  	});  
  	  	it("should prompt the user to enter a number for the gross monthly income value", function(){
			employmentCapturePage.setGrossMonthlyIndividualIncome(0);
			employmentCapturePage.goForward();
			capitec.notifications.checkIfValidationMessageExists("Gross Monthly Individual Income must be a number");
  	  	});
	});
	
	describe("when clicking the next button without providing employment details for a 'Salaried with Commission' applicant ->", function() {
	  	it("should prompt the user to enter a Gross Monthly Individual Income  value", function() {
	  		employmentCapturePage.grossIncome.clear();
			employmentCapturePage.selectEmploymentType("Salaried with Commission");	
			employmentCapturePage.goForward();
			capitec.notifications.checkIfValidationMessageExists("Gross Monthly Individual Income is required");
  	  	});  
  	  	it("should prompt the user to enter a value for the 3 months average commission field", function(){
			capitec.notifications.checkIfValidationMessageExists("3 Month Average Commission is required.");
  	  	});
  	  	it("should prompt the user to enter a number for the Gross Monthly Individual Income value", function(){
			employmentCapturePage.setGrossMonthlyIndividualIncome(0);
			employmentCapturePage.goForward();
			capitec.notifications.checkIfValidationMessageExists("Gross Monthly Individual Income must be a number");
  	  	});
	});
	describe("when clicking the next button without providing employment details for a 'Salaried With Housing Allowance' applicant ->", function() {
	  	it("should prompt the user to enter a Gross Monthly Individual Income value", function() {
	  		employmentCapturePage.grossIncome.clear();
			employmentCapturePage.selectEmploymentType("Salaried with Housing Allowance");
			employmentCapturePage.goForward();
  			capitec.notifications.checkIfValidationMessageExists("Gross Monthly Individual Income is required");	   
  	  	});  
  	  	it("should prompt the user to enter a value for the housing allowance field", function(){
  	  		capitec.notifications.checkIfValidationMessageExists("Housing Allowance is required");
  	  	});
  	  	it("should prompt the user to enter a number for the Gross Monthly Individual Income value", function(){
			employmentCapturePage.setGrossMonthlyIndividualIncome(0);
			employmentCapturePage.goForward();
  	  		capitec.notifications.checkIfValidationMessageExists("Gross Monthly Individual Income must be a number.");
  	  	});
	});

	describe("when providing valid employment information and clicking on the Next button -> ", function() {
		it("should allow the user to navigate to the declarations page of the wizard", function() {
		  	declarationsPage = employmentCapturePage.captureEmployment("Salaried", "50000");
		  	expect(browser.getCurrentUrl()).toContain(declarationsPage.url);
		});
	});
	describe("when clicking the back button from the declarations page", function() {
		it("should return to the employment page with the previously populated details maintained", function() {
			var grossIncomeValue = "R 50,000";
		    declarationsPage.goBack();
		    expect(employmentCapturePage.employmentTypeSpan.getText()).toEqual("Salaried");
		    expect(employmentCapturePage.grossIncome.getAttribute('value')).toEqual(grossIncomeValue);
	  	});  
	});
	describe("when capturing employment for an Salaried with Housing Allowance applicant", function() {
		it("should show an additional field to record the value of the housing allowance", function() {
  		  	employmentCapturePage.reset();
  		  	employmentCapturePage.selectEmploymentType('Salaried with Housing Allowance');
  		  	expect(employmentCapturePage.housingAllowance.isDisplayed()).toBeTruthy();  			    
	  	});
	  	it("should add in the description of the housing allowance field", function(){
	  		expect(employmentCapturePage.lblHousingAllowance.isDisplayed()).toBeTruthy();
	  	});
	  	it("should set the description of the housing allowance label to 'Monthly Housing Allowance'", function(){
	  		expect(employmentCapturePage.lblHousingAllowance.getText()).toEqual('Monthly Housing Allowance');		
	  	});
	  	it("should change the label of the income capture field to display 'Gross Monthly Individual Income (Excl. Housing Allowance)'", function(){
	  		expect(employmentCapturePage.lblGrossIncomeExHousingAllowance.isDisplayed()).toBeTruthy();
	  		expect(employmentCapturePage.lblGrossIncomeExHousingAllowance.getText()).toEqual('Gross Monthly Individual Income (Excl. Housing Allowance)');			
	  	});  
	});	
	describe("when capturing employment for an Salaried with Commission applicant", function() {
		it("should show an additional field to record the 3 month average commission value", function() {
		  	employmentCapturePage.reset();
  		  	employmentCapturePage.selectEmploymentType('Salaried with Commission');
  		  	expect(employmentCapturePage.threeMonthAverageCommission.isDisplayed()).toBeTruthy();  
	  	});
	  	it("should add in the description of the commission capture field", function(){
	  		expect(employmentCapturePage.lblMonthlyCommission.isDisplayed()).toBeTruthy();
	  	});
	  	it("should set the description of the commission label to 'Monthly Commission (3-Month Average)'", function(){
	  		expect(employmentCapturePage.lblMonthlyCommission.getText()).toEqual('Monthly Commission (3-Month Average)');		
	  	});
	  	it("should change the label of the income capture field to display 'Gross Monthly Individual Income (Excl. Commission)'", function(){
	  		expect(employmentCapturePage.lblGrossIncomeExCommission.isDisplayed()).toBeTruthy();
	  		expect(employmentCapturePage.lblGrossIncomeExCommission.getText()).toEqual('Gross Monthly Individual Income (Excl. Commission)');	
	  	});  
	});
	describe("when capturing employment for an unemployed applicant", function() {
	  	it("should allow the user to continue to the following step of the client capture wizard", function(){
	  		employmentCapturePage.reset();
	  		declarationsPage = employmentCapturePage.captureEmployment('Unemployed', "0");
	  		expect(declarationsPage.title()).toContain('declarations');	  		
	  	});
	});
});
