'use strict';

angular.module('sahl.js.core.signalR', [
    'sahl.js.core.logging'
])
    .factory('$signalRHubManager', ['$rootScope', '$q', '$logger',
        function ($rootScope, $q, $logger) {
            function signalRHubProxyFactory(serverUrl, hubName, startOptions) {
                var connection = $.hubConnection(serverUrl);
                var proxy = connection.createHubProxy(hubName);

                return {
                    start: function () {
                        var deferred = $q.defer();

                        connection.start(startOptions)
                            .done(function () {
                                $logger.logInfo('Connection created to hub: ' + hubName);
                                deferred.resolve();
                            })
                            .fail(function () {
                                $logger.logInfo('Could not connect!');
                                deferred.reject();
                            });

                        return deferred.promise;
                    },
                    on: function (eventName, callback) {
                        proxy.on(eventName, function (result) {
                            $rootScope.$apply(function () {
                                if (callback) {
                                    callback(result);
                                }
                            });
                        });
                    },
                    off: function (eventName, callback) {
                        proxy.off(eventName, function (result) {
                            $rootScope.$apply(function () {
                                if (callback) {
                                    callback(result);
                                }
                            });
                        });
                    },
                    invoke: function (methodName, methodParameters) {
                        var deferred = $q.defer();

                        proxy.invoke(methodName, methodParameters)
                            .done(function (result) {
                                $rootScope.$apply(function () {
                                    deferred.resolve(result);
                                });
                            })
                            .fail(function (error) {
                                deferred.reject(error);
                            });

                        return deferred.promise;
                    },
                    connection: connection
                };
            };

            return signalRHubProxyFactory;
        }]);
