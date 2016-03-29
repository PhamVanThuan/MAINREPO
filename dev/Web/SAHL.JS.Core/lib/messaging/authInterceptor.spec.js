'use strict';

describe('[sahl.js.core.messaging]', function () {
    beforeEach(module('sahl.js.core.messaging'));
    
    describe(' - (Factory: authInterceptor)-', function () {
        var authInterceptor, config;
        
        beforeEach(inject(function () {
            config = {
                headers:{
                }
            };
        }));

        describe('when authenticated user is available', function () {
            beforeEach(inject(function ($injector) {
                authInterceptor = $injector.get('$authInterceptor');
            }));

            it('it should add fields to the header', function () {
                authInterceptor.request(config);
                expect(Object.keys(config.headers).length).not.toEqual(0);
            });
        });
        
    });
});