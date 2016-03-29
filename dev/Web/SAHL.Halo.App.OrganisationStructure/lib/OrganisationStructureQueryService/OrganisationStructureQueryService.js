'use strict';
angular.module('sahl.halo.app.organisationStructure.organisationStructureQueryService', [])
.service('$organisationStructureQueryServiceService', ['$rootScope','$http',
function ($rootScope, $http) {
    
    var operations = {
        start: function () {

            $rootScope.baseCompany = { "key": "1", "name": "South African Home Loans (Pty) Ltd", "title": "Company" };

            $rootScope.organisationStructureModel = go.Model.fromJson({
                "class": "go.TreeModel",
                "nodeDataArray": [
                    $rootScope.baseCompany
                ]
            });

            $rootScope.two = 2;

            return $http.get('http://localhost/QueryService/api/Fake');
            //.then(onDataFetchComplete);
        }
    };

    function initialiseBaseTree() {
        return go.Model.fromJson({
            "class": "go.TreeModel",
            "nodeDataArray": []
        });
    }

    function initTreeWithArray(nodeData) {
        return {
            "class": "go.TreeModel",
            "nodeDataArray": nodeData
        };
    }

    return {
        start: operations.start,
        initialiseBaseTree: initialiseBaseTree,
        initTreeWithArray: initTreeWithArray
    };
}]);
