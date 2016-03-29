'use strict';
describe('[sahl.js.core.logging]', function () {
    beforeEach(module('sahl.js.core.logging'));
    var $loggingService;
    beforeEach(inject(function ($injector, $q) {
        $loggingService = $injector.get("$loggingService");
    }));

    describe(' - (Service: loggingService)-', function () {
        describe('when using logInfo', function () {
            it('should be empty method', function () {
                expect($loggingService.logInfo).not.toThrow();
            });
        });

        describe('when using logWarning', function () {
            it('should be empty method', function () {
                expect($loggingService.logInfo).not.toThrow();
            });
        });

        describe('when using logInfo', function () {
            it('should be empty method', function () {
                expect($loggingService.logInfo).not.toThrow();
            });
        });
    });
});
