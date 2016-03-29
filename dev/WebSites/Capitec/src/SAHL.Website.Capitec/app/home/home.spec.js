describe('capitecApp', function () {
    beforeEach(module('capitecApp'));

    describe(' - (Home) - ', function () {
        // set up global vars
        var $rootScope, $state, $scope, $capitecQueries, promise, $controller;

        beforeEach(inject(function ($injector, $q) {
            // Setup the root scope
            $rootScope = $injector.get('$rootScope');
            $state = $injector.get('$state');
            $scope = $rootScope.$new();

            // Set up the controller under test
            $controller = $injector.get('$controller');
            createController = function () {
                return $controller('HomeCtrl', {
                    '$scope': $scope,
                    '$state': $state
                });
            };

            // Set up some variables
            var homeController = createController();            
            spyOn($state, 'transitionTo').and.callThrough();
        }));
        
    });
});