'use strict';

angular.module('sahl.tools.app.home.design.file-menu.print', [
  'ui.router',
  'sahl.tools.app.services'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.design.file-menu.print', {
        url: "/print",
        templateUrl: "./app/home/design/file-menu/print/print.tpl.html",
        controller: 'FilePrintCtrl'
    });
})

.controller('FilePrintCtrl', ['$rootScope', '$scope', '$state', '$keyboardManager', '$documentManager', '$stateParams',
    function FilePrintCtrl($rootScope, $scope, $state, $keyboardManager, $documentManager, $stateParams) {

    }]);