angular.module('capitecApp.home.content.apply.for-switch', [
  'ui.router',
  'SAHL.Services.Interfaces.Capitec.queries',
  'capitecApp.services',
  'capitecApp.home.apply.client-capture',
  'capitecApp.home.content.apply.for-switch.client-capture',
  'capitecApp.home.content.apply.for-switch.calculation-result',
  'capitecApp.home.content.apply.for-switch.application-result',
  'SAHL.Services.Interfaces.Capitec.sharedmodels'
])

.config(['$stateProvider', function config($stateProvider) {
    $stateProvider.state('home.content.apply.application-precheck-for-switch.switch', {
        url: '/for-switch',
        templateUrl: 'switch.tpl.html',
        data: { breadcrumb: 'Financial Info', pageHeading: 'financial info for switching a home loan' },
        controller: 'ApplySwitchCtrl'
    });
}])
.controller('ApplySwitchCtrl', ['$rootScope', '$scope', '$state', '$calculatorDataService', '$queryManager', '$capitecQueries', '$activityManager', '$dropdownData', '$feesService', '$capitecSharedModels', '$validationManager', '$notificationService', '$q', '$templateCache',
    function ApplySwitchController($rootScope, $scope, $state, $calculatorDataService, $queryManager, $capitecQueries, $activityManager, $dropdownData, $feesService, $capitecSharedModels, $validationManager, $notificationService, $q, $templateCache) {
        $scope.occupancyTypes = [];
        $scope.incomeTypes = [];
        $scope.cashRequired = 0;
        $scope.currentBalance = 0;
        var calculatedResult = {};

        var getOccupancyTypesQuery = new $capitecQueries.GetOccupancyTypesQuery();
        $queryManager.sendQueryAsync(getOccupancyTypesQuery).then(function (data) {
            $scope.occupancyTypes = data.data.ReturnData.Results.$values;
        });
        var getEmploymentTypesQuery = new $capitecQueries.GetEmploymentTypesQuery();
        $queryManager.sendQueryAsync(getEmploymentTypesQuery).then(function (data) {
            $scope.incomeTypes = data.data.ReturnData.Results.$values;
        });

        $scope.back = function () {
            $activityManager.stopActivity();
            $state.transitionTo($state.$current.parent.name);
        };

        $scope.validateLoanBalance = function () {
            var currentBalance = $scope.currentBalance;
            if (currentBalance === 0 || currentBalance === "0") {
                $scope.currentBalance = "0";
                $notificationService.notifyInfo("Info", "Are you sure you have no balance on your bond account?");
            } 
        };

        $scope.calculate = function () {
            var loanDetails = new $capitecSharedModels.SwitchLoanDetails(
                $scope.occupancyType, $scope.incomeType, $scope.householdIncome, $scope.marketValue, 
                $scope.cashRequired, $scope.currentBalance, 0, 0, 240, true);                
            
                $scope.validateLoanBalance();
            if ($validationManager.Validate(loanDetails)) {
                $activityManager.startActivity();
                $q.when(calculateFees(loanDetails)).then(function(results) {
                    calculatedResult = results;
                    loanDetails.fees = results.total;
                    loanDetails.interimInterest = results.InterimInterest;

                    calculateLoanApplication(loanDetails)['finally'](function () {
                        $activityManager.stopActivity();
                    });
                });
            }
            else {
                $activityManager.stopActivity();
            }
        };

        function calculateFees(loanDetails) {
            var deferred = $q.defer();
            var calculateRequest = {
                calculator: 'switch',
                loanAmount: $scope.cashRequired + $scope.currentBalance,
                cashOut: $scope.cashRequired,
                termInMonths: 240
            };
            $feesService.CalculateFees(calculateRequest, $queryManager, $activityManager, $capitecQueries,
                function onCompletion(err, results) {
                    if(err) {
                        $notificationService.notifyError(err); 
                        deferred.reject(err);
                    } else {
                        deferred.resolve(results);
                    }
                });
            return deferred.promise;
        }

        function calculateLoanApplication(loanDetails) {
            var query = new $capitecQueries.CalculateApplicationScenarioSwitchQuery(loanDetails);
            return $q.all([
                $queryManager.sendQueryAsync(query, undefined, undefined, undefined, true),
                getServiceFeeAmount()
            ]).then(function (data) {
                processInformation(data[0], loanDetails);
            }, function (errorMessage) {
                $scope.errorMessage = 'query failure.';
            });
        }

        function processInformation(data, loanDetails){
            var result = data.data.ReturnData;
            if (result && result.EligibleApplication === true) {
                updateCalculatedResultWithLoanResult(result);
                var application = createApplication(loanDetails);

                $calculatorDataService.addData('currentApplicant', 0);
                $calculatorDataService.addData('application', application);
                $calculatorDataService.addData('calculationResult', calculatedResult);

                $state.transitionTo('home.content.apply.application-precheck-for-switch.switch.calculation-result');
            } else {
                var warningMessages = [];
                angular.forEach(result.DecisionTreeMessages.AllMessages.$values, function(message) {
                    if(message.Severity===0)
                        warningMessages.push(message);
                });
                $scope.errorMessages = warningMessages;
                $activityManager.stopActivity();
                $state.transitionTo('home.content.apply.application-precheck-for-switch.switch.calculation-result-fail');
            }           
        }
        /* Call to GetControlValueQuery('SAHLMonthlyFEE')*/
        var getServiceFeeAmount = function () {
            var getMonthlyServiceFee = new $capitecQueries.GetControlValueQuery('SAHLMonthlyFee');
            $queryManager.sendQueryAsync(getMonthlyServiceFee).then(handleSuccess, handleFailure);
        };

        /* Handle failure for GetControlValueQuery('SAHLMonthlyFEE')*/
        var handleFailure = function (result) {
            $scope.errorMessage = result;
            $activityManager.stopActivity();           
        };

        /* Handle success for GetControlValueQuery('SAHLMonthlyFEE')*/
        var handleSuccess = function (data) {
            if (data.data.ReturnData.Results.$values[0].ControlNumeric) {
                $scope.monthlyServiceFee = data.data.ReturnData.Results.$values[0].ControlNumeric;
            }            
        };

        function createApplication(loanDetails) {
            var applicationCaptureStart = $calculatorDataService.getDataValue('applicationCaptureStart');
            var application = new $capitecSharedModels.SwitchLoanApplication(loanDetails, [], $rootScope.userId, applicationCaptureStart);
            application.applicants.push(new $capitecSharedModels.Applicant(
                new $capitecSharedModels.ApplicantInformation(),
                new $capitecSharedModels.ApplicantResidentialAddress(),
                new $capitecSharedModels.ApplicantEmploymentDetails(),
                new $capitecSharedModels.ApplicantDeclarations())
            );
            application.applicants[0].errorValidationMessages = [];
            return application;
        }

        function updateCalculatedResultWithLoanResult(result) {
            calculatedResult.PTI = result.PTI;
            calculatedResult.LTV = result.LTV;
            calculatedResult.interestRate = result.InterestRate;
            calculatedResult.instalment = result.Instalment + $scope.monthlyServiceFee;
            calculatedResult.loanAmount = result.LoanAmount;
            calculatedResult.PTIAsPercent = result.PTIAsPercent;
            calculatedResult.LTVAsPercent = result.LTVAsPercent;
            calculatedResult.interestRateAsPercent = result.InterestRateAsPercent;
            calculatedResult.monthlyServiceFee = $scope.monthlyServiceFee;
        }
    }]);