'use strict';

angular.module('sahl.tools.app.home.notauthenticated', [
  'ui.router'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.notauthenticated', {
        url: "/notauthenticated",
        templateUrl: "./app/home/notauthenticated/notauthenticated.tpl.html",
        controller: "NotAuthenticatedCtrl",
    });
})

.controller('NotAuthenticatedCtrl', ['$rootScope', '$scope', '$state', function HomeController($rootScope, $scope, $state) {

}]);
