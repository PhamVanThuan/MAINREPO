'use strict';

angular.module('sahl.tools.app.services.queryGlobalsVersionService', [
    'sahl.tools.app.services.eventAggregatorService',
    'sahl.tools.app.services.eventDefinitions'
])
.factory('$queryGlobalsVersionService', ['$rootScope', '$q', '$enumerationDataManager', 'messageDataManager', '$variableDataManager', '$eventDefinitions', '$eventAggregatorService',function ($rootScope, $q, enumerationDataManager, messageDataManager, variableDataManager, $eventDefinitions, $eventAggregatorService) {
    var objectModels = {
        QueryGlobalsVersions: function () {
            return {
                "VariablesVersions": [],
                "MessagesVersions": [],
                "EnumerationsVersions": []
            }
        },
        QueryGlobalsVersion : function() {
            this.VariablesVersion = -1;
            this.MessagesVersion = -1;
            this.EnumerationsVersion = -1;
            this._name = 'SAHL.DecisionTree.Shared.QueryGlobalsVersion,SAHL.DecisionTree.Shared';
        }
    }

    var queryGlobalsVersionService = {
        data: null,
        model : null,
        initialize: function () {
            queryGlobalsVersionService.data = objectModels.QueryGlobalsVersions();
            queryGlobalsVersionService.model = new objectModels.QueryGlobalsVersion();

            var enumVersions = $q.defer();
            var msgVersions = $q.defer();
            var varVersions = $q.defer();

            enumerationDataManager.GetAllEnumerationVersionsQueryAsync().then(function (data) {
                queryGlobalsVersionService.genericMapper(data.data.ReturnData.Results.$values, queryGlobalsVersionService.data.EnumerationsVersions, "EnumerationsVersion");
                enumVersions.resolve();
            });
            messageDataManager.GetAllMessageVersionsQueryAsync().then(function (data) {
                queryGlobalsVersionService.genericMapper(data.data.ReturnData.Results.$values, queryGlobalsVersionService.data.MessagesVersions, "MessagesVersion");
                msgVersions.resolve();
            })
            variableDataManager.GetAllVariableVersionsQueryAsync().then(function (data) {
                queryGlobalsVersionService.genericMapper(data.data.ReturnData.Results.$values, queryGlobalsVersionService.data.VariablesVersions, "VariablesVersion");
                varVersions.resolve();
            })
            $q.all([enumVersions, msgVersions, varVersions]).then(function(){
                events.onGlobalsLoaded();
            })
        },
        destroy: function () {
            queryGlobalsVersionService.model = objectModels.QueryGlobalsVersions();
        },
        genericMapper: function (dataValues,dataModel,model) {
            if (dataValues.length > 0) {
                queryGlobalsVersionService.model[model] = dataValues[0].Version;
                angular.forEach(dataValues, function (item) {
                    dataModel.push({ 'version': item.Version, 'label': item.Version });
                });
            }
        }
    }
    var events = {
        onGlobalsLoaded: function () {
            $eventAggregatorService.publish($eventDefinitions.onGlobalsLoaded, { 'data': queryGlobalsVersionService.data, 'model': queryGlobalsVersionService.model });
        }
    }
    var eventHandlers = {
        onDocumentLoaded: function (message) {
            queryGlobalsVersionService.initialize();
        },
        onDocumentUnloaded: function (message) {
            queryGlobalsVersionService.destroy();
        }
    }

    $eventAggregatorService.subscribe($eventDefinitions.onDocumentLoaded, eventHandlers.onDocumentLoaded);
    $eventAggregatorService.subscribe($eventDefinitions.onDocumentUnloaded, eventHandlers.onDocumentUnloaded);

    return {
        QueryGlobalsVersion: function () { return queryGlobalsVersionService.model; },
        start: function () { }
    };
}]);