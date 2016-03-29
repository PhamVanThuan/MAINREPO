'use strict';

// Declare app level module which depends on filters, and services
angular.module('sahl.tools.app', [
  'sahl.tools.app.filters',
  'sahl.tools.app.services',
  'sahl.tools.app.directives',
  'ui.router',
  'sahl.tools.app.home',
  'sahl.core.app.messaging',
  'SAHL.Services.Interfaces.DecisionTreeDesign.commands',
  'SAHL.Services.Interfaces.DecisionTreeDesign.queries'
])
.config(function ($stateProvider, $urlRouterProvider, $locationProvider, $provide, $httpProvider, $compileProvider) {
    //var cacheBustVersion = (new Date).valueOf();

    $httpProvider.interceptors.push('$httpInterceptor');
    // check if the browser supports html5 history so we can remove the hashes in the urls if it does
    if (window.history && window.history.pushState) {
        //$locationProvider.html5Mode(true);
    }

    //
    // For any unmatched url, redirect to /state1
    $urlRouterProvider.otherwise("/");

    $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|data):/);

    //$provide.decorator("$http", ["$delegate", function ($delegate) {
    //    var get = $delegate.get;
    //    $delegate.get = function (url, config) {
    //        // Check is to avoid breaking AngularUI ui-bootstrap-tpls.js: "template/accordion/accordion-group.html"
    //        if ((url.indexOf('tpl.html')) || (url.indexOf('js/'))) {
    //            // Append ?v=[cacheBustVersion] to url
    //            //url += (url.indexOf("?") === -1 ? "?" : "&");
    //            //url += "v=" + cacheBustVersion;
    //        }
    //        return get(url, config);
    //    };
    //    return $delegate;
    //}]);
})

.controller('AppCtrl', ['$rootScope', '$http', function AppController($rootScope, $queryManager, $decisionTreeDesignQueries, $http) {
}])

.run(
    ['$rootScope', '$state', '$stateParams', '$queryManager', '$decisionTreeDesignQueries', '$serviceConfig', '$activityManager', '$window', '$startableServices', '$eventAggregatorService', '$eventDefinitions', '$applicationService', '$timeout',
function ($rootScope, $state, $stateParams, $queryManager, $decisionTreeDesignQueries, $serviceConfig, $activityManager, $window, $startableServices, $eventAggregatorService, $eventDefinitions, $applicationService, $timeout) {
    $rootScope.authenticated = false;
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;
    $rootScope.userAuthToken = '';
    $rootScope.userDisplayName = "";
    $rootScope.userEmailAddress = "";
    $rootScope.username = "";
    $rootScope.userRoles = [];
    $rootScope.username = "";
    $rootScope.userimagesrc = "";
    $rootScope.loading = true;
    $activityManager.startActivityWithKey("application-load");
    var initialTime = new Date();
    $rootScope.spin = function () { };

    $rootScope.$on('$stateChangeStart', function (evt, toState, toParams, fromState, fromParams) {
        if (fromState.name == "home.design.file-menu") {
            angular.element($window).unbind('resize');
        }
    });

    $applicationService.startApp().then(function () {
        
        $timeout(function () {
            var currentTime = new Date();
            var timeDiff = currentTime - initialTime;
            if (timeDiff < 3000) {
                timeDiff = 3000 - timeDiff;
            }

            setTimeout(function () {
                $timeout(function () {
                    $activityManager.stopActivityWithKey("application-load");
                    $rootScope.loading = false;
                }, timeDiff);
            });
        });
    });
}]);