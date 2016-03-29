'use strict';
angular.module('sahl.js.core.routing')
    .service('$uiStateManagerService', [function () {
        var store = {};
        var operations = {
            set: function (model) {
                store = model;
            },
            transition: function (stateParams) {
                if (store instanceof Object && !(store instanceof Array)) {
                    for (var modelName in store) {
                        if (store.hasOwnProperty(modelName)) {
                            stateParams[modelName] = store[modelName];
                        }
                    }
                }
                store = {};
            }
        };
        return {
            set: operations.set,
            transition: operations.transition
        };
    }]);
