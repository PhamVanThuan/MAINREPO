'use strict';

angular.module('sahl.js.ui.graphing')
    .directive('sahlCanvas', ['$window',function ($window) {
        return {
            restrict: 'A',
            scope: {
            },
            controller: ['$scope', function ($scope) {
                $scope.canvasElement = undefined;
                $scope.canvasElements = [];
                var internal = {
                    addInstruction: function (fn) {
                        return $scope.canvasElements.push({ 'fn': fn, 'set': undefined }) - 1;
                    },

                    draw: function () {
                        var length = $scope.canvasElements.length - 1;
                        for (var i = length; i >= 0; i--) {
                            $scope.canvasElement.setStart();
                            $scope.canvasElements[i].fn($scope.canvasElement);
                            $scope.canvasElements[i].set = $scope.canvasElement.setFinish();
                        }
                    },
                    redraw: function (instructionId) {
                        $scope.canvasElements[instructionId].set.remove();
                        $scope.canvasElement.setStart();
                        $scope.canvasElements[instructionId].fn($scope.canvasElement);
                        $scope.canvasElements[instructionId].set = $scope.canvasElement.setFinish();
                        for (var i = instructionId - 1; i >= 0; i--) {
                            $scope.canvasElements[i].set.toFront();
                        }
                    },
                    getWidth : function(){
                      return $scope.width;
                    },
                    getHeight : function(){
                      return $scope.height;
                    }
                };

                $scope.draw = internal.draw;
                $scope.setCanvas = function (canvas,width,height) {
                    $scope.canvasElement = canvas;
                    $scope.canvasElement.setViewBox(0,0,width,height,true);
                    var element = $($scope.canvasElement.canvas);
                    element.removeAttr('width');
                    element.removeAttr('height');
                    element.attr('preserveAspectRatio','none');
                    element.addClass('full-height');
                    element.addClass('full-width');

                };

                this.addInstruction = internal.addInstruction;
                this.redraw = internal.redraw;
                this.getWidth = internal.getWidth;
                this.getHeight = internal.getHeight;
            }],
            link: function ($scope, element, attrs, controller) {
              if(attrs.width){
                $scope.width = Number(attrs.width);
              }else{
                var width = 0;
                if(element && element['width']){
                   width = element.width();
                }
                attrs.width = $scope.width = width;
              }
              if(attrs.height){
                $scope.height = Number(attrs.height);
              }else{
                var height = 0;
                if(element && element['parent'] && element.parent()['height']){
                   height = element.parent().height();
                }
                attrs.height = $scope.height = height;
              }

              element.ready(function () {
                  $scope.setCanvas(Raphael($(element)[0]),$scope.width, $scope.height);
                  $scope.draw();
              });
            }
        };
    }]);
