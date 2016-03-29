'use strict';

angular.module('sahl.tools.app.services.documentVersionProviders.version_0_4', [
    'sahl.tools.app.services.documentVersionManager'
])
.service('$documentVersionProviderVersion_0_4', ['$documentVersionManager', '$rootScope' , function ($documentVersionManager, $rootScope) {
    var provider = {
        emptyDocument: function () {
            return {
                "jsonversion": "0.4",
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
                "testCases": [],
                "testVariables": []
            };
        },
        loadDocument: function (document) {
            if (document.testVariables === undefined) {
                document.testVariables = []
            }
            return document;
        },
        validate: function (document) {
            if (document.testCases !== undefined) {
                angular.forEach(document.testCases, function (testCase) {
                    angular.forEach(testCase.scenarios, function (scenario) {
                        angular.forEach(scenario.output_values, function (outputValue) {
                            if (outputValue.expectationType === undefined) {
                                outputValue.expectationType = "inline";
                            }
                        });
                    })
                })
            }

            angular.forEach(document.tree.variables, function (variable) {
                if (variable.usage == "output") {
                    if($rootScope.globalData.isVariableAnEnum(variable)){
                        $rootScope.globalData.getEnumerationValuesForVariable(variable);
                    }
                }
            })

            return document;
        }
    }
    $documentVersionManager.registerProvider("0.4", provider);
    return {
        start: function () { }
    };
}]);