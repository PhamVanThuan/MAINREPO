'use strict';
angular.module('SAHL.Services.Interfaces.X2Service.commands', []).factory('$x2Commands', function() {
    return (function() {

        function CreateInstanceRequest(correlationID, activityName, processName, workflowName, serviceRequestMetadata, ignoreWarnings, ReturnActivityId, SourceInstanceId, mapVariables, data) {
            this.$type = 'SAHL.Core.X2.Messages.X2CreateInstanceRequest,SAHL.Core.X2';
            this.correlationID = correlationID || '';
            this.activityName = activityName || '';
            this.processName = processName || '';
            this.workflowName = workflowName || '';
            this.serviceRequestMetadata = serviceRequestMetadata || null;
            this.ignoreWarnings = ignoreWarnings || false;
            this.ReturnActivityId = ReturnActivityId || null;
            this.SourceInstanceId = SourceInstanceId || null;
            this.mapVariables = mapVariables || null;
            this.data = data || null;
        }

        function StartActivityRequest(correlationID, instanceId, serviceRequestMetadata, activityName, ignoreWarnings, mapVariables, data) {
            this.$type = 'SAHL.Core.X2.Messages.X2RequestForExistingInstance,SAHL.Core.X2';
            this.action = 2;
            this.correlationID = correlationID || '';
            this.instanceId = instanceId || '';
            this.serviceRequestMetadata = serviceRequestMetadata || null;
            this.activityName = activityName || '';
            this.ignoreWarnings = ignoreWarnings || false;
            this.mapVariables = mapVariables || null;
            this.data = data || null;
        }

        function CancelActivityRequest(correlationID, instanceId, serviceRequestMetadata, activityName, ignoreWarnings, mapVariables, data) {
            this.$type = 'SAHL.Core.X2.Messages.X2RequestForExistingInstance,SAHL.Core.X2';
            this.action = 8;
            this.correlationID = correlationID || '';
            this.instanceId = instanceId || '';
            this.serviceRequestMetadata = serviceRequestMetadata || null;
            this.activityName = activityName || '';
            this.ignoreWarnings = ignoreWarnings || false;
            this.mapVariables = mapVariables || null;
            this.data = data || null;
        }

        function CompleteActivityRequest(correlationID, instanceId, serviceRequestMetadata, activityName, ignoreWarnings, mapVariables, data) {
            this.$type = 'SAHL.Core.X2.Messages.X2RequestForExistingInstance,SAHL.Core.X2';
            this.action = 4;
            this.correlationID = correlationID || '';
            this.instanceId = instanceId || '';
            this.serviceRequestMetadata = serviceRequestMetadata || null;
            this.activityName = activityName || '';
            this.ignoreWarnings = ignoreWarnings || false;
            this.mapVariables = mapVariables || null;
            this.data = data || null;
        }

        function CompleteCreateInstanceRequest(correlationID, instanceId, serviceRequestMetadata, activityName, ignoreWarnings, mapVariables, data) {
            this.$type = 'SAHL.Core.X2.Messages.X2RequestForExistingInstance,SAHL.Core.X2';
            this.action = 1;
            this.correlationID = correlationID || '';
            this.instanceId = instanceId || '';
            this.serviceRequestMetadata = serviceRequestMetadata || null;
            this.activityName = activityName || '';
            this.ignoreWarnings = ignoreWarnings || false;
            this.mapVariables = mapVariables || null;
            this.data = data || null;
        }
        return {
            startActivityRequest: StartActivityRequest,
            cancelActivityRequest: CancelActivityRequest,
            completeActivityRequest: CompleteActivityRequest,
            createInstanceRequest: CreateInstanceRequest,
            completeCreateInstanceRequest: CompleteCreateInstanceRequest
        };
    }());
});

angular.module('sahl.services.interfaces.x2service.templates', []);
