'use strict';

/* Services */

angular.module('sahl.tools.app.services.signalRSvc', [
    'sahl.tools.app.serviceConfig'
])
.factory('$signalRSvc', ['$rootScope', '$q', '$serviceConfig', function ($rootScope, $q, $serviceConfig) {
    var connection = null;
    var proxy = null;
    var lockScheduleProxy = null;
    var startingDeferredPromise = null;
    var initialisingDeferredPromise = null;

    var eventHandlers = {
        onConnectionError: function (error) {
            console.log('hub connection error: ' + error);
        },
        onStarting: function () {
        },
        onReconnecting: function () {
        },
        onReconnected: function () {
        },
        onDisconnected: function () {
            setTimeout(function () {
                startingDeferredPromise = null;
                internalOperations.startHub();
            }, 500); // Restart connection after 0.5 seconds.
        }
    }

    var internalOperations = {
        createConnection: function(){
            //Getting the connection object
            connection = $.hubConnection($serviceConfig.SignalRService, { useDefaultPath: false });
            connection.transportConnectTimeout = 60000;
            //connection.logging = true;
            connection.error(eventHandlers.onConnectionError);
        },
        initialiseProxies: function () {
            if (initialisingDeferredPromise !== null) {
                return initialisingDeferredPromise;
            }
            if (proxy != null) {
                throw ("proxy is already initialised");
            }
            if (lockScheduleProxy != null) {
                throw ("Lock schedule proxy is already initialised");
            }

            var deferred = $q.defer();
            initialisingDeferredPromise = deferred.promise;
            // create the connection
            this.createConnection();

            //Creating debug proxy
            proxy = connection.createHubProxy('decisionTreeDebugHub');
            //Creating lockSchedule proxy
            lockScheduleProxy = connection.createHubProxy('lockScheduleHub');

            deferred.resolve();

            return initialisingDeferredPromise;
        },
        startHub: function () {
            if (startingDeferredPromise !== null) {
                return startingDeferredPromise;
            }

            var deferred = $q.defer();

            //Starting connection
            connection.start({ transport: 'longPolling', }).done(function () {
                deferred.resolve();
                console.log('hub connection done');
            }).fail(function () {
                deferred.reject();
                console.log('hub connection failed');
            });

            startingDeferredPromise = deferred.promise;
            return startingDeferredPromise;
        },
        stopHub: function () {
            if (connection != null) {
                connection.stop();
                connection = null;
            }
            if (startingDeferredPromise !== null) {
                startingDeferredPromise = null;
            }
            if (initialisingDeferredPromise != null) {
                initialisingDeferredPromise = null;
            }

            if (proxy != null) {
                proxy = null;
            }

            if (lockScheduleProxy != null) {
                lockScheduleProxy = null;
            }
        }
    }
    return {
        getProxy: function () {
            return proxy
        },
        getLockScheduleProxy: function () {
            return lockScheduleProxy;
        },
        initialiseProxies: function () {
            return internalOperations.initialiseProxies();
        },
        startHub: function () {
            return internalOperations.startHub();
        },
        stopHub: function () {
            internalOperations.stopHub();
        },
        destroyHub: function () {
            internalOperations.stopHub();

            if (proxy !== null) {
                proxy = null;
            }
        }
    }
}])