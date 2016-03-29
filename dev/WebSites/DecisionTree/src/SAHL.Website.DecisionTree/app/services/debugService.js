'use strict';

/* Services */

angular.module('sahl.tools.app.services.debugService', [
    'sahl.tools.app.serviceConfig',
    'sahl.tools.app.home.design.dockpanels.debug-breakpoints'
])
.factory('$debugService', ['$rootScope', '$signalRSvc', '$eventAggregatorService', '$eventDefinitions', '$queryManager', '$decisionTreeDesignQueries', '$q', '$breakpointService', '$queryGlobalsVersionService', '$notificationService', '$documentManager', '$designSurfaceManager', '$utils',
    function ($rootScope, $signalRSvc, $eventAggregatorService, $eventDefinitions, $queryManager, $decisionTreeDesignQueries, $q, $breakpointService, $queryGlobalsVersionService, $notificationService, $documentManager, $designSurfaceManager, $utils) {
        var objectModels = {
            debugModel: function () {
                return {
                    "debugSessionId": $rootScope.document.debugger.currentSessionId,
                    "name": $rootScope.document.data.name,
                    "version": $rootScope.document.data.version,
                    "treeVersionId": $rootScope.document.treeVersionId,
                    "breakpoints": $breakpointService.getActiveBreakPoints(),
                    'globalsVersions': $queryGlobalsVersionService.QueryGlobalsVersion(),
                    "input_variables": getInputVariables()
                };
            },
            inputVariable: function (id, name, value) {
                return {
                    "id": id,
                    "name": name,
                    "value": value
                };
            }
        }

        var getInputVariables = function () {
            var retVal = new Array();
            angular.forEach($rootScope.document.data.tree.variables, function (item) {
                if (item.usage === "input") {
                    retVal.push(objectModels.inputVariable(item.id, item.name, item.$debugValue));
                }
            });
            return retVal;
        }

        var connectingPromise = null;

        var nodesExecuting = [];
        var subtreeVariablesDictionary = {};

        var populateSubtreeVariablesDictionary = function (subtreeId, variables_array, variables_usage) {
            if(!(subtreeId in subtreeVariablesDictionary)){
                subtreeVariablesDictionary[subtreeId] = {};
            }
            subtreeVariablesDictionary[subtreeId][variables_usage] = variables_array;
        }

        var scenarioExecutedSubtreesDictionary = {}
        var populateScenarioExecutedSubtreesDictionary = function (scenario_id, subtree_name) {
            if (!(scenario_id in scenarioExecutedSubtreesDictionary)) {
                scenarioExecutedSubtreesDictionary[scenario_id] = [];
            }
            subtree_name = $utils.string.sanitise(subtree_name);
            subtree_name = $utils.string.capitaliseFirstLetter(subtree_name);
            scenarioExecutedSubtreesDictionary[scenario_id].push(subtree_name);
        }

        function configureTimeout(key, timeout) {
            setTimeout(function () { $eventAggregatorService.publish($eventDefinitions.onChangeSelectedNode, key); }, timeout);
        }

        var runThroughTree = function (nodes) {   
            var i = 1            
            while (i < nodes.length + 1) {
                configureTimeout(nodes[i - 1], 450 * i);
                i++;
            }
        }        

        var eventHandlers = {
            onDocumentLoaded: function (loadedDocument) {
                var query = new $decisionTreeDesignQueries.GetNewGuidQuery();
                $queryManager.sendQueryAsync(query).then(function (data) {
                    var newSessionId = data.data.ReturnData.Results.$values[0].Id;
                    // otherwise use user selected versions if available
                    loadedDocument.debugger = {
                        isDebugging: false,
                        isStopped: false,
                        currentSessionId: newSessionId,
                        breakpoints: [],
                        input_variables: [],
                    }
                });
            },
            onDocumentUnloaded: function (unloadedDocument) {
                unloadedDocument.debugger = null;
            },
            onDebugSessionStarted: function (debugSessionId) {
                nodesExecuting = [];
                $rootScope.document.debugger.isDebugging = true;
                $rootScope.document.debugger.isStopped = false;
                console.log('debug started on server');
                $eventAggregatorService.publish($eventDefinitions.onDebugSessionStarted, { "debugSessionId": debugSessionId });
            },
            onDebugError: function (debugSessionId, errorMessage) {
                $rootScope.document.debugger.isDebugging = false;
                $rootScope.document.debugger.isStopped = false;

                if (typeof errorMessage === 'object') {
                    $documentManager.addOutputMessage({ 'message': errorMessage.message, 'type': 'error', 'source': errorMessage.source, 'line': errorMessage.line, 'node': errorMessage.node, 'errortype': errorMessage.errortype });
                }
                else {
                    $documentManager.addOutputMessage({ 'message': 'Debug error message: ' + errorMessage, 'type': 'error', 'source': 'Debug session:- ' + debugSessionId, 'line': errorMessage.line });
                }
                $eventAggregatorService.publish($eventDefinitions.onDebugError, { "debugSessionId": debugSessionId, "errorMessage": errorMessage });
            },
            onDebugSessionEnded: function (debugSessionId) {
                $rootScope.document.debugger.isDebugging = false;
                $rootScope.document.debugger.isStopped = false;
                $eventAggregatorService.publish($eventDefinitions.onDebugSessionEnded, { "debugSessionId": debugSessionId });
            },
            onDebugLocationChanged: function (debugSessionId, previousNodeId, previousNodeResult, newNodeId, nodeResult) {
                nodesExecuting.push(newNodeId);
                $eventAggregatorService.publish($eventDefinitions.onDebugLocationChanged, { "debugSessionId": debugSessionId, "previousNodeId": previousNodeId, "previousNodeResult": previousNodeResult, "newNodeId": newNodeId, "nodeResult": nodeResult });
            },
            onExecutionStoppedAtNode: function (debugSessionId, nodeId, output_variables, messages) {
                $rootScope.document.debugger.isStopped = true;
                $eventAggregatorService.publish($eventDefinitions.onExecutionStoppedAtNode, { "debugSessionId": debugSessionId, "nodeId": nodeId, "output_variables": output_variables, "messages": messages });
            },
            onExecutionCompleted: function (debugSessionId, output_variables, messages) {
                console.log(nodesExecuting);
                runThroughTree(nodesExecuting);
                $rootScope.document.debugger.isDebugging = false;
                $rootScope.document.debugger.isStopped = false;
                $eventAggregatorService.publish($eventDefinitions.onExecutionCompleted, { "debugSessionId": debugSessionId, "output_variables": output_variables, "messages": messages });
            },
            onTestSuiteExecutionStarted: function (testSuiteSessionId) {
                $eventAggregatorService.publish($eventDefinitions.onTestSuiteExecutionStarted, { "testSuiteSessionId": testSuiteSessionId });
            },
            onTestSuiteStoryExecutionStarted: function (testSuiteStoryId) {
                scenarioExecutedSubtreesDictionary = {};
                $eventAggregatorService.publish($eventDefinitions.onTestSuiteStoryExecutionStarted, { "testSuiteStoryId": testSuiteStoryId });
            },
            onTestSuiteStoryScenarioExecutionStarted: function (testSuiteStoryId, testSuiteStoryScenarioId) {
                $eventAggregatorService.publish($eventDefinitions.onTestSuiteStoryScenarioExecutionStarted, { "testSuiteStoryId": testSuiteStoryId, "testSuiteStoryScenarioId": testSuiteStoryScenarioId });
            },
            onTestSuiteExecutionLocationChanged: function (testSuiteSessionId, previousNodeId, previousNodeResult, newNodeId, nodeResult) {
                $eventAggregatorService.publish($eventDefinitions.onTestSuiteExecutionLocationChanged, { "testSuiteSessionId": testSuiteSessionId, "previousNodeId": previousNodeId, "previousNodeResult": previousNodeResult, "newNodeId": newNodeId, "nodeResult": nodeResult });
            },
            onTestSuiteStoryScenarioExecutionError: function (testSuiteStoryId, testSuiteStoryScenarioId, errorMessage) {
                $eventAggregatorService.publish($eventDefinitions.onTestSuiteStoryScenarioExecutionError, { "testSuiteStoryId": testSuiteStoryId, "testSuiteStoryScenarioId": testSuiteStoryScenarioId, "errorMessage": errorMessage });
            },
            onTestSuiteStoryScenarioExecutionCompleted: function (testSuiteStoryId, testSuiteStoryScenarioId, output_variables, messages) {
                $eventAggregatorService.publish($eventDefinitions.onTestSuiteStoryScenarioExecutionCompleted, { "testSuiteStoryId": testSuiteStoryId, "testSuiteStoryScenarioId": testSuiteStoryScenarioId, "output_variables": output_variables, "messages": messages });
            },
            onTestSuiteStoryExecutionCompleted: function (testSuiteStoryId) {
                $eventAggregatorService.publish($eventDefinitions.onTestSuiteStoryExecutionCompleted, { "testSuiteStoryId": testSuiteStoryId });
            },
            onTestSuiteExecutionCompleted: function (testSuiteSessionId) {
                $eventAggregatorService.publish($eventDefinitions.onTestSuiteExecutionCompleted, { "testSuiteSessionId": testSuiteSessionId });
            },
            onTestSuiteSubtreeExecutionStarted: function (testSuiteSessionId, testSuiteStoryId, testSuiteStoryScenarioId, subtreeId, subtreeName, input_variables) {
                populateScenarioExecutedSubtreesDictionary(testSuiteStoryScenarioId, subtreeName);
                $eventAggregatorService.publish($eventDefinitions.onTestSuiteSubtreeExecutionStarted, { "testSuiteSessionId": testSuiteSessionId, "testSuiteStoryId": testSuiteStoryId, "testSuiteStoryScenarioId": testSuiteStoryScenarioId, "subtreeId": subtreeId, "input_variables": input_variables });
            },
            onTestSuiteSubtreeExecutionCompleted: function (testSuiteSessionId, testSuiteStoryId, testSuiteStoryScenarioId, subtreeId, output_variables, messages) {
                $eventAggregatorService.publish($eventDefinitions.onTestSuiteSubtreeCompleted, { "testSuiteSessionId": testSuiteSessionId, "testSuiteStoryId": testSuiteStoryId, "testSuiteStoryScenarioId": testSuiteStoryScenarioId, "subtreeId": subtreeId, "output_variables": output_variables, "messages": messages });
            },
            onDebugSubtreeExecutionStarted: function (debugSessionId, subtreeId, input_variables) {
                populateSubtreeVariablesDictionary(subtreeId, input_variables, "inputs");
                $eventAggregatorService.publish($eventDefinitions.onDebugSubtreeExecutionStarted, { "debugSessionId": debugSessionId, "subtreeId": subtreeId, "input_variables": input_variables });
            },
     
            onDebugSubtreeExecutionCompleted: function (debugSessionId, subtreeId, output_variables, messages) {
                populateSubtreeVariablesDictionary(subtreeId, output_variables, "outputs", messages);
                $eventAggregatorService.publish($eventDefinitions.onDebugSubtreeExecutionCompleted, { "debugSessionId": debugSessionId, "subtreeId": subtreeId, "output_variables": output_variables, "messages": messages });
            }
        }

        function ensureSignalRunning() {
            return $signalRSvc.startHub();
        }

        $eventAggregatorService.subscribe($eventDefinitions.onDebugInputVariablesChanged, function(){subtreeVariablesDictionary = {}});
        $eventAggregatorService.subscribe($eventDefinitions.onDocumentLoaded, eventHandlers.onDocumentLoaded);
        $eventAggregatorService.subscribe($eventDefinitions.onDocumentUnloaded, eventHandlers.onDocumentUnloaded);

        return {
            start: function () {
                var deferred = $q.defer();
                $signalRSvc.initialiseProxies().then(function () {
                    // setup a callback for each message typer now
                    $signalRSvc.getProxy().on('onDebugSessionStarted', function (debugSessionId) {
                        eventHandlers.onDebugSessionStarted(debugSessionId);
                        if ($rootScope.document.dataModel.outputMessagesArray !== undefined) {
                            $rootScope.document.dataModel.outputMessagesArray.length = 0;
                        }
                    });
                    $signalRSvc.getProxy().on('onDebugError', function (debugSessionId, errorMessage) {
                        eventHandlers.onDebugError(debugSessionId, errorMessage);
                    });

                    $signalRSvc.getProxy().on('onDebugSessionEnded', function (debugSessionId) {
                        eventHandlers.onDebugSessionEnded(debugSessionId);
                    });

                    $signalRSvc.getProxy().on('onDebugLocationChanged', function (debugSessionId, previousNodeId, previousNodeResult, newNodeId, nodeResult) {
                        eventHandlers.onDebugLocationChanged(debugSessionId, previousNodeId, previousNodeResult, newNodeId, nodeResult);
                    });

                    $signalRSvc.getProxy().on('onExecutionStoppedAtNode', function (debugSessionId, nodeId, output_variables, messages) {
                        eventHandlers.onExecutionStoppedAtNode(debugSessionId, nodeId, output_variables, messages);
                    });

                    $signalRSvc.getProxy().on('onExecutionCompleted', function (debugSessionId, output_variables, messages) {
                        eventHandlers.onExecutionCompleted(debugSessionId, output_variables, messages);
                        if (messages === undefined) {
                            messages = [];
                        }
                        if ($rootScope.document.dataModel.outputMessagesArray !== undefined) {
                            for (var i = 0, c = messages.length; i < c; i++) {

                                $documentManager.addOutputMessage({ 'message': messages[i].message, 'type': messages[i].type, 'source': '', 'line': '' });
                            }
                        }
                        $notificationService.notifyInfo("Debugging completed", "")
                    });

                    $signalRSvc.getProxy().on('onTestSuiteExecutionStarted', function (testSuiteSessionId) {
                        eventHandlers.onTestSuiteExecutionStarted(testSuiteSessionId);
                    });

                    $signalRSvc.getProxy().on('onTestSuiteStoryExecutionStarted', function (testSuiteStoryId) {
                        eventHandlers.onTestSuiteStoryExecutionStarted(testSuiteStoryId);
                    });

                    $signalRSvc.getProxy().on('onTestSuiteStoryScenarioExecutionStarted', function (testSuiteStoryId, testSuiteStoryScenarioId) {
                        eventHandlers.onTestSuiteStoryScenarioExecutionStarted(testSuiteStoryId, testSuiteStoryScenarioId);
                    });

                    $signalRSvc.getProxy().on('onTestSuiteStoryScenarioExecutionError', function (testSuiteStoryId, testSuiteStoryScenarioId, errorMessage) {
                        eventHandlers.onTestSuiteStoryScenarioExecutionError(testSuiteStoryId, testSuiteStoryScenarioId, errorMessage);
                    });

                    $signalRSvc.getProxy().on('onTestSuiteStoryScenarioExecutionCompleted', function (testSuiteStoryId, testSuiteStoryScenarioId, output_variables, messages) {
                        eventHandlers.onTestSuiteStoryScenarioExecutionCompleted(testSuiteStoryId, testSuiteStoryScenarioId, output_variables, messages);
                    });

                    $signalRSvc.getProxy().on('onTestSuiteStoryExecutionCompleted', function (testSuiteStoryId) {
                        eventHandlers.onTestSuiteStoryExecutionCompleted(testSuiteStoryId);
                    });

                    $signalRSvc.getProxy().on('onTestSuiteExecutionCompleted', function (testSuiteSessionId) {
                        eventHandlers.onTestSuiteExecutionCompleted(testSuiteSessionId);
                    });

                    $signalRSvc.getProxy().on('onTestSuiteExecutionLocationChanged', function (testSuiteSessionId, previousNodeId, previousNodeResult, newNodeId, nodeResult) {
                        eventHandlers.onTestSuiteExecutionLocationChanged(testSuiteSessionId, previousNodeId, previousNodeResult, newNodeId, nodeResult);
                    });

                    $signalRSvc.getProxy().on('onTestSuiteSubtreeExecutionStarted', function (testSuiteSessionId, testSuiteStoryId, testSuiteStoryScenarioId, subtreeId, subtreeName, input_variables) {
                        eventHandlers.onTestSuiteSubtreeExecutionStarted(testSuiteSessionId, testSuiteStoryId, testSuiteStoryScenarioId, subtreeId, subtreeName, input_variables);
                    });

                    $signalRSvc.getProxy().on('onTestSuiteSubtreeExecutionCompleted', function (testSuiteSessionId, testSuiteStoryId, testSuiteStoryScenarioId, subtreeId, output_variables, messages) {
                        eventHandlers.onTestSuiteSubtreeExecutionCompleted(testSuiteSessionId, testSuiteStoryId, testSuiteStoryScenarioId, subtreeId, output_variables, messages);
                    });

                    $signalRSvc.getProxy().on('onDebugSubtreeExecutionStarted', function (debugSessionId, subtreeId, input_variables) {
                        eventHandlers.onDebugSubtreeExecutionStarted(debugSessionId, subtreeId, input_variables);
                    });

                    $signalRSvc.getProxy().on('onDebugSubtreeExecutionCompleted', function (debugSessionId, subtreeId, output_variables) {
                        eventHandlers.onDebugSubtreeExecutionCompleted(debugSessionId, subtreeId, output_variables);
                    });

                    deferred.resolve();
                });

                return deferred.promise;
            },
            startDebugSession: function () {
                ensureSignalRunning().then(function () {
                    console.log('starting debug');
                    $signalRSvc.getProxy().invoke('StartDebugSession', objectModels.debugModel());
                });
            },
            StartDebugSessionAndStepOver: function () {
                ensureSignalRunning().then(function () {
                    $signalRSvc.getProxy().invoke('StartDebugSessionAndStepOver', objectModels.debugModel());
                });
            },
            stepOverNode: function () {
                ensureSignalRunning().then(function () {
                    $signalRSvc.getProxy().invoke('StepOverNode', $rootScope.document.debugger.currentSessionId);
                });
            },

            continueDebugSession: function () {
                ensureSignalRunning().then(function () {
                    $signalRSvc.getProxy().invoke('ContinueDebugSession', {
                        "debugSessionId": $rootScope.document.debugger.currentSessionId,
                        "breakpoints": $rootScope.document.debugger.breakpoints
                    });
                });
            },
            stopDebugSession: function () {
                ensureSignalRunning().then(function () {
                    $signalRSvc.getProxy().invoke('StopDebugSession', {
                        "debugSessionId": $rootScope.document.debugger.currentSessionId
                    });
                });
            },

            startTestSuite: function (testSuiteData) {
                testSuiteData.testSuiteSessionId = $rootScope.document.debugger.currentSessionId;

                ensureSignalRunning().then(function () {
                    $signalRSvc.getProxy().invoke('ExecuteTestSuite', testSuiteData);
                });
            },
            cancelTestSuite: function (testSuiteSessionId) {
                testSuiteSessionId = $rootScope.document.debugger.currentSessionId,
                ensureSignalRunning().then(function () {
                    $signalRSvc.getProxy().invoke('CancelTestSuiteExecution', testSuiteSessionId);
                });
            },

            getDebugSessionSubtreeInputs: function (subtreeId) {
                var result = [];
                if (subtreeVariablesDictionary[subtreeId] != undefined && subtreeVariablesDictionary[subtreeId]["inputs"] != undefined) {
                    result = subtreeVariablesDictionary[subtreeId]["inputs"];
                }
                return result;
            },
            getDebugSessionSubtreeOutputs: function (subtreeId) {
                var result = [];
                if (subtreeVariablesDictionary[subtreeId] != undefined && subtreeVariablesDictionary[subtreeId]["outputs"] != undefined) {
                    result = subtreeVariablesDictionary[subtreeId]["outputs"];
                }
                return result;
            },

            getTestSuiteExecutedSubtrees: function () {
                return scenarioExecutedSubtreesDictionary;
            }
        }
    }]);