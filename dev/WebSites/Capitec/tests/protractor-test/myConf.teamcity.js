exports.config = {
  seleniumServerJar: 'selenium/selenium-server-standalone-2.40.0.jar',
  chromeDriver: 'selenium/chromedriver.exe',
  allScriptsTimeout: 30000,
  capabilities: {
    'browserName': 'chrome',
    'chromeOptions': {
      'args': ['--start-maximized']
  	}
  },
  suites: {
      clientcapture: ['specs/client-capture/single-applicant-switch-spec.js',
                    'specs/client-capture/single-applicant-newpurchase-spec.js',
                    'specs/client-capture/multiple-applicants-spec.js',
                    'specs/client-capture/application-already-exists-for-client-spec.js',
                    'specs/client-capture/removing-applicants-spec.js',
                    'specs/client-capture/applicant-is-not-an-income-contributor-spec.js',
                    'specs/client-capture/applicant-married-in-cop-spec.js'],
      wizardsteps: ['specs/client-capture/address-capture-spec.js',
                    'specs/client-capture/declarations-capture-spec.js',
                    'specs/client-capture/main-contact-for-application-spec.js'],
      clientcapturescreen: ['specs/client-capture/client-capture-spec.js', 
                            'specs/client-capture/client-capture-process-spec.js'],
      employmentcapture: ['specs/client-capture/employment-capture-spec.js'],
      apply: ['specs/apply/*spec.js'],
      tracking: ['specs/track/*spec.js'],
      calculate: ['specs/calculate/*spec.js'],
      knockout: ['specs/client-capture/credit-bureau-knock-out-rules-spec.js'],
      applicationsubmit: ['specs/client-capture/application-submit-spec.js'],
      test: ['specs/client-capture/single-applicant-switch-spec.js']
  },
  params: {
      adminUserLogin: {
        user: 'ClintonS',
        password: 'Natal123',
        username: 'Clinton Speed',
        loggedIn: false
      },
      branchUserLogin : {
        user: 'TristanZ',
        password: 'Natal123',
        username: 'Tristan Zwart',
        loggedIn: false
      },   
      superUserLogin: {
        user: 'MarchuanV',
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
    isVerbose: false,
    // If true, print colors to the terminal.
    showColors: false,
    // If true, include stack traces in failures.
    includeStackTrace: true,
    // Default time to wait in ms before a test fails.
    defaultTimeoutInterval: 60000
    },
    onPrepare: function() {
      // The require statement must be down here, since jasmine-reporters needs jasmine to be in the global and protractor does not guarantee
      // this until inside the onPrepare function.
      require('jasmine-reporters');
      jasmine.getEnv().addReporter(
        new jasmine.TeamcityReporter());
    }
}