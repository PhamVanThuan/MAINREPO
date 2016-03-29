'use strict';

angular.module('sahl.js.ui.graphing')
    .directive('sahlBar', ['$arrayHelperService', '$graphMathService', '$popupHelperService', function ($arrayHelperService, $graphMathService, $popupHelperService) {
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
                if (!attrs.width || !attrs.height) {
                    //don't register a thing as there a missing attributes.
                    return;
                }

                var internal = {
                    init: function () {
                        scope.fullWidth = attrs.width ? Number(attrs.width) : 0;
                        scope.fullHeight = attrs.height ? Number(attrs.height) : 0;
                        //where most of our minds go, this is actually like a padding, but for the dots
                        scope.gutter = attrs.gutter ? Number(attrs.gutter) : 0;
                        scope.width = attrs.width ? Number(attrs.width) - (scope.gutter * 2) : 0;
                        scope.height = attrs.height ? Number(attrs.height) - (scope.gutter * 2) : 0;
                        scope.gap = attrs.gap ? Number(attrs.gap) : 4;
                    },
                    events: function (barPiece, popup, x, y, data) {
                        (function (barPiece, popup, x, y, data) {
                            var text = internal.popupText(data);
                            barPiece.hover(function (element) {
                                popup.update(text, x - 5, y, 6, scope.width, scope.height);
                                scope.mouseover && scope.mouseover(data);
                            }, function (element) {
                                popup.hide();
                                scope.mouseout && scope.mouseout(data);
                            });
                        })(barPiece, popup, x, y, data);
                    },
                    popupText: function (dataValue, color) {
                        if (scope.format) {
                            return scope.format(dataValue, color);
                        }
                        return [
                            {
                                text: dataValue.value,
                                attr: {
                                    font: '12px Helvetica, Arial',
                                    fill: '#fff'
                                }
                            }
                        ];
                    }
                };

                internal.init();

                if (!scope.data) {
                    return;
                }

                scope.id = canvasController.addInstruction(function (paper) {
                    var maxy = $arrayHelperService.max(scope.data, function (item) {
                        return item.value;
                    });
                    var miny = $arrayHelperService.min(scope.data, function (item) {
                        return item.value;
                    });
                    miny = (miny > 0) ? 0 : miny;
                    var rangeY = maxy - miny;
                    var zeroPointY = maxy;
                    var ratioY = scope.height / rangeY;
                    var barWidth = (scope.width - (scope.gap * (scope.data.length + 1))) / scope.data.length;
                    if (gridController) {
                        gridController.render(paper, scope.fullWidth, scope.fullHeight, scope.gutter, 0, rangeY, 0, zeroPointY, 1, ratioY);
                    }
                    var popup = $popupHelperService.createPopup(paper);
                    for (var i = 0, c = scope.data.length; i < c; i++) {
                        var pointValue = $graphMathService.getPoint(1, ratioY, ((i + 1) * scope.gap) + (i * barWidth), scope.data[i].value, 0, zeroPointY, scope.gutter);
                        var pointZero = $graphMathService.getPoint(1, ratioY, ((i + 1) * scope.gap) + (i * barWidth), 0, 0, zeroPointY, scope.gutter);
                        var path = $graphMathService.getBar(pointValue, pointZero, barWidth);
                        var barPiece = paper.path(path).attr({fill: scope.data[i].color, 'stroke-width': 0});
                        internal.events(barPiece, popup, pointValue.x + barWidth, pointValue.y, scope.data[i]);
                    }
                });

                scope.$watch('data', function (newValue, oldValue) {
                    if (oldValue !== newValue) {
                        canvasController.redraw(scope.id);
                    }
                });
            }
        };
    }]);
