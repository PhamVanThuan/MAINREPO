'use strict';

angular.module('sahl.js.ui.graphing')
    .service('$graphMathService', ['$rootScope',
        function ($rootScope) {
            var constants = {
                rad: (Math.PI / 180)
            };

            var points = {
                getCurvePoints: function (previous, current, next) {
                    var partialDiffX1 = (current.x - previous.x) / 2,
                        partialDiffX2 = (next.x - current.x) / 2,
                        a = Math.atan((current.x - previous.x) / Math.abs(current.y - previous.y)),
                        b = Math.atan((next.x - current.x) / Math.abs(current.y - next.y));
                    a = previous.y < current.y ? Math.PI - a : a;
                    b = next.y < current.y ? Math.PI - b : b;
                    var alpha = Math.PI / 2 - ((a + b) % (Math.PI * 2)) / 2;
                    return {
                        previous: { 'x': (current.x - (partialDiffX1 * Math.sin(alpha + a))), 'y': (current.y + (partialDiffX1 * Math.cos(alpha + a))) },
                        next: { 'x': (current.x + (partialDiffX2 * Math.sin(alpha + b))), 'y': (current.y + (partialDiffX2 * Math.cos(alpha + b))) }
                    };
                }
            };

            var lines = {
                getPoint: function (ratioX, ratioY, valueX, valueY, zeroX, zeroY, gutter) {
                    var x = (valueX - zeroX) * ratioX;
                    var y = (zeroY - valueY) * ratioY;
                    return { 'x': x + gutter, 'y': y + gutter };
                },
                getStartingPoint: function (point, isCurvy) {
                    if (isCurvy) {
                        return ['M', point.x, point.y, 'C', point.x, point.y];
                    }
                    return ['M', point.x, point.y, 'L'];
                },
                lineTo: function (previous, current, next, isCurvy) {
                    if (isCurvy) {
                        var tempPoints = points.getCurvePoints(previous, current, next);
                        return [tempPoints.previous.x, tempPoints.previous.y, current.x, current.y, tempPoints.next.x, tempPoints.next.y];
                    }
                    return [current.x, current.y];
                },
                getEndPoint: function (current) {
                    return [current.x, current.y, current.x, current.y];
                },
                horizontalLine: function (positionY, width, gutter) {
                    return ['M', 0 + gutter, positionY, 'L', width-gutter, positionY, 'z'];
                },
                verticalLine: function (positionX, height, gutter) {
                    return ['M', positionX, 0 + gutter, 'L', positionX, height - gutter, 'z'];
                }
            };

            var pathing = {
                getDoughnutSectionPath: function (positionX, positionY, radius, innerRadius, startAngle, endAngle) {
                    var x1 = positionX + radius * Math.cos(-startAngle * constants.rad),
                        x2 = positionX + radius * Math.cos(-endAngle * constants.rad),
                        y1 = positionY + radius * Math.sin(-startAngle * constants.rad),
                        y2 = positionY + radius * Math.sin(-endAngle * constants.rad),
                        xx1 = positionX + innerRadius * Math.cos(-startAngle * constants.rad),
                        xx2 = positionX + innerRadius * Math.cos(-endAngle * constants.rad),
                        yy1 = positionY + innerRadius * Math.sin(-startAngle * constants.rad),
                        yy2 = positionY + innerRadius * Math.sin(-endAngle * constants.rad);
                    return ['M', xx1, yy1, 'L', x1, y1, 'A', radius, radius, 0, +(endAngle - startAngle > 180), 0, x2, y2, 'L', xx2, yy2, 'A',
                            innerRadius, innerRadius, 0, +(endAngle - startAngle > 180), 1, xx1, yy1, 'z'];
                },

                getPieSectionPath: function (positionX, positionY, radius, startAngle, endAngle) {
                    var x1 = positionX + radius * Math.cos(-startAngle * constants.rad),
                        x2 = positionX + radius * Math.cos(-endAngle * constants.rad),
                        y1 = positionY + radius * Math.sin(-startAngle * constants.rad),
                        y2 = positionY + radius * Math.sin(-endAngle * constants.rad);
                    return ['M', positionX, positionY, 'L', x1, y1, 'A', radius, radius, 0, +(endAngle - startAngle > 180), 0, x2, y2, 'L', positionX, positionY, 'z'];
                },
                getBar: function (valuePoint, zeroPoint, width) {
                    return ['M', zeroPoint.x, zeroPoint.y, 'L', valuePoint.x, valuePoint.y, 'L', valuePoint.x + width, valuePoint.y, 'L',
                            valuePoint.x + width, zeroPoint.y, 'L', zeroPoint.x, zeroPoint.y, 'z'];
                },
                getPopupPath: function (positionX, positionY, width, height, padding, space, isLeft, isTop) {
                    var x1 = positionX - padding;
                    var y1 = positionY - padding;
                    var x2 = positionX + width + padding;
                    var y2 = positionY + height + padding;

                    var pointerPointX = isLeft ? x1 - space : x2 + space;
                    var pointerPointY = isTop ? positionY + space + 1 : positionY + height - (1 + space);
                    var pointPointY1 = pointerPointY + space;
                    var pointPointY2 = pointerPointY - space;
                    var retVal = ['M', x1, y1, 'L', x2, y1];
                    if (!isLeft) {
                        retVal = retVal.concat(['L', x2, pointPointY2, 'L', pointerPointX, pointerPointY, 'L', x2, pointPointY1 ]);
                    }
                    retVal = retVal.concat(['L', x2, y2, 'L', x1, y2]);
                    if (isLeft) {
                        retVal = retVal.concat(['L', x1, pointPointY1, 'L', pointerPointX, pointerPointY, 'L', x1, pointPointY2 ]);
                    }
                    return retVal.concat(['z']);
                }
            };

            return {
                getDoughnutSectionPath: pathing.getDoughnutSectionPath,
                getPieSectionPath: pathing.getPieSectionPath,
                getCurvePoints: points.getCurvePoints,
                getPoint: lines.getPoint,
                getStartingPoint: lines.getStartingPoint,
                getEndPoint: lines.getEndPoint,
                lineTo: lines.lineTo,
                horizontalLine: lines.horizontalLine,
                verticalLine: lines.verticalLine,
                getBar: pathing.getBar,
                getPopupPath: pathing.getPopupPath
            };
        }]);
