'use strict';

/* Filters */

angular.module('sahl.tools.app.filters', [])
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
}).filter('exact', function(){
    return function(items, match){
        var matching = [], matches, falsely = true;

        angular.forEach(match, function(value, key){
            falsely = falsely && !value;
        });
        if(falsely){
            return items;
        }

        angular.forEach(items, function(item){ 
            matches = true;
            angular.forEach(match, function(value, key){ 
                if(!!value){ 
                    matches = matches && (item[key] === value);  
                }
            });
            if(matches){
                matching.push(item);  
            }
        });
        return matching;
    }
})
.filter('searchFor', function () {

    // All filters must return a function. The first parameter
    // is the data that is to be filtered, and the second is an
    // argument that may be passed with a colon (searchFor:searchString)

    return function (arr, searchString) {

        if (!searchString) {
            return arr;
        }

        var result = [];

        searchString = searchString.toLowerCase();

        // Using the forEach helper method to loop through the array
        angular.forEach(arr, function (item) {

            if (item.Name.toLowerCase().indexOf(searchString) !== -1) {
                result.push(item);
            }

        });

        return result;
    };

});
;