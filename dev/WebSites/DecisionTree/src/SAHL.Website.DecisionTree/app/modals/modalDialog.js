'use strict';
/*
    modalDialog
    use :
*/
angular.module('sahl.tools.app.services.modalDialogManager', [
    'sahl.tools.app.serviceConfig',
])
.factory('$modalDialogManager', ['$rootScope', '$compile', '$parseHttpService', '$q', '$queryManager', '$decisionTreeDesignQueries', '$commandManager', '$decisionTreeDesignCommands', 'filterFilter','$timeout', '$keyboardManager',
function ($rootScope, $compile, $parseHttpService, $q, $queryManager, $decisionTreeDesignQueries, $commandManager, $decisionTreeDesignCommands, filterFilter, $timeout, $keyboardManager) {
        var deferred = undefined;
        var scope = undefined;

        var options = undefined;

        $.Dialog.autoResize = function () {
            if (!$.Dialog.opened || METRO_DIALOG == undefined) {
                return false;
            }

            var _content = METRO_DIALOG.children(".content");
            var _caption = METRO_DIALOG.children(".caption");

            var top = ($(window).height() - METRO_DIALOG.outerHeight()) / 2;
            var left = ($(window).width() - METRO_DIALOG.outerWidth()) / 2;

            METRO_DIALOG.css({
                width: _content.outerWidth(),
                height: _content.outerHeight() + _caption.outerHeight(),
                top: top,
                left: left
            });

            return true;
        }

        var dialogOperations = {
            closeCurrent: function () {
                var check = $.Dialog.close();
                if (deferred !== undefined) {
                    deferred.resolve();
                    deferred = undefined;
                }
            },
            cancelCurrent: function () {
                var check = $.Dialog.close();
                if (deferred !== undefined) {
                    deferred.reject();
                    deferred = undefined;
                }
            },
            cancelCurrentFromKeyboard: function ($event) {
                $event.preventDefault();
                dialogOperations.cancelCurrent();
            },
            initialise: function () {
                scope = $rootScope.$new(true);
                scope.closeDialog = function () {
                    dialogOperations.closeCurrent();
                }
                scope.cancelDialog = function () {
                    dialogOperations.cancelCurrent();
                }
                scope.validate = dialogOperations.validate;
                scope.isValid = false;
                scope.data = options !== undefined ? options.data : null;

                $keyboardManager.bind('esc', dialogOperations.cancelCurrentFromKeyboard, { "propagate": false });
            },
            destroy: function () {
                if (scope.closeDialog) {
                    scope.closeDialog = undefined;
                }
                if (scope.cancelDialog) {
                    scope.cancelDialog = undefined;
                }
                if (scope) {
                    scope.$destroy();
                    scope = undefined;
                }
                options = undefined;
                $keyboardManager.unbind('esc');
            },
            showWideModalDialog: function (title, template, icon) {
                $parseHttpService.getString(template).then(function (templateString) {
                    var tpl = null;
                    if (title == undefined)
                        return;
                    if (icon == undefined)
                        icon = '';//'<img src="assets/img/logo-sahl.png">';
                    var handle = $.Dialog({
                        overlay: true,
                        shadow: true,
                        flat: true,
                        draggable: true,
                        icon: icon,
                        title: title,
                        content: '',
                        padding: 5,
                        width: '100%',
                        height: 'auto',
                        sysButtons: {
                            btnMin: false,
                            btnMax: false,
                            btnClose: true
                        },
                        sysBtnCloseClick: function (e) {
                            if (deferred !== undefined) {
                                deferred.reject();
                                deferred = null;
                            }
                        },
                        onShow: function (_dialog) {
                            $.Metro.initInputs();
                            tpl = $compile(templateString)(scope);
                            $.Dialog.content( tpl);
                        }
                    });
                });
                deferred = $q.defer();
                return deferred.promise;
            },
            validate: function (data) {
                if (options != undefined) {
                    if (options.validate != undefined) {
                        scope.isValid = options.validate(data);
                    }
                    if (options.validateAsync != undefined) {
                        $.debounce(1000, false,options.validateAsync(data).then(function () {
                            scope.isValid = true;
                        }, function () {
                            scope.isValid = false;
                        }));
                    }
                }
            }
        }

        var dialogs = {
            deleteTreeConfirm: {
                show: function () {
                    var deferred = $q.defer();
                    dialogOperations.initialise();
                    dialogOperations.showWideModalDialog("Confirm Delete", './app/modals/delete/deleteVersionConfirm.tpl.html')
                            .then(function () {
                                deferred.resolve();
                                dialogOperations.destroy();
                            }, function () {
                                deferred.reject();
                                dialogOperations.destroy();
                            });
                    return deferred.promise;
                }
            },
            cannotDelete: {
                show: function () {
                    dialogOperations.initialise();
                    dialogOperations.showWideModalDialog("Illegal Operation", './app/modals/delete/nodeletepublish.tpl.html')
                }
            },
            addMessageGroup: {
                show: function (optionsData) {
                    var deferred = $q.defer();
                    options = optionsData;
                    dialogOperations.initialise();
                    scope.groupName = '';
                    dialogOperations.showWideModalDialog(options.title, './app/modals/messages/addGroup.tpl.html')
                            .then(function () {
                                deferred.resolve(scope.groupName);
                                dialogOperations.destroy();
                            }, function () {
                                deferred.reject();
                                dialogOperations.destroy();

                            });
                    return deferred.promise;
                }
            },
            deleteMessageGroupConfirm: {
                show: function (optionsData) {
                    var deferred = $q.defer();
                    options = optionsData;
                    dialogOperations.initialise();
                    scope.name = options.name;
                    dialogOperations.showWideModalDialog("Confirm Delete", './app/modals/messages/deleteGroupConfirm.tpl.html')
                            .then(function () {
                                deferred.resolve(scope.groupName);
                                dialogOperations.destroy();
                            }, function () {
                                dialogOperations.destroy();
                            });
                    return deferred.promise;
                }
            },
            deleteMessageConfirm: {
                show: function (optionsData) {
                    var deferred = $q.defer();
                    options = optionsData;
                    dialogOperations.initialise();
                    scope.name = options.name;
                    dialogOperations.showWideModalDialog("Confirm Delete", './app/modals/messages/deleteMessageConfirm.tpl.html')
                            .then(function () {
                                deferred.resolve(scope.groupName);
                                dialogOperations.destroy();
                            }, function () {
                                dialogOperations.destroy();
                            });
                    return deferred.promise;
                }
            },
            addVariableGroup: {
                show: function (optionsData) {
                    var deferred = $q.defer();
                    options = optionsData;
                    dialogOperations.initialise();
                    scope.newGroupName = '';
                    dialogOperations.showWideModalDialog("Create Variable Group", './app/modals/variables/addGroup.tpl.html')
                            .then(function () {
                                deferred.resolve(scope.newGroupName);
                                dialogOperations.destroy();
                            }, function () {
                                deferred.reject();
                                dialogOperations.destroy();

                            });
                    return deferred.promise;
                }
            },
            addEnumGroup: {
                show: function (optionsData) {
                    var deferred = $q.defer();
                    options = optionsData;
                    dialogOperations.initialise();
                    dialogOperations.showWideModalDialog(options.title, './app/modals/enumerations/addGroup.tpl.html')
                            .then(function () {
                                deferred.resolve(scope.groupName);
                                dialogOperations.destroy();
                            }, function () {
                                dialogOperations.destroy();
                            });
                    return deferred.promise;
                }
            },
            addEnum: {
                show: function (optionsData) {
                    var deferred = $q.defer();
                    options = optionsData;
                    dialogOperations.initialise();
                    dialogOperations.showWideModalDialog("Add Enumeration", './app/modals/enumerations/addEnumeration.tpl.html')
                            .then(function () {
                                deferred.resolve(scope.enumName);
                                dialogOperations.destroy();
                            }, function () {
                                dialogOperations.destroy();
                            });
                    return deferred.promise;
                }
            },
            deleteEnumGroupConfirm: {
                show: function (optionsData) {
                    var deferred = $q.defer();
                    options = optionsData;
                    dialogOperations.initialise();
                    scope.name = options.name;
                    dialogOperations.showWideModalDialog("Confirm Delete", './app/modals/enumerations/deleteGroupConfirm.tpl.html')
                            .then(function () {
                                deferred.resolve(scope.groupName);
                                dialogOperations.destroy();
                            }, function () {
                                dialogOperations.destroy();
                            });
                    return deferred.promise;
                }
            },
            deleteEnumConfirm: {
                show: function (optionsData) {
                    var deferred = $q.defer();
                    options = optionsData;
                    dialogOperations.initialise();
                    scope.name = options.name;
                    dialogOperations.showWideModalDialog("Confirm Delete", './app/modals/enumerations/deleteEnumConfirm.tpl.html')
                            .then(function () {
                                deferred.resolve(scope.groupName);
                                dialogOperations.destroy();
                            }, function () {
                                dialogOperations.destroy();
                            });
                    return deferred.promise;
                }
            },
            deleteVariableGroupConfirm: {
                show: function (optionsData) {
                    var deferred = $q.defer();
                    options = optionsData;
                    dialogOperations.initialise();
                    scope.name = options.name;
                    dialogOperations.showWideModalDialog("Confirm Delete", './app/modals/variables/deleteGroupConfirm.tpl.html')
                            .then(function () {
                                deferred.resolve(scope.groupName);
                                dialogOperations.destroy();
                            }, function () {
                                dialogOperations.destroy();
                            });
                    return deferred.promise;
                }
            },
            saveAs: {
                show: function (optionsData) {
                    var deferred = $q.defer();
                    options = optionsData;
                    dialogOperations.initialise();
                    dialogOperations.showWideModalDialog("Save Document As", './app/modals/saveAs/saveAs.tpl.html')
                            .then(function () {
                                deferred.resolve(scope.newDocumentName);
                                dialogOperations.destroy();
                            }, function () {
                                dialogOperations.destroy();
                            });
                    return deferred.promise;
                }
            },
            unsavedChanges: {
                show: function () {
                    var deferred = $q.defer();

                    dialogOperations.initialise();
                    scope.shouldSave = true;
                    dialogOperations.showWideModalDialog("Save Document", './app/modals/unsavedChanges/unsavedChanges.tpl.html')
                            .then(function () {
                                deferred.resolve(scope.shouldSave);
                                dialogOperations.destroy();
                            }, function () {
                                deferred.reject();
                                dialogOperations.destroy();
                            });
                    return deferred.promise;
                }
            },
            publishConfirm: {
                show: function (optionsData) {
                    var deferred = $q.defer();
                    options = optionsData;
                    dialogOperations.initialise();
                    scope.name = options.name;
                    dialogOperations.showWideModalDialog("Publish Document", './app/modals/design/publish.tpl.html')
                            .then(function () {
                                deferred.resolve();
                                dialogOperations.destroy();
                            }, function () {
                                deferred.reject();
                                dialogOperations.destroy();
                            });
                    return deferred.promise;
                }
            },
            documentUnlocked: {
                show: function () {
                    var deferred = $q.defer();
                    dialogOperations.initialise();
                    dialogOperations.showWideModalDialog("Document Unlocked", './app/modals/design/unlocked.tpl.html')
                            .then(function () {
                                deferred.resolve();
                                dialogOperations.destroy();
                            }, function () {
                                deferred.reject();
                                dialogOperations.destroy();
                            });
                    return deferred.promise;
                }
            },
            savePublishedConfirm: {
                show: function (optionsData) {
                    var deferred = $q.defer();
                    options = optionsData;
                    dialogOperations.initialise();
                    scope.name = options.name;
                    dialogOperations.showWideModalDialog("Save Document", './app/modals/design/savePublished.tpl.html')
                            .then(function () {
                                deferred.resolve();
                                dialogOperations.destroy();
                            }, function () {
                                deferred.reject();
                                dialogOperations.destroy();
                            });
                    return deferred.promise;
                }
            },
        
 
            addTestingOutputVariable: {
                show: function (outputVariables, expectationTypes, testingVariables, optionsData) {
                    var deferred = $q.defer();
                    options = optionsData;
                    dialogOperations.initialise();
                    scope.outputVariables = outputVariables;
                    scope.expectationTypes = expectationTypes;
                    scope.selectedOutputVariable = scope.outputVariables[0];
                    scope.testingVariables = testingVariables;
                    scope.boolSelect = [true, false];
                    dialogOperations.showWideModalDialog('Select an output variable', './app/modals/test/addTestOutputVariable.tpl.html')
                        .then(function() {
                            deferred.resolve(scope.selectedOutputVariable);
                            dialogOperations.destroy();
                        }, function() {
                            deferred.reject();
                            dialogOperations.destroy();
                        }
                    );
                    return deferred.promise;
                }
            },
            addTestingVariable: {
                show: function (variable, optionsData) {
                    var deferred = $q.defer();
                    options = optionsData;
                    dialogOperations.initialise();
                    scope.selectedOutputVariable = variable;
                    scope.boolSelect = $rootScope.boolSelect;
                    scope.globalData = $rootScope.globalData;
                    scope.changeVariable = function () {
                        if (!$rootScope.globalData.isVariableAnEnum(scope.selectedOutputVariable)) {
                            scope.selectedOutputVariable.value = ''
                        }
                    }
                    dialogOperations.showWideModalDialog('Create a test variable', './app/modals/test/addTestingVariable.tpl.html')
                        .then(function() {
                            deferred.resolve(scope.selectedOutputVariable);
                            dialogOperations.destroy();
                        }, function() {
                            deferred.reject();
                            dialogOperations.destroy();
                        }
                    );
                    return deferred.promise;
                }
            },
            addSubtreeExpectation: {
                show: function(availableSubtrees){
                    var deferred = $q.defer();
                    dialogOperations.initialise();
                    scope.availableSubtrees = availableSubtrees;
                    scope.selectedSubtree = scope.availableSubtrees[0];
                    dialogOperations.showWideModalDialog('Select an subtree', './app/modals/test/addSubtreeExpectation.tpl.html')
                        .then(function() {
                            deferred.resolve(scope.selectedSubtree);
                            dialogOperations.destroy();
                        }, function() {
                            deferred.reject();
                            dialogOperations.destroy();
                        }
                    );
                    return deferred.promise;
                }
            },
            addTestingOutputMessage: {
                show: function(outputMessages) {
                    var deferred = $q.defer();
                    dialogOperations.initialise();
                    scope.outputMessages = outputMessages;
                    scope.selectedOutputMessage = scope.outputMessages[0];
                    dialogOperations.showWideModalDialog('Select a message', './app/modals/test/addTestOutputMessage.tpl.html')
                        .then(function() {
                            deferred.resolve(scope.selectedOutputMessage);
                            dialogOperations.destroy();
                        }, function() {
                            deferred.reject();
                            dialogOperations.destroy();
                        }
                    );
                    return deferred.promise;
                }
            },
            addTestingInputVariable: {
                show: function(inputVariables) {
                    var deferred = $q.defer();
                    dialogOperations.initialise();
                    scope.inputVariables = inputVariables;
                    scope.selectedInputVariable = scope.inputVariables[0];
                    dialogOperations.showWideModalDialog('Select an input variable', './app/modals/test/addTestInputVariable.tpl.html')
                        .then(function() {
                            deferred.resolve(scope.selectedInputVariable);
                            dialogOperations.destroy();
                        }, function() {
                            deferred.reject();
                            dialogOperations.destroy();
                        }
                    );
                    return deferred.promise;
                }
            },
            deleteTestScenarioConfirm: {
                show: function(scenarioName) {
                    var deferred = $q.defer();
                    dialogOperations.initialise();
                    scope.scenarioName = scenarioName;
                     dialogOperations.showWideModalDialog('Confirm Delete', './app/modals/test/deleteTestScenarioConfirm.tpl.html')
                        .then(function() {
                            deferred.resolve();
                            dialogOperations.destroy();
                        }, function() {
                            deferred.reject();
                            dialogOperations.destroy();
                        }
                    );
                    return deferred.promise;
                }
            },
            deleteTestStoryConfirm: {
                show: function(testStoryName) {
                    var deferred = $q.defer();
                    dialogOperations.initialise();
                    scope.testStoryName = testStoryName;
                    dialogOperations.showWideModalDialog('Confirm Delete', './app/modals/test/deleteTestStoryConfirm.tpl.html')
                       .then(function() {
                           deferred.resolve();
                           dialogOperations.destroy();
                       }, function() {
                           deferred.reject();
                           dialogOperations.destroy();
                       }
                   );
                    return deferred.promise;
                }
            },
            addTestStory: {
                show: function() {
                    var deferred = $q.defer();
                    dialogOperations.initialise();
                    scope.newName = '';
                     dialogOperations.showWideModalDialog('Create Test Story', './app/modals/test/addTestStory.tpl.html')
                        .then(function() {
                            if(scope.newName.substring(0, 4)!=='When' && scope.newName.substring(0, 4)!=='when'){
                                scope.newName = 'when ' + scope.newName;
                            }
                            deferred.resolve(scope.newName);
                            dialogOperations.destroy();
                        }, function() {
                            deferred.reject();
                            dialogOperations.destroy();
                        }
                    );
                    return deferred.promise;
                }
            },
            addTestScenario: {
                show: function() {
                    var deferred = $q.defer();
                    dialogOperations.initialise();
                    scope.newName = '';
                     dialogOperations.showWideModalDialog('Create Scenario', './app/modals/test/addTestScenario.tpl.html')
                        .then(function() {
                            if(scope.newName.substring(0, 5)!=='Given' && scope.newName.substring(0, 5)!=='given'){
                                scope.newName = 'given ' + scope.newName;
                            }
                            deferred.resolve(scope.newName);
                            dialogOperations.destroy();
                        }, function() {
                            deferred.reject();
                            dialogOperations.destroy();
                        }
                    );
                    return deferred.promise;
                }
            },
            warnOfSessionTimeout: {
                show: function() {
                    var deferred = $q.defer();

                    dialogOperations.initialise();
                    
                    var confirmTimeout = setTimeout(function () {
                        if (scope) {
                            scope.cancelDialog();
                            deferred.reject();
                            dialogOperations.destroy();
                        }
                    }, 10 * 60 * 1000);

                    dialogOperations.showWideModalDialog("Session Timeout", './app/modals/design/warnOfSessionTimeout.tpl.html')
                        .then(function() {
                            clearTimeout(confirmTimeout);
                            deferred.resolve();
                            dialogOperations.destroy();
                        }, function() {
                            clearTimeout(confirmTimeout);
                            deferred.reject();
                            dialogOperations.destroy();
                        });

                    return deferred.promise;
                }
            },
            selectSubTree: {
                show: function() {
                    var deferred = $q.defer();

                    dialogOperations.initialise();
                    scope.selectedSubTree = {};
                    scope.availableSubTrees = [];
                    scope.formatSubtree = function(name, version) {
                        return name + ' (v.' + version + ')';
                    }
                    var query = new $decisionTreeDesignQueries.GetLatestDecisionTreesQuery();
                    $queryManager.sendQueryAsync(query).then(function(data) {
                        data.data.ReturnData.Results.$values.map(function(item) {
                            scope.availableSubTrees.push({
                                name:item.Name,
                                version: item.ThisVersion
                            });
                        });
                        scope.selectedSubTree = scope.availableSubTrees[0];

                        dialogOperations.showWideModalDialog("Select a Sub Tree", './app/modals/selectSubTree/selectSubTree.tpl.html')
                            .then(function() {
                                deferred.resolve(scope.selectedSubTree);
                                dialogOperations.destroy();
                            }, function () {
                                deferred.reject();
                                dialogOperations.destroy();
                            }
                        );
                    });
                    return deferred.promise;
                }
            },
            invalidDebugVariables: {
                show: function() {
                    var deferred = $q.defer();
                    dialogOperations.initialise();
                    dialogOperations.showWideModalDialog('Debug Variables Not Set', './app/modals/debug/invalidDebugVariables.tpl.html')
                       .then(function() {
                           deferred.resolve();
                           dialogOperations.destroy();
                       });
                    return deferred.promise;
                }
            },debugInitilizing: {
                show: function () {
                    var deferred = $q.defer();
                    dialogOperations.initialise();
                    dialogOperations.showWideModalDialog('Debug Service Initilizing', './app/modals/debug/debuggerInitilizing.tpl.html')
                       .then(function () {
                           deferred.resolve();
                           dialogOperations.destroy();
                       });
                    return deferred.promise;
                }
            }
        }

        return {
            dialogs: dialogs
        }
    }]);