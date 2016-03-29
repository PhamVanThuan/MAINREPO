describe("[capitecApp]", function () {
    beforeEach(module('capitecApp'));
    //beforeEach(module('templates'));

    // testing the apply controller
    describe(' - (ApplyController) - ', function () {
        var $httpBackend, $rootScope, $state, $queryManager, $capitecQueries, createController, $httpInterceptor, $capitecCommands;

        beforeEach(inject(function ($injector, $q) {
            // get the root scope
            $rootScope = $injector.get('$rootScope');
            $scope = $rootScope.$new();
            $commandManager = $injector.get('$commandManager');
            $queryManager = $injector.get('$queryManager');
            $capitecQueries = $injector.get('$capitecQueries');
            $httpBackend = $injector.get('$httpBackend');
            $httpInterceptor = $injector.get('$httpInterceptor');            
            $capitecCommands = $injector.get('$capitecCommands');

            // setup the controller under test
            var $controller = $injector.get('$controller');
            createController = function () {
                return $controller('ApplyCtrl', {
                    '$rootScope': $rootScope,
                    '$scope': $scope,
                    '$commandManager': $commandManager,
                    '$queryManager': $queryManager,
                    '$capitecQueries': $capitecQueries,                    
                    '$capitecCommands': $capitecCommands
                });
            };

            var ApplyController = createController();
            spyOn($queryManager, 'sendQueryAsync').and.callThrough();
        }));

        describe('when clicking "Next" on the Apply page,', function () {
            it('should', function () {
                //$scope.testQuery();
                //expect($queryManager.sendQueryAsync).toHaveBeenCalled();
            });
        });
    });
});