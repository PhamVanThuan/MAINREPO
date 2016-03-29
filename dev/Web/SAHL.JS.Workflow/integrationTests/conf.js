// An example configuration file.
exports.config = {
    // The address of a running selenium server.
    // seleniumAddress: 'http://localhost:4444/wd/hub',

    // Capabilities to be passed to the webdriver instance.
    capabilities: {
        'browserName': 'internet explorer',
        'platform': 'ANY',
        'version': '11'
    },
    seleniumArgs: ['-Dwebdriver.ie.driver=node_modules/protractor/selenium/IEDriverServer.exe'],
        // Spec patterns are relative to the configuration file location passed
        // to proractor (in this example conf.js).
        // They may include glob patterns.
        // specs: ['scenarioMaps/*.spec.js']
    
    baseUrl: 'http://localhost:9001/', //default test port with Yeoman

    // Options to be passed to Jasmine-node.
    jasmineNodeOpts: {
        showColors: true, // Use colors in the command line report.
    },
};
