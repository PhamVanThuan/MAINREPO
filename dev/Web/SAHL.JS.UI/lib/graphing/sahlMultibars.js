'use strict';

angular.module('sahl.js.ui.graphing')
    .directive('sahlMultibar', ['$arrayHelperService', '$graphMathService', '$popupHelperService', function ($arrayHelperService, $graphMathService, $popupHelperService) {
        return {
            restrict: 'A',
            require: ['^sahlCanvas', '?^sahlGrid'],
            scope: {
                data: '=',
                mouseover: '=',
                mouseout: '=',
                format: '='
            },
            link: function (scope, element, attrs, controllers) {
                var canvasController = controllers[0];
                var gridController = controllers[1];

                var internal = {
                    init: function (width,height) {
                        scope.gutter = attrs.gutter ? Number(attrs.gutter) : 0;
                        scope.width = width ? width - (scope.gutter * 2) : 0;
                        scope.height = height ? height - (scope.gutter * 2) : 0;
                        scope.gap = attrs.gap ? Number(attrs.gap) : 4;
                    },
                    events: function (barPiece, popup, x, y, data,color,dataIndex) {
                        (function (barPiece, popup, x, y, data) {
                            var text = internal.popupText(dataIndex,color, data);
                            barPiece.hover(function (element) {
                                popup.update(text, x - 5, y, 6, scope.width, scope.height);
                                scope.mouseover && scope.mouseover(data);
                            }, function (element) {
                                popup.hide();
                                scope.mouseout && scope.mouseout(data);
                            });
                        })(barPiece, popup, x, y, data);
                    },
                    popupText: function (dataIndex,color, dataValue) {
                        var c = Raphael.color(color);
                        if (scope.format) {
                            return scope.format(dataIndex,dataValue, color);
                        }
                        return [{ text: dataValue.x, attr: { font: '12px Helvetica, Arial', fill: '#fff' } },
                                { text: dataValue.y, attr: { font: '10px Helvetica, Arial', fill: Raphael.hsl(c.h, c.s, c.l * 1.4) } }];
                    },
                    getAxisInformation : function(data){
                      var maxy = $arrayHelperService.max(data, function (itemArray) {
                          return $arrayHelperService.max(itemArray.values, function (value) {
                              return value.y;
                          });
                      });
                      var miny = $arrayHelperService.min(data, function (itemArray) {
                          return $arrayHelperService.min(itemArray.values, function (value) {
                              return value.y;
                          });
                      });
                      var xAxis = _.union.apply(_,_.map(scope.data,function(item){
                        return _.map(item.values,function(dataitem){
                          return dataitem.x;
                        });
                      }));

                      miny = miny < 0 ? miny : 0;

                      var rangeY = (maxy-miny);
                      return {
                        xAxis : xAxis,
                        maxy : maxy,
                        miny : miny,
                        rangeY : rangeY
                      };
                    }
                };

                if (!scope.data) {
                    return;
                }

                scope.id = canvasController.addInstruction(function (paper) {
                    scope.fullWidth = attrs.width ? Number(attrs.width) : (canvasController.getWidth());
                    scope.fullHeight = attrs.height ? Number(attrs.height) :(canvasController.getHeight());
                    internal.init(scope.fullWidth,scope.fullHeight);

                    var axisInformation = internal.getAxisInformation(scope.data);

                    var ratioY = scope.height / axisInformation.rangeY;

                    var barsPerXValue = scope.data.length;
                    var collectionCount = axisInformation.xAxis.length;

                    var optimalSpacing = scope.width / ((5.0*(barsPerXValue*collectionCount))+(2.0*collectionCount)+3.0);

                    var barWidth = optimalSpacing*4.0;
                    var gapWidth = optimalSpacing*3.0;
                    var spaceWidth = optimalSpacing;

                    if (gridController) {
                        gridController.render(paper, scope.fullWidth, scope.fullHeight, scope.gutter, collectionCount, axisInformation.rangeY, 0, axisInformation.maxy, 1, ratioY,collectionCount,axisInformation.maxy);
                    }

                    var popup = $popupHelperService.createPopup(paper);

                    var xShift = 0;
                    _.each(axisInformation.xAxis,function(axisitem,axisCount){
                      xShift = xShift + gapWidth;
                      _.each(scope.data,function(item,dataCount){
                        var result = _.where(item.values,{x:axisitem});
                        if(dataCount!==0){
                          xShift = xShift + spaceWidth;
                        }
                        if(result.length!==0){
                          var pointValue = $graphMathService.getPoint(1, ratioY, xShift, result[0].y, 0, axisInformation.maxy, scope.gutter);
                          var pointZero = $graphMathService.getPoint(1, ratioY, xShift, 0, 0, axisInformation.maxy, scope.gutter);

                          var path = $graphMathService.getBar(pointValue, pointZero, barWidth);
                          var barPiece = paper.path(path).attr({fill: item.color, 'stroke-width': 0});
                          internal.events(barPiece, popup, pointValue.x + barWidth, pointValue.y, result[0],item.color,dataCount);
                        }
                        xShift = xShift + barWidth;
                      });
                    });
                });

                scope.$watch('data', function (newValue, oldValue) {
                    if (oldValue !== newValue) {
                        canvasController.redraw(scope.id);
                    }
                });
            }
        };
    }]);
