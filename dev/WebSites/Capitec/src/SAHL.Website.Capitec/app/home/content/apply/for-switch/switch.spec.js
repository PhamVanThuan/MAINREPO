describe("CapitecApp", function () {
    beforeEach(module('capitecApp'));

    describe(' - (Switch Controller) - ', function () {

        var $rootScope, $scope, $state, $calculatorDataService, $capitecSharedModels,$queryManager, $notificationService, $capitecQueries, $activityManager, $dropdownData, 
            $feesService, $capitecSharedModels, $validationManager, $httpBackend, $_q, occupancyTypeData, employmentTypeData, data, switchController, feeResults;

        beforeEach(inject(function ($injector, $q) {

            $rootScope = $injector.get('$rootScope');
            $scope = $rootScope.$new(); 
            $state = $injector.get('$state'); 
            $calculatorDataService = $injector.get('$calculatorDataService');
            $queryManager = $injector.get('$queryManager');
            $capitecQueries = $injector.get('$capitecQueries');
            $activityManager = $injector.get('$activityManager');
            $dropdownData = $injector.get('$dropdownData');
            $feesService = $injector.get('$feesService');
            $capitecSharedModels = $injector.get('$capitecSharedModels');
            $validationManager = $injector.get('$validationManager');
            $httpBackend = $injector.get('$httpBackend');
            $notificationService = $injector.get('$notificationService');
            $rootScope.authenticated = true;
            $_q = $q;            

            var $controller = $injector.get('$controller');
            createController = function () {
                return $controller('ApplySwitchCtrl', {
                    '$rootScope' : $rootScope,
                    '$scope' : $scope,
                    '$state' : $state,
                    '$calculatorDataService' : $calculatorDataService,
                    '$queryManager' : $queryManager,
                    '$capitecQueries' : $capitecQueries,
                    '$activityManager' : $activityManager,
                    '$dropdownData' : $dropdownData,
                    '$feesService' : $feesService,
                    '$capitecSharedModels' : $capitecSharedModels,
                    '$validationManager' : $validationManager,
                    '$q' : $q
                });
            };

            occupancyTypeData =
            {
                data:
                {
                    ReturnData:
                    {
                        Results:
                        {
                            $values : new Array()
                        }
                    }
                }
            };
            occupancyTypeData.data.ReturnData.Results.$values[0] = {Id : '81B73055-CFE4-4676-9B8E-A2D500AC88C8', Name : 'Owner Occupied'};
            occupancyTypeData.data.ReturnData.Results.$values[1] = {Id : '7FC97060-4748-4024-AF69-A2D500AC88CA', Name : 'Investment Property'};

            employmentTypeData =
            {
                data:
                {
                    ReturnData:
                    {
                        Results:
                        {
                            $values : new Array()
                        }
                    }
                }
            };
            employmentTypeData.data.ReturnData.Results.$values[0] = {Id : 'DBEA5E1C-A711-48DC-9CB6-A2D500AB5A72', Name : 'Salaried',  IsActive : 1};
            employmentTypeData.data.ReturnData.Results.$values[1] = {Id : '6795B5CE-2835-4675-8039-A2D500AB5A73', Name : 'Self Employed', IsActive : 1};
            employmentTypeData.data.ReturnData.Results.$values[2] = {Id : 'DE1590B5-25FD-4334-97CC-A2D500AB5A74', Name : 'Salaried with Housing Allowance', IsActive : 1};
            employmentTypeData.data.ReturnData.Results.$values[3] = {Id : '199534AD-B097-48A2-A1A4-A2ED00F7D232', Name : 'Salaried with Commission', IsActive : 1};
            $scope.cashRequired = 100000.00;

            data = {
                data: 
                {
                    ReturnData: 
                    {
                        InterestRate: 9.20,
                        LTV: 88.89,
                        PTI: 14.60,
                        Instalment: 7301.03,
                        LoanAmount: 800000.00,
                        TermInMonths: 240,
                        InterestRateAsPercent: '9.20%',
                        LTVAsPercent: '88.89%',
                        PTIAsPercent: '14.60%',
                        EligibleApplication: true,
                        DecisionTreeMessages: {
                            AllMessages: {
                                $values:[]
                            }
                        },
                    }
                }
            };

            feeCalcData = {
                calculator: 'switch',
                InterestRate: 9.20,
                CategoryKey: 4,
                LTV: 88.89,
                PTI: 14.60,
                Instalment: 7301.03,
                LoanAmount: 800000.00,
                TermInMonths: 240,
                Results:{
                    $values : new Array({
                        CancellationFee: 0,
                        InitiationFee: 0,
                        RegistrationFee: 0,
                        interimInterest: 0,
                        total: 0
                    })
                }
            };

            dataFee = {
                data:
                    {
                        ReturnData:
                        {                           
                            Results: {
                                $values: new Array({
                                   ControlNumeric:50
                                })
                            }
                        }
                    }
            };

            feeResults = { 
                    InterimInterest: "5000.00",
                    LTV: "88.89",
                    PTI: "14.60",
                    calculator: "switch",
                    cancellationFee: "1500.00",
                    categoryKey: 4,
                    initiationFee: "5529.00",
                    instalment: "7301.03",
                    interestRate: "9.20",
                    loanAmount: "800000.00",
                    registrationFee: "11248.57",
                    termInMonths: 240,
                    total: "16777.57"
                };      

        }));

        describe('when initailising the switch controller', function() {
            beforeEach(function() {
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function (queryCommand) {
                    $deferred = $_q.defer();
                    if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetOccupancyTypesQuery,SAHL.Services.Interfaces.Capitec') 
                    {
                        $deferred.resolve(occupancyTypeData);
                    }
                    else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetEmploymentTypesQuery,SAHL.Services.Interfaces.Capitec') 
                    {
                        $deferred.resolve(employmentTypeData);
                    }
                    return $deferred.promise;
                });
                switchController = createController();
                $scope.$digest();
            });
            it('should set the $scope.occupancyTypes value', function () {
                expect($scope.occupancyTypes[0]).not.toBe(undefined);
            });

            it('should set the $scope.incomeTypes value', function () {
                expect($scope.incomeTypes[0]).not.toBe(undefined);
            });

            it('should default the cash required value to 0', function() {
                expect($scope.cashRequired).toEqual(0);
            });
        });

        describe('when calculating successfully using the switch calculator', function() {
            var calculateQuery = { _name: 'SAHL.Services.Interfaces.Capitec.Queries.CalculateApplicationScenarioSwitchQuery,SAHL.Services.Interfaces.Capitec' };
            var skipActivityManager = true;
            beforeEach(function() {
                spyOn($feesService, 'CalculateFees').and.callFake(function(calcRequest, queryManager, activityManager, queries, done) {
                    done(null, feeCalcData);
                });
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function (queryCommand, undefined, undefined, undefined, skipActivityManager) {
                    $deferred = $_q.defer();
                    if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetControlValueQuery,SAHL.Services.Interfaces.Capitec')
                    {                        
                        $deferred.resolve(dataFee);
                    }
                    else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetOccupancyTypesQuery,SAHL.Services.Interfaces.Capitec') 
                    {
                        $deferred.resolve(occupancyTypeData);
                    }
                    else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetEmploymentTypesQuery,SAHL.Services.Interfaces.Capitec') 
                    {
                        $deferred.resolve(employmentTypeData);
                    }
                     else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.CalculateApplicationScenarioSwitchQuery,SAHL.Services.Interfaces.Capitec') 
                    {
                       $deferred.resolve(data);
                    }
                    else
                    {
                        throw 'Command '+ queryCommand._name + ' was not found';
                    }
                    return $deferred.promise;
                });
                spyOn($capitecQueries, 'CalculateApplicationScenarioSwitchQuery').and.callFake(function (loanDetails) {
                    calculateQuery.laonDetails = loanDetails;
                    return calculateQuery;
                });
                spyOn($validationManager, 'Validate').and.returnValue(true);
                switchController = createController();
                $scope.occupancyType = { Id: '81B73055-CFE4-4676-9B8E-A2D500AC88C8', Name: 'Owner Occupied' };
                $scope.incomeType = { Id: 'DBEA5E1C-A711-48DC-9CB6-A2D500AB5A72', Name: 'Salaried', IsActive: 1 };
                $scope.marketValue = 1000000;
                $scope.cashRequired = 500000;
                $scope.currentBalance = 100000;
                $scope.householdIncome = 50000;
                $rootScope.$digest();
                $scope.calculate();
                $scope.$digest();
            });

            it('should calculate the fees', function() {
                expect($feesService.CalculateFees).toHaveBeenCalled();
            });
            it('should validate the loan details', function() {
                expect($validationManager.Validate).toHaveBeenCalled();
            });
            it('should get the service fee amount', function() {
                expect($scope.monthlyServiceFee).toEqual(dataFee.data.ReturnData.Results.$values[0].ControlNumeric);
            });
            it('should calculate the loan', function() {
                expect($capitecQueries.CalculateApplicationScenarioSwitchQuery).toHaveBeenCalled();
                expect($capitecQueries.CalculateApplicationScenarioSwitchQuery.calls.argsFor(0)[0].cashRequired).toEqual($scope.cashRequired);
                expect($queryManager.sendQueryAsync).toHaveBeenCalledWith(calculateQuery, undefined, undefined, undefined, true);
            });
        });


        describe('when calculating using the switch calculator', function () {
            beforeEach(function () {   
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function (queryCommand) {
                    $deferred = $_q.defer();
                    if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetOccupancyTypesQuery,SAHL.Services.Interfaces.Capitec') 
                    {
                        $deferred.resolve(occupancyTypeData);
                    }
                    else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetEmploymentTypesQuery,SAHL.Services.Interfaces.Capitec') 
                    {
                        $deferred.resolve(employmentTypeData);
                    }
                    else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.CalculateApplicationScenarioSwitchQuery,SAHL.Services.Interfaces.Capitec') 
                    {
                       $deferred.resolve(data);
                    }
                    else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetControlValueQuery,SAHL.Services.Interfaces.Capitec')
                    {                        
                        $deferred.resolve(dataFee);
                    }
                    else
                    {
                        throw 'Command '+ queryCommand._name + ' was not found';
                    }
                    return $deferred.promise;
                });

                spyOn($feesService, 'CalculateFees').and.callFake(function (calculateRequest, $queryManager, $activityManager, $capitecQueries, done) { 
                    done('An error has occured');
                });
                spyOn($notificationService, 'notifyError').and.callFake(function(){});

                switchController = createController();
                $rootScope.$digest();
            });

            afterEach(function () {
                $httpBackend.verifyNoOutstandingExpectation();
            });

            it('should call the notifyError function when CalculateFees returns an error', function() { 
                $scope.calculate(data);
                expect($notificationService.notifyError).toHaveBeenCalled();
            });

            it('should transition to the parent state', function() {
                $state.$current.parent = { name : 'home.content.apply' };
                var expectedTransitionState = $state.$current.parent.name;

                spyOn($state, 'transitionTo').and.callThrough();
                switchController = createController();    
                $scope.back();

                $rootScope.$digest();
                expect($state.transitionTo).toHaveBeenCalledWith(expectedTransitionState);
            });
        });

        describe('when calculated values do not meet the credit criteria', function () {
          
            beforeEach(function () {   
                var failingTreeData = data;
                failingTreeData.data.ReturnData.DecisionTreeMessages = 
                {
                    AllMessages: {
                        $values: [
                            {message: 'does not match credit criteria.', Severity:0}
                        ]
                    }
                };
                failingTreeData.data.ReturnData.EligibleApplication = false;
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function (queryCommand) {
                    $deferred = $_q.defer();
                    if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetOccupancyTypesQuery,SAHL.Services.Interfaces.Capitec') 
                    {
                        $deferred.resolve(occupancyTypeData);
                    }
                    else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetEmploymentTypesQuery,SAHL.Services.Interfaces.Capitec') 
                    {
                        $deferred.resolve(employmentTypeData);
                    }
                    else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.CalculateApplicationScenarioSwitchQuery,SAHL.Services.Interfaces.Capitec') 
                    {
                       $deferred.resolve(failingTreeData);
                    }
                    else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetControlValueQuery,SAHL.Services.Interfaces.Capitec')
                    {                        
                        $deferred.resolve(dataFee);
                    }
                    else
                    {
                        throw 'Command '+ queryCommand._name + ' was not found';
                    }
                    return $deferred.promise;
                });
                spyOn($feesService, 'CalculateFees').and.callFake(function (calculateRequest, $queryManager, $activityManager, $capitecQueries, done) { 
                    done(null, feeResults);
                });
            });

            it('should fail with an error message', function() {

                switchController = createController();        
                spyOn($validationManager,'Validate').and.returnValue(true);

                $scope.calculate();

                $rootScope.$digest();
                expect($scope.errorMessages[0].message).toBe('does not match credit criteria.');
            });
        });   
        
        describe('when returning the calculated result', function () {
            var calculationResult;
            beforeEach(function () {   
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function (queryCommand) {
                    
                    $deferred = $_q.defer();
                    if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetOccupancyTypesQuery,SAHL.Services.Interfaces.Capitec') 
                    {
                        $deferred.resolve(occupancyTypeData);
                    }
                    else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetEmploymentTypesQuery,SAHL.Services.Interfaces.Capitec') 
                    {
                        $deferred.resolve(employmentTypeData);
                    }
                    else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.CalculateApplicationScenarioSwitchQuery,SAHL.Services.Interfaces.Capitec') 
                    {
                        $deferred.resolve(data);
                    }
                    else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetCalculatorFeeQuery,SAHL.Services.Interfaces.Capitec')
                    {
                        $deferred.resolve(data);
                    }
                    else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetControlValueQuery,SAHL.Services.Interfaces.Capitec')
                    {                        
                        $deferred.resolve(dataFee);
                    }
                    else
                    {
                        throw 'Command '+ queryCommand._name + ' was not found';
                    }
                    return $deferred.promise;
                });
                spyOn($feesService, 'CalculateFees').and.callFake(function (calculateRequest, $queryManager, $activityManager, $capitecQueries, done) { 
                    done(null, feeResults);
                });
                createController();
                $scope.calculate();
                $scope.monthlyServiceFee = 50;

                $rootScope.$digest();
                $state.$current = 'home.content.apply.application-precheck-for-switch.switch';
                calculationResult = $calculatorDataService.getDataValue('calculationResult');
            });

            it("should set the correct calculator name", function () {
                expect(calculationResult.calculator).toEqual('switch');
            });

            it("should set the correct term ", function () {
                expect(calculationResult.termInMonths).toEqual(data.data.ReturnData.TermInMonths);
            });

            it("should calculate the correct loan amount ", function () {
                expect(calculationResult.loanAmount.toFixed(2).toString()).toEqual(data.data.ReturnData.LoanAmount.toFixed(2).toString());
            });

            it("should calculate the correct LTV ", function () {
                expect(calculationResult.LTV.toFixed(2).toString()).toEqual(data.data.ReturnData.LTV.toFixed(2).toString());
            });

            it("should calculate the correct PTI ", function () {
                expect(calculationResult.PTI.toFixed(2).toString()).toEqual(data.data.ReturnData.PTI.toFixed(2).toString());
            });

            it("should use the correct category key ", function () {
                expect(calculationResult.categoryKey).toEqual(feeCalcData.CategoryKey);
            });

            it("should use the correct interestRate ", function () {
                expect(calculationResult.interestRate.toFixed(2).toString()).toEqual(data.data.ReturnData.InterestRate.toFixed(2).toString());
            });

            it("should calculate the correct instalment ", function () {
                var instalment = parseFloat(data.data.ReturnData.Instalment.toFixed(2));
                expect(calculationResult.instalment.toString()).toEqual((instalment + $scope.monthlyServiceFee).toFixed(2).toString());
            });
        });

        describe('when the CalculateApplicationScenarioSwitchQuery fails', function() {
            var calculateQuery = {_name:'SAHL.Services.Interfaces.Capitec.Queries.CalculateApplicationScenarioSwitchQuery,SAHL.Services.Interfaces.Capitec'};
            beforeEach(function() {
                spyOn($feesService, 'CalculateFees').and.callFake(function(calcRequest, queryManager, activityManager, queries, done) {
                    done(null, feeCalcData);
                });
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function (queryCommand) {
                    $deferred = $_q.defer();
                    if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetControlValueQuery,SAHL.Services.Interfaces.Capitec')
                    {                        
                        $deferred.resolve(dataFee);
                    }
                    else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetOccupancyTypesQuery,SAHL.Services.Interfaces.Capitec') 
                    {
                        $deferred.resolve(occupancyTypeData);
                    }
                    else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetEmploymentTypesQuery,SAHL.Services.Interfaces.Capitec') 
                    {
                        $deferred.resolve(employmentTypeData);
                    }
                     else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.CalculateApplicationScenarioSwitchQuery,SAHL.Services.Interfaces.Capitec') 
                    {
                       $deferred.reject(data);
                    }
                    else
                    {
                        throw 'Command '+ queryCommand._name + ' was not found';
                    }
                    return $deferred.promise;
                });
                spyOn($capitecQueries, 'CalculateApplicationScenarioSwitchQuery').and.callFake(function(loanDetails){
                    calculateQuery.loanDetails = loanDetails
                    return calculateQuery;
                });
                spyOn($validationManager, 'Validate').and.returnValue(true);
                switchController = createController();
                $rootScope.$digest();
                $scope.calculate();
                $scope.$digest();
            });

            it('should fail with an error message', function() {
                expect($scope.errorMessage).toBe('query failure.');
            });
        });
    });
});