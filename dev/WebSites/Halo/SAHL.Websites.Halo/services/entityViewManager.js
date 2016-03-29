'use strict';
angular.module('sahl.websites.halo.services.entityViewManagement', [

    ])
    .service('$entityViewManagerService', ['$rootScope', '$logger', '$eventAggregatorService', '$entityManagementEvents', '$state',
        function($rootScope, $logger, $eventAggregator, $entityManagementEvents, $state) {

            var eventHandlers = {
                onEntityRemoved: function(entityToBeRemoved) {
                    if (!_.isNull($rootScope.entitiesModel) && $rootScope.entitiesModel.entities.length === 0) {

                        $state.go('start.portalPages.search', null, {
                            location: true,
                            inherit: false
                        });
                    }
                },
                onEntityActivated: function(activatedEntity) {
                    if (!_.isNull(activatedEntity)) {
                        // entities
                        if (activatedEntity.entityType === 'client') {
                            $state.go('start.portalPages.client', {
                                entityId: activatedEntity.id,
                                entityData: activatedEntity.entityData
                            }, {
                                location: true,
                                inherit: false
                            });
                        } else if (activatedEntity.entityType === 'thirdparty') {
                            // third parties (businessKeyType is a placeholder for real value)
                            $state.go('start.portalPages.thirdparty', {
                                entityId: activatedEntity.id,
                                entityData: activatedEntity.entityData
                            }, {
                                location: true,
                                inherit: false
                            });
                        } else if (activatedEntity.entityType === 'task') {
                            // workflowTasks (businessKeyType is a placeholder for real value)
                            $state.go('start.portalPages.task', {
                                entityId: activatedEntity.id,
                                entityData: activatedEntity.entityData
                            }, {
                                location: true,
                                inherit: false
                            });
                        }
                    }
                }
            };

            return {
                start: function() {
                    // subscribe to the entity management events
                    $eventAggregator.subscribe($entityManagementEvents.onEntityRemoved, eventHandlers.onEntityRemoved);
                    $eventAggregator.subscribe($entityManagementEvents.onEntityActivated, eventHandlers.onEntityActivated);

                    $rootScope.isEntityViewActive = function(entity) {
                        if (!_.isNull(entity) && !_.isNull($rootScope.entitiesModel.activeEntity) && entity.id === $rootScope.entitiesModel.activeEntity.id) {
                            return true;
                        }

                        return false;
                    };
                }
            };
        }
    ]);
