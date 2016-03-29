'use strict';

angular.module('sahl.tools.app.services.highlightingService', [
    'sahl.tools.app.services.eventAggregatorService',
    'sahl.tools.app.services.eventDefinitions',
    'sahl.tools.app.services.designSurfaceManager'
])
.factory('$highlightingService', ['$rootScope', '$eventDefinitions', '$eventAggregatorService', 'filterFilter', '$designSurfaceManager', function ($rootScope, $eventDefinitions, $eventAggregatorService, filterFilter, $designSurfaceManager) {
    var designLinks = [];
    var linksLength = -1;
    var debuggerModel = null;

    var linkTypes = { true: "decision_yes", false: "decision_no" };

    var colours = {
        green: "#7FE01F",
        red: "#FF2E00",
        grey: "grey"
    }

    var internal = {
        activePath: function (from, to, nodeResult) {
            for (var i = 0; i < linksLength; i++) {
                if (designLinks[i].from == from && designLinks[i].to == to) {
                    if (internal.checkForYesNoPath(designLinks[i], nodeResult)){
                        designLinks[i].$status = colours.green;
                    } else {
                        designLinks[i].$status = colours.red;
                    }
                }
            }
        },
        inactivePath: function (from, to) {
            for (var i = 0; i < linksLength; i++) {
                if ((designLinks[i].from == from && designLinks[i].to != to)) {
                    internal.traverseNodes(designLinks[i], to);
                }
            }
        },
        traverseNodes: function (link, parentId) {
            if (link.$status && (link.$status === colours.green || link.$status === colours.red || link.from == parentId)) {
                return;
            }
            link.$status = colours.red;
            for (var i = 0; i < linksLength; i++) {
                if (designLinks[i].from == link.to && !internal.hasParentOf(designLinks[i].from, designLinks[i].from, parentId)) {
                    internal.traverseNodes(designLinks[i], parentId);
                }
            }
        },
        hasParentOf: function (id,currentNodeId,parentId) {
            var results = false;
            for (var i = 0; i < linksLength; i++) {
                if (designLinks[i].to == id) {
                    if (designLinks[i].from == parentId) {
                        return true;
                    } else if (currentNodeId == designLinks[i].from) {
                        return false
                    } else {
                        results = results || internal.hasParentOf(designLinks[i].from, currentNodeId, parentId);
                    }
                }
            }
            return results;
        },
        checkForYesNoPath: function (designLink,nodeResult) {
            if(nodeResult !== undefined){
                if (designLink.linkType !== undefined && designLink.linkType.indexOf("decision") == 0) {
                    if (designLink.linkType.indexOf(linkTypes[nodeResult]) == 0){
                        return true;
                    } else {
                        return false;
                    }
                }
            }
            return true;
        },
        updateCanvas: function () {
            $designSurfaceManager.updateBindings();
            $designSurfaceManager.requestUpdate();
        },
        reset: function () {
            angular.forEach(designLinks, function (link) {
                link.$status = colours.grey;
            })
        }
    };
    
    var highlightingService = {
        initialize: function (message) {
            designLinks = message.dataModel.linkDataArray;
        },
        destroy: function () {
            designLinks = [];
            linksLength = null;
            debuggerModel = null;
        },
        initDebug: function () {
            if(!debuggerModel){
                debuggerModel = $rootScope.document.debugger;
            }
            highlightingService.resetLinks();
            linksLength = designLinks.length;
        },
        destroyDebug : function(){
            linksLength = -1;
            $eventAggregatorService.subscribe($eventDefinitions.onModelChanged, eventHandlers.onModelChanged);
            $eventAggregatorService.subscribe($eventDefinitions.onDebugInputVariablesChanged, eventHandlers.onModelChanged);
        },
        updateLinks: function (message) {
            internal.activePath(message.previousNodeId, message.newNodeId, message.previousNodeResult);
            //LeighB: removing this cause it is. broken internal.inactivePath(message.previousNodeId, message.newNodeId);
            internal.updateCanvas();
        },
        resetLinks: function () {
            $eventAggregatorService.unsubscribe($eventDefinitions.onModelChanged, eventHandlers.onModelChanged);
            $eventAggregatorService.unsubscribe($eventDefinitions.onDebugInputVariablesChanged, eventHandlers.onModelChanged);
            internal.reset();
            internal.updateCanvas();
        }
    };

    var eventHandlers = {
        onDocumentLoaded: function (message) {
            highlightingService.initialize(message)
        },
        onDocumentUnloaded: function (message) {
            highlightingService.destroy();
        },
        onDebugSessionStarted : function(message){
            highlightingService.initDebug();
        },
        onDebugSessionEnded: function (message) {
            highlightingService.destroyDebug();
        },
        onDebugLocationChanged: function (message) {
            highlightingService.updateLinks(message);
        },
        onModelChanged: function (message) {
            if (message.propertyName !== "points") {
                $.debounce(500, false, highlightingService.resetLinks());
            }
        }
    };

    $eventAggregatorService.subscribe($eventDefinitions.onDocumentLoaded, eventHandlers.onDocumentLoaded);
    $eventAggregatorService.subscribe($eventDefinitions.onDocumentUnloaded, eventHandlers.onDocumentUnloaded);
    $eventAggregatorService.subscribe($eventDefinitions.onDebugSessionStarted, eventHandlers.onDebugSessionStarted);
    $eventAggregatorService.subscribe($eventDefinitions.onDebugSessionEnded, eventHandlers.onDebugSessionEnded);
    $eventAggregatorService.subscribe($eventDefinitions.onDebugLocationChanged, eventHandlers.onDebugLocationChanged);
    return {
        start: function () { }
    };
}]);