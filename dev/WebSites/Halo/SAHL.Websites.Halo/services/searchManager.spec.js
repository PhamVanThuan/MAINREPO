'use strict';

describe('[sahl.websites.halo.services.searchManagement]', function () {

    beforeEach(module('sahl.websites.halo.services.searchManagement'));
    //beforeEach(module('halo.webservices'));
    beforeEach(module('sahl.js.core.lookup'));
    beforeEach(module('sahl.websites.halo.services.lookup'));
    beforeEach(module('SAHL.Services.Interfaces.Search.queries'));
    beforeEach(module('SAHL.Services.Interfaces.Search.sharedmodels'));
    beforeEach(module('SAHL.Services.Interfaces.WorkflowTask.queries'));
    beforeEach(module('sahl.websites.halo.services.lookupDataService'));

    var $searchManagerService,
        $q,
        $queryServiceRest,
        $searchWebService,
        $searchQueries,
        $searchSharedModels,
        $workflowTaskWebService,
        $workflowTaskQueries,
        $lookupDataService,
        $lookupService,
        $timeout,
        rootScope;
        

    var deferred,
	    promise = null,
	    invoiceId = 100;
    
    beforeEach(inject(function ($injector, $rootScope) {
        $q = $injector.get('$q');
        deferred = $q.defer();
        promise = deferred.promise;
        rootScope = $rootScope;

        $queryServiceRest = $injector.get('$queryServiceRest');
        $searchWebService = $injector.get('$searchWebService');
        $searchQueries = $injector.get('$searchQueries');
        $searchSharedModels = $injector.get('$searchSharedModels');
        $workflowTaskWebService = $injector.get('$workflowTaskWebService');
        $workflowTaskQueries = $injector.get('$workflowTaskQueries');
        $lookupDataService = $injector.get('$lookupDataService');
        $lookupService = $injector.get('$lookupService');

        $searchManagerService = $injector.get('$searchManagerService');
    }));

    describe('search for clients', function () {
        var freeTextTerms = 'tak';
        var indexName = 'Clients';
        var pageSize = 10;
        var currentPage = 2;

        beforeEach(function () {

            spyOn($searchQueries, 'SearchForClientQuery').and.returnValue("");
            spyOn($searchWebService, 'sendQueryAsync').and.returnValue(deferred.promise);

            $searchManagerService.getNewSearchTypesAndFilters(null,null);
            $searchManagerService.searchForClients(freeTextTerms, indexName, pageSize, currentPage);
        });


        it('should call the query service to get the clients data', function () {
            expect($searchWebService.sendQueryAsync).toHaveBeenCalled();
        });

        //it("should return the clients data when the promise is resolved", function () {
        //    var data = {
        //        data: {
        //            ReturnData: {
        //                QueryDurationInMilliseconds: 100,
        //                ResultCountInAllPages: 900,
        //                Results: {
        //                    $values: 'clients metadata'
        //                }
        //            }
        //        }
        //    };
        //    deferred.resolve(data);
        //    rootScope.$apply();
        //});

        it("should return error messages when the service call fails", function(){
            var data = "error Messages";
            deferred.reject(data);
            rootScope.$apply();
        });
    });

    describe('get client search details', function () {
        var businessKey = 123;
        this.getClientSearchResultDetailsPromise = '';

        beforeEach(function () {
            this.getClientSearchDetailQuery = $searchQueries.GetClientSearchDetailQuery(businessKey);
            spyOn($searchQueries, 'GetClientSearchDetailQuery').and.returnValue(this.getClientSearchDetailQuery);
            spyOn($searchWebService, 'sendQueryAsync').and.returnValue(deferred.promise);

            $searchManagerService.getClientSearchResultDetails(businessKey);
        });

        it('should call the query service to get the clients search data', function () {
            expect($searchWebService.sendQueryAsync).toHaveBeenCalled();
        });

        it('should return the client data when promise is resolved', function () {
                var data = {
                    data: {
                        ReturnData: {
                            QueryDurationInMilliseconds: 100,
                            ResultCountInAllPages: 355,
                            Results: {
                                $values: 'client search metadata'
                            }
                        }
                    }
                };
                deferred.resolve(data);
                rootScope.$apply();
        });
    });

    describe('search for third parties', function () {
        var freeTextTerms = 'straus';
        var indexName = 'ThirdParty';
        var pageSize = 10;
        var currentPage = 2;
        var filters = [];

        beforeEach(function () {
            var searchForThirdPartyQuery = $searchQueries.SearchForThirdPartyQuery(freeTextTerms, filters, indexName);

            spyOn($searchQueries, 'SearchForThirdPartyQuery').and.returnValue(searchForThirdPartyQuery);
            spyOn($searchWebService, 'sendQueryAsync').and.returnValue(deferred.promise);

            $searchManagerService.getNewSearchTypesAndFilters('searchTypeName', []);
            $searchManagerService.searchForClients(freeTextTerms, indexName, pageSize, currentPage);
        });


        it('should call the query service to get the clients data', function () {
            expect($searchWebService.sendQueryAsync).toHaveBeenCalled();
        });

        it('should return the third party data when promise is resolved', function () {
            var data = {
                data: {
                    ReturnData: {
                        QueryDurationInMilliseconds: 100,
                        ResultCountInAllPages: 355,
                        Results: {
                            $values: 'third party metadata'
                        }
                    }
                }
            };
            deferred.resolve(data);
            rootScope.$apply();
        });
    });

    describe('get third party search result details', function () {
        var businessKey = 123;

        beforeEach(function () {
            var getThirdPartySearchDetailQuery = $searchQueries.GetThirdPartySearchDetailQuery(businessKey);
            spyOn($searchQueries, 'GetThirdPartySearchDetailQuery').and.returnValue(getThirdPartySearchDetailQuery);
            spyOn($searchWebService, 'sendQueryAsync').and.returnValue(deferred.promise);

            $searchManagerService.getThirdPartySearchResultDetails(businessKey);
        });

        it('should call the query service to get the third party data', function () {
            expect($searchWebService.sendQueryAsync).toHaveBeenCalled();
        });

        it('should return the third party data when promise is resolved', function () {
            var data = {
                data: {
                    ReturnData: {
                        QueryDurationInMilliseconds: 100,
                        ResultCountInAllPages: 355,
                        Results: {
                            $values: 'third party metadata'
                        }
                    }
                }
            };
            deferred.resolve(data);
            rootScope.$apply();
        });
    });

    describe('search for tasks', function () {
        var freeTextTerms = 'straus';
        var indexName = 'Tasks';
        var pageSize = 10;
        var currentPage = 2;
        var filters = [];

        beforeEach(function () {
            var searchForTaskQuery = $searchQueries.SearchForTaskQuery(freeTextTerms, filters, indexName);

            spyOn($searchQueries, 'SearchForTaskQuery').and.returnValue(searchForTaskQuery);
            spyOn($searchWebService, 'sendQueryAsync').and.returnValue(deferred.promise);

            $searchManagerService.getNewSearchTypesAndFilters('searchTypeName', []);
            $searchManagerService.searchForTasks(freeTextTerms, indexName, pageSize, currentPage);
        });


        it('should call the query service to get the tasks data', function () {
            expect($searchWebService.sendQueryAsync).toHaveBeenCalled();
        });

        it('should return the tasks data when promise is resolved', function () {
            var data = {
                data: {
                    ReturnData: {
                        QueryDurationInMilliseconds: 100,
                        ResultCountInAllPages: 355,
                        Results: {
                            $values: 'tasks metadata'
                        }
                    }
                }
            };
            deferred.resolve(data);
            rootScope.$apply();
        });
    });

    describe('get task search result details', function () {
        var businessKey = 123;

        beforeEach(function () {
            var getTaskSearchDetailQuery = $searchQueries.GetTaskSearchDetailQuery(businessKey);
            spyOn($searchQueries, 'GetTaskSearchDetailQuery').and.returnValue(getTaskSearchDetailQuery);
            spyOn($searchWebService, 'sendQueryAsync').and.returnValue(deferred.promise);

            $searchManagerService.getTaskSearchResultDetails(businessKey);
        });

        it('should call the query service to get the tasks details', function () {
            expect($searchWebService.sendQueryAsync).toHaveBeenCalled();
        });

        it('should return the tasks details when promise is resolved', function () {
            var data = {
                data: {
                    ReturnData: {
                        QueryDurationInMilliseconds: 100,
                        ResultCountInAllPages: 355,
                        Results: {
                            $values: 'tasks metadata'
                        }
                    }
                }
            };
            deferred.resolve(data);
            rootScope.$apply();
        });
    });

    describe('search for third party invoices', function () {
        var freeTextTerms = 'inv';
        var indexName = 'ThirdPartyInvoices';
        var pageSize = 10;
        var currentPage = 2;
        var filters = [];

        beforeEach(function () {
            var searchForThirdPartyInvoicesQuery = $searchQueries.SearchForThirdPartyInvoicesQuery(freeTextTerms, filters, indexName);

            spyOn($searchQueries, 'SearchForTaskQuery').and.returnValue(searchForThirdPartyInvoicesQuery);
            spyOn($searchWebService, 'sendQueryAsync').and.returnValue(deferred.promise);

            $searchManagerService.getNewSearchTypesAndFilters('searchTypeName', []);
            $searchManagerService.searchForThirdPartyInvoices(freeTextTerms, indexName, pageSize, currentPage);
        });


        it('should call the query service to get the third party invoice data', function () {
            expect($searchWebService.sendQueryAsync).toHaveBeenCalled();
        });

        it('should return the third party invoice data when promise is resolved', function () {
            var data = {
                data: {
                    ReturnData: {
                        QueryDurationInMilliseconds: 100,
                        ResultCountInAllPages: 355,
                        Results: {
                            $values: 'tasks metadata'
                        }
                    }
                }
            };
            deferred.resolve(data);
            rootScope.$apply();
        });
    });

    describe('get workflow options', function () {
        var workflowName = 'Third Party Invoices';
        var workflowState = 'WORKFLOW_STATE';
        var data = [];

        beforeEach(function () {
            spyOn($lookupDataService, 'getLookup').and.returnValue(data);
            spyOn($searchManagerService, 'getWorkflowOptions').and.returnValue('');

            $lookupDataService.getLookup(workflowState);
            $searchManagerService.getWorkflowOptions(workflowName);
        });

        it('should get the lookup data', function () {
            expect($lookupDataService.getLookup).toHaveBeenCalled();
        });
    });

    describe('get invoice status filter', function () {
        var workflowName = 'Third Party Invoices';
        var workflowState = 'WORKFLOW_STATE';
        var data = [];

        beforeEach(function () {
            spyOn($lookupDataService, 'getLookup').and.returnValue(data);
            spyOn($searchManagerService, 'getInvoiceStatusFilterOptions').and.returnValue('');

            $lookupDataService.getLookup(workflowState);
            $searchManagerService.getInvoiceStatusFilterOptions();
        });

        it('should get the lookup data', function () {
            expect($lookupDataService.getLookup).toHaveBeenCalled();
        });
    });
});