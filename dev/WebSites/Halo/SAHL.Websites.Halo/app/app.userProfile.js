'use strict';
angular.module('halo.userProfile', [
        'halo.webservices',

        'sahl.js.core.activityManagement',
        'SAHL.Services.Interfaces.JsonDocument.queries',
        'SAHL.Services.Interfaces.JsonDocument.commands',
        'SAHL.Services.Interfaces.UserProfile.queries',
        'sahl.js.core.documentManagement'
])
.provider('$userProfile', [function () {
    this.decoration = ['$delegate', '$q', '$rootScope', '$documentManagerService',
                       '$userProfileQueries', '$userProfileWebService', '$activityManager', '$authenticatedUser',
        function ($delegate, $q, $rootScope, $documentManagerService, $userProfileQueries,
                  $userProfileWebService, $activityManager, $authenticatedUser) {
            var userProfileDocumentType = 'userprofile';
            
            var operations = {
                getUserProfile: function () {
                    var deferred = $q.defer();
                    $documentManagerService.openDocumentByName($authenticatedUser.fullAdName, userProfileDocumentType).then(function (document) {
                        $rootScope.ApplicationDocuments.UserProfile = document;
                        deferred.resolve(document);
                    }, function (data) {
                        deferred.reject(data);
                    });
                    return deferred.promise;
                },
                saveUserProfile: function () {
                    var deferred = $q.defer();
                    $documentManagerService.saveDocument($rootScope.ApplicationDocuments.UserProfile).then(function () {
                        deferred.resolve();
                    });
                    return deferred.promise;
                },
                getCapabilitiesForUserOrganisationStructure: function (organisationStructureKey) {
                    var deferred = $q.defer();
                    var query = new $userProfileQueries.GetCapabilitiesForUserOrganisationStructureQuery(organisationStructureKey);
                    $userProfileWebService.sendQueryAsync(query).then(function (data) {
                        deferred.resolve(data.data.ReturnData.Results.$values[0].Capabilities.$values);
                    }, function (data) {
                        deferred.reject(data);
                    });
                    return deferred.promise;
                },
                getCapabilitiesForUser:function(){
                    var deferred = $q.defer();
                    var query = new $userProfileQueries.GetCapabilitiesForUserQuery($authenticatedUser.fullAdName);
                    $activityManager.startActivityWithKey("default");
                    $userProfileWebService.sendQueryAsync(query).then(function (data) {
                        deferred.resolve(data.data.ReturnData.Results.$values[0].RoleCapabilities.$values);
                        $activityManager.stopActivityWithKey("default");
                    }, function (data) {
                        $activityManager.stopActivityWithKey("default");
                        deferred.reject(data);
                    });
                    return deferred.promise;
                }
            };
            return {
                getUserProfile: operations.getUserProfile,
                saveUserProfile: operations.saveUserProfile,
                getCapabilitiesForUserOrganisationStructure: operations.getCapabilitiesForUserOrganisationStructure,
                getCapabilitiesForUser: operations.getCapabilitiesForUser
            };
        }
    ];

    this.$get = [function () {}];
}]);
