'use strict';
angular.module('sahl.halo.app.variables.start', [
])
.config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state('start.portalPages.apps.variables', {
        url: 'variables',
        templateUrl: 'app/start/start.tpl.html',
        controller: 'VariablesStartCtrl'
    });
}])
.controller('VariablesStartCtrl', ['$scope', function StartController($scope) {
    $scope.one = 1;
    $scope.$on('$destroy', function () {
        delete $scope.one;
    });
}]);