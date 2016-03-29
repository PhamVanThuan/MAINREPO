'use strict';
angular.module('sahl.websites.halo.services.searchManagement', [
        'halo.webservices',
        'SAHL.Services.Interfaces.Search.queries',
        'SAHL.Services.Interfaces.Search.sharedmodels',
        'SAHL.Services.Interfaces.WorkflowTask.queries',
        'sahl.websites.halo.services.lookupDataService'
])
    .service('$searchManagerService', ['$timeout', '$searchWebService', '$searchQueries', '$searchSharedModels', '$q', '$workflowTaskWebService', '$workflowTaskQueries', '$lookupDataService', '$lookupService',
        function ($timeout, $searchWebService, $searchQueries, $searchSharedModels, $q, $workflowTaskWebService, $workflowTaskQueries, $lookupDataService, $lookupService) {
            var WORKFLOW_STATE_LOOKUPS = "WORKFLOW_STATES";
            var searchFilters = {};

            var operations = {
                searchForClients: function (freeTextTerms, indexName, pageSize, currentPage) {
                    var deferred = $q.defer();
                    var filters = [];
                    var InvoiceStatus = [];
                    if (!_.isNull(searchFilters.activeSearchType)) {
                        filters = searchFilters.$getActiveFilters();
                    }

                    var query = new $searchQueries.SearchForClientQuery(freeTextTerms, filters, indexName);
                    $searchWebService.sendQueryAsync(query, {
                        currentPage: currentPage,
                        pageSize: pageSize
                    }).
                    then(function (data) {
                        deferred.resolve({
                            queryDuration: data.data.ReturnData.QueryDurationInMilliseconds,
                            numberOfResults: data.data.ReturnData.ResultCountInAllPages,
                            numberOfPages: data.data.ReturnData.NumberOfPages,
                            results: data.data.ReturnData.Results.$values
                        });
                    }, function (errorMsg) {
                        deferred.reject(errorMsg);
                    });

                    return deferred.promise;
                },
                getClientSearchResultDetails: function (businessKey) {
                    var deferred = $q.defer();

                    var query = new $searchQueries.GetClientSearchDetailQuery(businessKey);
                    $searchWebService.sendQueryAsync(query).
                    then(function (data) {
                        deferred.resolve({
                            results: data.data.ReturnData.Results.$values
                        });
                    }, function (errorMsg) {
                        deferred.reject(errorMsg);
                    });

                    return deferred.promise;
                },
                searchForThirdParties: function (freeTextTerms, indexName, pageSize, currentPage) {
                    var deferred = $q.defer();
                    var filters = [];
                    if (!_.isNull(searchFilters.activeSearchType)) {
                        filters = searchFilters.$getActiveFilters();
                    }

                    var query = new $searchQueries.SearchForThirdPartyQuery(freeTextTerms, filters, indexName);
                    $searchWebService.sendQueryAsync(query, {
                        currentPage: currentPage,
                        pageSize: pageSize
                    }).
                    then(function (data) {
                        deferred.resolve({
                            queryDuration: data.data.ReturnData.QueryDurationInMilliseconds,
                            numberOfResults: data.data.ReturnData.ResultCountInAllPages,
                            numberOfPages: data.data.ReturnData.NumberOfPages,
                            results: data.data.ReturnData.Results.$values
                        });
                    }, function (errorMsg) {
                        deferred.reject(errorMsg);
                    });

                    return deferred.promise;
                },               
                getThirdPartySearchResultDetails: function (businessKey) {
                    var deferred = $q.defer();

                    var query = new $searchQueries.GetThirdPartySearchDetailQuery(businessKey);
                    $searchWebService.sendQueryAsync(query).
                    then(function (data) {
                        deferred.resolve({
                            results: data.data.ReturnData.Results.$values
                        });
                    }, function (errorMsg) {
                        deferred.reject(errorMsg);
                    });

                    return deferred.promise;
                },
                searchForTasks: function (freeTextTerms, indexName, pageSize, currentPage) {
                    var deferred = $q.defer();
                    var filters = [];
                    if (!_.isNull(searchFilters.activeSearchType)) {
                        filters = searchFilters.$getActiveFilters();
                    }

                    var query = new $searchQueries.SearchForTaskQuery(freeTextTerms, filters, indexName);
                    $searchWebService.sendQueryAsync(query, {
                        currentPage: currentPage,
                        pageSize: pageSize
                    }).
                    then(function (data) {
                        deferred.resolve({
                            queryDuration: data.data.ReturnData.QueryDurationInMilliseconds,
                            numberOfResults: data.data.ReturnData.ResultCountInAllPages,
                            numberOfPages: data.data.ReturnData.NumberOfPages,
                            results: data.data.ReturnData.Results.$values
                        });
                    }, function (errorMsg) {
                        deferred.reject(errorMsg);
                    });

                    return deferred.promise;
                },
                getTaskSearchResultDetails: function (businessKey) {
                    var deferred = $q.defer();

                    var query = new $searchQueries.GetTaskSearchDetailQuery(businessKey);
                    $searchWebService.sendQueryAsync(query).
                    then(function (data) {
                        var results = data.data.ReturnData.Results.$values;
                        // now get the task history
                        var queryHist = new $searchQueries.GetTaskHistoryQuery(businessKey);
                        $searchWebService.sendQueryAsync(queryHist).
                        then(function (data) {
                            if (results.length === 1) {
                                results[0].taskHistory = data.data.ReturnData.Results.$values;
                            }

                            deferred.resolve({
                                results: results
                            });
                        });
                    }, function (errorMsg) {
                        deferred.reject(errorMsg);
                    });

                    return deferred.promise;
                },
                searchForThirdPartyInvoices: function (freeTextTerms, indexName, pageSize, currentPage, moreFilters) {
                    var deferred = $q.defer();
                    var filters = [];
                    if (!_.isNull(searchFilters.activeSearchType)) {
                        filters = searchFilters.$getActiveFilters();
                        
                        if (moreFilters) {
                            filters.push(moreFilters);
                        }
                    }
                    
                    var query = new $searchQueries.SearchForThirdPartyInvoicesQuery(freeTextTerms, filters, indexName);
                    $searchWebService.sendQueryAsync(query, {
                        currentPage: currentPage,
                        pageSize: pageSize
                    }).
                    then(function (data) {
                        deferred.resolve({
                            queryDuration: data.data.ReturnData.QueryDurationInMilliseconds,
                            numberOfResults: data.data.ReturnData.ResultCountInAllPages,
                            numberOfPages: data.data.ReturnData.NumberOfPages,
                            results: data.data.ReturnData.Results.$values
                        });
                    });

                    return deferred.promise;
                },              
                getStateOptions: function (workflowName) {
                    var data = $lookupDataService.getLookup(WORKFLOW_STATE_LOOKUPS);
                    return _.find(data, function (dataItem) {
                        return dataItem.workflow === workflowName;
                    }).states;
                },
                getWorkflowOptions: function () {
                    var data = $lookupDataService.getLookup(WORKFLOW_STATE_LOOKUPS);
                    var workflows = _.pluck(data, 'workflow');
                    return workflows;
                },
                getInvoiceStatusFilterOptions:function (){
                    var InvoiceStatus = [];
                    InvoiceStatus = $lookupService.getByLookupType('InvoiceStatus')
                    //    .then(function (data) {
                    //    InvoiceStatus = data.data._embedded.InvoiceStatus;
                    //    $("[data-role=dropdown]").dropdown(); 
                    //});
                    return InvoiceStatus;
                },
                createSearchFilters: function (defaultSearchTypeName, searchTypes) {
                    return {
                        documentInfo: {
                            version: 0.1,
                            type: 'searchFilter'
                        },
                        defaultSearchTypeName: defaultSearchTypeName,
                        activeSearchType: null,
                        searchTypes: searchTypes,
                        $activateSearchType: function (searchTypeName) {
                            var searchType = _.find(searchFilters.searchTypes, function (searchType) {
                                return searchType.name === searchTypeName;
                            });
                            if (!_.isNull(searchType)) {
                                searchFilters.activeSearchType = searchType;

                                // set the default filter
                                searchFilters.$activateDefaultFilters(searchType);

                                // re-initialise the dropdowns
                                $timeout(function () {
                                    $("[data-role=dropdown]").dropdown();
                                });
                            }
                        },
                        $activateDefaultSearchType: function () {
                            searchFilters.$activateSearchType(searchFilters.defaultSearchTypeName);
                        },
                        $activateDefaultFilters: function (searchType) {
                            _.each(searchType.filters, function (filter) {
                                if (filter.filterType === 'dropdown' && filter.options.length >= 0) {
                                    if (_.isUndefined(filter.defaultOption)) {
                                        filter.selectedFilter = filter.options[0];
                                    }
                                    else {
                                        var defaultFilter = filter.defaultOption;
                                        var selectedFilter = _.find(filter.options, function (item) {
                                            return item === defaultFilter;
                                        });
                                        if (!_.isUndefined(selectedFilter)) {
                                            filter.selectedFilter = selectedFilter;
                                        } else {
                                            filter.selectedFilter = filter.options[0];
                                        }
                                    }
                                }

                                if (!_.isUndefined(filter.cascadeSource) && filter.cascadeSource !== '') {
                                    filter.options.length = 1;
                                }
                            });
                            searchFilters.$hasActiveFilters = false;
                        },
                        $selectOption: function (searchfilter, option) {
                            // validate that the filter is an option
                            searchfilter.selectedFilter = option;

                            // check if other filters cascade from this one
                            var hasActiveFilters = false;
                            _.each(searchFilters.activeSearchType.filters, function (filter) {
                                // clear the options except the first one
                                if (filter !== searchfilter && !_.isUndefined(filter.cascadeSource) && filter.cascadeSource === searchfilter.label) {
                                    filter.options.length = 1;
                                    if (!_.isUndefined(filter.cascadeAction)) {
                                        filter.options.push.apply(filter.options, filter.cascadeAction(option));
                                        filter.selectedFilter = filter.options[0];
                                    }
                                }

                                if (searchFilters.$isNonDefaultOptionSelected(filter)) {
                                    hasActiveFilters = true;
                                }
                            });

                            searchFilters.$hasActiveFilters = hasActiveFilters;
                        },
                        $isSelectedOption: function (searchFilter, option) {
                            if (searchFilter.selectedFilter === option) {
                                return true;
                            }
                            return false;
                        },
                        $isNonDefaultOptionSelected: function (searchFilter) {
                            if (!_.isNull(searchFilter.selectedFilter) && _.indexOf(searchFilter.options, searchFilter.selectedFilter) !== 0) {
                                return true;
                            }
                            return false;
                        },
                        $getActiveFilters: function () {
                            var activeFilters = [];
                            _.each(searchFilters.activeSearchType.filters, function (filter) {
                                if (!_.isNull(filter.selectedFilter) && filter.options.length > 0 && filter.selectedFilter !== filter.options[0]) {
                                    activeFilters.push({
                                        name: filter.label,
                                        value: filter.selectedFilter
                                    });
                                }
                            });
                            return activeFilters;
                        },
                        $getActiveFilter: function (filterName) {
                            return _.find(searchFilters.activeSearchType.filters, function (filter) {
                                return filter.label === filterName;
                            });
                        },
                        $hasActiveFilters: false
                    };
                }

            };

            return {
                getSearchTypesAndFilters: function () {
                    return searchFilters;
                },
                getNewSearchTypesAndFilters: function (defaultSearchTypeName, searchTypes) {
                    searchFilters = operations.createSearchFilters(defaultSearchTypeName, searchTypes);
                    return searchFilters;
                },
                searchForThirdPartyInvoices: operations.searchForThirdPartyInvoices,
                searchForClients: operations.searchForClients,
                searchForThirdParties: operations.searchForThirdParties,
                getSearchResultDetails: operations.getSearchResultDetails,
                getInvoiceStatusFilterOptions: operations.getInvoiceStatusFilterOptions,
                getClientSearchResultDetails: operations.getClientSearchResultDetails,
                getThirdPartySearchResultDetails: operations.getThirdPartySearchResultDetails,
                getTaskSearchResultDetails: operations.getTaskSearchResultDetails,
                getWorkflowOptions: operations.getWorkflowOptions,
                searchForTasks: operations.searchForTasks,
                start: function () {
                    // load the static lookup data for workflows
                    var query = new $workflowTaskQueries.GetAllWorkflowStatesQuery();
                    $workflowTaskWebService.sendQueryAsync(query).
                    then(function (data) {
                        var results = data.data.ReturnData.Results.$values;

                        // store the results using the lookup manager
                        var dataItems = [];
                        var currentWorkflow = '';
                        var currentWorkflowData = null;
                        _.each(results, function (item) {
                            var workflow = item.Workflow;
                            var state = item.State;

                            // check if the workflow exists yet
                            if (currentWorkflow !== workflow) {
                                currentWorkflowData = {
                                    workflow: workflow,
                                    states: []
                                };
                                dataItems.push(currentWorkflowData);
                                currentWorkflow = workflow;
                            }

                            currentWorkflowData.states.push(state);
                        });

                        $lookupDataService.addLookup(WORKFLOW_STATE_LOOKUPS, dataItems);

                        // initialise the search types
                        _.each(searchFilters.searchTypes, function (searchType) {
                            searchType.initialise(searchType);
                        });
                    });
                }
            };
        }
    ]);