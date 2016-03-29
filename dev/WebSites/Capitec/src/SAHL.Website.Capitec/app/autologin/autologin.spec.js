describe('capitecApp', function () {
    beforeEach(module('capitecApp'));

    describe(' - (Autologin) - ', function () {
        // set up global vars
        var userInfoData, $rootScope, $state, $scope, $commandManager, $_q, $capitecCommands, $capitecQueries, $queryManager, $activityManager, $cookieService, $stateParams, $timeout, $controller, createController;

        beforeEach(inject(function ($injector, $q) {
            // Setup the root scope
            $rootScope = $injector.get('$rootScope');
            $state = $injector.get('$state');
            $queryManager = $injector.get('$queryManager');
            $scope = $rootScope.$new();
            $commandManager = $injector.get('$commandManager');
            $capitecCommands = $injector.get('$capitecCommands');
            $capitecQueries = $injector.get('$capitecQueries');
            $activityManager = $injector.get('$activityManager');
            $cookieService = $injector.get('$cookieService');
            $stateParams = $injector.get('$stateParams');
            $timeout = $injector.get('$timeout');
            $_q = $q;
            // Set up the controller under test
            $controller = $injector.get('$controller');
            createController = function () {
                return $controller('AutoLoginCtrl', {
                    '$rootScope': $rootScope,
                    '$scope': $scope,
                    '$state': $state,
                    '$commandManager': $commandManager,
                    '$capitecCommands': $capitecCommands,
                    '$queryManager': $queryManager,
                    '$capitecQueries': $capitecQueries,
                    '$activityManager': $activityManager,
                    '$cookieService' : $cookieService,
                    '$stateParams': $stateParams,
                    '$timeout': $timeout 
                });
            };

            userInfoData = {
                data: {
                    ReturnData: {
                        Results: {
                            $values: [
                                {
                                    LastName: 'User',
                                    Roles: '00011-000000-00000-0000',
                                    UserName: 'MrUser',
                                    Id: '00011-000000-00000-0000'
                                }
                            ]
                        }
                    }
                }
            };

            spyOn($activityManager, 'startActivity').and.callThrough();
        }));
        
        describe('when logging in with invalid cp and branch', function() {
            beforeEach(function() {
                spyOn($queryManager, 'sendQueryAsync').and.returnValue($_q.defer());
                spyOn($commandManager, 'sendCommandAsync').and.returnValue($_q.defer());

                $stateParams.cp = "";
                $stateParams.branch = "";
                createController();
                $scope.$digest();
            });
            it('should set the page heading to "Login failed."', function() {
                expect($scope.title).toEqual('Login failed.');
            });
            it('should set the status message to "We are unable to process your login"', function() {
                expect($scope.message).toEqual('We are unable to process your login request.');
            });
        });

        describe('when logging in fails', function() {
            beforeEach(function() {
                spyOn($commandManager, 'sendCommandAsync').and.callFake(function() {
                    var deferred = $_q.defer();
                    deferred.reject();
                    return deferred.promise;
                });

                $stateParams.cp = "MrUser";
                $stateParams.branch = "Somewhere";
                createController();
                $scope.$digest();
            });
            it('should set the page heading to "Login failed."', function() {
                expect($scope.title).toEqual('Login failed.');
            });
            it('should set the status message to "We are unable to process your login"', function() {
                expect($scope.message).toEqual('We are unable to process your login request.');
            });
        });

        describe('when logging in with valid cp and branch', function() {
            var cp = "MrUser";
            var branch = "Somewhere";
            var loginCommand = {};
            var getUserQuery = {};
            beforeEach(function() {
                spyOn($capitecCommands, 'AutoLoginCommand').and.returnValue(loginCommand);
                spyOn($capitecQueries, 'GetUserFromAuthTokenQuery').and.returnValue(getUserQuery);
                spyOn($commandManager, 'sendCommandAsync').and.callFake(function() {
                    var deferred = $_q.defer();
                    deferred.resolve();
                    return deferred.promise;
                });
                spyOn($queryManager, 'sendQueryAsync').and.callFake(function() {
                    var deferred = $_q.defer();
                    deferred.resolve(userInfoData);
                    return deferred.promise;
                });
                spyOn($state,'transitionTo').and.callFake(function(){});

                $stateParams.cp = cp;
                $stateParams.branch = branch;
                createController();
                $scope.$digest();
            });
            it ('should start an activity', function() {
                expect($activityManager.startActivity).toHaveBeenCalled();
            });
            it('should create a login command', function() {
                expect($capitecCommands.AutoLoginCommand).toHaveBeenCalledWith(cp, branch, 'Natal123');
            });
            it('should execute the login command', function() {
                expect($commandManager.sendCommandAsync).toHaveBeenCalledWith(loginCommand);
            });
            it('should set the authenticated flag to true', function(){
                expect($rootScope.authenticated).toEqual(true);
            });
            it('should create a user query', function() {
                expect($capitecQueries.GetUserFromAuthTokenQuery).toHaveBeenCalled();
            });
            it('should execute the user query', function() {
                expect($queryManager.sendQueryAsync).toHaveBeenCalledWith(getUserQuery);
            });
            it('should set the rootScope variables', function() {
                expect($rootScope.userDisplayName).toBe(userInfoData.data.ReturnData.Results.$values[0].LastName);
                expect($rootScope.userRoles).toBe(userInfoData.data.ReturnData.Results.$values[0].Roles);
                expect($rootScope.username).toBe(userInfoData.data.ReturnData.Results.$values[0].Username);
                expect($rootScope.userId).toBe(userInfoData.data.ReturnData.Results.$values[0].Id);
            });
            it('should transition to the splashscreen', function() {
                expect($state.transitionTo).toHaveBeenCalledWith('home.splashscreen');
            });
        });
    });
});