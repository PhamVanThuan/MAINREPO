'use strict';
angular.module('sahl.websites.halo.services.attorneysManager', [
        'halo.webservices'
])
.service('$attorneysManagerService', ['$q', '$queryServiceRest', '$queryWebService',
    function ($q, $queryServiceRest, $queryWebService) {
        
        var operations = {                
            getAttorneys: function () {
                var deferred = $q.defer();           
                var query = $queryServiceRest.api.attorneys;
                
                $queryWebService.getQueryAsync(query).then(function (data) {
                    deferred.resolve({
                        results: data.data
                    });
                });
                return deferred.promise;
            }
        };

        return {
            getAttorneys: operations.getAttorneys
        };
    }
]);