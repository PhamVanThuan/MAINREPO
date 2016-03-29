'use strict';
angular.module('sahl.js.ui.filters')
.filter('dateViewFilter', function () {
    return function (input) {
        if (_.isEmpty(input)) {
            return "-";
        }
        if (input === "0001-01-01T00:00:00") {
            return "-";
        }
        return moment(input).format("DD/MM/YYYY");
    };
});