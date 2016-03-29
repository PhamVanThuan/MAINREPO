'use strict';

angular.module('sahl.tools.app.home.enumerations', [
  'ui.router',
  'sahl.tools.app.home.enumerations.editor',
  'sahl.tools.app.home.enumerations.info'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.enumerations', {
        abstract: true,
        url: "/enumerations",
        templateUrl: "./app/home/enumerations/enumerations.tpl.html",
        controller: "EnumerationsCtrl",
    });
})
.controller("EnumerationsCtrl", ['$rootScope', '$scope', '$state', '$sessionLockService', '$eventAggregatorService', '$eventDefinitions','$modalDialogManager', function ($rootScope, $scope, $state, $sessionLockService, $eventAggregatorService, $eventDefinitions, $modalDialogManager) {
    $scope.SelectedEnumerationSetID = null;
    $scope.ChildFunctions = {};

    $scope.eventHandlers = {
        onPendingLockRelease: function (documentInfo) {
            if (!$scope.LockedBy && $scope.SelectedEnumerationSetVersionId == documentInfo.DocumentVersionId) {
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
        $scope.SelectedEnumerationSetVersionId = latestVersionId;
        $sessionLockService.openLockForEnumerations(latestVersionId, $rootScope.username);
    }

    $scope.info = function () {
        $state.transitionTo('home.enumerations.info');
    };

    $scope.save = function () {
        $scope.ChildFunctions.Save();
    };

    $scope.publish = function () {
        $scope.ChildFunctions.Publish();
    };

    $scope.onChildClose = function () {
        if ($state.current.name == 'home.enumerations.info') {
            $state.transitionTo('home.enumerations.editor');
        }
        else if ($state.current.name == 'home.enumerations.editor') {
            $state.go('home.start-page');
        }
    };

    $scope.close = function () {
        $eventAggregatorService.unsubscribe($eventDefinitions.onPendingLockRelease, $scope.eventHandlers.onPendingLockRelease);
        if (!$scope.LockedBy) {
            $sessionLockService.closeLockForEnumerations($scope.SelectedEnumerationSetVersionId, $rootScope.username);
        }
        $scope.ChildFunctions.Close($scope.onChildClose);
    };

    $scope.Parent_ChangeEnumerationSetID = function (enumerationSetID) {
        $scope.SelectedEnumerationSetID = enumerationSetID;
        $state.transitionTo('home.enumerations.editor');
    };

    $eventAggregatorService.subscribe($eventDefinitions.onPendingLockRelease, $scope.eventHandlers.onPendingLockRelease);
}]);