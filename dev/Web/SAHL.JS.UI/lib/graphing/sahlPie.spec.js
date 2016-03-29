'use strict';
describe('[sahl.js.ui.graphing]', function () {
    beforeEach(module('sahl.js.ui.graphing'));
    var $compile, $RootScope, $graphMathService, $canvasController;

    beforeEach(inject(function (_$compile_, $rootScope, $injector) {
        $compile = _$compile_;
        $RootScope = $rootScope;
        $graphMathService = $injector.get('$graphMathService');
    }));

    var compileAndDigest = function (target, scope, postDigestFn) {
        var $element = $compile(target)(scope);
        postDigestFn && scope.$$postDigest(postDigestFn);
        $RootScope.$digest();
        return $element.scope();
    };

    describe(' - (Directive: sahlPie)-', function () {
        describe('given that a pie is not inside of a canvas', function () {
            var pieElement = angular.element('<div sahl-pie></div>');

            it('should throw an error', function () {
                expect($compile(pieElement)).toThrow();
            });
        });

        describe('given that a pie is inside of a canvas', function () {
            var $scope;
            var pieElement = angular.element('<div sahl-canvas><div sahl-pie></div></div>');
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
            }));
            it('should throw error due to no radius', function () {
                expect(function () {
                    compileAndDigest(pieElement, $scope);
                }).toThrow();
            });
        });

        describe('given that a pie with x and y set', function () {
            var $scope;
            var pieElement = angular.element('<div sahl-canvas><div sahl-pie radius="1" x="1" y="1"></div></div>');
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                compileAndDigest(pieElement, $scope);
            }));
            it('should set scope x and y values', function () {
                expect($scope.$$childHead.$$nextSibling.x).toEqual(1);
                expect($scope.$$childHead.$$nextSibling.y).toEqual(1);
            });
        });

        describe('given that a pie is inside of a canvas with radius', function () {
            var $scope;
            var pieElement = angular.element('<div sahl-canvas><div sahl-pie radius="1"></div></div>');
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
            }));
            it('should setup scope variables and radius', function () {
                expect(function () {
                    compileAndDigest(pieElement, $scope);
                }).not.toThrow('radius is a required field');

                expect($scope.$$childHead.$$nextSibling.x).toEqual(0.5);
                expect($scope.$$childHead.$$nextSibling.y).toEqual(0.5);
                expect($scope.$$childHead.$$nextSibling.radius).toEqual(1);
            });
        });

        describe('given that a pie has now got a dataset with one value', function () {
            var $scope;
            var pieElement = angular.element('<div sahl-canvas><div sahl-pie radius="10" data="data" ></div></div>');
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                $scope.data = [{name: "Java", value: 40, color: "Navy"}];
            }));
            it('should draw when data and digest is called', function () {
                expect(function () {
                    compileAndDigest(pieElement, $scope);
                }).not.toThrow();
                expect($scope.$$childHead.$$nextSibling.x).toEqual(0.5);
                expect($scope.$$childHead.$$nextSibling.y).toEqual(0.5);
                expect($scope.$$childHead.$$nextSibling.radius).toEqual(10);
            });
        });


        describe('given that a pie has now got a dataset with one value and changes', function () {
            var $scope;
            var pieElement = angular.element('<div sahl-canvas><div sahl-pie radius="10" data="data" ></div></div>');
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                $scope.data = [{name: "Java", value: 40, color: "Navy"}];
                compileAndDigest(pieElement, $scope);
                $scope.$$childHead.setCanvas(Raphael($(pieElement)[0], 0, 0));
                $scope.$$childHead.draw();
            }));
            afterEach(function () {
                $scope.data = [{name: "Java", value: 50, color: "Navy"}];
                $scope.$digest();
            });
            it('should redraw when data and digest is called', function () {
                expect($scope.$$childHead.$$nextSibling.x).toEqual(0.5);
                expect($scope.$$childHead.$$nextSibling.y).toEqual(0.5);
                expect($scope.$$childHead.$$nextSibling.radius).toEqual(10);
                expect($scope.$$childHead.$$nextSibling.id).toEqual(0);
            });
        });

        describe('given that a pie has now got a dataset with multiple values', function () {
            var $scope;
            var pieElement = angular.element('<div sahl-canvas><div sahl-pie radius="10" data="data" ></div></div>');
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                $scope.data = [{name: "Java", value: 40, color: "Navy"}, {
                    name: "Java",
                    value: 40,
                    color: "White"
                }, {name: "Java", value: 40, color: "Green"}];
            }));
            it('should draw when data and digest is called', function () {
                expect(function () {
                    compileAndDigest(pieElement, $scope);
                }).not.toThrow();
                expect($scope.$$childHead.$$nextSibling.x).toEqual(0.5);
                expect($scope.$$childHead.$$nextSibling.y).toEqual(0.5);
                expect($scope.$$childHead.$$nextSibling.radius).toEqual(10);
            });
        });


        describe('given that a pie has now got a dataset with multiple values and changes', function () {
            var $scope;
            var pieElement = angular.element('<div sahl-canvas><div sahl-pie radius="10" data="data" ></div></div>');
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                $scope.data = [{name: "Java", value: 40, color: "Navy"}, {
                    name: "Java",
                    value: 40,
                    color: "White"
                }, {name: "Java", value: 40, color: "Green"}];
                compileAndDigest(pieElement, $scope);
                $scope.$$childHead.setCanvas(Raphael($(pieElement)[0], 0, 0));
                $scope.$$childHead.draw();
            }));
            it('should redraw when data and digest is called', function () {
                expect($scope.$$childHead.$$nextSibling.x).toEqual(0.5);
                expect($scope.$$childHead.$$nextSibling.y).toEqual(0.5);
                expect($scope.$$childHead.$$nextSibling.radius).toEqual(10);
                expect($scope.$$childHead.$$nextSibling.id).toEqual(0);

                $scope.data = [{name: "Java", value: 50, color: "Navy"}, {
                    name: "Java",
                    value: 50,
                    color: "White"
                }, {name: "Java", value: 40, color: "Green"}];
                $scope.$digest();
            });
        });
    });
});