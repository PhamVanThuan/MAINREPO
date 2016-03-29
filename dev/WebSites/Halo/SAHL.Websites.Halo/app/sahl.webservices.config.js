'use strict';
var webservices = angular.module('halo.webservices', ['sahl.js.core.messaging']);
angular.forEach(globalConfiguration.webServices, function (value, key) {
    webservices.service('$' + key + 'WebService', [
        '$messageManager', function ($messageManager) {
            var urls = {
                commandUrl: value + '/api/CommandHttpHandler/PerformHttpCommand',
                queryUrl: value + '/api/QueryHttpHandler/PerformHttpQuery',
                imageurl: value + '/api/profile/getimage',
                x2CommandWithRetuenedData: value + '/api/commandhandlerwithreturneddata/PerformCommandWithResult',
                x2Command: value + '/api/commandhandler/performcommand',
                restUrl : value
            };

            return {
                webservices: urls,
                sendCommandAsync: function (command) {
                    return $messageManager.postMessage(command, urls.commandUrl);
                },
                sendQueryAsync: function (query, queryOptions) {
                    var filter = '';
                    if (arguments.length > 1) {
                        filter = '?';
                    }
                    var isFirst = false;
                    _.each(queryOptions, function (value, property) {
                        if (!isFirst) {
                            filter = filter + '&';
                        } else {
                            isFirst = false;
                        }
                        filter = filter + property + '=' + value;
                    });

                    return $messageManager.postMessage(query, urls.queryUrl + filter);
                },
                sendX2RequestWithReturnedDataAsync: function (command) {
                    return $messageManager.postMessage(command, urls.x2CommandWithRetuenedData);
                },
                sendX2RequestAsync: function (command) {
                    return $messageManager.postMessage(command, urls.x2Command);
                },
                getQueryAsync: function (query) {
                    //if a direct link has been sent from where to get the query from
                    if (typeof query === 'string') {
                        return $messageManager.getMessage(query);
                    }
                    var sendingUrl = urls.restUrl + query.compiledUrl();
                    return $messageManager.getMessage(sendingUrl);
                }
            };
        }
    ]);
});
