'use strict';
angular.module('sahl.ui.halo.views.pages.common.invoices.thirdPartyCorrespondencePageState.tpl.html', [])
    .controller('Common_Invoices_ThirdPartyCorrespondencePageStateCtrl', 
                ['$scope', '$stateParams', '$activityManager', '$thirdPartyInvoiceManagerService',
     function ($scope, $stateParams, $activityManager, $thirdPartyInvoiceManagerService) {

            var spinString = "loadingData";
         
            var thirdPartyInvoiceKey = $stateParams.model.businessContext.BusinessKey.Key.toString();
            var invoiceCurrentStateName = $stateParams.model.businessContext.Context;
            var isInInvoiceQueryState = invoiceCurrentStateName === 'Invoice Query';

            $scope.title = isInInvoiceQueryState ? 'Invoice Queries' : 'Invoice Correspondence';

            var loadInvoiceQueries = function () {
                $activityManager.startActivityWithKey(spinString);
                
                $thirdPartyInvoiceManagerService.getThirdPartyInvoiceCorrespondence(thirdPartyInvoiceKey)
                    .then(function (data) {
                        var invoiceQueries = data.results;

                        if (isInInvoiceQueryState) {
                            invoiceQueries = _.filter(invoiceQueries, function (correspondence) {
                                return correspondence.CorrespondenceReason === 'Third Party Invoice Query';
                            });
                        }

                        _.each(invoiceQueries, function (qry) {
                            qry.DateRange = getDateRange(qry.Date);
                        });

                        invoiceQueries = _.chain(invoiceQueries)
                            .groupBy('DateRange')
                            .map(function (invoiceList, dateRange) {
                                return {
                                    DateRange: dateRange,
                                    invoices: invoiceList
                                }
                            }).value();
                        
                        invoiceQueries = _.sortBy(invoiceQueries, 'DateRange');
                    
                        $scope.invoiceQueries = invoiceQueries;
                        $scope.$watch('invoiceQueries', function () {
                            if ($scope.invoiceQueries) {
                                $("#accordion").accordion();
                            }
                        });

                    $activityManager.stopActivityWithKey(spinString);
                    
                    });
            };

            loadInvoiceQueries();

            $scope.loadInvoiceQuery = function (invoiceQuery) {
                $scope.invoiceQuery = invoiceQuery;
            };

            function getDateRange(qryDate) {
                var today = new Date();
                qryDate = new Date(qryDate);
                var moreThan7days = new Date();
                moreThan7days.setDate(moreThan7days.getDate() - 8);

                var dateRange = '3_More than 7 days';
                if (qryDate) {
                    if (qryDate.setHours(0, 0, 0, 0) === today.setHours(0, 0, 0, 0)) {
                        dateRange = '1_Today';
                    } else if ((qryDate.setHours(0, 0, 0, 0) < today.setHours(0, 0, 0, 0)) && (qryDate.setHours(0, 0, 0, 0) > moreThan7days.setHours(0, 0, 0, 0))) {
                        dateRange = '2_Last 7 days';
                    } else {
                        dateRange = '3_More than 7 days';
                    }

                }

                return dateRange;
            }

            $scope.$on('$destroy', function () {});
            }]);