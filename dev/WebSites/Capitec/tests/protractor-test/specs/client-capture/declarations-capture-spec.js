describe("declarations capture specifications ->", function() {
  	var capitec = require('../../capitec');
  	var AsyncSpec = require('jasmine-async')(jasmine);
  	var landingPage;
	var applyPage;
	var switchPage;
	var switchResultsPage;
	var applicationPreCheckPage;
	var clientCapturePage;
	var addressCapturePage;
	var employmentCapturePage;
	var declarationsPage;
	var clientAddress;
	var nextAvailableIDNumber;
	var doNotNavigate = false;

	function navigate(){
	  	landingPage = capitec.login.LoginAsRandomTestUser();
		applyPage = landingPage.goToApplyPage();
		applicationPreCheckPage = applyPage.goToSwitchApplication();
		switchPage = applicationPreCheckPage.proceedForSwitchApplication(1);
		switchResultsPage = switchPage.fillForm();
		clientCapturePage = switchResultsPage.apply();
		addressCapturePage = clientCapturePage.fillForm(nextAvailableIDNumber);
		employmentCapturePage = addressCapturePage.fillForm();
		declarationsPage = employmentCapturePage.captureEmployment("Self Employed", 25000);
	};

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
		capitec.common.ensureBrowserIsAtPage(declarationsPage, navigate);
	});

	describe("when navigating to the declarations capture screen for the first applicant -> ", function() {
		it("should contain the correct URL", function() {
		  	expect(browser.getCurrentUrl()).toContain(declarationsPage.url);
		});
		it("should contain the correct page title", function() {
			expect(declarationsPage.title()).toContain("declarations for mr clinton speed");			  	  
		});
		it("should contain the correct page header", function() {
			expect(declarationsPage.header()).toContain(declarationsPage.headerText);  	  
	  	});
	  	it("should display the income contributor question and answer fields", function() {
	  	  	expect(declarationsPage.incomeContributorQuestionText.getText()).toEqual("Are you an income contributor?");
	  	  	expect(declarationsPage.isIncomeContributor.yes.isDisplayed()).toBeTruthy();
	  	  	expect(declarationsPage.isIncomeContributor.yes.isDisplayed()).toBeTruthy();	 	  
  	  	});
	  	it("should display the credit bureau question and answer fields", function() {
	  	  	expect(declarationsPage.permissionForITCQuestionText.getText()).toEqual("Do you give permission for SA Home Loans to conduct a credit bureau check?");
	  	  	expect(declarationsPage.isHappyWeDoITC.yes.isDisplayed()).toBeTruthy();
	  	  	expect(declarationsPage.isHappyWeDoITC.no.isDisplayed()).toBeTruthy();	  	  	
  	  	});  
	  	it("should display the Capitec transactional bank account question and answer fields", function() {
	  	  	expect(declarationsPage.salaryPaidToCapitecQuestionText.getText()).toEqual("Is your salary paid into a Capitec transactional account?");
	  	  	expect(declarationsPage.isSalaryPaidToCapitec.yes.isDisplayed()).toBeTruthy();
	  	  	expect(declarationsPage.isSalaryPaidToCapitec.no.isDisplayed()).toBeTruthy();	  	  	
  	  	});  
	  	it("should display the married in COP question and answer fields", function() {
	  	  	expect(declarationsPage.marriedInCOPQuestionText.getText()).toEqual("Are you married in community of property?");
	  	  	expect(declarationsPage.isMarriedInCOP.yes.isDisplayed()).toBeTruthy();
	  	  	expect(declarationsPage.isMarriedInCOP.no.isDisplayed()).toBeTruthy();	  	  	
  	  	});
	  	 	  	   	
	});
	describe("when clicking next without answering any of the declarations -> ", function() {
		var notifications = [];
		async.it("should prompt the user to answer all of the declarations", function(done) {
	  		declarationsPage.printConsentForm();	  		
	  		capitec.notifications.getAllMessages(function(items){
	  			notifications = items;
	  			done();
	  		});
		});
		it("should ask the user to provide an answer for the income contributor declaration", function(){
	  		expect(notifications).toContain('Please indicate if the client is an income contributor');
		});
		it("should ask the user to provide an answer for the credit bureau check declaration", function(){
	  		expect(notifications).toContain('Please indicate if the client will allow a credit bureau check to be conducted on them');
		});
		it("should ask the user to provide an answer for the married in COP declaration", function(){
	  		expect(notifications).toContain('Please indicate if the client is married in community of property');
		});
		it("should ask the user to provide an answer for the Capitec transactional account declaration", function(){
	  		expect(notifications).toContain('Please indicate if the client has a Capitec transactional account');
		});						
	});

	describe("when navigating back without answering the declarations ->", function() {
		it("should not prompt the user to answer any unanswered questions", function() {
			doNotNavigate = true;
			clientCapturePage = declarationsPage.goBack();
	  		expect(capitec.notifications.notificationsExists()).toBeFalsy();
		});
		it("should navigate to the requested page", function(){
			doNotNavigate = false;
			expect(browser.getCurrentUrl()).toContain(employmentCapturePage.url);
		});
	});

	describe("when navigating to and from the declarations page where declarations have been answered ->", function() {
		it("should maintain the answers already provided", function() {
			var answers = { isIncomeContributor: 'Yes', isHappyWeDoITC: 'Yes', isSalaryPaidToCapitec: 'Yes', isMarriedInCOP: 'No' };
			declarationsPage.answerDeclarations(answers, true);
			declarationsPage.goBack();
			employmentCapturePage.goForward();
			expect(declarationsPage.isMarriedInCOP.no.isSelected()).toBeTruthy();
			expect(declarationsPage.isSalaryPaidToCapitec.yes.isSelected()).toBeTruthy();
			expect(declarationsPage.isIncomeContributor.yes.isSelected()).toBeTruthy();
			expect(declarationsPage.isHappyWeDoITC.yes.isSelected()).toBeTruthy();		
		});
	});

	describe("when the client does not give permission for SAHL to conduct a credit bureau check", function(){
		it("should display a message to the user explaining why this is required", function(){
			var answers = { isIncomeContributor: 'Yes', isHappyWeDoITC: 'No', isSalaryPaidToCapitec: 'Yes', isMarriedInCOP: 'No'} ;
			declarationsPage.answerDeclarations(answers);
			capitec.informationToolTip.checkToolTipMessageIsDisplayed("If an applicant is present at the time of capture, SA Home Loans requires their permission prior to performing a Credit Bureau Check.");
			capitec.informationToolTip.closeAll();
		});
		it("should ask the user to provide an answer for the income contributor declaration", function(){
			declarationsPage.printConsentForm();	
			expect(capitec.notifications.errorMessages())
			.toContain("If an applicant is present at the time of capture, SA Home Loans requires their permission prior to performing a Credit Bureau Check."); 
		});
	});

	describe("when capturing the declarations for an additional applicant", function(){
		it("should display the income contributor question and answer fields", function() {
	  	  	expect(declarationsPage.incomeContributorQuestionText.getText()).toEqual("Are you an income contributor?");
	  	  	expect(declarationsPage.isIncomeContributor.yes.isDisplayed()).toBeTruthy();
	  	  	expect(declarationsPage.isIncomeContributor.no.isDisplayed()).toBeTruthy();	 	  
  	  	});
	  	it("should display the credit bureau question and answer fields", function() {
	  	  	expect(declarationsPage.permissionForITCQuestionText.getText()).toEqual("Do you give permission for SA Home Loans to conduct a credit bureau check?");
	  	  	expect(declarationsPage.isHappyWeDoITC.yes.isDisplayed()).toBeTruthy();
	  	  	expect(declarationsPage.isHappyWeDoITC.no.isDisplayed()).toBeTruthy();	  	  	
  	  	});  
	  	it("should display the Capitec transactional bank account question and answer fields", function() {
	  	  	expect(declarationsPage.salaryPaidToCapitecQuestionText.getText()).toEqual("Is your salary paid into a Capitec transactional account?");
	  	  	expect(declarationsPage.isSalaryPaidToCapitec.yes.isDisplayed()).toBeTruthy();
	  	  	expect(declarationsPage.isSalaryPaidToCapitec.no.isDisplayed()).toBeTruthy();	  	  	
  	  	});  
	  	it("should display the married in COP question and answer fields", function() {
	  	  	expect(declarationsPage.marriedInCOPQuestionText.getText()).toEqual("Are you married in community of property?");
	  	  	expect(declarationsPage.isMarriedInCOP.yes.isDisplayed()).toBeTruthy();
	  	  	expect(declarationsPage.isMarriedInCOP.no.isDisplayed()).toBeTruthy();	  	  	
  	  	}); 	  	    	
	});
});