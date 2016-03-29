describe('Capitec Apply Form Specifications -->', function(){
	
	var landingPage;
	var applyPage;
	var capitec = require('../../capitec.js');

	beforeEach(function() {
		
		landingPage = capitec.login.LoginAsRandomTestUser();
		applyPage = landingPage.goToApplyPage();		
	});

	describe('when navigating to the apply page', function() {
		it('should display the New Home splashbox', function(){
			expect(applyPage.newHomeApp.isDisplayed()).toBeTruthy();
		});

		it('should display the Switch splashbox', function(){
			expect(applyPage.switchApp.isDisplayed()).toBeTruthy();
		});
	})
});