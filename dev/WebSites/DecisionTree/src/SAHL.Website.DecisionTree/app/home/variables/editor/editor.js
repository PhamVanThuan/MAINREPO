'use strict';

angular.module('sahl.tools.app.home.variables.editor', [
  'ui.router'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.variables.editor', {
        url: "/editor",
        templateUrl: "./app/home/variables/editor/editor.tpl.html",
        controller: "VariablesEditorCtrl",
    });
})

.controller('VariablesEditorCtrl', ['$rootScope', '$scope', '$state', '$variableDataManager', 'filterFilter', '$enumerationDataManager', '$activityManager', '$modalDialogManager', '$notificationService', '$jsonPropertyValidator', '$q', '$eventAggregatorService', '$eventDefinitions',
function ($rootScope, $scope, $state, variableDataManager, filterFilter, enumerationDataManager, $activityManager, $modalDialogManager, $notificationService, $jsonPropertyValidator, $q, $eventAggregatorService, $eventDefinitions) {
    $scope.variableTypes = [];
    $scope.currentGroups = {};
    $scope.selectedItemId = 0;
    $scope.newGroupName = "";
    $scope.groupToRemove = "";
    $scope.newSubgroupParent;
    $scope.breadcrumbs = [];

    $scope.latestVersion = null;

    $scope.setSelectedItem = function (id) {
        $scope.selectedItemId = id;
    };

    $scope.addVariable = function (groupId, groupIndex) {
        var group = getGroupByGroupId($scope.variableGroups, groupId);
        var id = Number(getHighestVarId()) + 1;
        var index = group.variables.push(
        {
            id: id,
            type: "string",
            usage: "global",
            name: "",
            value: "",
            typeDisplay: $scope.variableTypes[0]
        }) - 1;
        $scope.$$postDigest(function () {
            document.getElementById($scope.getId(groupIndex, index)).focus();
        })
    };

    $scope.removeVariable = function (id, groupId) {
        var group = getGroupByGroupId($scope.variableGroups, groupId);
        var variableIndex = -1;
        for (var i = 0; i < group.variables.length; i++) {
            if (group.variables[i].id == id) {
                variableIndex = i;
                break;
            }
        }
        group.variables.splice(variableIndex, 1);
    };

    $scope.deleteGroup = function () {

        var parent = GetParentForGroup($scope.variableGroups, $scope.groupToRemove);

        var groupList = parent ? parent.groups : $scope.variableGroups;

        var indexInList = -1;
        for (var i = 0; i < groupList.length; i++) {
            if (groupList[i].id == $scope.groupToRemove) {
                indexInList = i;
            }
        }

        groupList.splice(indexInList, 1);

        $scope.gotoGroup(parent.id);
    };

    $scope.addGroup = function (parentGroupId) {
        $modalDialogManager.dialogs.addVariableGroup.show({ title: "Create Variable Group", validate: $scope.validateVariableGroup }).then(function (newGroupName) {
            var group = null;

            if (parentGroupId) {
                var parentGroup = getGroupByGroupId($scope.currentGroups, parentGroupId);
                if (!parentGroup.groups) {
                    parentGroup.groups = [];
                }
                group = parentGroup.groups;
            }
            else {
                group = $scope.currentGroups;
            }

            if (newGroupName) {
                var newId = Number(getHighestGroupID()) + 1;
                var newGroup = {
                    "id": newId,
                    "name": newGroupName,
                    "variables": [],
                    "groups": []
                };
                group.push(newGroup);

                newGroupName = '';
                $scope.selectedId = $scope.currentGroups[$scope.currentGroups.length - 1].id;
                $scope.gotoGroup(newId);
            }
        });
    };

    $scope.validateVariableGroup = function (groupName) {
        if (groupName.length === 0) {
            return false;
        }

        if (!$jsonPropertyValidator.isValidName(groupName)) {
            return false;
        };

        var groupOccurences = filterFilter($scope.data.variables.groups, function (itemName) {
            return itemName.name.toLowerCase() === groupName.toLowerCase()
        });
        if (groupOccurences.length === 0) {
            return true;
        }
        else {
            return false;
        }
    };

    $scope.removeGroup = function (groupId, $event) {
        $scope.groupToRemove = groupId;

        var groupSet = getGroupByGroupId($scope.variableGroups, groupId);

        $modalDialogManager.dialogs.deleteVariableGroupConfirm.show({ name: groupSet.name }).then(function () {
            $scope.deleteGroup();
        });
    };

    $scope.SetData = function (data) {
        $scope.Id = data.data.ReturnData.Results.$values[0].ID;
        $scope.version = data.data.ReturnData.Results.$values[0].Version;
        $scope.originalData = data.data.ReturnData.Results.$values[0].Data;
        $scope.data = angular.fromJson($scope.originalData);
        $scope.IsPublished = data.data.ReturnData.Results.$values[0].IsPublished;
        $scope.Publisher = data.data.ReturnData.Results.$values[0].Publisher;
        $scope.variableGroups = GetValuedVariableSetFromGroupSet($scope.data.variables.groups, $scope.data.variables.values);
        $scope.currentGroups = $scope.variableGroups;
        $scope.selectedItemId = $scope.currentGroups[0] ? $scope.currentGroups[0].id : -1;
        $activityManager.stopActivityWithKey('loadVariables');
        $scope.breadcrumbs = getCurrentBreadcrumbs();
        $scope.isLatestVersion = ($scope.latestVersion == $scope.version);
        $scope.$$phase || $scope.$apply();
    };

    $scope.load = function () {
        var deferred = $q.defer();

        $scope.variableTypes = [];
        SetupBasicTypes();
        $activityManager.startActivityWithKey('loadVariables');

        enumerationDataManager.GetLatestEnumerationSetQueryAsync().then(function (data) {
            if (data.data.ReturnData.Results.$values.length !== 1) {
                data.data.ReturnData.Results.$values = [{
                    "Id": "00000000-0000-0000-0000-000000000000",
                    "Version": 0,
                    "IsPublished": false,
                    "Publisher": "",
                    "Data": "{\"groups\" : []}",
                }];
            }

            var enums = enumerationDataManager.$extend(angular.fromJson(data.data.ReturnData.Results.$values[0].Data));

            SetupEnumTypes(enums);

            variableDataManager.GetLatestVariableSetQueryAsync().then(function (data) {
                var latestVariableSet = data;

                if (latestVariableSet.data.ReturnData.Results.$values.length !== 1) {
                    latestVariableSet.data.ReturnData.Results.$values = [{
                        "ID": "00000000-0000-0000-0000-000000000000",
                        "Version": 0,
                        "IsPublished": false,
                        "Publisher": "",
                        "Data": "{\"variables\":{\"groups\":[],\"values\":[]}}"
                    }];
                }
                var latestVariableSetSetId = latestVariableSet.data.ReturnData.Results.$values[0].ID;

                $scope.latestVersion = latestVariableSet.data.ReturnData.Results.$values[0].Version;

                if ($scope.latestVersion != 0) {
                    $scope.Parent_LockLatestVersion(latestVariableSetSetId);
                }
                if ($scope.LockedBy) {
                    delete $scope.lockedByUser
                }
                if (latestVariableSetSetId != ($scope.SelectedVariableSetID || latestVariableSetSetId)) { // have we selected the latest version or an older one?
                    variableDataManager.GetVariableSetByVariableSetIdQueryAsync($scope.SelectedVariableSetID).then(function (variableSetData) {
                        try{
                            $scope.SetData(variableSetData);
                        }
                        catch (e) {
                            $activityManager.stopActivityWithKey('loadVariables');
                        }
                    
                        $scope.Parent_ShowNavButtons(true, false, false, false);
                        deferred.resolve();
                    });
                }
                else
                {
                    $scope.SetData(latestVariableSet);
                    if (latestVariableSet.data.ReturnData.Results.$values[0].LockedBy) {
                        var lockedByUser = latestVariableSet.data.ReturnData.Results.$values[0].LockedBy;
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
        });
        return deferred.promise;
    };

    $scope.save = function () {
        if ($scope.variablesForm.$valid) {
            var variableSet = ParseVariableGroupsIntoJson();
            var jsonVariableSet = angular.toJson(variableSet);
            variableDataManager.SaveVariableSetCommandAsync($scope.Id, $scope.version, jsonVariableSet).then(function (data) {
                $eventAggregatorService.publish($eventDefinitions.onGlobalVariablesSaved, jsonVariableSet);
                $scope.load().then(function () {
                    $notificationService.notifySuccess('Saved', 'Variable changes have been saved.');
                });
            });
        }
        else {
            NotifyInvalidData();
        }
    };

    $scope.publish = function () {
        if ($scope.variablesForm.$valid) {
            var variableSet = ParseVariableGroupsIntoJson();
            var jsonVariableSet = angular.toJson(variableSet);
            variableDataManager.SaveAndPublishVariableSetCommandAsync($scope.Id, $scope.version, jsonVariableSet, $scope.$root.username).then(function (data) {
                $eventAggregatorService.publish($eventDefinitions.onGlobalVariablesSaved, jsonVariableSet);
                $scope.load();
                $notificationService.notifySuccess('Published', 'Variable set saved and published.');
            }, function () {
                $notificationService.notifyError('Fail', 'Error: ' + errorMessage);
            });
        }
        else {
            NotifyInvalidData();
        }
    };

    $scope.gotoGroup = function (id) {
        if ($scope.variablesForm.$valid) {
            var groupSet = getGroupSetByGroupId($scope.variableGroups, id);
            $scope.currentGroups = groupSet;
            $scope.selectedItemId = id;
            $scope.breadcrumbs = getCurrentBreadcrumbs();
        }
        else {
            NotifyInvalidData();
        }
    };

    $scope.getId = function (groupIndex, variableIndex) {
        return groupIndex + "_" + variableIndex;
    }

    $scope.validateVariableName = function (groupIndex, variableIndex) {
        var group = $scope.currentGroups[groupIndex];
        var val = group.variables[variableIndex].name;

        if (!val) {
            return;
        }

        var element = angular.element(document.getElementById($scope.getId(groupIndex, variableIndex)));

        if (!$jsonPropertyValidator.isValidName(val)) {
            element.addClass("ng-invalid");
            NotifyInvalidData("Variable Name '" + val + "' is invalid. It can only contain alphanumeric characters.");
            return;
        }
        else {
            element.removeClass("ng-invalid");
        }

        var groupOccurences = filterFilter(group.variables, function (variable) {
            return val.toLowerCase() === variable.name.toLowerCase();
        });

        if (groupOccurences.length > 1) {
            NotifyInvalidData("Variable Name '" + val + "' already exists in variable group.");
            element.focus();
            element.addClass("ng-invalid");
        } else {
            element.removeClass("ng-invalid");
        }
    };

    $scope.changesMade = function () {
        var variableSet = ParseVariableGroupsIntoJson();
        var data = angular.toJson(variableSet);
        return $scope.originalData != data;
    }

    $scope.close = function (onExitReady) {
        if (!$scope.changesMade()) {
            onExitReady();
            return;
        }

        if ($scope.variablesForm.$valid) {
            $modalDialogManager.dialogs.unsavedChanges.show().then(function () {
                var variableSet = ParseVariableGroupsIntoJson();
                variableDataManager.SaveVariableSetCommandAsync($scope.Id, $scope.version, angular.toJson(variableSet)).then(function (data) {
                    $notificationService.notifySuccess('Saved', 'Variable changes have been saved.');
                    onExitReady();
                });
            }, onExitReady);
        }
        else {
            NotifyInvalidData();
        }
    }

    function getGroupSetByGroupId(groupSet, id) {
        for (var i = 0; i < groupSet.length; i++) {
            var group = groupSet[i];
            if (group.id == id) {
                return groupSet;
            }
            else if (group.groups) {
                var childGroupSet = getGroupSetByGroupId(group.groups, id);
                if (childGroupSet) {
                    return childGroupSet;
                }
            }
        }
    }

    function getGroupByGroupId(groupSet, id) {
        var foundGroup;
        if (!groupSet) {
            groupSet = $scope.variableGroups;
        }
        for (var i = 0; i < groupSet.length; i++) {
            if (groupSet[i].id == id) {
                foundGroup = groupSet[i];
                break;
            }
            if (groupSet[i].groups && groupSet[i].groups.length > 0) {
                var fromChildren = getGroupByGroupId(groupSet[i].groups, id);
                if (fromChildren) {
                    foundGroup = fromChildren;
                    break;
                }
            }
        }

        return foundGroup;
    }

    function getCurrentBreadcrumbs() {
        var crumbs = [];
        var done = false;
        var currId = $scope.selectedItemId;

        var group = getGroupByGroupId($scope.variableGroups, currId);

        if (group) {
            crumbs.push({
                id: group.id,
                name: group.name
            });
        }

        while (done !== true) {
            var parent = GetParentForGroup($scope.variableGroups, currId);
            if (parent) {
                currId = parent.id;
                var crumb = {
                    id: parent.id,
                    name: parent.name
                };
                crumbs.push(crumb);
            }
            else {
                done = true;
            }
        }
        return crumbs.reverse();
    }

    function GetParentForGroup(groupSet, groupId) {
        for (var i = 0; i < groupSet.length; i++) {
            if (groupSet[i].groups) {
                for (var j = 0; j < groupSet[i].groups.length; j++) {
                    if (groupSet[i].groups[j].id == groupId) {
                        return groupSet[i];
                    }
                    else {
                        var result = GetParentForGroup(groupSet[i].groups, groupId);
                        if (result) {
                            return result;
                        }
                    }
                }
            }
        }
    }

    function GetVariableTypeByID(ID) {
        for (var i = 0; i < $scope.variableTypes.length; i++) {
            if ($scope.variableTypes[i].id == ID) {
                return $scope.variableTypes[i];
            }
        }
    }

    function GetVariableValueByID(varID, values) {
        for (var i = 0; i < values.length; i++) {
            if (values[i].variableId === varID) {
                return values[i].value;
            }
        }
    };

    function SetupBasicTypes() {
        var variableTypes = variableDataManager.GetBasicTypes();
        for (var i = 0; i < variableTypes.length; i++) {
            var typePattern = /.*/;
            if (variableTypes[i] === 'double') typePattern = /^\d+(\.\d+)?$/;
            if (variableTypes[i] === 'float') typePattern = /^\d+(\.\d+)?$/;
            if (variableTypes[i] === 'int') typePattern = /^\d+$/;
            var varType = {
                id: variableTypes[i],
                name: variableTypes[i],
                type: variableTypes[i],
                pattern: typePattern
            };
            if (varType.name == 'bool') {
                varType.values = [true,false];
            }
            $scope.variableTypes.push(varType);
        }
    }

    function SetupEnumTypes(enums) {
        if (enums) {
            for (var i = 0, c = enums.groups.length; i < c; i++) {
                if (enums.groups[i]["enumerations"]) {
                    for (var ii = 0, cc = enums.groups[i].enumerations.length; ii < cc; ii++) {
                        var enumToUse = enums.groups[i].enumerations[ii];
                        $scope.variableTypes.push({
                            id: enumToUse.$getId(),
                            name: enumToUse.$getFullName(),
                            type: 'enumeration',
                            pattern: /.*/,
                            values: enumToUse.value,
                        });
                    }
                }
                if (enums.groups[i]["groups"]) {
                    SetupEnumTypes(enums.groups[i]);
                }
            }
        }
    }

    function GetValuedVariableSetFromGroupSet(groupSet, values) {
        var parsedGroupSet = [];
        for (var i = 0; i < groupSet.length; i++) {
            var parsedGroup = {};
            var variables = [];
            for (var j = 0; (groupSet[i].variables && j < groupSet[i].variables.length) ; j++) {
                var currVariable = groupSet[i].variables[j];
                var value = GetVariableValueByID(currVariable.id, values);
                if (value) {
                    currVariable.value = value;
                }
                else {
                    currVariable.value = "";
                }
                if (currVariable.type === "enumeration") {
                    currVariable.typeDisplay = GetVariableTypeByID(currVariable.enumeration_group_id);
                }
                else {
                    currVariable.typeDisplay = GetVariableTypeByID(currVariable.type);
                }
                variables.push(currVariable);
            }
            parsedGroup = {
                id: groupSet[i].id,
                name: groupSet[i].name,
                variables: variables
            };
            if (groupSet[i].groups) {
                parsedGroup.groups = GetValuedVariableSetFromGroupSet(groupSet[i].groups, values);
            }
            parsedGroupSet.push(parsedGroup);
        }
        return parsedGroupSet;
    }

    function ParseVariableGroupsIntoJson() {
        var parsedGroups = [];
        var values = [];
        for (var i = 0; i < $scope.variableGroups.length; i++) {
            var parseResult = ParseGroupIntoJson($scope.variableGroups[i]);
            values = values.concat(parseResult.values);
            parsedGroups.push(parseResult.group);
        }
        var result = {
            variables: {
                groups: parsedGroups,
                values: values
            }
        };
        return result;
    }

    function ParseGroupIntoJson(group) {
        var parsedGroup = {};
        var variables = [];
        var childGroups = [];
        var values = [];

        // Get the variables
        for (var i = 0; i < group.variables.length; i++) {
            var variable = group.variables[i];

            var newVar = {
                "id": variable.id,
                "name": variable.name,
                "usage": variable.usage,
                "type": variable.typeDisplay.type,
                "definition": variable.definition
            };
            if (newVar.type === "enumeration") {
                newVar.enumeration_group_id = variable.typeDisplay.id;
                newVar.fullName = variable.typeDisplay.name;
            }
            variables.push(newVar);

            //Get the variable value
            var varValue = variable.value;
            if (!varValue) {
                varValue = variableDataManager.GetDefaultValueForType(variable.typeDisplay.type);

            }
            values.push({
                "variableId": variable.id,
                "value": varValue
            });
        }

        // Get the groups
        if (group.groups) {
            for (var i = 0; i < group.groups.length; i++) {
                var childGroupResult = ParseGroupIntoJson(group.groups[i]);
                values = values.concat(childGroupResult.values);
                childGroups.push(childGroupResult.group);
            }
        }
        parsedGroup.id = group.id;
        parsedGroup.name = group.name;
        if (variables.length > 0) { parsedGroup.variables = variables; }
        if (childGroups.length > 0) { parsedGroup.groups = childGroups; }

        return {
            group: parsedGroup,
            values: values
        };
    }

    function getHighestVarId() {
        return getHighestVarIdInGroupSet($scope.variableGroups);
    }

    function getHighestVarIdInGroupSet(groupSet) {
        var highestID = 0;
        for (var i = 0; i < groupSet.length; i++) {
            if (groupSet[i].variables) {
                for (var j = 0; j < groupSet[i].variables.length; j++) {
                    if (Number(groupSet[i].variables[j].id) > Number(highestID)) {
                        highestID = Number(groupSet[i].variables[j].id);
                    }
                }
            }
            if (groupSet[i].groups) {
                var childrenID = getHighestVarIdInGroupSet(groupSet[i].groups);
                if (childrenID > highestID) {
                    highestID = childrenID;
                }
            }
        }
        return highestID;
    }

    function getHighestGroupID() {
        return getHighestIdInGroupSet($scope.variableGroups);
    }

    function getHighestIdInGroupSet(groupSet) {
        var highestID = 0;
        for (var i = 0; i < groupSet.length; i++) {
            if (groupSet[i].id > highestID) {
                highestID = groupSet[i].id;
            }
            if (groupSet[i].groups) {
                var childrenID = getHighestIdInGroupSet(groupSet[i].groups);
                if (childrenID > highestID) {
                    highestID = childrenID;
                }
            }
        }
        return highestID;
    }

    function NotifyInvalidData(message) {
        $notificationService.notifyError('Error', message || 'There are validation errors on the page. Please correct them.');
    }

    $scope.Parent_ShowNavButtons(true, true, true, true);

    $scope.Parent_RegisterChildFunctions($scope.save, $scope.publish, $scope.close);

    $scope.load();
}]);