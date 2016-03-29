describe('Capitec Tracking Page Specifications -->', function(){
	
  	var capitec = require('../../capitec.js');
  	var AsyncSpec = require('jasmine-async')(jasmine);
	var trackingPage;
	var trackingResultsPage;
	var doNotNavigate = false;		
	var searchCriteria;
	beforeEach(function(){
		if (doNotNavigate)
			return;
		landingPage = capitec.login.LoginAsRandomTestUser();
		trackingPage = landingPage.goToTrackingPage();
	});

	afterEach(function(){
		browser.getCurrentUrl().then(function(url){
				if(trackingResultsPage !== undefined && url.indexOf(trackingResultsPage.url) !== -1 )
					trackingResultsPage.back();
		});
	})

	describe("when navigating to the tracking page via the landing page", function(){

		it("should take the user to the correct page", function(){
			doNotNavigate = true;
			expect(browser.getCurrentUrl()).toContain(trackingPage.url);
		});
		it("should have the correct page header", function(){
			expect(trackingPage.header()).toContain(trackingPage.pageHeader);
		});
		it("should have the correct page title", function(){
			expect(trackingPage.title()).toContain(trackingPage.pageTitle);
		});
		it("should see the Application Number field", function(){
			expect(trackingPage.applicationNumber.isDisplayed()).toBeTruthy();
			expect(trackingPage.applicationNumber.getAttribute('value')).toEqual("");
		});
		it("should see the ID Number field", function(){
			expect(trackingPage.idNumber.isDisplayed()).toBeTruthy();
			expect(trackingPage.idNumber.getAttribute('value')).toEqual("");
		});
		it("should have a Track button", function(){
			expect(trackingPage.btnTrack.isDisplayed()).toBeTruthy();
		});;
	});

	describe("when searching without supplying any search criteria", function(){

		it("should see validation message indicating that valid search criteria need to be supplied", function(){
			capitec.notifications.notificationsExists("At least one search criteria is required to search on.");
		});
	});

	describe("when searching by application number", function(){
		afterEach(function(){
			capitec.notifications.closeAll();
		});
		it("should not be able to input any alphabetic value", function(){
			searchCriteria = {applicationNumber : "R1"};
			trackingResultsPage = trackingPage.search(searchCriteria);
			expect(capitec.notifications.validationMessages()).toContain("Application Number must be a number.")
		});
		it("should not be able to input any currency value", function(){
			searchCriteria = {applicationNumber : "$1"};
			trackingResultsPage = trackingPage.search(searchCriteria);
			expect(capitec.notifications.validationMessages()).toContain("Application Number must be a number.")
		});
		it("should not be able to input a decimal", function(){
			searchCriteria = {applicationNumber : "1.1"};
			trackingResultsPage = trackingPage.search(searchCriteria);
			expect(capitec.notifications.validationMessages()).toContain("Application Number must be a number.")
		});
		it("should not be able to input a negative number", function(){
			searchCriteria = {applicationNumber : "-1"};
			trackingResultsPage = trackingPage.search(searchCriteria);
			expect(capitec.notifications.validationMessages()).toContain("Application Number must be a number.")
		});
	});

	describe("when searching by id number", function(){
		afterEach(function(){
			capitec.notifications.closeAll();
		});
		it("should not be able to input any alphabetic value", function(){
			searchCriteria = { applicationNumber :"", idNumber : "R1" };
			trackingResultsPage = trackingPage.search(searchCriteria);
			expect(capitec.notifications.validationMessages()).toContain("ID Number must be a number.");
		});
		it("should not be able to input any currency value", function(){
			searchCriteria = { idNumber : "$1" };
			trackingResultsPage = trackingPage.search(searchCriteria);
			expect(capitec.notifications.validationMessages()).toContain("ID Number must be a number.");
		});
		it("should not be able to input a decimal", function(){
			searchCriteria = { idNumber : "1.1" };
			trackingResultsPage = trackingPage.search(searchCriteria);
			expect(capitec.notifications.validationMessages()).toContain("ID Number must be a number.");
		});
		it("should not be able to input a negative number", function(){
			searchCriteria = { idNumber : "-1" };
			trackingResultsPage = trackingPage.search(searchCriteria);
			expect(capitec.notifications.validationMessages()).toContain("ID Number must be a number.");
		});
		it("should not allow a partial ID number", function(){
			searchCriteria = { idNumber : "821104" };
			trackingResultsPage = trackingPage.search(searchCriteria);
			expect(capitec.notifications.validationMessages()).toContain("ID Number must be 13 digits.");
		});
	});

	describe("when searching for an application with an application number and ID number that do not correspond", function(){

		it("should return the track results page with a message indicating no results were found", function(){
			searchCriteria = { applicationNumber : "1461362", idNumber : "8902050057081" };
			trackingResultsPage = trackingPage.search(searchCriteria);
			expect(browser.getCurrentUrl()).toContain(trackingResultsPage.url);
			expect(trackingResultsPage.title()).toContain('no results found');
			expect(trackingResultsPage.noResultsMessage.getText()).toEqual('No matching applications were found.');
		});
	});	

	describe("when searching for an application with an application number that has a valid result", function(){
		var async = new AsyncSpec(this);
		async.beforeEach(function(done){
			 		capitec.queryDB(capitec.queries.getApplicationNumberByStageAndStatus, 
			 		[{ name: 'Stage', value: 'Gathering Documentation' },{name: 'Status', value: 'In Progress' }], 
			 		function(err, application){
						searchCriteria = { 
							applicationNumber : application[0].ApplicationNumber,
							idNumber: ''
							};
						done();
					});
		});
		it("should return the track results page", function(){
			trackingResultsPage = trackingPage.search(searchCriteria);
			expect(browser.getCurrentUrl()).toContain(trackingResultsPage.url);
			expect(trackingResultsPage.title()).toContain('your application is currently at stage');
			expect(trackingResultsPage.ApplicationNo.getText()).toEqual(searchCriteria.applicationNumber.toString());
		});
	});

	describe("when searching for an application with an ID number that has a valid result", function(){
		var async = new AsyncSpec(this);
		async.beforeEach(function(done){
			 		capitec.queryDB(capitec.queries.getApplicationNumberByStageAndStatus, 
			 		[{ name: 'Stage', value: 'Gathering Documentation' },{ name: 'Status', value: 'In Progress' }], 
			 		function(err, application){
						var appNumber = application[0].ApplicationNumber;
						capitec.queryDB(capitec.queries.getApplicantsForApplication, 
						[{ name: 'ApplicationNumber', value: appNumber}],
							function(err, applicants){
								searchCriteria = { applicationNumber: '', idNumber: applicants[0].IdentityNumber};
								done();
							});
					});
		});
		it("should return the track results page", function(){
			trackingResultsPage = trackingPage.search(searchCriteria);
			expect(browser.getCurrentUrl()).toContain(trackingResultsPage.url);
			expect(trackingResultsPage.title()).toContain('your application is currently at stage');
			expect(trackingResultsPage.Applicants.getText()).toContain(searchCriteria.idNumber.toString());
		});
	});

	describe("when searching for an application with an ID number and application number that correspond", function(){
		var async = new AsyncSpec(this);
		async.beforeEach(function(done){
			 		capitec.queryDB(capitec.queries.getApplicationNumberByStageAndStatus, 
			 		[{ name: 'Stage', value: 'Gathering Documentation' },{ name: 'Status', value: 'In Progress' }], 
			 		function(err, application){
						var appNumber = application[0].ApplicationNumber;
						capitec.queryDB(capitec.queries.getApplicantsForApplication, 
						[{ name: 'ApplicationNumber', value: appNumber}],
							function(err, applicants){
								searchCriteria = { applicationNumber: appNumber, idNumber: applicants[0].IdentityNumber};
								done();
							});
					});
		});
		it("should return the track results page", function(){
			trackingResultsPage = trackingPage.search(searchCriteria);
			expect(browser.getCurrentUrl()).toContain(trackingResultsPage.url);
			expect(trackingResultsPage.title()).toContain('your application is currently at stage');
			expect(trackingResultsPage.ApplicationNo.getText()).toEqual(searchCriteria.applicationNumber.toString());
		});
	});

});