'use strict';
angular.module('sahl.js.core.userManagement')
    .service('$userManagerService', ['$rootScope', '$userProfileService', '$q', '$authenticatedUser', '$eventAggregatorService','$timeout',
        function ($rootScope, $userProfileService, $q, $authenticatedUser, $eventAggregatorService, $timeout) {
            var userStates = {
                NOACCESS: 'NoAccess',
                NOORGROLE: 'NoOrgRole',
                VALID: 'ValidUser'
            };

            var userEvents = {
                OrgRoleChanged: 'orgRoleChanged'
            };

            var operations = {
                getAuthenticatedUser: function () {
                    return {
                        fullAdName: $authenticatedUser.fullAdName,
                        domain: $authenticatedUser.domain,
                        adName: $authenticatedUser.adName,
                        emailAddress: $authenticatedUser.emailAddress,
                        displayName: $authenticatedUser.displayName,
                        currentOrgRole: $authenticatedUser.currentOrgRole,
                        orgRoles: $authenticatedUser.orgRoles,
                        capabilities: $authenticatedUser.capabilities,
                        state: $authenticatedUser.state
                    };
                },
                selectOrgRole: function (role) {
                    var deferred = $q.defer();
                    if (role && role.UserOrganisationStructureKey !== $authenticatedUser.currentOrgRole.UserOrganisationStructureKey){
                        $userProfileService.getCapabilitiesForUserOrganisationStructure(role.UserOrganisationStructureKey).then(function(capabilities){
                            $authenticatedUser.capabilities = capabilities;
                            $authenticatedUser.currentOrgRole = role;
                            $eventAggregatorService.publish(userEvents.OrgRoleChanged, {role:role,capabilities:capabilities});
                            deferred.resolve();
                        });
                    }else{
                        $timeout(function(){
                            deferred.resolve();
                        },500);
                    }

                    return deferred.promise;
                },
                getUserProfile: function () {
                    var deferred = $q.defer();
                    var userProfile = $rootScope.ApplicationDocuments.UserProfile;
                    if (_.isUndefined(userProfile)) {
                        $userProfileService.getUserProfile($authenticatedUser.fullAdName).then(function () {
                            var role = _.find($authenticatedUser.orgRoles, function (role) {
                                return role.UserOrganisationStructureKey === $rootScope.ApplicationDocuments.UserProfile.document.defaultRole;
                            });
                            if(_.isEmpty(role)){
                                role = $authenticatedUser.currentOrgRole;
                            }
                            operations.selectOrgRole(role);
                            deferred.resolve($rootScope.ApplicationDocuments.UserProfile.document);
                        });
                    } else {
                        deferred.resolve(userProfile.document);
                    }
                    return deferred.promise;
                },
                saveUserProfile: function (userProfile) {
                    $userProfileService.saveUserProfile(userProfile).then(function () {
                        $userProfileService.getUserProfile($authenticatedUser.fullAdName);
                    });
                }
            };
            return {
                userStates: userStates,
                getAuthenticatedUser: operations.getAuthenticatedUser,
                selectOrgRole: operations.selectOrgRole,
                getUserProfile: operations.getUserProfile,
                saveUserProfile: operations.saveUserProfile
            };
        }
    ]);
