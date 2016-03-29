'use strict';

// Declare app level module which depends on filters, and services
angular.module('capitecApp', ['templates-main',
  'capitecApp.filters',
  'capitecApp.services',
  'capitecApp.directives',
  'ui.router',
  'ngCookies',
  'capitecApp.home',
  'capitecApp.autologin',
  'SAHL.Services.Interfaces.Capitec.commands',
  'SAHL.Services.Interfaces.Capitec.queries',
  'SAHL.Services.Interfaces.CapitecSearch.searchqueries',
  'SAHL.Services.Interfaces.Capitec.sharedmodels'
])
.config(['$stateProvider', '$urlRouterProvider', '$locationProvider', '$provide', '$httpProvider', function ($stateProvider, $urlRouterProvider, $locationProvider, $provide, $httpProvider) {
    //var cacheBustVersion = (new Date).valueOf();

    $httpProvider.interceptors.push('$httpInterceptor');
    // check if the browser supports html5 history so we can remove the hashes in the urls if it does
    //if (window.history && window.history.pushState) {
    //    $locationProvider.html5Mode(true);
    //}

    //
    // For any unmatched url, redirect to /state1
    $urlRouterProvider.otherwise('/');
}])

.controller('AppCtrl', ['$rootScope', '$commandManager', '$http', function AppController($rootScope, $commandManager, $http) {
}])

.run(
    ['$rootScope', '$state', '$stateParams', '$cookieService', '$capitecQueries', '$queryManager',
    function ($rootScope, $state, $stateParams, $cookieService, $capitecQueries, $queryManager) {
        $rootScope.authenticated = false;
        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;
        $rootScope.userAuthToken = '';
        $rootScope.userDisplayName = '';
        $rootScope.userRoles = [];
        $rootScope.username = '';

        // Protect "secure pages"
        $rootScope.$on('$stateChangeStart', function (evt, toState, toParams, fromState, fromParams) {
            if (toState.name !== 'autologin' && !$rootScope.authenticated) {  // if not authenticated and not going to autologin screen
                var authTokenCookie = $cookieService.getItem('authToken'); // check if there's an auth cookie
                if (authTokenCookie) {
                    evt.preventDefault(); // Stop transitioning

                    var query = new $capitecQueries.GetUserFromAuthTokenQuery(authTokenCookie); // try logging in with the cookie
                    $queryManager.sendQueryAsync(query).then(function (data) {
                        if (data.data.ReturnData.Results.$values.length === 1) {
                            $rootScope.authenticated = true;
                            $rootScope.userAuthToken = authTokenCookie;
                            var firstName = data.data.ReturnData.Results.$values[0].FirstName;
                            var lastName = data.data.ReturnData.Results.$values[0].LastName;
                            $rootScope.userDisplayName = (firstName === lastName) ? lastName : (firstName + ' ' + lastName);
                            $rootScope.userRoles = data.data.ReturnData.Results.$values[0].Roles;
                            $rootScope.username = data.data.ReturnData.Results.$values[0].Username;
                            $rootScope.userId = data.data.ReturnData.Results.$values[0].Id;

                            $state.transitionTo(toState.name);
                        }
                    }, function (errorMessage) {
                        $rootScope.authenticated = false;
                        $cookieService.removeItem('authToken');
                        $scope.errorMessage = 'query failure.';

                        $state.transitionTo('autologin');
                    });
                }
                else // not authenticated and no authCookie
                {
                    evt.preventDefault();
                    $state.transitionTo('autologin');
                }
            } 
            else if ((toState.name.indexOf('application-precheck') >= 0 
                        && fromState.url.indexOf('/application-result') >= 0)) {
                evt.preventDefault();
                $state.transitionTo('home.splashscreen');
            } else if ((toState.url == '/declarations' && fromState.url == '/application-result') ||
                    ($rootScope.authenticated && toState.name === 'autologin')) {
                evt.preventDefault();
                $state.transitionTo('home.splashscreen');
            } 
        });
    }]);