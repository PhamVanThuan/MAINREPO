'use strict';
angular.module('Home.LossControlManager.PaidInvoicesRandsPerDayRootTileConfiguration.tpl.html',['sahl.js.ui.graphing'])
.controller('Home_LossControlManager_PaidInvoicesRandsPerDayRootTileConfigurationCtrl',['$scope','$activityManager','$paidInvoicesRandsPerDayService',
function($scope,$activityManager,$paidInvoicesRandsPerDayService){
	var operations = {
		load : function(){
			$activityManager.startActivityWithKey('paidInvoicesRandsPerDay');
			$paidInvoicesRandsPerDayService.getData().then(function(data){
				$scope.dataModel = data;
				$activityManager.stopActivityWithKey('paidInvoicesRandsPerDay');
			},function(){
				$activityManager.stopActivityWithKey('paidInvoicesRandsPerDay');
			});
		},
		getOrdinal : function(n) {
		   var s=["th","st","nd","rd"],
		       v=n%100;
		   return n+(s[(v-20)%10]||s[v]||s[0]);
		}
	};

	$scope.popupTemplate = function(index,data,color){
		return [{
							text : 'R '+data.y,
							attr : {
								font : '14px Helvetica, Arial',
								fill : '#fff'
							}
						}, {
							text : operations.getOrdinal(data.x)+' of '+$scope.dataModel[index].month,
							attr : {
								font : '12px Helvetica, Arial',
								fill : color
							}
						}
					];
	};

	operations.load();
}]);

angular.module('Home.LossControlManager.PaidInvoicesRandsPerDayRootTileConfiguration.tpl.html')
.service('$paidInvoicesRandsPerDayService',['$q',function($q){
	var fakeData = [
							    { name: 'Previous month', month: 'July', color: "#FF5800", values: [{x:1,y:77},{x:2,y:97},{x:3,y:141},{x:4,y:170},{x:5,y:246},{x:6,y:301},{x:7,y:339},{x:8,y:392},{x:9,y:414},{x:10,y:478},{x:11,y:519},{x:12,y:558},{x:13,y:593},{x:14,y:620},{x:15,y:650},{x:16,y:662},{x:17,y:747},{x:18,y:773},{x:19,y:787},{x:20,y:830},{x:21,y:840},{x:22,y:843},{x:23,y:893},{x:24,y:974},{x:25,y:1037},{x:26,y:1111},{x:27,y:1131},{x:28,y:1198},{x:29,y:1267},{x:30,y:1279},{x:31,y:1279}]},
									{ name: 'This month', month: 'August', color: "#FEA500", values: [{x:1,y:74},{x:2,y:137},{x:3,y:219},{x:4,y:221},{x:5,y:302},{x:6,y:312},{x:7,y:336},{x:8,y:368},{x:9,y:399},{x:10,y:453},{x:11,y:531},{x:12,y:540},{x:13,y:608},{x:14,y:704},{x:15,y:796},{x:16,y:796}]}
								];

	var operations = {
		getData : function(){
			var deferred = $q.defer();
			setTimeout(function(){
				deferred.resolve(fakeData);
			},Math.random()*1000);
			return deferred.promise;
		}
	};
	return {
		getData : operations.getData
	}
}]);
