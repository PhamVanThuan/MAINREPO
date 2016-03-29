'use strict';
angular.module('sahl.js.ui.filters')
.filter('emptyStringDecorator', function () {
    return function (input) {
        if (_.isEmpty(input) && /^\d+$/g.test(input)===false) {
            return "-";
        }
        return input;
    };
});