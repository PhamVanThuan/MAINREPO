'use strict';
describe('[sahl.js.core.fluentDecorator]', function () {
    var $fluentDecorator, $fluentDecoratorInjected, $mockService, $stackedMockService;

    var mock = {
        test:function(){
            return "mock";
        }
    };

    var stackedMock = {
        test: function () {
            return "mock";
        }
    };

    var mockDecorator = function ($delegate) {
        return {
            test: function () {
                return $delegate.test() + " decorator";
            }
        };
    };

    beforeEach(function(){
        var fakeModule = angular.module('test.sahl.js.core.fluentDecorator', ['sahl.js.core.fluentDecorator'], function ($provide) {
            $provide.value('mock', mock);
            $provide.value('stackedMock', stackedMock);
        });
        fakeModule.config(function ($fluentDecoratorProvider) {
            $fluentDecorator = $fluentDecoratorProvider;

            $fluentDecorator.decorate('mock').with(mockDecorator);
            $fluentDecorator.decorate('stackedMock').with(mockDecorator).with(mockDecorator).with(mockDecorator).with(mockDecorator).with(mockDecorator);
        });
    });
    
    beforeEach(module('sahl.js.core.fluentDecorator'));
    beforeEach(module('test.sahl.js.core.fluentDecorator'));
    
    beforeEach(inject(function ($injector) {
        $fluentDecoratorInjected = $injector.get('$fluentDecorator');
        $mockService = $injector.get('mock');
        $stackedMockService = $injector.get('stackedMock');
    }));
    
    describe('when using decorator outside of app config', function () {
        it('it should return empty object', function () {
            expect($fluentDecoratorInjected).not.toEqual(undefined);
            expect(typeof $fluentDecoratorInjected).toEqual("object");
        });
    });
    
    describe('when using decorator inside of app config', function () {
        it('it should only provide within config', function () {
            expect($fluentDecorator).not.toEqual(undefined);
            expect($fluentDecorator.decorate).toBeDefined();
            
        });

        it('it should decorate mock service',function(){
            expect($mockService.test()).toEqual("mock decorator"); 
        });

        it('it should decorate mock service multiple times', function () {
            expect($fluentDecorator).not.toEqual(undefined);
            expect($fluentDecorator.decorate).not.toEqual(undefined);
            expect($stackedMockService.test()).toEqual("mock decorator decorator decorator decorator decorator");
        });
    });
});
