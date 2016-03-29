'use strict';

angular.module('sahl.tools.app.services.documentVersionProviders.version_0_2', [
    'sahl.tools.app.services.documentVersionManager'
])
.service('$documentVersionProviderVersion_0_2', ['$documentVersionManager', function ($documentVersionManager) {
    var provider = {
        emptyDocument: function () {
            return {
                "jsonversion": "0.2",
                "version": 1,
                "name": "New Decision Tree",
                "description": "New Decision Tree",
                "tree": {
                    "variables": [],
                    "nodes": [{
                        "id": 1,
                        "name": "Start",
                        "type": "Start",
                        "required_variables": [],
                        "output_variables": [],
                        "code": ""
                    }],
                    "links": [],
                },
                "layout": {
                    "nodes": [{
                        "id": 1,
                        "loc": "0 0"
                    }],
                    "links": [],
                },
                "testCases": []
            };
        },
        loadDocument: function (document) {
            document.testCases = new Array();
            return document;
        },
        validate: function (document) {

            return document;
        }
    }
    $documentVersionManager.registerProvider("0.2", provider);
    return {
        start: function () {}
    };
}]);