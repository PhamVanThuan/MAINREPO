'use strict';
describe('[sahl.js.ui.notifications]', function () {
    beforeEach(module('sahl.js.ui.notifications'));

    var toastManagerService;
    beforeEach(inject(function ($injector, $q) {
        toastManagerService = $injector.get('$toastManagerService');
    }));

    describe(' - (Service: toastManager)-', function () {
        it('service should not be null', function () {
            expect(toastManagerService).not.toEqual(null);
        });

        describe('when calling base notify', function () {
            var result;
            var pnotifyCalled = false;
            var input = {};

            beforeEach(function () {
                window.PNotify = function () {
                    pnotifyCalled = true;
                    return this;
                };
                result = toastManagerService.notice(input);
            });

            it('should return object with promise and toast object', function () {
                expect(pnotifyCalled).toEqual(true);
                expect(result).not.toBe(null);
                expect(result.promise).not.toBe(null);
                expect(result.toast).not.toBe(null);
            });
        });

        describe('when showing a notice toast', function () {
            var result;
            var pnotifyCalled = false;
            var input = {};

            beforeEach(function () {
                window.PNotify = function () {
                    pnotifyCalled = true;
                    return this;
                };
                result = toastManagerService.notice(input);
            });

            it('should return object with promise and toast object', function () {
                expect(pnotifyCalled).toEqual(true);
                expect(result).not.toBe(null);
                expect(result.promise).not.toBe(null);
                expect(result.toast).not.toBe(null);
            });
        });

        describe('when showing a info toast', function () {
            var result;
            var pnotifyCalled = false;
            var input = {};

            beforeEach(function () {
                window.PNotify = function () {
                    pnotifyCalled = true;
                    return this;
                };
                result = toastManagerService.info(input);
            });

            it('should return object with promise and toast object', function () {
                expect(pnotifyCalled).toEqual(true);
                expect(result).not.toBe(null);
                expect(result.promise).not.toBe(null);
                expect(result.toast).not.toBe(null);
            });
        });

        describe('when showing a error toast', function () {
            var result;
            var pnotifyCalled = false;
            var input = {};

            beforeEach(function () {
                window.PNotify = function () {
                    pnotifyCalled = true;
                    return this;
                };
                result = toastManagerService.error(input);
            });

            it('should return object with promise and toast object', function () {
                expect(pnotifyCalled).toEqual(true);
                expect(result).not.toBe(null);
                expect(result.promise).not.toBe(null);
                expect(result.toast).not.toBe(null);
            });
        });

        describe('when showing a success toast', function () {
            var result;
            var pnotifyCalled = false;
            var input = {};

            beforeEach(function () {
                window.PNotify = function () {
                    pnotifyCalled = true;
                    return this;
                };
                result = toastManagerService.success(input);
            });

            it('should return object with promise and toast object', function () {
                expect(pnotifyCalled).toEqual(true);
                expect(result).not.toBe(null);
                expect(result.promise).not.toBe(null);
                expect(result.toast).not.toBe(null);
            });
        });
    });
});
