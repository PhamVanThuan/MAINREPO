angular.module('capitecApp.home.content.apply.for-new-home', [
  'ui.router',
  'SAHL.Services.Interfaces.Capitec.queries',
  'capitecApp.services',
  'capitecApp.home.apply.client-capture',
  'capitecApp.home.content.apply.for-new-home.client-capture',
  'capitecApp.home.content.apply.for-new-home.calculation-result',
  'capitecApp.home.content.apply.for-new-home.application-result',
  'SAHL.Services.Interfaces.Capitec.sharedmodels'
])

.config(['$stateProvider', function config($stateProvider) {
    $stateProvider.state('home.content.apply.application-precheck-for-new-home.new-home', {
        url: '/for-new-home',
        templateUrl: 'new-home.tpl.html',
        data: { breadcrumb: 'Financial Info', pageHeading: 'financial info for purchasing a new home', next: 'home.content.apply.application-precheck-for-new-home.new-home.calculation-result' },
        controller: 'ApplyNewHomeCtrl'
    });
}])
.controller('ApplyNewHomeCtrl', ['$rootScope', '$scope', '$state', '$calculatorDataService', '$queryManager', '$capitecQueries', '$activityManager', '$notificationService', '$dropdownData', '$feesService', '$capitecSharedModels', '$validationManager', '$cookieStore', '$q', '$templateCache',

    function ApplyNewHomeController($rootScope, $scope, $state, $calculatorDataService, $queryManager, $capitecQueries, $activityManager, $notificationService, $dropdownData, $feesService, $capitecSharedModels, $validationManager, $cookieStore, $q, $templateCache) {
        $scope.occupancyTypes = [];
        $scope.incomeTypes = [];
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

        $scope.calculate = function () {
            var loanDetails = new $capitecSharedModels.NewPurchaseLoanDetails($scope.occupancyType, $scope.incomeType, 
                $scope.householdIncome, $scope.purchasePrice, $scope.deposit, 0, 240, false);
            if ($validationManager.Validate(loanDetails)) {
                $activityManager.startActivity();
                $q.when(calculateFees(loanDetails)).then(function(results) {
                    calculatedResult = results;
                    loanDetails.fees = results.total;

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
                calculator: 'newHome',
                loanAmount: $scope.purchasePrice - $scope.deposit,
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
            var query = new $capitecQueries.CalculateApplicationScenarioNewPurchaseQuery(loanDetails);
            return $q.all([
                $queryManager.sendQueryAsync(query, undefined, undefined, undefined, true),
                getServiceFeeAmount()
            ]).then(function (data) {
                processInformation(data[0], loanDetails);
            }, function (errorMessage) {
                $scope.errorMessage = 'query failure.';
            });
        }

        function processInformation(data, loanDetails) {
            var result = data.data.ReturnData;
            if (result && result.EligibleApplication === true) {
                updateCalculatedResultWithLoanResult(result);
                var application = createApplication(loanDetails);

                $calculatorDataService.addData('currentApplicant', 0);
                $calculatorDataService.addData('application', application);
                $calculatorDataService.addData('calculationResult', calculatedResult);

                $state.transitionTo('home.content.apply.application-precheck-for-new-home.new-home.calculation-result');
            } else {
                var warningMessages = [];
                angular.forEach(result.DecisionTreeMessages.AllMessages.$values, function (message) {
                    if (message.Severity === 0)
                        warningMessages.push(message);
                });
                $scope.errorMessages = warningMessages;
                $activityManager.stopActivity();
                $state.transitionTo('home.content.apply.application-precheck-for-new-home.new-home.calculation-result-fail');
            }
        };

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
            var application = new $capitecSharedModels.NewPurchaseApplication(loanDetails, [], $rootScope.userId, applicationCaptureStart);
            $scope.application = application;

            $scope.application.applicants.push(new $capitecSharedModels.Applicant(
                new $capitecSharedModels.ApplicantInformation(),
                new $capitecSharedModels.ApplicantResidentialAddress(),
                new $capitecSharedModels.ApplicantEmploymentDetails(),
                new $capitecSharedModels.ApplicantDeclarations()));
            $scope.application.applicants[0].errorValidationMessages = [];
            return application;
        }

        function updateCalculatedResultWithLoanResult(result) {
            calculatedResult.PTI = result.PTI;
            calculatedResult.LTV = result.LTV;
            calculatedResult.interestRate = result.InterestRate;
            calculatedResult.instalment = parseFloat(result.Instalment) + parseFloat($scope.monthlyServiceFee);
            calculatedResult.loanAmount = result.LoanAmount;
            calculatedResult.PTIAsPercent = result.PTIAsPercent;
            calculatedResult.LTVAsPercent = result.LTVAsPercent;
            calculatedResult.interestRateAsPercent = result.InterestRateAsPercent;
            calculatedResult.monthlyServiceFee = $scope.monthlyServiceFee;
        }
    }]);