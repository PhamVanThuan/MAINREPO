'use strict';

angular.module('sahl.tools.app.services.testDataManager', [])

.factory('$testDataManager', ['$rootScope', '$documentManager', '$eventAggregatorService', '$eventDefinitions', '$enumerationDataManager', '$queryGlobalsVersionService', 'messageDataManager',
    function ($rootScope, $documentManager, $eventAggregatorService, $eventDefinitions, enumerationDataManager, $queryGlobalsVersionService, messageDataManager) {
        var inputVariables = [];
        var outputVariables = [];
        var outputMessages = [{ name: "custom message", value: "" }];
        var subtrees = [];
        var availableEnumerations = [];
        var messages = [];
        var messageCollections = [''];
        var testCases = [];
        var eventHandlers = {
            'onDocumentLoaded': function () {
                testCases = $rootScope.document.data.testCases || [];
                refreshVariables();
            }
        };
        $eventAggregatorService.subscribe($eventDefinitions.onDocumentLoaded, eventHandlers.onDocumentLoaded);

        function refreshVariables() {
            inputVariables = [];
            outputVariables = [];
            subtrees = [];
            var variables = $rootScope.document.data.tree.variables;
            variables.map(function (item) {
                //decorateVariable(item);
                if (item.usage === 'input') {
                    inputVariables.push(item);
                } else if (item.usage === 'output') {
                    outputVariables.push(item);
                }
            });

            subtrees = $rootScope.document.data.tree.nodes.filter(function (node) {
                return node.type == "SubTree";
            }).map(function (item) {
                return {
                    id: item.id,
                    name: item.subtreeName,
                    assertion: ""
                }
            })
        }

        function getNewTestCaseId() {
            var highestId = 0;
            testCases.map(function (testCase) {
                if (Number(testCase.id) > Number(highestId)) {
                    highestId = testCase.id;
                }
            });
            return Number(highestId) + 1;
        }

        function getNewScenarioId() {
            var highestId = 0;
            testCases.map(function (testCase) {
                testCase.scenarios.map(function (scenario) {
                    if (Number(scenario.id) > Number(highestId)) {
                        highestId = scenario.id;
                    }
                });
            });
            return Number(highestId) + 1;
        }

        function getTestSuiteJsonData(testCases) {
            var testCasesJson = [];
            for (var i = 0; i < testCases.length; i++) {
                testCasesJson.push(getTestCaseJsonForTest(testCases[i]));
            }
            return {
                "name": $rootScope.document.data.name,
                "version": $rootScope.document.data.version,
                'globalsVersions': $queryGlobalsVersionService.QueryGlobalsVersion(),
                "testCases": testCasesJson
            };
        }

        function getTestCaseJsonForTest(testCase) {
            var testCaseJson = {
                name: testCase.name,
                id: testCase.id,
                scenarios: [],
                execute: 'all'
            };

            for (var i = 0; i < testCase.scenarios.length; i++) {
                var scenario = testCase.scenarios[i];
                var scInputs = getScenarioInputJsonForTest(scenario, testCase);
                testCaseJson.scenarios.push({
                    name: scenario.name,
                    id: scenario.id,
                    input_values: scInputs
                })
            };
            return testCaseJson;
        }

        function getScenarioInputJsonForTest(scenario, testCase) {
            var inputs = [];
            testCase.input_values.map(function (tcInput) {
                var scInput = findVariableById(scenario.input_values, tcInput.id);
                if (scInput) {
                    inputs.push(getInputJsonForTest(scInput));
                } else {
                    inputs.push(getInputJsonForTest(tcInput));
                }
            });
            return inputs;
        }

        function getInputJsonForTest(input) {
            return {
                id: input.id,
                name: input.name,
                value: input.value
            }
        }

        function findVariableById(variables, id) {
            if (variables) {
                for (var i = 0; i < variables.length; i++) {
                    if (Number(variables[i].id) === Number(id)) {
                        return variables[i];
                    }
                }
            }
        }

        function findMessageByMessageName(messages, messageName) {
            if (messages) {
                for (var i = 0; i < messages.length; i++) {
                    if (messages[i].message.name === messageName) {
                        return messages[i];
                    }
                }
            }
        }

        function findSubtreeById(subtrees, id) {
            if (subtrees) {
                for (var i = 0, c = subtrees.length; i < c; i++) {
                    if (Number(subtrees[i].id) === Number(id)) {
                        return subtrees[i];
                    }
                }
            }
        }

        function findSubtreeByName(subtrees, name) {
            if (subtrees) {
                for (var i = 0, c = subtrees.length; i < c; i++) {
                    if (subtrees[i].name === name) {
                        return subtrees[i];
                    }
                }
            }
        }

        function getScenario(scenarios, scenarioId) {
            return $.grep(scenarios, function (item) {
                return item.id == scenarioId;
            })[0];
        }

        function getTestCase(testCases, testCaseId) {
            return $.grep(testCases, function (item) {
                return item.id === testCaseId;
            })[0];
        }

        function flattenMessageTypes(messages, flattenedMessages) {
            if (messages) {
                for (var i = 0, c = messages.groups.length; i < c; i++) {
                    if (messages.groups[i]["messages"]) {
                        for (var ii = 0, cc = messages.groups[i].messages.length; ii < cc; ii++) {
                            var messageToUse = messages.groups[i].messages[ii];
                            flattenedMessages.push({
                                name: messageToUse.$getFullName(),
                                value: messageToUse.value,
                            });
                        }
                    }
                    if (messages.groups[i]["groups"]) {
                        flattenMessageTypes(messages.groups[i], flattenedMessages);
                    }
                }
            }
        }

        function flattenEnumerationTypes(enumerations, flattenedEnumerations) {
            if (enumerations) {
                for (var i = 0, c = enumerations.groups.length; i < c; i++) {
                    if (enumerations.groups[i]["enumerations"]) {
                        for (var ii = 0, cc = enumerations.groups[i].enumerations.length; ii < cc; ii++) {
                            var messageToUse = enumerations.groups[i].enumerations[ii];
                            flattenedEnumerations.push({
                                name: messageToUse.$getFullName(),
                                value: messageToUse.value,
                            });
                        }
                    }
                    if (enumerations.groups[i]["groups"]) {
                        flattenEnumerationTypes(enumerations.groups[i], flattenedEnumerations);
                    }
                }
            }
        }

        function isInputAnEnum(input) {
            var realTypes = ['bool', 'int', 'float', 'string', 'double'];
            return realTypes.indexOf(input.type.toLowerCase()) === -1;
        }

        var load = function () {
            messageDataManager.GetLatestMessageSetQueryAsync().then(function (data) {
                var originalData = data.data.ReturnData.Results.$values[0].Data;
                var messageData = messageDataManager.$extend(angular.fromJson(originalData));
                flattenMessageTypes(messageData, outputMessages);
            });

            enumerationDataManager.GetLatestEnumerationSetQueryAsync().then(function (data) {
                var originalData = data.data.ReturnData.Results.$values[0].Data;
                var enumerationData = enumerationDataManager.$extend(angular.fromJson(originalData));
                flattenEnumerationTypes(enumerationData, availableEnumerations);
            });
        }

        load();

        var stripInvalidChars = function (original) {
            return original.replace(/ /g, '').replace(/_/g, '').replace(/g-/, '').replace(/\(/, '').replace(/\)/, '');
        }

        var decorateVariable = function (variable) {
            if (variable) {
                // if its an enumtype add the available types
                if (isInputAnEnum(variable)) {
                    variable.$isEnum = true;
                    var enums = availableEnumerations.filter(function (item) {
                        var actualName = item.name.replace(/\./g, '::');
                        if (actualName.toLowerCase() == variable.type.toLowerCase()) {
                            return true;
                        }
                    });
                    variable.$enums = [];
                    for (var m = 0, n = enums.length; m < n; m++) {
                        var enumpointers = enums[m].name.split(".");

                        var nameSpace = "Enumerations::";
                        for (var p = 0, q = enumpointers.length; p < q; p++) {
                            nameSpace += enumpointers[p][0].toLowerCase() + enumpointers[p].substring(1);
                            if (p < (q - 1)) {
                                nameSpace += "::";
                            }
                        }
                        if (m === (n - 1)) {
                            nameSpace += ".";
                            for (var k = 0, j = enums[m].value.length; k < j; k++) {
                                variable.$enums.push({ name: nameSpace + stripInvalidChars(enums[m].value[k]), value: enums[m].value[k] });
                            }
                        }
                    }
                }
                else {
                    variable.$isEnum = false;
                }

                if (variable.type == 'string') {
                    variable.$inputType = "text";
                    variable.inputType = "text";
                }
                else
                    if (variable.type == 'bool') {
                        variable.$inputType = "bool";
                        variable.inputType = "bool";
                    }
                    else {
                        variable.$inputType = "number";
                        variable.inputType = "number";
                    }
            }
        }

        var getLatestLocalTestVariableID = function () {
            var varArray = $rootScope.document.data.testVariables;
            if ((!varArray) || (varArray.length == 0))
                return "1";
            var largest = Math.max.apply(Math, varArray.map(function (varEl) { return parseInt(varEl.id); }));
            return (largest + 1).toString();
        }

        return {
            getInputVariables: function () {
                return inputVariables;
            },
            getOutputVariables: function () {
                return outputVariables;
            },
            getOutputMessages: function () {
                return outputMessages;
            },
            getSubtrees: function () {
                return subtrees;
            },
            findVariableById: findVariableById,
            findMessageByMessageName: findMessageByMessageName,
            findSubtreeById: findSubtreeById,
            findSubtreeByName: findSubtreeByName,
            getTestCase: getTestCase,
            getScenario: getScenario,
            findVariableByName: function (varSet, name) {
                return $.grep(varSet, function (item) {
                    return item.name === name;
                })[0];
            },
            addNewTestCase: function (newName) {
                var newId = getNewTestCaseId();

                var testCaseInputs = [];
                for (var i = 0, c = inputVariables.length; i < c; i++) {
                    var variable = {
                        "id": inputVariables[i].id,
                        "value": '',
                        "name": inputVariables[i].name,
                        "type": inputVariables[i].type
                    };
                    decorateVariable(variable);
                    testCaseInputs.push(variable);
                }

                testCases.push({
                    "name": newName,
                    "input_values": testCaseInputs,
                    "id": newId,
                    "scenarios": []
                });
            },
            addScenario: function (newName, testCaseId) {
                var testCase = getTestCase(testCases, testCaseId);
                if (!testCase.scenarios) {
                    testCase.scenarios = [];
                }
                var newId = getNewScenarioId();
                testCase.scenarios.push({
                    "name": newName,
                    "id": newId,
                    "output_messages": [{
                        message: "",
                        assertion: "should be empty",
                        messageCollection: "Error Messages",
                        "$hasEditableMessage": false
                    }],
                    "output_values": [],
                    "subtrees": []
                });
            },
            addOutputVariable: function (output, scenario) {
                if (!scenario.output_values) {
                    scenario.output_values = [];
                }

                var variable = {
                    "name": output.name,
                    "id": output.id,
                    "value": output.value,
                    "type": output.type,
                    "assertion": output.assertion,
                    "expectationType": output.expectationType
                };

                decorateVariable(variable);

                scenario.output_values.push(variable);
            },
            addOutputMessage: function (message, scenario) {
                if (!scenario.output_messages) {
                    scenario.output_messages = [];
                }
                scenario.output_messages.push({
                    "message": { name: "custom message", value: message },
                    "assertion": "",
                    "messageCollection": "",
                    "$update": function () {
                        if (this.assertion == "should be empty") {
                            this.message = "";
                            this.$hasEditableMessage = false;
                        }
                        else {
                            this.$hasEditableMessage = true;
                        }
                    },
                    "$hasEditableMessage": true
                });
            },
            addInputVariable: function (input, scenario) {
                if (!scenario.input_values) {
                    scenario.input_values = [];
                }
                var variable = {
                    "id": input.id,
                    "value": '',
                    "name": input.name,
                    "type": input.type
                };

                decorateVariable(variable);

                scenario.input_values.push(variable);
            },
            addSubtreeExpectation: function (subtreeData, scenario) {
                if (!scenario.subtrees) {
                    scenario.subtrees = [];
                }

                scenario.subtrees.push({
                    id: subtreeData.id,
                    name: subtreeData.name,
                    assertion: ""
                })
            },
            addTestVariable: function (testVariableData, testVariables) {
                var variable = {
                    "name": testVariableData.name,
                    "id": testVariableData.id,
                    "value": testVariableData.value,
                    "type": testVariableData.type
                };

                decorateVariable(variable);
                $rootScope.document.data.testVariables.push(variable)
            },
            getTestSuiteDataForAll: function () {
                return getTestSuiteJsonData(testCases);
            },
            getTestSuiteDataForSuite: function (suiteId) {
                var testCase = getTestCase(testCases, suiteId);
                return getTestSuiteJsonData([testCase]);
            },
            getUnusedVariablesInSet: function (variableSet, usedVariables) {
                var unusedVars = [];
                if (!usedVariables || usedVariables.length === 0) {
                    for (var i = 0, c = variableSet.length; i < c; i++) {

                        variable = variableSet[i]
                        newVariable = {
                            "name": variable.name,
                            "id": variable.id,
                            "value": "",
                            "type": variable.type,
                            "assertion": "",
                            "expectationType": variable.expectationType
                        }
                        decorateVariable(newVariable);
                        unusedVars.push(newVariable);
                    }
                } else {
                    for (var i = 0; i < variableSet.length; i++) {
                        var variable = findVariableById(usedVariables, variableSet[i].id);
                        if (!variable) {
                            variable = variableSet[i];
                            var newVariable = {
                                "name": variable.name,
                                "id": variable.id,
                                "value": "",
                                "type": variable.type,
                                "assertion": "",
                                "expectationType": variable.expectationType
                            }
                            decorateVariable(newVariable);
                            unusedVars.push(newVariable);
                        }
                    }
                }
                return unusedVars;
            },
            getUnusedMessagesInSet: function (messageSet, usedMessages) {
                var unusedMessages = [];
                if (!unusedMessages || unusedMessages.length === 0) {
                    return messageSet;
                }
                for (var i = 0; i < messageSet.length; i++) {
                    var message = findMessageByMessageName(usedMessages, messageSet[i].message.Name);
                    if (!message) {
                        unusedMessages.push(messageSet[i]);
                    }
                }
                return unusedMessages;
            },
            getUnusedSubtreesInSet: function (subtreeSet, usedSubtrees) {
                var unusedSubtrees = [];
                if (!usedSubtrees || usedSubtrees.length === 0) {
                    return subtreeSet;
                }
                for (var i = 0; i < subtreeSet.length; i++) {
                    var variable = findSubtreeById(usedSubtrees, subtreeSet[i].id);
                    if (!variable) {
                        unusedSubtrees.push(subtreeSet[i]);
                    }
                }
                return unusedSubtrees;
            },
            setInputValuesForTestCase: function (testCase) {
                for (var i = 0; i < inputVariables.length; i++) {
                    var variable = findVariableById(testCase.input_values, inputVariables[i].id);
                    if (variable) {
                        variable.name = inputVariables[i].name;
                        variable.type = inputVariables[i].type;
                    } else {
                        variable = {
                            id: inputVariables[i].id,
                            value: '',
                            name: inputVariables[i].name,
                            type: inputVariables[i].type
                        }

                        testCase.input_values.push(variable);
                    }

                    decorateVariable(variable);
                }

                // strip any variables that no longer exist
                var len = testCase.input_values.length;
                while (len--) {
                    var testCaseVar = testCase.input_values[len];
                    var matchedVariable = null;
                    var matchedVariables = inputVariables.filter(function (variable) {
                        return variable.id === testCaseVar.id && variable.type === testCaseVar.type && variable.usage === 'input';
                    })
                    if (matchedVariables.length === 0) {
                        testCase.input_values.splice(len, 1);
                    }
                }
            },
            setTestVariables: function (testVariables) {
                angular.forEach(testVariables, function (testVariable) {
                    decorateVariable(testVariable);
                });
            },
            setVariablesForTestCaseScenarios: function (testCaseId) {
                var testCase = getTestCase(testCases, testCaseId);
                angular.forEach(testCase.scenarios, function (scenario) {
                    if (!scenario.input_values) {
                        scenario.input_values = [];
                    } else {
                        for (var i = 0; i < scenario.input_values.length; i++) {
                            var variable = findVariableById(inputVariables, scenario.input_values[i].id);
                            if (variable) {
                                scenario.input_values[i].name = variable.name;
                                scenario.input_values[i].id = variable.id;
                                scenario.input_values[i].type = variable.type;

                                decorateVariable(scenario.input_values[i]);
                            }
                        }
                    }

                    if (!scenario.output_values) {
                        scenario.output_values = [];
                    }
                    else {

                        // strip any variables that no longer exist
                        var len = scenario.output_values.length;
                        while (len--) {
                            var testCaseVar = scenario.output_values[len];
                            var matchedVariable = null;
                            var matchedVariables = outputVariables.filter(function (variable) {
                                return variable.id === testCaseVar.id && variable.type === testCaseVar.type && variable.usage === 'output';
                            })
                            if (matchedVariables.length === 0) {
                                scenario.output_values.splice(len, 1);
                            }
                        }

                        for (var i = 0; i < scenario.output_values.length; i++) {
                            var variable = findVariableById(outputVariables, scenario.output_values[i].id);
                            if (variable) {
                                scenario.output_values[i].name = variable.name;
                                scenario.output_values[i].id = variable.id;
                                scenario.output_values[i].type = variable.type;

                                decorateVariable(scenario.output_values[i]);
                            }
                        }
                    }
                });
            },
            getAvailableEnumerations: function () {
                return availableEnumerations;
            },
            refreshVariables: refreshVariables,
                getEmptyTestVariable: function() {
                    var variable = {
                        "name": '',
                        "id": getLatestLocalTestVariableID(),
                        "value": undefined,
                        "type": 'string'
                    };

                    decorateVariable(variable);
                    return variable;
                }
        }
    }
]);