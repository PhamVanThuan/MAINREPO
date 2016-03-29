'use strict';
angular.module('LossControlManager.InvoicePaymentsRootTileConfiguration.tpl.html', ['SAHL.Services.Interfaces.DomainProcessManagerProxy.commands', 'SAHL.Services.Interfaces.DomainProcessManagerProxy.sharedmodels', 'SAHL.Services.Interfaces.CATS.queries'])
.controller('LossControlManager_InvoicePaymentsRootTileConfigurationCtrl',
 ['$scope', 'uiGridConstants', '$thirdPartyInvoiceStatus', '$logger', '$activityManager', '$q', '$searchManagerService', '$documentDownloadManagerService',
 	'$domainProcessManagerProxyCommands', '$domainProcessManagerProxySharedModels', '$domainProcessManagerProxyWebService', '$catsWebService', '$cATSQueries', '$toastManagerService',
 	function ($scope, uiGridConstants, $thirdPartyInvoiceStatus, $logger, $activityManager, $q, $searchManagerService, $documentDownloadManagerService,
 		 $domainProcessManagerProxyCommands, $domainProcessManagerProxySharedModels, $domainProcessManagerProxyWebService, $catsWebService, $cATSQueries, $toastManagerService) {
 	    var spinString = "loadingLossControlInvoices";
 	    var LOSS_CONTROL_STORE_ID = 44;
 	    var gridCellHeaderDirective = '<grid-cell-header></grid-cell-header>';

 	    function initialiseScope() {
            $scope.paymentCanBeMade = true;
 	        $scope.invoicesToPay = {
 	            $values: [],
 	            invoiceTotal: 0,
 	            invoiceCount: 0,
 	            calculateTotal: function () {
 	                var newTotal = $scope.sumInvoiceCollection($scope.invoicesToPay.$values);
 	                $scope.invoicesToPay.invoiceTotal = newTotal;
 	                $scope.invoicesToPay.invoiceCount = $scope.invoicesToPay.$values.length;
 	                return newTotal;
 	            },
 	            clear: function () {
 	                _.each($scope.invoicesToPay.$values, function (invoice) { invoice.Paid = false; });
 	                $scope.invoicesToPay.$values = [];
 	                $scope.invoicesToPay.invoiceTotal = 0;
 	                $scope.invoicesToPay.invoiceCount = 0;
 	            }
 	        };

 	        $scope.sumInvoiceCollection = function (invoiceCollection) {
 	            return _.reduce(invoiceCollection, function (runningTotal, invoice) {

 	                return parseFloat(parseFloat(runningTotal) + parseFloat(invoice.TotalAmountIncludingVAT)).toFixed(2);
 	            }, 0);
 	        };

 	        $scope.$watchCollection('invoicesToPay.$values', function () {
 	            $scope.invoicesToPay.calculateTotal();
 	        });

 	        $scope.addToInvoicesToPay = function (invoice) {
 	            invoice.Paid = true;
 	            $scope.invoicesToPay.$values.push(invoice);
 	        };

 	        $scope.addAllAttorneyInvoicesToPay = function (attorney, $event) {
 	            $event.stopPropagation();
 	            _.each(attorney.invoices, function (invoice) {

 	                if (!_.contains($scope.invoicesToPay.$values, invoice)) {
 	                    $scope.addToInvoicesToPay(invoice);
 	                }
 	            });
 	            attorney.hasAllInvoicesInPaidInvoices = true;
 	        };

 	        $scope.removeAllAttorneyInvoicesToPay = function (attorney, $event) {
 	            $event.stopPropagation();
 	            _.each(attorney.invoices, function (invoice) {

 	                if (_.contains($scope.invoicesToPay.$values, invoice)) {
 	                    $scope.removeFromInvoicesToPay(invoice);
 	                }
 	            });
 	            attorney.hasAllInvoicesInPaidInvoices = false;
 	        };

 	        $scope.queryInvoice = function (invoice) {
 	            //invoke third party workflow instance
 	            invoice.Status = 'In Query';
 	        };

 	        $scope.removeFromInvoicesToPay = function (invoice, grid) {
 	            invoice.Paid = false;
 	            $scope.invoicesToPay.$values = _.without($scope.invoicesToPay.$values, invoice);
 	            if (grid != undefined && grid != null) {
 	                grid.appScope.attorney.hasAllInvoicesInPaidInvoices = false;
 	            }
 	        };

 	        $scope.hasBeenPaid = function (invoice) {
 	            return _.contains($scope.invoicesToPay.$values, invoice);
 	        };

 	        $scope.getTableHeight = function (gridOptions) {
 	            var rowHeight = 30; // your row height
 	            var headerHeight = 30; // your header height
 	            return {
 	                height: ($scope.updatedInvoices.length * rowHeight + headerHeight) + "px"
 	            };
 	        };

 	        $scope.submitInvoicePayment = function () {
 	            $activityManager.startActivityWithKey(spinString);
                
 	            var invoiceCollection = [];
 	            
 	            _.each($scope.invoicesToPay.$values, function (invoiceToPay) {
 	                invoiceCollection
                       .push(new $domainProcessManagerProxySharedModels.ThirdPartyPaymentModel(
                           invoiceToPay.ThirdPartyInvoiceKey
                           , invoiceToPay.InstanceID
                           , invoiceToPay.AccountKey
                           , invoiceToPay.SahlReference
                       )
                    );
 	            });

 	            var postData = function () {
 	                var deferred = $q.defer();
 	                var command = new $domainProcessManagerProxyCommands.StartPayAttorneyProcessCommand(invoiceCollection);
 	                $domainProcessManagerProxyWebService.sendCommandAsync(command).then(function (data) {
 	                    if (data.data) {
 	                        if (data.data.SystemMessages.HasErrors) {
 	                            showFailureToast(data.data.SystemMessages.AllMessages.$values[0].Message);
 	                            deferred.reject(data);
 	                            $activityManager.stopActivityWithKey(spinString);
 	                        }
 	                        else {
                                $scope.invoicesToPay.clear();
 	                            showSuccessfulToast();
                                $scope.paymentCanBeMade = true;
                                refreshView();
 	                            deferred.resolve(data);
 	                            $activityManager.stopActivityWithKey(spinString);
 	                        }
 	                    }
 	                    else {
                            showFailureToast('');
 	                        deferred.reject(data);
 	                        $activityManager.stopActivityWithKey(spinString);
 	                    }
 	                    
 	                },
	                function (data) {
	                    showFailureToast(data.data.SystemMessages.AllMessages.$values[0].Message);
	                    deferred.reject(data);
	                    $activityManager.stopActivityWithKey(spinString);
	                });
 	                return deferred.promise;
 	            };

 	            postData();

                var refreshView = function(){
                    var allSpvInvoicesGroups = $scope.groupedData;
                    var allInvoices = [];
                    
                    _.each(allSpvInvoicesGroups, function(spvInvoicesGroup){
                        _.each(spvInvoicesGroup.attorneys, function(attorney){
                            _.each(attorney.invoices, function(invoice){
                                if(!invoice.Paid){
                                    allInvoices.push(invoice);
                                }
                            });
                        });
                    });
                    
                    groupInvoices(allInvoices);
                };
 	        };

 	        $scope.collapseAttorneyGrid = function (attorney) {
 	            attorney.gridVisible = !attorney.gridVisible;
 	        };

 	        $scope.collapseSpvGroup = function (spv) {
 	            spv.groupVisible = !spv.groupVisible;
 	        };

 	        $scope.loadDocument = function (row) {
 	            var documentGuid = row.entity.DocumentGuid;
 	            $documentDownloadManagerService.downloadDocumentFromStor(documentGuid, LOSS_CONTROL_STORE_ID);
 	        };

 	        $scope.getGridOptions = function (attorneyInvoices) {
 	            var defaultGridOptions = {
 	                enableFiltering: false,
 	                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
 	                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
 	                rowTemplate: '<loss-control-invoice-row></loss-control-invoice-row>',
 	                columnDefs: [
				      {
				          name: ' ', field: "InvoiceDocument", headerCellTemplate: gridCellHeaderDirective, width: 20,
				          cellTemplate: '<div class="ui-grid-cell-contents"><div class="pdf-icon-16" ng-click="grid.appScope.loadDocument(row)"></div></div>'
				      },
				      { name: 'InvoiceNumber', displayName: 'Invoice Number', headerCellTemplate: gridCellHeaderDirective },
				      { name: 'SahlReference', displayName: 'SAHL Reference', headerCellTemplate: gridCellHeaderDirective },
				      { name: 'InvoiceDate', displayName: 'Invoice Date', cellFilter: 'dateViewFilter', headerCellTemplate: gridCellHeaderDirective },
				      { name: 'TotalAmountIncludingVAT', displayName: 'Amount', cellFilter: 'currencyFilter', headerCellTemplate: gridCellHeaderDirective },
				      { name: 'Manage Invoice', cellTemplate: '<manage-cell></manage-cell>', headerCellTemplate: gridCellHeaderDirective }
 	                ],
 	                onRegisterApi: function (gridApi) {
 	                    $scope.gridApi = gridApi;
 	                }
 	            };
 	            defaultGridOptions.data = attorneyInvoices;
 	            defaultGridOptions.gridHeight = defaultGridOptions.data.length * 30 + 30;
 	            return defaultGridOptions;
 	        };

 	        $scope.getRowStyle = function (invoice) {
 	            if (_.contains($scope.invoicesToPay.$values, invoice)) {
 	                return '#FFE3B6';
 	            };
 	            return '';
 	        };

 	        $scope.invoices = {
 	            $values: []
 	        };
 	    }

 	    var getInvoicesPaidSum = function (invoices) {
 	        var paidInvoices = _.where(invoices, { Paid: true });
 	        return $scope.sumInvoiceCollection(paidInvoices);
 	    };

 	    var getInvoicesPaidCount = function (invoices) {
 	        var paidInvoices = _.where(invoices, { Paid: true });
 	        return paidInvoices.length;
 	    };

 	    var getSpvPaidCount = function (attorneys) {
 	        var allPaidInvoicesForGroup = getAllPaidInvoicesForAttorneys(attorneys);
 	        return allPaidInvoicesForGroup.length;
 	    }

 	    var getSpvPaidSum = function (attorneys) {
 	        var allPaidInvoicesForGroup = getAllPaidInvoicesForAttorneys(attorneys);
 	        return $scope.sumInvoiceCollection(allPaidInvoicesForGroup);
 	    };

 	    var getAllPaidInvoicesForAttorneys = function (attorneys) {
 	        var allPaidInvoicesForGroup = [];
 	        _.each(attorneys, function (attorney) {
 	            var paidInvoices = _.where(attorney.invoices, { Paid: true });
 	            allPaidInvoicesForGroup = allPaidInvoicesForGroup.concat(paidInvoices);
 	        });
 	        return allPaidInvoicesForGroup;
 	    };

 	    var getSpvCount = function (attorneys) {
 	        var allInvoicesForGroup = getAllInvoicesForAttorneys(attorneys);
 	        return allInvoicesForGroup.length;
 	    }

 	    var getSpvSum = function (attorneys) {
 	        var allInvoicesForGroup = getAllInvoicesForAttorneys(attorneys);
 	        return $scope.sumInvoiceCollection(allInvoicesForGroup);
 	    };

 	    var getAllInvoicesForAttorneys = function (attorneys) {
 	        var allInvoicesForGroup = [];
 	        _.each(attorneys, function (attorney) {
 	            allInvoicesForGroup = allInvoicesForGroup.concat(attorney.invoices);
 	        });
 	        return allInvoicesForGroup;
 	    };

 	    var groupInvoices = function (data) {
 	        var thirdPartyInvoices = data;
 	        var invoicesGroupedBySpv = _.chain(thirdPartyInvoices)
                    .groupBy("SpvDescription")
                    .map(function (invoices, SpvDescription) {

                        var attorneyDataGroup = _.chain(invoices).groupBy("ThirdParty").map(function (attoneyInvoices, attorney) {
                            return {
                                attorney: attorney,
                                hasAllInvoicesInPaidInvoices: false,
                                gridOptions: $scope.getGridOptions(attoneyInvoices),
                                invoicesPaidCount: getInvoicesPaidCount,
                                invoicesPaidSum: getInvoicesPaidSum,
                                invoices: attoneyInvoices
                            };
                        }).value();

                        return {
                            Spv: SpvDescription,
                            invoiceSpvPaidSum: getSpvPaidSum,
                            groupVisible: true,
                            attorneys: attorneyDataGroup,
                            invoiceSpvPaidCount: getSpvPaidCount,
                            invoiceSpvSum: getSpvSum,
                            invoiceSpvCount: getSpvCount
                        };
                    }).value();
 	        $scope.groupedData = invoicesGroupedBySpv;
 	    };

 	    var getInvoicesFromSolr = function () {
 	        var deferred = $q.defer(),
            	pageSize = 1000,
            	pageNumber = 1;
            
            $searchManagerService.getNewSearchTypesAndFilters('thirdpartyInvoiceStatus', 
            [{
                    name: 'thirdpartyInvoiceStatus',
                    label: 'Invoices',
                    index: 'ThirdPartyInvoice',
                    search: null,
                    searchDetail: null,
                    filters: [{
                        filterType: 'dropdown',
                        label: 'InvoiceStatusDescription',
                        options: ['Any Status'],
                        defaultOption:'Any Status',
                        selectedFilter: null
                    }]
                    ,
                    initialise: function (searchType) {
                       
                    }
              }]);
            
 	        $searchManagerService
				.searchForThirdPartyInvoices("InvoiceStatusDescription:Approved", 'ThirdPartyInvoice', pageSize, pageNumber)
				.then(function (data) {
				    groupInvoices(data.results);
				    deferred.resolve();
				});

 	        return deferred.promise;
 	    };

 	    var loadData = function () {
 	        $activityManager.startActivityWithKey(spinString);

 	        getInvoicesFromSolr().then(function () {
 	            $activityManager.stopActivityWithKey(spinString);
 	        });
 	    };

 	    var doesCatsPaymentBatchForTodayExist = function () {
 	        var query = new $cATSQueries.DoesCatsPaymentBatchForTodayExistQuery();
 	        $catsWebService.sendQueryAsync(query).then(function (data) {
 	            var batchForTodayExists = data.data.ReturnData.Results.$values[0].BatchExists;
 	            if (batchForTodayExists) {
 	                showCannotCreatePayBatchToast();
                    $scope.paymentCanBeMade = $scope.paymentCanBeMade && !batchForTodayExists;
 	            }
                else{
                    isThereAPendingCatsPaymentForProfile();
                }
 	        });
 	    };
        
        var isThereAPendingCatsPaymentForProfile = function () {
 	        var query = new $cATSQueries.IsThereACatsFileBeingProcessedForProfileQuery(1);
 	        $catsWebService.sendQueryAsync(query).then(function (data) {
 	            var isThereAPendingCatsPaymentForProfile = data.data.ReturnData.Results.$values[0];
 	            $scope.paymentCanBeMade = $scope.paymentCanBeMade && !isThereAPendingCatsPaymentForProfile;
 	            if (isThereAPendingCatsPaymentForProfile) {
 	                $toastManagerService.error({
 	            title: 'Warning',
 	            text: 'There is a pending payment, please try again after 15 minutes.'
 	        });
 	            }
 	        });
 	    };

 	    var showCannotCreatePayBatchToast = function () {
 	        $toastManagerService.error({
 	            title: 'Warning',
 	            text: 'You cannot submit a payment batch for today, one has already been created.'
 	        });
 	    };

 	    var showFailureToast = function (message) {
            if(!message){
                message = 'An error submitting this batch occurred, please try again later.';
            }
 	        $toastManagerService.error({
 	            title: 'Error',
 	            text: message
 	        });
 	    };

 	    var showSuccessfulToast = function () {
 	        return $toastManagerService.success({
 	            title: 'Success',
 	            text: 'Payment batch successfully submitted; and Note that you will not be able to submit any more batches for today.'
 	        }).promise;
 	    };

 	    function initialiseController() {
 	        initialiseScope();
 	        doesCatsPaymentBatchForTodayExist();
 	        loadData();
 	    }
 	    initialiseController();
 	}
 ]).directive('manageCell', [function () {
     return {
         restrict: 'E',
         templateUrl: 'lib/views/lossControlManager/losscontrolInvoices.manageCell.tpl.html'
     };
 }]).directive('lossControlInvoiceRow', [function () {
     return {
         restrict: 'E',
         templateUrl: 'lib/views/lossControlManager/losscontrolInvoices.invoiceRow.tpl.html'
     };
 }]).directive('gridCellHeader', [function () {
     return {
         restrict: 'E',
         templateUrl: 'lib/views/lossControlManager/losscontrolInvoices.gridCellHeader.tpl.html'
     };
 }]).directive('lossControlPayAttorneyButton', [function () {
     return {
         restrict: 'E',
         templateUrl: 'lib/views/lossControlManager/losscontrolinvoices.payAttorneyButton.tpl.html'
     };
 }]);


