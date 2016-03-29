'use strict';

angular.module('halo.start', [
    'halo.start.portalPages',
    'sahl.js.core.userManagement'
])
.config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state('start', {
        url: '/',
        templateUrl: 'app/start/start.tpl.html',
        controller: 'StartCtrl'
    });
}])
.controller('StartCtrl', ['$state', function ($state){

}]);
