'use strict';
describe('[sahl.js.core.userManagement]', function () {
    beforeEach(module('sahl.js.core.userManagement'));
    var userManagerService, userProfileService, rootScope;
    var getUserProfileDeferred;
    beforeEach(inject(function ($injector, $q, $httpBackend, $rootScope, $userProfileService) {
        userManagerService = $injector.get('$userManagerService');
        userProfileService = $userProfileService;
        spyOn(userProfileService, 'getUserProfile').and.callFake(function () {
            getUserProfileDeferred = $q.defer();
            return getUserProfileDeferred.promise;
        });

        spyOn(userProfileService, 'saveUserProfile').and.callFake(function () {
            getUserProfileDeferred = $q.defer();
            return getUserProfileDeferred.promise;
        });
        rootScope = $rootScope;
    }));

    describe(' - (Service: userManagerService)-', function () {
        describe('when getting authenticated user', function () {
            var authenticatedUser;
            beforeEach(function () {
                authenticatedUser = userManagerService.getAuthenticatedUser();
            });

            it('should contain fullAdName', function () {
                expect(authenticatedUser.fullAdName).not.toBeNull();
            });

            it('should contain domain', function () {
                expect(authenticatedUser.domain).not.toBeNull();
            });

            it('should contain adName', function () {
                expect(authenticatedUser.adName).not.toBeNull();
            });

            it('should contain emailAddress', function () {
                expect(authenticatedUser.emailAddress).not.toBeNull();
            });

            it('should contain displayName', function () {
                expect(authenticatedUser.displayName).not.toBeNull();
            });

            it('should contain currentOrgRole', function () {
                expect(authenticatedUser.currentOrgRole).not.toBeNull();
            });

            it('should contain orgRoles', function () {
                expect(authenticatedUser.orgRoles).not.toBeNull();
            });

            it('should contain state', function () {
                expect(authenticatedUser.state).not.toBeNull();
            });
        });

        describe('when getting user profile', function () {
            var userProfile;
            beforeEach(function () {
                rootScope.ApplicationDocuments = {};
                userProfile = userManagerService.getUserProfile();
            });

            it('should return user profile object', function () {
                expect(userProfile).not.toBeNull();
            });
        });

        describe('when saving a user profile', function () {
            var userProfile;
            var expectedInput;
            beforeEach(function () {
                expectedInput = 'test';
                rootScope.ApplicationDocuments = {};
                userProfile = userManagerService.saveUserProfile(expectedInput);
            });

            it('should save user profile with expected input', function () {
                expect(userProfileService.saveUserProfile).toHaveBeenCalledWith(expectedInput);
            });
        });
    });
});
