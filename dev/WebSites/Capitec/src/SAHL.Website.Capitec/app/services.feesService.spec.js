describe("capitecApp", function () {
    beforeEach(module('capitecApp.services'));
    beforeEach(module('SAHL.Services.Interfaces.Capitec.queries'));

    describe(' Capitec Fees Service ', function () {
        
        var $feesService, $_q, $queryManager, $activityManager,  $rootScope, $capitecQueries, expectedCalculationRequest, doneCalled, 
        doneCallBack, deferred, actualCalculationResult, doneMessage;

        beforeEach(inject(function ($injector, $q) {  
            $feesService = $injector.get('$feesService');
            $queryManager = $injector.get('$queryManager');
            $capitecQueries = $injector.get('$capitecQueries');
            $activityManager = $injector.get('$activityManager');
            $rootScope = $injector.get('$rootScope');
            deferred = $q.defer();

            expectedCalculationRequest = {
                        calculator: 'newHome',
                        interestRate: 9.10,
                        loanAmount: 900000.00,
                        cashOut: 100000
            };
          
            doneCallBack = function(message,results) { 
                doneCalled = true; 
                doneMessage = message;
                actualCalculationResult = results;
            };
            spyOn($capitecQueries, 'GetCalculatorFeeQuery').and.callFake(function() {});
        }));

        describe('when calculation request is invalid', function() {
            afterEach(function() {
                doneCalled = false;
                doneMessage = undefined;
            });

            it('should ensure that the service calls done with a message and returns', function() {  

                expectedCalculationRequest = undefined;              
                $feesService.CalculateFees(expectedCalculationRequest, $queryManager, $activityManager, $capitecQueries, doneCallBack);
                expect(doneCalled).toBe(true);
                expect(doneMessage).toBe('Please provide valid information for the application');
                expect($capitecQueries.GetCalculatorFeeQuery).not.toHaveBeenCalled();
            });
        });

        describe('when calling the GetCalculatorFeeQuery', function() {

            beforeEach(function(){
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function() {
                    var inputData = {
                        data: 
                        {
                            ReturnData: 
                            {
                                Results:{
                                    $values : new Array({
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
                    deferred.resolve(inputData);
                    return deferred.promise; 
                });
            });

            afterEach(function() {
                doneCalled = false;
                doneMessage = undefined;
            });

            it('should default to an offer type of New Purchase', function() {
                expectedCalculationRequest.calculator = 'new home';
                $feesService.CalculateFees(expectedCalculationRequest, $queryManager, $activityManager, $capitecQueries, doneCallBack);
                $rootScope.$digest();
                expect($capitecQueries.GetCalculatorFeeQuery).toHaveBeenCalledWith(7, expectedCalculationRequest.loanAmount, expectedCalculationRequest.cashOut);
            });

            it('should set offer type of Switch if using the Switch calculator', function() {
                expectedCalculationRequest.calculator = 'switch';
                $feesService.CalculateFees(expectedCalculationRequest, $queryManager, $activityManager, $capitecQueries, doneCallBack);
                $rootScope.$digest();
                expect($capitecQueries.GetCalculatorFeeQuery).toHaveBeenCalledWith(6, expectedCalculationRequest.loanAmount, expectedCalculationRequest.cashOut);
            });

            it('should call done with calculation results', function(){
                $feesService.CalculateFees(expectedCalculationRequest, $queryManager, $activityManager, $capitecQueries, doneCallBack);
                $rootScope.$digest();
                expect(doneCalled).toBe(true);
                expect(doneMessage).toBe(null);
                expect($capitecQueries.GetCalculatorFeeQuery).toHaveBeenCalled();
                expect(actualCalculationResult).not.toBe(undefined);
            });
        });
        
        describe('when there is no fee configuration for a given loan amount', function() {
            beforeEach(function() {
                doneCalled = false;
                doneMessage = undefined;
            });

            it('should ensure that the service calls done with a message', function() {
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function(queryCommand) { 
                    var inputData = {
                        data: 
                        {
                            ReturnData: 
                            {
                                Results:{
                                    $values : new Array()
                                }
                            }
                        }
                    };
                    deferred.resolve(inputData);
                    return deferred.promise; 
                });
                $feesService.CalculateFees(expectedCalculationRequest, $queryManager, $activityManager, $capitecQueries, doneCallBack);
                $rootScope.$digest();
                expect(doneCalled).toBeTruthy();
            });
        });

        describe('when sendQueryAsync executes GetCalculatorFeeQuery and throws an error', function() {

            beforeEach(function(){
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function() {
                    deferred.reject();
                    return deferred.promise; 
                });
            });

            afterEach(function() {
                doneCalled = false;
                doneMessage = undefined;
            });

            it('should return an error message', function() {
                $feesService.CalculateFees(expectedCalculationRequest, $queryManager, $activityManager, $capitecQueries, doneCallBack);
                $rootScope.$digest();
                expect(doneCalled).toBe(true);
                expect(doneMessage).toBe('The fees could not be calculated. Please try again later.');
                expect(actualCalculationResult).toBeFalsy();
            });
        });
    });
});