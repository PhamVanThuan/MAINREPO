'use strict';
'SAHL.Services.Interfaces.Capitec.commands',
'SAHL.Services.Interfaces.Capitec.queries',
'capitecApp.services'

angular.module('capitecApp.home', [
  'ui.router',
  'capitecApp.home.splashscreen',
  'capitecApp.home.content'
])

.config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state('home', {
        abstract: true,
        templateUrl: 'home.tpl.html',
        controller: 'HomeCtrl',
    });
}])

.controller('HomeCtrl', ['$rootScope', '$scope', '$state', '$cookieService', '$capitecQueries', '$queryManager', function HomeController($rootScope, $scope, $state, $cookieService, $capitecQueries, $queryManager) {
    /** Check user role and set to true if in role admin **/
    var initialiseController = function () {
        queryUserRoles();
    };

    /** Queries the roles for a particular user 
    ** param {Guid} userId
    **/
    var queryUserRoles = function () {
        var getRolesFromUserQuery = new $capitecQueries.GetRolesFromUserQuery($rootScope.userId);
        $queryManager.sendQueryAsync(getRolesFromUserQuery).then(
            function success(data) {
                $scope.roleNames = data.data.ReturnData.Results.$values;
            },
            function failure(error) {
                $scope.roleNames = '';
            }            
        );
    };

    /**Checks if the user is part of admin role**/
    $scope.isInRoleAdmin = function () {
        var isInRole = false;
        angular.forEach($scope.roleNames, function (role) {
            if (role.Name === 'Admin' || role.Name === 'ASuperUser') {
                isInRole = true;
            }
        });
        return isInRole;
    };
    initialiseController();
}]);