'use strict';
angular.module('sahl.websites.halo.services.lookup', ['SAHL.Services.Query.rest', 'halo.webservices'])
    .provider('$lookup', [function () {
        this.decoration = ['$queryServiceRest', '$queryWebService', '$q',
            function ($queryServiceRest, $queryWebService, $q) {

                var operations = {

                    getByLookupType: function (lookupType) {
                        var query = $queryServiceRest.api.lookup.getByLookupType(lookupType);
                        var deferred = $q.defer();
                        $queryWebService.getQueryAsync(query).then(function (data) {
                            return deferred.resolve(data);
                        });
                        return deferred.promise;
                    },
                    getByLookupTypeId: function (lookupType, id) {
                        var deferred = $q.defer();
                        var query = $queryServiceRest.api.lookup.getByLookupType(lookupType).getById(id);
                        $queryWebService.getQueryAsync(query).then(function (data) {
                            return deferred.resolve(data);
                        });
                        return deferred.promise;
                    },
                    getByLineItemCategoryId: function (id) {
                        var deferred = $q.defer();
                        var query = $queryServiceRest.api.lookup.InvoiceLineItemCategory.getById(id).InvoiceLineItemDescription;
                        $queryWebService.getQueryAsync(query).then(function (data) {
                            return deferred.resolve(data);
                        });
                        return deferred.promise;
                    }

                };
                return {
                    getByLookupType: operations.getByLookupType,
                    getByLookupTypeId: operations.getByLookupTypeId,
                    getByLineItemCategoryId: operations.getByLineItemCategoryId
                };
            }
        ];
        this.$get = [function () {
        }];
    }]);
