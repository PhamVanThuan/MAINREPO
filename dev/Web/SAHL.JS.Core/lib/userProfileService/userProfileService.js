'use strict';
angular.module('sahl.js.core.userProfile', [])
    .service('$userProfileService', ['$q', '$rootScope', function ($q, $rootScope) {
        var operations = {
            getUserProfile: function (adName) {},
            saveUserProfile: function (userProfileDocument) {},
            getCapabilitiesForUserOrganisationStructure:function(organisationStructureKey){},
            getCapabilitiesForUser:function(){}
        };
        return {
            getUserProfile: operations.getUserProfile,
            saveUserProfile: operations.saveUserProfile,
            getCapabilitiesForUserOrganisationStructure: operations.getCapabilitiesForUserOrganisationStructure,
            getCapabilitiesForUser: operations.getCapabilitiesForUser
        };
    }]);
