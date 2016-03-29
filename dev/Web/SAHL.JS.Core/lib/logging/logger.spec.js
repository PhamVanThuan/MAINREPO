'use strict';
describe('[sahl.js.core.logging]', function () {
    beforeEach(module('sahl.js.core.logging'));
    var $logger, $loggingService;
    var consoleLogMethod;
    beforeEach(inject(function ($injector, $q) {
        consoleLogMethod = console.log;
    }));
    describe(' - (Service: logger)-', function () {

        describe('when debug disabled', function () {
            beforeEach(inject(function ($injector) {
                $loggingService = $injector.get('$loggingService');
                $logger = $injector.get('$logger');
            }));
            describe('when logging an information message', function () {
                beforeEach(inject(function () {
                    spyOn($loggingService, "logInfo").and.callThrough();
                    $logger.start();
                    $logger.logInfo("test");
                }));

                it('should try and write to the logging service', function () {
                    expect($loggingService.logInfo).toHaveBeenCalled();
                });

                it('should not throw any errors', function () {
                    expect($logger.logInfo).not.toThrow();
                });
            });

            describe('when logging an information message and loggingService is unavailable', function () {
                beforeEach(inject(function () {
                    spyOn($loggingService, "logInfo").and.callFake(function () {
                        throw "This should never happen";
                    });
                    $logger.start();
                    $logger.logInfo("test");
                }));

                it('should try and write to the logging service', function () {
                    expect($loggingService.logInfo).toHaveBeenCalled();
                });

                it('should not throw any errors', function () {
                    expect($logger.logInfo).not.toThrow();
                });
            });

            describe('when logging an warning message', function () {
                beforeEach(inject(function () {
                    spyOn($loggingService, "logWarning").and.callThrough();
                    $logger.start();
                    $logger.logWarning("test");
                }));

                it('should try and write to the logging service', function () {
                    expect($loggingService.logWarning).toHaveBeenCalled();
                });

                it('should not throw any errors', function () {
                    expect($logger.logWarning).not.toThrow();
                });
            });

            describe('when logging an warning message and loggingService is unavailable', function () {
                beforeEach(inject(function () {
                    spyOn($loggingService, "logWarning").and.callFake(function () {
                        throw "This should never happen";
                    });
                    $logger.start();
                    $logger.logWarning("test");
                }));

                it('should try and write to the logging service', function () {
                    expect($loggingService.logWarning).toHaveBeenCalled();
                });

                it('should not throw any errors', function () {
                    expect($logger.logWarning).not.toThrow();
                });
            });

            describe('when logging an error message', function () {
                beforeEach(inject(function () {
                    spyOn($loggingService, "logError").and.callThrough();
                    $logger.start();
                    $logger.logError("test", "test");
                }));

                it('should try and write to the logging service', function () {
                    expect($loggingService.logError).toHaveBeenCalled();
                });

                it('should not throw any errors', function () {
                    expect($logger.logError).not.toThrow();
                });
            });

            describe('when logging an error message and there is an error in loggingService', function () {
                beforeEach(inject(function () {
                    spyOn($loggingService, "logError").and.callFake(function () {
                        throw "This should never happen";
                    });
                    $logger.start();
                    $logger.logError("test", "test");
                }));

                it('should try and write to the logging service', function () {
                    expect($loggingService.logError).toHaveBeenCalled();
                });

                it('should not throw any errors', function () {
                    expect($logger.logError).not.toThrow();
                });
            });
        });

        describe('when debug enabled', function () {

            beforeEach(inject(function ($injector) {
                window.debug = true;
                $logger = $injector.get('$logger');
                $loggingService = $injector.get('$loggingService');
            }));

            describe('when logging an information message', function () {
                beforeEach(inject(function () {
                    spyOn($loggingService, "logInfo");

                    $logger.start();
                    $logger.logInfo("test");
                }));

                it('should try and write to the logging service', function () {
                    expect($loggingService.logInfo).not.toHaveBeenCalled();
                });

                it('should not throw any errors', function () {
                    expect($logger.logInfo).not.toThrow();
                });
            });

            describe('when logging an warning message', function () {
                beforeEach(inject(function () {
                    spyOn($loggingService, "logWarning");
                    $logger.start();
                    $logger.logWarning("test");
                }));

                it('should try and write to the logging service', function () {
                    expect($loggingService.logWarning).not.toHaveBeenCalled();
                });

                it('should not throw any errors', function () {
                    expect($logger.logWarning).not.toThrow();
                });
            });

            describe('when logging an error message', function () {
                beforeEach(inject(function () {
                    spyOn($loggingService, "logError");
                    $logger.start();
                    $logger.logError("test", "test");
                }));

                it('should try and write to the logging service', function () {
                    expect($loggingService.logError).not.toHaveBeenCalled();
                });

                it('should not throw any errors', function () {
                    expect($logger.logError).not.toThrow();
                });
            });
        });
        afterEach(function(){
            console.log = consoleLogMethod;
        });
    });
});
