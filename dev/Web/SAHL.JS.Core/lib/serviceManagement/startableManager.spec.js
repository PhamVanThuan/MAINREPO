'use strict';
describe('[sahl.js.core.serviceManagement]', function () {
    beforeEach(module('sahl.js.core.serviceManagement'));
    var $startableManagerService, fakeFunctions, $rootScope;
    beforeEach(inject(function ($injector) {
        $startableManagerService = $injector.get('$startableManagerService');
        $rootScope = $injector.get('$rootScope');
        fakeFunctions = {
            deferFn : function () { },
            failFn : function () { }
        };
        spyOn(fakeFunctions, 'deferFn');
        spyOn(fakeFunctions,'failFn');
    }));

    describe(' - (Service: startableManager)-', function () {
        describe('when no services are passed in', function () {
            var $result;
            beforeEach(function () {
                $startableManagerService.startServices().then(fakeFunctions.deferFn, fakeFunctions.failFn);
                $rootScope.$digest();
            });
            it('should defer', function () {
                expect(fakeFunctions.deferFn).toHaveBeenCalled();
            });
            it('should not fail', function () {
                expect(fakeFunctions.failFn).not.toHaveBeenCalled();
            });
        });

        describe('when one service is passed in, that returns no deferred', function () {
            var $result,fakeService;
            beforeEach(function () {
                fakeService = {
                    start: function () { }
                };
                $startableManagerService.startServices(fakeService).then(fakeFunctions.deferFn, fakeFunctions.failFn);
                $rootScope.$digest();

            });
            it('should defer', function () {
                expect(fakeFunctions.deferFn).toHaveBeenCalled();
            });
            it('should not fail', function () {
                expect(fakeFunctions.failFn).not.toHaveBeenCalled();
            });
        });

        describe('when one service is passed in, that returns deferred', function () {
            var $result, fakeService,deferred;

            beforeEach(inject(function ($q) {
                deferred = $q.defer();
                fakeService = {
                    start: function () {
                        return deferred.promise;
                    }
                };
                $startableManagerService.startServices(fakeService).then(fakeFunctions.deferFn, fakeFunctions.failFn);
                $rootScope.$digest();
            }));
            it('should defer when expected', function () {
                expect(fakeFunctions.deferFn).not.toHaveBeenCalled();
                deferred.resolve();
                $rootScope.$digest();
                expect(fakeFunctions.deferFn).toHaveBeenCalled();
            });
            it('should not fail', function () {
                expect(fakeFunctions.failFn).not.toHaveBeenCalled();
            });
        });

        describe('when mixing services is passed in', function () {
            var $result, fakeServiceOne,fakeServiceTwo, deferred;

            beforeEach(inject(function ($q) {
                deferred = $q.defer();
                fakeServiceOne = {
                    start: function () {
                        return deferred.promise;
                    }
                };
                fakeServiceTwo = {
                    start: function () {
                    }
                };
                $startableManagerService.startServices(fakeServiceOne, fakeServiceTwo).then(fakeFunctions.deferFn, fakeFunctions.failFn);
                $rootScope.$digest();
                deferred.resolve();
                $rootScope.$digest();
            }));
            it('should defer when expected', function () {
                expect(fakeFunctions.deferFn).toHaveBeenCalled();
            });
            it('should not fail', function () {
                expect(fakeFunctions.failFn).not.toHaveBeenCalled();
            });
        });

        describe('when service throws an error', function () {
            var $result, fakeServiceOne, fakeServiceTwo, deferred;

            beforeEach(inject(function ($q) {
                deferred = $q.defer();
                fakeServiceOne = {
                    start: function () {
                        return deferred.promise;
                    }
                };
                $startableManagerService.startServices(fakeServiceOne).then(fakeFunctions.deferFn, fakeFunctions.failFn);
                deferred.reject();
                $rootScope.$digest();
            }));
            it('should not defer when expected', function () {
                expect(fakeFunctions.deferFn).not.toHaveBeenCalled();
            });
            it('should fail', function () {
                expect(fakeFunctions.failFn).toHaveBeenCalled();
            });
        });
    });
});
