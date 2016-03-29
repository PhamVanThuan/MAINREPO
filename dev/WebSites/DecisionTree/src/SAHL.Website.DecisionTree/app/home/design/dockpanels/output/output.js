'use strict';
angular.module('sahl.tools.app.home.design.dockpanels.output', [
	'sahl.tools.app.services'
])
.controller('OutputPanelCtrl', ['$rootScope', '$scope', '$eventAggregatorService', '$eventDefinitions','filterFilter','$codeMirrorService',
	function OutputPanelController($rootScope, $scope, $eventAggregatorService, $eventDefinitions, filterFilter, $codeMirrorService) {
	    $scope.boolSelect = [true, false];


	    $scope.msgSelected = function (key, line, source) {
	        $eventAggregatorService.publish($eventDefinitions.onChangeSelectedNode, key);
	        $eventAggregatorService.publish($eventDefinitions.onSearchItemChanged, key);
	        var chr = source.length;
	        if (!chr) {
	            chr = 1;
	        }
	        $codeMirrorService.setSelection(line, 0, 1);
	        $codeMirrorService.scrollIntoView(line, 0, 1);
	    };


	    var internalEventHandlers = {
	        onOutputMessagesChanged: function () {
	            $scope.outputMessages.load($rootScope.document.dataModel.outputMessagesArray);
	        }
	    }

	    var debouncedEventHandlers = {
	        debouncedOnOutputMessagesChanged: $.debounce(800, false, internalEventHandlers.onOutputMessagesChanged),
	    }

	    $scope.outputMessages = {
	        view: [],
	        errors: {
	            active: true,
	        },
	        warnings: {
	            active: true,
	        },
	        info: {
	            active: true,
	        },
	        toggleErrors: function () {
	            $scope.outputMessages.errors.active = !$scope.outputMessages.errors.active
	        },
	        toggleWarnings: function () {
	            $scope.outputMessages.warnings.active = !$scope.outputMessages.warnings.active
	        },
	        toggleInfo: function () {
	            $scope.outputMessages.info.active = !$scope.outputMessages.info.active
	        },
	        load: function (outputMessages) {
	            $scope.outputMessages.view = outputMessages;
	        }
	    };

	    if ($rootScope.document) {
            $scope.outputMessages.load($rootScope.document.dataModel.outputMessagesArray);
	    }
	    $eventAggregatorService.subscribe($eventDefinitions.onOutputMessagesChanged, debouncedEventHandlers.debouncedOnOutputMessagesChanged);

	    $scope.$on('$destroy', function () {
	        $eventAggregatorService.unsubscribe($eventDefinitions.onOutputMessagesChanged, debouncedEventHandlers.debouncedOnOutputMessagesChanged);
	    })
	}]);