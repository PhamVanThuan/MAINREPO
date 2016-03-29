'use strict';
angular.module('sahl.websites.halo.services.thirdPartyInvoiceManager', [
        'halo.webservices',      
        'SAHL.Services.Interfaces.FinanceDomain.commands',
        'SAHL.Services.Interfaces.FinanceDomain.sharedmodels',
        'SAHL.Services.Interfaces.DocumentManager.queries',
        'SAHL.Services.Interfaces.DocumentManager.sharedmodels',
        'SAHL.Services.Interfaces.FinanceDomain.queries'
])
.service('$thirdPartyInvoiceManagerService', ['$timeout', '$financeDomainWebService', '$financeDomainCommands', '$q',
    '$queryServiceRest', '$queryWebService', '$activityManager', '$documentManagerQueries', '$documentManagerWebService', '$lookupService',
    '$fluentRestQuery', '$fluentWhereBuilder', '$financeDomainQueries',
    function($timeout, $financeDomainWebService, $financeDomainCommands, $q,
                $queryServiceRest, $queryWebService, $activityManager, $documentManagerQueries, $documentManagerWebService, $lookupService, $fluentRestQuery, $fluentWhereBuilder, $financeDomainQueries) {
        var searchFilters = {};

        var operations = {                
            getInvoiceDataById: function (invoiceId) {
                var deferred = $q.defer();           
                var query = $queryServiceRest.api.finance.thirdpartyinvoices.getById(invoiceId);
                var includes = ['documents'];
                query.include(includes);
               
                $queryWebService.getQueryAsync(query).then(function (data) {
                    deferred.resolve({
                        results: data.data
                    });
                });
                return deferred.promise;
            },
            getInvoicesByStatusKey : function(statusKey){
                var deferred = $q.defer();           
                var query = $queryServiceRest.api.finance.thirdpartyinvoices;                               
                var filterdQuery = query.where('invoiceStatusKey').isEqual(statusKey)                                 
                                    .endWhere();
                var includes = ['account'];
                filterdQuery.include(includes);
                $queryWebService.getQueryAsync(filterdQuery).then(function (data) {
                    deferred.resolve({
                        results: data.data
                    });
                });
                return deferred.promise;
            },
            getInvoiceDataList: function () {
                var deferred = $q.defer();           
                var query = $queryServiceRest.api.finance.thirdpartyinvoices;                               
                var filterdQuery = query.where('invoiceStatusKey').isEqual(1,2,3,4)                                 
                                    .endWhere();
                var includes = ['account'];
                filterdQuery.include(includes);
                $queryWebService.getQueryAsync(filterdQuery).then(function (data) {
                    deferred.resolve({
                        results: data.data
                    });
                });
                return deferred.promise;
            }, 
            getInvoiceLineItems: function (invoiceId) {                
                var deferred = $q.defer();
                var query = $queryServiceRest.api.finance.thirdpartyinvoices.getById(invoiceId).lineItems;
                
                $queryWebService.getQueryAsync(query).then(function (data) {
                    deferred.resolve({
                        results: data
                    });
                });
                return deferred.promise;
            },
            getLookUpLineItemTypes: function () {
                return $lookupService.getByLookupType('InvoiceLineItemCategory');
            },


            getLookUpLineItemDescriptions: function (categoryId) {
                return $lookupService.getByLineItemCategoryId(categoryId);
            },

            amendThirdPartyInvoice: function (thirdPartyInvoiceModel) {
                var deferred = $q.defer();
                var command = new $financeDomainCommands.AmendThirdPartyInvoiceCommand(thirdPartyInvoiceModel);
                $financeDomainWebService.sendCommandAsync(command).then(function (data) {
                    deferred.resolve({                    
                        results: data.data.SystemMessages
                    });
                },
                function (results) {
                    deferred.reject({
                        results: results.data.SystemMessages.AllMessages.$values
                    });
                });
                return deferred.promise;
            },
            captureThirdPartyInvoice: function (thirdPartyInvoiceModel) {
                var deferred = $q.defer();
                var command = new $financeDomainCommands.CaptureThirdPartyInvoiceCommand(thirdPartyInvoiceModel);
                $financeDomainWebService.sendCommandAsync(command).then(function (data) {
                    deferred.resolve({                    
                        results: data.data.SystemMessages
                    });
                },
                function (results) {
                    deferred.reject({
                        results: results.data.SystemMessages.AllMessages.$values
                    });                   
                });
                return deferred.promise;
            },
            getMandatedUserListForEscalation: function (thirdPartyInvoiceKey) {
                var deferred = $q.defer();
                var query = new $financeDomainQueries.GetMandatedUsersForThirdPartyInvoiceEscalationQuery(thirdPartyInvoiceKey);
                $financeDomainWebService.sendQueryAsync(query).then(function (data) {
                    deferred.resolve({
                        results: data.data.ReturnData.Results.$values
                    });
                });
                return deferred.promise;                
            },
            getThirdPartyInvoiceCorrespondence: function (thirdPartyInvoiceKey) {
                var deferred = $q.defer();
                var query = new $financeDomainQueries.GetThirdPartyInvoiceCorrespondenceQuery(thirdPartyInvoiceKey);
                $financeDomainWebService.sendQueryAsync(query).then(function (data) {
                    deferred.resolve({
                        results: data.data.ReturnData.Results.$values
                    });
                });
                return deferred.promise;                
            },

            getThirdParties : function(){
                var deferred = $q.defer();           
                var query = $queryServiceRest.api.thirdparties;
                
                $queryWebService.getQueryAsync(query).then(function (data) {
                    deferred.resolve({
                        results: data.data
                    });
                });
                return deferred.promise;
            },

            getMonthBreakdownByAttorney: function () {
                var deferred = $q.defer();
                var query = new $financeDomainQueries.GetAttorneyInvoiceMonthlyBreakDownQuery();
                $financeDomainWebService.sendQueryAsync(query).then(function (data) {
                    deferred.resolve({
                        results: data.data.ReturnData.Results.$values
                    });
                });
                return deferred.promise;
            },
            
            getInvoicesPaidPreviousMonthBreakDown: function () {
                var deferred = $q.defer();
                var query = new $financeDomainQueries.GetAttorneyInvoicesPaidPreviousMonthBreakDownQuery();
                $financeDomainWebService.sendQueryAsync(query).then(function (data) {
                    deferred.resolve({
                        results: data.data.ReturnData.Results.$values
                    });
                });
                return deferred.promise;                
            },

            getInvoicesPaidThisMonthBreakDown: function () {
                var deferred = $q.defer();
                var query = new $financeDomainQueries.GetAttorneyInvoicesPaidThisMonthBreakDownQuery();
                $financeDomainWebService.sendQueryAsync(query).then(function (data) {
                    deferred.resolve({
                        results: data.data.ReturnData.Results.$values
                    });
                });
                return deferred.promise;
            },
            
            getInvoicesPaidThisYearBreakDown: function () {
                var deferred = $q.defer();
                var query = new $financeDomainQueries.GetAttorneyInvoicesPaidThisYearBreakDownQuery();
                $financeDomainWebService.sendQueryAsync(query).then(function (data) {
                    deferred.resolve({
                        results: data.data.ReturnData.Results.$values
                    });
                });
                return deferred.promise;                
            },
            
            getInvoicesNotProcessedPreviousMonthBreakDown: function () {
                var deferred = $q.defer();
                var query = new $financeDomainQueries.GetAttorneyInvoicesNotProcessedPreviousMonthBreakDownQuery();
                $financeDomainWebService.sendQueryAsync(query).then(function (data) {
                    deferred.resolve({
                        results: data.data.ReturnData.Results.$values
                    });
                });
                return deferred.promise;                
            },

            getInvoicesNotProcessedThisMonthBreakDown: function () {
                var deferred = $q.defer();
                var query = new $financeDomainQueries.GetAttorneyInvoicesNotProcessedThisMonthBreakDownQuery();
                $financeDomainWebService.sendQueryAsync(query).then(function (data) {
                    deferred.resolve({
                        results: data.data.ReturnData.Results.$values
                    });
                });
                return deferred.promise;
            },
            
            getInvoicesNotProcessedThisYearBreakDown: function () {
                var deferred = $q.defer();
                var query = new $financeDomainQueries.GetAttorneyInvoicesNotProcessedThisYearBreakDownQuery();
                $financeDomainWebService.sendQueryAsync(query).then(function (data) {
                    deferred.resolve({
                        results: data.data.ReturnData.Results.$values
                    });
                });
                return deferred.promise;                
            }

        };
        return {
            getInvoiceDataById: operations.getInvoiceDataById,
            getInvoiceLineItems: operations.getInvoiceLineItems,
            amendThirdPartyInvoice: operations.amendThirdPartyInvoice,
            getLookupLineItemTypes: operations.getLookUpLineItemTypes,
            getLookUpLineItemDescriptions: operations.getLookUpLineItemDescriptions,
            captureThirdPartyInvoice: operations.captureThirdPartyInvoice,
            getInvoiceDataList: operations.getInvoiceDataList,
            getMandatedUserListForEscalation: operations.getMandatedUserListForEscalation,
            getInvoicesByStatusKey: operations.getInvoicesByStatusKey,
            getThirdParties: operations.getThirdParties,
            getThirdPartyInvoiceCorrespondence: operations.getThirdPartyInvoiceCorrespondence,
            getMonthBreakdownByAttorney: operations.getMonthBreakdownByAttorney,
            getInvoicesPaidPreviousMonthBreakDown: operations.getInvoicesPaidPreviousMonthBreakDown,
            getInvoicesPaidThisMonthBreakDown: operations.getInvoicesPaidThisMonthBreakDown,
            getInvoicesPaidThisYearBreakDown: operations.getInvoicesPaidThisYearBreakDown,
            getInvoicesNotProcessedPreviousMonthBreakDown: operations.getInvoicesNotProcessedPreviousMonthBreakDown,
            getInvoicesNotProcessedThisMonthBreakDown: operations.getInvoicesNotProcessedThisMonthBreakDown,
            getInvoicesNotProcessedThisYearBreakDown: operations.getInvoicesNotProcessedThisYearBreakDown

        };
    }
])
.constant('$thirdPartyInvoiceStatus', {
    received : 1,
    awaitingApproval : 2,
    approved : 3,
    processingPayment : 4,
    rejected : 5,
    paid : 6,
    captured : 7
});