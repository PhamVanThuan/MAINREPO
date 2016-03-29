'use strict';
angular.module('sahl.js.core.workflowAssignmentManagement', [
    'SAHL.Services.Interfaces.WorkflowAssignmentDomain.queries', 'halo.webservices'
]).provider('$workflowAssignmentManagerDecoration', [function () {
    this.decoration = ['$delegate','$q', '$workflowAssignmentDomainQueries', '$workflowAssignmentDomainWebService',
    function ($delegate, $q, $workflowAssignmentDomainQueries, $workflowAssignmentDomainWebService) {

        var operations = {
            getActiveUsersWithCapability: function (capabilityKey) {
                var deferred = $q.defer();
                var query = new $workflowAssignmentDomainQueries.GetActiveUsersWithCapabilityQuery(capabilityKey);
                $workflowAssignmentDomainWebService.sendQueryAsync(query).then(function (data) {
                    deferred.resolve(data.data.ReturnData.Results.$values);
                });
                return deferred.promise;
            },
            getCurrentlyAssignedUserForInstance: function (instanceId) {
                var deferred = $q.defer();
                var query = new $workflowAssignmentDomainQueries.GetCurrentlyAssignedUserForInstanceQuery(instanceId);
                $workflowAssignmentDomainWebService.sendQueryAsync(query).then(function (data) {
                    deferred.resolve(data.data.ReturnData.Results.$values);
                });
                return deferred.promise;
            }
        };
        return {
            getActiveUsersWithCapability: operations.getActiveUsersWithCapability,
            getCurrentlyAssignedUserForInstance: operations.getCurrentlyAssignedUserForInstance
        };
    }
    ];
    this.$get = [function () { }];
}]);
