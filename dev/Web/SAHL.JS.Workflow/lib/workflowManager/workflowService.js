'use strict';
angular.module('sahl.js.workflow.workflowManager')
    .service('$workflowService', [function() {
        var operations = {
            startActivity: function(instanceId, correlationID, activityName, ignoreWarnings, mapVariables, data) { },
            cancelActivity: function(instanceId, correlationID,  activityName, ignoreWarnings, mapVariables, data) { },
            completeActivity: function(instanceId, correlationID, activityName, ignoreWarnings, mapVariables, data) { },
            createInstance: function(correlationID, activityName, processName, workflowName, ignoreWarnings, ReturnActivityId, SourceInstanceId, mapVariables, data) { },
            completeCreateInstanceRequest: function(instanceId, correlationID, activityName, ignoreWarnings, mapVariables, data) { },
        };
        return {
            startActivity: operations.startActivity,
            cancelActivity: operations.cancelActivity,
            completeActivity: operations.completeActivity,
            createInstance: operations.createInstance,
            completeCreateInstanceRequest : operations.completeCreateInstanceRequest
        };
    }]);
