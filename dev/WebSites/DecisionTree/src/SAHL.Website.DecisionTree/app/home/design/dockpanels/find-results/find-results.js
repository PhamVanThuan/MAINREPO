'use strict';
angular.module('sahl.tools.app.home.design.dockpanels.find-results', [
	'sahl.tools.app.services'
])
.controller('FindResultsPanelCtrl', ['$rootScope', '$scope', '$eventAggregatorService', '$eventDefinitions','$searchManager',
	function FindResultsPanelController($rootScope, $scope, $eventAggregatorService, $eventDefinitions, $searchManager) {
	    $scope.searchResultSelected = function (key, searchIndex) {
	        $eventAggregatorService.publish($eventDefinitions.onChangeSelectedNode, key);
	        $eventAggregatorService.publish($eventDefinitions.onSearchItemChanged, key);
	        $eventAggregatorService.publish($eventDefinitions.onSearchResultItemChanged, searchIndex);
	        $searchManager.highlightCode(searchIndex);
	    };
	    
	    $scope.searchFilterClicked = function (filter){
	        $eventAggregatorService.publish($eventDefinitions.onSearchFilterChanged, filter);
	    }

	}]);