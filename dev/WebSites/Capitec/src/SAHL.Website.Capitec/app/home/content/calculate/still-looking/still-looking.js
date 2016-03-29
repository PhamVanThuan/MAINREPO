angular.module('capitecApp.home.still-looking', [
  'ui.router',  
  'SAHL.Services.Interfaces.Capitec.queries',
  'capitecApp.services',
  'capitecApp.config',
  'capitecApp.home.still-looking.calculation-result'
])

.config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state('home.content.still-looking', {
        url: '/still-looking',
        templateUrl: 'still-looking.tpl.html',
        data: { title: 'calculate', breadcrumb: 'Your Affordability', pageHeading: 'see how much you could qualify for' },
        controller: 'CalculateStillLookingCtrl'
    });
}])

.controller('CalculateStillLookingCtrl', ['$scope', '$state', '$queryManager', '$capitecQueries', '$config', '$activityManager', '$calculatorDataService','$timeout', '$templateCache', function CalculateStillLookingController($scope, $state, $queryManager, $capitecQueries, $config, $activityManager, $calculatorDataService, $timeout, $templateCache) {
    var self = this;
    var eventHandler;

    var initialiseControl = function () {        
        $scope.errorMessages = {};
        $scope.errorMessage = '';
        $scope.userAdjustedInterestRate = false;
        $scope.calcRate = 0;
        eventHandler = {
            debouncedEvent: $.debounce(500, false, performInterestRateQuery)
        };
        getServiceFeeAmount();
        performInterestRateQuery();
        self.initialiseFocusItems();
    };

    /** Initialises the scope focus items to false  **/
    self.initialiseFocusItems = function () {
        $scope.model = {
            totalIncomeFocus: true,
            cashDepositFocus: false,
            interestRateFocus: false
        };
    };

    $scope.incomeChanged = function () {
       eventHandler.debouncedEvent();
    };

    function performInterestRateQuery() {
        var income = 0;
        if($scope.householdIncome) { income = $scope.householdIncome; }
        var query = new $capitecQueries.GetAffordabilityInterestRateQuery(income, 0,0,true);
        $queryManager.sendQueryAsync(query).then(function(data) {
            $scope.assignedInterestRate = data.data.ReturnData.InterestRate;
            $scope.calcRate = (parseFloat($scope.assignedInterestRate) * 100).toFixed(2);
        }, function() {
            $scope.errorMessage = 'query failure';
        });
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

    $scope.checkIfUserUpdatedRate = function () {
        if (!$scope.assignedInterestRate) { return; }
        if ($scope.assignedInterestRate != $scope.affordabilityForm.calcRate.value) {
            $timeout(function () {
                $scope.userAdjustedInterestRate = true;
                $('#userAdjustedInterestRate div.validationTooltip span').slideDown();
            });
        }
    };
   
    $scope.calculate = function () {
        $activityManager.startActivity();
        var cashDeposit = $scope.cashDeposit;
        if (!cashDeposit) {
            cashDeposit = 0;
        }
        var interestRate = parseFloat($scope.calcRate) / 100;

        var query = new $capitecQueries.GetAffordabilityInterestRateQuery($scope.householdIncome, cashDeposit, interestRate, false);
        $queryManager.sendQueryAsync(query).then(function (data) {
            if (!data.data.SystemMessages.HasWarnings)
            {
                processInformation(data);
            }           
            $activityManager.stopActivity();
        }, function(errorMessage) {
            $scope.errorMessage = errorMessage;
            $activityManager.stopActivity();
        });
    };

    function processInformation(data) {
        var result = data.data.ReturnData;
        if (result) {
            var results = {
                calculator: 'stilllooking',
                interestRate: parseFloat($scope.calcRate).toFixed(2),
                term : 20,
                instalment: (result.Instalment + $scope.monthlyServiceFee).toFixed(0),
                amountQualifiedFor : result.AmountQualifiedFor.toFixed(0),
                propertyPriceQualifiedFor: result.PropertyPriceQualifiedFor.toFixed(0),
                termInMonths: result.TermInMonths,
                monthlyServiceFee: $scope.monthlyServiceFee
            };        
            $scope.calculationResult = results;
            $calculatorDataService.addData('calculationResult', results);  
            $state.transitionTo('home.content.still-looking.calculation-result');
            $activityManager.stopActivity();
        }
    }

    return initialiseControl();
}]);