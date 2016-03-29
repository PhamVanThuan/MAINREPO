exports.config = {
  rootElement: 'html',
  allScriptsTimeout: 60000,
  framework: 'jasmine2',
  capabilities:
  {
    'browserName': 'chrome',
    'chromeOptions': {
        'args': ['--start-maximized']
      }
  },
  specs: ['specs/*.js'],
  params: {
      testUserLogin: {
        user: 'HaloUser',
        password: 'Natal123',
        username: 'HaloUser',
        loggedIn: false
      },
      dbConfig: {
        user: 'eworkadmin',
        password: 'W0rdpass',
        server: 'devb03',
        database: '[2AM]'
      },
      domain: "localhost"
  },
  onPrepare: function() {
    "use strict";
    browser.loginAs = function(user){
        var path = "localhost/halov3";
        var autenticationUrl = "http://" + user.username.toString() + ":" + 
                              user.password.toString() + "@" + path.toString();
        browser.driver.get(autenticationUrl);
        browser.driver.wait(function() {
          return browser.driver.getCurrentUrl().then(function(url) {
              if (url.indexOf(path) > 0) {
                return true;
              }
              return false;
          });
        });
    };
    var loginUser = {
        user: 'HaloUser',
        password: 'Natal123',
        username: 'HaloUser',
        loggedIn: false
      };
    browser.loginAs(loginUser);
  },
  jasmineNodeOpts: {
    // onComplete will be called just before the driver quits.
    onComplete: null,
    // If true, display spec names.
    isVerbose: true,
    // If true, print colors to the terminal.
    showColors: true,
    // If true, include stack traces in failures.
    includeStackTrace: true,
    // Default time to wait in ms before a test fails.
    defaultTimeoutInterval: 60000
  }
}