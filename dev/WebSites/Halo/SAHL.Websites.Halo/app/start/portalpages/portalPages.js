'use strict';
angular.module('halo.start.portalPages', [
    'sahl.websites.halo.services.entityManagement',
    'halo.start.portalpages.entity',
    'halo.start.portalpages.myhalo',
    'halo.start.portalpages.search',
    'halo.start.portalpages.tasks',
    'halo.start.portalpages.wipbar',
    'halo.start.portalpages.settings'
])
.config(['$stateProvider',
    function ($stateProvider) {
        $stateProvider.state('start.portalPages', {
            abstract: true,
            templateUrl: 'app/start/portalPages/portalPages.tpl.html',
            controller: 'PortalPageCtrl'
        });
    }
])
.controller('PortalPageCtrl', ['$scope', '$rootScope', '$logger', '$entityManagerService', '$tileManagerService', '$document', '$timeout', '$eventAggregatorService', '$entityManagementEvents',
    function ($scope, $rootScope, $logger, $entityManager, $tileManagerService, $document, $timeout, $eventAggregatorService, $entityManagementEvents) {

      $tileManagerService.loadApplicationConfiguration('Home').then(function (applicationConfiguration) {
          $rootScope.tileApplicationConfiguration = applicationConfiguration;
      }, function () {
          throw 'Unable to load the Home Application configuration';
      });            
    }
]);
