'use strict';
describe('[sahl.js.core.fluentRestQuery]', function () {
    var orderByDirectionBuilder;
    var returnedValue;
    beforeEach(module('sahl.js.core.fluentRestQuery'));
    beforeEach(inject(function ($orderByDirectionBuilder) {
        orderByDirectionBuilder = new $orderByDirectionBuilder(function(value){
            returnedValue = value;
        });
    }));
    describe('when choosing a direction', function () {
        describe('- desc', function () {
            beforeEach(function () {
                orderByDirectionBuilder.descending();
            });
            it('should return to callback function with "desc"', function () {

                expect(returnedValue).toEqual('desc');
            });
        });
    });
});