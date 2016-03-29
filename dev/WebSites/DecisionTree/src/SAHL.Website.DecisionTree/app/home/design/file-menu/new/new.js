'use strict';

angular.module('sahl.tools.app.home.design.file-menu.new', [
  'ui.router',
  'sahl.tools.app.services'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.design.file-menu.new', {
        url: "/new",
        templateUrl: "./app/home/design/file-menu/new/new.tpl.html",
        controller: 'FileNewCtrl'
    });
})

.controller('FileNewCtrl', ['$rootScope', '$scope', '$state', '$keyboardManager', '$documentManager', '$stateParams', '$queryManager', '$decisionTreeDesignQueries', '$activityManager',
function FileNewCtrl($rootScope, $scope, $state, $keyboardManager, $documentManager, $stateParams, $queryManager, $decisionTreeDesignQueries, $activityManager) {
        $scope.setData = function (data) {
            $activityManager.stopActivityWithKey("loadTrees");
            $scope.trees = data;
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

        $scope.newFromExistingTree = function (existingTreeId, existingTreeVersionId, isPublished) {
            // get a new GUID for the tree
            var query = new $decisionTreeDesignQueries.GetNewGuidQuery();
            $queryManager.sendQueryAsync(query).then(function (data) {
                var newTreeId = data.data.ReturnData.Results.$values[0].Id

                $queryManager.sendQueryAsync(query).then(function (data) {
                    var newTreeVersionId = data.data.ReturnData.Results.$values[0].Id

                    $state.go("home.design", { treeId: newTreeId, treeVersionId: newTreeVersionId, isNew: true, templateTreeVersionId: existingTreeVersionId });
                })
            });
        }

        $scope.loadTrees();
    }]);