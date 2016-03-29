'use strict';
angular.module('sahl.websites.halo.services.workflowManagement', [
    'halo.webservices',
    'sahl.js.core.logging',
    'SAHL.Services.Interfaces.WorkflowTask.commands',
    'SAHL.Services.Interfaces.WorkflowTask.queries',
    'sahl.js.core.userManagement'
]).service('$workflowTaskService', ['$q', '$workflowTaskWebService', '$workflowTaskCommands', '$workflowTaskQueries', '$userManagerService',
    function ($q, $workflowTaskWebService, $workflowTaskCommands, $workflowTaskQueries, $userManagerService) {
        var workflowtasks = [];
        var validResponse = function (response) {
            return response &&
                response.data &&
                response.data.ReturnData &&
                response.data.ReturnData.Results &&
                response.data.ReturnData.Results.$values;
        };

        var internal = {
            getAssignedTasks: function (username, capabilities) {
                var deferred = $q.defer();
                var query = new $workflowTaskQueries.GetAssignedTasksForUserQuery(username, capabilities);
                $workflowTaskWebService.sendQueryAsync(query).then(function (response) {
                    if (validResponse(response)) {
                        var tasks = response.data.ReturnData.Results.$values[0].BusinessProcesses;
                        deferred.resolve(tasks);
                    } else {
                        deferred.reject();
                    }
                }, deferred.reject);
                return deferred.promise;
            }
        };

        var operations = {
            getAssignedTasks: function () {
                var deferred = $q.defer();
                var username = $userManagerService.getAuthenticatedUser().fullAdName;
                var capabilities = $userManagerService.getAuthenticatedUser().capabilities;

                internal.getAssignedTasks(username, capabilities).then(function (tasks) {
                    workflowtasks = tasks;
                    deferred.resolve(workflowtasks);
                }, deferred.reject);

                return deferred.promise;
            }
        };

        return {
            getAssignedTasks: operations.getAssignedTasks
        };
    }
]);
