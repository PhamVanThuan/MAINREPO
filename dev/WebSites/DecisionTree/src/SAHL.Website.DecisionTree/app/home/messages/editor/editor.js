'use strict';

angular.module('sahl.tools.app.home.messages.editor', [
  'ui.router',
])

.config(function config($stateProvider) {
    $stateProvider.state('home.messages.editor', {
        url: "/editor",
        templateUrl: "./app/home/messages/editor/editor.tpl.html",
        controller: "MessagesEditorCtrl",
    });
})

.controller('MessagesEditorCtrl', ['$rootScope', '$scope', '$state', 'messageDataManager', 'filterFilter', '$activityManager', '$notificationService', '$modalDialogManager', '$q', '$jsonPropertyValidator', '$eventAggregatorService', '$eventDefinitions',
    function MessagesController($rootScope, $scope, $state, messageDataManager, filterFilter, $activityManager, $notificationService, $modalDialogManager, $q, $jsonPropertyValidator, $eventAggregatorService, $eventDefinitions) {
    $scope.breadcrumbs = new Array();
    $scope.latestVersion = null;
    $scope.originalData = null;

    $scope.SelectGroupInContext = function (index) {

        var currentGroup = $scope.currentContext.groups[index];

        if (currentGroup) {
            $scope.breadcrumbs.pop();
            $scope.breadcrumbs.push({ "index": index, "name": currentGroup.name });
        }
    }

    $scope.SelectSubGroupInContext = function (groupIndex, index) {
        $scope.currentContext = $scope.currentContext.groups[groupIndex];
        $scope.breadcrumbs.push({ "index": index, "name": $scope.currentContext.groups[index].name });
    }

    $scope.GoToGroupItem = function (index) {
        var data = $scope.data;
        var c = index + 1;
        $scope.breadcrumbs.splice(c, $scope.breadcrumbs.length - c);
        for (var i = 1; i < c; i++) {
            data = data.groups[$scope.breadcrumbs[i - 1].index];
        }
        $scope.currentContext = data;
    }

    $scope.GetSelectedIndexInContext = function(){
        if ($scope.breadcrumbs.length > 0) {
            return $scope.breadcrumbs[$scope.breadcrumbs.length - 1].index;
        }
        return -1;
    }

    $scope.validateMessageGroup = function (groupName) {
        if (groupName.length === 0) {
            return false;
        }

        if (!$jsonPropertyValidator.isValidName(groupName)) {
            return false;
        };

        var groupOccurences = filterFilter($scope.filterContext.groups, function (itemName) {
            return itemName.name.toLowerCase() === groupName.toLowerCase()
        });
        if (groupOccurences.length === 0) {
            return true;
        }
        return false;
    }

    $scope.addNewGroup = function () {
        if ($scope.messageSet.$valid) {
            $scope.filterContext = $scope.currentContext;
            $modalDialogManager.dialogs.addMessageGroup.show({ title: "Create Message Group", validate: $scope.validateMessageGroup }).then(function (newGroupName) {
                var selectIndex = saveNewGroupForContext($scope.currentContext, newGroupName)
                $scope.SelectGroupInContext(selectIndex);
                delete $scope.filterContext;
            });
        } else {
            invalidData();
        }
    }

    $scope.addNewSubGroup = function (groupIndex) {
        if ($scope.messageSet.$valid) {
            $scope.filterContext = $scope.currentContext.groups[groupIndex];
            $modalDialogManager.dialogs.addMessageGroup.show({ title: "Create Message Sub Group", validate: $scope.validateMessageGroup }).then(function (newGroupName) {
                var selectIndex = saveNewGroupForContext($scope.currentContext.groups[groupIndex], newGroupName);
                $scope.SelectSubGroupInContext(groupIndex, selectIndex);
                delete $scope.filterContext;
            });
        } else {
            invalidData();
        }
    }

    var saveNewGroupForContext = function (dataContext, newGroupName) {
        dataContext.$lastGroupId += 1;
        return dataContext.groups.push(generateGroup(dataContext.$lastGroupId, newGroupName)) - 1;
    }

    var generateGroup = function (id, name) {
        $scope.changed = true;
        return { "id": id, "name": name, "groups": [], "messages": [], "$lastGroupId": 0, "$lastMessageId": 0 };
    }

    $scope.deleteGroup = function (index) {
        $modalDialogManager.dialogs.deleteMessageGroupConfirm.show({ name: $scope.currentContext.groups[index].name }).then(function () {
            deleteGroup($scope.currentContext, index);
            $scope.SelectGroupInContext(0);
        });
    }

    $scope.deleteSubGroup = function (groupIndex, index) {
        $modalDialogManager.dialogs.deleteMessageGroupConfirm.show({ name: $scope.currentContext.groups[groupIndex].groups[index].name }).then(function () {
            deleteGroup($scope.currentContext.groups[groupIndex], index);
        });
    }

    var deleteGroup = function (currentGroup, index) {
        $scope.changed = true;
        currentGroup.groups.splice(index, 1);
    }

    $scope.addNewMessage = function (groupIndex) {
        if ($scope.messageSet.$valid){
            $scope.currentContext.groups[groupIndex].$lastMessageId += 1;
            var index = $scope.currentContext.groups[groupIndex].messages.push(generateMessage($scope.currentContext.$lastMessageId)) - 1;
            $scope.$$postDigest(function () {
                document.getElementById($scope.getId(groupIndex, index)).focus();
            })
            if ($scope.GetSelectedIndexInContext() != groupIndex) {
                $scope.SelectGroupInContext(groupIndex);
            }
        } else {
            invalidData();
        }
    }

    function generateMessage(id) {
        $scope.changed = true;
        return { "id": id, "name": "", "value": "" };
    }

    $scope.deleteMessageItem = function (groupIndex, itemIndex) {
        $scope.currentContext.groups[groupIndex].messages.splice(itemIndex, 1);
    }

    $scope.validateMessageItem = function (groupIndex, itemIndex) {
        $.debounce(500, false, validateInputs(groupIndex, itemIndex));
    }

    var validateInputs = function (groupIndex, itemIndex) {
        var element = angular.element(document.getElementById($scope.getId(groupIndex, itemIndex)));
        var val = $scope.currentContext.groups[groupIndex].messages[itemIndex].name;

        if (!val) {
            return;
        }

        if (!$jsonPropertyValidator.isValidName(val)) {
            element.addClass("ng-invalid");
            invalidName("Message Name '" + val + "' is invalid. It can only contain alphanumeric characters.");
            return;
        }
        else {
            element.removeClass("ng-invalid");
        }

        var groupOccurences = filterFilter($scope.currentContext.groups[groupIndex].messages, function (item) {
            if (item.name){
                return val.toLowerCase() === item.name.toLowerCase();
            }
        });

        if (groupOccurences.length > 1) {
            invalidName("Message Name '" + val + "' already exists in message group.");
            element.focus();
            $scope.noDuplicates = false;
            element.addClass("ng-invalid");
        } else {
            $scope.noDuplicates = true;
            element.removeClass("ng-invalid");
        }
    }

    $scope.getId = function (groupIndex, itemIndex) {
        return groupIndex + "_" + itemIndex;
    }

    $scope.save = function () {
        if ($scope.messageSet.$valid) {
            var jsonMessages = angular.toJson(messageDataManager.$shrink($scope.data));
            messageDataManager.SaveMessageSetCommandAsync($scope.Id, $scope.version, jsonMessages).then(function (data) {
                $eventAggregatorService.publish($eventDefinitions.onGlobalMessagesSaved, jsonMessages);
                $scope.load().then(function ()
                {
                    $notificationService.notifySuccess('Success', 'Messages have been saved.');
                });
            }, function (errorMessage) {
                $notificationService.notifyError('Fail', 'Error: ' + errorMessage);
            });
        } else {
            invalidData();
        }
    }

    $scope.publish = function () {
        if ($scope.messageSet.$valid) {
            var jsonMessages = angular.toJson(messageDataManager.$shrink($scope.data));
            messageDataManager.SaveAndPublishMessageSetCommandAsync($scope.Id, $scope.version, jsonMessages, $scope.$root.username).then(function (data) {
                $eventAggregatorService.publish($eventDefinitions.onGlobalMessagesSaved, jsonMessages);
                $scope.load().then(function () {
                    $notificationService.notifySuccess('Success', 'Message set saved and published.');
                });
            }, function (errorMessage) {
                $notificationService.notifyError('Fail', 'Error: ' + errorMessage);
            });
        } else {
            invalidData();
        }
    }

    $scope.close = function (onExitReadyFn) {
        if (!$scope.changesMade()) {
            onExitReadyFn();
            return;
        }

        if ($scope.messageSet.$valid) {
            $modalDialogManager.dialogs.unsavedChanges.show().then(function () {
                messageDataManager.SaveMessageSetCommandAsync($scope.Id, $scope.version, angular.toJson(messageDataManager.$shrink($scope.data))).then(function (data) {
                    $notificationService.notifySuccess('Save', 'Messages have been saved.');
                    onExitReadyFn();
                }, function (errorMessage) {
                    $notificationService.notifyError('Fail', 'Error: ' + errorMessage);
                });
            }, onExitReadyFn);

        } else {
            invalidData();
        }
    };

    $scope.SetData = function (data) {
        $scope.selectedIndex = 0;
        $scope.Id = data.data.ReturnData.Results.$values[0].Id;
        $scope.version = data.data.ReturnData.Results.$values[0].Version;
        $scope.IsPublished = data.data.ReturnData.Results.$values[0].IsPublished;
        $scope.Publisher = data.data.ReturnData.Results.$values[0].Publisher;
        $scope.originalData = data.data.ReturnData.Results.$values[0].Data;
        $scope.data = messageDataManager.$extend(angular.fromJson($scope.originalData));
        $scope.currentContext = $scope.data;
        $scope.SelectGroupInContext(0);
        $scope.changed = false;
        $activityManager.stopActivityWithKey('loadMessages');
        $scope.isLatestVersion = ($scope.latestVersion == $scope.version);
    };

    $scope.load = function () {
        var deferred = $q.defer();
        $activityManager.startActivityWithKey('loadMessages');

        messageDataManager.GetLatestMessageSetQueryAsync().then(function (data) {
            var latestMessageSet = data;
            if (latestMessageSet.data.ReturnData.Results.$values.length !== 1) {
                latestMessageSet.data.ReturnData.Results.$values = [{
                    "Id": "00000000-0000-0000-0000-000000000000",
                    "Version": 0,
                    "IsPublished": false,
                    "Publisher": "",
                    "Data": "{\"groups\" : []}",
                }];
            }

            var latestMessageSetId = latestMessageSet.data.ReturnData.Results.$values[0].Id;

            $scope.latestVersion = latestMessageSet.data.ReturnData.Results.$values[0].Version
            if ($scope.version != 0) {
                $scope.Parent_LockLatestVersion(latestMessageSetId);
            }

            if ($scope.LockedBy) {
                delete $scope.lockedByUser
            }
            if (latestMessageSetId != ($scope.SelectedMessageSetID || latestMessageSetId)) { // have we selected the latest version?
                messageDataManager.GetMessageSetByMessageSetIdQueryAsync($scope.SelectedMessageSetID).then(function (messageSetData) {
                    try{
                        $scope.SetData(messageSetData);
                    }
                    catch (e) {
                        $activityManager.stopActivityWithKey('loadMessages');
                    }
                    
                    $scope.Parent_ShowNavButtons(true, false, false, false);
                    deferred.resolve();
                });
            }
            else
            {
                $scope.SetData(latestMessageSet);
                if (latestMessageSet.data.ReturnData.Results.$values[0].LockedBy) {
                    var lockedByUser = latestMessageSet.data.ReturnData.Results.$values[0].LockedBy;
                    if (lockedByUser !== $rootScope.username) {
                        $scope.lockedByUser = lockedByUser;
                        $scope.Parent_ShowNavButtons(true, false, false, false);
                        deferred.resolve();
                        return;
                    }
                }
                $scope.Parent_ShowNavButtons(true, true, true, true);
                deferred.resolve();
            }
        });

        return deferred.promise;
    };

    var invalidData = function () {
        $notificationService.notifyError('Input Validation Error', '"Name", "Message" and "Message Definition" fields cannot be empty.');
    }

    var invalidName = function (message) {
        $notificationService.notifyError('Input Validation Error', message || 'Name field cannot be duplicated.');
    }

    $scope.changesMade = function () {
        return $scope.originalData != angular.toJson(messageDataManager.$shrink($scope.data));
    };

    $scope.Parent_RegisterChildFunctions($scope.save, $scope.publish, $scope.close);

    $scope.load();
}]);