'use strict';

angular.module('halo.start.portalpages.entity.common', [
    'halo.start.portalpages.entity.common.edit',
    'halo.start.portalpages.entity.common.page',
    'halo.start.portalpages.entity.common.wizard',
    'sahl.js.ui.pages'
])
.config([
    '$stateProvider',
    function ($stateProvider) {
        var stateSettings = function () {
            this.abstract = true;
            this.templateUrl = 'app/start/portalPages/entity/common/common.tpl.html';
            this.controller = 'CommonEntityCtrl';
        };
        _.each(['client', 'task', 'thirdparty'], function (entity) {
            $stateProvider.state(
                'start.portalPages.' + entity + '.common',
                new stateSettings()
            );
        });
    }
])
.controller('CommonEntityCtrl', ['$scope', '$pageFactory',
function CommonEntityController($scope,$pageFactory) {
    $scope.cancel = $pageFactory.back;
}]);
