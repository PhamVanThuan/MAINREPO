'use strict';
angular.module('sahl.js.core.messaging')
    .factory('$messageManager', ['$http', '$q', function($http, $q) {
            return {
                postMessage: function(messageToSend, url) {
                    var deferred = $q.defer();
                    var json = angular.fromJson(messageToSend);
                    $http.post(url, json).success(function(data, status, headers) {
                        deferred.resolve({
                            data: data,
                            status: status,
                            headers: headers
                        });
                    }).error(function(reason, status, headers) {
                            deferred.reject({
                                data: reason,
                                status: status,
                                headers: headers
                            });
                    });
                return deferred.promise;
            },
            getMessage: function(url) {
                var deferred = $q.defer();
                $http.get(url).success(function(data, status, headers) {
                    deferred.resolve({
                        data: data,
                        status: status,
                        headers: headers
                    });
                }).error(function() {
                    deferred.reject('An error occurred while accessing the service over http.');
                });
                return deferred.promise;
            }
        };
    }]);
