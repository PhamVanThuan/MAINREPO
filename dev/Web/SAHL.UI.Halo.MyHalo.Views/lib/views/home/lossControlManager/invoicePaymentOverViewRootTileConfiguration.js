'use strict';
angular.module('Home.LossControlManager.InvoicePaymentOverViewRootTileConfiguration.tpl.html',[])
.controller('Home_LossControlManager_InvoicePaymentOverViewRootTileConfigurationCtrl',['$scope','$activityManager','$paymentOverviewService',
function($scope,$activityManager,$paymentOverviewService){
	var operations = {
		load : function(){
			$activityManager.startActivityWithKey('paymentOverview');
			$paymentOverviewService.getData().then(function(data){
				$activityManager.stopActivityWithKey('paymentOverview');
				$scope.dataModel = data;
			},function(){
				$activityManager.stopActivityWithKey('paymentOverview');
			});
		}
	};

	operations.load();
}]);
//service to be swapped out with real service
angular.module('Home.LossControlManager.InvoicePaymentOverViewRootTileConfiguration.tpl.html')
.service('$paymentOverviewService',['$q','$thirdPartyInvoiceManagerService',function($q, $thirdPartyInvoiceManagerService){
	var paymentOverview = {
	 thisMonth : {
		 InvoicesPaidAmount : 0.00,
		 InvoicesPaidCount : 0,
		 InvoiceUnprocessed : 0
	 },
	 lastMonth : {
		 InvoicesPaidAmount : 0.00,
		 InvoicesPaidCount : 0,
		 InvoiceUnprocessed : 0
	 },
	 totalsToDate : {
		 InvoicesPaidAmount : 0.00,
		 InvoicesUnProcessedCount : 0
	 }
	};

	var internal = {
		getInvoicesPaidPreviousMonthBreakDown : function(){
            var deferred = $q.defer();
			$thirdPartyInvoiceManagerService.getInvoicesPaidPreviousMonthBreakDown().then(function (data) {
                paymentOverview.lastMonth.InvoicesPaidAmount = data.results.length ? data.results[0].Value : 0;
                paymentOverview.lastMonth.InvoicesPaidCount = data.results.length ? data.results[0].Count : 0;
                
                deferred.resolve();
            });
            return deferred.promise;
		},

		getInvoicesPaidThisMonthBreakDown : function(){
            var deferred = $q.defer();
			$thirdPartyInvoiceManagerService.getInvoicesPaidThisMonthBreakDown().then(function (data) {
                paymentOverview.thisMonth.InvoicesPaidAmount = data.results.length ? data.results[0].Value : 0;
                paymentOverview.thisMonth.InvoicesPaidCount = data.results.length ? data.results[0].Count : 0;
                deferred.resolve();
            });
            return deferred.promise;
		},

		getInvoicesPaidThisYearBreakDown: function(){
            var deferred = $q.defer();
			$thirdPartyInvoiceManagerService.getInvoicesPaidThisYearBreakDown().then(function (data) {
                paymentOverview.totalsToDate.InvoicesPaidAmount = data.results.length ? data.results[0].Value : 0;
                deferred.resolve();
            });
            return deferred.promise;
		},

		getInvoicesNotProcessedPreviousMonthBreakDown: function(){
            var deferred = $q.defer();
			$thirdPartyInvoiceManagerService.getInvoicesNotProcessedPreviousMonthBreakDown().then(function (data) {
                paymentOverview.lastMonth.InvoiceUnprocessed = data.results.length ? data.results[0].Count : 0;
                deferred.resolve(data.results);
            });
            return deferred.promise;
		},

		getInvoicesNotProcessedThisMonthBreakDown: function(){
            var deferred = $q.defer();
			$thirdPartyInvoiceManagerService.getInvoicesNotProcessedThisMonthBreakDown().then(function (data) {
                paymentOverview.thisMonth.InvoiceUnprocessed = data.results.length ? data.results[0].Count : 0;
                deferred.resolve(data.results);
            });
            return deferred.promise;
		},

		getInvoicesNotProcessedThisYearBreakDown: function(){
            var deferred = $q.defer();
			$thirdPartyInvoiceManagerService.getInvoicesNotProcessedThisYearBreakDown().then(function (data) {
                paymentOverview.totalsToDate.InvoicesUnProcessedCount = data.results.length ? data.results[0].Count : 0;
                deferred.resolve();
            });
            return deferred.promise;
		}

	};

	var operations = {
		getData : function(){
			var deferred = $q.defer();
			var waitingFor = [];

			waitingFor.push(internal.getInvoicesPaidPreviousMonthBreakDown());
			waitingFor.push(internal.getInvoicesPaidThisMonthBreakDown());
			waitingFor.push(internal.getInvoicesPaidThisYearBreakDown());
			waitingFor.push(internal.getInvoicesNotProcessedPreviousMonthBreakDown());
			waitingFor.push(internal.getInvoicesNotProcessedThisMonthBreakDown());
			waitingFor.push(internal.getInvoicesNotProcessedThisYearBreakDown());
			
			$q.all(waitingFor).then(function(){
				deferred.resolve(paymentOverview);
			});

			return deferred.promise;
		}
	};
	return {
		getData : operations.getData
	}
}]);
