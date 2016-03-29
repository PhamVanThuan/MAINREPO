'use strict';
angular.module('sahl.websites.halo.services.thirdPartyManager', [
        'halo.webservices'
])
.service('$thirdPartyManagerService', ['$q', '$queryServiceRest', '$queryWebService',
    function ($q, $queryServiceRest, $queryWebService) {
        
        var operations = {                
            getThirdParties: function () {
                var deferred = $q.defer();           
                var query = $queryServiceRest.api.thirdparties;
                var includes = ['paymentbankaccount', 'attorneys'];
                query.include(includes);
                $queryWebService.getQueryAsync(query).then(function (data) {
                    deferred.resolve({
                        results: data.data
                    });
                });
                return deferred.promise;
            }
        };

        return {
            getThirdParties: operations.getThirdParties
        };
    }
]);