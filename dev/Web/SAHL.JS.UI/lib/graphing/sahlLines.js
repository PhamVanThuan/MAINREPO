'use strict';

angular.module('sahl.js.ui.graphing')
//this one is a little more hairy than the others, but it's totally worth it.
.directive('sahlLines', ['$arrayHelperService', '$graphMathService', '$popupHelperService', function ($arrayHelperService, $graphMathService, $popupHelperService) {
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
                    scope.curved = attrs.curved ? true : false;
                    //where most of our minds go, this is actually like a padding, but for the dots
                    scope.gutter = attrs.gutter ? Number(attrs.gutter) : 0;
                    scope.width = width - (scope.gutter * 2);
                    scope.height = height - (scope.gutter * 2);
                    scope.strokeWidth = attrs.strokewidth ? Number(attrs.strokewidth) : 2;
                    scope.hidepoints = attrs.hidepoints ? (attrs.hidepoints === 'true' ? true : false) : false;
                    scope.pointradius = attrs.pointradius ? Number(attrs.pointradius) : 4;
                },
                events: function (dot, dotX, dotY, dataValue, popup, color,dataIndex) {
                    var text = internal.popupText(dataIndex,color, dataValue);
                    (function (dot, dotX, dotY, text, popup, color) {
                        dot.hover(function (element, mouseX, mouseY) {
                            var space = scope.pointradius * 1.4;
                            popup.update(text, dotX, dotY, space, scope.width, scope.height);
                            dot.attr('r', space);
                            scope.mouseover && scope.mouseover(data);
                        }, function (element) {
                            popup.hide();
                            dot.attr('r', scope.pointradius);
                            scope.mouseout && scope.mouseout(data);
                        });
                    })(dot, dotX, dotY, text, popup, color);
                },
                popupText: function (dataIndex,color, dataValue) {
                    var c = Raphael.color(color);
                    if (scope.format) {
                        return scope.format(dataIndex,dataValue, color);
                    }
                    return [{ text: dataValue.x, attr: { font: '12px Helvetica, Arial', fill: '#fff' } },
                            { text: dataValue.y, attr: { font: '10px Helvetica, Arial', fill: Raphael.hsl(c.h, c.s, c.l * 1.4) } }];
                }
            };

            if (!scope.data) {
                return;
            }



            scope.id = canvasController.addInstruction(function (paper) {
                scope.fullWidth = attrs.width ? Number(attrs.width) : (canvasController.getWidth());
                scope.fullHeight = attrs.height ? Number(attrs.height) :(canvasController.getHeight());
                internal.init(scope.fullWidth,scope.fullHeight);

                var maxy = $arrayHelperService.max(scope.data, function (itemArray) {
                    return $arrayHelperService.max(itemArray.values, function (value) {
                        return value.y;
                    });
                });
                var miny = $arrayHelperService.min(scope.data, function (itemArray) {
                    return $arrayHelperService.min(itemArray.values, function (value) {
                        return value.y;
                    });
                });

                var maxx = $arrayHelperService.max(scope.data, function (itemArray) {
                    return $arrayHelperService.max(itemArray.values, function (value) {
                        return value.x;
                    });
                });
                var minx = $arrayHelperService.min(scope.data, function (itemArray) {
                    return $arrayHelperService.min(itemArray.values, function (value) {
                        return value.x;
                    });
                });

                var endSpace = 0;
                if(minx > 0){
                  endSpace = minx;
                }

                miny = miny > 0 ? 0 : miny;
                minx = minx > 0 ? 0 : minx;

                var rangeY = maxy - miny;
                var rangeX = maxx - minx;
                var zeroPointY = maxy;
                var zeroPointX = minx;
                var ratioY = scope.height / rangeY;
                var ratioX = scope.width / (rangeX+endSpace);
                var points = paper.set();
                var popup;
                if (gridController) {
                    gridController.render(paper, scope.fullWidth, scope.fullHeight, scope.gutter, rangeX, rangeY, zeroPointX, zeroPointY, ratioX, ratioY,maxx,maxy);
                }
                if (!scope.hidepoints) {
                    popup = $popupHelperService.createPopup(paper);
                }

                for (var i = 0, c = scope.data.length; i < c; i++) {
                    var data = scope.data[i].values;
                    var path = paper.path().attr({ 'stroke': scope.data[i].color, 'stroke-width': scope.strokeWidth });
                    var dotStyle = { 'fill': '#FFF', 'stroke': scope.data[i].color, 'stroke-width': (scope.pointradius / 2) };
                    var pathPoints;
                    for (var ii = 0, cc = data.length; ii < cc; ii++) {
                        var point = $graphMathService.getPoint(ratioX, ratioY, data[ii].x, data[ii].y, zeroPointX, zeroPointY, scope.gutter);
                        if (!scope.hidepoints) {
                            var dot = paper.circle(point.x, point.y, scope.pointradius).attr(dotStyle);
                            internal.events(dot, point.x, point.y, data[ii], popup, scope.data[i].color,i);
                            points.push(dot, popup);
                        }

                        if (ii === 0) {
                            pathPoints = $graphMathService.getStartingPoint(point, scope.curved);
                        } else if (ii === (cc - 1)) {
                            pathPoints = pathPoints.concat($graphMathService.getEndPoint(point));
                        } else {
                            var previous = $graphMathService.getPoint(ratioX, ratioY, data[ii - 1].x, data[ii - 1].y, zeroPointX, zeroPointY, scope.gutter);
                            var next = $graphMathService.getPoint(ratioX, ratioY, data[ii + 1].x, data[ii + 1].y, zeroPointX, zeroPointY, scope.gutter);
                            pathPoints = pathPoints.concat($graphMathService.lineTo(previous, point, next, scope.curved));
                        }
                    }
                    path.attr({ path: pathPoints });
                }
                points.toFront();
            });

            scope.$watch('data', function (newValue, oldValue) {
                if (oldValue !== newValue) {
                    canvasController.redraw(scope.id);
                }
            });
        }
    };
}]);
