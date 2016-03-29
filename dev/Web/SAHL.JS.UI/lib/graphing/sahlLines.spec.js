'use strict';
describe('[sahl.js.ui.graphing]', function () {
    beforeEach(module('sahl.js.ui.graphing'));
    beforeEach(module('sahl.js.core.arrays'));
    var $compile, $RootScope, $graphMathService, $canvasController, $arrayHelperService;

    beforeEach(inject(function (_$compile_, $rootScope, $injector) {
        $compile = _$compile_;
        $RootScope = $rootScope;
        $graphMathService = $injector.get('$graphMathService');
        $arrayHelperService = $injector.get('$arrayHelperService');
    }));

    var compileAndDigest = function (target, scope, postDigestFn) {
        var $element = $compile(target)(scope);
        postDigestFn && scope.$$postDigest(postDigestFn);
        $RootScope.$digest();
        return $element.scope();
    };

    describe(' - (Directive: sahlLine)-', function () {
        describe('given that a line is not inside of a canvas', function () {
            var linesElement = angular.element('<div sahl-lines></div>');
            var $scope;
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
            }));
            it('should throw an error', function () {
                expect(function () {
                    compileAndDigest(linesElement, $scope);
                }).toThrow();
            });
        });

        describe('given that lines is inside of a canvas', function () {
            var linesElement = angular.element('<div sahl-canvas><div sahl-lines></div></div>');
            var $scope;
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
            }));

            it('should not throw an error', function () {
                expect(function () {
                    compileAndDigest(linesElement, $scope);
                }).not.toThrow();
            });
        });

        describe('given that has no height and width set', function () {
            var linesElement = angular.element('<div sahl-canvas><div sahl-lines></div></div>');
            var $scope, canvasScope;
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                canvasScope = compileAndDigest(linesElement, $scope);
            }));

            it('should not set width and height on the scope', function () {
                expect(canvasScope.$$childTail.height).toBeUndefined();
                expect(canvasScope.$$childTail.width).toBeUndefined();
            });
        });

        describe('given that data is set', function () {
            var linesElement = angular.element('<div sahl-canvas><div height="100" width="100" gutter="10" data="data" sahl-lines></div></div>');
            var $scope, canvasScope;
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                $scope.data = [{
                    name: 'test1',
                    values: [{x: -10, y: 2}, {x: 1, y: 30}, {x: 4.5, y: 66}, {x: 7, y: 100}, {x: 7.7, y: 90}, {
                        x: 8,
                        y: 77
                    }, { x: 10, y: 32}, {x: 11, y: 40}, {x: 12, y: 45}, {x: 13, y: 26}, {x: 14, y: 17}, {
                        x: 15,
                        y: 5
                    }, {x: 16, y: 1}],
                    color: "Red"
                }, {
                    name: 'test2',
                    values: [{x: 0, y: 160}, {x: 2, y: 150}, {x: 3, y: 145}, {x: 4, y: 140}, {x: 5, y: 120}, {
                        x: 6,
                        y: 78
                    }, {x: 7, y: 90}, {x: 8, y: 80}, {x: 9, y: 64}, {x: 15, y: -2}, {x: 16, y: -20}],
                    color: "Green"
                }];
            }));

            it('should not throw any errors', function () {
                expect(function () {
                    compileAndDigest(linesElement, $scope);
                }).not.toThrow();
            });
        });
    });
});
