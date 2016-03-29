describe("CapitecApp", function () {
    beforeEach(module('capitecApp'));
    beforeEach(module('templates'));

    describe(" - (TrackResultsController) - ", function () {

        var $scope, $state;

        beforeEach(inject(function ($injector, $q) {
            $rootScope = $injector.get('$rootScope');
            $scope = $rootScope.$new();
            $state = $injector.get('$state');

            $rootScope.authenticated = true;

            $scope.application = {};
            $state.current = { data: {} };
            var $controller = $injector.get('$controller');
            createController = function () {
                return $controller('TrackResultsCtrl', {
                    '$scope': $scope,
                    '$state': $state
                });
            };
        }));

        describe('when date string is undefined', function () {
            it('should return an empty string', function () {
                var trackResultController = createController();
                var date;
                expect($scope.formatDateString(date)).toBe('');
            });
        });

        describe('when date string is greater than 10 characters', function () {
            it('should return first 10 characters of string', function () {
                var trackResultController = createController();
                expect($scope.formatDateString('1234567890abcde')).toBe('1234567890');
                $scope.$digest();
            });
        });

        describe('when date string is less than 10 characters', function () {
            it('should return first date', function () {
                var trackResultController = createController();
                expect($scope.formatDateString('123456789')).toBe('123456789');
                $scope.$digest();
            });
        });

        describe('when going back', function () {
            beforeEach(function () {
                var trackResultController = createController();
                spyOn($state, 'transitionTo');
                $state.$current = { parent: new Array() };
                $state.$current.parent = { name: 'test' };
                $scope.back();
            });
            it('should transition to parent', function () {
                expect($state.transitionTo).toHaveBeenCalledWith('test');
            });
        });

        describe('when application status is in progress', function () {
            it('should set page heading to your application is currently at stage', function () {
                $scope.application.ApplicationStatus = 'In Progress';
                $scope.application.ApplicationStage = 'Testing'
                var trackResultController = createController();
                expect($state.current.data.pageHeading).toBe('your application is currently at stage Testing');
            });
        });

        describe('when application status is ntu', function () {
            it('should set page heading to client not proceeding with application', function () {
                $scope.application.ApplicationStatus = 'NTU';
                var trackResultController = createController();
                expect($state.current.data.pageHeading).toBe('client not proceeding with application');
            });
        });

        describe('when application status is declined', function () {
            it('should set page heading to application has been declined', function () {
                $scope.application.ApplicationStatus = 'Decline';
                var trackResultController = createController();
                expect($state.current.data.pageHeading).toBe('application has been declined');
            });
        });

        describe('when no application is found', function () {
            it('should set page the heading to no results found', function () {
                $scope.application = null;
                var trackResultController = createController();
                expect($state.current.data.pageHeading).toBe('no results found');
            });
        });

        describe('when going back', function () {
            it('it should return to its parent state', function () {
                spyOn($state, 'transitionTo');
                $state.$current.parent = { name: 'home.content.track' };
                var trackResultController = createController();
                $scope.back();
                expect($state.transitionTo).toHaveBeenCalledWith('home.content.track')
            });
        });
    });
});