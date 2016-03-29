'use strict';

/* Services */

angular.module('sahl.tools.app.services.selectionManager', [
    'sahl.tools.app.serviceConfig',
    'SAHL.Services.Interfaces.DecisionTreeDesign.queries',
    'SAHL.Services.Interfaces.DecisionTreeDesign.commands'
])
.service('$selectionManager', ['$rootScope', '$eventAggregatorService', '$eventDefinitions', '$debugService', function ($rootScope, $eventAggregatorService, $eventDefinitions, $debugService) {
    var designSurface = null;

    var internalOperations = {
        deleteSelection: function () {
            designSurface.commandHandler.deleteSelection();
        }
    }
    var eventHandlers = {
        onNodeSelectionChanged: function (selectedNode) {
            if (selectedNode != null && selectedNode.data != null) {
                var key = selectedNode.data.key;
                var thisNode = $rootScope.document.dataModel.nodeDataArray.filter(function (node) {
                    return node.key == key;
                });
                var selectedTreeNode = null;
                if (thisNode.length == 1) {
                    selectedTreeNode = thisNode[0];
                }
                if (selectedNode.category === 'Decision' || selectedNode.category === 'Process' || selectedNode.category === 'SubTree' || selectedNode.category === 'ClearMessages' || selectedNode.category === 'Start' || selectedNode.category === 'End') {
                    $rootScope.document.selectionManager.current = { type: "node", data: selectedTreeNode, debugInputData: $debugService.getDebugSessionSubtreeInputs(key), debugOutputData: $debugService.getDebugSessionSubtreeOutputs(key) };
                    if (selectedTreeNode && (selectedTreeNode.text == "Start")) {
                        $rootScope.document.selectionManager.canDelete = false;
                    }
                    else {
                        $rootScope.document.selectionManager.canDelete = true;
                    }
                }
                else
                    if(selectedNode.category === '' && selectedNode.isTreeLink == true) {
                        $rootScope.document.selectionManager.current = { type: "link", data: selectedTreeNode };
                    }
            }
            else {
                $rootScope.document.selectionManager.current = { type: "tree", data: $rootScope.document.dataModel };
                $rootScope.document.selectionManager.canDelete = false;
            }
            setTimeout(function () { $rootScope.$apply(); }, 1);
        },
        onDocumentLoaded: function (documentLoaded) {
            documentLoaded.selectionManager = {
                current: { type: "tree", data: $rootScope.document.dataModel },
                canDelete: false,
                deleteSelection: internalOperations.deleteSelection
            };
        },
        onDocumentUnloaded: function (unloadedDocument) {
            unloadedDocument.selectionManager = undefined;
        },
        onSurfaceAttached: function (attachedDesignSurface) {
            designSurface = attachedDesignSurface;
        },
        onSurfaceDetached: function () {
            designSurface = undefined;
        },
    }

    //subscribe to node selection event
    $eventAggregatorService.subscribe($eventDefinitions.onNodeSelectionChanged, eventHandlers.onNodeSelectionChanged);
    // subscribe to the document loaded event
    $eventAggregatorService.subscribe($eventDefinitions.onDocumentLoaded, eventHandlers.onDocumentLoaded);

    $eventAggregatorService.subscribe($eventDefinitions.onDocumentUnloaded, eventHandlers.onDocumentUnloaded);

    $eventAggregatorService.subscribe($eventDefinitions.onSurfaceAttached, eventHandlers.onSurfaceAttached);

    $eventAggregatorService.subscribe($eventDefinitions.onSurfaceDetached, eventHandlers.onSurfaceDetached);

    return {
        start: function () { }
    }
}])