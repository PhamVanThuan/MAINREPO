'use strict';
angular.module('sahl.tools.app.home.design.dockpanels.debug-variables', [
	'sahl.tools.app.services'
])
.controller('DebugVariablesPanelCtrl', ['$rootScope', '$scope', '$eventAggregatorService', '$eventDefinitions', 'filterFilter', '$enumerationDataManager', '$q', '$timeout', '$utils',
function DebugVariablesPanelController($rootScope, $scope, $eventAggregatorService, $eventDefinitions, filterFilter, enumerationDataManager, $q, $timeout, $utils) {

    $scope.variables = {
        view: [],
        inputs: {
            active: true
        },
        outputs: {
            active: true
        },
        toggleInput: function () {
            $scope.variables.inputs.active = !$scope.variables.inputs.active
        },
        toggleOutput: function () {
            $scope.variables.outputs.active = !$scope.variables.outputs.active
        },
        load: function () {
            if ($rootScope.document) {
                $scope.variables.view = $rootScope.document.data.tree.variables;
            }
        },
        clearOutputs: function () {     
            $eventAggregatorService.unsubscribe($eventDefinitions.onModelChanged, externalEvents.onModelChanged);
            for (var i = 0, c = $scope.variables.view.length; i < c; i++) {
                if ($scope.variables.view[i].usage === "output") {
                    $scope.variables.view[i].$debugValue = "";
                }
            }
        }
    };

    $scope.changeVariable = function () {
        $.debounce(500, false, $scope.variables.clearOutputs());
        $eventAggregatorService.publish($eventDefinitions.onDebugInputVariablesChanged, {})
    }

    var externalEvents = {
        onModelChanged: function (message) {
            if (message.propertyName !== "points") {
                $.debounce(500, false, $scope.variables.clearOutputs());
            }
        },
        onRequestSetDebugVariables: function (newDebugVariables) {
            $scope.variables.clearOutputs();
            $timeout(function () {
                for (var i = 0, c = $rootScope.document.data.tree.variables.length; i < c; i++) {
                    var debugVariable = $rootScope.document.data.tree.variables[i];
                    var newVariable = undefined;
                    var newVariables = newDebugVariables.filter(function (theNewVariable) {
                        return theNewVariable.id === debugVariable.id;
                    })
                    if (newVariables.length == 1) {
                        newVariable = newVariables[0];
                    }
                    if (newVariable !== undefined) {
                        debugVariable.$debugValue = newVariable.value;
                    }
                }
            })
            $eventAggregatorService.publish($eventDefinitions.onDebugInputVariablesChanged, {})
        }
    }

    $scope.debugging = {
        onExecutionCompleted: function (packet) {
            for (var i = 0, c = packet.output_variables.length; i < c; i++) {
                var outputVariable = packet.output_variables[i];
                for (var ii = 0, cc = $scope.variables.view.length; ii < cc; ii++) {
                    if ($utils.string.sanitise($scope.variables.view[ii].name) === outputVariable.name && $scope.variables.view[ii].usage === 'output') {
                        $scope.variables.view[ii].$debugValue = outputVariable.value;
                    }
                }
            }
            $eventAggregatorService.subscribe($eventDefinitions.onModelChanged, externalEvents.onModelChanged);
        }
    }

    $scope.setVisiblity = function (variable, expectedValue) {
        if (expectedValue.indexOf(variable.type) > -1) {
            return true;
        }
        return false;
    }

    $scope.variables.load();

    $eventAggregatorService.subscribe($eventDefinitions.onDocumentLoaded, $scope.variables.load);
    $eventAggregatorService.subscribe($eventDefinitions.onExecutionCompleted, $scope.debugging.onExecutionCompleted);
    $eventAggregatorService.subscribe($eventDefinitions.onDebugSessionStarted, $scope.variables.clearOutputs)
    $eventAggregatorService.subscribe($eventDefinitions.onRequestSetDebugVariables, externalEvents.onRequestSetDebugVariables);

    $scope.$on('$destroy', function () {
        $eventAggregatorService.unsubscribe($eventDefinitions.onDocumentLoaded, $scope.variables.load);
        $eventAggregatorService.unsubscribe($eventDefinitions.onExecutionCompleted, $scope.debugging.onExecutionCompleted);
        $eventAggregatorService.unsubscribe($eventDefinitions.onDebugSessionStarted, $scope.variables.clearOutputs)
        $eventAggregatorService.unsubscribe($eventDefinitions.onRequestSetDebugVariables, externalEvents.onRequestSetDebugVariables);
    })
}]);