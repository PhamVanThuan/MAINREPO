angular.module('sahl.js.core.fluentRestQuery').factory('$orderByDirectionBuilder', [function () {
    return function (returningFunction) {
        return {
            ascending: function () {
                return returningFunction('asc');
            },
            descending: function () {
                return returningFunction('desc');
            }
        };
    };
}]);
