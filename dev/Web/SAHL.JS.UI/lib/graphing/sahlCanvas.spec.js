'use strict';
describe('[sAHL.jS.uI.graphing]', function () {
    beforeEach(module('sahl.js.ui.graphing'));
    var $compile, $RootScope;

    beforeEach(inject(function (_$compile_, $rootScope) {
        $compile = _$compile_;
        $RootScope = $rootScope;
    }));

    var compileAndDigest = function (target, scope) {
        var $element = $compile(target)(scope);
        scope.$digest();
        return $element.scope();
    };

    describe(' - (Directive: svgCanvas)-', function () {
        describe('given that we are just rendering a raphael svg canvas', function () {
            var $scope;
            var canvasElement = angular.element('<div sahl-canvas></div>');
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
            }));
            it('should render canvas at 0px x 0px', function () {
                expect(function () {
                    compileAndDigest(canvasElement, $scope);
                }).not.toThrow();
                expect($scope.$$childHead.width).toEqual(0);
                expect($scope.$$childHead.height).toEqual(0);
            });
        });

        describe('given that we are just rendering a raphael svg canvas with width and height', function () {
            var $scope;
            var canvasElement = angular.element('<div sahl-canvas width="10" height="10"></div>');
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
            }));
            it('should render canvas at 10px x 10px', function () {
                expect(function () {
                    compileAndDigest(canvasElement, $scope);
                }).not.toThrow();
                expect($scope.$$childHead.width).toEqual(10);
                expect($scope.$$childHead.height).toEqual(10);
            });
        });
    });
});