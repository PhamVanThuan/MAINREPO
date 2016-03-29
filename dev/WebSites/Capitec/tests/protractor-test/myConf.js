exports.config = {
  seleniumServerJar: 'selenium/selenium-server-standalone-2.40.0.jar',
  chromeDriver: 'selenium/chromedriver.exe',
  allScriptsTimeout: 60000,
  capabilities: {
    'browserName': 'chrome',
    'chromeOptions': {
    'args': ['--start-maximized']
  	}
  },
    params: {
      adminUserLogin: {
        user: 'Andrewk1',
        password: 'Natal123',
        username: 'Clinton Speed',
        loggedIn: false
      },
      branchUserLogin : {
        user: 'Andrewk1',
        password: 'Natal123',
        username: 'Tristan Zwart',
        loggedIn: false
      },
      superUserLogin: {
        user: 'Andrewk1',
        password: 'Natal123',
        username: 'Marchuan van der Merwe',
        loggedIn: false
      },

      dbConfig: {
        user: 'capitecadmin',
        password: 'W0rdpass',
        server: 'sysacdb01',
        database: 'Capitec'
      },
      domain: "sysacw"
    },
  baseUrl: 'http://sysacw/capitec/',
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