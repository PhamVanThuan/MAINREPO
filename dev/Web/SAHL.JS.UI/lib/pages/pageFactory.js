'use strict';
angular.module('sahl.js.ui.pages')
    .factory('$pageFactory', ['$rootScope', '$state', function ($rootScope, $state) {
        var pageFactory = function () {
            var internalModel = {
                lastState: null,
                internalReloadFunction: null
            };

            var internal = {
                tileDataFunction: function (fn) {
                    internalModel.internalReloadFunction = fn;
                }
            };

            var operations = {
                setViewData: function (lastState) {
                    internalModel.lastState = lastState;
                },
                back: function (forceReload) {
                    if (internalModel.lastState === null) {
                        throw "no last state found";
                    }

                    $rootScope.entitiesModel.activeEntity.breadcrumbs.pop();
                    $state.go(internalModel.lastState, null);
                    if (forceReload) {
                        internalModel.internalReloadFunction();
                    }
                }
            };


            return {
                internal: internal,
                setViewData: operations.setViewData,
                back: operations.back
            };
        };
        return new pageFactory();
    }]);