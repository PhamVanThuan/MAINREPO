'use strict';
describe('[sahl.websites.halo.services.entityManagement]', function() {
    beforeEach(module('sahl.websites.halo.services.entityManagement'));
    beforeEach(module('sahl.js.core.logging'));
    beforeEach(module('sahl.js.core.eventAggregation'));
    beforeEach(module('sahl.websites.halo.events'));
    beforeEach(module('sahl.websites.halo.events.entityManagement'));

    beforeEach(inject(function($injector, $q) {}));

    describe(' - (Service: entityManager)-', function() {
        var $rootScope, $entityManager, $eventAggregator, $entityManagementEvents;
        beforeEach(inject(function($injector, $q) {
            $rootScope = $injector.get('$rootScope');
            $entityManager = $injector.get('$entityManagerService');
            $eventAggregator = $injector.get('$eventAggregatorService');
            $entityManagementEvents = $injector.get('$entityManagementEvents');
        }));

        // Creating the manager
        describe('when a new entityManager has been created', function() {

            it('it should be startable', function () {
                expect($entityManager.start).not.toBeNull();
            });
        });

        // Starting the manager
        describe('when the entityManager is started', function() {

            it('it should add the entities model to the rootScope', function () {
                $entityManager.start();
                expect($rootScope.entitiesModel).not.toBeNull();
            });

            it('it should not have an active entity', function () {
                $entityManager.start();
                expect($rootScope.entitiesModel.activeEntity).toBeNull();
            });
        });


        // Creating an entity model
        describe('when an entityModel is created', function() {
            var entityModel = null;
            beforeEach(function() {
                entityModel = $entityManager.createEntity('Mr Smith', 5, 54321, '');
            });

            it('it should set the displayName property', function() {
                expect(entityModel.displayName).toEqual('Mr Smith');
            });

            it('it should set the businessKeyType property', function() {
                expect(entityModel.businessKeyType).toEqual(5);
            });

            it('it should set the businessKey property', function() {
                expect(entityModel.businessKey).toEqual(54321);
            });

            it('it should correctly set the id property', function() {
                expect(entityModel.id).toEqual(54321);
            });
        });

        // Adding a entity
        describe('when adding a entity', function() {
            var entityModel = null;
            beforeEach(function() {
                $entityManager.start();
                entityModel = $entityManager.createEntity('Mr Smith', 5, 54321, '');
            });

            it('it should add the entity to the list of entities being managed', function() {
                $entityManager.addEntity(entityModel);
                expect($rootScope.entitiesModel.entities).toContain(entityModel);
            });

            it('it should raise the onEntityAdded event', function() {
                spyOn($eventAggregator, 'publish').and.callThrough();
                $entityManager.addEntity(entityModel);
                expect($eventAggregator.publish).toHaveBeenCalledWith($entityManagementEvents.onEntityAdded, entityModel);
            });

            describe(' - given the entity has already been added', function() {
                it('it should not add the entity a second time and not raise the onEntityAdded event', function() {
                    var spy = spyOn($eventAggregator, 'publish').and.callThrough();
                    $entityManager.addEntity(entityModel);
                    $entityManager.addEntity(entityModel);
                    expect($rootScope.entitiesModel.entities.length).toEqual(1);
                    expect(spy.calls.count()).toEqual(1);
                });
            });
        });

        // Removing a entity
        describe('when removing a entity', function() {
            var entityModel = null;
            var entityModel1 = null;
            var entityModel2 = null;

            beforeEach(function() {
                $entityManager.start();
                entityModel = $entityManager.createEntity('Mr Smith', 5, 54321, '');
            });

            it('it should remove the entity from the list of entities being managed', function () {
                $entityManager.addEntity(entityModel);
                $entityManager.removeEntity(entityModel);
                expect($rootScope.entitiesModel.entities).not.toContain(entityModel);
            });

            it('it should raise the onEntityRemoved event', function() {
                spyOn($eventAggregator, 'publish').and.callThrough();
                $entityManager.addEntity(entityModel);
                $entityManager.removeEntity(entityModel);
                expect($eventAggregator.publish).toHaveBeenCalledWith($entityManagementEvents.onEntityRemoved, entityModel);
            });

            describe(' - given the entity has not already been added', function() {
                it('it should not raise the onEntityRemoved event', function() {
                    var spy = spyOn($eventAggregator, 'publish').and.callThrough();
                    $entityManager.removeEntity(entityModel);

                    expect($eventAggregator.publish).not.toHaveBeenCalledWith($entityManagementEvents.onEntityRemoved, entityModel);
                });
            });

            describe(' - given the entity is the active entity', function() {
                beforeEach(function() {
                    $entityManager.addEntity(entityModel);
                    $entityManager.makeEntityActive(entityModel);
                });

                describe(' - and it is the only entity in the list', function() {
                    it('it should set the activeentity to null after removal', function() {
                        $entityManager.removeEntity(entityModel);
                        expect($rootScope.entitiesModel.activeEntity).toBeNull();
                    });
                });

                describe(' - and it is the first entity in a list of many', function () {
                    beforeEach(function() {
                        entityModel1 = $entityManager.createEntity('Mr Jones', 5, 879321, '');
                        $entityManager.addEntity(entityModel1);
                    });

                    it('it should set the activeentity to next ordered entity after removal', function() {
                        $entityManager.removeEntity(entityModel);
                        expect($rootScope.entitiesModel.activeEntity).toBe(entityModel1);
                    });

                    it('it should raise the onEntityActivated event for the new active entity', function() {
                        var spy = spyOn($eventAggregator, 'publish').and.callThrough();
                        $entityManager.removeEntity(entityModel);
                        expect($eventAggregator.publish).toHaveBeenCalledWith($entityManagementEvents.onEntityActivated, entityModel1);
                    });
                });

                describe(' - and it is the last entity in a list of many', function() {
                    it('it should set the activeentity to previous ordered entity after removal', function() {

                        entityModel1 = $entityManager.createEntity('Mr Jones', 5, 879321, '');
                        $entityManager.addEntity(entityModel1);
                        $entityManager.makeEntityActive(entityModel1);
                        $entityManager.removeEntity(entityModel1);
                        expect($rootScope.entitiesModel.activeEntity).toBe(entityModel);
                    });
                });

                describe(' - and it is neither the first nor last entity in a list of many', function() {
                    it('it should set the activeentity to previous ordered entity after removal', function() {

                        entityModel1 = $entityManager.createEntity('Mr Jones', 5, 879321, '');
                        entityModel2 = $entityManager.createEntity('Mr Maker', 5, 432541, '');
                        $entityManager.addEntity(entityModel1);
                        $entityManager.addEntity(entityModel2);
                        $entityManager.makeEntityActive(entityModel1);
                        $entityManager.removeEntity(entityModel1);
                        expect($rootScope.entitiesModel.activeEntity).toBe(entityModel2);
                    });
                });
            });
        });

        // Make entity active
        describe('when making a entity active', function() {
            var entityModel = null;
            beforeEach(function() {
                $entityManager.start();
                entityModel = $entityManager.createEntity('Mr Smith', 5, 54321, '');
            });

            it('it should set the entity as the active entity from the list of entities being managed', function() {
                $entityManager.addEntity(entityModel);
                $entityManager.makeEntityActive(entityModel);
                expect($rootScope.entitiesModel.activeEntity).toEqual(entityModel);
            });

            it('it should raise the onEntityActivated event', function() {
                spyOn($eventAggregator, 'publish').and.callThrough();
                $entityManager.addEntity(entityModel);
                $entityManager.makeEntityActive(entityModel);
                expect($eventAggregator.publish).toHaveBeenCalledWith($entityManagementEvents.onEntityActivated, entityModel);
            });

            describe(' - given the entity has not already been added', function() {
                it('it should not raise the onEntityActivated event', function() {
                    var spy = spyOn($eventAggregator, 'publish').and.callThrough();
                    $entityManager.makeEntityActive(entityModel);

                    expect($eventAggregator.publish).not.toHaveBeenCalledWith($entityManagementEvents.onEntityActivated, entityModel);
                });
            });
        });
    });
});
