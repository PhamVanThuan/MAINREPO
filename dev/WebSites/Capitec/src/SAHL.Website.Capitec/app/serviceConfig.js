'use strict';

angular.module('capitecApp.serviceConfig', [])
.factory('$serviceConfig', function () {
    return {
        SearchService: 'http://localhost/CapitecSearchService/api/QueryHttpHandler/PerformHttpQuery',
        CommandService: 'http://localhost/CapitecService/api/CommandHttpHandler/performhttpcommand',
        QueryService: 'http://localhost/CapitecService/api/QueryHttpHandler/PerformHttpQuery',
        DecisionTreeService: 'http://localhost/DecisionTreeService/api/QueryHttpHandler/PerformHttpQuery',
        timeout: 1000 *120,
        timeoutRetryCount: 2
    }
});