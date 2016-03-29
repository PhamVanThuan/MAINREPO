'use strict';

angular.module('sahl.websites.halo.services.haloService', [
        'halo.webservices',
        'sahl.js.core.logging',
        'SAHL.Services.Interfaces.Halo.queries',
        'SAHL.Services.Interfaces.Halo.sharedmodels'
    ])
    .provider('$haloService', [
        function() {
            this.decoration = [
                '$delegate', '$haloWebService', '$haloQueries', '$haloSharedModels', '$q',
                function($delegate, $haloWebService, $haloQueries, $haloSharedModels, $q) {

                    var operations = {
                        getAllApplications: function() {
                            var deferred = $q.defer();
                            var allApplicationsQuery = new $haloQueries.GetAllApplicationsQuery();

                            $haloWebService.sendQueryAsync(allApplicationsQuery).then(function(data) {
                                deferred.resolve(data.data.ReturnData.Results.$values);
                            });

                            return deferred.promise;
                        },
                        getApplicationConfiguration: function(applicationName) {
                            var deferred = $q.defer();
                            var applicationQuery = new $haloQueries.GetApplicationConfigurationQuery(applicationName);

                            $haloWebService.sendQueryAsync(applicationQuery).then(function(applicationData) {
                                if (applicationData &&
                                    applicationData.data &&
                                    applicationData.data.ReturnData &&
                                    applicationData.data.ReturnData.Results &&
                                    applicationData.data.ReturnData.Results.$values) {
                                    
                                    deferred.resolve(applicationData.data.ReturnData.Results.$values[0].HaloApplicationModel);
                                } else {
                                    deferred.resolve(null);
                                }
                            }, function() {
                                deferred.reject();
                            });

                            return deferred.promise;
                        },
                        getApplicationConfigurationMenuItems: function (applicationName, roleName, capabilities) {
                            var deferred  = $q.defer();
                            var roleModel = new $haloSharedModels.HaloRoleModel('', roleName, capabilities);
                            var query     = new $haloQueries.GetApplicationConfigurationMenuItemsQuery(applicationName, roleModel);

                            $haloWebService.sendQueryAsync(query).then(function(menuItemsData) {
                                if (menuItemsData &&
                                    menuItemsData.data &&
                                    menuItemsData.data.ReturnData &&
                                    menuItemsData.data.ReturnData.Results &&
                                    menuItemsData.data.ReturnData.Results.$values) {

                                    deferred.resolve(menuItemsData.data.ReturnData.Results.$values);
                                } else {
                                    deferred.resolve(null);
                                }
                            });

                            return deferred.promise;
                        },
                        getModuleConfiguration: function (applicationName, moduleName, returnAllRoots, moduleParameters, roleName, capabilities) {
                            var deferred = $q.defer();
                            var roleModel = new $haloSharedModels.HaloRoleModel('', roleName, capabilities);
                            var moduleQuery = new $haloQueries.GetModuleConfigurationQuery(applicationName, moduleName, returnAllRoots, moduleParameters, roleModel);

                            $haloWebService.sendQueryAsync(moduleQuery).then(function(moduleData) {
                                if (moduleData &&
                                    moduleData.data &&
                                    moduleData.data.ReturnData &&
                                    moduleData.data.ReturnData.Results &&
                                    moduleData.data.ReturnData.Results.$values) {

                                    deferred.resolve(moduleData.data.ReturnData.Results.$values[0].ModuleConfiguration);
                                } else {
                                    deferred.resolve(null);
                                }
                            }, function() {
                                deferred.reject();
                            });

                            return deferred.promise;
                        },
                        getTileData: function(applicationName, moduleName, tileName, businessContext, businessKeyType, businessKeyValue) {
                            var deferred = $q.defer();
                            var businessKey = new $haloSharedModels.BusinessContext(businessContext, businessKeyType, businessKeyValue);
                            var query = new $haloQueries.GetTileDataQuery(applicationName, moduleName, tileName, businessKey);

                            $haloWebService.sendQueryAsync(query).then(function(queryResult) {
                                if (queryResult &&
                                    queryResult.data &&
                                    queryResult.data.ReturnData &&
                                    queryResult.data.ReturnData.Results &&
                                    queryResult.data.ReturnData.Results.$values) {

                                    deferred.resolve(queryResult.data.ReturnData.Results.$values);
                                } else {
                                    deferred.resolve(null);
                                }
                            });

                            return deferred.promise;
                        },
                        getRootTileConfiguration: function (applicationName, moduleName, rootTileName, businessContext, businessKeyType, businessKeyValue,
                                                            roleName, capabilities) {
                            var deferred    = $q.defer();
                            var businessKey = new $haloSharedModels.BusinessContext(businessContext, businessKeyType, businessKeyValue);
                            var roleModel   = new $haloSharedModels.HaloRoleModel('', roleName, capabilities);
                            var query       = new $haloQueries.GetRootTileConfigurationQuery(applicationName, moduleName, rootTileName, businessKey, roleModel);

                            $haloWebService.sendQueryAsync(query).then(function (queryResult) {
                                if (queryResult &&
                                    queryResult.data &&
                                    queryResult.data.ReturnData &&
                                    queryResult.data.ReturnData.Results &&
                                    queryResult.data.ReturnData.Results.$values) {

                                    deferred.resolve(queryResult.data.ReturnData.Results.$values[0].RootTileConfiguration);
                                } else {
                                    deferred.resolve(null);
                                }
                            });

                            return deferred.promise;
                        },
                       
                        getWizardConfiguration: function (wizardName, processName, workflowName, activityName, businessContext) {
                            var deferred = $q.defer();
                            var query = new $haloQueries.GetWizardConfigurationQuery(wizardName, processName, workflowName, activityName, businessContext);
                            $haloWebService.sendQueryAsync(query).then(function (queryResult) {
                                if (queryResult &&
                                    queryResult.data &&
                                    queryResult.data.ReturnData &&
                                    queryResult.data.ReturnData.Results &&
                                    queryResult.data.ReturnData.Results.$values) {

                                    deferred.resolve(queryResult.data.ReturnData.Results.$values);
                                } else {
                                    deferred.reject();
                                }
                            }, deferred.reject);
                            return deferred.promise;
                        }
                };
                    return {
                        getAllApplications: operations.getAllApplications,
                        getApplicationConfiguration: operations.getApplicationConfiguration,
                        getModuleConfiguration: operations.getModuleConfiguration,
                        getApplicationConfigurationMenuItems: operations.getApplicationConfigurationMenuItems,
                        getTileData: operations.getTileData,
                        getRootTileConfiguration: operations.getRootTileConfiguration,
                        getLoanTransactionDetailData:operations.getLoanTransactionDetailData,
                        getArrearTransactionDetailData: operations.getArrearTransactionDetailData,
                        getWizardConfiguration : operations.getWizardConfiguration
                    };
                }
            ];

            this.$get = [function() {}];
        }
    ]);

