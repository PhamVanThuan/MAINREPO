'use strict';

/* Services */

angular.module('sahl.tools.app.services.codemirrorVariablesService', [
    'sahl.tools.app.serviceConfig',
    'SAHL.Services.Interfaces.DecisionTreeDesign.queries',
    'SAHL.Services.Interfaces.DecisionTreeDesign.commands'
])
.factory('$codemirrorVariablesService', ['$q', '$eventAggregatorService', '$eventDefinitions','$rootScope','$utils',
function ($q, $eventAggregatorService, $eventDefinitions, $rootScope, $utils) {
    var operations = {
        loadDocumentData: function () {
            var treeVariables = [];
            operations.getVariables(treeVariables, $rootScope.document.data.tree.variables);
            operations.getSubTreeOutputs(treeVariables,$rootScope.document.data.tree.nodes);
            return treeVariables;
        },
        getVariables: function (treeVariables, documentVariables) {
            for (var i = 0, c = documentVariables.length; i < c; i++) {
                var inputOutputObj = documentVariables[i];
                treeVariables.push("Variables::" + inputOutputObj.usage + "s." + $utils.string.sanitise(inputOutputObj.name));
            }
        },
        getSubTreeOutputs: function (treeVariables, nodes) {
            for (var i = 0, c = nodes.length; i < c; i++) {
                if (nodes[i].type === "SubTree") {
                    var subTree = nodes[i];
                    for (var ii = 0, cc = subTree.subtreeVariables.length; ii < cc; ii++) {
                        var inputOutputObj = subTree.subtreeVariables[ii];
                        if (inputOutputObj.usage === "output") {
                            treeVariables.push("Variables::" + $utils.string.smallFirstLetter(subTree.name, true) + "::" + inputOutputObj.usage + "s." + $utils.string.sanitise(inputOutputObj.name));
                        }
                    }
                }
            }
        },
    };
    return {
        loadData: operations.loadDocumentData,
        start: function () {}
    }
}])