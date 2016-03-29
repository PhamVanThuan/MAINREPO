'use strict';
angular.module('sahl.websites.halo.services.treasuryManager', [
        'halo.webservices'
])
.service('$treasuryManagerService', ['$q', '$queryServiceRest', '$queryWebService',
    function ($q, $queryServiceRest, $queryWebService) {
        
        var operations = {                
            getSPVs: function () {
                var deferred = $q.defer();           
                var query = $queryServiceRest.api.treasury.spvs;
                
                $queryWebService.getQueryAsync(query).then(function (data) {
                    deferred.resolve({
                        results: data.data
                    });
                });
                return deferred.promise;
            }
        };

        return {
            getSPVs: operations.getSPVs
        };
    }
]);