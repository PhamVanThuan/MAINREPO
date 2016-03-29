'use strict';

/* Services */

angular.module('sahl.tools.app.services.enumerationDataManager', [
    'sahl.tools.app.serviceConfig',
    'SAHL.Services.Interfaces.DecisionTreeDesign.queries',
    'SAHL.Services.Interfaces.DecisionTreeDesign.commands'
])
.factory('$enumerationDataManager', ['$decisionTreeDesignQueries', '$queryManager', '$decisionTreeDesignCommands', '$commandManager', function ($decisionTreeDesignQueries, $queryManager, $decisionTreeDesignCommands, $commandManager) {
    var GetLatestEnumerationSetQueryAsync = function () {
        var query = new $decisionTreeDesignQueries.GetLatestEnumerationSetQuery();
        return $queryManager.sendQueryAsync(query);
    }

    var SaveEnumerationSetCommandAsync = function (id, version, data) {
        var command = new $decisionTreeDesignCommands.SaveEnumerationSetCommand(id, version, data);
        return $commandManager.sendCommandAsync(command);
    }

    var SaveAndPublishEnumerationSetCommandAsync = function (id, version, data, publisher) {
        var command = new $decisionTreeDesignCommands.SaveAndPublishEnumerationSetCommand(id, version, data, publisher);
        return $commandManager.sendCommandAsync(command);
    }

    var GetAllEnumerationVersionsQueryAsync = function () {
        var query = new $decisionTreeDesignQueries.GetAllEnumerationVersionsQuery();
        return $queryManager.sendQueryAsync(query);
    }

    var GetEnumerationAtVersionQueryAsync = function (id) {
        var query = new $decisionTreeDesignQueries.GetEnumerationAtVersionQuery(id);
        return $queryManager.sendQueryAsync(query);
    }

    var sanitize = function (input,firstIsLowered){
        var output = input.replace(/[\W]+/g, "");
        if(firstIsLowered){
            output = output[0].toLowerCase() + output.substring(1);
        }
        return output;
    }

    var extend = function (data) {
        data.$lastGroupId = 0;
        if(data.groups.length > 0){
            data.$lastGroupId = data.groups[data.groups.length - 1].id;
        }
        if (data.hasOwnProperty("enumerations")) {
            data.$lastEnumId = 0;
            if(data.enumerations.length > 0){
                data.$lastEnumId = data.enumerations[data.enumerations.length - 1].id;
                for (var i = 0, c = data.enumerations.length; i < c; i++) {
                    data.enumerations[i].$getId = function () {
                        return data.$getId() + "_E" + data.enumerations.indexOf(this);
                    }
                    data.enumerations[i].$getFullName = function () {
                        return data.$getFullName() + "." + sanitize(this.name);
                    }
                }
            }
        }
        for (var i = 0, c = data.groups.length; i < c; i++) {
            data.groups[i] = extend(data.groups[i]);
            data.groups[i].$getId = function () {
                if (data["$getId"] !== undefined) {
                    return data.$getId() + "_G" + data.groups.indexOf(this);
                } else {
                    return "G"+data.groups.indexOf(this);
                }
            }
            data.groups[i].$getFullName = function () {
                if (data["$getFullName"] !== undefined) {
                    return data.$getFullName() + "." + sanitize(this.name,true);
                } else {
                    return sanitize(this.name, true);
                }
            }
        }
        return data;
    }
    
    var shrink = function (data) {
        for (var property in data) {
            if (property.indexOf("$") === 0) {
                delete data[property];
            }
        }
        for (var i = 0, c = data.groups.length; i < c; i++) {
            data.groups[i] = shrink(data.groups[i]);
        }
        return data;
    }

    var getElementById = function (id,data) {
        var arr = id.split("_");
        var retVal = data;
        for (var key in arr) {
            var index = arr[key][1];
            if (arr[key][0] == "G") {
                retVal = retVal.groups[index];
            } else if (arr[key][0] == "E") {
                retVal = retVal.enumerations[index];
            }
        }
        return retVal;
    }

    return {
        GetLatestEnumerationSetQueryAsync: GetLatestEnumerationSetQueryAsync,
        SaveEnumerationSetCommandAsync: SaveEnumerationSetCommandAsync,
        SaveAndPublishEnumerationSetCommandAsync: SaveAndPublishEnumerationSetCommandAsync,
        GetAllEnumerationVersionsQueryAsync: GetAllEnumerationVersionsQueryAsync,
        GetEnumerationAtVersionQueryAsync: GetEnumerationAtVersionQueryAsync,

        $extend: extend,
        $shrink: shrink,
        $getElementById: getElementById
    }
}])