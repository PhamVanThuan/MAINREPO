'use strict';
angular.module('sahl.tools.app.home.design.statuspanels.zoom-panel', [
	'sahl.tools.app.services'
])
.controller('ZoomPanelCtrl', ['$rootScope', '$scope', '$eventAggregatorService', '$eventDefinitions', '$designSurfaceManager',
	function ZoomPanelController($rootScope, $scope, $eventAggregatorService, $eventDefinitions, $designSurfaceManager) {

	    var zoomMin = 10;
	    var zoomMax = 510;
	    var zoomLimit = zoomMax - zoomMin;
	    var zoomRange = zoomMax - zoomMin;


	    var convertSliderValueToZoomPercent = function (sliderValue) {
	        var origValue = Math.round(sliderValue * zoomRange / 100.0);
	        var newZoom = zoomMin + 5.0 * Math.round((origValue) / 5.0);
	        return newZoom;
	    }
	    var convertZoomPercentToSliderValue = function (zoomPercent) {
	        var sv = Math.round(zoomPercent / zoomRange * 100.0);
            return sv;
	    }

	    var eventHandlers = {
	        onViewportBoundsChanged: function (e) {
	            $("#zoomSlider").slider("value", Math.round((e.diagram.scale * 20) - 2));
	            $scope.$apply();
	        }
	    }
	    $eventAggregatorService.subscribe($eventDefinitions.onViewportBoundsChanged, eventHandlers.onViewportBoundsChanged);

	    $scope.$on('destroy', function () {
	        $eventAggregatorService.unsubscribe($eventDefinitions.onViewportBoundsChanged, eventHandlers.onViewportBoundsChanged);
	    })

	    $rootScope.onSliderChanged = function (value, slider) {
	        $scope.zoomLevel = convertSliderValueToZoomPercent(value);
	        if (Math.abs($scope.zoomLevel - 100) < 5) {
	            $scope.zoomLevel = 100;
	        }
	        if ($scope.zoomLevel > zoomLimit) {
	            $scope.zoomLevel = zoomLimit;
	        }
	        $scope.$apply();

	        $designSurfaceManager.zoom($scope.zoomLevel);
	    }

	    // initial value of 100%
	    $scope.zoomLevel = 100;
	    $scope.initialZoom = convertZoomPercentToSliderValue($scope.zoomLevel);

	    $rootScope.$$postDigest(function () {
	        var zoomSlider = $("#zoomSlider");
	        zoomSlider.slider({ change: $scope.onSliderChanged });
	    });
	}]);