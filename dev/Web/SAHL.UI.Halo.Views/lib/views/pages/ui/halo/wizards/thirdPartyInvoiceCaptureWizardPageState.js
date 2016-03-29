'use strict';
angular.module('sahl.ui.halo.wizards.thirdPartyInvoiceCaptureWizardPageState.tpl.html',[])
.controller('UI_Halo_Wizards_ThirdPartyInvoiceCaptureWizardPageStateCtrl',['$scope', 
    '$stateParams', '$queryServiceRest', '$activityManager','$queryWebService','$q',
	function($scope, $stateParams, $queryServiceRest, $activityManager, $queryWebService, $q){
            function initialiseController(){
                $scope.invoiceData = {};
                $scope.invoice = {}; 
                $scope.lineItems = {};

                $activityManager.startActivityWithKey("loading");
                getInvoiceData($stateParams.businessContext);
                getInvoiceLineItems($stateParams.businessContext);
                $activityManager.stopActivityWithKey("loading");
            }

            
             $scope.invoiceGrid = { 
                  multiSelect: true,
                  enableRowSelection: true,
                  enableSelectAll: true,
                  selectionRowHeaderWidth: 35,
                  enableFiltering: true,
                  paginationPageSizes: [20, 30, 50, 100],
                  columnDefs: [
                    { name: 'lineItemType', displayName: 'Type', width: '25%', enableGrouping: true, enableFiltering: true },
                    { name: 'lineItemDesc', displayName: 'Description', enableGrouping: true, enableFiltering: true },
                    { name: 'lineItemAmount', displayName: 'Amount', cellFilter: 'currencyFilter', enableGrouping: false, enableFiltering: false },
                    { name: 'isVatable', displayName: 'Is Vatable', cellClass: 'right-align-text', enableGrouping: true, enableFiltering: true },
                    { name: 'lineItemVatAmount', displayName: 'VAT', cellFilter: 'currencyFilter', enableGrouping: false, enableFiltering: false },
                    { name: 'lineItemTotalAmtInclVAT', displayName: 'Amount Including VAT', cellFilter: 'currencyFilter', enableGrouping: false, enableFiltering: false }
                                      
                ]
              };

              $scope.invoiceGrid.onRegisterApi = function(gridApi){             
                $scope.gridApi = gridApi;
                  gridApi.selection.on.rowSelectionChanged($scope,function(row){
                      var msg = 'row selected ' + row.isSelected;               
                  });
              };


              $scope.toggleMultiSelect = function() {
                  $scope.gridApi.selection.setMultiSelect(!$scope.gridApi.grid.options.multiSelect);
              };

              $scope.toggleModifierKeysToMultiSelect = function() {
                  $scope.gridApi.selection.setModifierKeysToMultiSelect(!$scope.gridApi.grid.options.modifierKeysToMultiSelect);
              };

              $scope.selectAll = function() {
                  $scope.gridApi.selection.selectAllRows();
              };

              $scope.clearAll = function() {
                  $scope.gridApi.selection.clearSelectedRows();
              };

              $scope.toggleRow1 = function() {
                  $scope.gridApi.selection.toggleRowSelection($scope.invoiceGrid.data[0]);
              };

              $scope.deleteSelected = function(){
                angular.forEach($scope.gridApi.selection.getSelectedRows(), function (data, index) {
                  $scope.invoiceGrid.data.splice($scope.invoiceGrid.data.lastIndexOf(data), 1);
                });
              };

            var getInvoiceData = function(invoiceId){
                var query = $queryServiceRest.ThirdPartyInvoice.getById(invoiceId);
                var includes = ['documents'];
                query.include(includes);
                $queryWebService.getQueryAsync(query).then(function(data) {
                        $scope.invoiceData = data.data;
                        $scope.invoiceDocument = $scope.invoice._embedded.documents._embedded.thirdPartyInvoiceDocument[0];
                    });
            };

            var getInvoiceLineItems = function(invoiceId){
                var plsetakeout = 2;  //HARDCODED ID OVER HERE! : Take out.  Using this as I don't have any data for the passed in id all the time.
                var query = $queryServiceRest.ThirdPartyInvoiceLineItems.getByInvoiceIdIncludeLineItems(plsetakeout);
                    $queryWebService.getQueryAsync(query).then(function(data) {
                        if(data.data.totalCount > 0){
                            $scope.invoiceGrid.invoiceHasLineItems = true;
                            $scope.lineItems = data.data._embedded.thirdPartyInvoiceLineItems;
                            $scope.invoiceGrid.data = data.data._embedded.thirdPartyInvoiceLineItems;   
                        }
                    });
            };

            $scope.loadPdf = function(documentGuid){
                //do some thing                                     
                doucmentModel = {
                    $type : "SAHL.UI.Halo.Models.ThirdParty.Invoices.InvoiceDocumentChildModel, SAHL.UI.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
                    DocumentGuid : '{'+documentGuid+'}'
                }
                // operations.loadRootTileConfiguration("Invoice Document Details",
                //                                          invoiceId,
                //                                          0,
                //                                           $stateParams.entityId,
                //                                          true,
                //                                          doucmentModel);
                var documents = doucmentModel;
            };
           
            initialiseController();
}]);