describe('capitecApp', function() {
    beforeEach(module('capitecApp'));
    beforeEach(module('templates'));

    describe(' - (Application Submit Controller) - ', function() {

        // set up global vars
        var $rootScope, $scope, $state, $commandManager, $capitecCommands, $queryManager, $capitecQueries, $notificationService,
            $printingService, $calculatorDataService, $activityManager, deferred, $config, $_q;
        var applicationID = '0000-0000-0000-0000';
        var application = {
            applicants: [{
                information: {
                    firstName: 'Jane',
                    surname: 'Doe',
                    identityNumber: '2134567890123'
                },
                declarations: {
                    excludeFromDirectMarketing: 'F54495A4-AAEE-4031-8099-A2D500AB5A75' 
                }
            }, {
                information: {
                    firstName: 'John',
                    surname: 'Doe',
                    identityNumber: '3210987654321'
                },
                declarations: {
                    excludeFromDirectMarketing: '5BE6C2E7-9EC3-44A8-A572-A2D500AB5A76' 
                }
            }]
        };
        var applicationIDResult = {
            data: {
                ReturnData: {
                    NewGuid: applicationID
                }
            }
        };
        var applicationSubmitResult = {
            data: {
                SystemMessages: {
                    HasErrors: false
                }
            }
        };
        var declarationLookupResult = {
            data: {
                ReturnData: {
                    Results: {
                        $values: [
                            {Name:'Yes', Id:'F54495A4-AAEE-4031-8099-A2D500AB5A75'},
                            {Name:'No', Id:'5BE6C2E7-9EC3-44A8-A572-A2D500AB5A76'}
                        ]
                    }
                }
            }
        }
        var templateUrl = "consent-form.tpl.html";

        beforeEach(inject(function($injector, $q) {

            // Setup the root scope
            $rootScope = $injector.get('$rootScope');
            $scope = $rootScope.$new();
            $state = $injector.get('$state');
            $commandManager = $injector.get('$commandManager');
            $capitecCommands = $injector.get('$capitecCommands');
            $queryManager = $injector.get('$queryManager');
            $capitecQueries = $injector.get('$capitecQueries');
            $notificationService = $injector.get('$notificationService');
            $rootScope.authenticated = true;
            $printingService = $injector.get('$printingService');
            $calculatorDataService = $injector.get('$calculatorDataService');
            $activityManager = $injector.get('$activityManager');
            deferred = $q.defer();
            $config = $injector.get('$config');
            $config.PrintConsentForm = true;
            $_q = $q;
            // Set up the controller under test
            var $controller = $injector.get('$controller');
            createController = function() {
                return $controller('applicationSubmitCtrl', {
                    '$rootScope': $rootScope,
                    '$scope': $scope,
                    '$state': $state,
                    '$commandManager': $commandManager,
                    '$capitecCommands': $capitecCommands,
                    '$queryManager': $queryManager,
                    '$capitecQueries': $capitecQueries,
                    '$notificationService': $notificationService,
                    '$printingService': $printingService,
                    '$calculatorDataService': $calculatorDataService,
                    '$activityManager': $activityManager,
                    '$config': $config
                });
            };
        }));

        describe('when initialized', function() {
            beforeEach(function() {
                spyOn($calculatorDataService, 'getDataValue').and.returnValue(application);
                spyOn($printingService, 'printFromTemplateUrl').and.callThrough();
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function(query) {
                    var queryDeferred = $_q.defer();
                    queryDeferred.resolve(declarationLookupResult);
                    return queryDeferred.promise;
                });
                createController();
                $scope.$digest();
            });
            it('should get the application from the calculator data service', function() {
                expect($calculatorDataService.getDataValue).toHaveBeenCalledWith('application');
            });
            it('should set the applicants to the scope', function() {
                expect($scope.applicants.length).toEqual(2);
                expect($scope.applicants[0].firstName).toEqual('Jane');
                expect($scope.applicants[1].firstName).toEqual('John');
            });
            it('should default the hasSigned property to false', function() {
                expect($scope.applicants[0].hasSigned).toEqual(false);
                expect($scope.applicants[1].hasSigned).toEqual(false);
            });
            it('should set the exclude from direct marketing property on all the applicants', function() {
                expect($scope.applicants[0].excludeFromDirectMarketing).toEqual(true);
                expect($scope.applicants[1].excludeFromDirectMarketing).toEqual(false);
            });
            it('should print a consent form for all applicants', function() {
                $scope.$apply();
                expect($printingService.printFromTemplateUrl).toHaveBeenCalledWith(templateUrl, {
                    applicants: $scope.applicants
                });
            });
        });

        describe('when reprinting a consent form for all applicants', function() {
            beforeEach(function() {
                spyOn($calculatorDataService, 'getDataValue').and.returnValue(application);
                spyOn($printingService, 'printFromTemplateUrl').and.callThrough();
                spyOn($capitecQueries, 'GetNewGuidQuery').and.callThrough();
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function(query) {
                    var queryDeferred = $_q.defer();
                    if(query._name==='SAHL.Services.Interfaces.Capitec.Queries.GetNewGuidQuery,SAHL.Services.Interfaces.Capitec') {
                        queryDeferred.resolve(applicationIDResult);
                    } else if(query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetDeclarationTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        queryDeferred.resolve(declarationLookupResult);
                    }
                    return queryDeferred.promise;
                });
                createController();
            });
            it('should call the print service and give it all applicants', function() {
                $scope.printAllApplicants();
                expect($printingService.printFromTemplateUrl).toHaveBeenCalledWith(templateUrl, {
                    applicants: $scope.applicants
                });
            });
        });

        describe('when reprinting a single applicant', function() {
            beforeEach(function() {
                spyOn($calculatorDataService, 'getDataValue').and.returnValue(application);
                spyOn($printingService, 'printFromTemplateUrl').and.callThrough();
                spyOn($capitecQueries, 'GetNewGuidQuery').and.callThrough();
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function(query) {
                    var queryDeferred = $_q.defer();
                    if(query._name==='SAHL.Services.Interfaces.Capitec.Queries.GetNewGuidQuery,SAHL.Services.Interfaces.Capitec') {
                        queryDeferred.resolve(applicationIDResult);
                    } else if(query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetDeclarationTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        queryDeferred.resolve(declarationLookupResult);
                    }
                    return queryDeferred.promise;
                });
                createController();
                $scope.$digest();
            });
            it('should call the print service with only that applicant', function() {
                $scope.printForApplicant($scope.applicants[0]);
                expect($printingService.printFromTemplateUrl).toHaveBeenCalledWith(templateUrl, {
                    applicants: [$scope.applicants[0]]
                });
            });
        });

        describe('when submitting an application for switch', function() {
            var expectedState = 'home.content.apply.application-precheck-for-switch.switch.application-result';
            beforeEach(function() {
                $state.current.name = 'for-switch';
                $state.current.data = {
                    next: 'home.content.apply.application-precheck-for-switch.switch.application-result'
                };
                spyOn($state, 'transitionTo').and.callThrough();
                spyOn($activityManager, 'startActivityWithKey').and.callThrough();
                spyOn($activityManager, 'stopActivityWithKey').and.callThrough();
                spyOn($calculatorDataService, 'getDataValue').and.returnValue(application);
                spyOn($capitecCommands, 'ApplyForSwitchLoanCommand').and.callThrough();
                spyOn($capitecQueries, 'GetNewGuidQuery').and.callThrough();
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function(query) {
                    var queryDeferred = $_q.defer();
                    if(query._name==='SAHL.Services.Interfaces.Capitec.Queries.GetNewGuidQuery,SAHL.Services.Interfaces.Capitec') {
                        queryDeferred.resolve(applicationIDResult);
                    } else if(query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetDeclarationTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        queryDeferred.resolve(declarationLookupResult);
                    }
                    return queryDeferred.promise;
                });
                spyOn($commandManager, 'sendCommandAsync').and.callFake(function() {
                    deferred.resolve(applicationSubmitResult);
                    return deferred.promise;
                });
                createController();
                $scope.SubmitApplication();
                $rootScope.$digest();
            });
            it('should start an activity for submitting the application',function() {
                expect($activityManager.startActivityWithKey).toHaveBeenCalledWith('submittingApplication');
            });
            it('should get an applicationID to use when the application is submitted', function() {
                expect($capitecQueries.GetNewGuidQuery).toHaveBeenCalled();
                expect($queryManager.sendQueryAsync).toHaveBeenCalled();
            });
            it('should create an applyForSwitch command', function() {
                expect($capitecCommands.ApplyForSwitchLoanCommand).toHaveBeenCalled();
            });
            it('should send the apply for switch command', function() {
                expect($commandManager.sendCommandAsync).toHaveBeenCalled();
            });
            it('should stop the activity for submitting an application', function() {
                expect($activityManager.stopActivityWithKey).toHaveBeenCalledWith('submittingApplication');
            });
            it('should transition to the expected state for switch applications', function() {
                expect($state.transitionTo).toHaveBeenCalledWith(expectedState, jasmine.any(Object));
            });
        });

        describe('when submitting an application for a new purchase', function() {
            var expectedState = 'home.content.apply.application-precheck-for-new-home.new-home.application-result';
            beforeEach(function() {
                $state.current.name = 'for-new-home';
                $state.current.data = {
                    next: 'home.content.apply.application-precheck-for-new-home.new-home.application-result'
                };

                spyOn($state, 'transitionTo').and.callThrough();
                spyOn($activityManager, 'startActivityWithKey').and.callThrough();
                spyOn($activityManager, 'stopActivityWithKey').and.callThrough();
                spyOn($calculatorDataService, 'getDataValue').and.returnValue(application);
                spyOn($capitecCommands, 'ApplyForNewPurchaseCommand').and.callThrough();
                spyOn($capitecQueries, 'GetNewGuidQuery').and.callThrough();
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function(query) {
                    var queryDeferred = $_q.defer();
                    if(query._name==='SAHL.Services.Interfaces.Capitec.Queries.GetNewGuidQuery,SAHL.Services.Interfaces.Capitec') {
                        queryDeferred.resolve(applicationIDResult);
                    } else if(query._name === 'SAHL.Services.Interfaces.Capitec.Queries.GetDeclarationTypesQuery,SAHL.Services.Interfaces.Capitec') {
                        queryDeferred.resolve(declarationLookupResult);
                    }
                    return queryDeferred.promise;
                });
                spyOn($commandManager, 'sendCommandAsync').and.callFake(function() {
                    deferred.resolve(applicationSubmitResult);
                    return deferred.promise;
                });
                createController();
                $scope.SubmitApplication();
                $rootScope.$digest();
            });
            it('should start an activity for submitting the application',function() {
                expect($activityManager.startActivityWithKey).toHaveBeenCalledWith('submittingApplication');
            });
            it('should get an applicationID to use when the application is submitted', function() {
                expect($capitecQueries.GetNewGuidQuery).toHaveBeenCalled();
                expect($queryManager.sendQueryAsync).toHaveBeenCalled();
            });
            it('should create an ApplyForNewPurchase command', function() {
                expect($capitecCommands.ApplyForNewPurchaseCommand).toHaveBeenCalled();
            });
            it('should send the apply for new purchase command', function() {
                expect($commandManager.sendCommandAsync).toHaveBeenCalled();
            });
            it('should stop the activity for submitting an application', function() {
                expect($activityManager.stopActivityWithKey).toHaveBeenCalledWith('submittingApplication');
            });
            it('should transition to the expected state for new purchase applications', function() {
                expect($state.transitionTo).toHaveBeenCalledWith(expectedState, jasmine.any(Object));
            });
        });
    });
});
