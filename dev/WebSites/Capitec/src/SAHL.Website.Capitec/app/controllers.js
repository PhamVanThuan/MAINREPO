'use strict';

angular.module('capitecApp.controllers', [
  'ui.router'
])

/* Shared Controllers */

.controller('CalculationResultCtrl', ['$scope', '$state', '$activityManager', '$calculatorDataService', '$timeout',  function CalculationResultCtrl($scope, $state, $activityManager, $calculatorDataService, $timeout) {
    if ($scope.calculationResult == null)
    {
        $scope.calculationResult = $calculatorDataService.getDataValue('calculationResult');
        if ($scope.calculationResult == null) {
            $state.transitionTo($state.$current.parent.name);
        }
    }

    if ($scope.calculationResult.calculator == 'stilllooking') {
        $state.current.data.pageHeading = 'calculation results - subject to full credit assessment';
        $state.current.data.icon = 'false';
    }

    $scope.back = function () {
        $timeout(function () {
            $activityManager.stopActivity();
            $state.transitionTo($state.$current.parent.name);
        });     
    };

    $scope.apply = function () {
        $state.transitionTo($state.current.data.next);
    };
}]).
controller('CalculationResultFailCtrl', ['$scope', '$state', '$activityManager', function ($scope, $state, $activityManager) {
    $scope.back = function () {
        $activityManager.stopActivity();
        $state.transitionTo($state.$current.parent.name);
    };
}]);