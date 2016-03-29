'use strict';

angular.module('sahl.tools.app.services.breakpointService', [
    'sahl.tools.app.services.eventAggregatorService',
    'sahl.tools.app.services.eventDefinitions'
])
.factory('$breakpointService', ['$rootScope', '$eventDefinitions', '$eventAggregatorService', 'filterFilter', function ($rootScope, $eventDefinitions, $eventAggregatorService, filterFilter) {
    var objectModels = {
        breakpointModel : function(packet) {
            return { 'enabled': true, 'nodeId': packet.id, 'name': packet.text };
        },
        debugBreakpointModel: function(breakPoint) {
            return { "nodeId":breakPoint.nodeId };
        },
        breakpointStatusModel: function () {
            return { 'exists' : false,'enabled': false };
        },
        queryGlobalsVersionModel: function () {
            return {
                "enumerationsVersions": enumerationsVersions,
                "messagesVersions": messagesVersions,
                "variablesVersions": variablesVersions
            }
        }
    }

    var breakPoints = undefined;


    var breakpointService = {
        initialize: function(){
            breakPoints = new Array();

        },
        destroy: function(){
            breakPoints = undefined;
        },
        addBreakPoint: function (packet) {
            var bp = objectModels.breakpointModel(packet);
            breakPoints.push(bp);
            $eventAggregatorService.publish($eventDefinitions.onBreakpointsChanged, { "breakpoints": breakPoints });
            $eventAggregatorService.publish($eventDefinitions.onBreakpointStatusChanged, { "breakpoint": bp });
        },
        removeBreakPoint: function (packet) {
            var results = filterFilter(breakPoints, function (breakpoint) {
                return breakpoint.nodeId === packet.id && breakpoint.name === packet.text;
            });
            if (results.length > 0) {
                var indexOfPacket = breakPoints.indexOf(results[0]);
                breakPoints.splice(indexOfPacket, 1);
                $eventAggregatorService.publish($eventDefinitions.onBreakpointsChanged, { "breakpoints": breakPoints });
                $eventAggregatorService.publish($eventDefinitions.onBreakpointStatusChanged, {"breakpoint": results[0]} );
                
            }
        },
        enableBreakPoint: function (packet) {
            var results = filterFilter(breakPoints, function (breakpoint) {
                return breakpoint.nodeId === packet.id && breakpoint.name === packet.text;
            });
            if (results.length > 0) {
                var index = breakPoints.indexOf(results[0]);
                breakPoints[index].enabled = true;
                $eventAggregatorService.publish($eventDefinitions.onBreakpointsChanged, { "breakpoints": breakPoints });
                $eventAggregatorService.publish($eventDefinitions.onBreakpointStatusChanged, { "breakpoint": results[0] });
            }
        },
        disableBreakPoint: function (packet) {
            var results = filterFilter(breakPoints, function (breakpoint) {
                return breakpoint.nodeId === packet.id && breakpoint.name === packet.text;
            });
            if (results.length > 0) {
                var index = breakPoints.indexOf(results[0]);
                breakPoints[index].enabled = false;
                $eventAggregatorService.publish($eventDefinitions.onBreakpointsChanged, { "breakpoints": breakPoints });
                $eventAggregatorService.publish($eventDefinitions.onBreakpointStatusChanged, { "breakpoint": results[0] });
            }
        },
        getBreakpointStatus: function (packet) {
            var model = objectModels.breakpointStatusModel()
            var results = filterFilter(breakPoints, function (breakpoint) {
                return breakpoint.nodeId === packet.id && breakpoint.name === packet.text;
            });
            if (results.length > 0) {
                model.exists = true;
                model.enabled = results[0].enabled;
            }
            packet.validate(model);
        },
        getActiveBreakPoints: function() {
            var retVal = new Array();
            for (var index in breakPoints) {
                var bp = breakPoints[index];
                if (bp.enabled) {
                    retVal.push(objectModels.debugBreakpointModel(bp));
                }
            }
            return retVal;
        },
        getAvailableGlobalsVersions: function () {
            
        }
    }

    var eventHandlers = {
        onDocumentLoaded: function (message) {
            breakpointService.initialize();
        },
        onDocumentUnloaded: function (message) {
            breakpointService.destroy();
        },
        onBreakpointAdd: function (message) {
            breakpointService.addBreakPoint(message);
        },
        onBreakpointRemove: function (message) {
            breakpointService.removeBreakPoint(message);
        },
        onBreakpointEnable: function (message) {
            breakpointService.enableBreakPoint(message);
        },
        onBreakpointDisable: function (message) {
            breakpointService.disableBreakPoint(message);
        },
        onBreakpointStatus: function (message) {
            breakpointService.getBreakpointStatus(message);
        }
    }

    $eventAggregatorService.subscribe($eventDefinitions.onDocumentLoaded, eventHandlers.onDocumentLoaded);
    $eventAggregatorService.subscribe($eventDefinitions.onDocumentUnloaded, eventHandlers.onDocumentUnloaded);
    $eventAggregatorService.subscribe($eventDefinitions.onBreakpointAdd, eventHandlers.onBreakpointAdd);
    $eventAggregatorService.subscribe($eventDefinitions.onBreakpointRemove, eventHandlers.onBreakpointRemove);
    $eventAggregatorService.subscribe($eventDefinitions.onBreakpointEnable, eventHandlers.onBreakpointEnable);
    $eventAggregatorService.subscribe($eventDefinitions.onBreakpointDisable, eventHandlers.onBreakpointDisable);
    $eventAggregatorService.subscribe($eventDefinitions.onBreakpointStatus, eventHandlers.onBreakpointStatus);

    return {
        getActiveBreakPoints : breakpointService.getActiveBreakPoints,
        start: function(){}
    };
}]);