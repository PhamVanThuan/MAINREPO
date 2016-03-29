'use strict';
angular.module('sahl.js.core.logging')
.service('$loggingService', [function () {
    //empty service for now, gets decorated with actual implementation in app.
    //this is just an interface
    var operations = {
        logInfo: function (message) {},
        logWarning: function (message) {},
        logError: function (message, stackTrace) {}
    };
    return {
        logInfo: operations.logInfo,
        logWarning: operations.logWarning,
        logError: operations.logError
    };
}]);
