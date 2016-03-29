angular.module('capitecApp.home.apply.application-result', [
    'ui.router'
])

.controller('applicationResultCtrl', ['$rootScope', '$scope', '$state', '$stateParams', '$commandManager', '$capitecCommands', '$queryManager', '$capitecQueries', '$notificationService','$timeout', '$templateCache',
    function ApplicationResultController($rootScope, $scope, $state, $stateParams, $commandManager, $capitecCommands, $queryManager, $capitecQueries, $notificationService, $timeout, $templateCache) {
        var applicationID = $stateParams.applicationID;
        $scope.applicants = [];
        $scope.applicationResult = {};
        $scope.applicationSuccessful = null;

        function init() {
            if(applicationID) {
                var applicationResultQuery = new $capitecQueries.GetApplicationResultQuery(applicationID);
                $queryManager.sendQueryAsync(applicationResultQuery).then(function(data) {
                    $timeout(function() {
                        var result = data.data.ReturnData;
                        $scope.applicationSuccessful = result.Submitted;
                        $state.current.data.icon = result.Submitted ? "success" : "failure";
                        $state.current.data.pageHeading = result.Submitted ? "your application has been submitted" : "your application has been declined";
                        $scope.applicationNumber = result.ApplicationNumber;

                        var firstApplicant = createApplicant(result.FirstApplicantName, result.FirstApplicantITCPassed, result.FirstApplicantITCMessages);
                        var secondApplicant = createApplicant(result.SecondApplicantName, result.SecondApplicantITCPassed, result.SecondApplicantITCMessages);
                        
                        var applicationMessages = [];
                        if(result.ApplicationCalculationMessages) {
                            applicationMessages = result.ApplicationCalculationMessages.AllMessages.$values;
                        }
                        var applicationResult = {
                            passed: result.Submitted,
                            messages: applicationMessages
                        };
                        $scope.applicants.push(firstApplicant);
                        $scope.applicants.push(secondApplicant);
                        $scope.applicationResult = applicationResult;
                    });
                }).then(function() {
                    var date = new Date();
                    var applicationCaptureEnd = new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(), date.getMinutes(), date.getSeconds())).getTime();
                    var UpdateCaptureEndTimeCommand = new $capitecCommands.UpdateCaptureEndTimeCommand(applicationID, applicationCaptureEnd);
                    $commandManager.sendCommandAsync(UpdateCaptureEndTimeCommand);
                });
            }
        }

        function createApplicant(name, passed, itcMessages) {
            var messages = [];
            if(itcMessages) {
                messages = itcMessages.AllMessages.$values;
            }
            return {
                name: name,
                passed: passed,
                messages: messages
            };
        }
        init();
    }
]);

