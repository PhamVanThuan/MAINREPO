'use strict';
describe('[sahl.js.core.routing]', function () {
    beforeEach(module('sahl.js.core.routing'));
    var $uiStateManagerService, stateParams;
    var expectedResult;
    beforeEach(inject(function ($injector, $q) {
        stateParams = {};
        $uiStateManagerService = $injector.get('$uiStateManagerService');
    }));

    describe(' - (Service: uiStateManager)-', function () {
        describe('when model is not set', function () {
            beforeEach(function () {
                $uiStateManagerService.transition(stateParams);
                expectedResult = {};
            });

            it('stateParams should remain unchanged during transition between states', function () {
                expect(stateParams).toEqual(expectedResult);
            });
        });

        describe('when model is set to string', function () {
            beforeEach(function () {
                $uiStateManagerService.set("test");
                $uiStateManagerService.transition(stateParams);
                expectedResult = {};
            });

            it('stateParams should remain unchanged during transition between states', function () {
                expect(stateParams).toEqual(expectedResult);
            });
        });

        describe('when model is set to string', function () {
            beforeEach(function () {
                $uiStateManagerService.set([1,2,3,4]);
                $uiStateManagerService.transition(stateParams);
                expectedResult = {};
            });

            it('stateParams should remain unchanged during transition between states', function () {
                expect(stateParams).toEqual(expectedResult);
            });
        });

        describe('when model is set to object', function () {
            beforeEach(function () {
                $uiStateManagerService.set({ model: {}});
                $uiStateManagerService.transition(stateParams);
                expectedResult = { model: {}};
            });

            it('should add model as property to stateParams object during transition between states', function () {
                expect(stateParams).toEqual(expectedResult);
            });
        });
    });
});