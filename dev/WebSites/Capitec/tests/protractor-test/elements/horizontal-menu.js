	module.exports = function(applicationName){

	var menu = function(){

		var loginPage = require('../pages/login-page.js');

		this.calculate = element(by.linkText("Calculate"));
		this.apply = element(by.linkText("Apply"));
		this.track = element(by.linkText("Track"));
		this.administration = element(by.linkText("Administration"));
		this.home = element(by.css("a[class~='icon-home']"));
		this.settings = element(by.css("a[class~='icon-settings']"));
		this.eLogout = element(by.css("a[class~='icon-logout']"));
		this.userName = element(by.binding("userDisplayName"));

		this.goToCalculators = function(){
			this.calculate.click();
		};
		this.goToApply = function(){
			this.apply.click();
		};
		this.goToTracking = function(){
			this.track.click();
		};
		this.goToAdministration = function(){
			this.administration.click();
			return adminLandingPage;
		};
		this.logout = function(){
			var capitec = require('../capitec.js');
			if(this.eLogout.isDisplayed()){
				this.eLogout.click();
				capitec.login.removeAuthCookie();
				return loginPage;
			}
		};
		this.goToSettings = function(){
			this.settings.click();
			return changePasswordPage;
		};
		this.goToHome = function(){
			this.home.click();
		};
	};

	applicationName.horizontalMenu = new menu();
}