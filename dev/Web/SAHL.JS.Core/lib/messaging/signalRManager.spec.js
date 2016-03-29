'use strict';

describe('[sahl.js.core.signalR]', function() {
    beforeEach(module('sahl.js.core.signalR'));
	beforeEach(module('sahl.js.core.logging'));

    describe(' - (Service: $signalRFactory)-', function () {
        var $, $rootScope, $q, $signalRHubManager;

        beforeEach(inject(function($injector) {
            $rootScope = $injector.get('$rootScope');
            $q = $injector.get('$q');
            $signalRHubManager = $injector.get('$signalRHubManager');
            //$ = $injector.get('$');

            //spyOn($.connection, 'start').and.returnValue()
        }));

        describe('when creating a new hub', function() {
            it('should create a signalR Hub', function() {
                var hub = new $signalRHubManager('http://localhost', 'testHub', { transport: 'longPolling' });
                expect(hub).toBeDefined();
            });
        });

        describe('when calling start', function() {
            it('should create connection to specified proxy', function() {
                console.log('jQuery', $);
            });
        });
    });
});
