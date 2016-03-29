'use strict';

/* Services */

angular.module('sahl.tools.app.services.undoManager', [
    'sahl.tools.app.serviceConfig',
    'SAHL.Services.Interfaces.DecisionTreeDesign.queries',
    'SAHL.Services.Interfaces.DecisionTreeDesign.commands'
])
.factory('$undoManager', ['$rootScope', '$eventAggregatorService', '$eventDefinitions', '$keyboardManager', function ($rootScope, $eventAggregatorService, $eventDefinitions, $keyboardManager) {

    var modelUndoManager = null;
    var internalEventHandlers = {
        afterModelChanged: function (change) {
            internalEventHandlers.refreshUndoManager();
        },
        refreshUndoManager: function () {
            $rootScope.document.undoManager.canRedo = modelUndoManager.canRedo();
            $rootScope.document.undoManager.canUndo = modelUndoManager.canUndo();
            $rootScope.$apply();
        }
    }

    var debouncedEventHandlers = {
        debouncedAfterModelChanged: $.debounce(500, false, internalEventHandlers.afterModelChanged),
    }

    var eventHandlers = {

        onModelCreated: function (model) {
            modelUndoManager = model.undoManager;
            $rootScope.document.undoManager = undoManager;

            $keyboardManager.bind('CTRL+Z', internalEventHandlers.refreshUndoManager);
            $keyboardManager.bind('CTRL+Y', internalEventHandlers.refreshUndoManager);
        },
        onModelDestroyed: function () {
            $rootScope.document.undoManager = null;
            $keyboardManager.unbind('CTRL+Z', internalEventHandlers.refreshUndoManager);
            $keyboardManager.unbind('CTRL+Y', internalEventHandlers.refreshUndoManager);
        },
        onModelChanged: function (change) {
            debouncedEventHandlers.debouncedAfterModelChanged(change);
        }
    }

    //subscribe to node selection event
    $eventAggregatorService.subscribe($eventDefinitions.onModelChanged, eventHandlers.onModelChanged);
    $eventAggregatorService.subscribe($eventDefinitions.onModelCreated, eventHandlers.onModelCreated);
    $eventAggregatorService.subscribe($eventDefinitions.onModelDestroyed, eventHandlers.onModelDestroyed);

    document.body.onmouseup = function () {
        //internalEventHandlers.refreshUndoManager();
    }

    var undoManager = {
        canUndo: false,
        canRedo: false,
        undo: function () {
            modelUndoManager.undo();
        },
        redo: function () {
            modelUndoManager.redo();
        }
    }

    return {
        start: function () {

        }
    }
}])