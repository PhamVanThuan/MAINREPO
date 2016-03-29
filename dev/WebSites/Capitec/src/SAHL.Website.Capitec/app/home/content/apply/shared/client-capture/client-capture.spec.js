describe('capitecApp', function () {
    beforeEach(module('capitecApp'));
    beforeEach(module('templates'));

    describe(' Client Capture Controller - ', function () {
        var $rootScope, $scope, $state, $commandManager, $capitecCommands, $queryManager,
            $capitecQueries, clientCaptureContrl, $notificationService, $capitecSharedModels, $validationManager,
            $calculatorDataService, $validation, $activityManager;

        beforeEach(inject(function ($injector, $q) {
            $rootScope = $injector.get('$rootScope');
            $scope = $rootScope.$new();
            $state = $injector.get('$state');
            $commandManager = $injector.get('$commandManager');
            $capitecCommands = $injector.get('$capitecCommands');
            $queryManager = $injector.get('$queryManager');
            $capitecQueries = $injector.get('$capitecQueries');
            $notificationService = $injector.get('$notificationService');
            $capitecSharedModels = $injector.get('$capitecSharedModels');
            $validationManager = $injector.get('$validationManager');
            $calculatorDataService = $injector.get('$calculatorDataService');
            $validation = $injector.get('$validation');
            $activityManager = $injector.get('$activityManager');
            $httpBackend = $injector.get('$httpBackend');
            $deferred = $q.defer();
            $rootScope.authenticated = true;

            var $controller = $injector.get('$controller');
            createController = function () {
                return $controller('clientCaptureCtrl', {
                    '$rootScope': $rootScope,
                    '$scope': $scope,
                    '$state': $state,
                    '$commandManager': $commandManager,
                    '$capitecCommands': $capitecCommands,
                    '$queryManager': $queryManager,
                    '$capitecQueries': $capitecQueries,
                    '$notificationService': $notificationService,
                    '$capitecSharedModels': $capitecSharedModels,
                    '$validationManager': $validationManager,
                    '$calculatorDataService': $calculatorDataService,
                    '$validation': $validation,
                    '$activityManager': $activityManager
                });
            };
        }));

        describe('when initialising the client capture controller', function() {
            beforeEach(function() {
                $state.current.data = { next: 'home.content.apply.application-precheck-for-new-home.new-home.client-capture' };
                var mockedApp = { applicants: new Array() };
                $scope.application = mockedApp;
                $calculatorDataService.addData('application', mockedApp);
                $calculatorDataService.addData('NumberOfApplicants', 1);
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function () {
                    $deferred.resolve({data:{ReturnData: {Results:{}}}});
                    return $deferred.promise;
                });
            });
            it('should set the current applicant to the first applicant if none are set', function() {
                createController();
                $scope.$digest();
                expect($scope.currentApplicant).toBe(0);
            });
            it('should set the current applicant to the first applicant if the applicant set is greater than the number of applicanst', function() {
                $calculatorDataService.addData('currentApplicant', 3);
                createController();
                $scope.$digest();
                expect($scope.currentApplicant).toBe(0);
            })
            it('should set the current applicant to the second applicant if the second applicant is selected', function() {
                $calculatorDataService.addData('currentApplicant', 1);
                $calculatorDataService.addData('NumberOfApplicants', 2);
                createController();
                $scope.$digest();
                expect($scope.currentApplicant).toBe(1);
            });
            it('should set the main contact to the first applicant', function() {
                createController();
                $scope.$digest();
                expect($scope.application.applicants[0].information.mainContact).toBe(true);
            });
            it('should not change the main contact if there is one set', function() {
                var mockedApp ={applicants: [ 
                    {
                        information: {
                            mainContact: false
                        }, 
                        declarations: {
                        },
                        residentialAddress: {
                        }
                    }, {
                        information: {
                            mainContact: true
                        },
                        declarations: {
                        },
                        residentialAddress: {
                        }
                    }]
                };
                $calculatorDataService.addData('application', mockedApp);
                createController();
                $scope.$digest();
                expect($scope.application.applicants[0].information.mainContact).toBe(false);
                expect($scope.application.applicants[1].information.mainContact).toBe(true);
            });
        });

        describe('when changing the current applicant', function() {
            beforeEach(function() {
                $state.current.data = { next: 'home.content.apply.application-precheck-for-new-home.new-home.client-capture' };
                var mockedApp = { applicants: new Array() };
                $scope.application = mockedApp;
                $calculatorDataService.addData('application', mockedApp);
                $calculatorDataService.addData('NumberOfApplicants', 2);
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function () {
                    $deferred.resolve({data:{ReturnData: {Results:{}}}});
                    return $deferred.promise;
                });

                createController();
                $scope.$digest();
                spyOn($calculatorDataService, 'addData').and.callThrough();
            });
            it('should update the calculator data service with the new current applicant', function() {
                $scope.selectApplicant(1);
                $scope.$digest();
                expect($calculatorDataService.addData).toHaveBeenCalledWith('currentApplicant', 1);
            });
        });

        describe('when submitting an application and checking the income', function () {
            beforeEach(function () {
                $state.current.data = { next: 'home.content.apply.application-precheck-for-new-home.new-home.application-submit' };
                var mockedApp = { applicants: new Array() };
                $scope.application = mockedApp;
                $calculatorDataService.addData('application', mockedApp);
                $calculatorDataService.addData('NumberOfApplicants', 1);
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function () {
                    $deferred.resolve({data:{ReturnData: {Results:{}}}});
                    return $deferred.promise;
                });
            });

            it('should request spousal information if the applicant is married in C.O.P', function () {
                spyOn($notificationService, 'notifyInfo').and.callFake(function () { });
                createController();
                $scope.$digest();

                $scope.getCurrentApplicant().declarations.marriedInCommunityOfProperty = '1234567';
                $scope.declarationAnswersLookup = new Array({
                    Name: 'Yes',
                    Id: '1234567',
                });
                $scope.residentialAddress = new Array({})
                spyOn($scope, 'validate').and.callFake().and.returnValue(true);
                $scope.isKnownDatabaseSuburb = true;
                $scope.submitApplication();
                expect($notificationService.notifyInfo).toHaveBeenCalledWith('Please capture spouse details for applicants that are Married in Community of Property.');
            });

            it('should request for more information if the applicant is not an income contributor', function () {
                spyOn($notificationService, 'notifyInfo').and.callFake(function () { });
                createController();
                $scope.$digest();

                $scope.getCurrentApplicant().declarations.incomeContributor = '1234567';
                $scope.declarationAnswersLookup = new Array({
                    Name: 'No',
                    Id: '1234567',
                });
                spyOn($scope, 'validate').and.callFake().and.returnValue(true);
                $scope.isKnownDatabaseSuburb = true;
                $scope.submitApplication();
                expect($notificationService.notifyInfo).toHaveBeenCalledWith('Please capture details for an applicant that is an income contributor.');
            });

            it('should have a minimum of one income contributor on the application', function () {
                spyOn($notificationService, 'notifyInfo').and.callFake(function () { });
                createController();
                $scope.$digest();

                $scope.getCurrentApplicant().declarations.incomeContributor = '1234567';
                $scope.declarationAnswersLookup = new Array({
                    Name: 'No',
                    Id: '1234567',
                });

                spyOn($scope, 'validate').and.callFake().and.returnValue(true);
                $scope.isKnownDatabaseSuburb = true;
                $scope.submitApplication();

                expect($notificationService.notifyInfo).toHaveBeenCalledWith('Please capture details for an applicant that is an income contributor.');
            });

            it('should not notify when there is an income contributor on an application', function () {
                spyOn($notificationService, 'notifyInfo').and.callFake(function () { });
                createController();
                $scope.$digest();

                $scope.changeCurrentApplicantEmployment ('Salaried');
                $scope.getCurrentApplicant().declarations.incomeContributor = '1234567';
                $scope.getCurrentApplicant().information.mainContact = true;
                $scope.declarationAnswersLookup = new Array({
                    Name: 'Yes',
                    Id: '1234567',
                });

                spyOn($scope, 'validate').and.callFake().and.returnValue(true);
                $scope.isKnownDatabaseSuburb = true;
                $scope.submitApplication();
                expect($notificationService.notifyInfo).not.toHaveBeenCalled();
                $scope.getCurrentApplicant().information.mainContact = false;
            });

            it('should check that an application has one main contact', function () {
                spyOn($notificationService, 'notifyError').and.callFake(function () { });
                createController();
                $scope.$digest();

                $scope.getCurrentApplicant().information.mainContact = false;

                spyOn($scope, 'validate').and.callFake().and.returnValue(true);
                $scope.isKnownDatabaseSuburb = true;
                $scope.submitApplication();

                expect($notificationService.notifyError).toHaveBeenCalledWith('The application requires that one applicant must be selected as the main contact.');
            });
        });

        describe('when making an applicant a main contact', function () {
            beforeEach(function () {
                var mockedApp = { applicants: new Array() };
                $scope.application = mockedApp;
                $calculatorDataService.addData('application', mockedApp);
                $calculatorDataService.addData('NumberOfApplicants', 2);
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function () {
                    $deferred.resolve({data:{ReturnData:{Results:{$values:[]}}}});
                    return $deferred.promise;
                });
            });

            it('should only allow one main applicant', function () {
                spyOn($notificationService, 'notifyError').and.callFake(function () { });
                spyOn($state, 'transitionTo').and.callFake(function () { });

                clientCaptureContrl = createController();
                $scope.$digest();

                $scope.application.applicants[0].information.mainContact = true;
                $scope.application.applicants[1].information.mainContact = true;


                $scope.checkMainContactSelectedForApplication();
                spyOn($scope, 'changeCurrentApplicantEmployment').and.callFake(function () { });

                expect($notificationService.notifyError).toHaveBeenCalledWith('Only one applicant can be selected as the main contact on this application.')
            });
        });

        describe('when validating the id number of an applicant', function () {
            beforeEach(function () {
                var mockedApp = { applicants: new Array() };
                $scope.application = mockedApp;
                $calculatorDataService.addData('application', mockedApp);
                $calculatorDataService.addData('NumberOfApplicants', 1);
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function () {
                    var data = {
                        data: {
                            ReturnData: {
                                Results: {
                                    $values: new Array({
                                        ApplicationNumber: 123
                                    })
                                }
                            }
                        }
                    };
                    $deferred.resolve(data);
                    return $deferred.promise;
                });
            });

            it('should check if the id number is valid and return false if not valid', function () {
                spyOn($notificationService, 'notifySticky').and.callFake(function () { });
                createController();
                $scope.$digest();

                $scope.application.applicants[0].information.identityNumber = "8401160057089";
                $scope.checkExistingApplicationsForIdentityNumber();
                $scope.$digest();
                expect($notificationService.notifySticky).toHaveBeenCalled();
                expect($scope.application.applicants[$scope.currentApplicant].errorValidationMessages).toContain('Please provide a valid ID Number.')
            });
            it('should not validate an ID number that is not 13 digits long', function(){
                spyOn($notificationService, 'notifySticky').and.callFake(function () { });
                createController();
                $scope.$digest();

                $scope.application.applicants[0].information.identityNumber = "840116";
                $scope.checkExistingApplicationsForIdentityNumber();
                $scope.$digest();
                expect($notificationService.notifySticky).not.toHaveBeenCalled();
                expect($scope.application.applicants[$scope.currentApplicant].errorValidationMessages).not.toContain('Please provide a valid ID Number.')
            });
            it('should check if the id number is valid and return true if it exists', function () {
                spyOn($notificationService, 'notifySticky').and.callFake(function () { });
                spyOn($scope, '$watch').and.callFake(function () { });
                createController();
                $scope.$digest();

                $scope.application.applicants[0].information.identityNumber = '8606185144082';
                $scope.checkExistingApplicationsForIdentityNumber();
                $scope.$digest();
                expect($notificationService.notifySticky).toHaveBeenCalled();
                expect($scope.application.applicants[$scope.currentApplicant].errorValidationMessages)
                .toContain('Application 123 exists for applicant with identity number 8606185144082.')
            });
        });

        describe('when validating a valid id number of an applicant', function () {
            beforeEach(function () {
                var mockedApp = { applicants: new Array() };
                $scope.application = mockedApp;
                $calculatorDataService.addData('application', mockedApp);
                $calculatorDataService.addData('NumberOfApplicants', 1);
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function () {
                    var data = {
                        data: {
                            ReturnData: {
                                Results: {
                                    $values: new Array(

                                    )
                                }
                            }
                        }
                    };
                    $deferred.resolve(data);
                    return $deferred.promise;
                });
            });

            it('should check if the id number exists and return false if it doesnt exist', function () {
                spyOn($notificationService, 'notifySticky').and.callFake(function () { });
                spyOn($notificationService, 'removeAll').and.callFake(function () { });
                createController();
                $scope.$digest();

                $scope.application.applicants[0].information.identityNumber = "8606185144082";
                $scope.checkExistingApplicationsForIdentityNumber();
                $scope.$digest();
                expect($notificationService.notifySticky).not.toHaveBeenCalled();
                expect($notificationService.removeAll).toHaveBeenCalled();
                expect($scope.application.applicants[$scope.currentApplicant].errorValidationMessages).not
                .toContain('Application 123 exists for applicant with identity number 8606185144082.')
            });
        });

        describe('when submitting and making a loan purpose decision', function () {
            var data;
            beforeEach(function () {
                var mockedApp = { applicants: new Array() };
                $scope.application = mockedApp;
                $calculatorDataService.addData('application', mockedApp);
                $calculatorDataService.addData('NumberOfApplicants', 1);

                data = {
                    data: {
                        ReturnData: {
                            Results: {
                                $values: new Array({ Id: 'D6485D64-DD43-4655-BDA4-A2F800CFCB1B', Name: 'Mr' })
                            }
                        }
                    }
                };
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function (query) {
                    if (query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetSalutationsQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(data);
                    }
                    else if (query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetEmploymentTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve({});
                    }
                    else if (query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetDeclarationTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve({});
                    }
                    else {
                        throw 'Unhandled Query'
                    }
                    return $deferred.promise;
                });
            });
            it('should transition to the expected state for switch applications', function () {
                var expectedState = 'home.content.apply.application-precheck-for-switch.switch.calculation-submit';
                $state.current.name = 'for-new-home';
                spyOn($commandManager, 'sendCommandAsync').and.callFake(function () {
                    $deferred.resolve(null);
                    return $deferred.promise;
                });
                createController();
                $scope.$digest();

                spyOn($scope, 'getLookupName').and.callFake().and.returnValue('Mr');
                spyOn($scope, 'validate').and.callFake().and.returnValue(true);
                spyOn($state, 'transitionTo').and.callFake(function () { });
                $state.current.data = { next: expectedState };

                $scope.changeCurrentApplicantEmployment ('Salaried');
                var currentApplicant = $scope.getCurrentApplicant();
                currentApplicant.declarations.incomeContributor = '1234567';
                $scope.declarationAnswersLookup = new Array({ Name: 'Yes', Id: '1234567' });
                currentApplicant.information.mainContact = true;
                $scope.isKnownDatabaseSuburb = true;
                $scope.submitApplication();

                expect($state.transitionTo).toHaveBeenCalledWith(expectedState, {});
            });

            it('should transition to the expected state for new home applications', function () {
                var expectedState = 'home.content.apply.application-precheck-for-new-home.new-home.calculation-submit';
                $state.current.name = 'for-switch';
                spyOn($commandManager, 'sendCommandAsync').and.callFake(function () {
                    $deferred.resolve(null);
                    return $deferred.promise;
                });
                createController();
                $scope.$digest();

                spyOn($scope, 'getLookupName').and.callFake().and.returnValue('Mr');
                spyOn($scope, 'validate').and.callFake().and.returnValue(true);
                spyOn($state, 'transitionTo').and.callFake(function () { });
                $state.current.data = { next: expectedState };

                $scope.changeCurrentApplicantEmployment ('Salaried');
                var currentApplicant = $scope.getCurrentApplicant();
                currentApplicant.declarations.incomeContributor = '1234567';
                $scope.declarationAnswersLookup = new Array({ Name: 'Yes', Id: '1234567' });
                currentApplicant.information.mainContact = true;
                $scope.isKnownDatabaseSuburb = true;
                $scope.submitApplication();

                expect($state.transitionTo).toHaveBeenCalledWith(expectedState, {});
            });
        });

        describe('when submitting an application and giving permission for performing an ITC check', function () {
            beforeEach(function () {
                var mockedApp = { applicants: new Array() };
                $scope.application = mockedApp;

                $calculatorDataService.addData('NumberOfApplicants', 1);
                $calculatorDataService.addData('application', mockedApp);
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function () {
                    $deferred.resolve({data:{ReturnData:{Results:{$values:[]}}}});
                    return $deferred.promise;
                });
            });

            it('should require permission prior to performing an ITC', function () {
                spyOn($notificationService, 'notifyError').and.callFake(function () { });
                createController();
                $scope.$digest();
                spyOn($scope, 'validate').and.callFake().and.returnValue(true);

                $scope.getCurrentApplicant().declarations.allowCreditBureauCheck = '92032903';
                $scope.declarationAnswersLookup = new Array({
                    Name: 'Yes',
                    Id: '1234567',
                },
                {
                    Name: 'No',
                    Id: '92032903',
                });
                $scope.submitApplication();

                expect($notificationService.notifyError).toHaveBeenCalledWith('If an applicant is present at the time of capture, SA Home Loans requires their permission prior to performing a Credit Bureau Check.');
            });
        });

        describe('when submitting an application and setting the employment details', function () {
            beforeEach(function () {
                var mockedApp = { applicants: new Array() };
                $state.current.data = { next: 'home.content.apply.application-precheck-for-new-home.new-home.application-submit' };
                $scope.application = mockedApp;
                $calculatorDataService.addData('application', mockedApp);
                $calculatorDataService.addData('NumberOfApplicants', 1);
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function () {
                    $deferred.resolve({data:{ReturnData:{Results:{$values:[]}}}});
                    return $deferred.promise;
                });
                createController();
                $scope.$digest();
                spyOn($scope, 'validate').and.callFake().and.returnValue(true);
            });

            it('should set employment to salaried', function () {
                var currentApplicant = $scope.getCurrentApplicant();
                currentApplicant.declarations.incomeContributor = '1234567';
                $scope.declarationAnswersLookup = new Array({
                    Name: 'Yes',
                    Id: '1234567',
                });

                currentApplicant.information.mainContact = true;
                $scope.changeCurrentApplicantEmployment('Salaried');
                currentApplicant.employmentDetails.incomeDetail.grossMonthlyIncome = 36000;

                var expectedGrossIncome = currentApplicant.employmentDetails.incomeDetail.grossMonthlyIncome;
                var expectedTypeName = currentApplicant.employmentDetails.incomeDetail._name;
                $scope.isKnownDatabaseSuburb = true;
                $scope.submitApplication();

                var actualGrossIncome = currentApplicant.employmentDetails.salariedDetails.grossMonthlyIncome;
                var actualTypeName = currentApplicant.employmentDetails.salariedDetails._name;

                expect(expectedGrossIncome).toEqual(actualGrossIncome);
                expect(expectedTypeName).toEqual(actualTypeName);
            });

            it('should set employment to self employed', function () {
                var currentApplicant = $scope.getCurrentApplicant();
                currentApplicant.declarations.incomeContributor = '1234567';
                $scope.declarationAnswersLookup = new Array({
                    Name: 'Yes',
                    Id: '1234567',
                });

                currentApplicant.information.mainContact = true;
                $scope.changeCurrentApplicantEmployment('Self Employed');
                currentApplicant.employmentDetails.incomeDetail.grossMonthlyIncome = 36000;

                var expectedGrossIncome = currentApplicant.employmentDetails.incomeDetail.grossMonthlyIncome;
                var expectedTypeName = currentApplicant.employmentDetails.incomeDetail._name;
                $scope.isKnownDatabaseSuburb = true;
                $scope.submitApplication();

                var actualGrossIncome = currentApplicant.employmentDetails.selfEmployedDetails.grossMonthlyIncome;
                var actualTypeName = currentApplicant.employmentDetails.selfEmployedDetails._name;

                expect(expectedGrossIncome).toEqual(actualGrossIncome);
                expect(expectedTypeName).toEqual(actualTypeName);
            });

            it('should set employment to salaried with housing allowance', function () {
                var currentApplicant = $scope.getCurrentApplicant();
                currentApplicant.declarations.incomeContributor = '1234567';
                $scope.declarationAnswersLookup = new Array({
                    Name: 'Yes',
                    Id: '1234567',
                });

                currentApplicant.information.mainContact = true;
                $scope.changeCurrentApplicantEmployment('Salaried with Housing Allowance');
                currentApplicant.employmentDetails.incomeDetail.housingAllowance = '2000';
                currentApplicant.employmentDetails.incomeDetail.grossMonthlyIncome = 36000;

                var expectedGrossIncome = currentApplicant.employmentDetails.incomeDetail.grossMonthlyIncome;
                var expectedHousingAllowance = currentApplicant.employmentDetails.incomeDetail.housingAllowance;
                var expectedTypeName = currentApplicant.employmentDetails.incomeDetail._name;
                $scope.isKnownDatabaseSuburb = true;
                $scope.submitApplication();

                var actualGrossIncome = currentApplicant.employmentDetails.salariedWithHousingAllowanceDetails.grossMonthlyIncome;
                var actualHousingAllowance = currentApplicant.employmentDetails.salariedWithHousingAllowanceDetails.housingAllowance;
                var actualTypeName = currentApplicant.employmentDetails.salariedWithHousingAllowanceDetails._name;

                expect(expectedGrossIncome).toEqual(actualGrossIncome);
                expect(expectedHousingAllowance).toEqual(actualHousingAllowance);
                expect(expectedTypeName).toEqual(actualTypeName);
            });

            it('should set employment to salaried with commission', function () {
                var currentApplicant = $scope.getCurrentApplicant();
                currentApplicant.declarations.incomeContributor = '1234567';
                $scope.declarationAnswersLookup = new Array({
                    Name: 'Yes',
                    Id: '1234567',
                });

                currentApplicant.information.mainContact = true;
                $scope.changeCurrentApplicantEmployment('Salaried with Commission');
                currentApplicant.employmentDetails.incomeDetail.threeMonthAverageCommission = '5000';
                currentApplicant.employmentDetails.incomeDetail.grossMonthlyIncome = 36000;

                var expectedGrossIncome = currentApplicant.employmentDetails.incomeDetail.grossMonthlyIncome;
                var expectedThreeMonthAverageCommission = currentApplicant.employmentDetails.incomeDetail.threeMonthAverageCommission;
                var expectedTypeName = currentApplicant.employmentDetails.incomeDetail._name;
                $scope.isKnownDatabaseSuburb = true;
                $scope.submitApplication();

                var actualGrossIncome = currentApplicant.employmentDetails.salariedWithCommissionDetails.grossMonthlyIncome;
                var actualThreeMonthAverageCommission = currentApplicant.employmentDetails.salariedWithCommissionDetails.threeMonthAverageCommission;
                var actualTypeName = currentApplicant.employmentDetails.salariedWithCommissionDetails._name;

                expect(expectedGrossIncome).toEqual(actualGrossIncome);
                expect(expectedThreeMonthAverageCommission).toEqual(actualThreeMonthAverageCommission);
                expect(expectedTypeName).toEqual(actualTypeName);
            });

            it('should set employment to unemployed', function () {
                var currentApplicant = $scope.getCurrentApplicant();
                currentApplicant.declarations.incomeContributor = '1234567';
                $scope.declarationAnswersLookup = new Array({
                    Name: 'Yes',
                    Id: '1234567',
                });

                currentApplicant.information.mainContact = false;
                $scope.changeCurrentApplicantEmployment('Unemployed');
                currentApplicant.employmentDetails.incomeDetail.grossMonthlyIncome = 0;

                var expectedGrossIncome = currentApplicant.employmentDetails.incomeDetail.grossMonthlyIncome;
                var expectedTypeName = currentApplicant.employmentDetails.incomeDetail._name;
                $scope.isKnownDatabaseSuburb = true;
                $scope.submitApplication();

                var actualGrossIncome = currentApplicant.employmentDetails.incomeDetail.grossMonthlyIncome;
                var actualTypeName = currentApplicant.employmentDetails.incomeDetail._name;

                expect(expectedGrossIncome).toEqual(actualGrossIncome);
                expect(expectedTypeName).toEqual(actualTypeName);
            });
        });

        describe('when submitting and performing a lookup on the applicant salutation', function () {
            var data;
            beforeEach(function () {
                var mockedApp = { applicants: new Array() };
                $scope.application = mockedApp;
                $calculatorDataService.addData('NumberOfApplicants', 1);
                $calculatorDataService.addData('application', mockedApp);
                data = {
                    data: {
                        ReturnData: {
                            Results: {
                                $values: new Array({ Id: 'D6485D64-DD43-4655-BDA4-A2F800CFCB1B', Name: 'Mr' })
                            }
                        }
                    }
                };
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function (query) {
                    if (query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetSalutationsQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(data);
                    }
                    else if (query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetEmploymentTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve({});
                    }
                    else if (query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetDeclarationTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve({});
                    }
                    else {
                        throw 'Unhandled Query'
                    }
                    return $deferred.promise;
                });
                createController();
                $scope.$digest();
                spyOn($scope, 'validate').and.callFake().and.returnValue(true);
            });

            it('should return Mr for given salutation id', function () {
                expect($queryManager.sendQueryAsync).toHaveBeenCalled();

                $scope.application.applicants[0].information.salutationEnumId = data.data.ReturnData.Results.$values[0].Id;
                $scope.submitApplication();

                expect($scope.application.applicants[0].information.title).toEqual('Mr');
            });
        });

        describe('when adding an applicant', function() {
            beforeEach(function() {
                var data = {data: {ReturnData: {Results: {$values:[]}}}};
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function (query) {
                    $deferred.resolve(data);
                    return $deferred.promise;
                });
                var application = {
                    applicants: new Array()
                };
                $calculatorDataService.addData('application', application);
                $calculatorDataService.addData('NumberOfApplicants', 1);
                createController();
                $scope.$digest();
            });
            it('should create a new applicant', function() {
                $scope.addApplicant();
                expect($scope.application.applicants.length).toBe(2);
            });

            it('should not add a new applicant if there are already two applicants', function() {
                $scope.addApplicant();
                $scope.addApplicant();
                expect($scope.application.applicants.length).toBe(2);
            });
        })

        describe('when removing a applicant', function () {
            beforeEach(function () {
                spyOn($notificationService, 'notifyInfo').and.callThrough();
                var data = {data: {ReturnData: {Results: {$values:[]}}}};
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function (query) {
                    $deferred.resolve(data);
                    return $deferred.promise;
                });
            });

            it('should remove the applicant', function () {
                var application = {
                    applicants: new Array()
                };

                $calculatorDataService.addData('NumberOfApplicants', 2);
                $calculatorDataService.addData('application', application);
                createController();
                $scope.$digest();

                $scope.onClickRemoveClient();
                expect($scope.application.applicants.length).toEqual(1);
                expect($notificationService.notifyInfo).not.toHaveBeenCalled();
            });

            it('should not remove the applicant if there is only one applicant', function () {
                var application = {
                    applicants: new Array()
                };
                $calculatorDataService.addData('NumberOfApplicants', 1);
                $calculatorDataService.addData('application', application);
                createController();
                $scope.onClickRemoveClient();
                expect($scope.application.applicants.length).toEqual(1);
                expect($notificationService.notifyInfo).toHaveBeenCalledWith('The application requires at least one applicant.');
            });

            it('should decrease the number of applicants', function() {
                var application = {
                    applicants: new Array()
                };

                $calculatorDataService.addData('application', application);
                $calculatorDataService.addData('NumberOfApplicants', 2);
                createController();
                $scope.$digest();
                
                $scope.onClickRemoveClient();

                var numberApplicants = $calculatorDataService.getDataValue('NumberOfApplicants');
                expect(numberApplicants).toEqual(1);
            })
        });

        describe('when changing employment to Salaried', function () {
            beforeEach(function () {
                var mockedApp = { applicants: new Array() };
                $scope.application = mockedApp;
                $calculatorDataService.addData('NumberOfApplicants', 1);
                $calculatorDataService.addData('application', mockedApp);
                data = {
                    data: {
                        ReturnData: {
                            Results: {
                                $values: new Array({ Id: '532CA4D6-A752-4224-B33C-A2F90103D729', Name: 'Salaried', Reference: 1 })
                            }
                        }
                    }
                };

                spyOn($queryManager, 'sendQueryAsync').and.callFake(function (query) {
                    if (query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetEmploymentTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(data);
                    }
                    return $deferred.promise;
                });
                createController();
                $scope.$digest();
            });

            it('should do a lookup on employment type', function () {
                spyOn($scope, 'getLookupName').and.callThrough();
                $scope.getCurrentApplicant().employmentDetails.employmentTypeEnumId = '532CA4D6-A752-4224-B33C-A2F90103D729';
                $scope.onEmploymentTypeChanged();
                expect($queryManager.sendQueryAsync).toHaveBeenCalled();
                expect($scope.getLookupName).toHaveBeenCalledWith(data.data.ReturnData.Results.$values[0].Id, $scope.employmentTypeLookup);
            });
        });

        describe('when changing employment to Self Employed', function () {
            beforeEach(function () {
                var mockedApp = { applicants: new Array() };
                $scope.application = mockedApp;
                $calculatorDataService.addData('NumberOfApplicants', 1);
                $calculatorDataService.addData('application', mockedApp);
                data = {
                    data: {
                        ReturnData: {
                            Results: {
                                $values: new Array({ Id: '6795B5CE-2835-4675-8039-A2D500AB5A73', Name: 'Self Employed', Reference: 2 })
                            }
                        }
                    }
                };

                spyOn($queryManager, 'sendQueryAsync').and.callFake(function (query) {
                    if (query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetEmploymentTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(data);
                    }
                    return $deferred.promise;
                });
                createController();
                $scope.addApplicant();
                $scope.$digest();
            });

            it('should do a lookup on employment type', function () {
                spyOn($scope, 'getLookupName').and.callThrough();
                $scope.getCurrentApplicant().employmentDetails.employmentTypeEnumId = '6795B5CE-2835-4675-8039-A2D500AB5A73';
                $scope.onEmploymentTypeChanged();
                expect($queryManager.sendQueryAsync).toHaveBeenCalled();
                expect($scope.getLookupName).toHaveBeenCalledWith(data.data.ReturnData.Results.$values[0].Id, $scope.employmentTypeLookup);
            });
        });

        describe('when changing employment to Salaried with Housing Allowance', function () {
            beforeEach(function () {
                var mockedApp = { applicants: new Array() };
                $scope.application = mockedApp;
                $calculatorDataService.addData('NumberOfApplicants', 1);
                $calculatorDataService.addData('application', mockedApp);
                data = {
                    data: {
                        ReturnData: {
                            Results: {
                                $values: new Array({ Id: 'DE1590B5-25FD-4334-97CC-A2D500AB5A74', Name: 'Salaried with Housing Allowance', Reference: 3 })
                            }
                        }
                    }
                };

                spyOn($queryManager, 'sendQueryAsync').and.callFake(function (query) {
                    if (query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetEmploymentTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(data);
                    }
                    return $deferred.promise;
                });
                createController();
                $scope.$digest();
            });

            it('should do a lookup on employment type', function () {
                spyOn($scope, 'getLookupName').and.callThrough();
                $scope.getCurrentApplicant().employmentDetails.employmentTypeEnumId = 'DE1590B5-25FD-4334-97CC-A2D500AB5A74';
                $scope.onEmploymentTypeChanged();
                expect($queryManager.sendQueryAsync).toHaveBeenCalled();
                expect($scope.getLookupName).toHaveBeenCalledWith(data.data.ReturnData.Results.$values[0].Id, $scope.employmentTypeLookup);
            });
        });

        describe('when changing employment to Salaried with Commission', function () {
            beforeEach(function () {
                var mockedApp = { applicants: new Array() };
                $scope.application = mockedApp;
                $calculatorDataService.addData('NumberOfApplicants', 1);
                $calculatorDataService.addData('application', mockedApp);
                data = {
                    data: {
                        ReturnData: {
                            Results: {
                                $values: new Array({ Id: '199534AD-B097-48A2-A1A4-A2ED00F7D232', Name: 'Salaried with Commission', Reference: 3 })
                            }
                        }
                    }
                };

                spyOn($queryManager, 'sendQueryAsync').and.callFake(function (query) {
                    if (query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetEmploymentTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(data);
                    }
                    return $deferred.promise;
                });
                createController();
                $scope.$digest();
            });

            it('should do a lookup on employment type', function () {
                spyOn($scope, 'getLookupName').and.callThrough();
                $scope.getCurrentApplicant().employmentDetails.employmentTypeEnumId = '199534AD-B097-48A2-A1A4-A2ED00F7D232';
                $scope.onEmploymentTypeChanged();
                expect($queryManager.sendQueryAsync).toHaveBeenCalled();
                expect($scope.getLookupName).toHaveBeenCalledWith(data.data.ReturnData.Results.$values[0].Id, $scope.employmentTypeLookup);
            });
        });

        describe('when changing employment to Unemployed', function () {
            beforeEach(function () {
                var mockedApp = { applicants: new Array() };
                $scope.application = mockedApp;
                $calculatorDataService.addData('NumberOfApplicants', 1);
                $calculatorDataService.addData('application', mockedApp);
                data = {
                    data: {
                        ReturnData: {
                            Results: {
                                $values: new Array({ Id: '64DC7A68-A8A1-41DE-A0A5-A32B010B64E5', Name: 'Unemployed', Reference: 4 })
                            }
                        }
                    }
                };

                spyOn($queryManager, 'sendQueryAsync').and.callFake(function (query) {
                    if (query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetEmploymentTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        $deferred.resolve(data);
                    }
                    return $deferred.promise;

                });
                createController();
                $scope.$digest();
            });

            it('should do a lookup on employment type'), function () {
                spyOn($scope, 'getLookupName').addCallThrough();
                $scope.getCurrentApplicant().employmentDetails.employmentTypeEnumId = '64DC7A68-A8A1-41DE-A0A5-A32B010B64E5w';
                $scope.onEmploymentTypeChanged();
                expect($queryManager.sendQueryAsync).toHaveBeenCalled();
                expect($scope.getLookupName).toHaveBeenCalledWith(data.data.ReturnData.Results.$values[0].Id, $scope.employmentTypeLookup);
            }
        });

        describe('when client is not present at capture', function () {
            it('should return false', function () {
                var result = false;
                createController();
                var currentApplicant = {
                    declarations:
                        { presentAtCapture: ''}
                };
                spyOn($scope, 'getCurrentApplicant').and.returnValue(currentApplicant);

                result = $scope.isClientPresentAtCapture();
                expect(result).toBeTruthy();
                expect($scope.getCurrentApplicant).toHaveBeenCalled();
            });
        });

        describe('when client is present at capture', function () {
            it('should return true', function () {
                var result = true;
                createController();
                var currentApplicant = {
                    declarations:
                        { presentAtCapture: 'test' }
                };

                $scope.declarationAnswersLookup = 'Yes';
                spyOn($scope, 'getCurrentApplicant').and.returnValue(currentApplicant);
                spyOn($validation, 'compare').and.returnValue(false);

                result = $scope.isClientPresentAtCapture();
                expect($validation.compare).toHaveBeenCalledWith('test', 'Yes', 'Yes');
                expect($scope.getCurrentApplicant).toHaveBeenCalled();
                expect(result).toBeFalsy();
            });
        });

        describe('when user says no to credit bureau check', function () {
            it('should return true', function () {
                var result = false;
                createController();
                var currentApplicant = {
                    declarations:
                        { allowCreditBureauCheck: 'test' }
                };

                $scope.declarationAnswersLookup = 'No';
                spyOn($scope, 'getCurrentApplicant').and.returnValue(currentApplicant);
                spyOn($validation, 'compare').and.returnValue(true);

                result = $scope.requiresItcPermission();
                expect($validation.compare).toHaveBeenCalledWith('test', 'No', 'No');
                expect($scope.getCurrentApplicant).toHaveBeenCalled();
                expect(result).toBeTruthy();
            });
        });

        describe('when user says yes to credit bureau check', function () {
            it('should return false', function () {
                var result = true;
                var data = {data: {ReturnData: {Results: {$values:[]}}}};
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function (query) {
                    $deferred.resolve(data);
                    return $deferred.promise;
                });
                createController();
                $scope.$digest();
                var currentApplicant = {
                    declarations:
                        { allowCreditBureauCheck: 'test' }
                };
                $scope.declarationAnswersLookup = 'Yes';
                spyOn($validation, 'compare').and.returnValue(false);
                spyOn($scope, 'getCurrentApplicant').and.returnValue(currentApplicant);
                result = $scope.requiresItcPermission();
                expect(result).toBeFalsy();
                expect($validation.compare).toHaveBeenCalledWith('test', 'Yes', 'No');
                expect($scope.getCurrentApplicant).toHaveBeenCalled();
            });
        });

        describe('when two applicants are required', function() {
            beforeEach(function(){
                var data = {data: {ReturnData: {Results: {$values:[]}}}};
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function (query) {
                    $deferred.resolve(data);
                    return $deferred.promise;
                });
                $state.current.data = {
                    next : 'home.content.apply.application-precheck-for-new-home.new-home.application-submit'
                } ;
               var mockedApp = { applicants: new Array() };
                $calculatorDataService.addData('application', mockedApp);
                $calculatorDataService.addData('NumberOfApplicants', 2);
                createController();
                spyOn($scope, 'validate').and.returnValue(true);
                $scope.$digest();
                $scope.declarationAnswersLookup =[{Id: 'Yes', Name: 'Yes'}];
            });

            it('should create two applicants', function() {
                expect($scope.application.applicants.length).toBe(2);
            });
            it('should focus on the first applicant', function () {
                expect($scope.currentApplicant).toBe(0);
            });
            it('should set the submit button text to "next applicant" when submitting the first one', function () {
                expect($scope.getSubmitLabel()).toBe('Next Applicant');
            })
            it('should go to the next applicant when submitting on the first one', function() {
                $scope.submitApplication();
                expect($scope.currentApplicant).toBe(1);
            });
            it('should set the submit button text to "Print Consent Form" when submitting the second applicant', function () {
                $scope.submitApplication();
                $scope.submitApplication();
                $scope.$digest();
                expect($scope.getSubmitLabel()).toBe('Print Consent Form');
            })
            it('should print the consent forms when submitting the second applicant', function () {
                spyOn($state, 'transitionTo').and.callThrough();
                $scope.application.applicants.length = 2;
                $scope.application.applicants[0].information.mainContact = true;
                $scope.application.applicants[0].information.identityNumber = '8001075000059';
                $scope.application.applicants[1].information.identityNumber = '8001075000083';
                $scope.application.applicants[0].declarations.incomeContributor = 'Yes';
                $scope.isKnownDatabaseSuburb = true;
                $scope.changeCurrentApplicantEmployment('Salaried');

                $scope.currentApplicant = 1;
                $scope.$digest();
                $scope.changeCurrentApplicantEmployment('Salaried');
                
                $scope.submitApplication();
                expect($state.transitionTo).toHaveBeenCalledWith('home.content.apply.application-precheck-for-new-home.new-home.application-submit', {});
            });
        });
    });
});