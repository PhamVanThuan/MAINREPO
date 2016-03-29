  var page = require('../page.js');
  var landingPage = require('../pages/landing-page.js');
  var ptor = protractor.getInstance();
  var loginPage = new page();

  loginPage.usernameInput = element(by.model('username'));
  loginPage.passwordInput = element(by.model('password'));
  loginPage.loginButton = element(by.id("submit1"));
  loginPage.errorMessage = element(by.binding("errorMessage")); 
  loginPage.url = "/#/login";
  loginPage.titleText = "enter your login details to begin";
  loginPage.headerText = "login"; 
  loginPage.loggedInUser;

  loginPage.goTo = function(credentials) {
    var ptor = protractor.getInstance();
    var capitec = require('../capitec.js');
    capitec.login.removeAuthCookie();
    var url = ptor.baseUrl+'#/autologin?cp='+credentials.user+'&branch=2250';
    browser.get(url);
    loginPage.loggedInUser = credentials;
  };

  loginPage.loginWithCredentials = function(credentials){
    loginPage.goTo();
    loginPage.usernameInput.sendKeys(credentials.user);
    loginPage.passwordInput.sendKeys(credentials.password);
  	loginPage.loginButton.click();
    loginPage.loggedInUser = credentials;
  };

  loginPage.login = function(){
        loginPage.goTo();
        return loginPage;
  };

  loginPage.asAdmin = function(){
    loginPage.loginWithCredentials(ptor.params.adminUserLogin);
    return landingPage;
  };

  module.exports = loginPage;

