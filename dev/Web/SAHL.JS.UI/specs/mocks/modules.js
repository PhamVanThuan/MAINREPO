'use strict';
angular.module('halo.core.webservices', []).
    service('$haloWebService', ['$q', function ($q) {
        return {
            sendCommandAsync: function () {
            },
            sendQueryAsync: function () {
            }
        };
    }]);

angular.module('SAHL.Services.Interfaces.Halo.queries', []).
    factory('$haloQueries', [function () {
        var shared = (function () {
            function GetAllApplicationsQuery() {
            };

            function GetApplicationConfigurationForRoleQuery(applicationName, role) {
            };

            return {
                GetAllApplicationsQuery: GetAllApplicationsQuery,
                GetApplicationConfigurationForRoleQuery: GetApplicationConfigurationForRoleQuery
            };
        }());
        return shared;
    }]);