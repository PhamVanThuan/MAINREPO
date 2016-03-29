'use strict';

angular.module('sahl.tools.app.home', [
  'ui.router',
  'sahl.tools.app.home.start-page',
  'sahl.tools.app.home.design',
  'sahl.tools.app.home.messages',
  'sahl.tools.app.home.variables',
  'sahl.tools.app.home.enumerations'
])

.config(function config($stateProvider) {
    $stateProvider.state('home', {
        abstract:true,
        templateUrl: "./app/home/home.tpl.html",
        controller: "HomeCtrl",
    });
})

.controller('HomeCtrl', ['$rootScope', '$scope', '$state', function HomeController($rootScope, $scope, $state) {
    $scope.logoff = function () {
        $rootScope.authenticated = false;
        $rootScope.userAuthToken = '';
        $rootScope.userDisplayName = "";
        $rootScope.userRoles = [];
        $rootScope.username = "";

        //$state.transitionTo('login');
    }
}]);
