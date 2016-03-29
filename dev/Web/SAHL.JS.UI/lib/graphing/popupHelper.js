'use strict';

angular.module('sahl.js.ui.graphing')
    .service('$popupHelperService', ['$graphMathService',
        function ($graphMathService) {
            var fixedGap = 10;
            var textHelper = {
                getDefaultedText: function (paper, raphaelTextObject, textArray) {
                    for (var i = 0, c = textArray.length; i < c; i++) {
                        raphaelTextObject.push(paper.text(0, 0, textArray[i].text).attr(textArray[i].attr).attr({ 'text-anchor': 'start' }));
                    }
                    var totalHeight = 0;
                    for (var ii = 0, cc = raphaelTextObject.length; ii < cc; ii++) {
                        var bbx = raphaelTextObject[ii].getBBox();
                        raphaelTextObject[ii].attr({ y: (bbx.height / 2) + totalHeight });
                        totalHeight = totalHeight + bbx.height;
                    }
                    return raphaelTextObject.getBBox();
                },
                moveText: function (x, y, space, text, textBoxBoundry, isLeft, isTop) {
                    var diff = space + fixedGap;
                    var newX = isLeft ? x + diff : x - diff - textBoxBoundry.width;
                    var newY = isTop ? y - space : y - textBoxBoundry.height + space;
                    text.translate(newX, newY);
                }
            };

            var backgroundHelper = {
                isLeft: function (x, space, textWidth, canvasWidth) {
                    var total = x + fixedGap + space + textWidth;
                    if (total > canvasWidth) {
                        return false;
                    }
                    return true;
                },
                isTop: function (y, space, textHeight, canvasHeight) {
                    var total = y + fixedGap + space + textHeight;
                    if (total > canvasHeight) {
                        return false;
                    }
                    return true;
                },
                setBackground: function (bBox, x, y, space, isLeft, isTop) {
                    return $graphMathService.getPopupPath(bBox.x, bBox.y, bBox.width, bBox.height, 5, space, isLeft, isTop);
                }
            };

            var popup = function (paper) {
                var _self = this;

                var popupSet = paper.set();
                var text = paper.set();
                var background = paper.path().attr({ fill: '#000', stroke: '#666', 'stroke-width': 2, 'fill-opacity': 0.8 });
                popupSet.push(background);

                _self.hide = function () {
                    popupSet.hide();
                };

                _self.update = function (textArray, x, y, space, width, height) {
                    text.remove();
                    text.clear();
                    var textBoxBoundry = textHelper.getDefaultedText(paper, text, textArray);
                    var isLeft = backgroundHelper.isLeft(x, space, textBoxBoundry.width, width);
                    var isTop = backgroundHelper.isTop(y, space, textBoxBoundry.height, height);

                    textHelper.moveText(x, y, space, text, textBoxBoundry, isLeft, isTop);

                    var backgroundPath = backgroundHelper.setBackground(text.getBBox(), x, y, space, isLeft, isTop);

                    background.attr({ path: backgroundPath, fill: '#000' });
                    popupSet.push(text);

                    background.toFront();
                    text.toFront();
                    popupSet.show();
                };

                _self.hide();

                return _self;
            };

            var external = {
                createPopup: function (paper) {
                    return new popup(paper);
                }
            };
            return {
                createPopup: external.createPopup
            };
        }]);
