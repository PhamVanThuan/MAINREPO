'use strict';
angular.module('sahl.js.workflow.workflowManager')
    .service('$workflowActivityManager', ['$q', '$workflowService', '$toastManagerService', '$wizardFactory', function ($q, $workflowService, $toastManagerService, $wizardFactory) {
        var severityMap = {
            0: "notice",
            1: "error",
            2: "info",
            3: "error"
        };

        var internal = {
            displayMessage: function (systemMessage) {
                if (severityMap[systemMessage.Severity]) {
                    $toastManagerService[severityMap[systemMessage.Severity]]({
                        text: systemMessage.Message
                    });
                }
            }
        }
        var operations = {
            start: function (instanceId, correlationID, activityName, ignoreWarnings, mapVariables, data) {
                var deferred = $q.defer();
                $wizardFactory.setCanCancelWorkFlowActivity(true);
                
                $workflowService.startActivity(instanceId, correlationID, activityName, ignoreWarnings, mapVariables, data).then(
                    function (response) {
                        if (response.data.ReturnData.SystemMessages != null) {
                            $wizardFactory.setCanCancelWorkFlowActivity(false);
                            
                            _.each(response.data.ReturnData.SystemMessages.systemMessages.$values, function (message) {
                                internal.displayMessage(message);
                            });
                        }
                        if (response.data.ReturnData.IsErrorResponse) {
                            $wizardFactory.setCanCancelWorkFlowActivity(false);
                            
                            if (response.data.ReturnData.SystemMessages.systemMessages.$values.length === 0) {
                                $toastManagerService.error({
                                    text: response.data.ReturnData.Message
                                });
                            }
                            deferred.reject(response);
                        } else {
                            deferred.resolve(response);
                        }
                    },
                    function (response) {
                        $toastManagerService.error({
                            text: 'Cannot connect to the workflow service. Please try again later.'
                        });
                        $wizardFactory.setCanCancelWorkFlowActivity(false);
                        deferred.reject(response);
                    });
                return deferred.promise;
            },
            cancel: function (instanceId, correlationID, activityName, ignoreWarnings, mapVariables, data) {
                var deferred = $q.defer();
                if(!$wizardFactory.internal.canCancelWorkFlow()){
                    deferred.resolve({message: "Workflow Activity was not started, so it did not need to be cancelled"});
                    return deferred.promise;
                }
                
                $workflowService.cancelActivity(instanceId, correlationID, activityName, ignoreWarnings, mapVariables, data).then(
                    function (response) {

                        if (response.data.ReturnData.SystemMessages != null) {
                            _.each(response.data.ReturnData.SystemMessages.systemMessages.$values, function (message) {
                                internal.displayMessage(message);
                            });
                        }

                        if (response.data.ReturnData.IsErrorResponse) {

                            if (response.data.ReturnData.SystemMessages.systemMessages.$values.length === 0) {
                                $toastManagerService.error({
                                    text: response.data.ReturnData.Message
                                });
                            }
                            deferred.reject(response);
                        } else {
                            deferred.resolve(response);
                        }
                    },
                    function (response) {
                        $toastManagerService.error({
                            text: 'Cannot connect to the workflow service. Please try again later.'
                        });
                        deferred.reject(response);
                    });
                return deferred.promise;
            },
            complete: function (instanceId, correlationID, activityName, ignoreWarnings, mapVariables, data) {
                var deferred = $q.defer();
                $workflowService.completeActivity(instanceId, correlationID, activityName, ignoreWarnings, mapVariables, data).then(
                    function (response) {
                        if (response.data.ReturnData.SystemMessages != null) {
                            _.each(response.data.ReturnData.SystemMessages.systemMessages.$values, function (message) {
                                internal.displayMessage(message);
                            });
                        }

                        if (response.data.ReturnData.IsErrorResponse) {
                            if (response.data.ReturnData.SystemMessages.systemMessages.$values.length === 0) {
                                $toastManagerService.error({
                                    text: response.data.ReturnData.Message
                                });
                            }
                            deferred.reject(response);
                        } else {
                            deferred.resolve(response);
                        }
                    },
                    function (response) {
                        $toastManagerService.error({
                            text: 'Cannot connect to the workflow service. Please try again later.'
                        });
                        deferred.reject(response);
                    });
                return deferred.promise;
            }
        };
        return {
            start: operations.start,
            cancel: operations.cancel,
            complete: operations.complete
        };
    }]);