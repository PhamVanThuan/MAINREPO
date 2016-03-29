'use strict';

angular.module('sahl.tools.app.home.variables', [
  'ui.router',
  'sahl.tools.app.home.variables.editor',
  'sahl.tools.app.home.variables.info'
])
.config(function config($stateProvider) {
    $stateProvider.state('home.variables', {
        abstract: true,
        url: "/variables",
        templateUrl: "./app/home/variables/variables.tpl.html",
        controller: "VariablesCtrl",
    });
})
.controller("VariablesCtrl", ['$rootScope', '$scope', '$state', '$sessionLockService', '$eventAggregatorService', '$eventDefinitions', '$modalDialogManager', function ($rootScope, $scope, $state, $sessionLockService, $eventAggregatorService, $eventDefinitions, $modalDialogManager) {
    $scope.eventHandlers = {
        onPendingLockRelease: function (documentInfo) {
            if (!$scope.LockedBy && $scope.SelectedVariableSetVersionId == documentInfo.DocumentVersionId) {
                $modalDialogManager.dialogs.warnOfSessionTimeout.show().then(function () {
                    $sessionLockService.openLockForVariables(documentInfo.DocumentVersionId, $rootScope.username);
                }, function () {
                    $scope.close();
                });
            }
        }
    };

    $scope.SelectedVariableSetID = null;

    $scope.ChildFunctions = {};

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
        $scope.SelectedVariableSetVersionId = latestVersionId;
        $sessionLockService.openLockForVariables(latestVersionId, $rootScope.username);
    }

    $scope.info = function () {
        $state.transitionTo('home.variables.info');
    };

    $scope.save = function () {
        $scope.ChildFunctions.Save();
    };

    $scope.publish = function () {
        $scope.ChildFunctions.Publish();
    };

    $scope.onChildClose = function () {
        if ($state.current.name == 'home.variables.info') {
            $state.transitionTo('home.variables.editor');
        }
        else if ($state.current.name == 'home.variables.editor') {
            $state.go('home.start-page');
        }
    };

    $scope.close = function () {
        $eventAggregatorService.unsubscribe($eventDefinitions.onPendingLockRelease, $scope.eventHandlers.onPendingLockRelease);
        if (!$scope.LockedBy) {
            $sessionLockService.closeLockForVariables($scope.SelectedVariableSetVersionId, $rootScope.username);
        }
        $scope.ChildFunctions.Close($scope.onChildClose);
    };

    $scope.Parent_ChangeVariableSetID = function (variableSetID) {
        $scope.SelectedVariableSetID = variableSetID;
        $state.transitionTo('home.variables.editor');
    }

    $eventAggregatorService.subscribe($eventDefinitions.onPendingLockRelease, $scope.eventHandlers.onPendingLockRelease);
}]);