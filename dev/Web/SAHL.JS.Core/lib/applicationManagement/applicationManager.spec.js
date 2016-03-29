'use strict';

var authenticatedUser = {
    adName: "<%=Page.User.Identity.Name %>",
    displayName: '',
    currentOrgRole: '',
    orgRoles: [],
    state: 'NoOrgRole'
};

describe('[sahl.js.core.applicationManagement]', function() {
    beforeEach(module('sahl.js.core.eventAggregation'));
    beforeEach(module('sahl.js.core.activityManagement'));
    beforeEach(module('sahl.js.core.logging'));
    beforeEach(module('sahl.js.core.applicationManagement'));
    beforeEach(module('sahl.js.core.userManagement'));
    beforeEach(module('halo.core.webservices'));
    beforeEach(module('sahl.js.core.serviceManagement'));

    describe(' - (Service: applicationManager)-', function() {

        var $rootScope, $applicationManager, $userManager, $portalPageManager, $startableManager, $q, $eventAggregator, $timeout;

        beforeEach(inject(function($injector) {
            $rootScope = $injector.get('$rootScope');
            $userManager = $injector.get('$userManagerService');
            $applicationManager = $injector.get('$applicationManagerService');
            $startableManager = $injector.get('$startableManagerService');
            $eventAggregator = $injector.get('$eventAggregatorService');
            $timeout = $injector.get('$timeout');
            $q = $injector.get('$q');
        }));

        describe('given the application is a portal application', function() {

            describe('and there is a authenticated user that has a valid org role', function() {
                it('it should start the services', function() {
                    spyOn($userManager, 'getAuthenticatedUser').and.returnValue({
                        adName: "",
                        displayName: "",
                        currentOrgRole: "",
                        orgRoles: [],
                        state: $userManager.userStates.VALID
                    });
                    spyOn($startableManager, 'startServices').and.callThrough();

                    $applicationManager.startApp();

                    expect($userManager.getAuthenticatedUser).toHaveBeenCalled();
                    $rootScope.$digest();
                    expect($startableManager.startServices).toHaveBeenCalled();
                });

                it('should set the application state to starting', function() {
                    spyOn($userManager, 'getAuthenticatedUser').and.returnValue({
                        adName: "",
                        displayName: "",
                        currentOrgRole: "",
                        orgRoles: [],
                        state: $userManager.userStates.VALID
                    });
                    var deferred = $q.defer();

                    spyOn($startableManager, 'startServices').and.returnValue(deferred.promise);

                    $applicationManager.startApp();
                    $rootScope.$digest();
                    expect($applicationManager.getCurrentState()).toEqual($applicationManager.applicationStates.STARTING);

                });

                describe('given that the startables succeed', function() {
                    it('should set the applicationstate to started', function(done) {
                        spyOn($userManager, 'getAuthenticatedUser').and.returnValue({
                            adName: "",
                            displayName: "",
                            currentOrgRole: "",
                            orgRoles: [],
                            state: $userManager.userStates.VALID
                        });
                        spyOn($startableManager, 'startServices').and.callThrough();

                        $applicationManager.startApp().then(function(value){
                            expect($applicationManager.getCurrentState()).toEqual($applicationManager.applicationStates.STARTED);    
                        }).finally(done);

                        $rootScope.$digest();
                        $timeout.flush();
                        
                    });
                });

                describe('given that the startables fail', function() {
                    it('should set the applicationstate to failed', function() {
                        spyOn($userManager, 'getAuthenticatedUser').and.returnValue({
                            adName: "",
                            displayName: "",
                            currentOrgRole: "",
                            orgRoles: [],
                            state: $userManager.userStates.VALID
                        });
                        var deferred = $q.defer();
                        deferred.reject();
                        spyOn($startableManager, 'startServices').and.returnValue(deferred.promise);

                        $applicationManager.startApp();

                        $rootScope.$digest();
                        expect($applicationManager.getCurrentState()).toEqual($applicationManager.applicationStates.FAILED);
                    });
                });
            });
        });
    });
});
