'use strict';
angular.module('sahl.js.core.arrays', [])
.service('$arrayHelperService', [
function () {
    var internal = {
        comparator: function (array, comparisonFn, selector) {
            if (array && selector && array.length > 0) {
                var retVal = selector(array[0]);
                for (var i = 1, c = array.length; i < c; i++) {
                    retVal = comparisonFn(retVal, selector(array[i]));
                }
                return retVal;
            }
            return undefined;
        },
        min:function(array,selector){
            return internal.comparator(array, Math.min, selector);
        },
        max: function (array, selector) {
            return internal.comparator(array, Math.max, selector);
        }
    };

    return {
        min:internal.min,
        max:internal.max
    };
}]);
