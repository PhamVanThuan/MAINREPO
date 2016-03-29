'use strict';
describe('[halo.start.portalpages.wipbar]',function() {

	var service, wipBarService, scope, entityManager, rootScope, logger, moduleDocument, timeout, eventAggregatorService, entityManagementEvents;
	var controller;
	beforeEach(module('sahl.js.core.eventAggregation'));	

	beforeEach(module('sahl.websites.halo.events'));
	beforeEach(module('sahl.websites.halo.services.entityManagement'));

	beforeEach(module('sahl.js.core.logging'));

	beforeEach(module('sahl.websites.halo.services.wipbar'));
	beforeEach(module('halo.start.portalpages.wipbar'));	

	beforeEach(inject(function(_$rootScope_, $eventAggregatorService, $entityManagerService, $logger, _$document_, _$timeout_, $entityManagementEvents, $wipBarService){
		rootScope = _$rootScope_;
		wipBarService = $wipBarService;
		entityManager = $entityManagerService;
		logger = $logger;
		moduleDocument = _$document_;
		timeout = _$timeout_;
		eventAggregatorService = $eventAggregatorService;
		entityManagementEvents = $entityManagementEvents;
	}));

	var liChildrenListElement = function(liSelector) {
		return [{
			clientWidth: 150,
			className: ''
		},
		{
			clientWidth: 200,
			className: 'active'
		}];
	};
	var iconSource = 'assets/img/wipbar_dropdown.png';
	var iconSourceActive = 'assets/img/wipbar_dropdown_active.png';

	var documentParams = {
		find: function(idSelector) {
			return {
				find: liChildrenListElement,
				innerWidth: function() {
					return 205;
				},
				children: liChildrenListElement
			}
		}
	};

	describe(' - controller - ',function() {
		var wipBarCtrl;
		var currentTabBarWidth, eventToStop;

		beforeEach(inject(function(_$rootScope_, _$controller_) {
			rootScope = _$rootScope_;
			rootScope.isEntityViewActive = jasmine.createSpy();
			rootScope.entitiesModel = {
				entities: [{
					id: 1
				},{
					id: 2
				}]
			};

			scope = rootScope.$new();
			controller = _$controller_;
		}));

		var createController = function() {
			return controller('wipBarCtrl', {
				$scope: scope,
				$rootScope: rootScope,
				$logger: logger,
				$entityManager: entityManager,
				$document: documentParams,
				$timeout: timeout,
				$eventAggregatorService: eventAggregatorService,
				$entityManagementEvents: entityManagementEvents,
				$wipBarService: wipBarService
			});
		};

		

		it('should have a wipBarService',function(){
			expect(wipBarService).not.toBeNull();
		});

		beforeEach(function() {
			spyOn(eventAggregatorService, 'subscribe').and.callThrough();
			currentTabBarWidth = documentParams.find('#tabs').innerWidth();			
			wipBarCtrl = createController();
			
			eventToStop = { 
				stopPropagation: jasmine.createSpy(),
				preventDefault: jasmine.createSpy()
			};
		});

		it('should autocollapse tabs when the sum of all tab widths exceeds current window width',function() {	
			spyOn(scope,'collapseMostRightInactiveTab').and.callThrough();
			expect(scope.entitiesModel).toBeDefined();
			expect(scope.entitiesModel.entities).toBeDefined();			
			expect(scope.getSumOfNavTabWidths()).toEqual(350);
			expect(scope.getSumOfNavTabWidths()).toBeGreaterThan(currentTabBarWidth);
			expect(scope.collapsedEntityCollection.entities.length).toEqual(0);

			scope.autoCollapseTabs();
			timeout.flush();

			expect(scope.collapseMostRightInactiveTab).toHaveBeenCalled();
			expect(scope.collapsedEntityCollection.entities.length).toEqual(1);
		});

		it('should get the sum of Navigation Bar Tab Widths', function() {
			expect(scope.getSumOfNavTabWidths()).toEqual(350);
		});

		it('should subscribe onNewEntityAdded to $entityManagementEvents.onEntityActivated event',function() {
			expect(eventAggregatorService.subscribe).toHaveBeenCalled();
			expect(eventAggregatorService.getNumberOfRegisteredSubscribersForEvent(entityManagementEvents.onEntityActivated)).toEqual(1);
		});

		it('should call onNewEntityAdded when $entityManagementEvents.onEntityActivated event is published',function() {
			spyOn(scope,'startup').and.callThrough();
			spyOn(scope,'onNewEntityAdded');
			scope.startup(scope);

			expect(scope.onNewEntityAdded).not.toHaveBeenCalled();
			eventAggregatorService.publish(entityManagementEvents.onEntityActivated, { id: 3});
			expect(scope.onNewEntityAdded).toHaveBeenCalled();
		});

		it('should be able to activate entity', function() {
			spyOn(scope,'loadMostRecentlyUsedQueue');
			spyOn(scope,'loadEntityIntoVisibleNavigationBarTabs');
			spyOn(entityManager,'makeEntityActive');

			var entityToActivate = {id : 3};
			
			scope.activateEntity(entityToActivate, eventToStop);

			expect(eventToStop.stopPropagation).toHaveBeenCalled();
			expect(scope.loadMostRecentlyUsedQueue).toHaveBeenCalled();
			expect(scope.loadEntityIntoVisibleNavigationBarTabs).toHaveBeenCalled();
			expect(scope.currentActiveEntity).toEqual(entityToActivate);
		});

		it('should not be able to load entity into Navigation Bar if it is already there', function() {
			expect(scope.entitiesModel.entities.length).toEqual(2);
			scope.loadEntityIntoVisibleNavigationBarTabs({ id: 2});
			expect(scope.entitiesModel.entities.length).toEqual(2);
		});

		it('should load entity into navigation bar if it doesnt exist there yet', function(){
			expect(scope.entitiesModel.entities.length).toEqual(2);
			scope.loadEntityIntoVisibleNavigationBarTabs({ id: 3});
			expect(scope.entitiesModel.entities.length).toEqual(3);
		});

		it('should load up mostRecentlyUsedQueue if it is empty', function() {
			expect(scope.mostRecentlyUsedQueue.entities.length).toEqual(0);
			scope.loadMostRecentlyUsedQueue();
			expect(scope.mostRecentlyUsedQueue.entities.length).toEqual(2);
		});

		it('should be able to enqueue an entity on the mostRecentlyUsedQueue',function() {
			scope.loadMostRecentlyUsedQueue();
			scope.enqueueEntity({ id: 3});
			expect(scope.mostRecentlyUsedQueue.entities[0].id).toEqual(3);
		});

		it('should be able to remove entity from navigation bar', function() {
			spyOn(entityManager, 'removeEntity');
			var entityToRemove = {id: 2};
			scope.removeEntity(entityToRemove, eventToStop);
			expect(entityManager.removeEntity).toHaveBeenCalledWith(entityToRemove);
		});

		it('should refresh UI after removing an entity', function() {
			spyOn(entityManager, 'removeEntity');
			spyOn(scope,'autoCollapseTabs');
			var entityToRemove = {id: 2};
			scope.removeEntity(entityToRemove, eventToStop);
			expect(entityManager.removeEntity).toHaveBeenCalledWith(entityToRemove);
			expect(eventToStop.stopPropagation).toHaveBeenCalled();
			expect(eventToStop.preventDefault).toHaveBeenCalled();
			expect(scope.autoCollapseTabs).toHaveBeenCalled();
		});

		it('should be able to return entity to Navigation Bar from Collapsed entites list', function() {
			scope.collapsedEntityCollection.entities.push({id: 3});
			scope.returnEntityToWipBar();
			expect(scope.entitiesModel.entities[2].id).toEqual(3);
		});

		it('should have an inactive dropdown icon when there are no collapsed entities in the navigation bar', function() {
			expect(scope.getCurrentNavbarDropdownIcon()).toEqual(iconSource);
		});

		it('should have an active dropdown icon when an entity has been collapsed', function() {
			expect(scope.getCurrentNavbarDropdownIcon()).toEqual(iconSource);			
			scope.autoCollapseTabs();
			timeout.flush();
			expect(scope.getCurrentNavbarDropdownIcon()).toEqual(iconSourceActive);
		});

		it('should be able to retrieve all entities that are included in the navigation bar and collapsed list', function() {
			expect(scope.entitiesModel.entities.length).toEqual(2);
			expect(scope.collapsedEntityCollection.entities.length).toEqual(0);
			scope.autoCollapseTabs();
			timeout.flush();
			expect(scope.entitiesModel.entities.length).toEqual(1);
			expect(scope.collapsedEntityCollection.entities.length).toEqual(1);
			expect(scope.getAllWipEntities().length).toEqual(2);			
		});
	});	
});