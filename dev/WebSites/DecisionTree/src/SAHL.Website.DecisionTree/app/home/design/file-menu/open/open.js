'use strict';

angular.module('sahl.tools.app.home.design.file-menu.open', [
  'ui.router',
  'sahl.tools.app.services',
  'sahl.tools.app.home.design.file-menu.open.recent',
  'sahl.tools.app.home.design.file-menu.open.all'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.design.file-menu.open', {
        url: "/open",
        templateUrl: "./app/home/design/file-menu/open/open.tpl.html",
        controller: 'FileOpenCtrl'
    });
})

.controller('FileOpenCtrl', ['$rootScope', '$scope', '$state', '$keyboardManager', '$documentManager', '$stateParams', '$activityManager', '$queryManager', '$decisionTreeDesignQueries',
    function FileOpenCtrl($rootScope, $scope, $state, $keyboardManager, $documentManager, $stateParams, $activityManager, $queryManager, $decisionTreeDesignQueries) {


    }]);