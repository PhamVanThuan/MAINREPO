'use strict';

/* Services */

angular.module('sahl.tools.app.services.recentDocumentsService', [
])
.factory('$recentDocumentsService', ['$rootScope', '$eventAggregatorService', '$eventDefinitions', '$q', '$commandManager', '$decisionTreeDesignCommands',
    function ($rootScope, $eventAggregatorService, $eventDefinitions, $q, $commandManager, $decisionTreeDesignCommands) {

    var operations = {
        updateMRUTree: function () {
            var deferred = $q.defer();
            var command = new $decisionTreeDesignCommands.SaveMRUDecisionTreeCommand($rootScope.username, $rootScope.document.treeVersionId);

            $commandManager.sendCommandAsync(command).then(function () {
                deferred.resolve();
            }, function () {
                deferred.reject();
            })

            return deferred.promise;
        }
    }

    var eventHandlers = {
        onDocumentLoaded: function (documentLoaded) {
            if (!documentLoaded.isNewDocument) {
                operations.updateMRUTree().then(function () {
                })
            }
        },
        onNewDocumentSaved: function (savedDocument) {
            operations.updateMRUTree().then(function () {
            })
        }
    }

    $eventAggregatorService.subscribe($eventDefinitions.onDocumentLoaded, eventHandlers.onDocumentLoaded);
    $eventAggregatorService.subscribe($eventDefinitions.onNewDocumentSaved, eventHandlers.onNewDocumentSaved);

    return {
        start: function () {
        }
    }
}]);