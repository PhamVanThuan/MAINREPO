'use strict';
angular.module('Home.LossControlManager.MonthBreakdownByAttorneyRootTileConfiguration.tpl.html',[])
.controller('Home_LossControlManager_MonthBreakdownByAttorneyRootTileConfigurationCtrl',['$scope','$activityManager','$paymentMonthlyBrakedownService',
function($scope,$activityManager,$paymentMonthlyBrakedownService){
	var operations = {
		load : function(){
			$activityManager.startActivityWithKey('monthlyPaymentOverview');
			$paymentMonthlyBrakedownService.getData().then(function(data){
				$activityManager.stopActivityWithKey('monthlyPaymentOverview');
				$scope.dataModel = data;
                operations.calculateTotals(data);
			},function(){
				$activityManager.stopActivityWithKey('monthlyPaymentOverview');
			});
		},
        calculateTotals : function(data){
            var totalsData = _.pluck(data, 'Total');
            $scope.AllTotals = _.reduce(totalsData, function(memo, num){ return memo + num; }, 0)
            var capitalisedData = _.pluck(data, 'Capitalised');
            $scope.CapitalisedTotal = _.reduce(capitalisedData, function(memo, num){ return memo + num; }, 0)
            var paidBySPVData = _.pluck(data, 'PaidBySPV');
            $scope.PaidBySPVTotal = _.reduce(paidBySPVData, function(memo, num){ return memo + num; }, 0)
            var debtReviewData = _.pluck(data, 'DebtReview');
            $scope.DebtReviewTotal = _.reduce(debtReviewData, function(memo, num){ return memo + num; }, 0)
            var paidData = _.pluck(data, 'Paid');
            $scope.PaidTotal = _.reduce(paidData, function(memo, num){ return memo + num; }, 0)
            var rejectedData = _.pluck(data, 'Rejected');
            $scope.RejectedTotal = _.reduce(rejectedData, function(memo, num){ return memo + num; }, 0)
            var unprocessedData = _.pluck(data, 'Unprocessed');
            $scope.UnprocessedTotal = _.reduce(unprocessedData, function(memo, num){ return memo + num; }, 0)
            var processedData = _.pluck(data, 'Processed');
            $scope.ProcessedTotal = _.reduce(processedData, function(memo, num){ return memo + num; }, 0)
            var accountsPaidData = _.pluck(data, 'AccountsPaid');
            $scope.AccountsPaidTotal = _.reduce(accountsPaidData, function(memo, num){ return memo + num; }, 0)
        }
	};

	operations.load();
}]);
//service to be swapped out with real service
angular.module('Home.LossControlManager.MonthBreakdownByAttorneyRootTileConfiguration.tpl.html')
.service('$paymentMonthlyBrakedownService',['$q','$thirdPartyInvoiceManagerService',function($q, $thirdPartyInvoiceManagerService){
	var fakeData = [{AttorneyName : 'Douglas Property Valuations C.C', Capitalised : 0, PaidBySPV : 0, DebtReview : 0, Total : 0, AvgRValuePerInvoice:0, AvgRValuePerAccount : 0, Paid : 0, Rejected : 0, Unprocessed : 1, Processed : 0, AccountsPaid : 0}
        ,{AttorneyName : 'Friedman Scheckter', Capitalised : 0, PaidBySPV : 0, DebtReview : 0, Total : 0, AvgRValuePerInvoice:0, AvgRValuePerAccount : 0, Paid : 0, Rejected : 2, Unprocessed : 0, Processed : 0, AccountsPaid : 0}
        ,{AttorneyName : 'Jan Holtzhuisen', Capitalised : 0, PaidBySPV : 0, DebtReview : 0, Total : 0, AvgRValuePerInvoice:0, AvgRValuePerAccount : 0, Paid : 0, Rejected : 0, Unprocessed : 1, Processed : 0, AccountsPaid : 0}
        ,{AttorneyName : 'Moodie And Robertson', Capitalised : 0, PaidBySPV : 0, DebtReview : 0, Total : 1256, AvgRValuePerInvoice:251.2, AvgRValuePerAccount : 251.2, Paid : 5, Rejected : 9, Unprocessed : 0, Processed : 99, AccountsPaid : 5}
        ,{AttorneyName : 'Randles Inc', Capitalised : 0, PaidBySPV : 628, DebtReview : 314, Total : 1256, AvgRValuePerInvoice:179.428571428571, AvgRValuePerAccount : 179.428571428571, Paid : 7, Rejected : 53, Unprocessed : 138, Processed : 49, AccountsPaid : 7}
        ,{AttorneyName : 'Shepstone & Wylie (Durban)', Capitalised : 0, PaidBySPV : 0, DebtReview : 0, Total : 0, AvgRValuePerInvoice:0, AvgRValuePerAccount : 0, Paid : 0, Rejected : 0, Unprocessed : 0, Processed : 1, AccountsPaid : 0}
        ,{AttorneyName : 'Strauss Daly (Western Cape) Inc.', Capitalised : 0, PaidBySPV : 0, DebtReview : 0, Total : 0, AvgRValuePerInvoice:0, AvgRValuePerAccount : 0, Paid : 0, Rejected : 0, Unprocessed : 0, Processed : 2, AccountsPaid : 0}
        ,{AttorneyName : 'Strauss Daly (Western Cape) Inc. (Stellenbosch)', Capitalised : 0, PaidBySPV : 0, DebtReview : 0, Total : 0, AvgRValuePerInvoice:0, AvgRValuePerAccount : 0, Paid : 0, Rejected : 0, Unprocessed : 1, Processed : 0, AccountsPaid : 0}
        ,{AttorneyName : 'Van Der Merwe, Tromp && Associates', Capitalised : 0, PaidBySPV : 0, DebtReview : 0, Total : 0, AvgRValuePerInvoice:0, AvgRValuePerAccount : 0, Paid : 0, Rejected : 0, Unprocessed : 1, Processed : 0, AccountsPaid : 0}]
    ;

	var operations = {
		getData : function(){
            var deferred = $q.defer();
            $thirdPartyInvoiceManagerService.getMonthBreakdownByAttorney().then(function (data) {
                deferred.resolve(data.results);
            });
         return deferred.promise;;
        }
    };
	return {
		getData : operations.getData
	}
}]);
