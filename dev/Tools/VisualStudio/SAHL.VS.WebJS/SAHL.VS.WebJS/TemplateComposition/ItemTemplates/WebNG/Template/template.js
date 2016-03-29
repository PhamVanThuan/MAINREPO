'use strict';
angular.module('%namespace%', [
])
.config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state('%state%', {
        url: '/%ccClassName%',
        templateUrl: '%location%',
        controller: '%pcClassName%Ctrl'
    });
}])

.controller('%pcClassName%Ctrl', ['$scope',function %pcClassName%Controller($scope){

    $scope.$on('$destroy', function () {
    });
}]);