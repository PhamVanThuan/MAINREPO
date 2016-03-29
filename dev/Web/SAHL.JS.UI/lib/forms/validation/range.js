'use strict';
angular.module('sahl.js.ui.forms.validation')
.directive('sahlRange', [function () {
    var fromFilter = /from\s*:\s*(-?\d+|\*)/gi;
    var toFilter = /to\s*:\s*(-?\d+|\*)/gi;
    var preFromFilter = /from\s*:/gi;
    var preToFilter = /to\s*:\s*/gi;
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            var data = { from: null, to: null };
            var operations = {
                getValue: function (value, regex) {
                    if (value) {
                        var matchGroups = regex.exec(value);
                        return matchGroups[1] === '*' ? Infinity : Number(matchGroups[1]);
                    }
                    return Infinity;
                },
                validate: function (value) {
                    var num = Number(value);
                    if (num !== isNaN) {
                        ngModel.$setValidity('range', (data.from <= value && value <= data.to));
                    }
                    return value;
                }
            };
            var fromMatch = attrs.sahlRange.match(fromFilter);
            var toMatch = attrs.sahlRange.match(toFilter);
            if ((preFromFilter.test(attrs.sahlRange) && !fromMatch) || (preToFilter.test(attrs.sahlRange) && !toMatch)) {
                ngModel.$setValidity('range', false);
                throw 'Invalid input format, sahl-range=\'from:[ number | * ],to:[ number | * ]\'';
            }
            else {
                if (ngModel) {
                    data.from = operations.getValue(fromMatch, fromFilter);
                    if (data.from === Infinity) {
                        data.from = -Infinity;
                    }
                    data.to = operations.getValue(toMatch, toFilter);
                    ngModel.$parsers.push(operations.validate);
                    ngModel.$formatters.push(operations.validate);
                }
            }
        }
    };
}]);