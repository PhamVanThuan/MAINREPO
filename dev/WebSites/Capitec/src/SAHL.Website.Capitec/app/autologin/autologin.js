angular.module('capitecApp.autologin', [
    'ui.router',
    'SAHL.Services.Interfaces.Capitec.commands',
    'SAHL.Services.Interfaces.Capitec.queries',
    'capitecApp.services'
])

.config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state('autologin', {
        url: '/autologin?cp&branch',
        authNotRequired: true,
        templateUrl: 'autologin.tpl.html',
        data: {}
    });
}])

.controller('AutoLoginCtrl', ['$rootScope', '$scope', '$state', '$commandManager', '$capitecCommands', '$queryManager', '$capitecQueries', '$activityManager', '$cookieService', '$stateParams', '$timeout', '$templateCache',
    function AutoLoginCtrl($rootScope, $scope, $state, $commandManager, $commands, $queryManager, $queries, $activityManager, $cookieService, $stateParams, $timeout, $templateCache) {
        var initialiseController = function () {
            initialiseScope();
            logoffCurrentUser();
            doAutoLogin();
        };

        var initialiseScope = function () {
            $scope.copyText = {};
            $scope.title = 'Processing...';
            $scope.message = 'Please wait while we process your login.';
        };

        var logoffCurrentUser = function () {
            $rootScope.authenticated = false;
            $rootScope.userAuthToken = '';
            $rootScope.userDisplayName = '';
            $rootScope.userRoles = [];
            $rootScope.username = '';
            $cookieService.removeItem('authToken');
        };

        var doAutoLogin = function () {
            // Ensure we have cp, branch and password
            if (!$stateParams.cp || !$stateParams.branch) {
                showLoginFailure();
                return;
            }

            $activityManager.startActivity();

            var branchCode = decodeBranchUrl($stateParams.branch);
            executeLoginCommand($stateParams.cp, branchCode, 'Natal123').then(
                function success(data) {
                    $rootScope.authenticated = true;
                    // Get this user's details
                    return executeUserQuery($rootScope.userAuthToken);
                }
            ).then(
                function success(data) {
                    $rootScope.userDisplayName = data.data.ReturnData.Results.$values[0].LastName;
                    $rootScope.userRoles = data.data.ReturnData.Results.$values[0].Roles;
                    $rootScope.username = data.data.ReturnData.Results.$values[0].Username;
                    $rootScope.userId = data.data.ReturnData.Results.$values[0].Id;
                    $state.transitionTo('home.splashscreen');
                },
                function failure(error) {
                    showLoginFailure();
                }
            )["finally"](cleanup);   //IE8 bug fix for promises https://github.com/angular/angular.js/commit/f078762d48d0d5d9796dcdf2cb0241198677582c            
        };

        var decodeBranchUrl = function (branch) {
            return decodeURIComponent(branch);
        };

        var cleanup = function() {
            $activityManager.stopActivity();
        };

        var showLoginFailure = function () {
            $scope.title = 'Login failed.';
            $scope.message = 'We are unable to process your login request.';
        };

        var executeLoginCommand = function (cp, branch, password) {
            var command = new $commands.AutoLoginCommand(cp, branch, password);
            return $commandManager.sendCommandAsync(command);
        };

        var executeUserQuery = function (userAuthToken) {
            var query = new $queries.GetUserFromAuthTokenQuery(userAuthToken);
            return $queryManager.sendQueryAsync(query);
        };

        initialiseController();
    }
]);