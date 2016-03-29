'use strict';
angular.module('halo.logging', [
        'halo.webservices',
        'SAHL.Services.Interfaces.Logging.commands'
])
    .provider('$logging', [function () {
        this.decoration = ['$delegate', '$loggingWebService', '$loggingCommands',
            function ($delegate, $loggingWebService, $loggingCommands) {
                var performLogCommand = function (command) {
                    $loggingWebService.sendCommandAsync(command);
                };

                return {
                    logInfo: function (message) {
                        performLogCommand(new $loggingCommands.LogInfoCommand('Halo', message));
                    },
                    logWarning: function (message) {
                        performLogCommand(new $loggingCommands.LogWarning('Halo', message));
                    },
                    logError: function (message, stackTrace) {
                        performLogCommand(new $loggingCommands.LogErrorCommand('Halo', message, stackTrace));
                    }
                };
            }
        ];

        this.$get = [function () { }];
    }]);