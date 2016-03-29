'use strict';
angular.module('sahl.js.ui.forms.validation')
.directive('errorHint', [function () {
    return {
        restrict: 'A',
        require: ['errorHint', '^form'],
        replace: true,
        scope: true,
        controller: ['$scope', function ($scope) {
            $scope.input = null;
            this.setErrors = function (input,form) {
                $scope.input = input;
                $scope.form = form;
            };
        }],
        link: function (scope, element, attrs, ctrls) {
            var control = ctrls[1][attrs.for];
            ctrls[0].setErrors(control, ctrls[1]);
        }
    };
}]);