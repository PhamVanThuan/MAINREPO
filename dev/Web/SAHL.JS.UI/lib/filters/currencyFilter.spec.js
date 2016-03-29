'use strict';
describe('sahl.js.ui.filters', function() {
    beforeEach(module('sahl.js.ui.filters'));

    var currencyFilter;
    beforeEach(inject(function($filter) {
        currencyFilter = $filter('currencyFilter');
    }));

    describe(' - (Filter: currency)-', function() {

        it('has a custom currency filter', function() {
            expect(currencyFilter).not.toBeNull();
        });

        describe('given that a number is posivite', function() {

            var amount = 12;
            var expected = 'R12.00';
            it('should add R currency Symbol', function() {
                expect(currencyFilter(amount)).toEqual(expected);
            });
        });

        describe('given that a number is negative', function() {
            var amount = -12;
            var expected = '-R12.00';
            it('should add a currency symbol (R) and negative symbol (-)', function() {
                expect(currencyFilter(amount)).toEqual(expected);
            });
        });

        describe('given that a fractionSize was not specified', function() {
            it('should leave the given number as it was provided', function() {
                var amount = 12;
                var expected = 'R12.00';
                expect(currencyFilter(amount)).toEqual(expected);
            });
        });

        describe('given that a fractionSize of 0 was specified', function() {
            var amount = 12;
            var expected = 'R12';
            it('should remove the decimal places from the given number', function() {
                expect(currencyFilter(amount,0)).toEqual(expected);
            });
        });

    });
});
