'use strict';

angular.module('sahl.tools.app.home.enumerations.editor', [
  'ui.router',
  'sahl.tools.app.services.modalDialogManager',
  'sahl.tools.app.directives'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.enumerations.editor', {
        url: "/editor",
        templateUrl: "./app/home/enumerations/editor/editor.tpl.html",
        controller: "EnumerationsEditorCtrl",
    });
})
.controller('EnumerationsEditorCtrl', ['$rootScope', '$scope', '$state', '$enumerationDataManager', '$modalDialogManager', '$notificationService', 'filterFilter', '$activityManager', '$q', '$jsonPropertyValidator', '$eventAggregatorService', '$eventDefinitions', 
    function ($rootScope, $scope, $state, enumerationDataManager, $modalDialogManager, $notificationService, filterFilter, $activityManager, $q, $jsonPropertyValidator, $eventAggregatorService, $eventDefinitions) {
    $scope.breadcrumbs = new Array();
    $scope.noDuplicates = true;

    $scope.latestVersion = null;

    var invalidData = function () {
        $notificationService.notifyError('Input Validation Error', 'Enumeration fields cannot be empty or duplicated. Please correct all errors before moving on.');
    }

    var invalidEnumerationValue = function () {
        $notificationService.notifyError('Input Validation Error', 'Enumeration values must contain alphanumeric . Please correct errors before moving on.');
    };

    $scope.SelectGroupInContext = function (index) {
        $scope.breadcrumbs.pop();
        if ($scope.currentContext.groups.length > 0) {
            $scope.breadcrumbs.push({ "index": index, "name": $scope.currentContext.groups[index].name });
        }
    }

    $scope.SelectSubGroupInContext = function (groupIndex, index) {
        if ($scope.enumerationsForm.$valid && $scope.noDuplicates) {
            $scope.currentContext = $scope.currentContext.groups[groupIndex];
            $scope.breadcrumbs.push({ "index": index, "name": $scope.currentContext.groups[index].name });
            $scope.refreshAccordian();
        } else {
            invalidData();
        }
    }

    $scope.GoToGroupItem = function (index) {
        if ($scope.enumerationsForm.$valid && $scope.noDuplicates) {
            var data = $scope.data;
            var c = index + 1;
            $scope.breadcrumbs.splice(c, $scope.breadcrumbs.length - c);
            for (var i = 1; i < c; i++) {
                data = data.groups[$scope.breadcrumbs[i - 1].index];
            }
            $scope.currentContext = data;
            $scope.refreshAccordian();
        } else {
            invalidData();
        }
    }

    $scope.validateEnumGroup = function (groupName) {
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

    $scope.GetSelectedIndexInContext = function () {
        if ($scope.breadcrumbs.length > 0) {
            return $scope.breadcrumbs[$scope.breadcrumbs.length - 1].index;
        }
        return -1;
    }

    $scope.addNewGroup = function () {
        $scope.filterContext = $scope.currentContext;
        $modalDialogManager.dialogs.addEnumGroup.show({ title: "Create Enumeration Group", validate: $scope.validateEnumGroup }).then(function (newGroupName) {
            var selectIndex = saveNewGroupForContext($scope.currentContext, newGroupName)
            $scope.SelectGroupInContext(selectIndex);
            delete $scope.filterContext;
        });
    }

    $scope.addNewSubGroup = function (groupIndex) {
        if ($scope.enumerationsForm.$valid && $scope.noDuplicates) {
            $scope.filterContext = $scope.currentContext.groups[groupIndex];
            $modalDialogManager.dialogs.addEnumGroup.show({ title: "Create Enumeration Sub Group", validate: $scope.validateEnumGroup }).then(function (newGroupName) {
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
        return { "id": id, "name": name, "groups": [], "enumerations": [], "$lastGroupId": 0, "$lastEnumId": 0 };
    }

    $scope.deleteGroup = function (index) {
        $modalDialogManager.dialogs.deleteEnumGroupConfirm.show({ name: $scope.currentContext.groups[index].name }).then(function () {
            deleteGroup($scope.currentContext, index);
            $scope.SelectGroupInContext(0);
        });
    }

    $scope.deleteSubGroup = function (groupIndex, index) {
        $modalDialogManager.dialogs.deleteEnumGroupConfirm.show({ name: $scope.currentContext.groups[groupIndex].groups[index].name }).then(function () {
            deleteGroup($scope.currentContext.groups[groupIndex], index);
        });
    }

    var deleteGroup = function (currentGroup, index) {
        $scope.changed = true;
        currentGroup.groups.splice(index, 1);
    }

    var validateEnumItemName = function (enumItemName) {
        if (enumItemName.length === 0) {
            return false;
        }

        if (!$jsonPropertyValidator.isValidName(enumItemName)) {
            invalidEnumerationValue();
            return false;
        };

        var groupOccurences = filterFilter($scope.filterEnumContext, function (itemName) {
            return itemName.name.toLowerCase() === enumItemName.toLowerCase()
        });
        if (groupOccurences.length === 0) {
            return true;
        }
        return false;
    }

    $scope.addNewEnum = function (groupIndex) {
        $scope.filterEnumContext = $scope.currentContext.groups[groupIndex].enumerations;
        $modalDialogManager.dialogs.addEnum.show({ validate: validateEnumItemName }).then(function (enumName) {
            delete $scope.filterEnumContext
            $scope.currentContext.groups[groupIndex].$lastEnumId += 1;
            $scope.currentContext.groups[groupIndex].enumerations.push(generateEnum($scope.currentContext.$lastEnumId, enumName));
            if ($scope.GetSelectedIndexInContext() != groupIndex) {
                $scope.SelectGroupInContext(groupIndex);
            }
            $scope.refreshAccordian();
        });
    }

    var generateEnum = function (id, name) {
        $scope.changed = true;
        return { "id": id, "name": name, "value": [] };
    }

    $scope.addNewEnumItem = function (groupIndex, enumIndex) {
        $scope.changed = true;
        var index = $scope.currentContext.groups[groupIndex].enumerations[enumIndex].value.push("") - 1;
        $scope.$$postDigest(function () {
            document.getElementById($scope.getId(groupIndex, enumIndex, index)).focus();
        })
    }

    $scope.deleteEnum = function (groupIndex, index) {
        $modalDialogManager.dialogs.deleteEnumGroupConfirm.show({ name: $scope.currentContext.groups[groupIndex].enumerations[index].name }).then(function () {
            deleteEnum($scope.currentContext.groups[groupIndex], index);
        });
    }

    var deleteEnum = function (currentGroup, index) {
        $scope.changed = true;
        currentGroup.enumerations.splice(index, 1);
    }

    $scope.deleteEnumItem = function (groupIndex, enumIdex, itemIndex) {
        $scope.changed = true;
        $scope.currentContext.groups[groupIndex].enumerations[enumIdex].value.splice(itemIndex, 1);
    }

    $scope.validateEnumItem = function (groupIndex, enumIdex, itemIndex) {
        $.debounce(500, false, validateInputs(groupIndex, enumIdex, itemIndex));
    }

    var validateInputs = function (groupIndex, enumIdex, itemIndex) {
        var element = angular.element(document.getElementById($scope.getId(groupIndex, enumIdex, itemIndex)));
        var val = $scope.currentContext.groups[groupIndex].enumerations[enumIdex].value[itemIndex];
        if (val) {

            if (!$jsonPropertyValidator.isValidName(val)) {
                element.addClass("ng-invalid");
                return;
            }
            else {
                element.removeClass("ng-invalid");
            }

            var groupOccurences = filterFilter($scope.currentContext.groups[groupIndex].enumerations[enumIdex].value, function (item) {
                return val.toLowerCase() === item.toLowerCase()
            });
            if (groupOccurences.length > 1) {
                invalidData();
                element.focus();
                $scope.noDuplicates = false;
                element.addClass("ng-invalid");
            } else {
                $scope.noDuplicates = true;
                element.removeClass("ng-invalid");
            }
        }
        else {
            invalidData();
        }
    }

    $scope.getId = function (groupIndex, enumIdex, itemIndex) {
        return groupIndex + "_" + enumIdex + "_" + itemIndex;
    }

    $scope.SetData = function (data) {
        $scope.selectedIndex = 0;
        $scope.Id = data.data.ReturnData.Results.$values[0].Id;
        $scope.version = data.data.ReturnData.Results.$values[0].Version;
        $scope.IsPublished = data.data.ReturnData.Results.$values[0].IsPublished;
        $scope.Publisher = data.data.ReturnData.Results.$values[0].Publisher;
        $scope.data = enumerationDataManager.$extend(angular.fromJson(data.data.ReturnData.Results.$values[0].Data));
        $scope.currentContext = $scope.data;
        $scope.SelectGroupInContext(0);
        $activityManager.stopActivityWithKey('loadEnumerations');
        $scope.changed = false;
        $scope.isLatestVersion = ($scope.latestVersion == $scope.version);
        $scope.refreshAccordian();
    }

    $scope.refreshAccordian = function () {
        $scope.$$postDigest(function () {
            $(".accordion").each(function (index, element) {
                var accord = $(element)
                accord.accordion({
                    closeAny: false, //true or false. if true other frames (when current opened) will be closed
                    open: function (frame) { }, // when current frame opened this function will be fired
                    action: function (frame) { } // when any frame opened this function will be fired
                });
            });
        });
    }

    $scope.load = function () {
        var deferred = $q.defer();
        $activityManager.startActivityWithKey('loadEnumerations');
        enumerationDataManager.GetLatestEnumerationSetQueryAsync().then(function (data) {

            var latestEnumerationSet = data;
            var latestEnumerationSetId = 0;
            $scope.latestVersion = 0;
            if (latestEnumerationSet.data.ReturnData.Results.$values.length === 1) {
                latestEnumerationSetId = latestEnumerationSet.data.ReturnData.Results.$values[0].Id;
                $scope.latestVersion = latestEnumerationSet.data.ReturnData.Results.$values[0].Version;
                $scope.Parent_LockLatestVersion(latestEnumerationSet.data.ReturnData.Results.$values[0].Id);

            } else {
                latestEnumerationSet.data.ReturnData.Results.$values =[{
                    "Id": "00000000-0000-0000-0000-000000000000",
                    "Version": 0,
                    "IsPublished": false,
                    "Publisher": "",
                    "Data": "{\"groups\" : []}",
                }];
            }
            if ($scope.LockedBy) {
                delete $scope.lockedByUser
            }
            if (latestEnumerationSetId != ($scope.SelectedEnumerationSetID || latestEnumerationSetId)) { // have we selected the latest version?
                
                enumerationDataManager.GetEnumerationAtVersionQueryAsync($scope.SelectedEnumerationSetID).then(function (enumerationSetData) {
                    try {
                        $scope.SetData(enumerationSetData);
                    }
                    catch (e) {
                        $activityManager.stopActivityWithKey('loadEnumerations');
                    }

                    $scope.Parent_ShowNavButtons(true, false, false, false);
                    deferred.resolve();
                });
            }
            else {
                $scope.SetData(latestEnumerationSet);
                if (latestEnumerationSet.data.ReturnData.Results.$values[0].LockedBy) {
                    var lockedByUser = latestEnumerationSet.data.ReturnData.Results.$values[0].LockedBy;
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

    $scope.save = function () {
        if ($scope.enumerationsForm.$valid && $scope.noDuplicates) {
            var jsonEnums = angular.toJson(enumerationDataManager.$shrink($scope.data));
            enumerationDataManager.SaveEnumerationSetCommandAsync($scope.Id, $scope.version, jsonEnums).then(function (data) {
                $eventAggregatorService.publish($eventDefinitions.onGlobalEnumerationsSaved, jsonEnums);
                $scope.load().then(function () {
                    $notificationService.notifySuccess('Save', 'Enumerations have been saved.');
                }, function (errorMessage) {
                    $notificationService.notifyError('Fail', 'Error: ' + errorMessage);
                });
            });
        } else {
            invalidData();
        }
    }

    $scope.publish = function () {
        if ($scope.enumerationsForm.$valid && $scope.noDuplicates) {
            var jsonEnums = angular.toJson(enumerationDataManager.$shrink($scope.data));
            enumerationDataManager.SaveAndPublishEnumerationSetCommandAsync($scope.Id, $scope.version, jsonEnums, $scope.$root.username).then(function (data) {
                $eventAggregatorService.publish($eventDefinitions.onGlobalEnumerationsSaved, jsonEnums);
                $scope.load().then(function () {
                    $notificationService.notifySuccess('Published', 'Enumeration set saved and published.');
                }, function (errorMessage) {
                    $notificationService.notifyError('Fail', 'Error: ' + errorMessage);
                });
            });
        } else {
            invalidData();
        }
    }

    $scope.close = function (onExitReadyFn) {

        if (!$scope.changed) {
            onExitReadyFn();
            return;
        }

        if ($scope.enumerationsForm.$valid && $scope.noDuplicates) {
            $modalDialogManager.dialogs.unsavedChanges.show().then(function () {
                enumerationDataManager.SaveEnumerationSetCommandAsync($scope.Id, $scope.version, angular.toJson(enumerationDataManager.$shrink($scope.data))).then(function (data) {
                    $notificationService.notifySuccess('Save', 'Enumerations have been saved.');
                    $.debounce(500, false, onExitReadyFn);
                }, function (errorMessage) {
                    $notificationService.notifyError('Fail', 'Error: ' + errorMessage);
                });
            }, onExitReadyFn);
        } else {
            invalidData();
        }
    }

    $scope.Parent_ShowNavButtons(true, true, true, true);

    $scope.Parent_RegisterChildFunctions($scope.save, $scope.publish, $scope.close);

    $scope.load();
}]);