'use strict';
describe('[sahl.js.core.messaging]', function () {
    beforeEach(module('sahl.js.core.messaging'));
    var $rootScope,$activityDecorator, $activityDecoratorInstance, $activityManager, $decoratorInstance;
    beforeEach(function () {
        var fakeModule = angular.module('test.sahl.js.core.messaging', ['sahl.js.core.messaging'], function () {
        });

        fakeModule.config(function ($activityDecoratorProvider) {
            $activityDecorator = $activityDecoratorProvider;
        });

        module('sahl.js.core.messaging', 'test.sahl.js.core.messaging');
    });

    beforeEach(function () {
        inject(function ($injector, $q) {
            $rootScope = $injector.get('$rootScope');
            $activityDecoratorInstance = $injector.get('$activityDecorator');
            $activityManager = $injector.get('$activityManager');
        });
    });

    describe(' - (Provider: $activityDecorator)-', function () {
        describe('when using decorator outside of app config', function () {
            it('it return undefined object', function () {
                expect($activityDecoratorInstance).toEqual(undefined);
            });
        });

        describe('when using decorator inside of app config', function () {
            var deferred, decoration;
            beforeEach(function () {
                var delegate;
                inject(function ($injector, $q) {
                     delegate = {
                        postMessage: function () {
                            deferred = $q.defer();
                            return deferred.promise;
                        }
                     };
                     decoration = $activityDecorator.decoration[3](delegate, $q, $activityManager);
                });
                
                spyOn($activityManager, 'startActivity');
                spyOn($activityManager, 'stopActivity');
            });
            describe('whenwhen $delegates promise is resolved', function () {
                it('when $delegates promise is resolved', function () {
                    decoration.postMessage({}, '');
                    deferred.resolve('');
                    $rootScope.$digest();
                    expect($activityManager.startActivity).toHaveBeenCalled();
                    expect($activityManager.stopActivity).toHaveBeenCalled();
                });

                it('when $delegates promise is rejected', function () {
                    decoration.postMessage({}, '');
                    deferred.reject('');
                    $rootScope.$digest();
                    expect($activityManager.startActivity).toHaveBeenCalled();
                    expect($activityManager.stopActivity).toHaveBeenCalled();
                });
            });
        });
    });
});