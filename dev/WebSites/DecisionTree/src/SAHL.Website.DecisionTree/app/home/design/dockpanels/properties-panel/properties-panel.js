'use strict';
angular.module('sahl.tools.app.home.design.dockpanels.properties-panel', [
    'sahl.tools.app.services',

])
.controller('PropertiesPanelCtrl', ['$rootScope', '$scope', '$enumerationDataManager', '$variableDataManager', '$documentManager', '$eventAggregatorService', '$eventDefinitions', '$q', '$utils',
        function PropertiesPanelController($rootScope, $scope, enumerationDataManager, variableDataManager, $documentManager, $eventAggregatorService, $eventDefinitions, $q, $utils) {
            var subtreeWatch = null;

            $scope.availableSubTrees = [];

            $scope.addOutput = function (ev) {
                ev.stopPropagation();
                if (isNodeSelected()) {
                    var newBlank = getBlankVariable();
                    newBlank.usage = "output";
                    newBlank.name = "newvar";
                    newBlank.id = $documentManager.getLatestLocalID();
                    $documentManager.addNewLocalVariable(newBlank);
                    $eventAggregatorService.publish($eventDefinitions.onDocumentVariableChanged, { usage: "output", oldName: newBlank.name, name: newBlank.name, action: "add" });
                }
            };

            $scope.addRequired = function (ev) {
                ev.stopPropagation();
                if (isNodeSelected()) {
                    var newBlank = getBlankVariable();
                    newBlank.usage = "input";
                    newBlank.name = "newvar";
                    newBlank.id = $documentManager.getLatestLocalID();
                    $documentManager.addNewLocalVariable(newBlank);
                    $eventAggregatorService.publish($eventDefinitions.onDocumentVariableChanged, { usage: "input", oldName: newBlank.name, name: newBlank.name, action: "add" });
                }
            };

            $scope.getInputVariablesForTree = function () {
                return $documentManager.getInputVariablesForTree();
            };

            $scope.getOutputVariablesForTree = function () {
                return $documentManager.getOutputVariablesForTree();
            };

            $scope.getAllVariablesForTree = function () {
                var inputs = $scope.getInputVariablesForTree();
                var outputs = $scope.getOutputVariablesForTree();
                return inputs.concat(outputs);
            };

            $scope.removeVariable = function (rVar) {
                $eventAggregatorService.publish($eventDefinitions.onDocumentVariableChanged, { usage: rVar.usage, oldName: rVar.name, name: rVar.name, action: "remove" });
                $documentManager.removeLocalVariable(rVar);
            };

            $scope.formatSubtreeName = function (name, version) {
                return name + ' (v.' + version + ')';
            }

            $scope.formatParentVariable = function (variable) {
                return variable.name + ' (' + variable.usage + ')';
            }

            function isNodeSelected() {
                return $rootScope.document && $rootScope.document.selectionManager.current.type === 'node';
            }

            function getBlankVariable() {
                return {
                    "id": "",
                    "type": "string",
                    "usage": "",
                    "name": ""
                };
            };
            $scope.existingVar = "";
            $scope.allInputs = $documentManager.getTreeRequiredVariables;

            var selectionManagerInScope = function () {
                return ($rootScope.document && $rootScope.document.selectionManager && $rootScope.document.selectionManager.current && $rootScope.document.selectionManager.current.data);
            }

            function setupSubTreeData() {
                getAvailableSubTrees();

                subtreeWatch = $scope.$watch('document.selectionManager.current.data.subtree', function () {
                    if (selectionManagerInScope() && $rootScope.document.selectionManager.current.data.subtree) {

                        bindSelectedSubTreeToAvailableSubtrees($rootScope.document.selectionManager.current.data.subtree);
                        bindSubTreeVariablesToParentVariables($rootScope.document.selectionManager.current.data.subtree);
                    }
                });
            }

            $scope.subTreeAccordionRequired = function () {
                var result = false;
                if (selectionManagerInScope()) {
                    result = ($rootScope.document.selectionManager.current.data.category == 'SubTree' || $rootScope.document.selectionManager.current.data.category == 'ClearMessages');
                }
                return result;
            }

            $scope.subTreeVariablesMappingRequired = function () {
                var result = false;
                if (selectionManagerInScope()) {
                    result = ($rootScope.document.selectionManager.current.data.category == 'SubTree');
                }
                return result;
            }

            function bindSelectedSubTreeToAvailableSubtrees(subtree) {
                var subtreeIndex = $scope.availableSubTrees.indexOf(subtree);
                if (subtreeIndex == -1) {
                    var found = false;
                    $scope.availableSubTrees.map(function (item, index) {
                        if (item.name === subtree.name && item.version === subtree.version) {
                            if (subtree.variables) {
                                $scope.availableSubTrees[index].variables = subtree.variables;
                            }
                            else {
                                subtree.variables = $scope.availableSubTrees[index].variables;
                            }
                            $rootScope.document.selectionManager.current.data.subtree = $scope.availableSubTrees[index];
                            found = true;
                        }
                    });
                    if (!found) {
                        $scope.availableSubTrees.push(subtree);
                    }
                }
            }

            function bindSubTreeVariablesToParentVariables(subtree) {
                angular.forEach(subtree.variables, function (item) {
                    var parentVariable;
                    if (item.parentVariable) {
                        angular.forEach($scope.getAllVariablesForTree(), function (parentVar) {
                            if (parentVar.name === item.parentVariable.name && parentVar.usage === item.parentVariable.usage) {
                                parentVariable = parentVar;
                            }
                        });

                    }
                    item.$parentVariable = parentVariable;
                })
            }

            function getAvailableSubTrees() {
                $documentManager.getAvailableSubTrees().then(function (data) {
                    $scope.availableSubTrees = [];
                    data.map(function (item) {
                        getSubtreeVariables(item.Name, item.ThisVersion).then(function (data) {
                            $scope.availableSubTrees.push(data);
                        });
                    });

                    $scope.$$phase || $scope.$apply();
                });
            }

            function getSubtreeVariables(name, version) {
                var deferred = $q.defer();
                $documentManager.getSubTreeVariables(name, version).then(function (varData) {
                    var subTreeToPush = {
                        name: name,
                        version: version,
                        variables: varData
                    };
                    deferred.resolve(subTreeToPush);
                });
                return deferred.promise;
            }

            function initPropertiesPanel() {

                setupSubTreeData();
                $(".accordion").accordion({
                    closeAny: false, //true or false. if true other frames (when current opened) will be closed
                    open: function (frame) { }, // when current frame opened this function will be fired
                    action: function (frame) { } // when any frame opened this function will be fired
                });
            }

            var eventHandlers = {
                onDocumentLoaded: function () {
                    initPropertiesPanel();
                }
            }

            // This could be a refresh on the screen, so the document might already be open.
            if ($rootScope.document) {
                initPropertiesPanel();
            }



            $eventAggregatorService.subscribe($eventDefinitions.onDocumentLoaded, eventHandlers.onDocumentLoaded);

            $scope.sanitise = $utils.string.sanitise;

            $scope.$on('$destroy', function () {
                if (subtreeWatch != null) {
                    subtreeWatch();
                    subtreeWatch = null;
                }
                $eventAggregatorService.unsubscribe($eventDefinitions.onDocumentLoaded, eventHandlers.onDocumentLoaded);
            })
        }
]).controller('InputsWatchController', ['$scope', '$eventAggregatorService', '$eventDefinitions',
function InputsWatchController($scope, $eventAggregatorService, $eventDefinitions) {
    var watcher = $scope.$watch('var.name', function (newValue, oldValue) {
        if (newValue != oldValue) {
            $eventAggregatorService.publish($eventDefinitions.onDocumentVariableChanged, { usage: "input", oldName: oldValue, name: newValue,action:"change" });
        }
    });
    $scope.$on('$destroy', function () {
        watcher();
    });
}]).controller('OutputsWatchController', ['$scope', '$eventAggregatorService', '$eventDefinitions',
function OutputsWatchController($scope, $eventAggregatorService, $eventDefinitions) {
    var watcher = $scope.$watch('variable.name', function (newValue, oldValue) {
        if (newValue != oldValue) {
            $eventAggregatorService.publish($eventDefinitions.onDocumentVariableChanged, { usage: "output", oldName: oldValue, name: newValue,action: "change"});
        }
    });
    $scope.$on('$destroy', function () {
        watcher();
    });
}]);