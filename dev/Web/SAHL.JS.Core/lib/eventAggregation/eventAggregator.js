'use strict';
angular.module('sahl.js.core.eventAggregation', [])
.service('$eventAggregatorService', [
function () {
    var messageHandlers = [];
    var keyInUse = '';
    var toRemoveFromKeyInUse = [];

    var operations = {
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
                    return existingMessageHandler === messageHandler;
                })) {
                    messageHandlersForMessageKey.push(messageHandler);
                }
            }
        },
        unsubscribe: function (messageKey, messageHandler) {
            if (keyInUse === messageKey) {
                toRemoveFromKeyInUse.push({
                    key: messageKey,
                    value: messageHandler
                });
            } else {
                var messageHandlersForMessageKey;
                var keyValuePair = messageHandlers.filter(function (existingMessageHandlerKey) {
                    return existingMessageHandlerKey.key === messageKey;
                });

                messageHandlersForMessageKey = keyValuePair[0].value;

                var index = messageHandlersForMessageKey.indexOf(messageHandler);

                if (index > -1) {
                    messageHandlersForMessageKey.splice(index, 1);
                    if (messageHandlersForMessageKey.length === 0) {
                        index = messageHandlers.indexOf(keyValuePair[0]);
                        messageHandlers.splice(index, 1);
                    }
                }
            }
        },
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
                for (var j = 0, c = toRemoveFromKeyInUse.length; j < c; j++) {
                    operations.unsubscribe(toRemoveFromKeyInUse[j].key, toRemoveFromKeyInUse[j].value);
                }
            }

            keyInUse = '';
            toRemoveFromKeyInUse.length = 0;
        },
        clear: function () {
            messageHandlers.length = 0;
        }
    };

    return {
        publish: function (messageKey, message) {
            operations.publish(messageKey, message);
        },
        subscribe: function (messageKey, messageHandler) {
            operations.subscribe(messageKey, messageHandler);
        },
        unsubscribe: function (messageKey, messageHandler) {
            operations.unsubscribe(messageKey, messageHandler);

        },
        clear: function () {
            operations.clear();
        },
        start: function () { },
        getNumberOfRegisteredSubscribers: function () {
            return messageHandlers.length;
        },
        getNumberOfRegisteredSubscribersForEvent: function (messageKey) {
            var keyValuePairs = messageHandlers.filter(function (existingMessageHandlerKey) {
                return existingMessageHandlerKey.key === messageKey;
            });

            return keyValuePairs.length;
        }
    };
}]);
