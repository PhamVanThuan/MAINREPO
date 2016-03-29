'use strict';
describe('[sahl.js.core.applicationModules]', function () {
    beforeEach(module('sahl.js.core.applicationModules'));
    var $charmManagerService;
    beforeEach(inject(function ($injector) {
        $charmManagerService = $injector.get('$charmManagerService');
    }));

    describe(' - (Service: charmManager)-', function () {
        describe(' - when registering a charm -', function () {
            it('should not throw error with empty parameters', function () {
                expect(function () {
                    $charmManagerService.registerCharm();
                }).not.toThrow();
            });

            it('should not throw error with parameters', function () {
                expect(function(){
                    $charmManagerService.registerCharm('test','test','test','test','test','test','test');
                }).not.toThrow();
            });
        });

        describe(' - when getting charms for group that does not exist', function () {
            var result;
            it('should return empty undefined', function () {
                result = $charmManagerService.getCharmsForGroup('test');
                expect(result).toEqual(undefined);
            });
        });

        describe(' - when registering a charm for group "test" -', function () {
            var result;
            beforeEach(inject(function ($injector) {
                $charmManagerService.registerCharm('test', 'test', 'test', 'test', 'test', 'test', 'test');
            }));

            it('should return test group', function () {
                result = $charmManagerService.getCharmsForGroup('test');
                expect(result).not.toEqual(undefined);
                expect(result.length).toEqual(1);
            });
        });

        describe(' - when registering charms for group "test" -', function () {
            var result;
            beforeEach(inject(function ($injector) {
                $charmManagerService.registerCharm('test', 'test', 'test', 'test', 'test', 'test', 'test');
                $charmManagerService.registerCharm('test', 'test', 'test', 'test', 'test', 'test', 'test');
            }));

            it('should return expected test group', function () {
                result = $charmManagerService.getCharmsForGroup('test');
                expect(result).not.toEqual(undefined);
                expect(result.length).toEqual(2);
            });
        });

        describe(' - when registering charms for group "test" -', function () {
            var result;
            beforeEach(inject(function ($injector) {
                $charmManagerService.registerCharm('test1', 'test', 'test', 'test', 'test', 'test', 'test');
                $charmManagerService.registerCharm('test2', 'test', 'test', 'test', 'test', 'test', 'test');
            }));

            it('should return test1 group', function () {
                result = $charmManagerService.getCharmsForGroup('test1');
                expect(result).not.toEqual(undefined);
                expect(result.length).toEqual(1);
            });
        });
    });
});