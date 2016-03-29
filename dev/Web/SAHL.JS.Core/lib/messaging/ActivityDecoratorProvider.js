'use strict';

angular.module('sahl.js.core.messaging')
    .provider('$activityDecorator', [function () {
        this.decoration = ['$delegate', '$q', '$activityManager', function ($delegate, $q, $activityManager) {
            return {
                postMessage: function (messageToSend, url) {
                    var deferred = $q.defer();
                    $activityManager.startActivity();
                    $delegate.postMessage(messageToSend, url).then(function (data) {
                        $activityManager.stopActivity();
                        deferred.resolve(data);
                    }, function (errorMsg) {
                        $activityManager.stopActivity();
                        deferred.reject(errorMsg);
                    });
                    return deferred.promise;
                },
                getMessage:function(url){
                    var deferred = $q.defer();
                    $activityManager.startActivity();
                    $delegate.getMessage(url).then(function (data) {
                        $activityManager.stopActivity();
                        deferred.resolve(data);
                    }, function (errorMsg) {
                        $activityManager.stopActivity();
                        deferred.reject(errorMsg);
                    });
                    return deferred.promise;
                }
            };
        }];

        this.$get = [function DecoratorFactory() {
        }];
    }]);
