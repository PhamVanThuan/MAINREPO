describe('capitecApp', function () {
    beforeEach(module('capitecApp'));
    beforeEach(module('templates'));

   describe(' - (Application Pre Check Controller) - ', function () {

        // set up global vars
       var $rootScope, $scope, $state, $commandManager, $capitecCommands, $queryManager, $capitecQueries, $notificationService, $calculatorDataService, appPreCheckController, application, $httpBackend, 
       declarationAnswers, yesAnswer, noAnswer, passingDeclarations;

        beforeEach(inject(function ($injector, $q) {
            
            // Setup the root scope
            $rootScope = $injector.get('$rootScope');
            $scope = $rootScope.$new();
            $state = $injector.get('$state');
            $commandManager = $injector.get('$commandManager');
            $capitecCommands = $injector.get('$capitecCommands');
            $queryManager = $injector.get('$queryManager');
            $capitecQueries = $injector.get('$capitecQueries');
            $notificationService = $injector.get('$notificationService');
            $calculatorDataService = $injector.get('$calculatorDataService');
            $httpBackend = $injector.get('$httpBackend');
            $rootScope.authenticated = true;
            $_q = $q;

            // Set up the controller under test
            var $controller = $injector.get('$controller');
            createController = function () {
                return $controller('applicationprecheckCtrl', {
                    '$rootScope' : $rootScope,
                    '$scope': $scope,
                    '$state': $state,
                    '$commandManager': $commandManager,
                    '$capitecCommands': $capitecCommands,
                    '$queryManager': $queryManager,
                    '$capitecQueries': $capitecQueries,
                    '$notificationService' : $notificationService,
                    '$calculatorDataService' : $calculatorDataService
                });
            };

            application = {
                _name : 'NewPurchaseApplication',
                interestRate:9.10,
                categoryKey:1,
                LTV:90.00,
                PTI: 27.19,
                instalment:8155.51,
                loanAmount: 900000.00,
                termInMonths:0,
                isUnderDebtCounselling: true, 
                hasTransactionalAccount: false, 
                isNewPurchase: '',
                withinSpecifiedAge : true,
                applicants: []
            };

            declarationAnswers = {
                data: {
                    ReturnData: {
                        Results: {
                            $values: new Array({
                                Id: 'f54495a4-aaee-4031-8099-a2d500ab5a75',
                                Name: 'Yes'
                            },
                            {
                                Id: '5be6c2e7-9ec3-44a8-a572-a2d500ab5a76',
                                Name: 'No'
                            })
                        }
                    }
                }
            };

            yesAnswer = 'f54495a4-aaee-4031-8099-a2d500ab5a75';
            noAnswer = '5be6c2e7-9ec3-44a8-a572-a2d500ab5a76';
            passingDeclarations = {
                numberOfApplicants: 1,
                allApplicantsArePresent: yesAnswer,
                isUnderDebtCounselling: noAnswer,
                withinSpecifiedAge: yesAnswer,
                propertyHasTitleDeed: yesAnswer,
                linkedToPropertySale: yesAnswer
            };
            
            $httpBackend.whenGET('./app/home/content/apply/for-new-home/new-home.tpl.html').respond([]);
        }));
        
        describe('when performing application pre-check', function () {

            it('should return true if application is a New Purchase ', function () {      

                $calculatorDataService.clearData();           
                $calculatorDataService.addData('application', application);
                $state.current = { data: { next: 'for-new-home' } };
                appPreCheckController = createController();

                expect($scope.newPurchaseApplication).toBe(true);
            });

            it('should return false if application is not a New Purchase ', function () {

                $calculatorDataService.clearData();
                application._name = 'Switch';
                $calculatorDataService.addData('application', application);
                $state.current = { data: { next: 'for-switch-home' } };
                appPreCheckController = createController();                

                expect($scope.newPurchaseApplication).toBe(false);
            });
     
        });

        describe('when checking if an application can proceed', function () {
            beforeEach(function () {
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function (queryCommand) {
                    $deferred = $_q.defer();
                    $deferred.resolve(declarationAnswers);
                    return $deferred.promise;
                });
                $calculatorDataService.clearData();           
                $calculatorDataService.addData('application', application);
                $state.current = { data: { next: 'for-new-home' } };
                appPreCheckController = createController();
                $scope.$digest();

                $scope.applicationDeclarations = passingDeclarations;
            });

            it('should return true when all the declarations are set correctly set', function() {
                expect($scope.canproceed()).toBe(true);
            });

            it('should return false if the number of applicants are not set', function() {
                $scope.applicationDeclarations.numberOfApplicants = null;
                expect($scope.canproceed()).toBe(false);
            });
            it('should return false if all applicants are not present', function() {
                $scope.applicationDeclarations.allApplicantsArePresent = noAnswer;
                expect($scope.canproceed()).toBe(false);
            });

            it('should return false if application is under Debt Counselling', function () {
                $scope.applicationDeclarations.isUnderDebtCounselling = yesAnswer;
                expect($scope.canproceed()).toBe(false);
            });

            it('should return false if withinSpecifiedAge is false', function() {
                $scope.applicationDeclarations.withinSpecifiedAge = noAnswer;
                expect($scope.canproceed()).toBe(false);
            });

            it('should return false if the property does not have a title deed', function() {
                $scope.applicationDeclarations.propertyHasTitleDeed = noAnswer;
                expect($scope.canproceed()).toBe(false);
            });

            it('should return false if calcType is a newHome and has no linkedToPropertySale', function () {
                $scope.applicationDeclarations.linkedToPropertySale = noAnswer;
                expect($scope.canproceed()).toBe(false);
            });

            it('should proceed when validation has passed', function () {
                expect($scope.canproceed()).toBe(true);
            });

            it('should store the number of applicants when proceeding', function() {
                $state.current = { data: { next: 'home.content.apply.application-precheck-for-new-home.new-home' } };
                spyOn($calculatorDataService, 'addData').and.callThrough();
                $scope.proceed();
                $scope.$digest();

                expect($calculatorDataService.addData).toHaveBeenCalledWith('NumberOfApplicants', 1);
            });
            it('should transitionTo the next state when proceeding', function() {
                $state.current = { data: { next: 'home.content.apply.application-precheck-for-new-home.new-home' } };
                var expectedTransitionState = $state.current.data.next;
                spyOn($state, 'transitionTo').and.callThrough();

                $scope.proceed();

                $rootScope.$digest();
                expect($state.transitionTo).toHaveBeenCalledWith(expectedTransitionState);
            });
        });
    });
});