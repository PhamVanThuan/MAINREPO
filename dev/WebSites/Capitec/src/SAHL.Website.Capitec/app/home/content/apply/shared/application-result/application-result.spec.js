describe('capitecApp', function() {
    beforeEach(module('capitecApp'));
    beforeEach(module('templates'));

    describe(' - (Application Result Controller) - ', function() {

        // set up global vars
        var $rootScope, $scope, $state, $commandManager, $capitecCommands, $queryManager, $capitecQueries, $notificationService, applicationResultControl, $stateParams, $timeout;

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
            $stateParams = $injector.get('$stateParams');
            $timeout = $injector.get('$timeout');
            $_q = $q;

            // Set up the controller under test
            var $controller = $injector.get('$controller');
            createController = function(state) {
                return $controller('applicationResultCtrl', {
                    '$rootScope': $rootScope,
                    '$scope': $scope,
                    '$state': state,
                    '$commandManager': $commandManager,
                    '$capitecCommands': $capitecCommands,
                    '$queryManager': $queryManager,
                    '$capitecQueries': $capitecQueries,
                    '$notificationService': $notificationService,
                    '$stateParams': $stateParams,
                    '$timeout': $timeout
                });
            };
        }));
        var firstApplicantITCMessages = {
            AllMessages: {
                $values: [{
                    message: 'Empirica score for John Doe is too low.'
                }]
            }
        };
        var secondApplicantITCMessages = {
            AllMessages: {
                $values: [{
                    message: 'Empirica score for Jane Doe is too low.'
                }]
            }
        };
        var applicationCalculationMessages = {
            AllMessages: {
                $values: []
            }
        };
        var declineData = {
            data: {
                ReturnData: {
                    FirstApplicantITCMessages: firstApplicantITCMessages,
                    FirstApplicantName: 'John Doe',
                    FirstApplicantITCPassed: false,
                    SecondApplicantITCMessages: secondApplicantITCMessages,
                    SecondApplicantName: 'Jane Doe',
                    SecondApplicantITCPassed: false,
                    ApplicationCalculationMessages: applicationCalculationMessages,
                    Submitted: false
                }
            }
        };
        var submitData = {
            data: {
                ReturnData: {
                    FirstApplicantName: 'John Doe',
                    FirstApplicantITCPassed: true,
                    SecondApplicantName: 'Jane Doe',
                    SecondApplicantITCPassed: true,
                    Submitted: true
                }
            }
        };
        describe('when an application has been declined', function() {
            var applicationID = '0000-0000-0000-0000';
            var state = {current:{}};
            beforeEach(function() {

                $stateParams.applicationID = applicationID;
                spyOn($capitecQueries, 'GetApplicationResultQuery').and.callThrough();
                spyOn($capitecCommands, 'UpdateCaptureEndTimeCommand').and.callThrough();
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function() {
                    var deferred = $_q.defer();
                    deferred.resolve(declineData);
                    return deferred.promise;
                });
                spyOn($commandManager, 'sendCommandAsync').and.callFake(function() {
                    var deferred = $_q.defer();
                    deferred.resolve();
                    return deferred.promise;
                });
                state.current.name='home.content.apply.application-precheck-for-new-home.new-home.calculation-result';
                state.current.data = {
                    pageHeading: '',
                    icon:''
                };
                createController(state);
                
                $scope.$digest();
                $timeout.flush();
            });
            it('should get the application result using the stateParams', function() {
                expect($capitecQueries.GetApplicationResultQuery).toHaveBeenCalledWith(applicationID);
                expect($queryManager.sendQueryAsync).toHaveBeenCalled();
            });
            it('should set the scope variables for the first applicant', function() {
                expect($scope.applicants[0].messages).toEqual(firstApplicantITCMessages.AllMessages.$values);
                expect($scope.applicants[0].passed).toEqual(false);
                expect($scope.applicants[0].name).toEqual('John Doe');
            });
            it('should set the scope variables for the second applicant', function() {
                expect($scope.applicants[1].messages).toEqual(secondApplicantITCMessages.AllMessages.$values);
                expect($scope.applicants[1].passed).toEqual(false);
                expect($scope.applicants[1].name).toEqual('Jane Doe');
            });
            it('should set the scope variables for the application calculation', function() {
                expect($scope.applicationResult.messages).toEqual(applicationCalculationMessages.AllMessages.$values);
                expect($scope.applicationResult.passed).toEqual(false);
            });
            it('should set the state icon to failure', function() {
                expect(state.current.data.icon).toEqual('failure');
            });
            it('should set the pageHeading to "your application has been declined"', function() {
                expect(state.current.data.pageHeading).toEqual('your application has been declined');
            });
            it('should create a command to update the capture end time', function () {
                expect($capitecCommands.UpdateCaptureEndTimeCommand).toHaveBeenCalled();
            });
            it('should send the command', function() {
                expect($commandManager.sendCommandAsync).toHaveBeenCalled();
            });
        });

        describe('when an application has been declined', function() {
            var applicationID = '0000-0000-0000-0000';
            var state = {current:{}};
            beforeEach(function() {
                $stateParams.applicationID = applicationID;
                spyOn($capitecQueries, 'GetApplicationResultQuery').and.callThrough();
                 spyOn($capitecCommands, 'UpdateCaptureEndTimeCommand').and.callThrough();
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function() {
                    var deferred = $_q.defer();
                    deferred.resolve(submitData);
                    return deferred.promise;
                });
                spyOn($commandManager, 'sendCommandAsync').and.callFake(function() {
                    var deferred = $_q.defer();
                    deferred.resolve();
                    return deferred.promise;
                });
                state.current.name='home.content.apply.application-precheck-for-new-home.new-home.calculation-result';
                state.current.data = {
                    pageHeading: '',
                    icon:''
                };
                createController(state);
                
                $scope.$digest();
                $timeout.flush();
            });
            it('should set the icon to success', function() {
                expect(state.current.data.icon).toBe('success');
            });
            it('should set the pageHeading to "your application has been submitted"', function() {
                expect(state.current.data.pageHeading).toEqual('your application has been submitted');
            });
            it('should set the applicant messages to an empty array', function() {
                expect($scope.applicants[0].messages).toEqual([]);
                expect($scope.applicants[1].messages).toEqual([]);
            });
            it('should set the application messages to an empty array', function() {
                expect($scope.applicationResult.messages).toEqual([]);
            });
            it('should create a command to update the capture end time', function () {
                expect($capitecCommands.UpdateCaptureEndTimeCommand).toHaveBeenCalled();
            });
            it('should send the command', function() {
                expect($commandManager.sendCommandAsync).toHaveBeenCalled();
            });
        });

        describe('when navigating back', function() {
           var applicationID = '0000-0000-0000-0000';
            beforeEach(function() {
                $stateParams.applicationID = applicationID;
                spyOn($capitecQueries, 'GetApplicationResultQuery').and.callThrough();
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function() {
                    var deferred = $_q.defer();
                    deferred.resolve(submitData);
                    return deferred.promise;
                });
                spyOn($commandManager, 'sendCommandAsync').and.callFake(function() {
                    var deferred = $_q.defer();
                    deferred.resolve();
                    return deferred.promise;
                });
                $state.current = {data:{}};
                $state.transitionTo('home.content.apply.application-precheck-for-new-home.new-home.application-result');

                createController($state);
                
                $timeout.flush();
                $scope.$digest();
            });
            it('should transition to the splashscreen', function() {
                $state.transitionTo('home.content.apply.application-precheck-for-new-home');
                $rootScope.$digest();
                expect($state.current.name).toEqual('home.splashscreen');
            });
        });
    });
});
