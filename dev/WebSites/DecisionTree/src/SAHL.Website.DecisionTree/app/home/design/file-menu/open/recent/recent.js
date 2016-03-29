'use strict';

angular.module('sahl.tools.app.home.design.file-menu.open.recent', [
  'ui.router',
  'sahl.tools.app.services'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.design.file-menu.open.recent', {
        url: "/recent",
        templateUrl: "./app/home/design/file-menu/open/recent/recent.tpl.html",
        controller: 'FileOpenRecentCtrl'
    });
})

.controller('FileOpenRecentCtrl', ['$rootScope', '$scope', '$state', '$keyboardManager', '$documentManager', '$stateParams', '$activityManager', '$queryManager', '$decisionTreeDesignQueries', '$serviceConfig', '$eventAggregatorService', '$eventDefinitions',
    function FileOpenRecentCtrl($rootScope, $scope, $state, $keyboardManager, $documentManager, $stateParams, $activityManager, $queryManager, $decisionTreeDesignQueries, $serviceConfig, $eventAggregatorService, $eventDefinitions) {        
        $scope.setData = function (data) {
            angular.forEach(data, function(tree) {
                if(tree.CurrentlyOpenBy) {
                    tree.CurrentlyOpenByImageSrc = $serviceConfig.UserProfileImageService + tree.CurrentlyOpenBy + "&size=48"
                }
            })
            $activityManager.stopActivityWithKey("loadTrees");
            $scope.trees = data;
        }

        $scope.loadTrees = function () {
            $activityManager.startActivityWithKey('loadTrees');
            var query = new $decisionTreeDesignQueries.GetMRUDecisionTreeQuery($rootScope.username);

            $queryManager.sendQueryAsync(query).then(function (data) {
                $scope.setData(data.data.ReturnData.Results.$values);
                $activityManager.stopActivityWithKey("loadTrees");
            });
        }

        $scope.openTree = function (treeId, treeVersionId, isPublished) {
            $state.go("home.design", { treeId: treeId, treeVersionId: treeVersionId, isNew: false });
        }

        var eventHandlers = {
            addLocks: function (treesLocked) {
                for (var i = 0, c = treesLocked.length; i < c; i++) {
                    for (var ii = 0, cc = $scope.trees.length; ii < cc; ii++) {
                        if ($scope.trees[ii].DecisionTreeVersionId == treesLocked[i].DocumentVersionId) {
                            $scope.trees[ii].CurrentlyOpenBy = treesLocked[i].Username;
                            $scope.trees[ii].CurrentlyOpenByImageSrc = $serviceConfig.UserProfileImageService + treesLocked[i].Username + "&size=48";
                        }
                    }
                }
                $scope.$apply();
            }
            ,removeLocks: function (lockIdsRemoved) {
                for (var i = 0, c = lockIdsRemoved.length; i < c; i++) {
                    for (var ii = 0, cc = $scope.trees.length; ii < cc; ii++) {
                        if ($scope.trees[ii].DecisionTreeVersionId == lockIdsRemoved[i].DocumentVersionId) {
                            delete $scope.trees[ii].CurrentlyOpenBy;
                            delete $scope.trees[ii].CurrentlyOpenByImageSrc;
                        }
                    }
                }
                $scope.$apply();
            }
        }

        $eventAggregatorService.subscribe($eventDefinitions.onAddedLocks, eventHandlers.addLocks);
        $eventAggregatorService.subscribe($eventDefinitions.onRemovedLocks, eventHandlers.removeLocks);

        $scope.$on('$destroy', function iVeBeenDismissed() {
            $eventAggregatorService.unsubscribe($eventDefinitions.onAddedLocks, eventHandlers.addLocks);
            $eventAggregatorService.unsubscribe($eventDefinitions.onRemovedLocks, eventHandlers.removeLocks);
        });

        $scope.loadTrees();

    }]);