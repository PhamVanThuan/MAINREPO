'use strict';

angular.module('halo.error', [
        'ui.router',
        'sahl.js.core.userManagement'
    ])
    .config([
        '$stateProvider', function($stateProvider) {
            $stateProvider.state('applicationError', {
                url: '/error',
                templateUrl: 'app/error/error.tpl.html',
                controller: 'ErrorCtrl'
            });
        }
    ])
    .controller('ErrorCtrl', [
        '$scope',
        '$userProfileWebService',
        '$userManagerService',
        function($scope, $userProfileWebService,$userManagerService) {
            $scope.user = $userManagerService.getAuthenticatedUser();
            $scope.displayName = authenticatedUser.displayName;
            $scope.getUserProfile = function(size) {
                return $userProfileWebService.webservices.imageurl + '?username=' + $scope.user.adName + '&size=' + size;
            };

        }
    ]);
