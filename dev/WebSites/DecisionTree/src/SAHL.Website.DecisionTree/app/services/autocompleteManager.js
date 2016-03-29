'use strict';

angular.module('sahl.tools.app.services.$autocompleteManager', [
    'sahl.tools.app.services.eventAggregatorService',
    'sahl.tools.app.services.eventDefinitions'
])
.factory('$autocompleteManager', ['$rootScope', '$eventDefinitions', '$eventAggregatorService', '$q', function ($rootScope, $eventDefinitions, $eventAggregatorService, $q) { 

    var autocomplete = {};

    var operations = {
        loadLatestVersions: function () {

        },
        loadEnumsVersion: function (version) {

        },
        loadMessagesVersion: function (version) {

        },
        loadVariablesVersion: function (version) {

        },
        selectVersions: function (enumsVersion, messagesVersion, variablesVersion) {

        }
    }

    // subscribe to document events so that an autocomplete list can be built for the trees input and output variables, and they can be updated when they are modified

    // initialise the autocomplete lists with the latest versions





    return {
        autocompleteList: autocomplete,
        loadEnumsVersion: function (version) {

        },
        loadMessagesVersion: function (version) {

        },
        loadVariablesVersion: function (version) {

        },
        selectVersions: function (enumsVersion, messagesVersion, variablesVersion) {

        }
    }
}]);