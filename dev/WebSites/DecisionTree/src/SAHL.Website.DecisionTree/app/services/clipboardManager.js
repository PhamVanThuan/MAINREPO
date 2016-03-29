'use strict';

/* Services */

angular.module('sahl.tools.app.services.clipboardManager', [
    'sahl.tools.app.serviceConfig',
    'SAHL.Services.Interfaces.DecisionTreeDesign.queries',
    'SAHL.Services.Interfaces.DecisionTreeDesign.commands'
])
.factory('$clipboardManager', ['$rootScope', '$eventAggregatorService', '$eventDefinitions', function ($rootScope, $eventAggregatorService, $eventDefinitions) {
    var p = new go.Point(300, 20);

    var designSurface = null;

    var internalOperations = {
        cut: function () {
            designSurface.commandHandler.cutSelection();
        },
        copy: function () {
            designSurface.commandHandler.copySelection();
        },
        paste: function () {
            designSurface.commandHandler.pasteSelection(p);
        }
    }

    var eventHandlers = {
        onNodeSelectionChanged: function (selectedNode) {
            if (selectedNode != null && selectedNode.data != null) {
                $rootScope.document.clipboardManager.canCopy = true;
                $rootScope.document.clipboardManager.canCut = true;
            }
            else {
                $rootScope.document.clipboardManager.canCopy = false;
                $rootScope.document.clipboardManager.canCut = false;
            }
            setTimeout(function () { $rootScope.$apply(); }, 1);
        },
        onDocumentLoaded: function (documentLoaded) {
            documentLoaded.clipboardManager = {
                canCopy: false,
                canCut: false,
                canPaste: false,
                cut: function () {
                    internalOperations.cut();
                },
                copy: function () {
                    internalOperations.copy();
                },
                paste: function () {
                    internalOperations.paste();
                }
            };
        },
        onDocumentUnloaded: function (unloadedDocument) {
            unloadedDocument.clipboardManager = undefined;
        },
        onSurfaceAttached: function (attachedDesignSurface) {
            designSurface = attachedDesignSurface;
        },
        onSurfaceDetached: function () {
            designSurface = undefined;
        },
        onClipboardChanged: function () {
            $rootScope.document.clipboardManager.canPaste = true;
            $rootScope.$apply();
        }
    }
    $eventAggregatorService.subscribe($eventDefinitions.onNodeSelectionChanged, eventHandlers.onNodeSelectionChanged);

    $eventAggregatorService.subscribe($eventDefinitions.onDocumentLoaded, eventHandlers.onDocumentLoaded);

    $eventAggregatorService.subscribe($eventDefinitions.onDocumentUnloaded, eventHandlers.onDocumentUnloaded);

    $eventAggregatorService.subscribe($eventDefinitions.onSurfaceAttached, eventHandlers.onSurfaceAttached);

    $eventAggregatorService.subscribe($eventDefinitions.onSurfaceDetached, eventHandlers.onSurfaceDetached);

    $eventAggregatorService.subscribe($eventDefinitions.onClipboardChanged, eventHandlers.onClipboardChanged);

    return {
        start: function(){}
    }
}])