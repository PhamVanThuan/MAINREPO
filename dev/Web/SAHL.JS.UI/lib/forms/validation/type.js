'use strict';
angular.module('sahl.js.ui.forms.validation')
.directive('sahlType', [function () {
    return {
        
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            var data = {
                restriction: null
            };
            var operations = {
                validate: function (value) {
                    if (value) {
                        if (data.restriction === "number") {
                            ngModel.$setValidity('type', (!isNaN(value)));
                        }
                        else{
                            ngModel.$setValidity('type', true);
                        }
                    }
                    return value;
                }
            };

            data.restriction = attrs.sahlType;

            if (ngModel) {
                ngModel.$parsers.push(operations.validate);
                ngModel.$formatters.push(operations.validate);
            }
        }
    };
}]);