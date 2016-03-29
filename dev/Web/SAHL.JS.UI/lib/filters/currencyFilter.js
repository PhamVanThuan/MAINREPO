'use strict';
angular.module('sahl.js.ui.filters')
    
.filter('currencyFilter', ['$filter', function($filter) {
        return function(amount, fractionSize) {
            var currencyFilter = $filter('currency');
            var currencySymbol = 'R';
            var currencyAmount = currencyFilter(amount, currencySymbol,fractionSize);

            if (amount < 0) {
                return currencyAmount.replace('(', '-').replace(')', '');
            }
            return currencyAmount;
        };
    }]);