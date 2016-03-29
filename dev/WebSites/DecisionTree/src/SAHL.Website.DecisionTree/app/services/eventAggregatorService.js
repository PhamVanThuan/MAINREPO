'use strict';

/* Services */

angular.module('sahl.tools.app.services.eventAggregatorService', [
    'sahl.tools.app.serviceConfig',
    'SAHL.Services.Interfaces.DecisionTreeDesign.queries',
    'SAHL.Services.Interfaces.DecisionTreeDesign.commands'
])
.service('$eventAggregatorService', ['$rootScope', function ($rootScope) {
    var messageHandlers = [];
    var keyInUse = '';
    var toRemoveFromKeyInUse = [];

    var operations = {
        unsubscribe: function (messageKey, messageHandler) {
            var messageHandlersForMessageKey;
            var keyValuePair = messageHandlers.filter(function (existingMessageHandlerKey) {
                return existingMessageHandlerKey.key === messageKey;
            });
            if (keyValuePair.length > 0) {
                messageHandlersForMessageKey = keyValuePair[0].value;
            }
            if (messageHandlersForMessageKey !== undefined) {
                var index = messageHandlersForMessageKey.indexOf(messageHandler);
                if (index >= -1) {
                    messageHandlersForMessageKey.splice(index, 1);
                    if (messageHandlersForMessageKey.length === 0) {
                        index = messageHandlers.indexOf(keyValuePair[0]);
                        if (index >= -1) {
                            messageHandlers.splice(index, 1);
                        }
                    }
                }
            }
        }
    }

    return {
        publish: function (messageKey, message) {
            keyInUse = messageKey;

            var messageHandlersForMessageKey;
            var keyValuePair = messageHandlers.filter(function (existingMessageHandlerKey) {
                return existingMessageHandlerKey.key === messageKey;
            });
            if (keyValuePair.length > 0) {
                messageHandlersForMessageKey = keyValuePair[0].value;
                for (var i = 0; i < messageHandlersForMessageKey.length; i++) {
                    if (messageHandlersForMessageKey[i] !== undefined) {
                        messageHandlersForMessageKey[i](message);
                    }
                }
            }

            if (toRemoveFromKeyInUse.length > 0) {
                for (var i = 0, c = toRemoveFromKeyInUse.length; i < c;i++){
                    operations.unsubscribe(toRemoveFromKeyInUse[i].key, toRemoveFromKeyInUse[i].value);
                }
            }

            keyInUse = '';
            toRemoveFromKeyInUse.length = 0;
        },
        subscribe: function (messageKey, messageHandler) {
            var messageHandlersForMessageKey;
            var keyValuePair = messageHandlers.filter(function (existingMessageHandlerKey) {
                return existingMessageHandlerKey.key === messageKey;
            });
            if (keyValuePair.length > 0) {
                messageHandlersForMessageKey = keyValuePair[0].value;
            }
            if (messageHandlersForMessageKey === undefined) {
                messageHandlersForMessageKey = [messageHandler];
                messageHandlers.push({
                    key: messageKey,
                    value: messageHandlersForMessageKey
                });
            } else {
                if (!messageHandlersForMessageKey.some(function (existingMessageHandler) {
                  existingMessageHandler === messageHandler;
                })) {
                    messageHandlersForMessageKey.push(messageHandler);
                }
            }
        },
        unsubscribe: function (messageKey, messageHandler) {
            if (keyInUse === messageKey) {
                toRemoveFromKeyInUse.push({ key: messageKey, value: messageHandler });
            }
            else {
                operations.unsubscribe(messageKey, messageHandler);
            }
        },
        clear: function () {
            messageHandlers.length = 0;
        },
        start: function () { }
    };
}]);