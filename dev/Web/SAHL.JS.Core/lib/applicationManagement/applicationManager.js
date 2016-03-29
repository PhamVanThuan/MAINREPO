'use strict';

angular.module('sahl.js.core.applicationManagement')
    .factory('$applicationManagerService', [ '$userManagerService', '$startableManagerService',
        '$q', '$eventAggregatorService', '$logger', '$timeout', '$activityManager','$rootScope',
        function ( $userManagerService, $startableManagerService,
            $q, $eventAggregatorService, $logger, $timeout, $activityManager,$rootScope) {
            var applicationStates = {
                NONE: 'None',
                STARTING: 'Starting',
                STARTED: 'Started',
                FAILED: 'Failed'
            };

            var applicationEvents = {
                applicationStarting: 'applicationStarting',
                applicationStarted: 'applicationStarted',
                applicationFailed: 'applicationFailed'
            };

            var currentState = applicationStates.NONE;
            var operations = {
                startApp: function () {
                    var deferred = $q.defer();
                    currentState = applicationStates.STARTING;
                    $rootScope.ApplicationDocuments = {};
                    $activityManager.startActivityWithKey(applicationStates.STARTING);
                    $eventAggregatorService.publish(applicationEvents.applicationStarting);

                    // get the authenticated user
                    var user = $userManagerService.getAuthenticatedUser();

                    // check that it has a role
                    if (user.state === $userManagerService.userStates.VALID) {

                        $startableManagerService.startServices().then(function () {
                            currentState = applicationStates.STARTED;
                            $eventAggregatorService.publish(applicationEvents.applicationStarted);
                            $activityManager.stopActivityWithKey(applicationStates.STARTING);
                            deferred.resolve();
                        }, function () {
                            currentState = applicationStates.FAILED;
                            $eventAggregatorService.publish(applicationEvents.applicationFailed);
                            $activityManager.stopActivityWithKey(applicationStates.STARTING);
                            deferred.reject();
                        });
                    } else {
                        currentState = applicationStates.FAILED;
                        $logger.logInfo(user.state);
                        $eventAggregatorService.publish(applicationEvents.applicationFailed);
                        $activityManager.stopActivityWithKey(applicationStates.STARTING);
                        deferred.reject();
                    }
                    return deferred.promise;
                }
            };

            return {
                applicationStates: applicationStates,
                applicationEvents: applicationEvents,
                startApp: operations.startApp,
                getCurrentState: function () {
                    return currentState;
                }
            };
        }
    ]);
