'use strict';

/* Services */

angular.module('sahl.tools.app.services.sessionLockService', [
    'sahl.tools.app.serviceConfig'
])
.factory('$sessionLockService', ['$rootScope', '$signalRSvc', '$eventAggregatorService', '$eventDefinitions', '$commandManager', '$decisionTreeDesignCommands', '$q',
    function ($rootScope, $signalRSvc, $eventAggregatorService, $eventDefinitions, $commandManager, $decisionTreeDesignCommands, $q) {
        var internal = {
            init: function () {
                var deferred = $q.defer();
                $signalRSvc.initialiseProxies().then(function () {
                    $signalRSvc.getLockScheduleProxy().on('onPendingLockRelease', function (data) {
                        $eventAggregatorService.publish($eventDefinitions.onPendingLockRelease, data);
                    });
                    $signalRSvc.getLockScheduleProxy().on('onRemovedLocks', function (data) {
                        $eventAggregatorService.publish($eventDefinitions.onRemovedLocks, data);
                    });
                    $signalRSvc.getLockScheduleProxy().on('onAddedLocks', function (data) {
                        $eventAggregatorService.publish($eventDefinitions.onAddedLocks, data);
                    });
                    deferred.resolve();
                });
                return deferred.promise;
            }
        }
        var operations = {
            closeLockForEnumerations: function (documentVersionId, username) {
                var deferred = $q.defer();
                var command = new $decisionTreeDesignCommands.CloseLockForEnumerationsCommand(documentVersionId, username);
                $commandManager.sendCommandAsync(command).then(function () {
                    deferred.resolve();
                }, function () {
                    deferred.reject();
                });
                return deferred.promise;
            },
            openLockForEnumerations: function (documentVersionId, username) {
                var deferred = $q.defer();
                var command = new $decisionTreeDesignCommands.OpenLockForEnumerationsCommand(documentVersionId, username);
                $commandManager.sendCommandAsync(command).then(function () {
                    deferred.resolve();
                }, function () {
                    deferred.reject();
                });
                return deferred.promise;
            },
            closeLockForMessages: function (documentVersionId, username) {
                var deferred = $q.defer();
                var command = new $decisionTreeDesignCommands.CloseLockForMessagesCommand(documentVersionId, username);
                $commandManager.sendCommandAsync(command).then(function () {
                    deferred.resolve();
                }, function () {
                    deferred.reject();
                });
                return deferred.promise;
            },
            openLockForMessages: function (documentVersionId, username) {
                var deferred = $q.defer();
                var command = new $decisionTreeDesignCommands.OpenLockForMessagesCommand(documentVersionId, username);
                $commandManager.sendCommandAsync(command).then(function () {
                    deferred.resolve();
                }, function () {
                    deferred.reject();
                });
                return deferred.promise;
            },
            closeLockForVariables: function (documentVersionId, username) {
                var deferred = $q.defer();
                var command = new $decisionTreeDesignCommands.CloseLockForVariablesCommand(documentVersionId, username);
                $commandManager.sendCommandAsync(command).then(function () {
                    deferred.resolve();
                }, function () {
                    deferred.reject();
                });
                return deferred.promise;
            },
            openLockForVariables: function (documentVersionId, username) {
                var deferred = $q.defer();
                var command = new $decisionTreeDesignCommands.OpenLockForVariablesCommand(documentVersionId, username);
                $commandManager.sendCommandAsync(command).then(function () {
                    deferred.resolve();
                }, function () {
                    deferred.reject();
                });
                return deferred.promise;
            },
            closeLockForTrees: function (documentVersionId, username) {
                var deferred = $q.defer();
                var command = new $decisionTreeDesignCommands.CloseLockForTreeCommand(documentVersionId, username);
                $commandManager.sendCommandAsync(command).then(function () {
                    deferred.resolve();
                }, function () {
                    deferred.reject();
                });
                return deferred.promise;
            },
            openLockForTrees: function (documentVersionId, username) {
                var deferred = $q.defer();
                var command = new $decisionTreeDesignCommands.OpenLockForTreeCommand(documentVersionId, username);
                $commandManager.sendCommandAsync(command).then(function () {
                    deferred.resolve();
                }, function () {
                    deferred.reject();
                });
                return deferred.promise;
            }
        };
        return {
            start: function () {
                return internal.init();
            },
            closeLockForEnumerations: operations.closeLockForEnumerations,
            openLockForEnumerations: operations.openLockForEnumerations,
            closeLockForMessages: operations.closeLockForMessages,
            openLockForMessages: operations.openLockForMessages,
            closeLockForVariables: operations.closeLockForVariables,
            openLockForVariables: operations.openLockForVariables,
            closeLockForTrees: operations.closeLockForTrees,
            openLockForTrees: operations.openLockForTrees
        }
    }]);