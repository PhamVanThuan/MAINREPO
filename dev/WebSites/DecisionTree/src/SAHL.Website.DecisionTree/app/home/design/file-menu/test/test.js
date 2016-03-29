'use strict';

angular.module('sahl.tools.app.home.design.file-menu.test', [
    'ui.router',
    'sahl.tools.app.services'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.design.file-menu.test', {
        url: "/test",
        templateUrl: "./app/home/design/file-menu/test/test.tpl.html",
        controller: 'FileTestCtrl'
    });
})
.controller('FileTestCtrl', ['$rootScope', '$scope', '$state', '$keyboardManager', '$documentManager', '$stateParams', '$eventAggregatorService', '$eventDefinitions', '$modalDialogManager', '$enumerationDataManager', '$debugService', '$notificationService', '$testDataManager', '$timeout', '$activityManager', '$utils',
        function FileTestCtrl($rootScope, $scope, $state, $keyboardManager, $documentManager, $stateParams, $eventAggregatorService, $eventDefinitions, $modalDialogManager, enumerationDataManager, $debugService, $notificationService, $testDataManager, $timeout, $activityManager, $utils) {
            $scope.currentTab = 'test-suite';

            $scope.boolSelect = [true, false];

            $scope.testCases = [];

            $scope.currentTestSuiteId = -1;

            $scope.isExecuting = false;

            $scope.hasTestResult = false;

            $scope.outputExpectationTypes = [
                "inline",
                "test-variable"
            ]

            $scope.assertions = [
                "to equal",
            ];

            $scope.messageAssertions = [
                "should contain",
                "should not contain",
                "should be empty"
            ];

            $scope.subtreeAssertions = [
                "should have been called",
                "should not have been called"
            ]

            $scope.subtreeAssertionResults = [
                "was called",
                "was not called"
            ]

            $scope.messageCollections = [
                "Error Messages",
                "Info Messages",
                "Warning Messages"
            ]

            $scope.nodeData = {};

            $scope.addNewStory = function () {
                $modalDialogManager.dialogs.addTestStory.show()
                    .then(function (data) {
                        $testDataManager.addNewTestCase(data);
                        refreshAccordions();
                    });
            }

            $scope.removeTestStory = function (testCaseIndex, e) {
                e.stopPropagation();
                $modalDialogManager.dialogs.deleteTestStoryConfirm.show($rootScope.document.data.testCases[testCaseIndex].name).then(function () {
                    $rootScope.document.data.testCases.splice(testCaseIndex, 1);
                });
            }

            $scope.addScenario = function (testCaseId) {
                $modalDialogManager.dialogs.addTestScenario.show()
                    .then(function (data) {
                        $testDataManager.addScenario(data, testCaseId);
                        refreshAccordionsAndOpenLast();
                    });
            }

            $scope.removeScenario = function (testCaseIndex, scenarioIndex) {
                var scenarios = $rootScope.document.data.testCases[testCaseIndex].scenarios
                $modalDialogManager.dialogs.deleteTestScenarioConfirm.show(scenarios[scenarioIndex].name).
                then(function () {
                    scenarios.splice(scenarioIndex, 1);
                });
            }

            $scope.addInput = function (testCaseIndex, scenarioIndex) {
                var scenario = $rootScope.document.data.testCases[testCaseIndex].scenarios[scenarioIndex];
                var availableInputs = $testDataManager.getUnusedVariablesInSet($scope.inputVariables, scenario.input_values);
                if (availableInputs.length > 0) {
                    $modalDialogManager.dialogs.addTestingInputVariable.show(availableInputs)
                        .then(function (data) {
                            $testDataManager.addInputVariable(data, scenario);
                        });
                } else {
                    $notificationService.notifyInfo('No more inputs', 'There are no more input variables available on the tree.');
                }
            }

            $scope.removeInput = function (testCaseIndex, scenarioIndex, varId) {
                var scenario = $rootScope.document.data.testCases[testCaseIndex].scenarios[scenarioIndex];
                var variable = $testDataManager.findVariableById(scenario.input_values, varId);
                var index = scenario.input_values.indexOf(variable);
                if (index >= 0) {
                    scenario.input_values.splice(index, 1);
                }
            }

            var validateVariableValue = function (variableValue) {
                if (variableValue !== undefined && variableValue !== '') {
                    return true;
                }

                return false;
            }

            $scope.addOutput = function (testCaseIndex, scenarioIndex) {
                var scenario = $rootScope.document.data.testCases[testCaseIndex].scenarios[scenarioIndex];
                var availableOutputs = $testDataManager.getUnusedVariablesInSet($scope.outputVariables, scenario.output_values);

                if (availableOutputs.length > 0) {
                    $modalDialogManager.dialogs.addTestingOutputVariable.show(availableOutputs, $scope.outputExpectationTypes, $rootScope.document.data.testVariables, { validate: validateVariableValue })
                        .then(function (data) {
                            $testDataManager.addOutputVariable(data, scenario);
                        });
                } else {
                    $notificationService.notifyInfo('No more outputs', 'There are no more output variables available on the tree.');
                }
            }

            $scope.removeOutput = function (testCaseIndex, scenarioIndex, varId) {
                var scenario = $rootScope.document.data.testCases[testCaseIndex].scenarios[scenarioIndex];
                var variable = $testDataManager.findVariableById(scenario.output_values, varId);
                var index = scenario.output_values.indexOf(variable);
                if (index >= 0) {
                    scenario.output_values.splice(index, 1);
                }
            }

            $scope.addMessageOutput = function (testCaseIndex, scenarioIndex) {
                var scenario = $rootScope.document.data.testCases[testCaseIndex].scenarios[scenarioIndex];
                if (scenario.output_messages === undefined) {
                    scenario.output_messages = [];
                }
                $testDataManager.addOutputMessage("", scenario);
            }

            $scope.selectMessage = function (testCaseIndex, scenarioIndex, outputMessage) {
                var scenario = $rootScope.document.data.testCases[testCaseIndex].scenarios[scenarioIndex];
                var availableMessages = $testDataManager.getUnusedMessagesInSet($scope.outputmessages, scenario.output_messages);
                if (availableMessages.length > 0) {
                    $modalDialogManager.dialogs.addTestingOutputMessage.show(availableMessages)
                        .then(function (data) {
                            outputMessage.message = data;
                        });
                } else {
                    $notificationService.notifyInfo('No more messages', 'There are no more messages available.');
                }
            }

            $scope.removeMessageOutput = function (testCaseIndex, scenarioIndex, messageName) {
                var scenario = $rootScope.document.data.testCases[testCaseIndex].scenarios[scenarioIndex];
                var message = $testDataManager.findMessageByMessageName(scenario.output_messages, messageName);
                var index = scenario.output_messages.indexOf(message);
                if (index >= 0) {
                    scenario.output_messages.splice(index, 1);
                }
            }

            $scope.addSubtreeExpectation = function (testCaseIndex, scenarioIndex) {
                var scenario = $rootScope.document.data.testCases[testCaseIndex].scenarios[scenarioIndex];

                var availableSubtrees = $testDataManager.getUnusedSubtreesInSet($scope.subtrees, scenario.subtrees);
                if (availableSubtrees.length > 0) {
                    $modalDialogManager.dialogs.addSubtreeExpectation.show(availableSubtrees)
                        .then(function (data) {
                            $testDataManager.addSubtreeExpectation(data, scenario);
                        });
                } else {
                    $notificationService.notifyInfo('No more subtrees', 'There are no more subtrees available on the tree.');
                }
            }

            $scope.removeSubtreeExpectation = function (testCaseIndex, scenarioIndex, subtreeName) {
                var scenario = $rootScope.document.data.testCases[testCaseIndex].scenarios[scenarioIndex];
                var subtree_expectation = $testDataManager.findSubtreeByName(scenario.subtrees, subtreeName);
                var index = scenario.subtrees.indexOf(subtree_expectation);
                if (index > 0) {
                    scenario.subtrees.splice(index, 1);
                }
            }

            $scope.addTestVariable = function () {
                var variable = $testDataManager.getEmptyTestVariable();

                $modalDialogManager.dialogs.addTestingVariable.show(variable, { validate: validateVariableValue })
                        .then(function (data) {
                            $testDataManager.addTestVariable(data, $rootScope.document.data.testVariables);
                        });
            }

            $scope.removeTestVariable = function (testVariable) {
                // remove the test variable and set any output expectations that use it to empty inline expectations
                var index = $rootScope.document.data.testVariables.indexOf(testVariable);
                if (index >= 0) {
                    $rootScope.document.data.testVariables.splice(index, 1);
                }

                for (var i = 0, c = $rootScope.document.data.testCases.length; i < c; i++) {
                    var testCase = $rootScope.document.data.testCases[i];
                    for (var k = 0, d = testCase.scenarios.length; k < d; k++) {
                        var scenario = testCase.scenarios[k];

                        var outputs = scenario.output_values.filter(function (item) {
                            return item.expectationType === "test-variable" && item.value === testVariable.id;
                        })

                        for (var j = 0, e = outputs.length; j < e; j++) {
                            var output = outputs[j];
                            output.expectationType = "inline";
                            output.value = "";
                        }
                    }
                }
            }

            $scope.runAllTests = function () {
                if (validateAllTestCases()) {
                    closeAllAccordions();
                    activateProgressBars();
                    var testSuiteData = $testDataManager.getTestSuiteDataForAll();
                    $debugService.startTestSuite(testSuiteData);
                } else {
                    $notificationService.notifyError('Validation errors', 'There are validation errors in the test cases. Please ensure that all tree inputs have a value.');
                }
            }

            $scope.runScenarios = function (testCaseId, e) {
                e.stopPropagation();
                var testCase = $testDataManager.getTestCase($rootScope.document.data.testCases, testCaseId);
                if (validateTestCase(testCase)) {
                    var testSuiteData = $testDataManager.getTestSuiteDataForSuite(testCaseId);
                    $debugService.startTestSuite(testSuiteData);
                } else {
                    $notificationService.notifyError('Validation errors', 'There are validation errors in the test cases. Please ensure that all tree inputs have a value.');
                }
            }

            $scope.cancelTests = function (testCaseId, e) {
                e.stopPropagation();

                var testCase = $testDataManager.getTestCase($rootScope.document.data.testCases, testCaseId);
                $debugService.cancelTestSuite($rootScope.document.debugger.currentSessionId);
            }

            $scope.cancelAllTests = function (e) {
                if ($scope.currentTestSuiteId !== -1) {
                    $scope.cancelTests($scope.currentTestSuiteId, e);
                }
            }

            $scope.debugScenario = function (testCaseIndex, scenarioIndex, e) {
                e.stopPropagation();

                var testCase = $rootScope.document.data.testCases[testCaseIndex];
                var scenario = $rootScope.document.data.testCases[testCaseIndex].scenarios[scenarioIndex];
                var mergedInputs = angular.fromJson(angular.toJson(testCase.input_values));

                // merge with the scenario inputs
                for (var i = 0, c = scenario.input_values.length; i < c; i++) {
                    var scenarioVariable = scenario.input_values[i];
                    var storyVariable = undefined;
                    var storyVariables = mergedInputs.filter(function (storyVariable) {
                        return storyVariable.id === scenarioVariable.id;
                    })
                    if (storyVariables.length == 1) {
                        storyVariable = storyVariables[0];
                    }
                    if (storyVariable !== undefined) {
                        storyVariable.value = scenarioVariable.value;
                    }
                }

                $eventAggregatorService.publish($eventDefinitions.onRequestSetDebugVariables, mergedInputs);
                setTimeout(function () {
                    $eventAggregatorService.publish($eventDefinitions.onRequestDebugSession);
                }, 500);
                $scope.go_back_no_check();
            }

            function checkAllTreeInputsEntered(scInputs, tcInputs) {
                var result = true;
                for (var i = 0; i < tcInputs.length; i++) {
                    var inputResult = true;
                    if ((tcInputs[i].type != "bool" && tcInputs[i].value == "") || (tcInputs[i].type == "bool" && tcInputs[i].value === "")) { // 0 is falsey, but it would be valid in this case. //&& (tcInputs[i].value !== false || tcInputs[i].value !== 0)
                        var scInput = $testDataManager.findVariableById(scInputs, tcInputs[i].id);
                        if (!scInput) {
                            inputResult = false;
                        } else if (scInput.value !== 0 && !scInput.value) {
                            inputResult = false;
                            scInput.$treeValueNotDefined = true;
                        } else {
                            scInput.$treeValueNotDefined = false;
                        }
                    }
                    tcInputs[i].$treeValueNotDefined = !inputResult;
                    if (!inputResult) {
                        result = false;
                    }
                }
                return result;
            }

            function validateAllTestCases() {
                var valid = true;
                $.each($rootScope.document.data.testCases, function (index, testCase) {
                    var testCaseValid = validateTestCase(testCase);
                    if (!testCaseValid) {
                        valid = false;
                    }
                });
                return valid;
            }

            function validateTestCase(testCase) {
                var valid = true;
                $.each(testCase.scenarios, function (i, scenario) {
                    var scenarioValid = checkAllTreeInputsEntered(scenario.input_values, testCase.input_values);
                    if (!scenarioValid) {
                        valid = false;
                        scenario.$treeValueNotDefined = true;
                    } else {
                        scenario.$treeValueNotDefined = false;
                    }
                });
                return valid;
            }

            var testEventHandlers = {
                onTestSuiteExecutionStarted: function (testSuiteSessionId) {
                    $scope.isExecuting = true;
                    $scope.hasTestResult = false;
                    $scope.nodeData.coveredLinks.length = 0;
                    $scope.nodeData.totalCoverage = 0;
                },
                onTestSuiteExecutionCompleted: function (testSuiteSessionId) {
                    $scope.isExecuting = false;
                    $scope.$apply();
                },
                onTestSuiteStoryExecutionStarted: function (testSuiteStoryId) {
                    setTestSuiteStoryToExecuting(testSuiteStoryId.testSuiteStoryId);
                },
                onTestSuiteStoryScenarioExecutionCompleted: function (event) {
                    updateTestCaseForScenarioComplete(event.testSuiteStoryId, event.testSuiteStoryScenarioId, event.output_variables, event.messages);
                },
                onTestSuiteStoryScenarioExecutionStarted: function (event) {
                    console.log('started');
                },
                onTestSuiteStoryExecutionCompleted: function (event) {
                    $scope.hasTestResult = true;
                    updateTestCaseForTestCaseComplete(event.testSuiteStoryId);
                },
                onTestSuiteStoryScenarioExecutionError: function (event) {
                    $scope.isExecuting = false;
                    notifyScenarioExecutionError(event.testSuiteStoryId, event.testSuiteStoryScenarioId, event.errorMessage);
                },
                onTestSuiteExecutionLocationChanged: function (event) {
                    var currentNodes = $scope.nodeData.nodes.filter(function (node) {
                        return node.id === event.previousNodeId;
                    })

                    if (currentNodes.length === 1) {
                        var currentNode = currentNodes[0];
                        if (currentNode !== undefined) {
                            // find the link
                            var links = currentNode.links.filter(function (link) {
                                return link.id === event.newNodeId;
                            })

                            if (links.length > 0) {
                                for (var i = 0, c = links.length; i < c; i++) {
                                    var link = links[i];
                                    if (link.type == "decision_yes") {
                                        if (event.nodeResult === false) {
                                            break;
                                        }
                                    } else
                                        if (link.type == "decision_no") {
                                            if (event.nodeResult === false) {
                                                break;
                                            }
                                        }
                                    if (link !== undefined) {
                                        link.isCovered = true;

                                        var existingNodesWithId = $scope.nodeData.coveredLinks.filter(function (existingLink) {
                                            return existingLink.id === link.id && existingLink.fromId === link.fromId && link.type === existingLink.type;
                                        })
                                        if (existingNodesWithId.length === 0) {
                                            $scope.nodeData.coveredLinks.push(link);
                                            $scope.nodeData.totalCoverage = $scope.nodeData.coveredLinks.length / $scope.nodeData.numberOfLinks * 100;
                                        }

                                        if (currentNode.links != undefined && currentNode.links.length > 0) {
                                            currentNode.coveragePercentage = currentNode.numberOfCoveredLinks() / currentNode.links.length * 100.0
                                        }
                                        else {
                                            currentNode.coveragePercentage = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            $eventAggregatorService.subscribe($eventDefinitions.onTestSuiteExecutionStarted, testEventHandlers.onTestSuiteExecutionStarted);
            $eventAggregatorService.subscribe($eventDefinitions.onTestSuiteExecutionCompleted, testEventHandlers.onTestSuiteExecutionCompleted);
            $eventAggregatorService.subscribe($eventDefinitions.onTestSuiteStoryExecutionStarted, testEventHandlers.onTestSuiteStoryExecutionStarted);
            $eventAggregatorService.subscribe($eventDefinitions.onTestSuiteStoryScenarioExecutionCompleted, testEventHandlers.onTestSuiteStoryScenarioExecutionCompleted);
            $eventAggregatorService.subscribe($eventDefinitions.onTestSuiteStoryScenarioExecutionStarted, testEventHandlers.onTestSuiteStoryScenarioExecutionStarted);
            $eventAggregatorService.subscribe($eventDefinitions.onTestSuiteStoryExecutionCompleted, testEventHandlers.onTestSuiteStoryExecutionCompleted);
            $eventAggregatorService.subscribe($eventDefinitions.onTestSuiteStoryScenarioExecutionError, testEventHandlers.onTestSuiteStoryScenarioExecutionError);
            $eventAggregatorService.subscribe($eventDefinitions.onTestSuiteExecutionLocationChanged, testEventHandlers.onTestSuiteExecutionLocationChanged);
            
            function setTestSuiteStoryToExecuting(testCaseId) {
                $('#progressbar' + testCaseId).progressbar('value', 0).progressbar('color', 'bg-green');
                var testCase = $testDataManager.getTestCase($rootScope.document.data.testCases, testCaseId);
                testCase.$passFailClass = getNotRunClass();
                testCase.$executing = true;
                $scope.currentTestSuiteId = testCase.id;
                testCase.$dirty = true;
                testCase.$progressValue = 0;
                testCase.$completed = 0;
                testCase.$successes = 0;
                testCase.$failures = 0;
                $scope.$$phase || $scope.$apply();
            }

            function notifyScenarioExecutionError(testCaseId, scenarioId, message) {
                var testCase = $testDataManager.getTestCase($rootScope.document.data.testCases, testCaseId);
                var scenario = $testDataManager.getScenario(testCase.scenarios, scenarioId);

                scenario.$success = false;
                scenario.$passFailClass = getPassFailClass(scenario.$success);
                testCase.$failures += 1;
                $notificationService.notifyError('There was an error executing scenario: ' + scenario.name, message)
            }

            function hasScenariosWithNoTestData(testCase) {
                for (var i = 0, c = testCase.scenarios.length; i < c; i++) {
                    var scenario = testCase.scenarios[i];
                    if (scenario.output_values.length === 0 && scenario.output_messages.length === 0) {
                        return true;
                    }
                }

                return false;
            }

            function updateTestCaseForTestCaseComplete(testCaseId) {
                var testCase = $testDataManager.getTestCase($rootScope.document.data.testCases, testCaseId);

                var hasNoTestData = hasScenariosWithNoTestData(testCase);
                testCase.$passFailClass = testCase.scenarios.length === 0 || hasNoTestData === true ? getNoTestsClass() : getPassFailClass(testCase.$failures === 0);
                testCase.$executing = false;
                $scope.currentTestSuiteId = -1;
                $scope.$$phase || $scope.$apply();
            }

            function updateTestCaseForScenarioComplete(testCaseId, scenarioId, output_values, messages) {
                var testCase = $testDataManager.getTestCase($rootScope.document.data.testCases, testCaseId);
                testCase.$completed += 1;
                var scenario = $testDataManager.getScenario(testCase.scenarios, scenarioId);
                setScenarioTestOutput(scenario, output_values, messages);
                if (scenario.$success) {
                    testCase.$successes += 1;
                } else {
                    testCase.$failures += 1;
                    $('#progressbar' + testCase.id).progressbar('color', 'bg-red');
                }
                scenario.$passFailClass = scenario.output_values.length === 0 && scenario.output_messages.length === 0 ? getNoTestsClass() : getPassFailClass(scenario.$success);
                testCase.$progressValue = Math.round(testCase.$completed / testCase.scenarios.length * 100);
                $('#progressbar' + testCase.id).progressbar('value', testCase.$progressValue);
                $scope.$$phase || $scope.$apply();
            }

            var stripInvalidChars = function (original) {
                return original.replace(/ /g, '').replace(/_/g, '').replace(/-/g, '').replace(/\(/g, '').replace(/\)/g, '');
            }

            function getTestVariableValueById(variableId) {
                var variables = $rootScope.document.data.testVariables.filter(function (item) {
                    return item.id == variableId
                })

                if (variables.length === 1) {
                    return variables[0].value
                }

                return null
            }

            var subtreeWasExecutedInScenario = function (scenario_id, subtree_name) {
                var scenarioExecutedSubtreesDictionary = $debugService.getTestSuiteExecutedSubtrees();
                return (scenario_id in scenarioExecutedSubtreesDictionary) && (scenarioExecutedSubtreesDictionary[scenario_id].indexOf(subtree_name) > -1)
            }

            function setScenarioTestOutput(scenario, output_values, messages) {
                var success = true;
                var messageSuccess = true;
                var subtreeSuccess = true;
                if (scenario.output_values) {
                    for (var i = 0; i < scenario.output_values.length; i++) {
                        scenario.output_values[i].$success = true;
                        scenario.output_values[i].$actualValue = '';
                        var variable = $testDataManager.findVariableByName(output_values, stripInvalidChars(scenario.output_values[i].name));
                        if (!variable) {
                            scenario.output_values[i].$actualValue = "Value was empty";
                            scenario.output_values[i].$success = false;
                            success = false;
                        } else {
                            if (scenario.output_values[i].$isEnum) {
                                scenario.output_values[i].$actualValue = variable.value.replace(/^Globals./, '').replace(/\./g, '::').replace(/::([^::]+)$/, '.$1');
                                var enumDisplayValue = '';
                                var enumDisplayValues = scenario.output_values[i].$enums.filter(function (item) {
                                    return item.name === scenario.output_values[i].$actualValue;
                                });
                                if (enumDisplayValues.length > 0) {
                                    enumDisplayValue = enumDisplayValues[0].value;
                                }
                                scenario.output_values[i].$actualDisplayValue = enumDisplayValue;
                            }
                            else {
                                scenario.output_values[i].$actualValue = variable.value;
                                scenario.output_values[i].$actualDisplayValue = variable.value;
                            }
                            if (scenario.output_values[i].type == 'bool') {
                                var result = scenario.output_values[i].value
                                if (scenario.output_values[i].expectationType == "test-variable") {
                                    result = getTestVariableValueById(scenario.output_values[i].id)
                                }
                                if (result != scenario.output_values[i].$actualValue) {
                                    scenario.output_values[i].$success = false;
                                    success = false;
                                }
                            }
                            else {
                                var result = scenario.output_values[i].value;
                                if (scenario.output_values[i].expectationType == "test-variable") {
                                    result = getTestVariableValueById(scenario.output_values[i].value)
                                }

                                if (result != scenario.output_values[i].$actualValue) {
                                    scenario.output_values[i].$success = false;
                                    success = false;
                                }
                            }
                        }
                    }
                }
                if (scenario.output_messages) {
                    for (var i = 0, c = scenario.output_messages.length; i < c; i++) {
                        // get the collection the message should be in
                        var collection = scenario.output_messages[i].messageCollection.toLowerCase();

                        var assertion = scenario.output_messages[i].assertion;

                        // get the expected result
                        var expectedResult = scenario.output_messages[i].message.value;
                        if (expectedResult === undefined) {
                            expectedResult = "";
                        }
                        if (assertion === "should contain") {
                            // check the collection for it
                            if (messages.some(function (message) {

                                    if (message.message.toLowerCase() === expectedResult.toLowerCase() && collection.indexOf(message.type.toLowerCase()) == 0) {
                                        return true;
                                    }

                                return false;
                            })) {
                                scenario.output_messages[i].$success = true;
                            }
                            else {
                                messageSuccess = false;
                                scenario.output_messages[i].$success = false;
                            }
                        }
                        else if (assertion === "should be empty") {
                            // check the collection has no messages
                            if (messages.some(function (message) {
                                    if (collection.indexOf(message.type.toLowerCase()) == 0) {
                                        return true;
                            }
                                        return false;
                            })) {
                                messageSuccess = false;
                                scenario.output_messages[i].$success = false;
                            }
                            else {
                                scenario.output_messages[i].$success = true;
                            }
                        }
                        else if (assertion === "should not contain") {
                            // check the collection for it
                            if (messages.some(function (message) {
                                    if (message.message.toLowerCase() === expectedResult.toLowerCase() && collection.indexOf(message.type.toLowerCase()) == 0) {
                                        return true;
                            }
                                        return false;
                            })) {
                                messageSuccess = false;
                                scenario.output_messages[i].$success = false;
                            }
                            else {
                                scenario.output_messages[i].$success = true;
                            }
                        }

                        // check for ANY error messages, if there are the tets should fail
                    }
                }
                if (scenario.subtrees) {
                    for (var i = 0; i < scenario.subtrees.length; i++) {
                        var assertion = scenario.subtrees[i].assertion;
                        var subtree_assertion_success
                        var subtree_name = $utils.string.sanitise(scenario.subtrees[i].name);
                        subtree_name = $utils.string.capitaliseFirstLetter(subtree_name);
                        if (assertion === 'should have been called') {
                            subtree_assertion_success = subtreeWasExecutedInScenario(scenario.id, subtree_name);
                            scenario.subtrees[i].$success = subtree_assertion_success;
                            scenario.subtrees[i].$assertionResult = subtree_assertion_success ? 'was called' : 'was not called'
                        }
                        else if (assertion === 'should not have been called') {
                            subtree_assertion_success = !subtreeWasExecutedInScenario(scenario.id, subtree_name);
                            scenario.subtrees[i].$success = subtree_assertion_success;
                            scenario.subtrees[i].$assertionResult = subtree_assertion_success ? 'was not called' : 'was called'
                        }
                        if (!subtree_assertion_success) {
                            subtreeSuccess = false;
                        }
                    }
                }
                scenario.$success = success && messageSuccess && subtreeSuccess;
                $scope.$$phase || $scope.$apply();
            }

            function getNotRunClass() {
                return {
                    'fg-white': false,
                    'bg-green': false,
                    'bg-red': false
                };
            }

            function get100CoverageClass() {
                return {
                    'fg-white': true,
                    'bg-green': true,
                    'bg-red': false
                };
            }

            function getSub100CoverageClass() {
                return {
                    'fg-white': true,
                    'bg-orange': true,
                    'bg-red': false
                };
            }

            function getNoTestsClass() {
                return {
                    'fg-white': true,
                    'bg-orange': true,
                    'bg-red': false
                };
            }

            function getPassFailClass(success) {
                return {
                    'fg-white': true,
                    'bg-green': success === true,
                    'bg-red': success === false
                };
            }

            function setupVariables() {
                $scope.inputVariables = $testDataManager.getInputVariables();
                $scope.outputVariables = $testDataManager.getOutputVariables();
                $scope.outputmessages = $testDataManager.getOutputMessages();
                $scope.subtrees = $testDataManager.getSubtrees();
                $scope.availableEnumerations = $testDataManager.getAvailableEnumerations();
                if ($rootScope.document.data.testCases) {
                    for (var i = 0; i < $rootScope.document.data.testCases.length; i++) {
                        $testDataManager.setInputValuesForTestCase($rootScope.document.data.testCases[i]);
                        $testDataManager.setVariablesForTestCaseScenarios($rootScope.document.data.testCases[i].id);
                    }
                }
                $testDataManager.setTestVariables($rootScope.document.data.testVariables);
            }

            function refreshAccordions() {
                $scope.$$postDigest(function () {
                    $(".accordion").each(
                        function (index, element) {
                            $(element).accordion();
                        }
                    )
                    activateProgressBars();
                });
            }

            function refreshAccordionsAndOpenLast() {
                $scope.$$postDigest(function () {
                    $(".accordion").each(
                        function (index, element) {
                            $(element).accordion();
                        }
                    )

                    $('.accordion').last().find(".heading").removeClass("collapsed");
                    $('.accordion').last().find(".content").css("display", "block");
                    activateProgressBars();
                });
            }

            function activateProgressBars() {
                $(".progress-bar").each(function (index, element) {
                    $(element).progressbar();
                });
            }

            function closeAllAccordions() {
                $(".accordion-frame").each(
                    function (index, element) {
                        var contents = $(element).children('.content');
                        $(contents).slideUp();
                        var headings = $(contents).parent().children('.heading')
                        $(headings).addClass("collapsed");
                    });
            }

            function initTests() {
                refreshAccordions();
                activateProgressBars();
                $testDataManager.refreshVariables();
                setupVariables();
                if ($rootScope.document.data.testCases) {
                    //for (var i = 0; i < $rootScope.document.data.testCases.length; i++) {
                    //    var testCase = $rootScope.document.data.testCases[i];
                    //    if (!testCase.id) {
                    //        testCase.id = i + 1;
                    //    }
                    //    if (testCase.scenarios) {
                    //        for (var j = 0; j < testCase.scenarios.length; j++) {
                    //            if (!testCase.scenarios[j].id) {
                    //                testCase.scenarios[j].id = j + 1;
                    //            }
                    //        }
                    //    }
                    //    else {
                    //        testCase.scenarios = [];
                    //    }
                    //}

                    // setup the coverage information
                    $scope.nodeData.nodes = $rootScope.document.data.tree.nodes.map(function (node) {
                        var links = $rootScope.document.data.tree.links.filter(function (link) {
                            return link.fromNodeId == node.id;
                        })

                        var namedLinks = links.map(function (link) {
                            var nodeLinkName = $rootScope.document.data.tree.nodes.filter(function (node) {
                                return node.id == link.toNodeId;
                            })

                            var linkedNodeName = nodeLinkName[0] === undefined ? "" : nodeLinkName[0].name;
                            return {
                                id: link.toNodeId,
                                fromId: link.fromNodeId,
                                type: link.type,
                                name: linkedNodeName,
                                isCovered: false
                            }
                        })

                        return {
                            id: node.id,
                            name: node.name,
                            type: node.type,
                            links: namedLinks != undefined ? namedLinks : [],
                            numberOfCoveredLinks: function () {
                                return this.links.filter(function (link) {
                                    return link.isCovered === true;
                                }).length
                            },
                            coveragePercentage: 0.0
                        }
                    }).filter(function (node) {
                        return node.type !== "Start" && node.type !== "End";
                    });

                    $scope.nodeData.numberOfLinks = $rootScope.document.data.tree.links.length - 1; // subtract 1 for start
                    $scope.nodeData.coveredLinks = [];
                    $scope.nodeData.totalCoverage = 0;
                }
            }

            var eventHandlers = {
                'onDocumentLoaded': function () {
                    initTests();
                }
            }
            if ($rootScope.document) {
                $activityManager.startActivityWithKey('loadingTestData');

                $timeout(function () {
                    $scope.testCases = $rootScope.document.data.testCases;
                    initTests();
                    $activityManager.stopActivityWithKey('loadingTestData');
                }, 400)
            }

            $eventAggregatorService.subscribe($eventDefinitions.onDocumentLoaded, eventHandlers.onDocumentLoaded);

            $scope.$on('$destroy', function () {
                $eventAggregatorService.unsubscribe($eventDefinitions.onDocumentLoaded, eventHandlers.onDocumentLoaded);
                $eventAggregatorService.unsubscribe($eventDefinitions.onTestSuiteExecutionStarted, testEventHandlers.onTestSuiteExecutionStarted);
                $eventAggregatorService.unsubscribe($eventDefinitions.onTestSuiteExecutionCompleted, testEventHandlers.onTestSuiteExecutionCompleted);
                $eventAggregatorService.unsubscribe($eventDefinitions.onTestSuiteStoryExecutionStarted, testEventHandlers.onTestSuiteStoryExecutionStarted);
                $eventAggregatorService.unsubscribe($eventDefinitions.onTestSuiteStoryScenarioExecutionCompleted, testEventHandlers.onTestSuiteStoryScenarioExecutionCompleted);
                $eventAggregatorService.unsubscribe($eventDefinitions.onTestSuiteStoryScenarioExecutionStarted, testEventHandlers.onTestSuiteStoryScenarioExecutionStarted);
                $eventAggregatorService.unsubscribe($eventDefinitions.onTestSuiteStoryExecutionCompleted, testEventHandlers.onTestSuiteStoryExecutionCompleted);
                $eventAggregatorService.unsubscribe($eventDefinitions.onTestSuiteStoryScenarioExecutionError, testEventHandlers.onTestSuiteStoryScenarioExecutionError);
                $eventAggregatorService.unsubscribe($eventDefinitions.onTestSuiteExecutionLocationChanged, testEventHandlers.onTestSuiteExecutionLocationChanged);
            })
        }
]);