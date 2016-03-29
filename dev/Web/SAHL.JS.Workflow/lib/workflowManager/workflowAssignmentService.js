'use strict';
angular.module('sahl.js.workflow.workflowManager')
    .service('$workflowAssignmentService', [function() {
        var operations = {
            getActiveUsersWithCapability: function(capabilityKey) { }
        };
        return {
            getActiveUsersWithCapability: operations.getActiveUsersWithCapability
        };
    }]);
