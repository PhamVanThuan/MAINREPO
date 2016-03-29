'use strict';

angular.module('halo.start.portalpages.myhalo', [
        'halo.logging',

        
        'sahl.js.ui.forms'
    ])
    .config(['$stateProvider',
        function ($stateProvider) {
            $stateProvider.state('start.portalPages.myhalo', {
                url: 'myhalo/{modulename}',
                templateUrl: 'app/start/portalPages/myhalo/myhalo.tpl.html',
                controller: 'MyHaloCtrl'
            });
        }
    ])
    .controller('MyHaloCtrl', ['$scope', '$tileManagerService', '$viewManagerService', '$logger', '$stateParams', '$state', '$authenticatedUser',
        function ($scope, $tileManagerService, $viewManagerService, $logger, $stateParams, $state, $authenticatedUser) {
            var pageModuleName = $stateParams.modulename;

            var operations = {
                pageLoad: function () {
                    if ($scope.tileApplicationConfiguration === undefined ||
                        $scope.tileApplicationConfiguration === null ||
                        $scope.currentModuleName === undefined ||
                        $scope.currentModuleName === null ||
                        $scope.tileApplicationConfiguration.Name !== 'My Halo') {
                        operations.loadApplication();
                    }
                },
                loadApplication: function() {
                    $tileManagerService.loadApplicationConfiguration('My Halo').then(function(applicationConfiguration) {
                        $scope.tileApplicationConfiguration = applicationConfiguration;

                        operations.loadMenuItems(applicationConfiguration.name);
                        operations.loadModuleConfiguration(pageModuleName);
                    }, function() {
                        throw 'Unable to load the My Halo Application configuration';
                    });
                },
                loadModuleConfiguration: function(moduleName) {
                    $tileManagerService.loadModuleConfiguration($scope.tileApplicationConfiguration.Name, moduleName, true, "",
                                                                $authenticatedUser.currentOrgRole.RoleName, $authenticatedUser.capabilities).then(function (moduleConfiguration) {
                        $scope.tileModuleConfiguration = moduleConfiguration;

                        if (!moduleConfiguration.IsTileBased) {
                            var newState = 'start.portalPages.myhalo.' + moduleConfiguration.NonTilePageState;
                            $state.go(newState, { modulename: moduleName });
                        } else {
                            if ($scope.tileModuleConfiguration !== null &&
                                $scope.tileModuleConfiguration.RootTileConfigurations !== null &&
                                $scope.tileModuleConfiguration.RootTileConfigurations.$values !== null) {
                                $scope.rootTiles = $scope.tileModuleConfiguration.RootTileConfigurations.$values;
                            }
                        }
                    }, function() {
                        throw (moduleName + ' Module Configuration cannot be loaded');
                    });
                },
                loadMenuItems: function() {
                    $tileManagerService.loadApplicationMenuItems($scope.tileApplicationConfiguration.Name,
                                                                 $authenticatedUser.currentOrgRole.RoleName, $authenticatedUser.capabilities).then(function (menuItems) {
                        $scope.menuItems = menuItems;
                    }, function () {
                        throw ('Menu Items for ' + $scope.tileApplicationConfiguration.Name + ' Application Configuration cannot be loaded');
                    });
                }
            };

            operations.pageLoad();

            $scope.view = function (dataModelType) {
                if (dataModelType !== null && dataModelType !== undefined){
                    return $viewManagerService.getDashboardView(dataModelType);
                }
                return "";
            };
        }
    ]);
