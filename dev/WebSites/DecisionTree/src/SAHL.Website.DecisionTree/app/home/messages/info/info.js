'use strict';

angular.module('sahl.tools.app.home.messages.info', [
  'ui.router',
  'sahl.tools.app.services'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.messages.info', {
        url: "/info",
        templateUrl: "./app/home/messages/info/info.tpl.html",
        controller: 'MessageInfoCtrl'
    });
})

.controller('MessageInfoCtrl', ['$scope', '$state', '$activityManager', 'messageDataManager', function ($scope, $state, $activityManager, messageDataManager) {

    $scope.messageSetVersions = [];

    function addFormattedDate(version) {
        if (version.PublishDate) {
            version.FormattedDate = new Date(version.PublishDate).toString();
        }
        return version;
    }

    $scope.load = function () {
        $activityManager.startActivityWithKey('loadMessageVersions');

        messageDataManager.GetAllMessageVersionsQueryAsync().then(function (data) {
            
            $scope.messageSetVersions = $.map(data.data.ReturnData.Results.$values, addFormattedDate);

            $activityManager.stopActivityWithKey('loadMessageVersions');
        });
    };

    $scope.setSelectedMessageSetID = function (messageSetId) {
        $scope.Parent_ChangeMessageSetID(messageSetId);
    };

    $scope.Parent_ShowNavButtons(false, false, false, false);

    $scope.Parent_RegisterChildFunctions(function () { }, function () { }, function (exitFn) { exitFn(); });

    $scope.load();

}]);