'use strict';
angular.module('sahl.js.ui.graphing')
.directive('sahlPie', ['$graphMathService', '$popupHelperService', function ($graphMathService, $popupHelperService) {
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
                throw new Error('radius is a required field');
            }
            scope.radius = Number(attrs.radius);

            var internal = {
                events: function (piePiece, popup, data) {
                    (function (piePiece, popup, data) {
                        var text = internal.popupText(data);
                        piePiece.hover(function (element) {
                            popup.update(text, element.offsetX, element.offsetY, 6, scope.width, scope.height);
                            piePiece.g = piePiece.glow();
                            scope.mouseover && scope.mouseover(data);
                        }, function (element) {
                            popup.hide();
                            piePiece.g && piePiece.g.remove();
                            scope.mouseout && scope.mouseout(data);
                        });
                    })(piePiece, popup, data);
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
                var popup = $popupHelperService.createPopup(paper);
                for (var index in scope.data) {
                    if (scope.data.hasOwnProperty(index)) {
                        total += scope.data[index].value;
                    }
                }
                for (var key in scope.data) {
                    if (scope.data.hasOwnProperty(key)) {
                        var endAngle = startAngle + (360 / total * scope.data[key].value);
                        var path = $graphMathService.getPieSectionPath(scope.radius + scope.x, scope.radius + scope.y, scope.radius, startAngle, endAngle);
                        var piePiece = paper.path(path).attr({ fill: scope.data[key].color, 'stroke-width': 0.5 });
                        internal.events(piePiece, popup, scope.data[key]);
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