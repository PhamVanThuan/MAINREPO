'use strict';

angular.module('sahl.websites.halo.services.transactionQueryService', [
        'halo.webservices',
        'sahl.js.core.logging',
        'SAHL.Services.Interfaces.Halo.queries',
        'SAHL.Services.Interfaces.Halo.sharedmodels'
])
    .provider('$transactionQueryDecoration', [
        function () {
            this.decoration = [
                '$delegate', '$haloWebService', '$haloQueries', '$haloSharedModels', '$q',
                function ($delegate, $haloWebService, $haloQueries, $haloSharedModels, $q) {

                    var operations = {

                        /** Temp solution until the rest api is complete and functional**/
                        getLoanTransactionDetailData: function (accountKey) {
                            var deferred = $q.defer();
                            var loanTransactionQuery = new $haloQueries.GetLoanTransactionDataByAccountKeyQuery(accountKey);
                            $haloWebService.sendQueryAsync(loanTransactionQuery).then(function (queryResult) {
                                if (queryResult &&
                                    queryResult.data &&
                                    queryResult.data.ReturnData &&
                                    queryResult.data.ReturnData.Results &&
                                    queryResult.data.ReturnData.Results.$values) {

                                    deferred.resolve(queryResult.data.ReturnData.Results.$values);
                                } else {
                                    deferred.resolve(null);
                                }
                            });
                            return deferred.promise;
                        },

                        /** Temp solution until the rest api is complete and functional**/
                        getArrearTransactionDetailData: function (businessKey) {
                            var deferred = $q.defer();
                            var arrearTransactionDetailData = new $haloQueries.GetArrearTransactionDataByAccountKeyQuery(businessKey);
                            $haloWebService.sendQueryAsync(arrearTransactionDetailData).then(function (queryResult) {
                                if (queryResult &&
                                    queryResult.data &&
                                    queryResult.data.ReturnData &&
                                    queryResult.data.ReturnData.Results &&
                                    queryResult.data.ReturnData.Results.$values) {

                                    deferred.resolve(queryResult.data.ReturnData.Results.$values);
                                } else {
                                    deferred.resolve(null);
                                }
                            });
                            return deferred.promise;
                        }
                    };
                    return {
                        getLoanTransactionDetailData: operations.getLoanTransactionDetailData,
                        getArrearTransactionDetailData: operations.getArrearTransactionDetailData
                    };
                }
            ];

            this.$get = [function () { }];
        }
    ]);

