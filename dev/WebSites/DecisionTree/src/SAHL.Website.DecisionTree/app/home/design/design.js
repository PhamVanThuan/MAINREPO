/// <reference path="dockpanels/debug-breakpoints/deubg-breakpoints.tpl.html" />
'use strict';

angular.module('sahl.tools.app.home.design', [
  'ui.router',
'sahl.tools.app.home.design.file-menu',
'sahl.tools.app.services',
'sahl.tools.app.home.design.dockpanels.code-panel',
'sahl.tools.app.home.design.dockpanels.find-results',
'sahl.tools.app.home.design.dockpanels.properties-panel',
'sahl.tools.app.home.design.dockpanels.debug-breakpoints',
'sahl.tools.app.home.design.dockpanels.debug-variables',
'sahl.tools.app.home.design.dockpanels.output',
'sahl.tools.app.home.design.statuspanels.zoom-panel',
'sahl.tools.app.services.keyboardManager'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.design', {
        url: "/design/{treeId}/{treeVersionId}?isNew&templateTreeVersionId&reload",
        templateUrl: "./app/home/design/design.tpl.html",
        controller: "DesignCtrl"
    });
})

.controller('DesignCtrl', ['$rootScope', '$scope', '$state', '$window', '$designSurfaceManager', '$documentManager', '$stateParams', '$enumerationDataManager', '$variableDataManager', '$eventAggregatorService', '$eventDefinitions', '$modalDialogManager', '$statusbarManager', '$debugService', '$searchManager', '$codeMirrorService', '$keyboardManager','$notificationService', '$timeout',
function DesignController($rootScope, $scope, $state, $window, $designSurfaceManager, $documentManager, $stateParams, enumerationDataManager, variableDataManager, $eventAggregatorService, $eventDefinitions, $modalDialogManager, $statusbarManager, $debugService, $searchManager, $codeMirrorService, $keyboardManager,$notificationService, $timeout) {
    $scope.currentTab = 'home';
    $scope.currentNode = null;
    $scope.bodyContent = $state.current.name !== 'home.design' ? 'bodyContent' : '';

    $scope.southPanelTemplates = [
        {
            name: 'Code',
            url: './app/home/design/dockpanels/code-panel/code-panel.tpl.html',
        },
        {
            name: 'Output',
            url: './app/home/design/dockpanels/output/output.tpl.html'
        },
        {
            name: 'Debug Variables',
            url: './app/home/design/dockpanels/debug-variables/debug-variables.tpl.html'
        },
        {
            name: 'Find Results',
            url: './app/home/design/dockpanels/find-results/find-results.tpl.html'
        }
    ];
    $scope.eastPanelTemplates = [
        {
            name: 'Properties',
            url: './app/home/design/dockpanels/properties-panel/properties-panel.tpl.html'
        },
        {
            name: 'Breakpoints',
            url: './app/home/design/dockpanels/debug-breakpoints/debug-breakpoints.tpl.html'
        }
    ];

    $scope.southPanelWindows = {
        current: $scope.southPanelTemplates[0],
        open: $scope.southPanelTemplates
    };
    $scope.eastPanelWindows = {
        current: $scope.eastPanelTemplates[0],
        open: $scope.eastPanelTemplates
    };
    var southPanelWindowsCurrentWatch = $scope.$watch('southPanelWindows.current', function () {
        // This is so that the code is refreshed if you change node without the code tab open.
        if ($scope.southPanelWindows.current.name == 'Code') {
            $scope.$$postDigest(function () { $codeMirrorService.refresh(); });
        }
    });
    function showSubTreeSelectModal() {
        $modalDialogManager.dialogs.selectSubTree.show().then(function (data) {
            $rootScope.document.selectionManager.current.data.subtree = data;
        },
        function () {
            $rootScope.document.selectionManager.deleteSelection();
        });
    }
    
    var outerLayout, innerLayout;

    var layoutSettings_Outer = {
        name: "outerLayout" // NO FUNCTIONAL USE, but could be used by custom code to 'identify' a layout
        // options.defaults apply to ALL PANES - but overridden by pane-specific settings
    , defaults: {
        size: "auto"
        , minSize: 0
        , paneClass: "pane" 		// default = 'ui-layout-pane'
        , resizerClass: "resizer"	// default = 'ui-layout-resizer'
        , togglerClass: "toggler"	// default = 'ui-layout-toggler'
        , buttonClass: "button"	// default = 'ui-layout-button'
        , contentSelector: ".dockcontent"	// inner div to auto-size so only it scrolls, not the entire pane!
        , contentIgnoreSelector: "span"		// 'paneSelector' for content to 'ignore' when measuring room for content
        , togglerLength_open: 35			// WIDTH of toggler on north/south edges - HEIGHT on east/west edges
        , togglerLength_closed: 35			// "100%" OR -1 = full height
        , hideTogglerOnSlide: true		// hide the toggler when pane is 'slid open'
        , togglerTip_open: "Close This Pane"
        , togglerTip_closed: "Open This Pane"
        , resizerTip: "Resize This Pane"
        //	effect defaults - overridden on some panes
        , fxName: "slide"		// none, slide, drop, scale
        , fxSpeed_open: 500
        , fxSpeed_close: 500
    }
    , south: {
        size: 250
        , spacing_closed: 21			// wider space when closed
        , togglerLength_closed: 21			// make toggler 'square' - 21x21
        , togglerAlign_closed: "top"		// align to top of resizer
        , togglerLength_open: 0 			// NONE - using custom togglers INSIDE east-pane
        , togglerTip_open: "Close South Pane"
        , togglerTip_closed: "Open South Pane"
        , resizerTip_open: "Resize South Pane"
        , slideTrigger_open: "mouseover"
        , initClosed: true
        , onclose_end: function () {
            $scope.codeDockVisible = false;
            $designSurfaceManager.requestUpdate();
            $scope.$apply();
        }
        , onopen_end: function () {
            $scope.codeDockVisible = true;
            $designSurfaceManager.requestUpdate();
            $codeMirrorService.refresh();
            $scope.$apply();
        }
        , onresize_end: function () {
            $designSurfaceManager.requestUpdate();
        }
        , fxSettings: { easing: "" } // nullify default easing
    }
    , east: {
        size: 350
        , spacing_closed: 21			// wider space when closed
        , togglerLength_closed: 21			// make toggler 'square' - 21x21
        , togglerAlign_closed: "top"		// align to top of resizer
        , togglerLength_open: 0 			// NONE - using custom togglers INSIDE east-pane
        , togglerTip_open: "Close East Pane"
        , togglerTip_closed: "Open East Pane"
        , resizerTip_open: "Resize East Pane"
        , slideTrigger_open: "mouseover"
        , initClosed: true
        , onclose_end: function () {
            $scope.varDockVisible = false;
            $designSurfaceManager.requestUpdate();
            $scope.$apply();
        }
        , onopen_end: function () {
            $scope.varDockVisible = true;
            $designSurfaceManager.requestUpdate();
            $scope.$apply();
        }
        , onresize_end: function () {
            $designSurfaceManager.requestUpdate();
        }
        , fxSettings: { easing: "" } // nullify default easing
    }
    , center: {
        paneSelector: "#mainContent" 			// sample: use an ID to select pane instead of a class
        , minWidth: 200
        , minHeight: 200
        , onresize_end: function () {
            $designSurfaceManager.requestUpdate();
        }
    }
    };

    $scope.closeDocument = function () {
        $documentManager.closeDocument();
    }

    $scope.initLayout = function () {
        outerLayout = $(".ui-layout-container").layout(layoutSettings_Outer);

        var eastSelector = ".ui-layout-east"; //outer-east pane
        var southSelector = ".ui-layout-south";

        outerLayout.addPinBtn(eastSelector + " .pin-button", "east");
        outerLayout.addPinBtn(southSelector + " .pin-button", "south");

        $eventAggregatorService.publish($eventDefinitions.onLayoutInitialized, { documentLoaded: $rootScope.document && true });
    };

    $scope.requestNoUpdate = function () {
    }

    $scope.requestUpdate = function () {
        $designSurfaceManager.requestUpdate();
    }

    $scope.toggleEastView = function () {
        outerLayout.toggle('east');
    }

    $scope.toggleSouthView = function () {
        outerLayout.toggle('south');
    }

    $scope.openSouthView = function () {
        outerLayout.open('south');
    }

    var currentTextWatch = $scope.$watch('document.selectionManager.current.data.text', function () {
        if ($rootScope.document) {
            $designSurfaceManager.updateBindings();
        }
    });

    var appLoadingWatch = $scope.$watch('loading', function () {
        if ($rootScope.loading === false) {
            if ($designSurfaceManager !== undefined) {
                if ($designSurfaceManager !== null) {
                    $designSurfaceManager.requestUpdate();
                }
            }
        }
    })

    $scope.$on('$destroy', function () {
        $keyboardManager.unbind('F3');
        $keyboardManager.unbind('DELETE');
        $keyboardManager.unbind('CTRL+F');
        $keyboardManager.unbind('CTRL+R');
        $keyboardManager.unbind('F4');
        $keyboardManager.unbind('F5');
        $keyboardManager.unbind('shift+F5');
        $keyboardManager.unbind('ctrl+shift+F5');
        $keyboardManager.unbind('F6');
        $keyboardManager.unbind('F9');
        $keyboardManager.unbind('F10');
        $keyboardManager.unbind("CTRL+S");

        if (currentTextWatch !== undefined) {
            currentTextWatch();
            currentTextWatch = undefined;
        }

        if (appLoadingWatch !== undefined) {
            appLoadingWatch();
            appLoadingWatch = undefined;
        }

        if (southPanelWindowsCurrentWatch !== undefined) {
            southPanelWindowsCurrentWatch();
            southPanelWindowsCurrentWatch = undefined;
        }

        if (outputMessagesWatch != undefined) {
            outputMessagesWatch();
            outputMessagesWatch = undefined;
        }
    })

    var internalEventHandlers = {
        afterModelChanged: function (change) {
            if (change.object && change.object.changes) {
                for (var i = 0; i < change.object.changes.count; i++) {
                    if (change.object.changes.get(i).change.name === 'Insert' &&
                       (change.object.changes.get(i).newValue.category === 'SubTree' || change.object.changes.get(i).newValue.category === 'ClearMessages')) {
                        showSubTreeSelectModal();
                        break;
                    }
                }
            }
        }
    }

    var debouncedEventHandlers = {
        debouncedAfterModelChanged: $.debounce(500, false, internalEventHandlers.afterModelChanged)
    };

    var eventHandlers = {
        onModelChanged: function (change) {
            internalEventHandlers.afterModelChanged(change);
        },
        onDocumentLoaded: function () {
            outerLayout.resizeAll();
            //if published AND later version, then set mode to readOnly
            if ($rootScope.document.isReadOnly) {
                //readOnly
                $designSurfaceManager.setReadOnly(true);
                $rootScope.document.hasChanges = false;
                
            }
        },
        debugVariablesLoaded: function (packet) {
            $rootScope.globals = {
                globalsVersionsData: packet.data,
                globalsVersionsModel: packet.model,
            }
        },
        onRequestDebugSession: function () {
            $timeout(function () {
                $scope.currentTab = 'debug';
                var debugWindows = $scope.southPanelWindows.open.filter(function(window){
                    return window.name == 'Debug Variables';
                })
                var debugWindow = debugWindows.length > 0 ? debugWindows[0] : undefined;
                if (debugWindow) {
                    $scope.southPanelWindows.current = debugWindow;
                }

            });
            $scope.openSouthView();
        }
    }
    
    //$designSurfaceManager.setReadOnly(false);
    $eventAggregatorService.subscribe($eventDefinitions.onGlobalsLoaded, eventHandlers.debugVariablesLoaded);
    $eventAggregatorService.subscribe($eventDefinitions.onModelCreated, eventHandlers.onModelCreated);
    $eventAggregatorService.subscribe($eventDefinitions.onModelChanged, eventHandlers.onModelChanged);
    $eventAggregatorService.subscribe($eventDefinitions.onDocumentLoaded, eventHandlers.onDocumentLoaded);
    $eventAggregatorService.subscribe($eventDefinitions.onRequestDebugSession, eventHandlers.onRequestDebugSession);

    $scope.initLayout();

    var openOrCreateTreeDocument = function (treeId, treeVersionId, isNew, templateTreeVersionId) {
        if (isNew === "true") {
            if (templateTreeVersionId === null) {
                $documentManager.newDocument(treeId, treeVersionId).then(function () {
                    // $scope.afterDocumentLoad();
                });
            }
            else {
                $documentManager.newFromExistingDocument(templateTreeVersionId, treeId, treeVersionId).then(function () {
                    // $scope.afterDocumentLoad();
                });
            }
        }
        else {
            $documentManager.openDocument(treeId, treeVersionId).then(function () {
                // $scope.afterDocumentLoad();
            });
        }
    }

    // get the treeids from the route
    var treeId = $stateParams.treeId;
    var treeVersionId = $stateParams.treeVersionId;
    var isNew = $stateParams.isNew;
    var templateTreeVersionId = $stateParams.templateTreeVersionId;
    var reloadDocument = $stateParams.reload === undefined ? false : $stateParams.reload;
    if ($rootScope.document === undefined) {
        // open or create a document if one is not currently defined
        openOrCreateTreeDocument(treeId, treeVersionId, isNew, templateTreeVersionId);
    }
    else
        if (($rootScope.document.treeId != treeId || $rootScope.document.treeVersionId != treeVersionId) || reloadDocument) {
            // open or create a document if one is currently defined but does not match the url arguments
            openOrCreateTreeDocument(treeId, treeVersionId, isNew, templateTreeVersionId);
        }
        else {
            // else if there is an existing document just reattach the surface
            //this fails sometimes. Must investigate (dc)
            $designSurfaceManager.attachSurface("designSurface", "designPalette");
        }

    var isInputsValid = function () {
        for (var i = 0, c = $rootScope.document.data.tree.variables.length; i < c; i++) {
            if ($rootScope.document.data.tree.variables[i].usage === "input") {
                if ($rootScope.document.data.tree.variables[i]["$debugValue"] === undefined) {
                    return false;
                }
            }
        }
        return true;
    }

    var debugVariablesInputError = function () {
        $modalDialogManager.dialogs.invalidDebugVariables.show().then(function () {
            if (!$scope.codeDockVisible) {
                $scope.toggleSouthView();
            }
            $scope.southPanelWindows = {
                current: $scope.southPanelTemplates[2],
                open: $scope.southPanelTemplates
            };
        });
    }

    $scope.startDebug = function () {
        if ($rootScope.document.hasChanges || !($rootScope.document.debugger && $rootScope.document.debugger.isDebugging)) {
            if (!$rootScope.document.isPublished && !$rootScope.document.isReadOnly) {
                $documentManager.saveDocument();
            }
            if (isInputsValid()) {
                $debugService.startDebugSession();
            } else {
                debugVariablesInputError();
            }
        }
        else {
            if ($rootScope.document.debugger && $rootScope.document.debugger.isDebugging) {
                $debugService.continueDebugSession();
            } else {
                if (isInputsValid()) {
                    $debugService.startDebugSession();
                } else {
                    debugVariablesInputError();
                }
            }
        }
    }

    $scope.stepOver = function () {
        if ($rootScope.document.debugger && $rootScope.document.debugger.isDebugging) {
            $debugService.stepOverNode();
        } else {
            if (isInputsValid()) {
                $debugService.StartDebugSessionAndStepOver();
            } else {
                debugVariablesInputError();
            }
        }
    }

    $scope.restartDebug = function () {
        $debugService.stopDebugSession();
        $debugService.startDebugSession();
    }

    $scope.stopDebug = function () {
        $debugService.stopDebugSession();
    }

    //signalRSvc.initialize(getMessages);
    $statusbarManager.addStatusbarSegmentedPanel("zoom", undefined, "right");
    // example status panel
    $statusbarManager.addStatusbarSegmentedPanel("textPosition", [{ name: 'Ln', value: 0 }, { name: 'Col', value: 0 }], "right");
    // example set value
    // $statusbarManager.setStatusbarSegmentedPanelValue("textPosition", "Ln", 5);
    // example add second panel
    $statusbarManager.addStatusbarSegmentedPanel("textSelection", [{ name: 'Sel', value: 0 }], "right");
    $statusbarManager.addStatusbarPanelWidget("zoom", "./app/home/design/statuspanels/zoom-panel.tpl.html");
    $statusbarManager.addStatusbarTextPanel("appStatus", "left", "ready");

    $scope.currentSearchItemIndex = -1;

    $scope.searchDocument = function (searchQuery, matchWholeWord, matchCase, filter) {
        if ((searchQuery) && (searchQuery != '')) {
            $searchManager.searchDocument(searchQuery, matchWholeWord, matchCase, filter);
            $scope.currentSearchItemIndex = -1;
            $scope.searchQuery = searchQuery;
            $scope.matchWholeWord = matchWholeWord;
            $scope.matchCase = matchCase;
            $scope.filter = filter;

            if (!$scope.codeDockVisible) {
                $scope.southPanelWindows = {
                    current: $scope.southPanelTemplates[3],
                    open: $scope.southPanelTemplates
                };
                $scope.toggleSouthView();
            } else {
                $scope.southPanelWindows = {
                    current: $scope.southPanelTemplates[3],
                    open: $scope.southPanelTemplates
                };
            }
        }
    }

    $scope.searchFindNextDocument = function () {
        if ($rootScope.searchResults && $rootScope.searchResults.length > 0) {
            $scope.currentSearchItemIndex = $scope.currentSearchItemIndex + 1;
            if ($scope.currentSearchItemIndex >= $rootScope.searchResults.length) {
                $scope.currentSearchItemIndex = 0;
            }
            $eventAggregatorService.publish($eventDefinitions.onChangeSelectedNode, $rootScope.searchResults[$scope.currentSearchItemIndex].nodeKey);
            $eventAggregatorService.publish($eventDefinitions.onSearchItemChanged, $rootScope.searchResults[$scope.currentSearchItemIndex].nodeKey);
            $searchManager.highlightCode($scope.currentSearchItemIndex);
        }
    }

    $scope.searchReplaceNextDocument = function (matchWholeWord, matchCase, replaceText) {
        if ((replaceText) && (replaceText != '') && ($rootScope.searchResults)) {
            $scope.currentSearchItemIndex = $scope.currentSearchItemIndex + 1;
            var code = '';
            if ($scope.currentSearchItemIndex >= 1) {
                code = $searchManager.replaceAtSearchIndex($scope.currentSearchItemIndex, matchWholeWord, matchCase, replaceText);
                $searchManager.searchDocument($scope.searchQuery, $scope.matchWholeWord, $scope.matchCase, $scope.filter);//update search results
            }

            if (code != '') {
                var ev = { data: {} };
                ev.data.code = code;
                $eventAggregatorService.publish($eventDefinitions.onNodeSelectionChanged, ev);
            }

            if ($scope.currentSearchItemIndex >= $rootScope.searchResults.length) {
                $scope.currentSearchItemIndex = 0;
            }
            $eventAggregatorService.publish($eventDefinitions.onChangeSelectedNode, $rootScope.searchResults[$scope.currentSearchItemIndex].nodeKey);
            $eventAggregatorService.publish($eventDefinitions.onSearchItemChanged, $rootScope.searchResults[$scope.currentSearchItemIndex].nodeKey);
            
            $searchManager.highlightCode($scope.currentSearchItemIndex);
        }
    }

    $scope.displayFind = function () {
        if ($('#infoDraggable').is(':visible')) {
            $('#infoDraggable').fadeOut('slow');
        }
        else {
            $('#infoDraggable').fadeIn('slow');
            $('#searchText').focus();
        }
    }

    $scope.removeSearchReplaceText = function () {
        $scope.searchReplaceText = '';
    }

    $scope.removeSearchText = function () {
        $scope.searchText = '';
    }

    var externalEventHandlers = {
        onSearchFilterChanged: function (filter) {
            if (filter == 'node') {
                if ($scope.filter == 'node') {
                    $scope.filter = null
                }
                else {
                    $scope.filter = 'node'
                }
            } else if (filter == 'code') {
                if ($scope.filter == 'code') {
                    $scope.filter = null
                }

                else {
                    $scope.filter = 'code'
                }
            }
            $scope.searchDocument($scope.searchQuery, $scope.matchWholeWord, $scope.matchCase, $scope.filter);
        },
        onSearchItemChanged: function (key) {
            if (!$scope.codeDockVisible) {
                $scope.southPanelWindows = {
                    current: $scope.southPanelTemplates[0],
                    open: $scope.southPanelTemplates
                };
                $scope.toggleSouthView();
            } else {
                $scope.southPanelWindows = {
                    current: $scope.southPanelTemplates[0],
                    open: $scope.southPanelTemplates
                };
            }
        },
        onSearchResultItemChanged: function (searchIndex) {
            $scope.currentSearchItemIndex = searchIndex;
        },
        selectionChanged: function (packet) {
            if ($scope.debugger.isSelectionvalid(packet)) {
                $eventAggregatorService.publish($eventDefinitions.onBreakpointStatus, { "id": packet.data.key, "text": packet.data.text, "validate": $scope.debugger.validate });
                $scope.debugger.node = { "id": packet.data.key, "text": packet.data.text };
                $scope.debugger.canActionBreakpoint = true;
            } else {
                $scope.debugger.reset(false, true);
            }
        },
        breakpointUpdated: function (packet) {
            if ($scope.debugger.node && $scope.debugger.canActionBreakpoint) {
                if (packet.breakpoint.nodeId == $scope.debugger.node.id && packet.breakpoint.name == $scope.debugger.node.text) {
                    $eventAggregatorService.publish($eventDefinitions.onBreakpointStatus, { "id": packet.breakpoint.nodeId, "text": packet.breakpoint.name, "validate": $scope.debugger.validate });
                }
            }
        }
    }

    $scope.debugger = {
        canActionBreakpoint: false,
        hasBreakpoint: false,
        enabled: true,
        node: null,
        validate: function (packet) {
            $scope.debugger.hasBreakpoint = packet.exists;
            $scope.debugger.enabled = packet.enabled;
        },
        toggle: function () {
            if ($scope.debugger.node) {
                var packet = { "id": $scope.debugger.node.id, "text": $scope.debugger.node.text };
                if ($scope.debugger.hasBreakpoint) {
                    if ($scope.debugger.enabled) {
                        $eventAggregatorService.publish($eventDefinitions.onBreakpointDisable, packet);
                    } else {
                        $eventAggregatorService.publish($eventDefinitions.onBreakpointEnable, packet);
                        $scope.debugger.enabled = true;
                    }
                } else {
                    $eventAggregatorService.publish($eventDefinitions.onBreakpointAdd, packet);
                }
            }
        },
        remove: function () {
            var packet = { "id": $scope.debugger.node.id, "text": $scope.debugger.node.text };
            $eventAggregatorService.publish($eventDefinitions.onBreakpointRemove, packet);
            this.reset(true, false);
        },
        reset: function (canActionBreakpoint, resetNode) {
            $scope.debugger.canActionBreakpoint = canActionBreakpoint;
            $scope.debugger.hasBreakpoint = false;
            if (resetNode) {
                $scope.debugger.node = null;
            }
        },
        isSelectionvalid: function (packet) {
            if (!packet) {
                return false;
            }
            if (packet.data["linkType"] !== undefined) {
                return false;
            }
            if (packet.data.category === "End" || packet.data.category === "Start") {
                return false;
            }
            return true
        }
    }

    $scope.getStatus = function () {
        if ($rootScope.document && $rootScope.document.debugger) {
            if ($rootScope.document.debugger.isDebugging) {
                return "debugging";
            }
        }
        return "ready";
    }
    $scope.events = new Array();

    var resetStatus = function () {
        setTimeout($statusbarManager.setStatusbarTextPanelValue("appStatus", $scope.getStatus()), 1000);
    }

    var statusHandler = {
        initilize: function () {
            var ignoreContains = ["Test", "Break", "Search", "Before"];
            for (var arg in $eventDefinitions) {
                var array = arg.substring(2).match(/[A-Z][a-z]+/g);
                if (ignoreContains.indexOf(array[0]) === -1) {
                    (function (innerArg, text) {
                        $scope.events[innerArg] = function () {
                            $statusbarManager.setStatusbarTextPanelValue("appStatus", text);
                            statusHandler.resetDebounced();
                        }
                        var subscriptionHandler = $.debounce(500, false, $scope.events[innerArg]);
                        $eventAggregatorService.subscribe($eventDefinitions[innerArg], subscriptionHandler);
                    })(arg, array.join(" "));
                }
            }
        },
        resetDebounced: $.debounce(2000, false, resetStatus)
    }

    statusHandler.initilize();

    $eventAggregatorService.subscribe($eventDefinitions.onNodeSelectionChanged, externalEventHandlers.selectionChanged);
    $eventAggregatorService.subscribe($eventDefinitions.onBreakpointStatusChanged, externalEventHandlers.breakpointUpdated);
    
    $eventAggregatorService.subscribe($eventDefinitions.onSearchItemChanged, externalEventHandlers.onSearchItemChanged);
    $eventAggregatorService.subscribe($eventDefinitions.onSearchResultItemChanged, externalEventHandlers.onSearchResultItemChanged);
    $eventAggregatorService.subscribe($eventDefinitions.onSearchFilterChanged, externalEventHandlers.onSearchFilterChanged);

    $keyboardManager.bind('f3', $scope.searchFindNextDocument);
    var targetForDelete = $("#designSurfaceContent");
    $keyboardManager.bind('DELETE', function () {
        if ($scope.document.selectionManager.canDelete) {
            $scope.document.selectionManager.deleteSelection()
        }
    }, { target: targetForDelete });
    $keyboardManager.bind('CTRL+F', $scope.displayFind);
    $keyboardManager.bind('CTRL+R', $scope.displayFind);
    $keyboardManager.bind('F4', $scope.toggleEastView);
    $keyboardManager.bind('F5', $scope.startDebug);
    $keyboardManager.bind('shift+F5', $scope.stopDebug);
    $keyboardManager.bind('ctrl+shift+F5', $scope.restartDebug);
    $keyboardManager.bind('F6', $scope.toggleSouthView);
    $keyboardManager.bind('F9', $scope.debugger.toggle);
    $keyboardManager.bind('F10', $scope.stepOver);
    $keyboardManager.bind("CTRL+S", $documentManager.saveDocument);

    var outputMessagesWatch = $scope.$watch('document.dataModel.outputMessagesArray', function () {
        if ($rootScope.document) {
            $eventAggregatorService.publish($eventDefinitions.onOutputMessagesChanged, $rootScope.document.dataModel.outputMessagesArray);
        }
    }, true
    );
}]);