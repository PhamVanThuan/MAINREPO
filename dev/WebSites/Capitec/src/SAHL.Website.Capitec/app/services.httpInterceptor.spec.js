describe("[capitecApp]", function () {
    beforeEach(module('capitecApp.services'));

    //testing the services
    describe(' - (CapitecAppServices http Interceptor) - ', function () {
        var $httpBackend, $http, $serviceConfig, $rootScope, interceptor, wrappedPromise, notifier, rejection, notifyErrorEvent, retryEvent;

        beforeEach(inject(function ($injector) {
            interceptor = $injector.get('$httpInterceptor');
            notifier = $injector.get('$notificationService');
            $http = $injector.get("$http");
            $httpBackend = $injector.get("$httpBackend");
            $serviceConfig = $injector.get('$serviceConfig');
            $rootScope = $injector.get('$rootScope');

            wrappedPromise = {};
            spyOn(notifier, 'notifyError');
            spyOn(notifier, 'notifyInfo');
        }));

        afterEach(function () {
            $httpBackend.verifyNoOutstandingExpectation();
            $httpBackend.verifyNoOutstandingRequest();
        });

        describe('using the httpInterceptor,', function () {
            it('should have a responseError method that accepts an http rejection', function () {
                expect(interceptor.responseError).toBeDefined();
            });

            it('should not intercept non-http errors', function () {
                $httpBackend.when('GET', "/").respond({
                    status: 301
                });

                spyOn(interceptor, 'responseError').and.callThrough();

                $http.get("/").then(function () {
                    expect(interceptor.responseError).not.toHaveBeenCalled();
                });
                $httpBackend.flush();
            });

            it('should notify presence of http errors', function () {
                rejection = {
                    status: 403,
                    config: {},
                    data: {}
                };
                interceptor.responseError(rejection);
                expect(notifier.notifyError).toHaveBeenCalled();
            });

            it('should notify information messages on successful requests', function() {
                response = {
                    status: 200,
                    config: {},
                    data: {
                        SystemMessages: {
                            AllMessages: {
                                $values: [{Message: 'Extra information', Severity : 2}]
                            }
                        }
                    }
                };
                interceptor.response(response);
                expect(notifier.notifyInfo).toHaveBeenCalledWith('Info', 'Extra information');
            });

            it('accepts a request timeout error and retries request', function () {
                $httpBackend.whenPOST('http://localhost/').respond(200, { foo: 'bar' });
                rejection = {
                    status: 0,
                    config: { method: 'POST', url: 'http://localhost/' },
                    data: {}
                };

                var new_promise = interceptor.responseError(rejection);
                $httpBackend.flush();

                var retry_result;
                new_promise.then(function (success_data) {
                    retry_result = success_data
                    retry_result.$response = 'success';
                },
                function (error_data) {
                    retry_result = error_data;
                    retry_result.$response = 'rejection';
                });

                $rootScope.$apply();
                expect(retry_result.data).toEqual({ foo: 'bar' });
                expect(retry_result.status).toEqual(200);
                expect(retry_result.$response).toEqual('success');
            });

            it('accepts a request timeout error and rejects the retry request if it times out', function () {
                $httpBackend.whenPOST('http://localhost/').respond(0);
                rejection = {
                    status: 0,
                    config: { method: 'POST', url: 'http://localhost/' },
                    data: {}
                };

                var new_promise = interceptor.responseError(rejection);
                $httpBackend.flush();

                var retry_result;
                new_promise.then(function (success_data) {
                    retry_result = success_data
                    retry_result.$response = 'success';
                },
                function (error_data) {
                    retry_result = error_data;
                    retry_result.$response = 'rejection';
                });

                $rootScope.$apply();
                expect(retry_result.data).toBeUndefined();
                expect(retry_result.status).toEqual(0);
                expect(retry_result.$response).toEqual('rejection');
            });

            it('should inform when retrying', function () {
                $httpBackend.whenPOST('http://localhost/').respond(200, { foo: 'bar' });
                rejection = {
                    status: 0,
                    config: { method: 'POST', url: 'http://localhost/' },
                    data: {}
                };
                interceptor.responseError(rejection);
                $httpBackend.flush();
                expect(notifier.notifyInfo).toHaveBeenCalled();
            });

            it('should accept any non-timeout http error and notifies user', function () {
                rejection = {
                    status: 401,
                    config: {},
                    data: {}
                };
                var newPromise = interceptor.responseError(rejection);

                newPromise.then(function (callback, errback, progressback) {

                });
                expect(notifier.notifyError).toHaveBeenCalled();
            });

            it('should use the message from the http service in notifications', function () {
                var wrappedData = {
                    SystemMessages: {
                        AllMessages: { $values: new Array({ Message: 'Access Denied.', Severity: 1 }) }
                    }
                };
                rejection = {
                    status: 401,
                    config: {},
                    data: wrappedData
                };

                var newPromise = interceptor.responseError(rejection);

                expect(notifier.notifyError).toHaveBeenCalled();
                expect(notifier.notifyError.calls.mostRecent().args.length).toEqual(2);
                expect(notifier.notifyError.calls.mostRecent().args[1]).toEqual(wrappedData.SystemMessages.AllMessages.$values[0].Message);
            });
        });
    });

    describe('- Handling a network timeout -', function () {
        var $httpBackend, $http, $httpProvider, $timeout, $q, interceptor, wrappedPromise, rejection;

        //beforeEach(module('capitecApp'));

        beforeEach(inject(function ($injector) {
            $q = $injector.get('$q');
            interceptor = $injector.get('$httpInterceptor');
            $http = $injector.get("$http");
            $httpBackend = $injector.get("$httpBackend");
            $timeout = $injector.get("$timeout");

            var fakeModule = angular.module('capitecApp', function () { });
            fakeModule.config(['$httpProvider', function ($httpProvider) {
                $httpProvider.defaults.timeout = 500;
                $httpProvider.decorator('$httpBackend', function ($delegate) {
                    var proxy = function (method, url, data, callback, headers) {
                        var interceptor = function () {
                            var _this = this,
                                _arguments = arguments;
                            $timeout(function () {
                                callback.apply(_this, _arguments);
                            }, 700);
                        };
                        return $delegate.call(this, method, url, data, interceptor, headers);
                    };
                    for (var key in $delegate) {
                        proxy[key] = $delegate[key];
                    }
                    return proxy;
                });
            }]);

            wrappedPromise = {};
            spyOn(localStorage, 'setItem');
        }));

        afterEach(function () {
            $httpBackend.verifyNoOutstandingExpectation();
            $httpBackend.verifyNoOutstandingRequest();
        });

        it('use local storage ..', function () {
            $httpBackend.when('GET', "/").respond(wrappedPromise);
            $http.get("/");
            $timeout.flush();
            $httpBackend.flush();
            //check that the promise was never returned due to the timeout
            //might need to move this into the apply controller, to have an actual results to test against. i.e. check that the application number did not return when clicking apply as we
            //expected the timeout to return
            //expect(ApplicationNumber).toBe(null);
            //expect(localStorage.setItem).toHaveBeenCalled();
        });
    });
});