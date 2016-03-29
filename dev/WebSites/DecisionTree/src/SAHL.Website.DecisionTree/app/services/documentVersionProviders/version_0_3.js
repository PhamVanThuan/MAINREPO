'use strict';

angular.module('sahl.tools.app.services.documentVersionProviders.version_0_3', [
    'sahl.tools.app.services.documentVersionManager'
])
.service('$documentVersionProviderVersion_0_3', ['$documentVersionManager', function ($documentVersionManager) {
    var provider = {
        emptyDocument: function () {
            return {
                "jsonversion": "0.3",
                "version": 1,
                "name": "New Decision Tree",
                "description": "New Decision Tree",
                "tree": {
                    "variables": [],
                    "nodes": [{
                        "id": 1,
                        "name": "Start",
                        "type": "Start",
                        "code": ""
                    }],
                    "links": [],
                },
                "layout": {
                    "nodes": [{
                        "id": "1",
                        "loc": "0 0"
                    }],
                    "links": [],
                },
                "testCases": []
            };
        },
        loadDocument: function (document) {
            angular.forEach(document.tree.nodes, function (node) {
                if (node.required_variables) {
                    delete node.required_variables;
                }
                if (node.output_variables) {
                    delete node.output_variables;
                }
                if (node.code === undefined) {
                    code = "";
                }
            });
            return document;
        },
        validate: function (document) {
            angular.forEach(document.tree.nodes, function (node) {
                if (node.code === undefined) {
                    node.code = "";
                }
            });

            angular.forEach(document.tree.variables, function (variable) {
                if (variable.value !== undefined) {
                    delete variable.value;
                }
            })

            if (document.testCases !== undefined) {
                angular.forEach(document.testCases, function (testCase) {
                    angular.forEach(testCase.scenarios, function (scenario) {
                        if (scenario.subtrees === undefined) {
                            scenario.subtrees = [];
                        }
                        angular.forEach(scenario.output_messages, function (outputMessage) {
                            if (outputMessage.messageCollection === "Information Messages") {
                                outputMessage.messageCollection = "Info Messages";
                            }
                        });

                        // check if there is an existing error message empty collection check
                        if (scenario.output_messages === undefined) {
                            scenario.output_messages = [{
                                message: "",
                                assertion: "should be empty",
                                messageCollection: "Error Messages",
                                "$hasEditableMessage": false
                            }]
                        }
                        else {
                            var messages = scenario.output_messages.filter(function (message) {
                                return message.assertion === "should be empty" && message.messageCollection === "Error Messages"
                            })

                            if (messages.length === 0) {
                                scenario.output_messages.splice(0, {
                                    message: "",
                                    assertion: "should be empty",
                                    messageCollection: "Error Messages",
                                    "$hasEditableMessage": false
                                })
                            }
                        }
                    });
                });
            }
            return document;
        }
    }
    $documentVersionManager.registerProvider("0.3", provider);
    return {
        start: function () { }
    };
}]);