'use strict';
angular.module('sahl.js.core.serviceManagement', [])
    .service('$startableManagerService', ['$q', '$timeout',
        function ($q, $timeout) {
            var operations = {
                startServices: function () {
                    var waitableServices = [];
                    angular.forEach(arguments, function (value) {
                        if (value['start'] !== undefined) {
                            var result = value.start();
                            if (result !== undefined && result['then']) {
                                waitableServices.push(result);
                            }
                        }
                    });
                    return $q.all(waitableServices);
                }
            };

            return {
                startServices: operations.startServices
            };
        }
    ]);
