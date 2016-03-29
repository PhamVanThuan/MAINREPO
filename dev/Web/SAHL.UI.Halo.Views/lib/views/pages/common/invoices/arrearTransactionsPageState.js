'use strict';
angular.module('sahl.ui.halo.views.pages.common.transactions.arrearTransactionsPageState.tpl.html',[])
.controller('Common_Transactions_ArrearTransactionsPageStateCtrl',[
        '$scope', '$stateParams', '$transactionQueryService', '$activityManager',
        function($scope, $stateParams, $transactionQueryService, $activityManager) {
            //initialise Scope
            $scope.transactions = {
                transactionData: null,
                entityId: $stateParams.businessKey
            };
            $scope.transactionsGrid = {
                enableFiltering: true,
                paginationPageSizes: [20, 30, 50, 100],
                columnDefs: [
                    { name: 'TransactionTypeDescription', displayName: 'Transaction Type', width: '20%', enableGrouping: true, enableFiltering: true },
                    { name: 'FinancialService', displayName: 'Service', enableGrouping: true, enableFiltering: true },
                    { name: 'TransactionGroup', enableGrouping: true, enableFiltering: true },
                    { name: 'Amount', cellFilter: 'currencyFilter', cellClass: 'right-align-text', enableGrouping: false, enableFiltering: false },
                    { name: 'InsertDate', cellFilter: 'dateViewFilter', enableGrouping: false, enableFiltering: false },
                    { name: 'EffectiveDate', cellFilter: 'dateViewFilter', enableGrouping: false, enableFiltering: false },
                    { name: 'Balance', cellFilter: 'currencyFilter', cellClass: 'right-align-text', enableGrouping: false, enableFiltering: false },
                    { name: 'AccountBalance', cellFilter: 'currencyFilter', cellClass: 'right-align-text', enableGrouping: false, enableFiltering: false },
                    { name: 'UserID', displayName: 'Changed By', enableGrouping: true, enableFiltering: true },
                    { name: 'Reference', enableGrouping: true, enableFiltering: true }
                ]
            };
            //load Transaction Data
            $activityManager.startActivityWithKey("loading");
            $transactionQueryService.getArrearTransactionDetailData($scope.transactions.entityId).then(function(data) {
                $activityManager.stopActivityWithKey("loading");
                $scope.transactionsGrid.data = data;
            }, function() {
                $activityManager.stopActivityWithKey("loading");
                throw (moduleName + ' Transaction data cannot be loaded');
            });
        }
    ]);