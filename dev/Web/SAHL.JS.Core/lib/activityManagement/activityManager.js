'use strict';
angular.module('sahl.js.core.activityManagement', [])
    .factory('$activityManager', [function () {
        var activityListeners = [];
        var keyedActivityListeners = {};
        var runningKeys = [];
        var startCount = 0;

        var service = {
            startActivity: function () {
                startCount++;
            },

            stopActivity: function () {
                startCount--;

                if (startCount <= 0) {
                    while (activityListeners.length > 0) {
                        if (activityListeners[0]) {
                            activityListeners[0]();
                        }
                        activityListeners.shift();
                    }
                }
            },

            startActivityWithKey: function (key) {
                var listeners = keyedActivityListeners[key];
                if (listeners) {
                    for (var i = 0; i <= listeners.length; i++) {
                        if (listeners[i]) {
                            listeners[i].onStartCallback();
                        }
                    }
                }
                runningKeys.push(key);
                service.startActivity();
            },

            stopActivityWithKey: function (key) {
                var listeners = keyedActivityListeners[key];
                if (listeners) {
                    for (var i = 0; i <= listeners.length; i++) {
                        if (listeners[i]) {
                            listeners[i].onStopCallback();
                        }
                    }
                }
                runningKeys = $.grep(runningKeys, function (value) {
                    return value !== key;
                });
                service.stopActivity();
            },

            getActivityWithKey: function (key) {
                var runningKey = $.grep(runningKeys, function (value) {
                    if (value === key) {
                        return value;
                    }
                });
                return runningKey;
            },

            registerSpinListener: function (onStopCallback) {
                activityListeners.push(onStopCallback);
            },

            registerSpinListenerForKey: function (start, stop, key, id) {
                if (!keyedActivityListeners[key]) {
                    keyedActivityListeners[key] = [];
                }
                keyedActivityListeners[key].push({
                    'onStartCallback': start,
                    'onStopCallback': stop,
                    'id': id
                });
                for (var i = 0; i < runningKeys.length; i++) {
                    if (runningKeys[i] === key) {
                        start();
                    }
                }
            },

            removeListenerForKey: function (id, key) {
                var listeners = keyedActivityListeners[key];
                var remainingListeners = [];
                if (listeners) {
                    remainingListeners = $.grep(listeners, function (value) {
                        return value.id === id;
                    });
                }
                keyedActivityListeners[key] = remainingListeners;
            },
            clearRunningKeyedActivities: function () {
                for (var i = 0; i < runningKeys.length; i++) {
                    service.stopActivityWithKey(runningKeys[i]);
                }
            }
        };

        return service;
    }]);
