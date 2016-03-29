angular.module('sahl.js.core.tileManager')
    .service('$tileManagerService', ['$tileManagerConfigurationService', '$q',
        function($tileManagerConfigurationService, $q) {
            var operations = {
                loadApplicationConfiguration: function(applicationName) {
                    var deferred = $q.defer();

                    $tileManagerConfigurationService.getApplicationConfiguration(applicationName).then(function(applicationConfiguration) {
                        deferred.resolve(applicationConfiguration);
                    }, function() {
                        deferred.reject();
                    });

                    return deferred.promise;
                },
                loadModuleConfiguration: function(applicationName, moduleName, returnAllRoots, moduleParameters, roleName, capabilities) {
                    var deferred = $q.defer();

                    $tileManagerConfigurationService.getModuleConfiguration(applicationName, moduleName, returnAllRoots, moduleParameters, roleName, capabilities).then(function(moduleConfiguration) {
                        deferred.resolve(moduleConfiguration);
                    }, function() {
                        deferred.reject();
                    });

                    return deferred.promise;
                },
                loadApplicationMenuItems: function(applicationName, roleName, capabilities) {
                    var deferred = $q.defer();

                    $tileManagerConfigurationService.getApplicationConfigurationMenuItems(applicationName, roleName, capabilities).then(function(model) {
                        deferred.resolve(model);
                    }, function() {
                        deferred.reject();
                    });

                    return deferred.promise;
                },
                loadRootTileConfiguration: function(applicationName, moduleName, rootTileName, businessContext, businessKeyType, businessKeyValue, roleName, capabilities) {
                    var deferred = $q.defer();

                    $tileManagerConfigurationService.getRootTileConfiguration(applicationName, moduleName, rootTileName, businessContext, businessKeyType, businessKeyValue, roleName, capabilities).then(function(model) {
                        deferred.resolve(model);
                    }, function() {
                        deferred.reject();
                    });

                    return deferred.promise;
                },
                loadWizardConfiguration: function(wizardName, processName, workflowName, activityName, businessContext) {
                    var deferred = $q.defer();

                    $tileManagerConfigurationService.getWizardConfiguration(wizardName, processName, workflowName, activityName, businessContext).then(function(model) {
                        deferred.resolve(model);
                    }, function() {
                        deferred.reject();
                    });

                    return deferred.promise;
                },
                findTileActionsForRootTileConfigurationModel: function(model) {
                    var allTileActions = [];

                    if (model === null) {
                        return allTileActions;
                    }

                    if (model.rootTileConfigurations !== null && model.rootTileConfigurations.$values.length > 0) {
                        var allRootTiles = model.rootTileConfigurations.$values;

                        for (var rootLoop = 0; rootLoop < allRootTiles.length; rootLoop++) {
                            var rootTileConfiguration = allRootTiles[rootLoop];
                            processTileActions(rootTileConfiguration, allTileActions);
                        }
                    }

                    if (model.childTileConfiguratiosn !== null && model.childTileConfigurations.$values.length > 0) {
                        var allChildTiles = model.childTileConfigurations.$values;

                        for (var childLoop = 0; childLoop < allChildTiles.length; childLoop++) {
                            var childTileConfiguration = allChildTiles[childLoop];
                            processTileActions(childTileConfiguration, allTileActions);
                        }
                    }

                    return allTileActions;
                }
            };

            function processTileActions(tileConfiguration, tileActionList) {
                if (tileConfiguration === null ||
                    tileConfiguration.tileActions === null ||
                    tileConfiguration.tileActions.length === 0) {
                    return [];
                }

                var tileActions = tileConfiguration.tileActions.$values;

                for (var tileActionLoop = 0; tileActionLoop < tileActions.length; tileActionLoop++) {
                    if (tileActions[tileActionLoop].actionType === 'Edit' ||
                        tileActions[tileActionLoop].actionType === 'Wizard' ||
                        tileActions[tileActionLoop].actionType === 'Workflow') {
                        var newTileAction = {
                            tileAction: tileActions[tileActionLoop],
                            tileConfiguration: tileConfiguration
                        };

                        tileActionList[tileActionList.length] = newTileAction;
                    }
                }
            }

            return {
                loadApplicationConfiguration: operations.loadApplicationConfiguration,
                loadApplicationMenuItems: operations.loadApplicationMenuItems,
                loadModuleConfiguration: operations.loadModuleConfiguration,
                loadRootTileConfiguration: operations.loadRootTileConfiguration,
                loadWizardConfiguration: operations.loadWizardConfiguration,
                findTileActionsForRootTileConfigurationModel: operations.findTileActionsForRootTileConfigurationModel
            };
        }
    ]);
