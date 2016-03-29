'use strict';
angular.module('sahl.js.ui.filters')
  .filter('visibleColumns', ['$filter', function($filter) {
    return function(data, grid, query) {
      var matches = [];

      if (query === undefined|| query==='') {
        return data;
      }    
      query = query.toLowerCase();
        for (var i = 0; i < data.length; i++) {
          for (var j = 0; j < grid.columnDefs.length; j++) {
          
            var dataItem = data[i];
            var fieldName = grid.columnDefs[j]['field'];
            
            //as soon as search term is found, add to match and move to next dataItem
            if (dataItem[fieldName].toString().toLowerCase().indexOf(query)>-1) {
              matches.push(dataItem);
              break;
            }
          }
        }
      return matches;
    };
}]);