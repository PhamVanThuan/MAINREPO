angular.module('halo.charms.userCharms', [
    'sahl.js.core.applicationModules'
])
.controller('UserCharmsCtrl', ['$scope', '$charmManagerService',
    function ($scope, $charmManagerService) {
        $scope.charms = $charmManagerService.getCharmsForGroup('user');
    }]);