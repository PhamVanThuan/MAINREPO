'use strict';
var app = angular.module('app',['ui.router','SAHL.Services.Query.rest','metadata'])
.config(function($stateProvider, $urlRouterProvider){
	$urlRouterProvider.otherwise('app');
	$stateProvider.state('app', {
    	url:"/app",
		templateUrl: 'app.html',
		controller: 'AppCtrl'
	});
}).run(['$rootScope','$state','$stateParams',function ($rootScope, $state, $stateParams) {
	$rootScope.$state = $state;
	$rootScope.$stateParams = $stateParams;
}]);