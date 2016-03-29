'use strict';

angular.module('sahl.tools.app.home.variables.info', [
  'ui.router',
  'sahl.tools.app.services'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.variables.info', {
        url: "/info",
        templateUrl: "./app/home/variables/info/info.tpl.html",
        controller: 'VariablesInfoCtrl'
    });
})

.controller('VariablesInfoCtrl', ['$scope', '$state', '$activityManager', '$variableDataManager', function ($scope, $state, $activityManager, variableDataManager) {

    $scope.variableSetVersions = [];

    function addFormattedDate(version) {
        if (version.PublishDate) {
            version.FormattedDate = new Date(version.PublishDate).toString();
        }
        return version;
    }

    $scope.load = function () {
        $activityManager.startActivityWithKey('loadVariableVersions');

        variableDataManager.GetAllVariableVersionsQueryAsync().then(function (data) {

            $scope.variableSetVersions = $.map(data.data.ReturnData.Results.$values, addFormattedDate);

            $activityManager.stopActivityWithKey('loadVariableVersions');
        });
    };

    $scope.setSelectedVariableSetID = function (variableSetID) {
        $scope.Parent_ChangeVariableSetID(variableSetID);
    };

    $scope.Parent_ShowNavButtons(false, false, false, false);

    $scope.Parent_RegisterChildFunctions(function () { }, function () { }, function (exitFn) { exitFn(); });

    $scope.load();
}]);