'use strict';
angular.module('sahl.websites.halo.services.accountsManager', [
        'halo.webservices'
])
.service('$accountsManagerService', ['$q', '$queryServiceRest', '$queryWebService',
    function ($q, $queryServiceRest, $queryWebService) {
        
        var operations = {                
            getLossControlProcessStage: function (accountNumber,businessProcess) {
                var deferred = $q.defer();           
                var query = $queryServiceRest.api.accounts.getById(accountNumber).processes.getByProcess(businessProcess).stage;
                
                $queryWebService.getQueryAsync(query).then(function (data) {
                    deferred.resolve({
                        results: data.data
                    });
                });
                return deferred.promise;
            },           
            getAccountAndSPVByAccountNumber: function (accountNumber) {
                var deferred = $q.defer();           
                var query = $queryServiceRest.api.accounts.getById(accountNumber);
                var includes = ['spv'];
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
            getLossControlProcessStage : operations.getLossControlProcessStage,
            getAccountAndSPVByAccountNumber: operations.getAccountAndSPVByAccountNumber         
        };
    }
]);