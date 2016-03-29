'use strict';

angular.module('sahl.js.core.transactionQueryService',[])
    .service('$transactionQueryService', function() {
        var operations = {
            /** Temp solution : data should be retrieved from query service.**/
            getLoanTransactionDetailData: function(accountKey) {},
            /** Temp solution : data should be retrieved from query service.**/
            getArrearTransactionDetailData: function(businessKey) {}
        };
        return {
            getLoanTransactionDetailData: operations.getLoanTransactionDetailData,
            getArrearTransactionDetailData: operations.getArrearTransactionDetailData
        };
    });
