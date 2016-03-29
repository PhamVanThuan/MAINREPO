'use strict';

angular.module('sahl.js.core.tileManager')
    .service('$tileManagerConfigurationService', function() {
        var operations = {
            getAllApplications: function () {},
            getApplicationConfiguration: function(applicationName) {},
            getApplicationConfigurationMenuItems: function(applicationName, roleName, capabilities) {},
            getModuleConfiguration: function(applicationName, moduleName, returnAllRoots, moduleParameters, roleName, capabilities) {},
            getRootTileConfiguration: function (applicationName, moduleName, rootTileName, businessContext, businessKeyType, businessKeyValue, roleName, capabilities) { },
            getWizardConfiguration: function (wizardName, processName, workflowName, activityName, businessContext) { }
        };

        return {
            getAllApplications: operations.getAllApplications,
            getApplicationConfiguration: operations.getApplicationConfiguration,
            getApplicationConfigurationMenuItems: operations.getApplicationConfigurationMenuItems,
            getModuleConfiguration: operations.getModuleConfiguration,
            getRootTileConfiguration: operations.getRootTileConfiguration,
            getWizardConfiguration: operations.getWizardConfiguration
        };
    });
