'use strict';

/* Services */

angular.module('sahl.tools.app.services.variableDataManager', [
    'sahl.tools.app.serviceConfig',
    'SAHL.Services.Interfaces.DecisionTreeDesign.queries',
    'SAHL.Services.Interfaces.DecisionTreeDesign.commands'
])
.factory('$variableDataManager', ['$q', '$decisionTreeDesignQueries', '$queryManager', '$decisionTreeDesignCommands', '$commandManager', function ($q, $decisionTreeDesignQueries, $queryManager, $decisionTreeDesignCommands, $commandManager) {
    return {
        GetBasicTypes: function () {
            return ['string', 'float', 'int', 'bool', 'double'];
        },
        GetLatestVariableSetQueryAsync: function () {
            var query = new $decisionTreeDesignQueries.GetLatestVariableSetQuery();
            return $queryManager.sendQueryAsync(query);
        },
        SaveVariableSetCommandAsync: function (id, version, data) {
            var command = new $decisionTreeDesignCommands.SaveVariableSetCommand(id, version, data);
            return $commandManager.sendCommandAsync(command);
        },
        SaveAndPublishVariableSetCommandAsync: function (id, version, data, publisher) {
            var command = new $decisionTreeDesignCommands.SaveAndPublishVariableSetCommand(id, version, data, publisher);
            return $commandManager.sendCommandAsync(command);
        },
        GetDefaultValueForType: function(type) {
            if (type == 'int' || type == 'float' || type == 'double' || type == 'enumeration') {
                return 0;
            }
            else if(type=='bool'){
                return 'false';
            }
            else {
                return '';
            }
        },
        GetAllVariableVersionsQueryAsync: function () {
            var query = new $decisionTreeDesignQueries.GetAllVariableVersionsQuery();
            return $queryManager.sendQueryAsync(query);
        },
        GetVariableSetByVariableSetIdQueryAsync: function (variableSetID) {
            var query = new $decisionTreeDesignQueries.GetVariableSetByVariableSetIdQuery(variableSetID);
            return $queryManager.sendQueryAsync(query);
        },
    };
}])