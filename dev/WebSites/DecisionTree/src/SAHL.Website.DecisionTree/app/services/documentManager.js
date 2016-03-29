'use strict';

/* Services */

angular.module('sahl.tools.app.services.documentManager', [
    'sahl.tools.app.serviceConfig',
    'SAHL.Services.Interfaces.DecisionTreeDesign.queries',
    'SAHL.Services.Interfaces.DecisionTreeDesign.commands'
])
.factory('$documentManager', ['$rootScope', '$queryManager', '$commandManager', '$decisionTreeDesignQueries', '$decisionTreeDesignCommands', '$state', '$q', '$activityManager', '$eventAggregatorService', '$eventDefinitions', '$modalDialogManager', '$documentVersionManager', '$designSurfaceManager', '$notificationService', 'filterFilter', '$sessionLockService',
function ($rootScope, $queryManager, $commandManager, $decisionTreeDesignQueries, $decisionTreeDesignCommands, $state, $q, $activityManager, $eventAggregatorService, $eventDefinitions, $modalDialogManager, $documentVersionManager, $designSurfaceManager, $notificationService, filterFilter, $sessionLockService) {
    var currentDocumentHasChangesWatcher = null;
    var currentDocumentNameChangeWatcher = null;
    var currentDocumentOutputsArrayChangesWatcher = null;
    var lastModifiedDate;

    function warnUserOfSessionTimeout(documentInfo) {
        if ($rootScope.document && !$rootScope.document.isReadOnly && $rootScope.document.treeVersionId == documentInfo.DocumentVersionId) {
            $modalDialogManager.dialogs.warnOfSessionTimeout.show().then(function () {
                operations.saveCurrentlyOpenDecisionTree($rootScope.document.treeVersionId, $rootScope.username);
            }, function () {
                
                if ($rootScope.document && $rootScope.document.hasChanges === true) {
                    operations.saveDocument().then(function () {
                        operations.closeDocument();
                        $state.go('home.start-page');
                    });
                } else {
                    operations.closeDocument();
                    $state.go('home.start-page');
                }
            });
        }
    }

    function parseSubtreeVariables(subtreeVars) {
        var parsedVars = [];
        angular.forEach(subtreeVars, function (item) {
            if (item.$parentVariable) {
                item.parentVariable = {
                    name: item.$parentVariable.name,
                    usage: item.$parentVariable.usage
                }
            }
            parsedVars.push(item);
        });
        return parsedVars;
    }

    var dataOperations = {
        getBlankMapObject: function () {
            return {
                "name": "",
                "description": "",
                "class": "go.GraphLinksModel",
                "linkFromPortIdProperty": "fromPort",
                "linkToPortIdProperty": "toPort",
                "nodeDataArray": [],
                "linkDataArray": [],
                "variableDataArray": [],
                "outputMessagesArray": []
            };
        },
        convertTreeDataToDataModel: function () {
            var treeData = $rootScope.document.data;
            var modelData = $rootScope.document.dataModel;

            modelData.name = treeData.name;
            modelData.description = treeData.description;
            var index;
            for (index = 0; index < treeData.layout.nodes.length; ++index) {
                if (treeData.layout.nodes[index].category === 'SubTree') {
                    dataOperations.addSubTreeNodeToModel(modelData, treeData.layout.nodes[index].id, treeData.tree.nodes[index].type, treeData.layout.nodes[index].loc, treeData.tree.nodes[index].name, treeData.tree.nodes[index].subtreeName, treeData.tree.nodes[index].subtreeVersion, treeData.tree.nodes[index].subtreeVariables);
                }
                else if (treeData.layout.nodes[index].category === 'ClearMessages') {
                    dataOperations.addClearMessagesNodeToModel(modelData, treeData.layout.nodes[index].id, treeData.tree.nodes[index].type, treeData.layout.nodes[index].loc, treeData.tree.nodes[index].name, treeData.tree.nodes[index].subtreeName, treeData.tree.nodes[index].subtreeVersion);
                }
                else {
                    dataOperations.addNodeToModel(modelData, treeData.layout.nodes[index].id, treeData.tree.nodes[index].type, treeData.layout.nodes[index].loc, treeData.tree.nodes[index].name);
                }
            }
            for (index = 0; index < treeData.layout.links.length; ++index) {
                // get the link from the id
                var links = treeData.tree.links.filter(function (element) {
                    return element.id === treeData.layout.links[index].linkid
                });
                var link = links[0];
                dataOperations.addLinkToModel(modelData, treeData.layout.links[index].from, treeData.layout.links[index].to, treeData.layout.links[index].fromPort, treeData.layout.links[index].toPort, treeData.layout.links[index].points, link.type);
            }

            for (index = 0; index < modelData.nodeDataArray.length; ++index) {
                dataOperations.addCodeInfoToModelNode(modelData.nodeDataArray[index], treeData.tree.nodes.filter(function (el) { return el.id == modelData.nodeDataArray[index].key; })); //key is id
            }

            modelData.description = treeData.description;
            modelData.name = treeData.name;
            modelData.variableDataArray = treeData.tree.variables;
        },
        convertDataModelToTreeData: function () {
            var treeData = $rootScope.document.data;
            var modelData = $rootScope.document.dataModel;

            treeData.tree.nodes.length = 0;
            treeData.tree.links.length = 0;
            treeData.layout.nodes.length = 0;
            treeData.layout.links.length = 0;

            var index;
            for (index = 0; index < modelData.nodeDataArray.length; ++index) {
                if (modelData.nodeDataArray[index].category === 'SubTree') {
                    var subtreeVariables = parseSubtreeVariables(modelData.nodeDataArray[index].subtree.variables);
                    dataOperations.addSubTreeNodeToTree(treeData, modelData.nodeDataArray[index].key, modelData.nodeDataArray[index].text, modelData.nodeDataArray[index].category, modelData.nodeDataArray[index].required_variables, modelData.nodeDataArray[index].output_variables, modelData.nodeDataArray[index].code, modelData.nodeDataArray[index].subtree.name, modelData.nodeDataArray[index].subtree.version, subtreeVariables);
                }
                else if (modelData.nodeDataArray[index].category === 'ClearMessages') {
                    dataOperations.addClearMessagesNodeToTree(treeData, modelData.nodeDataArray[index].key, modelData.nodeDataArray[index].text, modelData.nodeDataArray[index].category, modelData.nodeDataArray[index].code, modelData.nodeDataArray[index].subtree.name, modelData.nodeDataArray[index].subtree.version);
                }
                else {
                    dataOperations.addNodeToTree(treeData, modelData.nodeDataArray[index].key, modelData.nodeDataArray[index].text, modelData.nodeDataArray[index].category, modelData.nodeDataArray[index].required_variables, modelData.nodeDataArray[index].output_variables, modelData.nodeDataArray[index].code);
                }
            }

            for (index = 0; index < modelData.linkDataArray.length; ++index) {
                dataOperations.addLinkToTree(treeData, index, modelData.linkDataArray[index].linkType, modelData.linkDataArray[index].from, modelData.linkDataArray[index].to);
            }

            for (index = 0; index < modelData.linkDataArray.length; ++index) {
                dataOperations.addLayoutLinkToTree(treeData, index, modelData.linkDataArray[index].from, modelData.linkDataArray[index].to, modelData.linkDataArray[index].fromPort, modelData.linkDataArray[index].toPort, modelData.linkDataArray[index].points);
            }

            for (index = 0; index < modelData.nodeDataArray.length; ++index) {
                dataOperations.addLayoutNodeToTree(treeData, modelData.nodeDataArray[index].key, modelData.nodeDataArray[index].category, modelData.nodeDataArray[index].loc, modelData.nodeDataArray[index].text);
            }

            treeData.description = modelData.description;
            treeData.name = modelData.name;
            treeData.tree.variables = modelData.variableDataArray;
        },
        addCodeInfoToModelNode: function (mapObjectNodeArrayItem, jsonObjectNodeArrayItem) { //anonymous item has key, category, loc, text fields
            //need to add required variables(ids), output variables(ids), code (text)
            mapObjectNodeArrayItem["code"] = jsonObjectNodeArrayItem[0].code
            mapObjectNodeArrayItem["required_variables"] = jsonObjectNodeArrayItem[0].required_variables;
            mapObjectNodeArrayItem["output_variables"] = jsonObjectNodeArrayItem[0].output_variables;
        },
        addNodeToModel: function (modelData, key, category, loc, text) {
            modelData.nodeDataArray.push({ "key": key, "category": category, "loc": loc, "text": text });
        },
        addSubTreeNodeToModel: function (modelData, key, category, loc, text, subtreeName, subtreeVersion, subtreeVariables) {
            modelData.nodeDataArray.push({ "key": key, "category": category, "loc": loc, "text": text, "subtree": { "name": subtreeName, "version": subtreeVersion, "variables": subtreeVariables } });
        },
        addClearMessagesNodeToModel: function (modelData, key, category, loc, text, subtreeName, subtreeVersion) {
            modelData.nodeDataArray.push({ "key": key, "category": category, "loc": loc, "text": text, "subtree": { "name": subtreeName, "version": subtreeVersion } });
        },
        addLinkToModel: function (modelData, from, to, fromPort, toPort, points, linkType) {
            modelData.linkDataArray.push({ "from": from, "to": to, "fromPort": fromPort, "toPort": toPort, "points": points, "linkType": linkType });
        },
        addLinkToTree: function (treeData, id, type, fromNodeId, toNodeId) {
            treeData.tree.links.push({ "id": id, "type": type, "fromNodeId": fromNodeId, "toNodeId": toNodeId });
        },
        addNodeToTree: function (treeData, id, name, type, required_variables, output_variables, code) {
            code = code.replace(/'/g, "_sgl_quote_").replace(/\n/g, "_newline_").replace(/\r/g, "_carriage_").replace(/\t/g, "_tab_").replace(/"/g, "_quote_");
            treeData.tree.nodes.push({ "id": id, "name": name, "type": type, "required_variables": required_variables, "output_variables": output_variables, "code": code });
        },
        addSubTreeNodeToTree: function (treeData, id, name, type, required_variables, output_variables, code, subtreeName, subtreeVersion, variables) {
            code = code.replace(/'/g, "_sgl_quote_").replace(/\n/g, "_newline_").replace(/\r/g, "_carriage_").replace(/\t/g, "_tab_").replace(/"/g, "_quote_");

            var variableData = [];
            angular.forEach(variables, function (item) {
                variableData.push({
                    name: item.name,
                    parentName: item.parentName,
                    type: item.type,
                    usage: item.usage
                });
            });
            treeData.tree.nodes.push({ "id": id, "name": name, "type": type, "required_variables": required_variables, "output_variables": output_variables, "code": code, "subtreeName": subtreeName, "subtreeVersion": subtreeVersion, "subtreeVariables": variables });
        },
        addClearMessagesNodeToTree: function (treeData, id, name, type, code, subtreeName, subtreeVersion) {
            code = code.replace(/'/g, "_sgl_quote_").replace(/\n/g, "_newline_").replace(/\r/g, "_carriage_").replace(/\t/g, "_tab_").replace(/"/g, "_quote_");
            treeData.tree.nodes.push({ "id": id, "name": name, "type": type, "code": code, "subtreeName": subtreeName, "subtreeVersion": subtreeVersion });
        },
        addLayoutNodeToTree: function (treeData, id, category, loc, text) {
            treeData.layout.nodes.push({ "id": id, "category": category, "loc": loc, "text": text });
        },
        addLayoutLinkToTree: function (treeData, linkid, from, to, fromPort, toPort, points) {
            treeData.layout.links.push({ "linkid": linkid, "from": from, "to": to, "fromPort": fromPort, "toPort": toPort, "points": points });
        },
        addVariable: function (treeData, id, usage, type, type_enumeration_values, output_variables, code) {
            treeData.tree.variables.push({ "id": id, "usage": usage, "type": type, "type_enumeration_values": type_enumeration_values, "name": name });
        }
    }

    var operations = {
        newDocument: function (newTreeId, newTreeVersionId) {
            $rootScope.document =
                {
                    isReadOnly: false,
                    isPublished: false,
                    hasChanges: true,
                    isNewDocument: true,
                    treeId: newTreeId,
                    treeVersionId: newTreeVersionId,
                    displayName: "New Decision Tree",
                    data: $documentVersionManager.emptyDocument(),
                    dataModel: dataOperations.getBlankMapObject()
                };
            dataOperations.convertTreeDataToDataModel($rootScope.document.data);
            currentDocumentHasChangesWatcher = $rootScope.$watch('document.hasChanges', eventHandlers.onDocumentChanged);
            currentDocumentNameChangeWatcher = $rootScope.$watch('document.dataModel.name', eventHandlers.onDocumentNameChanged);

            operations.setDocumentTitle();
            $eventAggregatorService.publish($eventDefinitions.onDocumentLoaded, $rootScope.document);
            $eventAggregatorService.subscribe($eventDefinitions.onDocumentVariableChanged, eventHandlers.onDocumentVariableChanged);
        },
        newFromExistingDocument: function (fromTreeVersionId, newTreeId, newTreeVersionId) {
            var deferred = $q.defer();
            // first go query for the tree version data
            var query = new $decisionTreeDesignQueries.GetDecisionTreeQuery(fromTreeVersionId);
            $activityManager.startActivityWithKey('documentLoading');
            $queryManager.sendQueryAsync(query).then(function (data) {
                var result = data.data.ReturnData.Results.$values[0];
                $rootScope.document =
                    {
                        isReadOnly: false,
                        isPublished: false,
                        hasChanges: true,
                        isNewDocument: true,
                        treeId: newTreeId,
                        treeVersionId: newTreeVersionId,
                        displayName: "New Decision Tree",
                        data: angular.fromJson(result.Data),
                        dataModel: dataOperations.getBlankMapObject(),
                        selection: {}
                    };
                // reset some tree data
                $rootScope.document.data.version = 1;
                $rootScope.document.data.name = "New Decision Tree";
                $rootScope.document.data.description = "New Decision Tree";

                dataOperations.convertTreeDataToDataModel($rootScope.document.data);
                currentDocumentHasChangesWatcher = $rootScope.$watch('document.hasChanges', eventHandlers.onDocumentChanged);
                currentDocumentNameChangeWatcher = $rootScope.$watch('document.dataModel.name', eventHandlers.onDocumentNamedChanged);
                operations.setDocumentTitle();
                $eventAggregatorService.publish($eventDefinitions.onDocumentLoaded, $rootScope.document);
                $eventAggregatorService.subscribe($eventDefinitions.onDocumentVariableChanged, eventHandlers.onDocumentVariableChanged);
                $eventAggregatorService.publish($eventDefinitions.onNewDocumentLoaded, $rootScope.document);
                $activityManager.stopActivityWithKey('documentLoading');
                deferred.resolve();
            },
            function () {
                deferred.reject();
            });

            return deferred.promise;
        },
        deleteCurrentDocument: function () {
            var deferred = $q.defer();
            $modalDialogManager.dialogs.deleteTreeConfirm.show().then(function () {
                //delete this version only
                var treeVersionId = $rootScope.document.treeVersionId;
                var treeId = $rootScope.document.treeId;
                operations.closeInternalDocument().then(function () {
                    var deleteCommand = new $decisionTreeDesignCommands.DeleteDecisionTreeCommand(treeId, treeVersionId, $rootScope.username);
                    $commandManager.sendCommandAsync(deleteCommand).then(function () {
                        deferred.resolve();
                    }, function () {
                        deferred.reject();
                    });
                }, function () {
                    deferred.reject();
                });
            }, function () {
                deferred.reject();
            });
            return deferred.promise;
        },
        ensureCurrentDocumentClosed: function () {
            return operations.closeDocument();
        },
        ensureNewVersion: function () {
            var deferred = $q.defer();
            $modalDialogManager.dialogs.savePublishedConfirm.show({ name: $rootScope.document.data.name }).then(function () {
                operations.removeCurrentlyOpenDecisionTreeForUser($rootScope.document.treeVersionId, $rootScope.username).then(function () {
                    var query = new $decisionTreeDesignQueries.GetNewGuidQuery();
                    $queryManager.sendQueryAsync(query).then(function (data) {
                        var newTreeVersionId = data.data.ReturnData.Results.$values[0].Id;
                        $rootScope.document.treeVersionId = newTreeVersionId;
                        $rootScope.document.data.version++;
                        $rootScope.document.treeVersion++;
                        eventHandlers.onDocumentChanged();
                        deferred.resolve();
                    }, function () {
                        deferred.reject();
                    });
                }, function () {
                    deferred.reject();
                });
            }, function () {
                deferred.resolve();
            }, function () {
                deferred.reject();
            });

            return deferred.promise;
        },
        closeDocument: function () {
            var deferred = $q.defer();

            // check if there is an existing document open
            if ($rootScope.document !== undefined) {
                // check if the document has unsaved changes
                if ($rootScope.document.hasChanges) {
                    // notify the user about them losing changes
                    $modalDialogManager.dialogs.unsavedChanges.show().then(function (shouldSave) {
                        // save
                        if (shouldSave === true) {
                            operations.saveDocument().then(function () {
                                operations.closeInternalDocument();
                                deferred.resolve();
                            }, function () {
                                deferred.reject();
                            });
                        }
                        else {
                            // just close don't save
                            operations.closeInternalDocument();
                            deferred.resolve();
                        }
                    }, function () {
                        deferred.reject();
                    });
                }
                else {
                    operations.closeInternalDocument();
                    deferred.resolve();
                }
            }
            else {
                deferred.resolve();
            }

            return deferred.promise;
        },
        removeCurrentlyOpenDecisionTreeForUser: function (treeVersionId, username) {
            return $sessionLockService.closeLockForTrees(treeVersionId, username);
        },
        finaliseClose: function () {
            $eventAggregatorService.unsubscribe($eventDefinitions.onDocumentVariableChanged, eventHandlers.onDocumentVariableChanged);
            $eventAggregatorService.publish($eventDefinitions.onDocumentUnloaded, $rootScope.document);
            // remove the document
            $rootScope.document = undefined;
            operations.resetDocumentTitle();

            if (currentDocumentHasChangesWatcher != null) {
                currentDocumentHasChangesWatcher();
                currentDocumentHasChangesWatcher = null;
            }
            if (currentDocumentNameChangeWatcher != null) {
                currentDocumentNameChangeWatcher();
                currentDocumentNameChangeWatcher = null;
            }
            if (currentDocumentOutputsArrayChangesWatcher != null) {
                currentDocumentOutputsArrayChangesWatcher();
                currentDocumentOutputsArrayChangesWatcher = null;
            }
        },
        closeInternalDocument: function () {
            var deferred = $q.defer();
            if (!$rootScope.document.isReadOnly) {
                operations.removeCurrentlyOpenDecisionTreeForUser($rootScope.document.treeVersionId, $rootScope.username).then(function () {
                    operations.finaliseClose();
                    deferred.resolve();
                }, function () {
                    deferred.reject();
                });
            }
            else {
                operations.finaliseClose();
                deferred.resolve();
            }

            return deferred.promise;
        },
        saveNewTreeVersion: function () {
            var deferred = $q.defer();
            dataOperations.convertDataModelToTreeData();
            var data = angular.toJson($rootScope.document.data);
            var newTreeVersionId;
            var command = new $decisionTreeDesignCommands.SaveNewDecisionTreeVersionCommand($rootScope.document.treeVersionId, $rootScope.document.treeId, $rootScope.document.data.version, data, $rootScope.username);

            $commandManager.sendCommandAsync(command).then(function () {
                $rootScope.document.hasChanges = false;
                $rootScope.document.isPublished = false;
                operations.saveCurrentlyOpenDecisionTree(newTreeVersionId, $rootScope.username).then(function () {
                    eventHandlers.onDocumentChanged();
                    $eventAggregatorService.publish($eventDefinitions.onDocumentSaved, $rootScope.document);
                    deferred.resolve();
                }, function () {
                    deferred.reject();
                });
            }, function () {
                deferred.reject();
            })

            return deferred.promise;
        },
        publishDocument: function () {
            var deferred = $q.defer();

            // first update the data tree
            dataOperations.convertDataModelToTreeData();
            var data = angular.toJson($rootScope.document.data);

            var command = new $decisionTreeDesignCommands.SaveAndPublishDecisionTreeCommand($rootScope.document.treeId, $rootScope.document.data.name, $rootScope.document.data.description, true, $designSurfaceManager.getThumbnail(), $rootScope.document.treeVersionId, data, $rootScope.username, $rootScope.document.hasChanges);

            $commandManager.sendCommandAsync(command).then(function () {
                $rootScope.document.hasChanges = false;
                $rootScope.document.isPublished = true;
                $eventAggregatorService.publish($eventDefinitions.onDocumentSaved, $rootScope.document);
                deferred.resolve();
            }, function () {
                deferred.reject();
            })

            return deferred.promise;
        },
        saveDocument: function () {
            var deferred = $q.defer();
            if ($rootScope.document && !$rootScope.document.isReadOnly) {
                $eventAggregatorService.publish($eventDefinitions.onBeforeDocumentSaved, $rootScope.document);
                // first update the data tree
                dataOperations.convertDataModelToTreeData();
                var data = angular.toJson($rootScope.document.data);
                var command = new $decisionTreeDesignCommands.SaveDecisionTreeVersionCommand($rootScope.document.treeId, $rootScope.document.data.name, $rootScope.document.data.description, true, $designSurfaceManager.getThumbnail(), $rootScope.document.treeVersionId, data, $rootScope.username);

                $commandManager.sendCommandAsync(command).then(function () {
                    $rootScope.document.hasChanges = false;
                    $eventAggregatorService.publish($eventDefinitions.onDocumentSaved, $rootScope.document);
                    deferred.resolve();
                }, function () {
                    deferred.reject();
                })
            } else {
                setTimeout(deferred.reject(), 1000);
            }
            return deferred.promise;
        },
        validateDocumentName: function (newDocumentName) {
            var deferred = $q.defer();
            if (newDocumentName.length === 0) {
                deferred.reject();
            }
            var query = new $decisionTreeDesignQueries.GetDecisionTreeByNameQuery(newDocumentName);
            $queryManager.sendQueryAsync(query).then(function (data) {
                if (data.data.ReturnData.Results.$values.length === 0) {
                    deferred.resolve();
                }
                else {
                    deferred.reject();
                }
            }, function () {
                deferred.reject();
            });
            return deferred.promise;
        },
        saveDocumentAs: function () {
            var deferred = $q.defer();

            $modalDialogManager.dialogs.saveAs.show({ validateAsync: operations.validateDocumentName }).then(function (newDocumentName) {
                $rootScope.document.dataModel.name = newDocumentName;

                // first update the data tree
                dataOperations.convertDataModelToTreeData();
                var treeData = angular.toJson($rootScope.document.data);
                if ($rootScope.document.isNewDocument) {
                    var command = new $decisionTreeDesignCommands.SaveDecisionTreeAsCommand($rootScope.document.treeId, $rootScope.document.data.name, $rootScope.document.data.description, true, $rootScope.document.treeVersionId, treeData, $rootScope.username, $designSurfaceManager.getThumbnail());
                    $commandManager.sendCommandAsync(command).then(function () {
                        $rootScope.document.isNewDocument = false;
                        $rootScope.document.hasChanges = false;
                        operations.setDocumentTitle();
                        $eventAggregatorService.publish($eventDefinitions.onNewDocumentSaved, $rootScope.document);
                        $eventAggregatorService.publish($eventDefinitions.onDocumentSaved, $rootScope.document);
                        deferred.resolve();
                    }, function () {
                        deferred.reject();
                    })
                }
                else {
                    // if it is not a new document then we need new tree ids and version Ids
                    // get a new GUID for the tree
                    var oldId = $rootScope.document.treeVersionId;
                    var query = new $decisionTreeDesignQueries.GetNewGuidQuery();
                    $queryManager.sendQueryAsync(query).then(function (data) {
                        var newTreeId = data.data.ReturnData.Results.$values[0].Id
                        $rootScope.document.treeId = newTreeId;

                        $queryManager.sendQueryAsync(query).then(function (data) {
                            var newTreeVersionId = data.data.ReturnData.Results.$values[0].Id;
                            $rootScope.document.treeVersionId = newTreeVersionId;

                            var command = new $decisionTreeDesignCommands.SaveDecisionTreeAsCommand($rootScope.document.treeId, $rootScope.document.data.name, $rootScope.document.data.description, true, $rootScope.document.treeVersionId, treeData, $rootScope.username, $designSurfaceManager.getThumbnail());
                            $commandManager.sendCommandAsync(command).then(function () {
                                if (!$rootScope.document.isReadOnly) {
                                    operations.removeCurrentlyOpenDecisionTreeForUser(oldId, $rootScope.username);
                                }
                                $state.go("home.design", { treeId: $rootScope.document.treeId, treeVersionId: $rootScope.document.treeVersionId, isNew: false, reload: true }, { reload: true });
                                deferred.resolve();
                            }, function (error) {
                                deferred.reject();
                            })
                        }, function (error) {
                            deferred.reject();
                        });
                    }, function (error) {
                        deferred.reject();
                    });
                }
            }, function (error) {
                deferred.reject();
            });

            return deferred.promise;
        },
        checkIfCurrentlyOpenByAnotherUser: function (treeVersionId, username) {
            var deferred = $q.defer();
            var currentlyOpenQuery = new $decisionTreeDesignQueries.GetCurrentlyOpenDecisionTreeQuery(treeVersionId);
            $queryManager.sendQueryAsync(currentlyOpenQuery).then(function (resultData) {
                var currentlyOpenData = resultData.data.ReturnData.Results.$values[0];
                var isOpen = false;
                if (currentlyOpenData) {
                    isOpen = true;
                }
                var isOpenByAnotherUser = isOpen && currentlyOpenData.Username.toUpperCase() !== $rootScope.username.toUpperCase();
                var result = {
                    isOpen: isOpen,
                    isOpenByAnotherUser: isOpenByAnotherUser,
                    isOpenBy: isOpen == true ? currentlyOpenData.Username : ""
                }
                deferred.resolve(result);
            });
            return deferred.promise;
        },
        saveCurrentlyOpenDecisionTree: function (treeVersionId, username) {
            return $sessionLockService.openLockForTrees(treeVersionId, username);
        },
        refreshCurrentlyOpenDecisionTree: function (treeVersionId, date) {
            var refreshTreeCommand = new $decisionTreeDesignCommands.RefreshCurrentlyOpenDecisionTreeCommand(treeVersionId, date);
            $commandManager.sendCommandAsync(refreshTreeCommand);
        },
        opendocument: function (treeId, treeVersionId) {
            var deferred = $q.defer();
            // first go query for the tree version data
            var query = new $decisionTreeDesignQueries.GetDecisionTreeQuery(treeVersionId);
            $activityManager.startActivityWithKey('documentLoading');
            operations.checkIfCurrentlyOpenByAnotherUser(treeVersionId, $rootScope.username).then(function (currentlyOpenData) {
                if (!currentlyOpenData.isOpen) {
                    $eventAggregatorService.subscribe($eventDefinitions.onPendingLockRelease, eventHandlers.onPendingLockRelease);
                    operations.saveCurrentlyOpenDecisionTree(treeVersionId, $rootScope.username);
                }
                $queryManager.sendQueryAsync(query).then(function (data) {
                    var result = data.data.ReturnData.Results.$values[0];
                    result.Data = $documentVersionManager.loadDocument(result.Data);

                    var isReadOnly = (result.IsPublished == true) ? (result.Version < result.MaxVersion) : false;
                    var openBy = currentlyOpenData.isOpenBy;
                    if (currentlyOpenData.isOpenByAnotherUser) {
                        isReadOnly = true;
                        console.log("Tree is already open by another user");
                        $eventAggregatorService.subscribe($eventDefinitions.onRemovedLocks, eventHandlers.onRemovedLocks);
                    }

                    $rootScope.document =
                        {
                            isLockedByUser: (currentlyOpenData.isOpenByAnotherUser == true) ? openBy : null,
                            isReadOnly: isReadOnly,
                            isPublished: result.IsPublished,
                            hasChanges: false,
                            isNewDocument: false,
                            treeId: result.DecisionTreeId,
                            treeVersionId: result.DecisionTreeVersionId,
                            treeVersion: result.Version,
                            displayName: result.Name,
                            data: result.Data,
                            dataModel: dataOperations.getBlankMapObject(),
                            selection: {}
                        };


                    // we need to check if this tree uses subtrees, if so we need to refresh the subtree variable mapping
                    operations.getUsedSubTreeVariables().then(function (subTrees) {
                        var subTreeModifed = false;
                        for (var k = 0, f = subTrees.length; k < f; k++) {
                            // find the matching subtree node
                            var subtree = subTrees[k];
                            var subtreeVariables = subtree.output_variables.concat(subtree.input_variables);
                            var subtreeNodes = $rootScope.document.data.tree.nodes.filter(function (node) {
                                return node.type === "SubTree" && node.subtreeName === subtree.treeName && node.subtreeVersion === subtree.treeVersion;
                            })
                            var subtreeNode = undefined;
                            if (subtreeNodes.length === 1) {
                                subtreeNode = subtreeNodes[0];
                            }

                            if (subtreeNode !== undefined) {
                                // remove any variables that do not exist in the current list
                                var len = subtreeNode.subtreeVariables.length;
                                while(len--)  {

                                    var subtreeNodeVariable = subtreeNode.subtreeVariables[len];
                                    // try to find match in the loaded list
                                    var matchedVariables = subtreeVariables.filter(function (variable) {
                                        return variable.name === subtreeNodeVariable.name && variable.type === subtreeNodeVariable.type;
                                    })
                                    var matchedVariable = undefined;
                                    if (matchedVariables.length === 1) {
                                        matchedVariable = matchedVariables[0];
                                    }

                                    // if there was no match we need to get rid of the existing subtree variable link
                                    if (matchedVariable === undefined) {
                                        subtreeNode.subtreeVariables.splice(len, 1);
                                        subTreeModifed = true;
                                    }
                                }
                                // add any variables that don't exist in the current mapping
                                var len = subtreeVariables.length;
                                while (len--) {

                                    var subtreeNodeVariable = subtreeVariables[len];
                                    // try to find match in the loaded list
                                    var matchedVariables = subtreeNode.subtreeVariables.filter(function (variable) {
                                        return variable.name === subtreeNodeVariable.name && variable.type === subtreeNodeVariable.type;
                                    })
                                    var matchedVariable = undefined;
                                    if (matchedVariables.length === 1) {
                                        matchedVariable = matchedVariables[0];
                                    }

                                    // if there was no match we need to get add it as a subtree variable link
                                    if (matchedVariable === undefined) {
                                        subtreeNode.subtreeVariables.push(subtreeVariables[len]);
                                        subTreeModifed = true;
                                    }
                                }
                            }
                        }
                        if (subTreeModifed === true) {
                            $rootScope.document.hasChanges = true;
                            $notificationService.notifyInfo('Information', 'Subtree variables have changed, you may need to update the variable mappings.');
                        }

                        dataOperations.convertTreeDataToDataModel($rootScope.document.data);
                        currentDocumentHasChangesWatcher = $rootScope.$watch('document.hasChanges', eventHandlers.onDocumentChanged);
                        currentDocumentNameChangeWatcher = $rootScope.$watch('document.dataModel.name', eventHandlers.onDocumentNameChanged);
                        operations.setDocumentTitle();

                        $eventAggregatorService.publish($eventDefinitions.onDocumentLoaded, $rootScope.document);
                        $eventAggregatorService.subscribe($eventDefinitions.onDocumentVariableChanged, eventHandlers.onDocumentVariableChanged);
                        $activityManager.stopActivityWithKey('documentLoading');
                        deferred.resolve();
                    });


                }, function () {
                    $activityManager.stopActivityWithKey('documentLoading');
                    deferred.reject();
                })
            }, function () {
                $activityManager.stopActivityWithKey('documentLoading');
                deferred.reject();
            });

            return deferred.promise;
        },
        refreshDocumentSubTrees: function () {
        },
        setDocumentTitle: function () {
            if ($rootScope.document && $rootScope.document.dataModel) {
                document.title = $rootScope.document.dataModel.name;
                eventHandlers.onDocumentChanged();
            }
        },
        resetDocumentTitle: function () {
            document.title = "Decision Tree Designer";
        },
        updateDocumentInfo: function () {
            // check if there is an existing document open
            if ($rootScope.document !== null) {
                $activityManager.startActivityWithKey("loadingDocumentInfo");
                var deferred = $q.defer();
                var getDocumentHistory = new $decisionTreeDesignQueries.GetDecisionTreeHistoryInfoQuery($rootScope.document.treeId);
                $queryManager.sendQueryAsync(getDocumentHistory).then(function (data) {
                    $activityManager.stopActivityWithKey("loadingDocumentInfo");
                    $rootScope.document.info = data;
                    deferred.resolve(data);
                });
                return deferred.promise;
            }
        },
        getSubTreeVariables: function (treeName, treeVersion) {
            var deferred = $q.defer();
            var query = new $decisionTreeDesignQueries.GetDecisionTreeByNameAndVersionNumberQuery(treeName, treeVersion);
            $queryManager.sendQueryAsync(query).then(function (data) {
                if (data.data.ReturnData.Results.$values[0]) {
                    var subtreeData = angular.fromJson(data.data.ReturnData.Results.$values[0].Data);
                    var variables = subtreeData.tree.variables;
                    deferred.resolve(variables);
                } else {
                    deferred.resolve({});
                }
            });
            return deferred.promise;
        },
        getUsedSubTreeVariables: function () {
            var deferred = $q.defer();
            var subtreeNodes = $rootScope.document.data.tree.nodes.filter(function (node) {
                return node.type === "SubTree";
            })

            var results = [];
            var waitAll = [];
            for (var i = 0, c = subtreeNodes.length; i < c; i++) {
                var result = {
                    treeName: subtreeNodes[i].subtreeName,
                    treeVersion: subtreeNodes[i].subtreeVersion,
                    output_variables: [],
                    input_variables: []
                };
                results.push(result);

                // get the variables for this subtree
                var deferral = $q.defer();
                waitAll.push(deferral.promise);

                // using promises in a loop requires us to capture input parameters ina  closure
                var perform = function(result, i, deferral){
                    operations.getSubTreeVariables(subtreeNodes[i].subtreeName, subtreeNodes[i].subtreeVersion).then(function (variables) {


                        for (var j = 0, d = variables.length; j < d; j++) {
                            if (variables[j].usage === "output") {
                                result.output_variables.push(variables[j]);
                            }
                            else
                                if (variables[j].usage === "input") {
                                    result.input_variables.push(variables[j]);
                                }
                        }
                        deferral.resolve();
                    })
                }
                perform(result, i, deferral);
            }

            $q.all(waitAll).then(function () {
                deferred.resolve(results);
            }, function () {
                deferred.resolve({});
            })

            return deferred.promise;
        },
        stripInvalidChars: function (original) {
            return original.replace(/ /g, '').replace(/_/g, '').replace(/g-/, '').replace(/\(/, '').replace(/\)/, '');
        },
        downCaseFirstCharacter: function (word) {
            return word[0].toLowerCase() + word.substring(1);
        },
        hasMessage: function (searchMessage) {
            var duplicates = $.grep($rootScope.document.dataModel.outputMessagesArray, function (msg) {
                return ((msg.message === searchMessage.message) && (msg.type === searchMessage.type));
            });
            return duplicates.length > 0;
        },
        getIndexOfMessage: function (searchMessage) {
            var outputMessages = $rootScope.document.dataModel.outputMessagesArray;
            result = -1;
            for (var i = 0; i < outputMessages.length; i++) {
                if (outputMessages[i].message === searchMessage.message && outputMessages[i].type === searchMessage.type)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }
    }

    var eventHandlers = {
        onDocumentChanged: function () {
            var idx = document.title.indexOf("*");
            if ($rootScope.document !== undefined) {
                if ($rootScope.document.hasChanges) {
                    if (idx < 0) {
                        document.title += "*";
                    }
                    $rootScope.document.displayName = $rootScope.document.dataModel.name + " (v." + $rootScope.document.data.version + ")" + " * ";
                }
                else {
                    if (idx >= 0) {
                        document.title = document.title.substr(0, idx);
                    }
                    $rootScope.document.displayName = $rootScope.document.dataModel.name + " (v." + $rootScope.document.data.version + ")";
                }
            }
        },
        onModelChanged: function () {
            if (!$rootScope.document.isReadOnly) {
                lastModifiedDate = new Date();
            }
        },
        onPendingLockRelease: function (treeInfo) {
            $.debounce(2000, true, warnUserOfSessionTimeout(treeInfo));
        },
        onDocumentNameChanged: function () {
            eventHandlers.onDocumentChanged();
            operations.setDocumentTitle();
        },
        onRemovedLocks: $.debounce(3000, false, function (data) {
            if ($rootScope.document) {
                for (var i = 0, c = data.length; i < c; i++) {
                    if (data[i].DecisionTreeId == $rootScope.document.treeId && data[i].DecisionTreeVersionId == $rootScope.document.treeVersionId && data[i].Username != $rootScope.Username) {
                        $modalDialogManager.dialogs.documentUnlocked.show().then(function () {
                            $state.go("home.design", { treeId: $rootScope.document.treeId, treeVersionId: $rootScope.document.treeVersionId, isNew: false, reload:true }, { reload: true });
                        });
                        $eventAggregatorService.unsubscribe($eventDefinitions.onRemovedLocks, eventHandlers.onRemovedLocks);
                    }
                }
            }
        }),
        onDocumentSaved: function (savedDocument) {
            operations.updateDocumentInfo();
            $notificationService.notifySuccess('Information', 'Document saved successfully.');
        },
        onDocumentVariableChanged: function (updatedVariable) {
            if (updatedVariable.action == "change") {
                for (var i = 0, c = $rootScope.document.dataModel.nodeDataArray.length; i < c; i++) {
                    if ($rootScope.document.dataModel.nodeDataArray[i].code) {
                        $rootScope.document.dataModel.nodeDataArray[i].code = $rootScope.document.dataModel.nodeDataArray[i].code.replace(new RegExp(updatedVariable.oldName, "gm"), updatedVariable.name);
                    }
                }
                
            }
        }
    }
    $eventAggregatorService.subscribe($eventDefinitions.onModelChanged, eventHandlers.onModelChanged);
    
    $eventAggregatorService.subscribe($eventDefinitions.onDocumentSaved, eventHandlers.onDocumentSaved);

    return {
        newFromExistingDocument: function (fromTreeVersionId, newTreeId, newTreeVersionId) {
            var deferred = $q.defer();

            operations.ensureCurrentDocumentClosed()
                .then(function () {
                    // we can safely create the new document
                    operations.newFromExistingDocument(fromTreeVersionId, newTreeId, newTreeVersionId).then(function () {
                        deferred.resolve();
                    }, function () {
                        deferred.reject();
                    })
                },
            function () {
                deferred.reject();
            });

            return deferred.promise;
        },
        newDocument: function (newTreeId, newTreeVersionId) {
            var deferred = $q.defer();

            operations.ensureCurrentDocumentClosed()
                .then(function () {
                    // we can safely create the new document
                    operations.newDocument(newTreeId, newTreeVersionId);
                    deferred.resolve();
                },
            function () {
                deferred.reject();
            });

            return deferred.promise;
        },
        openDocument: function (treeId, treeVersionId) {
            var deferred = $q.defer();
            operations.ensureCurrentDocumentClosed()
                .then(function () {
                    operations.opendocument(treeId, treeVersionId).then(function () {
                        // if the tree has subtrees then we need to refresh the subtree variable links

                        deferred.resolve();
                    }, function () {
                        deferred.reject();
                    })
                },
                function () {
                    deferred.reject();
                });

            return deferred.promise;
        },
        closeDocument: function () {
            var deferred = $q.defer();
            operations.ensureCurrentDocumentClosed()
                .then(function () {
                    $eventAggregatorService.unsubscribe($eventDefinitions.onPendingLockRelease, eventHandlers.onPendingLockRelease);
                    $eventAggregatorService.unsubscribe($eventDefinitions.onRemovedLocks, eventHandlers.onRemovedLocks);
                    $state.go('home.start-page');
                    deferred.resolve();
                }, function () {
                    deferred.reject();
                });

            return deferred.promise;
        },
        deleteDocument: function () { //check user role
            if ($rootScope.document !== null && $rootScope.document.treeVersionId !== null) {
                if ($rootScope.document.isPublished == false) {
                    if ($rootScope.document.isNewDocument == false) {
                        operations.deleteCurrentDocument()
                        .then(function () {
                            $state.go('home.start-page');
                        }, function () {
                            $notificationService.notifyError('Abort', 'Abort');
                            $state.go('home.start-page');
                        });
                    }
                    else {
                        operations.closeInternalDocument();
                        $state.go('home.start-page');
                    }
                }
                else {
                    $modalDialogManager.dialogs.cannotDelete.show();
                }
            }
        },
        saveDocument: function () {
            // check if there is an existing document open
            if ($rootScope.document !== null) {
                if ($rootScope.document.isPublished) {
                    operations.ensureNewVersion().then(function () {
                        operations.saveNewTreeVersion();
                    }, function () {
                        //error?
                    });
                }
                else if ($rootScope.document.isNewDocument !== undefined && $rootScope.document.isNewDocument === true) {
                    operations.saveDocumentAs();
                }
                else {
                    operations.saveDocument();
                }
            }
        },
        publishDocument: function () {
            // check if there is an existing document open
            if ($rootScope.document !== null) {
                if ($rootScope.document.isNewDocument !== undefined && $rootScope.document.isNewDocument === true) {
                    $notificationService.notifyError('Error', 'Cannot publish draft document');
                    deferred.reject();
                }
                else {
                    var deferred = $q.defer();
                    $modalDialogManager.dialogs.publishConfirm.show({ name: $rootScope.document.data.name }).then(function () {
                        operations.publishDocument().then(function () {
                            deferred.resolve();
                        }, function () {
                            deferred.reject();
                        });
                    }, function () {
                        deferred.reject();
                    }, function () {
                        deferred.reject();
                    });
                }
            }
            return deferred.promise;
        },
        saveDocumentAs: function () {
            var deferred = $q.defer();
            // check if there is an existing document open
            if ($rootScope.document !== null) {
                operations.saveDocumentAs().then(function () {
                    deferred.resolve;
                }, function () {
                    deferred.reject();
                })
            }

            return deferred.promise;
        },
        getDocumentInfo: function () {
            var deferred = $q.defer();

            if ($rootScope.document.info === undefined) {
                operations.updateDocumentInfo().then(function (data) {
                    deferred.resolve(data);
                });
            }
            else {
                deferred.resolve($rootScope.document.info);
            }
            return deferred.promise;
        },
        getAvailableSubTrees: function () {
            var deferred = $q.defer();
            var query = new $decisionTreeDesignQueries.GetLatestDecisionTreesQuery();
            $queryManager.sendQueryAsync(query).then(function (data) {
                deferred.resolve(data.data.ReturnData.Results.$values);
            });
            return deferred.promise;
        },
        addNewLocalVariable: function (newvar) {
            $rootScope.document.hasChanges = true;
            $rootScope.document.dataModel.variableDataArray.push(newvar);
        },
        removeLocalVariable: function (rVar) {
            var ind = $rootScope.document.dataModel.variableDataArray.indexOf(rVar);
            if (ind > -1) {
                $rootScope.document.hasChanges = true;
                $rootScope.document.dataModel.variableDataArray.splice(ind, 1);
            }
        },
        getLatestLocalID: function () {
            var varArray = $rootScope.document.dataModel.variableDataArray;
            if ((!varArray) || (varArray.length == 0))
                return "1";
            var largest = Math.max.apply(Math, varArray.map(function (varEl) { return parseInt(varEl.id); }));
            return (largest + 1).toString();
        },
        getInputVariablesForTree: function () { //not globals! //document level
            //need a nice list of
            //{"id" ..., "name": ..., "type":...}
            var requiredList = [];
            if (!$rootScope.document || !$rootScope.document.data)
                return [];
            var allVariables = $rootScope.document.dataModel.variableDataArray;
            requiredList = allVariables.filter(function (el) {
                return el.usage == "input";
            });
            return requiredList;
        },
        getOutputVariablesForTree: function () { //not globals! per node
            //use datamap, not jsontree
            var outList = [];
            if (!$rootScope.document || !$rootScope.document.data)
                return [];
            var allVariables = $rootScope.document.dataModel.variableDataArray;
            outList = allVariables.filter(function (el) {
                return el.usage == "output";
            });
            return outList;
        },
        addOutputMessage: function (newMsg) {
            if (!operations.hasMessage(newMsg)) {
                $rootScope.document.dataModel.outputMessagesArray.push(newMsg);
            }
        },
        removeOutputMessage: function (msg) {
            var ind = operations.getIndexOfMessage(msg);
            if (ind > -1) {
                $rootScope.document.dataModel.outputMessagesArray.splice(ind, 1);
            }
        },
        getSubTreeVariables: function (treeName, treeVersion) {
            return operations.getSubTreeVariables(treeName, treeVersion);
        },
        getUsedSubTreeVariables: function () {
            return operations.getUsedSubTreeVariables();
        }
    }
}])