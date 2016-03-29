'use strict';
describe('[sahl.js.core.routing]', function () {
    beforeEach(module('sahl.js.core.routing'));
    var $uiStateDecorator, $uiStateDecoratorInstance;
    var $delegate, $uiStateManagerService, $rootScope;

    beforeEach(function () {
        var fakeModule = angular.module('test.sahl.js.core.routing', ['sahl.js.core.routing'], function () {
        });

        fakeModule.config(function ($uiStateDecoratorProvider) {
            $uiStateDecorator = $uiStateDecoratorProvider;
        });

        module('sahl.js.core.routing', 'test.sahl.js.core.routing');
    });
    
    beforeEach(inject(function ($injector, $q) {
        $uiStateDecoratorInstance = $injector.get('$uiStateDecorator');
        $delegate = {};
        $uiStateManagerService = {};
        $rootScope = {};
    }));

    describe(' - (Service: uiStateDecorator)-', function () {
        describe('when using decorator outside of app config', function () {
            it('it return undefined object', function () {
                expect($uiStateDecoratorInstance).toEqual(undefined);
            });
        });

        describe('when using decorator inside of app config', function () {
            it('it should return object with a decoration property', function () {
                expect($uiStateDecorator).not.toEqual(undefined);
                expect($uiStateDecorator.decoration).not.toEqual(undefined);
            });

            describe('when providing a delegate', function () {
                var delegate, stateManager, scope, decorated;
                beforeEach(function () {
                    delegate = {
                        goCalled:false,
                        go: function () {
                            delegate.goCalled = true;
                        }
                    };
                    stateManager = {
                        setCalledWith:null,
                        setCalled: false,
                        transition: function () { },
                        set: function () {
                            stateManager.setCalled = true;
                            stateManager.setCalledWith = arguments[0];
                        }
                    };
                    scope = {
                        $on: function(){}
                    };
                    decorated = $uiStateDecorator.decoration[3](delegate, stateManager, scope);
                });

                it('it should decorate only the go function', function () {
                    var input = {test:1};
                    decorated.go('test', input);
                    expect(delegate.goCalled).toEqual(true);
                    expect(stateManager.setCalled).toEqual(true);
                    expect(stateManager.setCalledWith).toEqual(input);
                });
            });
        });
    });
});