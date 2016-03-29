'use strict';

describe('[sahl.js.core.tileManager]', function () {
    beforeEach(module('sahl.js.core.tileManager'));
    beforeEach(module('sahl.js.core.logging'));

    var $q, $logger, $rootScope, $tileManagerConfigurationService;
    var $tileManagerService;

    var getApplicationConfigurationCalled = false;
    var getModuleConfigurationCalled = false;

    beforeEach(inject(function ($injector) {
        $q = $injector.get('$q');
        $logger = $injector.get('$logger');
        $rootScope = $injector.get('$rootScope');
        $tileManagerConfigurationService = $injector.get('$tileManagerConfigurationService');

        $tileManagerService = $injector.get('$tileManagerService');

        $tileManagerConfigurationService.getApplicationConfiguration = function(applicationName) {
            var deferred = $q.defer();
            getApplicationConfigurationCalled = true;

            var queryDataResult = null;

            if(applicationName === 'My Halo'){
                    queryDataResult = myHaloApplicationConfigurationQueryResult;
            }
            else if(applicationName === 'Home'){
                    queryDataResult = homeApplicationConfigurationQueryResult;
            }
            else{
            	$logger.logInfo('Failed to load application Configuration for ' + applicationName);
            }

            if (queryDataResult === null) {
                deferred.reject();
            }
            else {
                deferred.resolve(queryDataResult.ReturnData.Results.$values[0].HaloApplicationModel);
            }

            return deferred.promise;
        };

        $tileManagerConfigurationService.getModuleConfiguration = function(applicationName, moduleName, rootTileLevel, businessKeyType, businessKeyValue) {
            var deferred = $q.defer();
            getModuleConfigurationCalled = true;

            var queryDataResult = null;

            if(moduleName === 'Clients'){
            	queryDataResult = clientsModuleConfigurationResult;
            }
            else
            {
            	$logger.logInfo('Failed to load Module Configuration for ' + moduleName);
            }

            if (queryDataResult === null) {
                deferred.reject();
            }
            else {
                deferred.resolve(queryDataResult.ReturnData.Results.$values[0].ModuleConfiguration);
            }

            return deferred.promise;
        };
    }));

    describe('when loading existing application configuration', function() {
        var applicationConfiguration = null;

        beforeEach(function() {
            getApplicationConfigurationCalled = false;

            $tileManagerService.loadApplicationConfiguration('Home').then(function(configuration) {
                applicationConfiguration = configuration;
            }, function() {
            });
            $rootScope.$apply();
        });

        it ('should call getApplicationConfiguration', function() {
            expect(getApplicationConfigurationCalled).toEqual(true);
        });

        it ('should return application configuration', function() {
           expect(applicationConfiguration).not.toBe(null);
        });
    });

    describe('when loading invalid application configuration', function() {
        var applicationConfiguration = null;

        beforeEach(function() {
            getApplicationConfigurationCalled = false;

            $tileManagerService.loadApplicationConfiguration('Test').then(function(configuration) {
                applicationConfiguration = configuration;
            }, function() {
            });
            $rootScope.$apply();
        });

        it ('should return null', function() {
           expect(applicationConfiguration).toBe(null);
        });
    });

    describe('when loading existing module configuration', function() {
        var moduleConfiguration = null;

        beforeEach(function() {
            getModuleConfigurationCalled = false;
            $tileManagerService.loadModuleConfiguration('Home', 'Clients', 'Account', 1234, null).then(function(configuration) {
                moduleConfiguration = configuration;
            }, function() {
            });
            $rootScope.$apply();
        });

        it ('should call getModuleConfiguration', function() {
            expect(getModuleConfigurationCalled).toEqual(true);
        });

        it ('should return module configuration', function() {
            expect(moduleConfiguration).not.toBe(null);
        });
    });

    describe('when loading module configuration', function() {
        var moduleConfiguration = null;

        beforeEach(function() {
            getModuleConfigurationCalled = false;
            $tileManagerService.loadModuleConfiguration('Home', 'Test', 'Account', 1234).then(function(configuration) {
                moduleConfiguration = configuration;
            }, function() {
            });
            $rootScope.$apply();
        });

        it ('should return null', function() {
            expect(moduleConfiguration).toBe(null);
        });
    });
});
