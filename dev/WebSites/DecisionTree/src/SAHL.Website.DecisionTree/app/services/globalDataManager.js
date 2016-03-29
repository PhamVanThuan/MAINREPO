'use strict';

/* Services */

angular.module('sahl.tools.app.services.globalDataManager', [
    'sahl.tools.app.services',
    'SAHL.Services.Interfaces.DecisionTreeDesign.queries',
    'SAHL.Services.Interfaces.DecisionTreeDesign.commands'
])
.factory('$globalDataManager', ['$rootScope', '$queryManager', '$commandManager', '$decisionTreeDesignQueries', '$decisionTreeDesignCommands', '$state', '$q', '$activityManager', '$eventAggregatorService', '$eventDefinitions', '$variableDataManager', '$enumerationDataManager', '$utils', 'filterFilter', '$timeout',
    function ($rootScope, $queryManager, $commandManager, $decisionTreeDesignQueries, $decisionTreeDesignCommands, $state, $q, $activityManager, $eventAggregatorService, $eventDefinitions, variableDataManager, enumerationDataManager, $utils, filterFilter, $timeout) {
        var currentEnums = [];
        var currentGroupedEnums = [];

        var utilities = {
            flattenGroups: function (baseData, lastGroupName, flatList) {
                var innergroups = baseData.groups;
                innergroups.forEach(function (_group) {
                    var groupname = _group.name;
                    var enumList = _group.enumerations.map(function (en) {
                        return $utils.string.capitaliseFirstLetter(lastGroupName + groupname + "::" + en.name, true);
                    });
                    for (var i = 0, c = enumList.length; i < c; i++) {
                        flatList.push(enumList[i]);
                    }
                    if (_group.groups) {
                        utilities.flattenGroups(_group, lastGroupName + groupname + "::", flatList)
                    }
                });
            }
        }

        var operations = {
            setupAvailableVariableTypes: function () {
                $rootScope.globalData.nonEnumVariableTypes = variableDataManager.GetBasicTypes();
                $rootScope.globalData.variableTypes = [];
                $rootScope.globalData.variableTypes = $rootScope.globalData.variableTypes.concat($rootScope.globalData.nonEnumVariableTypes);
                var flatList = [];
                utilities.flattenGroups(currentEnums, '', flatList);

                $rootScope.globalData.variableTypes = $rootScope.globalData.variableTypes.concat(flatList);
            },
            setupEumerationValues: function () {
                var deferred = $q.defer();
                enumerationDataManager.GetLatestEnumerationSetQueryAsync().then(function (data) {
                    if (data.data.ReturnData.Results.$values.length == 1) {
                        currentEnums = enumerationDataManager.$extend(angular.fromJson(data.data.ReturnData.Results.$values[0].Data));
                    }
                    else {
                        currentEnums = { groups: [] };
                    }

                    operations.setupGroupedEnumerations();
                    deferred.resolve();
                });

                return deferred.promise;
            },
            setupGroupedEnumerations: function () {
                var array = new Array();

                var getEnumOption = function (enumerationGroup, groupNamespace) {
                    var resultArray = new Array();
                    groupNamespace = groupNamespace + '::' + $utils.string.smallFirstLetter(enumerationGroup.name, true);

                    if (enumerationGroup.enumerations.length > 0) {
                        angular.forEach(enumerationGroup.enumerations, function (enumItem) {
                            var enumNamespace = groupNamespace + '::' + $utils.string.smallFirstLetter(enumItem.name, true);
                            angular.forEach(enumItem.value, function (val) {
                                var enumObj = { group: enumNamespace, name: val, value: (enumNamespace + '.' + $utils.string.sanitise(val)) };
                                resultArray.push(enumObj);
                            });
                        });
                    };
                    if (enumerationGroup.groups.length > 0) {
                        angular.forEach(enumerationGroup.groups, function (groupItem) {
                            var x = getEnumOption(groupItem, groupNamespace)
                            Array.prototype.push.apply(resultArray, x);
                        })
                    }

                    return resultArray;
                };

                currentGroupedEnums = getEnumOption(currentEnums.groups[0], 'Enumerations');
            },
            getEnumerationValuesForVariable: function (variable) {
                var defaultOpt;

                var result = currentGroupedEnums.filter(function (opt) {
                    if (opt.name == 'Unknown') {
                        defaultOpt = opt;
                        return true;
                    }
                    var compareGroup = opt.group;
                    compareGroup = (compareGroup.replace('Enumerations::', '')).toLowerCase();
                    return compareGroup === variable.type.toLowerCase();
                });
                if (!variable.defaultValue) {
                    variable.defaultValue = defaultOpt;
                }

                return result;
            },
            getEnumerationValuesForType: function (variableType) {
                var array = new Array();
                if ($rootScope.globalData.nonEnumVariableTypes.indexOf(variableType) === -1) {
                    var enumpointers = variableType.split("::");

                    var nameSpace = "Enumerations::";
                    for (var i = 0, c = enumpointers.length; i < c; i++) {
                        nameSpace += enumpointers[i][0].toLowerCase() + enumpointers[i].substring(1);
                        if (i == (c - 1)) {
                            nameSpace += ".";
                        }
                        else {
                            nameSpace += "::";
                        }
                    }

                    var current = currentEnums;
                    for (var val in enumpointers) {
                        if (val == (enumpointers.length - 1)) {
                            var enumerations = filterFilter(current.enumerations, function (enumeration) {
                                return enumeration.name.replace(/ /g, "").toLowerCase() === enumpointers[val].toLowerCase()
                            })
                            if (enumerations.length > 0) {
                                angular.forEach(enumerations[0].value, function (val) {
                                    array.push({ name: val, value: (nameSpace + $utils.string.sanitise(val)) });
                                })
                            };
                        } else {
                            var current = filterFilter(current.groups, function (groupItem) {
                                return groupItem.name.replace(/ /g, "").toLowerCase() === enumpointers[val].toLowerCase()
                            })[0];
                        }
                    }
                }
                return array;
            }
        }

        var eventHandlers = {
            onGlobalVariablesSaved: function () {
            },
            onGlobalEnumerationsSaved: function () {
                operations.setupEumerationValues();
                operations.setupAvailableVariableTypes();
            },
            onGlobalMessagesSaved: function () {
            }
        }

        $eventAggregatorService.subscribe($eventDefinitions.onGlobalVariablesSaved, eventHandlers.onGlobalVariablesSaved);
        $eventAggregatorService.subscribe($eventDefinitions.onGlobalEnumerationsSaved, eventHandlers.onGlobalEnumerationsSaved);
        $eventAggregatorService.subscribe($eventDefinitions.onGlobalMessagesSaved, eventHandlers.onGlobalMessagesSaved);

        return {
            start: function () {
                $rootScope.globalData = {}

                operations.setupEumerationValues().then(function () {
                    operations.setupAvailableVariableTypes();
                });

                $rootScope.boolSelect = [false, true];
                $rootScope.globalData.getEnumerationValuesForType = operations.getEnumerationValuesForType;
                $rootScope.globalData.getEnumerationValuesForVariable = operations.getEnumerationValuesForVariable;
                $rootScope.globalData.isVariableofType = function (variable, expectedType) {
                    if (expectedType.indexOf(variable.type) > -1) {
                        return true;
                    }
                    return false;
                }
                $rootScope.globalData.isVariableAnEnum = function (variable) {
                    if ($rootScope.globalData.nonEnumVariableTypes.indexOf(variable.type) > -1) {
                        return false;
                    }
                    return true;
                }
            }
        }
    }])