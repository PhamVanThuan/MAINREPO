'use strict';

angular.module('LossControl.Invoices.InvoicesRootTileConfiguration.tpl.html',[])
  .controller('LossControl_Invoices_InvoicesRootTileConfigurationCtrl',['$scope', '$stateParams','$toastManagerService','$q',
              '$state','$rootScope', '$pageFactory', '$activityManager','$paginationHelperService','$filter',
              '$lookupService','$searchManagerService','$entityManagerService',
    function($scope, $stateParams, $toastManagerService, $q, $state, $rootScope, $pageFactory, $activityManager, $paginationHelperService, 
              $filter, $lookupService, $searchManagerService, $entityManagerService){
                  
      var searchPageSize = 15;
      
      var searchWord = null;
      var latestRequest = 0;
      var invoiceStatusList = [];
      var rejectedPaidFilter = { 'name': '-InvoiceStatusKey', 'value': '(5, 6)' };
        
      // search tools should not be shown by default
            $scope.toolsVisible = false;
            // setup the searchFilters
            

      /* initialise the controller and scope */
      function initialiseController(){
          // search tools should not be shown by default
          $activityManager.startActivityWithKey("loading");
          $scope.toolsVisible = false;
          
          setSearchPage();

          $scope.invoiceListGrid = gridUtility.gridSettings;   
          // invoice search status  
          $scope.selectedFilter = "Any Status";   
          $scope.searchFilter = $searchManagerService;
          $scope.searchTerm = '';
          $scope.currentPage = 1;        
        
          $scope.invoiceResult = {};
          $scope.invoiceList = {};
      }

      var setSearchPage = function (){
          getInvoiceStatusFilterOptions().then(function(){

          $scope.searchFilters = $searchManagerService.getNewSearchTypesAndFilters('thirdpartyInvoiceStatus', 
            [{
                    name: 'thirdpartyInvoiceStatus',
                    label: 'Invoices',
                    index: 'ThirdPartyInvoice',
                    search: null,
                    searchDetail: null,
                    filters: [{
                        filterType: 'dropdown',
                        label: 'InvoiceStatusDescription',
                        options: invoiceStatusList,
                        defaultOption:'Any Status',
                        selectedFilter: null
                    }]
                    ,
                    initialise: function (searchType) {
                       
                    }
              }]
            );
          $scope.searchFilters.$activateDefaultSearchType(); 
          getThirdPartyInvoices();

          });
      };

      
       /* filter drop-down for invoice status */
      var getInvoiceStatusFilterOptions  = function (){
        var deferred = $q.defer();        
         $lookupService.getByLookupType('InvoiceStatus').then(function(data){
              invoiceStatusList = _.union(["Any Status"],_.pluck(data.data._embedded.InvoiceStatus, 'description'));
             
             //remove Rejected and Paid statuses
              invoiceStatusList = _.without(invoiceStatusList, 'Rejected', 'Paid');
             
              deferred.resolve();
          });      
          return deferred.promise;   
      };

      /**
      * Sets the selected filter
      */
      $scope.setInvoiceStatusDropDownFilter = function (selectedFilter) {
          //clear search term as you should not beable to filter and search at the same time.  They are exclusive.
          $scope.searchTerm = '';
          $scope.selectedFilter = selectedFilter;                   
      };

      /** Clears the search results **/
      var clearSearchResults = function(){
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

      /** Calls SearchManagerService to retrieve the solr data for all invoices **/
      var getThirdPartyInvoices = function(){
        var latestRequest = 0;
        $activityManager.startActivityWithKey("searching");       
        var thisRequest = ++latestRequest;

          searchWord = "*";
          
          $searchManagerService.searchForThirdPartyInvoices(searchWord, 'ThirdPartyInvoice', 
                        searchPageSize, $scope.currentPage, rejectedPaidFilter)
          .then(function(data) {
              if (thisRequest === latestRequest) {
                  $scope.searchResult = data;
                  $scope.searchResult.resultDetails = [];
                  buildGridData($scope.searchResult.results);                 
                  $scope.searchResult.currentDetail = null;
                  $scope.searchResult.pages = $paginationHelperService.buildPagination(data.numberOfPages, searchPageSize, $scope.currentPage);
                  $activityManager.stopActivityWithKey("searching");
                  $(window).trigger('resize');

              $activityManager.stopActivityWithKey("searching");
              }
          }, function() {
              $activityManager.stopActivityWithKey("searching");
              $(window).trigger('resize');
          }); 
      };

      /** Debounce function for search **/                        
      var debouncedSearch = _.debounce(function() {
          if ($scope.searchTerm === '') {
             clearSearchResults();
             getThirdPartyInvoices();
          } else {              
              $activityManager.startActivityWithKey("searching");
              searchWord = $scope.searchTerm;
              var thisRequest = ++latestRequest;
              //reset the dropdown filter to all.

              $searchManagerService.searchForThirdPartyInvoices(searchWord, 'ThirdPartyInvoice', 
                                                                searchPageSize, $scope.currentPage, rejectedPaidFilter)
                  .then(function(data) {
                      if (thisRequest === latestRequest) {
                          $scope.searchResult = data;
                          $scope.searchResult.resultDetails = [];
                          buildGridData($scope.searchResult.results);
                          $scope.searchResult.currentDetail = null;
                          $scope.searchResult.pages = $paginationHelperService.buildPagination(data.numberOfPages, searchPageSize, $scope.currentPage);
                          $activityManager.stopActivityWithKey("searching");
                      }
                  }, function() {
                      $activityManager.stopActivityWithKey("searching");
                  });
          }
      }, 400);

      /** Returns the stage in workflow in a comma seperated list **/
      var workflowStageList = function(workflowStage){
          var result = '';         
          if(workflowStage.$values.length > 1){
            result = _.each(workflowStage.$values, ',', result);
          }else{
            result  = workflowStage.$values[0];
          }
          return result;
      };

      /** Assign the columns that the user will see to a itemList object and to scope **/
      var buildGridData = function(data){         
         var jsonInvoiceArray = [];                            
        for(var i = 0; i < data.length; i++){
          var itemList = { 
            "AssignedTo": data[i].AssignedTo, 
            "ReceivedDate": data[i].ReceivedDate,
            "AccountKey": data[i].AccountKey,
            "InvoiceNumber": data[i].InvoiceNumber,
            "InvoiceStatusDescription": data[i].InvoiceStatusDescription,
            "TotalAmountIncludingVAT": data[i].TotalAmountIncludingVAT,
            "SpvDescription": data[i].SpvDescription,
            "WorkflowStage": workflowStageList(data[i].WorkflowStage),
            "GenericKey" : data[i].GenericKey,
            "InstanceID" : data[i].InstanceID,
            "ReceivedFromEmailAddress" : data[i].ReceivedFromEmailAddress,
            "ThirdParty" : data[i].ThirdParty
          };
          jsonInvoiceArray.push(itemList);
        }
        $scope.invoiceList = jsonInvoiceArray;              
        $scope.invoiceListGrid.data = jsonInvoiceArray;
      };

      /* pagination */
      $scope.search = function(pageNumber) {
          if (_.isUndefined(pageNumber)) {
              $scope.currentPage = 1;
          } else {
              if (pageNumber > 0) {
                  $scope.currentPage = pageNumber;
              }
          }
          
          debouncedSearch();
      };

      $scope.resetFilters = function () {
                if (!_.isNull($scope.searchFilters.activeSearchType) && $scope.searchFilters.$getActiveFilters().length > 0) {
                    $scope.searchFilters.$activateDefaultFilters($scope.searchFilters.activeSearchType);
                    $scope.search();
                }
            };

      $scope.clear = function () {
            if($scope.searchTerm != ""){
              $scope.searchTerm = "";
              $scope.search();
            }
          };

      $scope.taskSelected = function (item) {
          var moduleParameters = "Third Party Invoices - Third Party Invoices";
          var displayName = "";
          if(item.ThirdParty == "Unassigned"){
              displayName = item.ReceivedFromEmailAddress;
          }else{
              displayName = item.ThirdParty;
          }
          var entity = $entityManagerService.createEntity(displayName, 54, item.GenericKey, item.InstanceID, 'task', null, moduleParameters);
          $entityManagerService.addEntity(entity);
          $entityManagerService.makeEntityActive(entity);
      };

      /* Resize grid to be the height of the data */
      $scope.getTableHeight = function(){
          var rowHeight = 30; // your row height
          var headerHeight = 30; // your header height     
          if(!_.isUndefined($scope.invoiceListGrid.data)) 
          {
            return {
              height: ($scope.invoiceListGrid.data.length * rowHeight + headerHeight) + "px"
            };
          }
      };
      
      /* gridFunctions */
      var gridUtility = {
        gridSettings: {              
              multiSelect: false,
              enableRowSelection: true,                  
              selectionRowHeaderWidth: 30,
              enableHorizontalScrollbar : 0,
              enableVerticalScrollbar: 0,              
              enableRowHeaderSelection: false,
              data: {length : 0},
              paginationPageSize: searchPageSize,   
              autoresize : true,                           
                columnDefs: [  
                  { name: 'AccountKey', displayName: 'Account Number', enableCellEdit: false, cellClass:'right-align-text', enableGrouping: true, enableFiltering: true },
                  { name: 'InvoiceNumber', displayName: 'Invoice Number', enableCellEdit: false, enableGrouping: true, enableFiltering: true},                   
                  { name: 'InvoiceStatusDescription', displayName: 'Invoice Status', enableCellEdit: false, enableGrouping: true, enableFiltering: true}, 
                  { name: 'TotalAmountIncludingVAT', cellFilter: 'currencyFilter', cellClass:'right-align-text', displayName: 'Total', enableCellEdit: false, enableGrouping: true, enableFiltering: true },
                  { name: 'SpvDescription', displayName: 'SPV', enableCellEdit: false, enableGrouping: true, enableFiltering: true },
                  { name: 'WorkflowStage', displayName: 'Stage in Loss Control', enableCellEdit: false, enableGrouping: true, enableFiltering: true },
                  { name: 'AssignedTo', displayName: 'Assigned To', enableCellEdit: false, enableGrouping: true, enableFiltering: true},
                  { name: 'ReceivedDate', displayName: 'Received', cellFilter: 'dateViewFilter', enableCellEdit: false, enableGrouping: true, enableFiltering: true},
                  { name: 'ReceivedFromEmailAddress', displayName: 'ReceivedFromEmailAddress', visible: false},
                  { name: 'InstanceID', displayName: 'InstanceID', visible: false  },
                  { name: 'GenericKey', displayName: 'GenericKey', visible: false },
                  { name: 'ThirdParty', displayName: 'ThirdParty', visible: false }
                ],                                              
            onRegisterApi: function(gridApi){  
                $scope.invoiceListGrid = gridApi;
                $scope.gridApi = gridApi;
                gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                    $scope.taskSelected(row.entity);
                });
            }           
        }
      };
      /* gridFunctions End */

      initialiseController();
}]);
