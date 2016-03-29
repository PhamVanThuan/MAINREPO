'use strict';

/* Services */

angular.module('sahl.tools.app.services.messageDataManager', [
    'sahl.tools.app.serviceConfig',
    'SAHL.Services.Interfaces.DecisionTreeDesign.queries',
    'SAHL.Services.Interfaces.DecisionTreeDesign.commands'
])
.factory('messageDataManager', ['$decisionTreeDesignQueries', '$queryManager', '$decisionTreeDesignCommands', '$commandManager', function ($decisionTreeDesignQueries, $queryManager, $decisionTreeDesignCommands, $commandManager) {
    var GetLatestMessageSetQueryAsync = function () {
        var query = new $decisionTreeDesignQueries.GetLatestMessageSetQuery();
        return $queryManager.sendQueryAsync(query);
    }

    var GetLatestPublishedMessageSetQueryAsync = function () {
        var query = new $decisionTreeDesignQueries.GetLatestPublishedMessageSetQuery();
        return $queryManager.sendQueryAsync(query);
    }

    var GetPublishedMessageSetByMessageSetIdQueryAsync = function (MessageSetId) {
        var query = new $decisionTreeDesignQueries.GetPublishedMessageSetByMessageSetIdQuery(MessageSetId);
        return $queryManager.sendQueryAsync(query);
    }

    var GetNewGuidQueryAsync = function () {
        var query = new $decisionTreeDesignQueries.GetNewGuidQuery();
        return $queryManager.sendQueryAsync(query);
    }

    var SaveMessageSetCommandAsync = function (id, version, data) {
        var command = new $decisionTreeDesignCommands.SaveMessageSetCommand(id, version, data);
        return $commandManager.sendCommandAsync(command);
    }

    var SaveAndPublishMessageSetCommandAsync = function (messageSetId, version, data, publishedMessageSetId, publisher) {
        var command = new $decisionTreeDesignCommands.SaveAndPublishMessageSetCommand(messageSetId, version, data, publishedMessageSetId, publisher);
        return $commandManager.sendCommandAsync(command);
    }

    var sanitize = function (input, firstIsLowered) {
        var output = input.replace(/[\W]+/g, "");
        if (firstIsLowered) {
            output = output[0].toLowerCase() + output.substring(1);
        }
        return output;
    }

    var extend = function (data) {
        data.$lastGroupId = 0;
        if (data.groups.length > 0) {
            data.$lastGroupId = data.groups[data.groups.length - 1].id;
        }
        if (data.hasOwnProperty("messages")) {
            data.$lastMessageId = 0;
            if (data.messages.length > 0) {
                data.$lastMessageId = data.messages[data.messages.length - 1].id;
                for (var i = 0, c = data.messages.length; i < c; i++) {
                    data.messages[i].$getFullName = function () {
                        return data.$getFullName() + "." + sanitize(this.name);
                    }
                }
            }
        }
        for (var i = 0, c = data.groups.length; i < c; i++) {
            data.groups[i] = extend(data.groups[i]);
            data.groups[i].$getFullName = function () {
                if (data["$getFullName"] !== undefined) {
                    return data.$getFullName() + "." + sanitize(this.name, true);
                } else {
                    return sanitize(this.name, true);
                }
            }
        }
        return data;
    }

    var shrink = function(data){
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

    var GetAllMessageVersionsQueryAsync = function () {
        var query = new $decisionTreeDesignQueries.GetAllMessageVersionsQuery();
        return $queryManager.sendQueryAsync(query);
    }

    var GetMessageSetByMessageSetIdQueryAsync = function (MessageSetId) {
        var query = new $decisionTreeDesignQueries.GetMessageSetByMessageSetIdQuery(MessageSetId);
        return $queryManager.sendQueryAsync(query);
    }

    return {
        GetLatestMessageSetQueryAsync: GetLatestMessageSetQueryAsync,
        GetLatestPublishedMessageSetQueryAsync: GetLatestPublishedMessageSetQueryAsync,
        GetPublishedMessageSetByMessageSetIdQueryAsync: GetPublishedMessageSetByMessageSetIdQueryAsync,
        GetNewGuidQueryAsync: GetNewGuidQueryAsync,
        SaveMessageSetCommandAsync: SaveMessageSetCommandAsync,
        SaveAndPublishMessageSetCommandAsync: SaveAndPublishMessageSetCommandAsync,
        $extend: extend,
        $shrink: shrink,
        GetAllMessageVersionsQueryAsync: GetAllMessageVersionsQueryAsync,
        GetMessageSetByMessageSetIdQueryAsync: GetMessageSetByMessageSetIdQueryAsync
    };
}])