'use strict';
angular.module('sahl.js.ui.forms.decorators', [])
.directive('sahlTag', [function () {
    return {
        restrict: 'AC',
        scope: {
            ngModel: '='
        },
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            angular.element(element).css('background', scope.ngModel.background);
            angular.element(element).css('color', scope.ngModel.forground);
        }
    };
}]);