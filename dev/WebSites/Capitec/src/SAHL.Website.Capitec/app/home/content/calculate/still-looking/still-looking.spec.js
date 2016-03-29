describe('capitecApp', function () {
    beforeEach(module('capitecApp'));

    describe(' - (Still Looking Controller) - ', function () {
        var $activityManager, $calculatorDataService, $config, $capitecQueries, $queryManager, $rootScope, $scope, $state, $data, $_q, promise, data, $timeout, $httpBackend;

        var processData;
        beforeEach(inject(function ($injector, $q) {
            // Get the root scope
            $activityManager = $injector.get('$activityManager');
            $config = $injector.get('$config');
            $capitecQueries = $injector.get('$capitecQueries');
            $queryManager = $injector.get('$queryManager');
            $rootScope = $injector.get('$rootScope');
            $scope = $rootScope.$new();
            $state = $injector.get('$state');
            $calculatorDataService = $injector.get('$calculatorDataService');
            $timeout = $injector.get('$timeout');
            $httpBackend = $injector.get('$httpBackend');
            $_q = $q;

            // Set up the controller under test
            var $controller = $injector.get('$controller');
            createController = function () {
                return $controller('CalculateStillLookingCtrl', {
                    '$scope': $scope,
                    '$state': $state,
                    '$queryManager': $queryManager,
                    '$capitecQueries': $capitecQueries,
                    '$config': $config,
                    '$activityManager': $activityManager,
                    '$calculatorDataService': $calculatorDataService
                });
            };

            $httpBackend.whenGET('./app/autologin/autologin.tpl.html').respond([]);
            $httpBackend.whenGET('./app/home/home.tpl.html').respond([]);
            $httpBackend.whenGET('./app/home/splashscreen/splashscreen.tpl.html').respond([]);
            $httpBackend.whenGET('./app/home/content/content.tpl.html').respond([]);
            $httpBackend.whenGET('./app/home/content/apply/shared/application-precheck/application-precheck.tpl.html').respond([]);
            $httpBackend.whenGET('./app/home/content/apply/apply.tpl.html').respond([]);
            $httpBackend.whenGET('./app/home/content/apply/for-switch/switch.tpl.html').respond([]);
            $httpBackend.whenGET('./app/home/content/calculate/calculation-result/calculation-result.tpl.html').respond([]);
            $httpBackend.whenGET('./app/home/content/calculate/calculation-result/calculation-result-fail.tpl.html').respond([]);
            
            // Set up some variables
            purchasePrice = 1500000;
            term = 20;
            termInMonths = term * 12;
            monthlyRate = ($scope.interestRate) / 12;
            amountQualifiedFor = 1000000;
            propertyPriceQualifiedFor = (amountQualifiedFor + ($scope.cashDeposit ? parseFloat($scope.cashDeposit) : 0));
            monthlyServiceFee = 50;
            instalment = (monthlyRate / (1 - (Math.pow((1 + monthlyRate), -(term))))) * propertyPriceQualifiedFor;
            instalmentAndFee = instalment + $scope.monthlyServiceFee;
            
            data = {
                data: {
                    ReturnData: new Array(),
                    SystemMessages: {
                        AllMessages: {
                            $values: new Array()
                        },
                        HasWarnings: false
                    }
                }
            };

            data.data.ReturnData = {
                InterestRate: 0.102,
                Term: term,
                Instalment: instalment,
                AmountQualifiedFor: amountQualifiedFor,
                PropertyPriceQualifiedFor: propertyPriceQualifiedFor,
                TermInMonths: termInMonths
            };
            feeData = {
                data: {
                    ReturnData: {
                        Results: {
                            $values: [
                                {ControlNumeric : 50}
                            ]
                        }
                    }
                }
            };
        }));

        describe('when initialised', function() {
            beforeEach(function() {
                spyOn($capitecQueries, 'GetAffordabilityInterestRateQuery').and.callThrough();
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function(query) {
                    var deferred = $_q.defer();
                    if(query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetAffordabilityInterestRateQuery,SAHL.Services.Interfaces.Capitec') {
                        deferred.resolve(data);
                    } else if (query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetControlValueQuery,SAHL.Services.Interfaces.Capitec') {
                        deferred.resolve(feeData);
                    }
                    return deferred.promise;
                });
                var controller = createController();
                $scope.$digest();
            });
            it('should get an interestRate query', function() {
                expect($capitecQueries.GetAffordabilityInterestRateQuery).toHaveBeenCalledWith(0, 0, 0, true);
            });
            it('should set the initial interestRate', function() {
                expect($scope.calcRate).toBe('10.20');
            });
            it('should set the monthly service fee', function() {
                expect($scope.monthlyServiceFee).toBe(50);
            });
        });

        describe('when the interest rate query fails', function() {
            beforeEach(function(done) {
                spyOn($capitecQueries, 'GetAffordabilityInterestRateQuery').and.callThrough();
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function(query) {
                    var deferred = $_q.defer();
                    if(query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetAffordabilityInterestRateQuery,SAHL.Services.Interfaces.Capitec') {
                        deferred.reject({});
                    } else if (query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetControlValueQuery,SAHL.Services.Interfaces.Capitec') {
                        deferred.resolve(feeData);
                    }
                    return deferred.promise;
                });
                var controller = createController();
                $scope.$digest();
                $scope.householdIncome = 15000;
                
                $scope.incomeChanged();
                
                setTimeout(function () { done();},750);
            });
            it('should set the query failure scope message', function() {
                expect($scope.errorMessage).toBe('query failure');
            });
        });

        describe('when the service fee query fails', function() {
            beforeEach(function(done) {
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function(query) {
                    var deferred = $_q.defer();
                    if(query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetAffordabilityInterestRateQuery,SAHL.Services.Interfaces.Capitec') {
                        deferred.resolve(data);
                    } else if (query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetControlValueQuery,SAHL.Services.Interfaces.Capitec') {
                        deferred.reject("Something went wrong");
                    }
                    return deferred.promise;
                });
                var controller = createController();
                $scope.$digest();
                $scope.householdIncome = 15000;
                
                $scope.incomeChanged();
                
                setTimeout(function () { done(); },750);
            });
            it('should set the query failure scope message', function() {
                expect($scope.errorMessage).toBe('Something went wrong');
            });
        });

        describe('when the income is changed', function() {
            beforeEach(function(done) {
                spyOn($capitecQueries, 'GetAffordabilityInterestRateQuery').and.callThrough();
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function(query) {
                    var deferred = $_q.defer();
                    if(query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetAffordabilityInterestRateQuery,SAHL.Services.Interfaces.Capitec') {
                        if(query.householdIncome === 15000) { data.data.ReturnData.InterestRate = 0.097;}
                        deferred.resolve(data);
                    } else if (query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetControlValueQuery,SAHL.Services.Interfaces.Capitec') {
                        deferred.resolve(feeData);
                    }
                    return deferred.promise;
                });
                var controller = createController();
                $scope.$digest();
                $scope.householdIncome = 15000;
                
                $scope.incomeChanged();
                setTimeout(function () { done(); },750);
            });
 
            it('should get an interestRate query', function() {
                expect($capitecQueries.GetAffordabilityInterestRateQuery).toHaveBeenCalledWith(15000, 0, 0, true);
            });
            it('should update the interestRate', function() {
                expect($scope.calcRate).toBe('10.20');
            });
        });

        describe('when calculating', function () {
            var processData;
             beforeEach(function(done) {
                spyOn($capitecQueries, 'GetAffordabilityInterestRateQuery').and.callThrough();
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function(query) {
                    var deferred = $_q.defer();
                    if(query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetAffordabilityInterestRateQuery,SAHL.Services.Interfaces.Capitec') {
                        deferred.resolve(data);
                    } else if (query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetControlValueQuery,SAHL.Services.Interfaces.Capitec') {
                        deferred.resolve(feeData);
                    }
                    return deferred.promise;
                });
                var controller = createController();
                $scope.$apply();
                $scope.cashDeposit = 0;
                
                $scope.householdIncome = 15000;
                setTimeout(function () { done(); },750);
                
                $scope.calculate();
                $scope.$digest();
                processData = $calculatorDataService.getDataValue('calculationResult');
                
            });

            it('should call the $capitecQueries GetAffordabilityInterestRateQuery() method', function () {
                expect($capitecQueries.GetAffordabilityInterestRateQuery.calls.argsFor(1)).toEqual([15000, 0, 0.102, false]);
            });
            it('should call the sendQueryAsync method', function() {
                expect($queryManager.sendQueryAsync).toHaveBeenCalled();
            });
            it("should set the correct calculator name", function () {
                expect(processData.calculator).toEqual('stilllooking');
            });
            it("should set the correct term in months", function () {
                expect(processData.termInMonths).toEqual(termInMonths);
            });
            it("should calculate the correct loan amount ", function () {
                expect(processData.propertyPriceQualifiedFor).toEqual(propertyPriceQualifiedFor.toString());
            });
            it("should use the correct interestRate ", function () {
                expect(processData.interestRate).toEqual('10.20');
            });
            it("should calculate the correct instalment ", function () {
                expect(processData.instalment).toEqual(instalmentAndFee.toFixed(0));
            });
        });

        describe('when calculating returns a warning', function () {
             beforeEach(function(done) {
                data.data.SystemMessages.HasWarnings = true;
                spyOn($capitecQueries, 'GetAffordabilityInterestRateQuery').and.callThrough();
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function(query) {
                    var deferred = $_q.defer();
                    if(query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetAffordabilityInterestRateQuery,SAHL.Services.Interfaces.Capitec') {
                        
                        deferred.resolve(data);
                    } else if (query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetControlValueQuery,SAHL.Services.Interfaces.Capitec') {
                        deferred.resolve(feeData);
                    }
                    return deferred.promise;
                });
                spyOn($state, 'transitionTo').and.callThrough();
                var controller = createController();
                $scope.$apply();
                $scope.cashDeposit = 0;
                
                $scope.householdIncome = 15000;
                
                setTimeout(function () { done(); },750);
                
                $scope.calculate();
                $scope.$digest();
                processData = $calculatorDataService.getDataValue('calculationResult');
            });

            it('should call the $capitecQueries GetAffordabilityInterestRateQuery() method', function () {
                expect($capitecQueries.GetAffordabilityInterestRateQuery.calls.argsFor(1)).toEqual([15000, 0, 0.102, false]);
            });
            it('should not transition to the next state', function() {
                expect($state.transitionTo).not.toHaveBeenCalledWith('home.content.still-looking.calculation-result');
            });
        });

        describe('when checking if the user updated the rate', function() {
            beforeEach(function() {
                spyOn($capitecQueries, 'GetAffordabilityInterestRateQuery').and.callThrough();
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function(query) {
                    var deferred = $_q.defer();
                    if(query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetAffordabilityInterestRateQuery,SAHL.Services.Interfaces.Capitec') {
                        if(query.householdIncome === 15000) { data.data.ReturnData.InterestRate = 0.097;}
                        deferred.resolve(data);
                    } else if (query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetControlValueQuery,SAHL.Services.Interfaces.Capitec') {
                        deferred.resolve(feeData);
                    }
                    return deferred.promise;
                });
                var controller = createController();
                $scope.affordabilityForm = {
                    calcRate: {value: 15.0}
                };
                $scope.assignedInterestRate = 2.0;
                $scope.$digest();
                $scope.checkIfUserUpdatedRate();
                $timeout.flush();
            });
            it('should set userAdjustedRate to true if the rate has been changed', function() {
                expect($scope.userAdjustedInterestRate).toBe(true);
            });
        });
    });
});