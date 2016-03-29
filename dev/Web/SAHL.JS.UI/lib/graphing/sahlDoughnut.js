'use strict';
angular.module('sahl.js.ui.graphing')
.directive('sahlDoughnut', ['$graphMathService', '$popupHelperService', function ($graphMathService, $popupHelperService) {
    return {
        restrict: 'A',
        require: '^sahlCanvas',
        scope: {
            data: '=',
            mouseover: '=',
            mouseout: '=',
            format: '='
        },
        link: function (scope, element, attrs, canvasController) {
            scope.x = attrs.x ? Number(attrs.x) : 0.5;
            scope.y = attrs.y ? Number(attrs.y) : 0.5;
            if (!attrs.radius) {
                throw 'radius is a required field';
            }
            scope.radius = Number(attrs.radius);
            scope.innerRadius = attrs.innerradius ? Number(attrs.innerradius) : 0.4 * scope.radius;

            var internal = {
                events: function (doughPiece, popup, data) {
                    (function (doughPiece, popup, data) {
                        var text = internal.popupText(data);
                        doughPiece.hover(function (element) {
                            popup.update(text, element.offsetX, element.offsetY, 6, scope.width, scope.height);
                            doughPiece.g = doughPiece.glow();
                            scope.mouseover && scope.mouseover(data);
                        }, function (element) {
                            popup.hide();
                            doughPiece.g && doughPiece.g.remove();
                            scope.mouseout && scope.mouseout(data);
                        });
                    })(doughPiece, popup, data);
                },
                popupText: function (dataValue, color) {
                    if (scope.format) {
                        return scope.format(dataValue, color);
                    }
                    return [{ text: dataValue.value, attr: { font: '12px Helvetica, Arial', fill: '#fff' } }];
                }
            };

            scope.id = canvasController.addInstruction(function (paper) {
                var total = 0,
                startAngle = 0;
                for (var index in scope.data) {
                    if (scope.data.hasOwnProperty(index)) {
                        total += scope.data[index].value;
                    }
                }
                var popup = $popupHelperService.createPopup(paper);
                for (var key in scope.data) {
                    if (scope.data.hasOwnProperty(key)) {
                        var endAngle = startAngle + (360 / total * scope.data[key].value);
                        var path = $graphMathService.getDoughnutSectionPath(scope.radius + scope.x, scope.radius + scope.y, scope.radius, scope.innerRadius, startAngle, endAngle);
                        var doughPiece = paper.path(path).attr({ fill: scope.data[key].color, 'stroke-width': 0.5 });
                        internal.events(doughPiece, popup, scope.data[key]);
                        startAngle = endAngle;
                    }
                }
            });

            scope.$watch('data', function (newValue, oldValue) {
                if (newValue !== oldValue) {
                    canvasController.redraw(scope.id);
                }
            });
        }
    };
}]);