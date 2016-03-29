describe("[capitecApp]", function () {
    beforeEach(module('capitecApp'));
    beforeEach(module('templates'));

    describe(' - (Calculation Result Controller) - ', function () {
        var $rootScope, $scope, $state, $activityManager, createController, $controller;

        beforeEach(inject(function ($injector, $q) {
            // Setup the root scope
            $rootScope = $injector.get('$rootScope');
            $scope = $rootScope.$new();
            $state = $injector.get('$state');
            $activityManager = $injector.get('$activityManager');

            // Set up the controller under test
            $controller = $injector.get('$controller');
            createController = function () {
                return $controller('CalculationResultCtrl', {
                    '$scope': $scope,
                    '$state': $state,
                    '$activityManager': $activityManager
                });
            };

            back = function () {
                $scope.back();
            };
        }));

        describe('when viewing the calculation result control,', function () {
            var processData, expectedCategory, expectedPageHeading, expectedIcon

            beforeEach(function () {
                $scope.calculationResult = {};
                $state = {
                    current: {
                        data: new Array()
                    },
                    $current: {
                        parent: new Array()
                    }
                };
            });

            afterEach(function () {
            });

            it('should set the page heading accordingly if calculator is set the stilllooking', function () {
                // Set up the controller
                $scope.calculationResult.calculator = 'stilllooking';
                createController();
                processData = $scope.calculationResult;

                // Establish the expectations
                expectedPageHeading = "calculation results - subject to full credit assessment";
                expectedIcon = "false";

                // Assert expectations
                expect($state.current.data.pageHeading).toEqual(expectedPageHeading);
                expect($state.current.data.icon).toEqual(expectedIcon);
            });
        });
    });

    describe(' - (Calculation Result Fail Controller) - ', function () {
        var $rootScope, $state, $activityManager;

        beforeEach(inject(function ($injector, $q) {
            // Setup the scope
            $rootScope = $injector.get('$rootScope');
            $scope = $rootScope.$new();
            $state = $injector.get('$state');
            $activityManager = $injector.get('$activityManager');

            // Set up the controller under test
            $controller = $injector.get('$controller');
            createController = function () {
                return $controller('CalculationResultFailCtrl', {
                    '$scope': $scope,
                    '$state': $state,
                    '$activityManager': $activityManager
                });
            };

            // Set up some variables
            calcResultFailController = createController();

            $state = {
                $current: { parent: new Array() }
            };

            $state.$current.parent = {
                name: 'Home'
            };

            // Call back method on controller under test
            back = function () {
                $scope.back();
            };
        }));

        describe('when a calculation fails,', function () {
            it("should set the transitionTo state to the current parent name", function () {
            });
        });
    });
});