'use strict';
angular.module('halo.start.portalpages.wipbar', [
	'sahl.js.core.eventAggregation',
	'sahl.websites.halo.events',
	'sahl.websites.halo.services.entityManagement',
	'sahl.websites.halo.services.wipbar',
	'sahl.js.core.logging'
])
.controller('wipBarCtrl', ['$scope', '$rootScope', '$logger', '$entityManagerService', '$document', '$timeout', '$eventAggregatorService', '$entityManagementEvents','$wipBarService',
      function ($scope, $rootScope, $logger, $entityManager, $document, $timeout, $eventAggregatorService, $entityManagementEvents, $wipBarService) {

        $scope.startup = function(scope) {
          scope._tabHeight = 0;
          scope._tabWidth = 0;
          scope.currentActiveEntity = {};        
            
          
          scope.collapsedEntityCollection = {
            entities: []
          };

          $eventAggregatorService.subscribe($entityManagementEvents.onEntityActivated, scope.onNewEntityAdded);
          $(window).on('resize', scope.autoCollapseTabs);
        };       


        $scope.autoCollapseTabs = function() {
          var currentTabWidth = $document.find('#tabs').innerWidth();
          $scope._tabWidth = currentTabWidth - 40;
          
          var entityFromCollapsed = $scope.returnEntityToWipBar();
          $timeout(function() {
            $scope.$apply();

            var sumOfTabItemWidths = $scope.getSumOfNavTabWidths();

            if (sumOfTabItemWidths >= $scope._tabWidth) {
              $scope.collapseMostRightInactiveTab();
            } 
            else {
              if ($scope.collapsedEntityCollection.entities.length > 0) {
                $scope.autoCollapseTabs();  
              };                
            };              
          }); 
        };
        

        $scope.getSumOfNavTabWidths = function() {
			return $wipBarService.getSumOfNavTabWidths($document);          
        };

        $scope.getOverflowingTabIndexes = function(maxWidth) {
        	return $wipBarService.getOverflowingTabIndexes($document, maxWidth);      	
        };

	  	  $scope.onNewEntityAdded = function(entityToActivate) {
	        $scope.enqueueEntity(entityToActivate); 
	        $timeout(function() {
	            $scope.$apply();
	            $scope.autoCollapseTabs();
	        });            
      	};
        
        $scope.startup($scope);
      
      	$scope.activateEntity = function (entityToActivate, $event) {
    			$event.stopPropagation(); 
    			$scope.loadMostRecentlyUsedQueue();
    			$scope.loadEntityIntoVisibleNavigationBarTabs(entityToActivate);      
    			$entityManager.makeEntityActive(entityToActivate);                                            
    			$scope.currentActiveEntity = entityToActivate;
      	};

  		$scope.loadEntityIntoVisibleNavigationBarTabs = function(entity) {
            var existing = _.findWhere($scope.entitiesModel.entities, {
                                id: entity.id
                            });
            if (_.isUndefined(existing)) {
              $scope.unshiftEntityToEntitiesModel(entity);

              var indexOfExistingEntity = $scope.collapsedEntityCollection.entities.indexOf(entity);            
              if(indexOfExistingEntity >= 0) {
                $scope.collapsedEntityCollection.entities.splice(indexOfExistingEntity,1);
              }; 
            };
      	};

  		$scope.loadMostRecentlyUsedQueue = function() {
            var priorityEntity = null;
            if($scope.mostRecentlyUsedQueue.entities.length < 1) {
              for (var i = 0; i < $scope.entitiesModel.entities.length; i++) {
                var entityFromModel = $scope.entitiesModel.entities[i];
                if ($scope.isEntityViewActive(entityFromModel)) {
                  priorityEntity = entityFromModel;
                };
                $scope.mostRecentlyUsedQueue.enqueue(entityFromModel);
              };

              if (priorityEntity) {
                $scope.mostRecentlyUsedQueue.enqueue(priorityEntity);
              };
            };
      	};

      	$scope.enqueueEntity = function(entity) {    
            $scope.mostRecentlyUsedQueue.enqueue(entity);
      	};

      	$scope.removeEntity = function (entityToRemove, $event) {
			$event.stopPropagation();
			$event.preventDefault();
			$entityManager.removeEntity(entityToRemove);

			$scope.autoCollapseTabs();
      	};

      	$scope.getTabHeight = function(tabs) {
	        return tabs.innerHeight();
      	};

      	$scope.collapseMostRightInactiveTab = function() {
            var tabs = $document.find('#tabs');
            var children = tabs.children('li');          
            var activeIndex = $scope.getActiveTabIndex(children);
            
              var lastInactiveTabIndexes = $scope.getOverflowingTabIndexes($scope._tabWidth).reverse();

              for (var i = 0; i < lastInactiveTabIndexes.length; i++) {
                var lastInactiveTabIndex = lastInactiveTabIndexes[i];

              	var lastActiveEntity = $scope.entitiesModel.entities[lastInactiveTabIndex];              

              	if (lastInactiveTabIndex < 0 || !lastActiveEntity) { 
              		continue;
              	};

                if(lastActiveEntity.id === $scope.currentActiveEntity.id) {
                  return;
                };

                $scope.collapsedEntityCollection.entities.push($scope.entitiesModel.entities[lastInactiveTabIndex]);            
                $scope.entitiesModel.entities.splice(lastInactiveTabIndex,1);
          	};
      	};

      	$scope.returnEntityToWipBar = function () {
            var entityFromCollapsed = $scope.collapsedEntityCollection.entities.pop();
            if (entityFromCollapsed) {
              $scope.pushEntityToEntitiesModel(entityFromCollapsed);                    
            };  
            return entityFromCollapsed;
      	};

      	$scope.mostRecentlyUsedQueue = {
            entities: []
      	};

      	$scope.mostRecentlyUsedQueue.enqueue = function(entity) {
            var index = $scope.mostRecentlyUsedQueue.entities.indexOf(entity);
            if (index > -1) {
              $scope.mostRecentlyUsedQueue.entities.splice(index, 1);
            };
            $scope.mostRecentlyUsedQueue.entities.unshift(entity);
      	};

      	$scope.mostRecentlyUsedQueue.dequeue = function() {
            return $scope.mostRecentlyUsedQueue.entities.pop();
      	};

   		$scope.getActiveTabIndex = function (children) {
	        for (var i = 0; i < children.length; i++) {
	          var child = children[i];
	          var containsActive = child.className.indexOf('active');
	          if (containsActive) {
	            return i;
	          };
	        };

	        return -1;
      	};

      	$scope.getCurrentNavbarDropdownIcon = function() {
          var iconSource = 'assets/img/wipbar_dropdown.png';
	        if ($scope.collapsedEntityCollection.entities.length > 0) {
	          iconSource = 'assets/img/wipbar_dropdown_active.png';
	        };
	        return iconSource;
      	};

      	$scope.unshiftEntityToEntitiesModel = function(entity) {     

            $scope.removeDuplicateEntitiesFromEntitiesModel(entity);

            $scope.entitiesModel.entities.unshift(entity);

            $timeout(function() {
                $scope.$apply();
            });
      	};

      	$scope.pushEntityToEntitiesModel = function(entity) {
            $scope.removeDuplicateEntitiesFromEntitiesModel(entity);
            $scope.entitiesModel.entities.push(entity);

            $timeout(function() {
                $scope.$apply();
            });
      	};

      	$scope.removeDuplicateEntitiesFromEntitiesModel = function(entity) {
            var indexesToRemove = [];
            _.find($scope.entitiesModel.entities, function(entityFromModel, entityIndex) {
              if (entityFromModel.id === entity.id) {
                indexesToRemove.push(entityIndex);
              };
            });

            _.each(indexesToRemove, function(indexToRemove) {
              $scope.entitiesModel.entities.splice(indexToRemove,1);
            });
      	};

      	$scope.getAllWipEntities = function() {
            var list = [];
            for (var i = 0; i < $scope.entitiesModel.entities.length; i++) {
              var originalEntity = $scope.entitiesModel.entities[i];
              list.push(originalEntity);
            };

            for (var j = 0; j < $scope.collapsedEntityCollection.entities.length; j++) {
              var collapsedEntity = $scope.collapsedEntityCollection.entities[j];
              if (_.where(list, {id: collapsedEntity.id }).length > 0) { 
                
                var testId = collapsedEntity.id;
                continue;	
              };

              list.push(collapsedEntity);
            };
            return list;
      	};

      	$scope.hasCollapsedTabs = function() {
            return $scope.getAllWipEntities().length > 0;
      	};     


      }
])
.directive('wipBarTabs',function() {
	return {
		restrict: 'E',
		scope: false,
		controller: '@',
		name: 'controllerName',
		templateUrl: 'app/start/portalPages/wipbar/wipbar.tpl.html'
	}
});