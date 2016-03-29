'use strict';
angular.module('sahl.js.ui.graphing')
.directive('sahlGrid', ['$graphMathService', function ($graphMathService) {
    return {
        restrict: 'A',
        require: '^sahlCanvas',
        scope:{},
        controller: ['$scope', function ($scope) {
            var internal = {
              calculateStep :function(range){
                var exp = Math.round(Math.log10(range));
                var power = Math.pow(10,exp);
                var step = (range/(power/5)) > 1 ? (power/5) : (power/10);
                return step;
              },
              render: function (paper, width, height, gutter, rangeX, rangeY, zeroPointX, zeroPointY, ratioX, ratioY,maxx,maxy) {
                  var zeroPoint = $graphMathService.getPoint(ratioX, ratioY, 0, 0, zeroPointX, zeroPointY, gutter);

                  if ($scope.showxaxis) {
                    paper.path($graphMathService.horizontalLine(zeroPoint.y, width, gutter)).attr({ 'stroke': '#555555', 'stroke-width': 0.1 });
                  }

                  if ($scope.showyaxis) {
                    paper.path($graphMathService.verticalLine(zeroPoint.x, height, gutter)).attr({ 'stroke': '#555555', 'stroke-width': 0.1 });
                  }

                  if($scope.ylines){
                    var step = internal.calculateStep(rangeY);
                    var count = 0;
                    var xLocation =  zeroPoint.x-5;
                    while((count+step) < maxy){
                      count = count + step;
                      var yLocation = zeroPoint.y - (count*ratioY);
                      paper.path($graphMathService.horizontalLine(yLocation,width, gutter)).attr({ 'stroke': '#555555', 'stroke-width': 0.1 });
                      paper.text(xLocation, yLocation, count).attr({'text-anchor': 'end','font-size':'14px','font-weight':'bold'});
                    }
                  }

                  if($scope.xlines){

                  }
              }
            };
            this.render = internal.render;
        }],
        link: function (scope, element, attrs, ngModel) {
            scope.showxaxis = attrs.showxaxis ? (attrs.showxaxis === 'true' ? true : false) : false;
            scope.showyaxis = attrs.showyaxis ? (attrs.showyaxis === 'true' ? true : false) : false;
            scope.ylines = attrs.ylines ? (attrs.ylines === 'true' ? true : false) : false;
        }
    };
}]);
