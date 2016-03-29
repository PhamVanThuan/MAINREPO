'use strict';

/* Services */

angular.module('sahl.tools.app.services.applicationService', [
    'sahl.tools.app.serviceConfig',
    'SAHL.Services.Interfaces.DecisionTreeDesign.queries',
    'SAHL.Services.Interfaces.DecisionTreeDesign.commands'
])
.factory('$applicationService', ['$rootScope', '$queryManager', '$commandManager', '$decisionTreeDesignQueries', '$decisionTreeDesignCommands', '$activityManager', '$eventAggregatorService', '$eventDefinitions', '$startableServices', '$state', '$serviceConfig', '$q', '$signalRSvc',
function ($rootScope, $queryManager, $commandManager, $decisionTreeDesignQueries, $decisionTreeDesignCommands, $activityManager, $eventAggregatorService, $eventDefinitions, $startableServices, $state, $serviceConfig, $q, $signalRSvc) {
    return {
        startApp: function () {
            var deferred = $q.defer();
            $activityManager.startActivityWithKey("getUserDetails");
            var query = new $decisionTreeDesignQueries.GetAuthenticatedUserDetailsQuery();
            $queryManager.sendQueryAsync(query).then(function (data) {
                if (data.data.ReturnData.Results.$values.length === 1) {
                    if (data.data.ReturnData.Results.$values.length == 1) {
                        if (data.data.ReturnData.Results.$values[0].Username !== "") {
                            $rootScope.userDisplayName = data.data.ReturnData.Results.$values[0].DisplayName;
                            $rootScope.userEmailAddress = data.data.ReturnData.Results.$values[0].EmailAddress;
                            $rootScope.username = data.data.ReturnData.Results.$values[0].Username;
                            $rootScope.userimagesrc = $serviceConfig.UserProfileImageService + $rootScope.username + "&size=48";
                            $rootScope.superUser = data.data.ReturnData.Results.$values[0].IsSuperUser;
                            $activityManager.stopActivityWithKey("getUserDetails");
                            $eventAggregatorService.publish($eventDefinitions.onUserAuthenticated, $rootScope.username);

                            $startableServices.startServices().then(function () {
                                $signalRSvc.startHub().then(function () {
                                    deferred.resolve();
                                })
                            })
                        }
                        else {
                            $state.go('home.notauthenticated');
                            deferred.reject();
                        }
                    }
                }
            }, function (errorMessage) {
                $state.go('home.notauthenticated');
                deferred.reject();
            });
            return deferred.promise;
        }
    }
}])