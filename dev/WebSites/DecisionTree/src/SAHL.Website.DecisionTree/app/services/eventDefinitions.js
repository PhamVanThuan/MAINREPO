'use strict';

/* Services */

angular.module('sahl.tools.app.services.eventDefinitions', [
    'sahl.tools.app.serviceConfig',
    'SAHL.Services.Interfaces.DecisionTreeDesign.queries',
    'SAHL.Services.Interfaces.DecisionTreeDesign.commands'
])
.service('$eventDefinitions', ['$rootScope', function ($rootScope) {
    var messageHandlers = [];
    return {
        onApplicationLoaded:  "onApplicationLoaded",

        onBeforeNodeSelectionChanged: "onBeforeNodeSelectionChanged",
        onNodeSelectionChanged: "onNodeSelectionChanged",

        onDocumentLoaded: "onDocumentLoaded",
        onNewDocumentLoaded: "onNewDocumentLoaded",
        onDocumentUnloaded: "onDocumentUnloaded",
        onBeforeDocumentSaved: "onBeforeDocumentSaved",
        onDocumentSaved: "onDocumentSaved",
        onNewDocumentSaved: "onNewDocumentSaved",
        onDocumentedChanged: "onDocumentChanged",
        onDocumentVariableChanged: "onDocumentVariableChanged",

        onGlobalVariablesSaved: "onGlobalVariablesSaved",
        onGlobalEnumerationsSaved: "onGlobalEnumerationsSaved",
        onGlobalMessagesSaved: "onGlobalMessagesSaved",

        onModelCreated: "onModelCreated",
        onModelChanged: "onModelChanged",
        onModelDestroyed: "onModelDestroyed",
        onSurfaceAttached: "onSurfaceAttached",
        onSurfaceDetached: "onSurfaceDetached",
        onClipboardChanged: "onClipboardChanged",
        onUserAuthenticated: "onUserAuthenticated",
        onChangeSelectedNode: "onChangeSelectedNode",
        onLayoutInitialized: "onLayoutInitialized",
        onViewportBoundsChanged: "onViewportBoundsChanged",
        onKeyUpCodeEditor: "onKeyUpCodeEditor",

        onSearchItemChanged: "onSearchItemChanged",

        onOutputMessagesChanged: "onOutputMessagesChanged",

        onTestSuiteExecutionStarted: "onTestSuiteExecutionStarted",
        onTestSuiteStoryExecutionStarted: "onTestSuiteStoryExecutionStarted",
        onTestSuiteStoryScenarioExecutionStarted: "onTestSuiteStoryScenarioExecutionStarted",
        onTestSuiteExecutionLocationChanged: "onTestSuiteExecutionLocationChanged",
        onTestSuiteStoryScenarioExecutionError: "onTestSuiteStoryScenarioExecutionError",
        onTestSuiteStoryScenarioExecutionCompleted: "onTestSuiteStoryScenarioExecutionCompleted",
        onTestSuiteStoryExecutionCompleted: "onTestSuiteStoryExecutionCompleted",
        onTestSuiteExecutionCompleted: "onTestSuiteExecutionCompleted",

        onDebugSessionStarted: "onDebugSessionStarted",
        onDebugSessionEnded: "onDebugSessionEnded",
        onDebugError: "onDebugError",
        onDebugLocationChanged: "onDebugLocationChanged",
        onExecutionStoppedAtNode: "onExecutionStoppedAtNode",
        onExecutionCompleted: "onExecutionCompleted",
        onDebugInputVariablesChanged: "onDebugInputVariablesChanged",
        onRequestSetDebugVariables: "onRequestSetDebugVariables",
        onRequestDebugSession: "onRequestDebugSession",

        onBreakpointAdd: "onBreakpointAdd",
        onBreakpointRemove: "onBreakpointRemove",
        onBreakpointEnable: "onBreakpointEnable",
        onBreakpointDisable: "onBreakpointDisable",
        onBreakpointsChanged: "onBreakpointsChanged",
        onBreakpointStatus: "onBreakpointStatus",
        onBreakpointStatusChanged: "onBreakpointStatusChanged",

        onSearchResultItemChanged: "onSearchResultItemChanged",
        onSearchFilterChanged: "onSearchFilterChanged",

        onGlobalsLoaded: "onGlobalsLoaded",

        onRemovedLocks: "onRemovedLocks",
        onAddedLocks: "onAddedLocks",
        onPendingLockRelease: "onPendingLockRelease",

        onDesignSurfaceLosingFocus: "onDesignSurfaceLosingFocus",
        onDesignSurfaceTakingFocus: "onDesignSurfaceTakingFocus",

        onSubtreeDebugExecutionStarted: "onDebugSubtreeExecutionStarted",
        onSubtreeExecutionCompleted: "onDebugSubtreeExecutionCompleted",

        onSubtreeTestSuiteExecutionStarted: "onTestSuiteSubtreeExecutionStarted",
        onSubtreeTestSuiteExecutionCompleted: "onTestSuiteSubtreeExecutionCompleted"
    }
}]);