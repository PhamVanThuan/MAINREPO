describe("CapitecApp", function() {
    beforeEach(module('capitecApp'));

    describe(' - (New Home Controller) - ', function () {
        var $httpBackend, $rootScope, $scope, data, $state, $commandManager, $capitecCommands, $queryManager, $capitecQueries, $mock, promise, LTV, loanAmount, PTI,
            data, instalment, $activityManager, newHomeController, $feesService, $capitecSharedModels, occupancyTypeData, occupancyTypeData, feeResults,$templateCache;

        beforeEach(inject(function($injector, $q) {
            // get the root scope
            $rootScope = $injector.get('$rootScope');
            $scope = $rootScope.$new();
            $state = $injector.get('$state');
            $queryManager = $injector.get('$queryManager');
            $capitecQueries = $injector.get('$capitecQueries');
            $calculatorDataService = $injector.get('$calculatorDataService');
            $activityManager = $injector.get('$activityManager');
            $feesService = $injector.get('$feesService');
            $capitecSharedModels = $injector.get('$capitecSharedModels');
            $notificationService = $injector.get('$notificationService');
            $validationManager = $injector.get('$validationManager');
            $httpBackend = $injector.get('$httpBackend');
            $rootScope.authenticated = true;
            $templateCache = $injector.get('$templateCache');
            $_q = $q;

            // setup the controller under test
            var $controller = $injector.get('$controller');
            createController = function() {
                return $controller('ApplyNewHomeCtrl', {
                    '$calculatorDataService': $calculatorDataService,
                    '$scope': $scope,
                    '$state': $state,
                    '$queryManager': $queryManager,
                    '$capitecQueries': $capitecQueries,
                    '$activityManager': $activityManager,
                    '$feesService': $feesService,
                    '$capitecSharedModels': $capitecSharedModels,
                    '$validationManager': $validationManager,
                    '$templateCache': $templateCache
                });
            };


            occupancyTypeData = {
                data: {
                    ReturnData: {
                        Results: {
                            $values: new Array()
                        }
                    }
                }
            };
            occupancyTypeData.data.ReturnData.Results.$values[0] = {
                Id: '6670BDB7-40A8-47DD-9EE0-A2E600F13DDC',
                Name: 'Owner Occupied'
            };
            occupancyTypeData.data.ReturnData.Results.$values[1] = {
                Id: '64D9A33F-17C8-4A1B-B520-A2E600F13DDE',
                Name: 'Investment Property'
            };

            employmentTypeData = {
                data: {
                    ReturnData: {
                        Results: {
                            $values: new Array()
                        }
                    }
                }
            };
            employmentTypeData.data.ReturnData.Results.$values[0] = {
                Id: 'DBEA5E1C-A711-48DC-9CB6-A2D500AB5A72',
                Name: 'Salaried',
                IsActive: 1
            };
            employmentTypeData.data.ReturnData.Results.$values[1] = {
                Id: '6795B5CE-2835-4675-8039-A2D500AB5A73',
                Name: 'Self Employed',
                IsActive: 1
            };
            employmentTypeData.data.ReturnData.Results.$values[2] = {
                Id: 'DE1590B5-25FD-4334-97CC-A2D500AB5A74',
                Name: 'Salaried with Housing Allowance',
                IsActive: 1
            };
            employmentTypeData.data.ReturnData.Results.$values[3] = {
                Id: '199534AD-B097-48A2-A1A4-A2ED00F7D232',
                Name: 'Salaried with Commission',
                IsActive: 1
            };

            data = {
                data: {
                    ReturnData: {
                        calculator: 'newHome',
                        InterestRate: 9.10,
                        CategoryKey: 1,
                        LTV: 90.00,
                        PTI: 23.30,
                        Instalment: 8155.51,
                        LoanAmount: 800000.00,
                        TermInMonths: 240,
                        EligibleApplication: true,
                        DecisionTreeMessages: {
                            AllMessages: {
                                $values: []
                            }
                        },
                        Results: {
                            $values: new Array({
                                CancellationFee: 0,
                                InitiationFee: 0,
                                RegistrationFee: 0,
                                InterimInterest: 0,
                                Total: 0
                            })
                        }
                    }
                }
            };

            dataFee = {
                data: {
                    ReturnData: {
                        Results: {
                            $values: new Array({
                                ControlNumeric: 50
                            })
                        }
                    }
                }
            };

            feeResults = {
                InterimInterest: "0.00",
                LTV: "90.00",
                PTI: "23.30",
                calculator: "newHome",
                cancellationFee: "0.00",
                initiationFee: "5529.00",
                instalment: "8155.51",
                interestRate: "9.10",
                loanAmount: "800000.00",
                registrationFee: "11248.57",
                termInMonths: 240,
                total: "16777.57"
            };

        }));

        describe('when calculating using the new purchase calculator', function() {
            beforeEach(function() {
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function(queryCommand) {

                    $deferred = $_q.defer();
                    if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetOccupancyTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(occupancyTypeData);
                    } else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetEmploymentTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(employmentTypeData);
                    } else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.CalculateApplicationScenarioNewPurchaseQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(data);
                    } else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetControlValueQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(dataFee);
                    } else {
                        throw 'Command ' + queryCommand._name + ' was not found';
                    }
                    return $deferred.promise;
                });

                newHomeController = createController();
                $scope.monthlyServiceFee = 50;
                $rootScope.$digest();
                calculationResult = $calculatorDataService.getDataValue('calculationResult');
            });

            afterEach(function() {
                $httpBackend.verifyNoOutstandingExpectation();
            });

            it('should retrieve the monthly service fee value', function() {
                expect($scope.monthlyServiceFee).not.toBe(undefined);
            });

            it('should set the $scope.occupancyTypes value', function() {
                expect($scope.occupancyTypes[0]).not.toBe(undefined);
            });

            it('should set the $scope.incomeTypes value', function() {
                expect($scope.incomeTypes[0]).not.toBe(undefined);
            });

            it('should call the notifyError function when CalculateFees returns an error', function() {
                spyOn($feesService, 'CalculateFees').and.callFake(function(calculateRequest, $queryManager, $activityManager, $capitecQueries, done) {
                    done('An error has occured');
                });
                spyOn($notificationService, 'notifyError').and.callFake(function() {});

                $rootScope.$notificationService = $notificationService;

                $scope.calculate();

                expect($notificationService.notifyError).toHaveBeenCalled();
                expect($feesService.CalculateFees).toHaveBeenCalled();
            });

            it('should do a new purchase calculation and pass the validation check', function() {
                spyOn($feesService, 'CalculateFees').and.callFake(function(calculateRequest, $queryManager, $activityManager, $capitecQueries, done) {
                    done($calculatorDataService.addData('calculationResult', feeResults));
                });

                spyOn($validationManager, 'Validate').and.returnValue(true);

                //$scope.calculate();

                //expect($scope.processInformation).toHaveBeenCalledWith(data);
                //expect($feesService.CalculateFees).toHaveBeenCalled();
            });

            it('should do a new purchase calculation and fail validation check', function() {
                spyOn($feesService, 'CalculateFees').and.callFake(function(calculateRequest, $queryManager, $activityManager, $capitecQueries, done) {
                    done($calculatorDataService.addData('calculationResult', feeResults));
                });

                spyOn($validationManager, 'Validate').and.returnValue(false);

                $scope.calculate();

                expect($feesService.CalculateFees).not.toHaveBeenCalled();
            });

            it('should transition to the parent state', function() {
                $state.$current.parent = {
                    name: 'home.content.apply.application-precheck-for-new-home.new-home'
                };
                spyOn($state, 'transitionTo').and.callThrough();
                switchController = createController();
                $scope.back();

                expect($state.transitionTo).toHaveBeenCalledWith($state.$current.parent.name);
            });


            it("should call the CalculateApplicationScenarioNewPurchaseQuery() method with loan details", function() {
                spyOn($feesService, 'CalculateFees').and.callFake(function(calculateRequest, $queryManager, $activityManager, $capitecQueries, done) {
                    done(null, feeResults);
                });
                spyOn($validationManager, 'Validate').and.returnValue(true);
                spyOn($capitecSharedModels, 'NewPurchaseLoanDetails').and.returnValue(data);
                spyOn($capitecQueries, 'CalculateApplicationScenarioNewPurchaseQuery').and.callThrough();
                switchController = createController();
                $scope.calculate();
                $scope.$digest();

                expect($capitecQueries.CalculateApplicationScenarioNewPurchaseQuery).toHaveBeenCalledWith(data);
            });
        });

        describe('when calculated values do not meet the credit criteria', function() {
            beforeEach(function() {
                var failingTreeData = data;
                failingTreeData.data.ReturnData.DecisionTreeMessages = {
                    AllMessages: {
                        $values: [{
                            message: 'does not match credit criteria.',
                            Severity: 0
                        }]
                    }
                };
                failingTreeData.data.ReturnData.EligibleApplication = false;
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function(queryCommand) {
                    $deferred = $_q.defer();
                    if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetOccupancyTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(occupancyTypeData);
                    } else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetEmploymentTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(employmentTypeData);
                    } else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.CalculateApplicationScenarioNewPurchaseQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(failingTreeData);
                    } else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetControlValueQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(dataFee);
                    } else {
                        throw 'Command ' + queryCommand._name + ' was not found';
                    }
                    return $deferred.promise;
                });
                spyOn($feesService, 'CalculateFees').and.callFake(function(request, queryManager, activityManager, queries, done) {
                    done(null, feeResults);
                });
            });

            it('should fail with an error message', function() {
                createController();
                spyOn($validationManager, 'Validate').and.returnValue(true);

                $scope.calculate();

                $rootScope.$digest();
                expect($scope.errorMessages[0].message).toBe('does not match credit criteria.');
            });
        });

        describe('when returning the calculated result', function() {
            var calculationResult = null;
            beforeEach(function() {
                spyOn($feesService, 'CalculateFees').and.callFake(function(calculateRequest, $queryManager, $activityManager, $capitecQueries, done) {
                    done(null, feeResults);
                });
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function(queryCommand) {

                    $deferred = $_q.defer();
                    if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetOccupancyTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(occupancyTypeData);
                    } else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetEmploymentTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(employmentTypeData);
                    } else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.CalculateApplicationScenarioNewPurchaseQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(data);
                    } else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetCalculatorFeeQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(data);
                    } else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetControlValueQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(dataFee);
                    } else {
                        throw 'Command ' + queryCommand._name + ' was not found';
                    }
                    return $deferred.promise;
                });
                createController();
                $scope.monthlyServiceFee = 50;
                $scope.calculate();

                $rootScope.$digest();
                calculationResult = $calculatorDataService.getDataValue('calculationResult');
            });

            it("should set the correct calculator name", function() {
                expect(calculationResult.calculator).toEqual('newHome');
            });

            it("should set the correct term ", function() {
                expect(calculationResult.termInMonths).toEqual(data.data.ReturnData.TermInMonths);
            });

            it("should calculate the correct loan amount ", function() {
                expect(calculationResult.loanAmount.toFixed(2).toString()).toEqual(data.data.ReturnData.LoanAmount.toFixed(2).toString());
            });

            it("should calculate the correct LTV ", function() {
                expect(calculationResult.LTV.toFixed(2).toString()).toEqual(data.data.ReturnData.LTV.toFixed(2).toString());
            });

            it("should calculate the correct PTI ", function() {
                expect(calculationResult.PTI.toFixed(2).toString()).toEqual(data.data.ReturnData.PTI.toFixed(2).toString());
            });

            it("should use the correct interestRate key ", function() {
                expect(calculationResult.interestRate.toFixed(2).toString()).toEqual(data.data.ReturnData.InterestRate.toFixed(2).toString());
            });

            it("should calculate the correct instalment ", function() {
                var actual = calculationResult.instalment.toFixed(2).toString();
                var expected = (data.data.ReturnData.Instalment + $scope.monthlyServiceFee).toFixed(2).toString();
                expect(actual).toEqual(expected);
            });
        });

        describe('when the CalculateApplicationScenarioNewPurchaseQuery fails', function() {

            beforeEach(function() {
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function(queryCommand) {

                    $deferred = $_q.defer();
                    if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetOccupancyTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(occupancyTypeData);
                    } else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetEmploymentTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(employmentTypeData);
                    } else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.CalculateApplicationScenarioNewPurchaseQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.reject();
                    } else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetCalculatorFeeQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(data);
                    } else if (queryCommand._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetControlValueQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(dataFee);
                    } else {
                        throw 'Command ' + queryCommand._name + ' was not found';
                    }
                    return $deferred.promise;
                });
            });

            it('should fail with an error message', function() {

                createController();
                spyOn($validationManager, 'Validate').and.returnValue(true);

                $scope.calculate();

                $rootScope.$digest();

                expect($scope.errorMessage).toBe('query failure.');
            });
        });
    });
});
