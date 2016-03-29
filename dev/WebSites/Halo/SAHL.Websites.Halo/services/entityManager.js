'use strict';
angular.module('sahl.websites.halo.services.entityManagement', [

    ])
    .service('$entityManagerService', ['$rootScope', '$eventAggregatorService', '$entityManagementEvents',
        function($rootScope, $eventAggregator, $entityManagementEvents) {
            return {
                start: function() {
                    // on start we should setup the $rootScope data model
                    $rootScope.entitiesModel = {
                        activeEntity: null,
                        entities: []
                    };
                },
                createEntity: function(displayName, businessKeyType, businessKey, contextKey, entityType, entityData, moduleParameters) {
                    // construct the JSON model for an entity
                    return {
                        id: businessKey,
                        displayName: displayName,
                        businessContext: contextKey,
                        businessKeyType: businessKeyType,
                        businessKey: businessKey,
                        entityType: entityType,
                        entityData: entityData,
                        moduleParameters: moduleParameters

                    };
                },
                addEntity: function(entityToAdd) {
                    // check if the entity already exists in our list
                    var existing = _.findWhere($rootScope.entitiesModel.entities, {
                        id: entityToAdd.id
                    });
                    if (_.isUndefined(existing)) {
                        $rootScope.entitiesModel.entities.unshift(entityToAdd);
                        $eventAggregator.publish($entityManagementEvents.onEntityAdded, entityToAdd);
                    }
                },
                removeAllEntity: function () {
                    $rootScope.entitiesModel.entities = [];
                },
                removeEntity: function(entityToRemove) {
                    // remove the entity from the list if it exists
                    // find the index of the entity to remove

                    var index = _.indexOf($rootScope.entitiesModel.entities, entityToRemove);
                    if (index !== -1) {
                        $rootScope.entitiesModel.entities = _.reject($rootScope.entitiesModel.entities, function(entity) {
                            return entity.id === entityToRemove.id;
                        });

                        var activatedEntity = null;
                        if ($rootScope.entitiesModel.entities.length === 0) {
                            $rootScope.entitiesModel.activeEntity = null;
                        } else {
                            if (index === 0) {
                                activatedEntity = $rootScope.entitiesModel.entities[0];
                            } else if (index >= $rootScope.entitiesModel.entities.length) {
                                activatedEntity = $rootScope.entitiesModel.entities[$rootScope.entitiesModel.entities.length - 1];
                            } else {
                                activatedEntity = $rootScope.entitiesModel.entities[index - 1];
                            }
                        }
                        $eventAggregator.publish($entityManagementEvents.onEntityRemoved, entityToRemove);
                        if (!_.isNull(activatedEntity)) {
                            if(!_.isNull($rootScope.entitiesModel.activeEntity) && (entityToRemove.id === $rootScope.entitiesModel.activeEntity.id)) {
                                this.makeEntityActive(activatedEntity);                            
                            };
                        }
                    }
                },
                makeEntityActive: function(entityToMakeActive) {
                    var existing = _.findWhere($rootScope.entitiesModel.entities, {
                        id: entityToMakeActive.id
                    });
                    if (!_.isUndefined(existing)) {
                        $rootScope.entitiesModel.activeEntity = entityToMakeActive;
                        $eventAggregator.publish($entityManagementEvents.onEntityActivated, entityToMakeActive);
                    }
                },
                makeEntityActiveById: function(idOfEntityToMakeActive) {
                    var existing = _.findWhere($rootScope.entitiesModel.entities, {
                        id: parseInt(idOfEntityToMakeActive)
                    });
                    if (!_.isUndefined(existing)) {
                        $rootScope.entitiesModel.activeEntity = existing;
                        $eventAggregator.publish($entityManagementEvents.onEntityActivated, existing);
                    }
                },
                deactivateEntity: function() {
                    $rootScope.entitiesModel.activeEntity = null;
                }
            };
        }
    ]);
