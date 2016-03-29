'use strict';

angular.module('halo.start.portalpages.entity.common.page', [
    'sahl.js.core.activityManagement',
    'sahl.js.ui.forms.validation',
    'sahl.js.ui.forms'
])
.config([
    '$stateProvider', function ($stateProvider) {
        var state = function () {
            this.url = '/page';
            this.templateUrl = 'app/start/portalPages/entity/common/page/page.tpl.html';
            this.controller = 'PageCtrl';
        };
        _.each(['client', 'task', 'thirdparty'], function (entity) {
            $stateProvider.state('start.portalPages.' + entity + '.common.page', new state());
        });
    }
])
.controller('PageCtrl', ['$scope', '$rootScope', '$viewManagerService', '$state', '$stateParams','$pageFactory',
function TileEditController($scope, $rootScope, $viewManagerService, $state, $stateParams, $pageFactory) {
    $scope.pageView = $viewManagerService.getPageView($stateParams.model.nonTilePageState);
    $pageFactory.setViewData($stateParams.previousState);
    $scope.cancel = $pageFactory.back;

    $scope.$on('$destroy', function () {
        $scope.pageView = null;
    });
}]);
