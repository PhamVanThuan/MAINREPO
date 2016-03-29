'use strict';

angular.module('halo.start.portalpages.entity', [
        'sahl.websites.halo.services.entityManagement',
        'sahl.ui.halo.views.templates',
        'sahl.js.core.activityManagement',
        'sahl.js.core.eventAggregation',
        'sahl.js.ui.forms',
        'halo.core.start.entityActionBar',
        'halo.start.portalpages.entity.common'
    ])
    .config(['$stateProvider', function ($stateProvider) {
        var stateSettings = function(name,businessType,moduleName){
            this.url = name + '/:entityId';
            this.templateUrl = 'app/start/portalPages/entity/entity.tpl.html';
            this.controller = 'EntityCtrl';
            this.businessTypeName = businessType;
            this.moduleName = moduleName;
        };
        _.each([
            { name: 'client', businessType: 'Client', moduleName: 'Clients' },
            { name: 'task', businessType: 'Task', moduleName: 'Task' },
            { name: 'thirdparty', businessType: 'Third Party', moduleName: 'ThirdParty' }
        ],function (entity) {
            $stateProvider.state(
                'start.portalPages.' + entity.name,
                new stateSettings(entity.name, entity.businessType, entity.moduleName)
            );
        });
    }])
    .controller('EntityCtrl', [
        '$scope', '$rootScope', '$activityManager', '$timeout', '$state', '$tileManagerService', '$viewManagerService', '$stateParams', '$q','$pageFactory', '$authenticatedUser',
        function ($scope, $rootScope, $activityManager, $timeout, $state, $tileManagerService, $viewManagerService, $stateParams, $q, $pageFactory, $authenticatedUser) {
            var models = {
                currentEntity: null
            };

            var operations = {
                goToBreadcrumb: function (index) {
                    var crumb = $rootScope.entitiesModel.activeEntity.breadcrumbs.goToIndex(index);
                    var tileData = crumb.data;
                    operations.loadRootTileConfiguration(
                        tileData.name,
                        tileData.businessContext.Context,
                        tileData.businessContext.BusinessKey.KeyType,
                        tileData.businessContext.BusinessKey.Key, false
                    ).then(function () {
                        if ($state.current.name !== crumb.state) {
                            $state.go(crumb.state);
                        }
                        $activityManager.stopActivityWithKey("loading");
                    });
                },
                pageLoad: function() {
                    $rootScope.breadcrumbs.forEntity($rootScope.entitiesModel.activeEntity);
                    models.currentEntity = $rootScope.entitiesModel.activeEntity;
                    if (($scope.currentModuleName === null) || ($scope.currentModuleName !== models.currentEntity.businessKeyType)) {
                        operations.loadTileModuleConfiguration($state.current.moduleName);
                    }
                },
                loadTileModuleConfiguration: function(moduleName) {
                    $activityManager.startActivityWithKey("loading");
                    var moduleParameters = null;
                    if (models.currentEntity.entityType === "task") {
                        moduleParameters = models.currentEntity.moduleParameters;
                    }

                    $tileManagerService.loadModuleConfiguration($rootScope.tileApplicationConfiguration.Name, moduleName, false, moduleParameters,
                                                                $authenticatedUser.currentOrgRole.RoleName, $authenticatedUser.capabilities).then(function (moduleConfiguration) {
                        $scope.tileModuleConfiguration = moduleConfiguration;
                        if (moduleConfiguration.RootTileConfigurations.$values.length > 0) {
                            $scope.rootTileConfiguration = moduleConfiguration.RootTileConfigurations.$values[0];
                            if (models.currentEntity.breadcrumbs.crumbs.length === 0) {
                                operations.loadRootTileConfiguration($scope.rootTileConfiguration.name,
                                        models.currentEntity.businessContext,
                                        models.currentEntity.businessKeyType,
                                        models.currentEntity.businessKey, true,
                                        $authenticatedUser.currentOrgRole.RoleName, $authenticatedUser.capabilities)
                                    .then(function() {
                                        $activityManager.stopActivityWithKey("loading");
                                    });
                            } else {
                                operations.goToBreadcrumb(models.currentEntity.breadcrumbs.crumbs.length - 1);
                            }
                        } else {
                            $activityManager.stopActivityWithKey("loading");
                            $scope.rootTileConfiguration = null;
                        }
                    }, function() {
                        $activityManager.stopActivityWithKey("loading");
                        throw (moduleName + ' Module Configuration cannot be loaded');
                    });
                },
                loadRootTileConfiguration: function (rootTileConfigurationName, businessContext, businessKeyType, businessKey, pushToCrumbs, optionalTileData) {

                    (function (rootTileConfigurationName, businessContext, businessKeyType, businessKey, optionalTileData) {
                        $pageFactory.internal.tileDataFunction(function () {
                            operations.loadRootTileConfiguration(rootTileConfigurationName, businessContext, businessKeyType, businessKey, false, optionalTileData);
                        });
                    })(rootTileConfigurationName, businessContext, businessKeyType, businessKey, optionalTileData);

                    var deferred = $q.defer();
                    $tileManagerService.loadRootTileConfiguration($rootScope.tileApplicationConfiguration.Name, $scope.tileModuleConfiguration.Name, rootTileConfigurationName,
                                                                  businessContext, businessKeyType, businessKey,
                                                                  $authenticatedUser.currentOrgRole.RoleName, $authenticatedUser.capabilities).then(function (model) {

                        models.currentEntity.businessContext = businessContext;

                        if (model.rootTileConfigurations.$values[0].isTileBased) {
                            $scope.allRootTiles   = model.rootTileConfigurations.$values;
                            $scope.allChildTiles  = model.childTileConfigurations.$values;
                            $scope.allTileActions = $tileManagerService.findTileActionsForRootTileConfigurationModel(model);
                            deferred.resolve();

                            if (pushToCrumbs) {
                                var name = models.currentEntity.breadcrumbs.crumbs.length !== 0 ? $scope.allRootTiles[0].name : $state.current.businessTypeName;
                                operations.pushToBreadCrumb(name,true);
                            }
                        }
                        else {
                            deferred.resolve();
                            var newState = model.rootTileConfigurations.$values[0].nonTilePageState;
                            $state.go($state.current.name + '.common.page', {
                                businessKey : businessKey,
                                model: model.rootTileConfigurations.$values[0],
                                currentModel : models.currentEntity,
                                previousState: $state.current.name,
                                tileData: optionalTileData
                            });
                            operations.pushToBreadCrumb(model.rootTileConfigurations.$values[0].name,false);
                        }
                    }, function() {
                        $activityManager.stopActivityWithKey("loading");
                        throw $scope.rootTileConfiguration.Name + ' Root Tile Configuration not found';
                    });
                    return deferred.promise;
                },
                view: function(model) {
                    return $viewManagerService.getView(model.dataModelType);
                },
                gridFilter:function(data, properties) {
                    return data.map(function(oldRowObject) {
                        var newObject = {};
                        if(properties.length > 0){
                            for (var prop in properties) {
                                if (properties.hasOwnProperty(prop)) {
                                    newObject[properties[prop]] = oldRowObject[properties[prop]];
                                }
                            }
                        }
                        return newObject;
                    });
                },
                findTileDrilldownAction: function(tile) {
                    if (tile && tile.tileActions && tile.tileActions.$values && tile.tileActions.$values.length !== 0) {
                        var tileActions = tile.tileActions.$values;
                        for (var i = 0; i < tileActions.length; i++) {
                            if (tileActions[i].actionType === 'DrillDown') {
                                return tileActions[i];
                            }
                        }
                    }
                    return null;
                },
                pushToBreadCrumb: function (name,isRoot) {
                    models.currentEntity.breadcrumbs.push(
                        name,
                        $state.current.name,
                        $scope.allRootTiles[0],
                        isRoot
                    );
                }
            };

            var actions = {
                tileDrilldown: function(tile) {
                    var tileAction = operations.findTileDrilldownAction(tile);
                    if (tileAction === null) {
                        return;
                    }
                    $scope.tileDrilldown = function() {};

                    operations.loadRootTileConfiguration(tileAction.rootTileConfigurationName,
                                                         tile.businessContext.Context,
                                                         tile.businessContext.BusinessKey.KeyType,
                                                         tile.businessContext.BusinessKey.Key,
                                                         true,
                                                         tile.tileData)
                    .then(function () {
                        $scope.tileDrilldown = actions.tileDrilldown;
                    });
                },
                
                performTileAction: function (tile) {
                    var newState = $state.current.name + '.common.' + tile.tileAction.actionType.toLowerCase();
                    operations.pushToBreadCrumb(tile.tileAction.actionType + ' : ' + tile.tileAction.name, false);
                    $state.go(newState, {
                        model: tile.tileConfiguration,
                        tileAction : tile.tileAction,
                        targetConfiguration: (tile.tileAction.tileConfiguration!=null?tile.tileAction.tileConfiguration.$type:''),
                        businessContext : models.currentEntity.businessContext,
                        previousState: $state.current.name
                    });
                }
            };

            $scope.checkEmpty = function(value) {
                if (!_.isUndefined(value)) {
                    return value;
                } else {
                    return " ";
                }
            };

            $scope.tileDrilldown = actions.tileDrilldown;
            $scope.goToBreadcrumb = operations.goToBreadcrumb;
            $scope.performTileAction = actions.performTileAction;
            $scope.view = operations.view;
            $scope.gridFilter = operations.gridFilter;
            operations.pageLoad();
            
        }
    ]);
