module.exports = function (config) {
    config.set({
        basePath: './',

        files: [
          'lib/jquery/jquery-*.js',
          'lib/angular/angular.js',
          'lib/angular/angular-*.js',
          'lib/angular/angular-mocks.js',
          'lib/specs/*.js',
          'lib/angular-ui-router/angular-ui-router.js',
          'lib/specs/*.js',
          'lib/pnotify/jquery.pnotify.min.js',
          'app/**/*.js',
          '**/*.tpl.html'
        ],

        exclude: [
          'lib/angular/angular-loader.js',
          'lib/angular/*.min.js',
          'lib/angular/angular-scenario.js'
        ],

        autoWatch: true,

        frameworks: ['jasmine'],

        browsers: ['PhantomJS'],//, 'IE'

        plugins: [
          'karma-chrome-launcher',
          'karma-firefox-launcher',
          'karma-phantomjs-launcher',
          'karma-ie-launcher',
          'karma-jasmine',
          'karma-html-reporter',
          'karma-ng-html2js-preprocessor',
          'karma-growler-reporter',
          'karma-coverage'
        ],

        reporters: ['progress', 'html', 'growler', 'coverage'],

        htmlReporter: {
            outputDir: __dirname + '/test-output',
            templatePath: __dirname + '/scripts/testtemplates/jasmine_template.html'
        },

        coverageReporter: {
            type: 'html',
            dir: 'coverage/'
        },

        logLevel: config.LOG_DEBUG,

        preprocessors: {
            '**/*.tpl.html': ['ng-html2js'],
            'app/**/*.js': ['coverage']
        },

        ngHtml2JsPreprocessor: {
            cacheIdFromPath: function (filepath) {
                var strip1 = "./" + filepath.substr(filepath.indexOf("app"));
                var to = strip1.indexOf(0, "?");
                return strip1;
                return strip1.substr(to);
            },
            moduleName: 'templates'
        }

    })
}
