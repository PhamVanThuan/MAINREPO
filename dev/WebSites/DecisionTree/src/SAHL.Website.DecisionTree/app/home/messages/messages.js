'use strict';

angular.module('sahl.tools.app.home.messages', [
  'ui.router',
  'sahl.tools.app.home.messages.editor',
  'sahl.tools.app.home.messages.info'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.messages', {
        abstract: true,
        url: "/messages",
        templateUrl: "./app/home/messages/messages.tpl.html",
        controller: "MessagesCtrl",
    });
})
.controller("MessagesCtrl", ['$rootScope', '$scope', '$state', '$sessionLockService', '$eventAggregatorService', '$eventDefinitions', '$modalDialogManager', function ($rootScope, $scope, $state, $sessionLockService, $eventAggregatorService, $eventDefinitions, $modalDialogManager) {
    $scope.SelectedMessageSetID = null;
    $scope.ChildFunctions = {};

    $scope.eventHandlers = {
        onPendingLockRelease: function (documentInfo) {
            if (!$scope.LockedBy && $scope.SelectedMessageSetVersionId == documentInfo.DocumentVersionId) {
                $modalDialogManager.dialogs.warnOfSessionTimeout.show().then(function () {
                    $sessionLockService.openLockForVariables(documentInfo.DocumentVersionId, $rootScope.username);
                }, function () {
                    $scope.close();
                });
            }
        }
    };

    $scope.Parent_ShowNavButtons = function (canInfo, canSave, canPublish, canClose) {
        $scope.canInfo = canInfo;
        $scope.canSave = canSave;
        $scope.canPublish = canPublish;
        $scope.canClose = canClose;
    };

    $scope.Parent_RegisterChildFunctions = function (saveFn, publishFn, closeFn) {
        $scope.ChildFunctions.Save = saveFn;
        $scope.ChildFunctions.Publish = publishFn;
        $scope.ChildFunctions.Close = closeFn;
    };

    $scope.Parent_LockLatestVersion = function (latestVersionId) {
        $scope.SelectedMessageSetVersionId = latestVersionId;
        $sessionLockService.openLockForMessages(latestVersionId, $rootScope.username);
    }

    $scope.info = function () {
        $state.transitionTo('home.messages.info');
    };

    $scope.save = function () {
        $scope.ChildFunctions.Save();
    };

    $scope.publish = function () {
        $scope.ChildFunctions.Publish();
    };

    $scope.onChildClose = function () {
        if ($state.current.name == 'home.messages.info') {
            $state.transitionTo('home.messages.editor');
        }
        else if ($state.current.name == 'home.messages.editor') {
            $state.transitionTo('home.start-page');
        }
    };

    $scope.close = function () {
        $eventAggregatorService.unsubscribe($eventDefinitions.onPendingLockRelease, $scope.eventHandlers.onPendingLockRelease);
        if (!$scope.LockedBy) {
            $sessionLockService.closeLockForMessages($scope.SelectedMessageSetVersionId, $rootScope.username);
        }
        $scope.ChildFunctions.Close($scope.onChildClose);
    };

    $scope.Parent_ChangeMessageSetID = function (messageSetID) {
        $scope.SelectedMessageSetID = messageSetID;
        $state.transitionTo('home.messages.editor');
    };

    $eventAggregatorService.subscribe($eventDefinitions.onPendingLockRelease, $scope.eventHandlers.onPendingLockRelease);
}]);
