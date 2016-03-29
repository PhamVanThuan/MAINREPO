'use strict';

angular.module('halo.start.portalpages.search', [
        'sahl.websites.halo.services.entityManagement',
        'sahl.websites.halo.services.searchManagement',
        'sahl.websites.halo.services.paginationHelpers',
        'sahl.js.core.activityManagement'
])
    .config(['$stateProvider',
        function ($stateProvider) {
            $stateProvider.state('start.portalPages.search', {
                url: 'search/',
                templateUrl: 'app/start/portalPages/search/search.tpl.html',
                controller: 'SearchCtrl'
            });
        }
    ])
    .controller('SearchCtrl', ['$scope', '$logger', '$entityManagerService', '$searchManagerService', '$timeout', '$paginationHelperService', '$activityManager','$toastManagerService',
        function ($scope, $logger, $entityManager, $searchManager, $timeout, $paginationHelper, $activityManager, $toastManagerService) {
            var searchPageSize = 8;
            var selectedSearch = null;
            var searchWord = null;

            // make sure we deactivate all entitys when we enter search mode
            $entityManager.deactivateEntity();
            // setup the searchTerm model
            $scope.searchTerm = "";
            // search tools should not be shown by default
            $scope.toolsVisible = false;
            // setup the searchFilters
            $scope.searchFilters = $searchManager.getNewSearchTypesAndFilters('clients',
            [{
                name: 'clients',
                label: 'Clients',
                index: 'Client',
                search: $searchManager.searchForClients,
                searchDetail: $searchManager.getClientSearchResultDetails,
                sortBy: ['Full Name', 'First Name', 'Surname'],
                filters: [{
                    filterType: 'dropdown',
                    label: 'LegalEntityType',
                    options: ['Any Client', 'Person', 'Business'],
                    selectedFilter: null
                }, {
                    filterType: 'dropdown',
                    label: 'Product',
                    options: ['Any Account Type', 'Mortgage Loan', 'Life', 'HOC', 'Personal Loan'],
                    selectedFilter: null
                }]
                    ,
                initialise: function (searchType) {
                }
            }, {
                name: 'thirdparties',
                label: 'Third Parties',
                index: 'ThirdParty',
                search: $searchManager.searchForThirdParties,
                searchDetail: $searchManager.getThirdPartySearchResultDetails,
                filters: [{
                    filterType: 'dropdown',
                    label: 'ThirdPartyType',
                    options: ['Any Third Party', 'Attorney', 'Valuer'],
                    selectedFilter: null
                }, {
                    filterType: 'dropdown',
                    label: 'ThirdPartySubType',
                    options: ['Any Role', 'Litigation Attorney', 'Registration Attorney'],
                    selectedFilter: null
                }]
                    ,
                initialise: function (searchType) {
                }
            }, {
                name: 'tasks',
                label: 'Tasks',
                index: 'Task',
                search: $searchManager.searchForTasks,
                searchDetail: $searchManager.getTaskSearchResultDetails,
                filters: [{
                    filterType: 'dropdown',
                    label: 'Status',
                    options: ['Any Status', 'In Progress', 'Archived'],
                    defaultOption: 'In Progress',
                    selectedFilter: null
                }, {
                    filterType: 'dropdown',
                    label: 'Workflow',
                    options: ['Any Workflow'],
                    selectedFilter: null
                }],
                initialise: function (searchType) {
                    searchType.filters[1].options.push.apply(searchType.filters[1].options, $searchManager.getWorkflowOptions());
                }
            }]
            );
            // setup the active searchFilter
            $scope.searchFilters.$activateDefaultSearchType();
            $searchManager.start();

            $scope.isActiveSearchType = function (searchType) {
                if (!_.isNull(searchType) && !_.isNull($scope.searchFilters.activeSearchType) && !_.isUndefined(searchType.name) && searchType.name === $scope.searchFilters.activeSearchType.name) {
                    return true;
                }
                return false;
            };

            $scope.currentPage = 1;

            $scope.clearSearchResults = function () {
                $scope.searchResult = {
                    queryDuration: 0,
                    numberOfResults: 0,
                    numberOfPages: 0,
                    results: [],
                    resultDetails: [],
                    currentResult: null,
                    currentDetail: null,
                    pages: {
                        isNextPageEnabled: false,
                        isNextPageGroupEnabled: false,
                        isPrevPageEnabled: false,
                        isPrevPageGroupEnabled: false,
                        pages: []
                    }
                };
            };

            // setup the search results model
            $scope.clearSearchResults();
            var latestRequest = 0;
            var debouncedSearch = _.debounce(function () {
                if ($scope.searchTerm === '') {
                    $scope.clearSearchResults();
                } else {

                    $activityManager.startActivityWithKey("searching");
                    searchWord = $scope.searchTerm;
                    var thisRequest = ++latestRequest;
                    $scope.searchFilters.activeSearchType.search($scope.searchTerm, $scope.searchFilters.activeSearchType.index, searchPageSize, $scope.currentPage)
                        .then(function (data) {
                            if (thisRequest === latestRequest) {
                                $scope.searchResult = data;
                                $scope.searchResult.resultDetails = [];
                                $scope.searchResult.currentDetail = null;
                                $scope.searchResult.pages = $paginationHelper.buildPagination(data.numberOfPages, searchPageSize, $scope.currentPage);
                                $activityManager.stopActivityWithKey("searching");
                            }
                        }, function () {
                            $toastManagerService.error({
                                title: 'Error',
                                text: 'A problem occurred while processing the request. Please try again later.'
                            });
                            $activityManager.stopActivityWithKey("searching");
                        });
                }
            }, 400);

            $scope.search = function (pageNumber) {
                if (_.isUndefined(pageNumber)) {
                    $scope.currentPage = 1;
                } else {
                    if (pageNumber > 0) {
                        $scope.currentPage = pageNumber;
                    }
                }
                debouncedSearch();
            };

            $scope.keyPressed = function ($event) {
                if ($event.keyCode === 13 || $event.keyCode === 8) {
                    if ($scope.searchTerm === '') {
                        $scope.clearSearchResults();
                    } else {
                        $scope.search();
                    }
                }
            };

            $scope.setSearchType = function (searchType) {
                if ($scope.searchFilters.activeSearchType !== searchType) {
                    // activate the selected searchtype
                    $scope.searchFilters.$activateSearchType(searchType.name);

                    // clear the current result set
                    $scope.clearSearchResults();

                    // perform the search
                    $scope.search();
                }
            };

            $scope.loadClientEntity = function (entityToAdd) {
                var entity = $entityManager.createEntity(entityToAdd.LegalName, 3, entityToAdd.LegalEntityKey, '', 'client');
                $entityManager.addEntity(entity);
                $entityManager.makeEntityActive(entity);
            };

            $scope.loadThirdPartyEntity = function (entityToAdd) {
                var entity = $entityManager.createEntity(entityToAdd.LegalName, 56, entityToAdd.LegalEntityKey, '', 'thirdparty');
                $entityManager.addEntity(entity);
                $entityManager.makeEntityActive(entity);
            };
            $scope.loadTaskEntity = function (entityToAdd) {
                var moduleParameters = entityToAdd.Process + " - " + entityToAdd.Workflow;
                var entity = $entityManager.createEntity(entityToAdd.Subject, entityToAdd.GenericKeyTypeKey, entityToAdd.GenericKeyValue, entityToAdd.InstanceId, 'task', null, moduleParameters);
                $entityManager.addEntity(entity);
                $entityManager.makeEntityActive(entity);
            };

            $scope.resetFilters = function () {
                if (!_.isNull($scope.searchFilters.activeSearchType) && $scope.searchFilters.$getActiveFilters().length > 0) {
                    $scope.searchFilters.$activateDefaultFilters($scope.searchFilters.activeSearchType);
                    $scope.search();
                }
            };

            var updateSearchDetails = function (currentResult, resultDetail) {
                $timeout(function () {
                    $scope.searchResult.currentResult = currentResult;
                    $scope.searchResult.currentDetail = resultDetail;
                });
            };

            $scope.getSearchResultDetail = function (currentResult, businessKey) {
                // check if we have the result detail already
                if ($scope.searchTerm.length > 0) {
                    var resultDetail = _.findWhere($scope.searchResult.resultDetails, {
                        "businessKey": businessKey
                    });

                    if (_.isUndefined(resultDetail)) {
                        selectedSearch = businessKey;
                        $scope.searchFilters.activeSearchType.searchDetail(businessKey).then(function (data) {
                            if (selectedSearch === businessKey) {
                                var resultDetail = {
                                    businessKey: businessKey,
                                    Details: data.results
                                };
                                updateSearchDetails(currentResult, resultDetail);
                            }
                        }, function () {
                            $toastManagerService.error({
                                title: 'Error',
                                text: 'A problem occurred while processing the request. Please try again later.'
                            });
                        });
                    } else {
                        updateSearchDetails(currentResult, resultDetail);
                    }
                }
            };

            $scope.isSelectedResult = function (businessKey) {
                if ($scope.searchResult.currentDetail !== null && $scope.searchResult.currentDetail.businessKey === businessKey) {
                    return true;
                }

                return false;
            };

            $scope.getDateTimeAgo = function (dateStr, hideSufix) {
                if (_.isUndefined(hideSufix)) {
                    hideSufix = false;
                }
                if (!_.isUndefined(dateStr)) {
                    var a = moment(dateStr);
                    var b = a.fromNow(hideSufix);
                    return b;
                } else {
                    return "";
                }
            };

            $scope.clear = function () {
                $scope.searchTerm = "";
                $scope.clearSearchResults();
            };

            $scope.getDate = function (dateStr) {
                return moment(dateStr).format("Do MMM YYYY, HH:mm:ss");
            };

            $scope.validData = function (input) {
                if (input) {
                    return (input.trim().length > 0);
                }
                return '';
            };
        }
    ]);
