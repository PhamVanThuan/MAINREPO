angular.module('capitecApp.home.apply.application-submit', [
    'ui.router',
    'capitecApp.home.content.apply.for-new-home.client-capture',
    'capitecApp.home.content.apply.for-switch.client-capture'
])

.controller('applicationSubmitCtrl', ['$rootScope', '$scope', '$state', '$stateParams', '$commandManager', '$capitecCommands', '$queryManager', '$capitecQueries', '$notificationService', '$printingService', '$calculatorDataService', '$activityManager', '$config', '$templateCache',
    function ApplicationSubmitController($rootScope, $scope, $state, $stateParams, $commandManager, $capitecCommands, $queryManager, $capitecQueries, $notificationService, $printingService, $calculatorDataService, $activityManager, $config, $templateCache) {
        $scope.application = $calculatorDataService.getDataValue('application');

        if (!$scope.application) {
            $state.transitionTo('home.content.apply');
        }

        $scope.templateUrl = "consent-form.tpl.html";

        $scope.allApplicantsSigned = function() {
            var allApplicantsSigned = true;
            angular.forEach($scope.applicants, function(applicant) {
                if (applicant.hasSigned !== true) {
                    allApplicantsSigned = false;
                }
            });
            return allApplicantsSigned;
        };

        $scope.SubmitApplication = function() {
            $activityManager.startActivityWithKey("submittingApplication");
            
            var newGuidQuery = new $capitecQueries.GetNewGuidQuery();

            $queryManager.sendQueryAsync(newGuidQuery).then(function(guidData) {
                var applicationID = guidData.data.ReturnData.NewGuid;
                var command = (($state.current.name.match("for-switch") != null) ? new $capitecCommands.ApplyForSwitchLoanCommand(applicationID, $scope.application) : new $capitecCommands.ApplyForNewPurchaseCommand(applicationID, $scope.application));
                $commandManager.sendCommandAsync(command).then(function(resultData) {
                    if (!resultData.data.SystemMessages.HasErrors) {
                        var data = {
                            applicationID: applicationID
                        };
                        $calculatorDataService.clearData();
                        $state.transitionTo($state.current.data.next, data);
                    }
                    $activityManager.stopActivityWithKey("submittingApplication");
                });
            });
        };

        $scope.printAllApplicants = function() {
            $printingService.printFromTemplateUrl($scope.templateUrl, {
                applicants: $scope.applicants
            });
        };

        $scope.printForApplicant = function(applicant) {
            $printingService.printFromTemplateUrl($scope.templateUrl, {
                applicants: [applicant]
            });
        };

        $scope.goBack = function() {
            $state.transitionTo($state.current.data.previous, {});
        };

        var declarationAnswersLookup;
        function loadApplicants() {
            $scope.applicants = [];
            angular.forEach($scope.application.applicants, function(applicant) {
                var yesAnswer = getLookupId('Yes', declarationAnswersLookup);
                var exclude = applicant.declarations.excludeFromDirectMarketing == yesAnswer;

                $scope.applicants.push({
                    firstName: applicant.information.firstName,
                    surname: applicant.information.surname,
                    identityNumber: applicant.information.identityNumber,
                    hasSigned: false,
                    excludeFromDirectMarketing: exclude
                });
            });
        }
        function initialise() {
            
            $queryManager.sendQueryAsync(new $capitecQueries.GetDeclarationTypesQuery()).then(function (data) {
                declarationAnswersLookup = data.data.ReturnData.Results.$values;
                loadApplicants(); 

                if ($config.PrintConsentForm) {
                    $scope.$evalAsync($scope.printAllApplicants);
                }
            });
        }
     
        initialise();

        function getLookupId(value, lookup) {
            if (value === undefined || lookup === undefined) {
                return '';
            }
            for (var i = 0; i < lookup.length; i++) {
                if (lookup[i].Name === value) {
                    return lookup[i].Id;
                }
            }
            return '';
        }
    }
]);
