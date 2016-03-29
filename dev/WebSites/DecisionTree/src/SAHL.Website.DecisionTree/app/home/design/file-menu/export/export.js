'use strict';

angular.module('sahl.tools.app.home.design.file-menu.export', [
  'ui.router',
  'sahl.tools.app.services',
  'sahl.tools.app.home.design.file-menu.export.pdf',
  'sahl.tools.app.home.design.file-menu.export.png',
  'sahl.tools.app.home.design.file-menu.export.svg'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.design.file-menu.export', {
        url: "/export",
        templateUrl: "./app/home/design/file-menu/export/export.tpl.html",
        controller: 'FileExportCtrl'
    });
})

.controller('FileExportCtrl', ['$rootScope', '$scope', '$state', '$keyboardManager', '$documentManager', '$stateParams', '$designSurfaceManager', '$eventAggregatorService', '$eventDefinitions',
    function FileExportCtrl($rootScope, $scope, $state, $keyboardManager, $documentManager, $stateParams, $designSurfaceManager, $eventAggregatorService, $eventDefinitions) {

    }]);