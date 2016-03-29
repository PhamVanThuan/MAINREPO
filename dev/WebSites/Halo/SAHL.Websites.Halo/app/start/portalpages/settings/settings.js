'use strict';
angular.module('halo.start.portalpages.settings', [
])
.config([
    '$stateProvider', function ($stateProvider) {
        $stateProvider.state('start.portalPages.settings', {
            url: 'settings/',
            templateUrl: 'app/start/portalPages/settings/settings.tpl.html',
            controller: 'SettingsCtrl'
        });
    }
])
.controller('SettingsCtrl', ['$rootScope', '$scope', '$authenticatedUser', '$userProfileService', '$state', '$stateParams','$toastManagerService',
                    function ($rootScope, $scope, $authenticatedUser, $userProfileService, $state,$stateParams,$toastManagerService) {
    var capabilities = null;
    
    $userProfileService.getCapabilitiesForUser($authenticatedUser.fullAdName).then(function (data) {
        capabilities = data;
        $scope.user = $authenticatedUser;
        $scope.defaultRole = $rootScope.ApplicationDocuments.UserProfile.document.defaultRole;
    });

    $scope.selectRole = function (UserOrganisationStructureKey) {
        $scope.defaultRole = UserOrganisationStructureKey;
        $rootScope.ApplicationDocuments.UserProfile.document.defaultRole = UserOrganisationStructureKey;
    };

    $scope.getRoleCapabilities = function (UserOrganisationStructureKey) {
        UserOrganisationStructureKey = Number(UserOrganisationStructureKey);
        var filter = _.filter(capabilities, function (data) {
            return data.Id === UserOrganisationStructureKey;
        });
        return filter !== null && filter !== undefined && filter.length > 0 ? filter[0].Capabilities.$values : ["None"];
    };

    $scope.back = function(){
        $state.go($stateParams.previousState);
    };

    $scope.save = function () {
        $userProfileService.saveUserProfile().then(function () {
            $toastManagerService.success({ title: 'Saved', text: 'User profile settings saved.' });
        });
    };
}]);
