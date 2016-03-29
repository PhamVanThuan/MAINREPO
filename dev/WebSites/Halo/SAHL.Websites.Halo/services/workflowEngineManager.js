'use strict';
angular.module('sahl.js.core.workflowEngineManagement', [
    'SAHL.Services.Interfaces.X2Service.commands', 'halo.webservices'
]).provider('$workflowManagerDecoration', [function () {
    this.decoration = ['$delegate', '$x2WebService', '$x2Commands', '$authenticatedUser',
        function ($delegate, $x2WebService, $x2Commands, $authenticatedUser) {
            var metaData = {
                $type: 'SAHL.Core.Services.ServiceRequestMetadata,SAHL.Core',
                username: $authenticatedUser.fullAdName,
                currentuserrole: $authenticatedUser.currentOrgRole.RoleName,
                userorganisationstructurekey: $authenticatedUser.currentOrgRole.UserOrganisationStructureKey,
                currentusercapabilities: $authenticatedUser.capabilities.toString()
            };

            var operations = {
                startActivity: function (instanceId, correlationID, activityName, ignoreWarnings, mapVariables, data) {
                    var command = new $x2Commands.startActivityRequest(correlationID, instanceId, metaData, activityName, ignoreWarnings, mapVariables, data);
                    return $x2WebService.sendX2RequestWithReturnedDataAsync(command);
                },
                cancelActivity: function (instanceId, correlationID, activityName, ignoreWarnings, mapVariables, data) {
                    var command = new $x2Commands.cancelActivityRequest(correlationID, instanceId, metaData, activityName, ignoreWarnings, mapVariables, data);
                    return $x2WebService.sendX2RequestWithReturnedDataAsync(command);
                },
                completeActivity: function (instanceId, correlationID, activityName, ignoreWarnings, mapVariables, data) {
                    var command = new $x2Commands.completeActivityRequest(correlationID, instanceId, metaData, activityName, ignoreWarnings, mapVariables, data);
                    return $x2WebService.sendX2RequestWithReturnedDataAsync(command);
                }
            };
            return {
                startActivity: operations.startActivity,
                cancelActivity: operations.cancelActivity,
                completeActivity: operations.completeActivity
            };
        }
    ];
    this.$get = [function () { }];
}]);
