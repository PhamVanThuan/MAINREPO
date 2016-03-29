module.exports = function(applicationName){
	var loginModule = function(){
	
	var landingPage = require('../pages/landing-page.js');
	var loginPage = require('../pages/login-page.js');
	var utils = {};
	require('../utils.js')(utils);
		var ptor = protractor.getInstance();
		
		this.ensureProfileIsSetTo = function(){
			return this;
		};
		this.adminUser = function(){
			return checkUserIsLoggedInAs('adminUserLogin');
		};
		this.branchUser = function(){
			return checkUserIsLoggedInAs('branchUserLogin');
		};
		this.superUser = function(){
			return checkUserIsLoggedInAs('superUserLogin');
		};

		this.removeAuthCookie = function(){
		    ptor.manage().deleteAllCookies();
		    resetProtractorConfig();
		};

		this.LoginAsRandomTestUser = function() {
			var credentials = {
        		user: 'TestUser' + utils.common.randomString(7, '1234567890'),
        		password: 'Natal123',
        		username: 'Test User',
        		loggedIn: false
        	};
			loginPage.goTo(credentials);
			//this.loggedIn = true;
			return landingPage;
		};

		
		function checkUserIsLoggedInAs(userRole){
			var credentials = getRoleFromPractorConfig(ptor.params, userRole);

			if(credentials.loggedIn){
				//go to home
				browser.get(landingPage.url);
				this.loggedIn = true;
				return landingPage;
			}
			else
			{
				//we are changing credentials
				resetProtractorConfig();
				loginPage.goTo(credentials);
				//loginPage.loginWithCredentials(credentials);
				this.loggedIn = true;
				ptor.params[userRole].loggedIn = true;
				return landingPage;
			}
		};

		function getRoleFromPractorConfig(config, role){
		  	return config[role];
		};

		function resetProtractorConfig(){
			ptor.params['adminUserLogin'].loggedIn = false;
			ptor.params['branchUserLogin'].loggedIn = false;
			ptor.params['superUserLogin'].loggedIn = false;
		};
	};

	applicationName.login = new loginModule();
}