'use strict';

angular.module('sahl.tools.app.home.design.file-menu.info', [
  'ui.router',
  'sahl.tools.app.services'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.design.file-menu.info', {
        url: "/info",
        templateUrl: "./app/home/design/file-menu/info/info.tpl.html",
        controller: 'FileInfoCtrl'
    });
})

.controller('FileInfoCtrl', ['$rootScope', '$scope', '$state', '$keyboardManager', '$documentManager', '$stateParams', '$eventAggregatorService', '$eventDefinitions', '$activityManager', '$timeout',
function FileInfoCtrl($rootScope, $scope, $state, $keyboardManager, $documentManager, $stateParams, $eventAggregatorService, $eventDefinitions, $activityManager, $timeout) {
    $scope.decisionTreeHistory = [];
    var loading = false;
    var loaded = false;
        var eventHandlers = {
            onDocumentLoaded: function (loadedDocument) {
                if (loading === false && loaded == false) {
                    loading = true;
                    $documentManager.getDocumentInfo().then(function (data) {
                        $scope.decisionTreeHistory = data.data.ReturnData.Results.$values;
                        for (var i = 0; i < $scope.decisionTreeHistory.length; i++) {
                            var d = new Date($scope.decisionTreeHistory[i].ModificationDate);
                            $scope.decisionTreeHistory[i].FormattedDate = d.toString();
                        }
                        $eventAggregatorService.unsubscribe($eventDefinitions.onDocumentLoaded, eventHandlers.onDocumentLoaded);
                        loaded = true;
                    });
                }
            }
        }

        $eventAggregatorService.subscribe($eventDefinitions.onDocumentLoaded, eventHandlers.onDocumentLoaded);

        $timeout(function () {
            if (loading == false && loaded == false) {
                if ($rootScope.document !== undefined) {
                    loading = true;
                    $documentManager.getDocumentInfo().then(function (data) {
                        $scope.decisionTreeHistory = data.data.ReturnData.Results.$values;
                        $activityManager.startActivityWithKey('loadingDocumentInfo');
                        $timeout(function () {
                            for (var i = 0; i < $scope.decisionTreeHistory.length; i++) {
                                var d = new Date($scope.decisionTreeHistory[i].ModificationDate);
                                $scope.decisionTreeHistory[i].FormattedDate = d.toString();
                                loaded = true;
                                loading = false;
                            }
                            $activityManager.stopActivityWithKey('loadingDocumentInfo');
                        }, 400)
                    });
                }
            }
        })

    	$scope.loadVersion = function (index) {
    	    var loadId=$scope.decisionTreeHistory[index].DecisionTreeVersionId;
    	    $state.go("home.design", { treeId: $rootScope.document.treeId, treeVersionId: loadId, isNew: false });
    	    
    	}
    }]);