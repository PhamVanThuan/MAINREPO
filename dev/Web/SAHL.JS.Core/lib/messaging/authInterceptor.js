'use strict';
angular.module('sahl.js.core.messaging')
    .factory('$authInterceptor', ['$q', '$authenticatedUser', function($q, $authenticatedUser) {
        var meta_prefix = "#md#_";
        var authInterceptor = {
            request: function(config) {
                if ($authenticatedUser) {
                    config.headers[meta_prefix + 'OrganisationArea'] = $authenticatedUser.currentOrgRole.OrganisationArea;
                    config.headers[meta_prefix + 'currentuserrole'] = $authenticatedUser.currentOrgRole.RoleName;
                    config.headers[meta_prefix + 'username'] = $authenticatedUser.adName;
                    config.headers[meta_prefix + 'UserOrganisationStructureKey'] = $authenticatedUser.currentOrgRole.UserOrganisationStructureKey;
                }
                return config;
            }
        };
        return authInterceptor;
    }]);
