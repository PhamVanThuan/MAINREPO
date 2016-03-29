'use strict';
angular.module('sahl.websites.halo.services.lookupDataService', [

])
    .service('$lookupDataService', ['$rootScope',
        function ($rootScope) {
            var operations = {
                addLookup: function (key, data) {
                    if (_.isUndefined(_.find($rootScope.lookups, function (dataItem) {
                        return dataItem.key === key;
                    }))) {
                        var dataItem = {
                            key: key,
                            data: data
                        };
                        $rootScope.lookups.push(dataItem);
                    }
                },
                removeLookup: function (key) {
                    if (!_.isUndefined(_.find($rootScope.lookups, function (dataItem) {
                        return dataItem.key === key;
                    }))) {
                        $rootScope.lookups = _.reject($rootScope.lookups, function (dataItem) {
                            return dataItem.Key === key;
                        });
                    }
                },
                getLookup: function (key) {
                    return _.find($rootScope.lookups, function (dataItem) {
                        return dataItem.key === key;
                    }).data;
                }
            };

            return {
                addLookup: operations.addLookup,
                removeLookup: operations.removeLookup,
                getLookup: operations.getLookup,
                start: function () {
                    $rootScope.lookups = [];
                }
            };
        }]);