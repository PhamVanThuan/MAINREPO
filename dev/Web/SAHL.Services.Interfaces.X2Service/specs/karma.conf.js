// Karma configuration
// Generated on Thu Jun 12 2014 11:18:02 GMT+0200 (South Africa Standard Time)

module.exports = function(config) {
    config.set({

        // base path, that will be used to resolve files and exclude
        basePath: '../',


        // frameworks to use
        frameworks: ['jasmine'],


        // list of files / patterns to load in the browser
        files: [
            'specs/lib/angular/*.js',
            'specs/lib/angular-mocks/*.js',
            'specs/lib/jquery/*.js',
            'specs/lib/jquery-signalr/*.js',
            'specs/lib/underscore/*.js',
            'specs/mocks/*.js',

            'app/**/_*.js',
            'app/**/*.js'
        ],


        // list of files to exclude
        exclude: [
            '/**/*.spec.js___jb_bak___'
        ],


        // web server port
        port: 9876,


        // enable / disable colors in the output (reporters and logs)
        colors: true,


        // level of logging
        // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
        logLevel: config.LOG_INFO,


        // enable / disable watching file and executing tests whenever any file changes
        autoWatch: true,

        plugins: [
            'karma-chrome-launcher',
            'karma-firefox-launcher',
            'karma-phantomjs-launcher',
            'karma-ie-launcher',
            'karma-jasmine',
            'karma-html-reporter',
            'karma-ng-html2js-preprocessor',
            'karma-growler-reporter',
            'karma-teamcity-reporter',
            'karma-coverage'
        ],

        // Start these browsers, currently available:
        // - Chrome
        // - ChromeCanary
        // - Firefox
        // - Opera (has to be installed with `npm install karma-opera-launcher`)
        // - Safari (only Mac; has to be installed with `npm install karma-safari-launcher`)
        // - PhantomJS
        // - IE (only Windows; has to be installed with `npm install karma-ie-launcher`)
        browsers: ['PhantomJS'],

        reporters: ['progress', 'html', 'growler', 'coverage'],

        htmlReporter: {
            outputDir: __dirname + '/../test-output',
            templatePath: __dirname + '/templates/jasmine_template.html'
        },

        coverageReporter: {
            type: 'lcov',
            dir: 'coverage/'
        },

        preprocessors: {
            '**/*.tpl.html': ['ng-html2js'] 
            ,'lib/**/!(*.spec)+(.js)': ['coverage']
        },

        ngHtml2JsPreprocessor: {
            cacheIdFromPath: function(filepath) {
                var strip1 = "./" + filepath.substr(filepath.indexOf("app"));
                var to = strip1.indexOf(0, "?");
                return strip1;
                return strip1.substr(to);
            },
            moduleName: 'templates'
        },

        // If browser does not capture in given timeout [ms], kill it
        captureTimeout: 60000,


        // Continuous Integration mode
        // if true, it capture browsers, run tests and exit
        singleRun: false
    });
};
