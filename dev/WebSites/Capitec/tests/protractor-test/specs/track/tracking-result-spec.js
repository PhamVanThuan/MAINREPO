describe('Capitec Tracking Result Page Specifications -->', function(){
	
	var capitec = require('../../capitec.js'); 
	var AsyncSpec = require('jasmine-async')(jasmine); 	
	var trackingPage;
	var trackingResultPage;
	var landingPage;
	var doNotNavigate = false;	
	var stageName = "";
	var Status = "In Progress";
	var searchCriteria;

	beforeEach(function(){
		if (doNotNavigate)
			return;
		landingPage = capitec.login.LoginAsRandomTestUser();
		trackingPage = landingPage.goToTrackingPage();
	});

	 describe("when clicking on a application to track", function(){
	 	var async = new AsyncSpec(this);
	 	async.beforeEach(function(done){
	 		 		stageName = "Gathering Documentation";
	 		 		capitec.queryDB(capitec.queries.getApplicationNumberByStageAndStatus, 
	 		 		[{ name: 'Stage', value: stageName },{name: 'Status', value: Status }], function(err, applicationNo){
	 				searchCriteria = { 
	 					applicationNumber : applicationNo[0].ApplicationNumber,
	 					idNumber: ''	
	 				};
	 				done();
	 			});
	 	});
	 	it("should take the user to the correct page", function(){
	 		doNotNavigate = true;
			trackingResultPage = trackingPage.search(searchCriteria);
	 		expect(browser.getCurrentUrl()).toContain(trackingResultPage.url);
	 	});
	 	it("should have the correct page header", function(){
	 		expect(trackingResultPage.header()).toContain(trackingResultPage.pageHeader);
	 	});
	 	it("should have the correct page title", function(){
	 		expect(trackingResultPage.title()).toContain("your application is currently at stage ");
	 	});
	 	it("should display the clients name and ID Number", function(){
			expect(trackingResultPage.Applicants).toBeTruthy();
	 	});
	 	it("should display the clients Application Number", function(){
	 		expect(trackingResultPage.ApplicationNo).toBeTruthy();
	 	});
	 	it("should display the Application date", function(){
	 		expect(trackingResultPage.ApplicationDate).toBeTruthy();
	 	});
	 	it("should display the consultants username", function(){
	 		expect(trackingResultPage.SAHLConsultant).toBeTruthy();
	 	});
	 	it("should display a Contact number", function(){
	 		expect(trackingResultPage.ContactNo).toBeTruthy();
	 		doNotNavigate = false;
	 	});
	 });
	/*
	xdescribe("when clicking on a application at stage 1", function(){

	 	it("should display the correct information", function(){
	 		stageName = "Gathering Documentation";
	 		capitec.queryDB(capitec.queries.getApplicationNumberByStageAndStatus, 
	 		[{ name: 'Stage', value: stageName },{name: 'Status', value: Status }], function(err, applicationNo){

			var searchCriteria = {applicationNumber : applicationNo[0].ApplicationNumber};
			trackingResultsPage = trackingPage.search(searchCriteria);
			trackingResultPage = trackingResultsPage.selectFirstApplicationNumber();

	 		expect(browser.getCurrentUrl()).toContain(trackingResultPage.url);
	 		expect(trackingResultPage.title()).toContain("your application is currently at stage gathering documentation");
			});
	 	});
	 });

	xdescribe("when clicking on a application at stage 2", function(){

	 	it("should display the correct information", function(){
	 		stageName = "Valuation Complete";
	 		capitec.queryDB(capitec.queries.getApplicationNumberByStageAndStatus, 
	 		[{ name: 'Stage', value: stageName },{name: 'Status', value: Status }], function(err, applicationNo){

			var searchCriteria = {applicationNumber : applicationNo[0].ApplicationNumber};
			trackingResultsPage = trackingPage.search(searchCriteria);
			trackingResultPage = trackingResultsPage.selectFirstApplicationNumber();

	 		expect(browser.getCurrentUrl()).toContain(trackingResultPage.url);
	 		expect(trackingResultPage.title()).toContain("your application is currently at stage valuation complete");
			});
	 	});
	 });

	xdescribe("when clicking on a application at stage 3", function(){

	 	it("should display the correct information", function(){
	 		stageName = "Credit Assessment";
	 		capitec.queryDB(capitec.queries.getApplicationNumberByStageAndStatus, 
	 		[{ name: 'Stage', value: stageName },{name: 'Status', value: Status }], function(err, applicationNo){

			var searchCriteria = {applicationNumber : applicationNo[0].ApplicationNumber};
			trackingResultsPage = trackingPage.search(searchCriteria);
			trackingResultPage = trackingResultsPage.selectFirstApplicationNumber();

	 		expect(browser.getCurrentUrl()).toContain(trackingResultPage.url);
	 		expect(trackingResultPage.title()).toContain("your application is currently at stage credit assessment");
			});
	 	});
	 });

	xdescribe("when clicking on a application at stage 4", function(){

	 	it("should display the correct information", function(){
	 		stageName = "Loan Approved";
	 		capitec.queryDB(capitec.queries.getApplicationNumberByStageAndStatus, 
	 		[{ name: 'Stage', value: stageName },{name: 'Status', value: Status }], function(err, applicationNo){

			var searchCriteria = {applicationNumber : applicationNo[0].ApplicationNumber};
			trackingResultsPage = trackingPage.search(searchCriteria);
			trackingResultPage = trackingResultsPage.selectFirstApplicationNumber();

	 		expect(browser.getCurrentUrl()).toContain(trackingResultPage.url);
	 		expect(trackingResultPage.title()).toContain("your application is currently at stage loan approved");
			});
	 	});
	 });

	xdescribe("when clicking on a application at stage 5", function(){

	 	it("should display the correct information", function(){
	 		stageName = "Attorney Instructed";
	 		capitec.queryDB(capitec.queries.getApplicationNumberByStageAndStatus, 
	 		[{ name: 'Stage', value: stageName },{name: 'Status', value: Status }], function(err, applicationNo){

			var searchCriteria = {applicationNumber : applicationNo[0].ApplicationNumber};
			trackingResultsPage = trackingPage.search(searchCriteria);
			trackingResultPage = trackingResultsPage.selectFirstApplicationNumber();

	 		expect(browser.getCurrentUrl()).toContain(trackingResultPage.url);
	 		expect(trackingResultPage.title()).toContain("your application is currently at stage attorney instructed");
			});
	 	});
	 });

	xdescribe("when clicking on a application at stage 6", function(){

	 	it("should display the correct information", function(){
	 		stageName = "Loan Registered";
	 		capitec.queryDB(capitec.queries.getApplicationNumberByStageAndStatus, 
	 		[{ name: 'Stage', value: stageName },{name: 'Status', value: Status }], function(err, applicationNo){

			var searchCriteria = {applicationNumber : applicationNo[0].ApplicationNumber};
			trackingResultsPage = trackingPage.search(searchCriteria);
			trackingResultPage = trackingResultsPage.selectFirstApplicationNumber();

	 		expect(browser.getCurrentUrl()).toContain(trackingResultPage.url);
	 		expect(trackingResultPage.title()).toContain("your application is currently at stage loan registered");
			});
	 	});
	 });

	xdescribe("when clicking on a application that has been declined", function(){

	 	it("should display the correct information", function(){
	 		stageName = "Gathering Documentation";
	 		Status = "Decline";
	 		capitec.queryDB(capitec.queries.getApplicationNumberByStageAndStatus, 
	 		[{ name: 'Stage', value: stageName },{name: 'Status', value: Status }], function(err, applicationNo){

			var searchCriteria = {applicationNumber : applicationNo[0].ApplicationNumber};
			trackingResultsPage = trackingPage.search(searchCriteria);
			trackingResultPage = trackingResultsPage.selectFirstApplicationNumber();

	 		expect(browser.getCurrentUrl()).toContain(trackingResultPage.url);
	 		expect(trackingResultPage.title()).toContain("application has been declined");
			});
	 	});
	 });

	xdescribe("when clicking on a application that has been NTU'd", function(){

	 	it("should display the correct information", function(){
	 		stageName = "Gathering Documentation";
	 		Status = "NTU";
	 		capitec.queryDB(capitec.queries.getApplicationNumberByStageAndStatus, 
	 		[{ name: 'Stage', value: stageName },{name: 'Status', value: Status }], function(err, applicationNo){

			var searchCriteria = {applicationNumber : applicationNo[0].ApplicationNumber};
			trackingResultsPage = trackingPage.search(searchCriteria);
			trackingResultPage = trackingResultsPage.selectFirstApplicationNumber();

	 		expect(browser.getCurrentUrl()).toContain(trackingResultPage.url);
	 		expect(trackingResultPage.title()).toContain("client not proceeding with application");
			});
	 	});
	 });*/
});