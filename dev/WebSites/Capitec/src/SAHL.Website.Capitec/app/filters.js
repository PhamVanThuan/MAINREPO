'use strict';

/* Filters */

angular.module('capitecApp.filters', [])
  //.filter('interpolate', ['version', function(version) {
  //  return function(text) {
  //    return String(text).replace(/\%VERSION\%/mg, version);
  //  }
  //}])
.filter('removeHiddenCrumbs', function () {
    return function (states) {
        var filtered = [];

        angular.forEach(states, function (state) {
            if (state.abstract === undefined || state.abstract === false) {
                filtered.push(state);
            }
        });
        return filtered;
    }
});