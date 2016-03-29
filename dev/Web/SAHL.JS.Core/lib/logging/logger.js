'use strict';
angular.module('sahl.js.core.logging')
    .service('$logger', ['$loggingService',
        function ($loggingService) {
            var rawLogger = {
                func: null,
                instance: null
            };

            rawLogger.func = console.log;
            rawLogger.instance = this;
            var operations = {
                start: function () {
                    console.log = function () {
                    };
                },
                logInfo: function (message) {
                    try {
                        $loggingService.logInfo(message);
                    } catch (err) {
                        rawLogger.func.call(console, "logging service unavailable");
                    }
                },
                logWarning: function (message) {
                    try {
                        $loggingService.logWarning(message);
                    } catch (err) {
                        rawLogger.func.call(console, "logging service unavailable");
                    }
                },
                logError: function (message, stackTrace) {
                    try {
                        $loggingService.logError(message, stackTrace);
                    } catch (err) {
                        rawLogger.func.call(console, "logging service unavailable");
                    }
                }
            };

            if (window.debug) {
                operations.start = function () {
                    console.log = function () {
                        rawLogger.func.call(console, 'Notice! console.log has been depricated!\nuse $logger within the "sahl.js.core.logging" module\nother options:\n$logger.logInfo\n$logger.logWarning\n$logger.logError');
                        rawLogger.func.call(console, arguments);
                    };
                };
                operations.logInfo = function (message) {
                    rawLogger.func.call(console, message);
                };
                operations.logWarning = function (message) {
                    rawLogger.func.call(console, message);
                };
                operations.logError = function (message, stackTrace) {
                    rawLogger.func.call(console, message + "\r\n" + stackTrace);
                };
            }

            return {
                start: operations.start,
                logInfo: operations.logInfo,
                logWarning: operations.logWarning,
                logError: operations.logError
            };
        }]);
