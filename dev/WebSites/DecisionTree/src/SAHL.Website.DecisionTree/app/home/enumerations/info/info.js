'use strict';

angular.module('sahl.tools.app.home.enumerations.info', [
  'ui.router',
  'sahl.tools.app.services'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.enumerations.info', {
        url: "/info",
        templateUrl: "./app/home/enumerations/info/info.tpl.html",
        controller: 'EnumerationsInfoCtrl'
    });
})

.controller('EnumerationsInfoCtrl', ['$scope', '$state', '$activityManager', '$enumerationDataManager', function ($scope, $state, $activityManager, enumerationDataManager) {

    $scope.enumerationSetVersions = [];

    function addFormattedDate(version) {
        if (version.PublishDate) {
            version.FormattedDate = new Date(version.PublishDate).toString();
        }
        return version;
    }

    $scope.load = function () {
        $activityManager.startActivityWithKey('loadEnumerationVersions');

        enumerationDataManager.GetAllEnumerationVersionsQueryAsync().then(function (data) {

            $scope.enumerationSetVersions = $.map(data.data.ReturnData.Results.$values, addFormattedDate);

            $activityManager.stopActivityWithKey('loadEnumerationVersions');
        });
    };

    $scope.setSelectedEnumerationSetID = function (enumerationSetID) {
        $scope.Parent_ChangeEnumerationSetID(enumerationSetID);
    };

    $scope.Parent_ShowNavButtons(false, false, false, false);

    $scope.Parent_RegisterChildFunctions(function () { }, function () { }, function (exitFn) { exitFn(); });

    $scope.load();
}]);
