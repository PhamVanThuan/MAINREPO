'use strict';
describe('[sahl.js.core.applicationManagement]', function () {
    beforeEach(module('sahl.js.core.applicationManagement'));
    var $navigationService;
    beforeEach(inject(function ($injector, $q) {
        $navigationService = $injector.get('$navigationService');
    }));

    describe(' - (Service: navigation)-', function () {
        describe('goHome function', function () {
            it('should be empty function', function () {
                expect($navigationService.goHome).toEqual($navigationService.goTo);
            });
        });

        describe('goClient function', function () {
            it('should be empty function', function () {
                expect($navigationService.goClient).toEqual($navigationService.goTo);
            });
        });
        describe('goTasks function', function () {
            it('should be empty function', function () {
                expect($navigationService.goTasks).toEqual($navigationService.goTo);
            });
        });
        describe('goApps function', function () {
            it('should be empty function', function () {
                expect($navigationService.goApps).toEqual($navigationService.goTo);
            });
        });
        describe('goTo function', function () {
            it('should be empty function', function () {
                expect($navigationService.goTo).toEqual($navigationService.goHome);
            });
        });

    });
});