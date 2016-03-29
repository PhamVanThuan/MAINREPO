'use strict';

angular.module('sahl.tools.app.home.start-page', [
  'ui.router'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.start-page', {
        url: "/",
        templateUrl: "./app/home/start-page/start-page.tpl.html",
        controller: "StartPageCtrl",
    });
})

.controller('StartPageCtrl', ['$rootScope', '$scope', '$state', '$queryManager', '$decisionTreeDesignQueries', '$activityManager', '$decisionTreeDesignCommands', '$commandManager', '$q', '$eventAggregatorService', '$eventDefinitions', '$keyboardManager', '$serviceConfig',
    function StartPageController($rootScope, $scope, $state, $queryManager, $decisionTreeDesignQueries, $activityManager, $decisionTreeDesignCommands, $commandManager, $q, $eventAggregatorService, $eventDefinitions, $keyboardManager, $serviceConfig) {
        $scope.documentsLocked = {};
        $scope.searchString = '';

        var clearSearchBox = function(){
            $scope.searchString = '';
        }



        $keyboardManager.bind('esc', clearSearchBox, { "target": "decisionTreeSearch" });
        $eventAggregatorService.subscribe($eventDefinitions.onUserAuthenticated, function (username) {
            $scope.loadRecentTrees(username);
        });

        $scope.setData = function (data) {
            angular.forEach(data, function(tree) {
                if(tree.CurrentlyOpenBy) {
                    tree.CurrentlyOpenByImageSrc = $serviceConfig.UserProfileImageService + tree.CurrentlyOpenBy + "&size=48"
                }
            })
            $scope.publishedTrees = data.filter(function (tree) { return (tree.IsPublished == true); });
            $scope.unpublishedTrees = data.filter(function (tree) { return (tree.IsPublished == false); });;
        }

        $scope.saveTreePinStatus = function (treeVersion, pinned) {
            $activityManager.startActivityWithKey('savePinStatusDecisionTree');
            var command = new $decisionTreeDesignCommands.SaveMRUDecisionTreePinStatusCommand($rootScope.username, treeVersion, !pinned);

            $commandManager.sendCommandAsync(command).then(function () {
                $scope.loadRecentTrees($rootScope.username);
            }, function () {

            })
        }

        $scope.setRecentData = function (data) {
            $activityManager.stopActivityWithKey("loadRecentTrees");
            $scope.recenttreespinned = [];
            $scope.recenttrees = [];
            $scope.hasPin = false;
            for (var i = 0; i < data.length; i++) {
                if (data[i].Pinned) {
                    $scope.hasPin = true;
                    $scope.recenttreespinned.push(data[i]);
                } else {
                    $scope.recenttrees.push(data[i]);
                }
            }
        }

        $scope.loadRecentTrees = function (username) {
            $activityManager.startActivityWithKey('loadRecentTrees');
            var query = new $decisionTreeDesignQueries.GetMRUDecisionTreeQuery(username);
            $queryManager.sendQueryAsync(query).then(function (data) {
                $scope.setRecentData(data.data.ReturnData.Results.$values);
                $activityManager.stopActivityWithKey("loadRecentTrees");
            });
        }

   

    $scope.loadTrees = function () {
        $activityManager.startActivityWithKey('loadTrees');
        var query = new $decisionTreeDesignQueries.GetLatestDecisionTreesQuery();

        $queryManager.sendQueryAsync(query).then(function (data) {
            $scope.setData(data.data.ReturnData.Results.$values);

            $activityManager.stopActivityWithKey("loadTrees");
            });
        }

        $scope.newTree = function () {
            // get a new GUID for the tree
            var query = new $decisionTreeDesignQueries.GetNewGuidQuery();
            $queryManager.sendQueryAsync(query).then(function (data) {
                var newTreeId = data.data.ReturnData.Results.$values[0].Id

                $queryManager.sendQueryAsync(query).then(function (data) {
                    var newTreeVersionId = data.data.ReturnData.Results.$values[0].Id

                    $state.go("home.design", { treeId: newTreeId, treeVersionId: newTreeVersionId, isNew: true });
                })
            });
        }

        $scope.openTree = function (treeId, treeVersionId, isPublished) {
            $state.go("home.design", { treeId: treeId, treeVersionId: treeVersionId, isNew: false });
        }
    
        $scope.loadLockedDocuments = function () {
            
            var query = new $decisionTreeDesignQueries.GetNonTreeDocumentLockStatusQuery();
            $queryManager.sendQueryAsync(query).then(function (data) {
                for (var i = 0, c = data.data.ReturnData.Results.$values.length; i < c; i++) {
                    $scope.documentsLocked[data.data.ReturnData.Results.$values[i].DocumentType] = data.data.ReturnData.Results.$values[i].Username;
                    $scope.documentsLocked[data.data.ReturnData.Results.$values[i].DocumentType + "Img"] = $serviceConfig.UserProfileImageService + data.data.ReturnData.Results.$values[i].Username + "&size=20";
                }
            });
        }

        $scope.loadTrees();
        $scope.loadLockedDocuments();

        if ($rootScope.username != undefined && $rootScope.username != "") {
            $scope.loadRecentTrees($rootScope.username);
        }

        $scope.$on('$destroy', function () {
            $keyboardManager.unbind('esc');
        })

        $scope.deleteTree = function (treeId) { //NB: limit who can do this

        }

        var eventHandlers = {
            addLocks: function (documentsLocked) {
                for (var i = 0, c = documentsLocked.length; i < c; i++) {
                    if (documentsLocked[i].DocumentTypeName == "Tree"){
                        if ($scope.publishedTrees !== undefined) {
                            for (var ii = 0, cc = $scope.publishedTrees.length; ii < cc; ii++) {
                                if ($scope.publishedTrees[ii].DecisionTreeVersionId == documentsLocked[i].DocumentVersionId) {
                                    $scope.publishedTrees[ii].CurrentlyOpenBy = documentsLocked[i].Username;
                                    $scope.publishedTrees[ii].CurrentlyOpenByImageSrc = $serviceConfig.UserProfileImageService + documentsLocked[i].Username + "&size=48";
                                }
                            }
                        }
                        if ($scope.unpublishedTrees !== undefined) {
                            for (var ii = 0, cc = $scope.unpublishedTrees.length; ii < cc; ii++) {
                                if ($scope.unpublishedTrees[ii].DecisionTreeVersionId == documentsLocked[i].DocumentVersionId) {
                                    $scope.unpublishedTrees[ii].CurrentlyOpenBy = documentsLocked[i].Username;
                                    $scope.unpublishedTrees[ii].CurrentlyOpenByImageSrc = $serviceConfig.UserProfileImageService + documentsLocked[i].Username + "&size=48";
                                }
                            }
                        }
                    } else {
                        $scope.documentsLocked[documentsLocked[i].DocumentTypeName] = documentsLocked[i].Username;
                        $scope.documentsLocked[documentsLocked[i].DocumentTypeName + "Img"] = $serviceConfig.UserProfileImageService + documentsLocked[i].Username + "&size=20";
                    }
                }
                $scope.$apply();
            },
            removeLocks: function (lockIdsRemoved) {
                for (var i = 0, c = lockIdsRemoved.length; i < c; i++) {
                    if (lockIdsRemoved[i].DocumentTypeName == "Tree") {
                        for (var ii = 0, cc = $scope.publishedTrees.length; ii < cc; ii++) {
                            if ($scope.publishedTrees[ii].DecisionTreeVersionId == lockIdsRemoved[i].DocumentVersionId) {
                                delete $scope.publishedTrees[ii].CurrentlyOpenBy;
                                delete $scope.publishedTrees[ii].CurrentlyOpenByImageSrc;
                            }
                        }
                        for (var ii = 0, cc = $scope.unpublishedTrees.length; ii < cc; ii++) {
                            if ($scope.unpublishedTrees[ii].DecisionTreeVersionId == lockIdsRemoved[i].DocumentVersionId) {
                                delete $scope.unpublishedTrees[ii].CurrentlyOpenBy;
                                delete $scope.unpublishedTrees[ii].CurrentlyOpenByImageSrc;
                            }
                        }
                    } else {
                        delete $scope.documentsLocked[lockIdsRemoved[i].DocumentTypeName]
                        delete $scope.documentsLocked[lockIdsRemoved[i].DocumentTypeName + "Img"]
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
        
    }]);