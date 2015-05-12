module.exports = function(config) {
    config.set({
        basePath: '../..',
        frameworks: ['jasmine'],
        files: [
             '../../root/scripts/UnitTests/angular/angular.js',
            //'angular/angular-mocks.js',
            '../../root/scripts/UnitTests/angular/**/*.js',
           // 't/js/angularTest/process/filtersSpec.js',
            '../root/scripts/UnitTests/calculations.js'
        ]
    });
};
