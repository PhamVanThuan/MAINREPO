describe('Capitec Landing Page Specifications -->', function(){
	
	var capitec = require('../capitec.js');
	var landingPage;

	beforeEach(function(){
		landingPage = capitec.login.LoginAsRandomTestUser();
	});

	describe('when arriving at the landing page', function(){

		it('should display the apply image', function(){
			expect(landingPage.apply.isDisplayed()).toBeTruthy();
		});

		it('should display the calculate image', function(){
			expect(landingPage.calculate.isDisplayed()).toBeTruthy();
		});

		it('should display the track image', function(){
			expect(landingPage.track.isDisplayed()).toBeTruthy();
		});
	});

	describe('Landing Page Navigation Specs -->', function(){

		describe('when clicking the calculator image', function(){
			it('should navigate to the calculator page', function(){
				landingPage.goToCalculatePage();
				expect(browser.getCurrentUrl()).toContain("/#/still-looking");
			});
		});

		describe('when clicking the apply image', function(){
			it('should navigate to the apply page', function(){
				landingPage.goToApplyPage();
				expect(browser.getCurrentUrl()).toContain("/#/apply");
			});
		});

		describe('when clicking the tracking image', function(){
			it('should navigate to the tracking page', function(){
				landingPage.goToTrackingPage();	
				expect(browser.getCurrentUrl()).toContain("/#/track");			
			});
		});
	});
});