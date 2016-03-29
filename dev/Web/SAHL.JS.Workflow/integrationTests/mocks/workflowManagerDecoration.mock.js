'use strict';
angular.module('mock.workflow.workflowManagerDecorator', [
        'mock.webservices',
        'SAHL.Services.Interfaces.X2Service.commands'
    ])
    .provider('$workflowManagerDecoration', [function() {
        this.decoration = ['$delegate', '$x2WebService', '$x2Commands', '$authenticatedUser',
            function($delegate, $x2WebService, $x2Commands, $authenticatedUser) {

                var metaData = {
                    $type: 'SAHL.Core.Services.ServiceRequestMetadata,SAHL.Core',
                    username: $authenticatedUser.fullAdName,
                    currentuserrole: $authenticatedUser.currentOrgRole.RoleName
                };

                var operations = {
                    startActivity: function(instanceId, correlationID, activityName, ignoreWarnings, mapVariables, data) {
                        console.log('startActivity');
                        var command = new $x2Commands.startActivityRequest(correlationID, instanceId, metaData, activityName, ignoreWarnings, mapVariables, data);
                        return $x2WebService.sendX2RequestWithReturnedDataAsync(command);
                    },
                    cancelActivity: function(instanceId, correlationID, activityName, ignoreWarnings, mapVariables, data) {
                        console.log('cancelActivity');
                        var command = new $x2Commands.cancelActivityRequest(correlationID, instanceId, metaData, activityName, ignoreWarnings, mapVariables, data);
                        return $x2WebService.sendX2RequestWithReturnedDataAsync(command);
                    },
                    completeActivity: function(instanceId, correlationID, activityName, ignoreWarnings, mapVariables, data) {
                        console.log('completeActivity');
                        var command = new $x2Commands.completeActivityRequest(correlationID, instanceId, metaData, activityName, ignoreWarnings, mapVariables, data);
                        return $x2WebService.sendX2RequestWithReturnedDataAsync(command);
                    },
                    createInstance: function(correlationID, activityName, processName, workflowName, ignoreWarnings, ReturnActivityId, SourceInstanceId, mapVariables, data) {
                        console.log('createInstance');
                        var command = new $x2Commands.createInstanceRequest(correlationID, activityName, processName, workflowName, metaData, ignoreWarnings, ReturnActivityId, SourceInstanceId, mapVariables, data);
                        return $x2WebService.sendX2RequestWithReturnedDataAsync(command);
                    },
                    completeCreateInstanceRequest: function(instanceId, correlationID, activityName, ignoreWarnings, mapVariables, data) {
                        console.log('completeCreateInstanceRequest');
                        var command = new $x2Commands.completeCreateInstanceRequest(correlationID, instanceId, metaData, activityName, ignoreWarnings, mapVariables, data);
                        return $x2WebService.sendX2RequestWithReturnedDataAsync(command);
                    },

                };

                return {
                    startActivity: operations.startActivity,
                    cancelActivity: operations.cancelActivity,
                    completeActivity: operations.completeActivity,
                    createInstance: operations.createInstance,
                    completeCreateInstanceRequest: operations.completeCreateInstanceRequest,
                };
            }
        ];

        this.$get = [function() {}];
    }]);
