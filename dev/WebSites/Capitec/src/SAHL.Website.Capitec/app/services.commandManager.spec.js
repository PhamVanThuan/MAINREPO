describe("capitecApp", function () {
    beforeEach(module('capitecApp.services'));
    beforeEach(module('SAHL.Services.Interfaces.Capitec.commands'));

    describe(' Capitec Command Manager Service ', function () {
        var $activityManager, $http, $httpBackend, $deferred,$httpBackend, $commandManager, $capitecCommands, $rootScope, $validationManager;
      
        beforeEach(inject(function ($injector, $q) {                  
            $activityManager = $injector.get('$activityManager');
            $http = $injector.get('$http');
            $httpBackend = $injector.get('$httpBackend');
            $commandManager = $injector.get('$commandManager');
            $capitecCommands = $injector.get('$capitecCommands');
            $rootScope = $injector.get('$rootScope');
            $validationManager = $injector.get('$validationManager');
        }));

        describe('when performing a command', function() {

            var commandToSend;

            beforeEach(function() {                
                spyOn($activityManager, 'stopActivity').and.callFake(function() {});
                commandToSend = new $capitecCommands.AddNewCityCommand('New City', 99, '0cf52cca-4ccd-463f-a84d-596e05be210a');
            });

            afterEach(function() {
                $httpBackend.verifyNoOutstandingExpectation();
                $httpBackend.verifyNoOutstandingRequest();
            });

            it('should call stopActivity() if the HTTP post fails', function() {
                spyOn($validationManager, 'Validate').and.returnValue(true);
                $commandManager.sendCommandAsync(commandToSend);
                $httpBackend.whenPOST("http://localhost/CapitecService/api/CommandHttpHandler/performhttpcommand").respond(500);
                $httpBackend.expect('POST','http://localhost/CapitecService/api/CommandHttpHandler/performhttpcommand');
                $httpBackend.flush();
                expect($activityManager.stopActivity).toHaveBeenCalled();
            });

            it('should call stopActivity() if the validation fails for the query', function() {
                spyOn($validationManager, 'Validate').and.returnValue(false);
                $commandManager.sendCommandAsync(commandToSend);
                expect($activityManager.stopActivity).toHaveBeenCalled();
            });
                      
        });
    });
});