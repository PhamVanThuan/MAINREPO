'use strict';

angular.module('halo.menu', [
        'ui.router',
        'halo.webservices'
    ])
    .controller('MenuCtrl', [
        '$rootScope', '$scope', '$userManagerService', '$userProfileWebService', '$state', '$authenticatedUser', '$window', '$entityManagerService',
function ($rootScope, $scope, $userManagerService, $userProfileWebService, $state, $authenticatedUser, $window, $entityManagerService) {
            $scope.user = $authenticatedUser;
            $scope.currentState = $state.current.name;


            $scope.getUserProfile = function(size) {
                return $userProfileWebService.webservices.imageurl + '?username=' + $authenticatedUser.adName + '&size=' + size;
            };

            $scope.changeUserOrgRole = function (role) {
                if (role && role.UserOrganisationStructureKey !== $userManagerService.getAuthenticatedUser().currentOrgRole.UserOrganisationStructureKey) {
                    $userManagerService.selectOrgRole(role).then(function(){
                        $userManagerService.getUserProfile().then(function (data) {
                            var landingPage = data.defaultLandingPage;
                            var params = data.params;
                            if (_.isEmpty(landingPage)) {
                                landingPage = "start.portalPages.search";
                            }
                            $entityManagerService.removeAllEntity();
                            $state.go(landingPage, params, {reload: true});
                        });

                    });
                }
            };

            var userOptions = {
                setDefaultLandingPage: function() {
                    $rootScope.ApplicationDocuments.UserProfile.document.defaultLandingPage = $state.current.name;
                    $rootScope.ApplicationDocuments.UserProfile.document.params = $state.params.valueOf();
                    $userManagerService.saveUserProfile();
                }

            };

            $scope.userOptions = userOptions;
            $("[data-role=dropdown]").dropdown();
        }
    ]);