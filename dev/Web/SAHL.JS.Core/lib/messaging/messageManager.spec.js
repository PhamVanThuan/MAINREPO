'use strict';
describe('[sahl.js.core.messaging]', function () {
    beforeEach(module('sahl.js.core.messaging'));
    var $messageManager, $q;

    var expectedResult = {
        id: 1
    };

    beforeEach(inject(function ($injector, $q) {
        $messageManager = $injector.get('$messageManager');
        $q = $injector.get('$q');
    }));

    describe(' - (Factory: messageManager)-', function () {
        var command, url, deferred;
        beforeEach(inject(function ($injector, $q) {
            url = '/test';
            deferred = $q.defer();
            spyOn($q, 'defer').and.returnValue(deferred);
        }));

        describe('given that a command needs to be sent', function () {
            var _$httpBackend;
            beforeEach(inject(function ($injector, $q, $httpBackend) {
                _$httpBackend = $httpBackend;
                _$httpBackend.whenPOST(url, command).respond(200, expectedResult);
            }));

            it('should post command and return expected data', function () {
                var result;
                $messageManager.postMessage(command, url).then(function (data) {
                    result = data;
                });
                _$httpBackend.flush();
                expect(result.data).toEqual(expectedResult);
                expect(result.status).toEqual(200);
            });
        });

        describe('given that a service is unavailable', function () {
            var _$httpBackend;
            beforeEach(inject(function ($injector, $q, $httpBackend) {
                _$httpBackend = $httpBackend;
                _$httpBackend.whenPOST(url, command).respond(500, {});
            }));
            it('should try and post command an fail', function () {
                var result;
                $messageManager.postMessage(command, url).then(function () {
                }, function (error) {
                    result = error;
                });
                _$httpBackend.flush();
                expect(result.status).toEqual(500);
            });
        });

        describe('given a restful query that needs to be sent', function () {
            var _$httpBackend;
            var expected = {id: 121};
            beforeEach(inject(function ($injector, $q, $httpBackend) {
                _$httpBackend = $httpBackend;
                _$httpBackend.whenGET(url).respond(200, expected);
            }));
            it('should call to the service', function () {
                var result;
                $messageManager.getMessage(url).then(function (data) {
                    result = data;
                });
                _$httpBackend.flush();
                expect(result.data).toEqual(expected);
            });
        });
    });
});

