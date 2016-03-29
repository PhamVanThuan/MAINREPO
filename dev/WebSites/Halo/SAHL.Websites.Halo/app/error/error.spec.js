'use strict';

describe('[halo.error]', function () {
    beforeEach(module('halo.error'));

    describe(' - (ErrorCtrl) - ', function () {
        it('should set stateProvider error state', function () {
            var state;
            inject(function ($state) {
                state = $state;
            });

            var startState = state.get('applicationError');

            expect(startState).not.toBeNull();
            expect(startState.name).toEqual('applicationError');
            expect(startState.url).toEqual('/error');
            expect(startState.templateUrl).toEqual('app/error/error.tpl.html');
            expect(startState.controller).toEqual('ErrorCtrl');
        });
    });
});
