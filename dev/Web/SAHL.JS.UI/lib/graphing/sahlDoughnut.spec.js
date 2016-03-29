'use strict';
describe('[sahl.js.ui.graphing]', function () {
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

    describe(' - (Directive: sahlDoughnut)-', function () {
        describe('given that a doughnut is not inside of a canvas', function () {
            var doughnutElement = angular.element('<div sahl-doughnut></div>');

            it('should throw an error', function () {
                expect($compile(doughnutElement)).toThrow();
            });
        });

        describe('given that a doughnut is inside of a canvas', function () {
            var $scope;
            var doughnutElement = angular.element('<div sahl-canvas><div sahl-doughnut></div></div>');
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
            }));
            it('should throw error due to no radius', function () {
                expect(function () {
                    compileAndDigest(doughnutElement, $scope);
                }).toThrow('radius is a required field');
            });
        });

        describe('given that a doughnut with x and y set', function () {
            var $scope;
            var doughnutElement = angular.element('<div sahl-canvas><div sahl-doughnut radius="10" x="1" y="1"></div></div>');
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                compileAndDigest(doughnutElement, $scope);
            }));
            it('should set scope x and y values', function () {
                expect($scope.$$childHead.$$nextSibling.x).toEqual(1);
                expect($scope.$$childHead.$$nextSibling.y).toEqual(1);
            });
        });

        describe('given that a doughnut without inner radius set', function () {
            var $scope;
            var doughnutElement = angular.element('<div sahl-canvas><div sahl-doughnut radius="10"></div></div>');
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                compileAndDigest(doughnutElement, $scope);
            }));
            it('should set inner radius on scope to 40% of radius, 4', function () {
                expect($scope.$$childHead.$$nextSibling.innerRadius).toEqual(4);
            });
        });

        describe('given that a doughnut with inner radius set', function () {
            var $scope;
            var doughnutElement = angular.element('<div sahl-canvas><div sahl-doughnut radius="10" innerRadius="1"></div></div>');
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                compileAndDigest(doughnutElement, $scope);
            }));
            it('should set inner radius on scope', function () {
                expect($scope.$$childHead.$$nextSibling.innerRadius).toEqual(1);
            });
        });

        describe('given that a doughnut is inside of a canvas with radius', function () {
            var $scope;
            var doughnutElement = angular.element('<div sahl-canvas><div sahl-doughnut radius="1"></div></div>');
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
            }));
            it('should setup scope variables and radius', function () {
                expect(function () {
                    compileAndDigest(doughnutElement, $scope)
                }).not.toThrow('radius is a required field');

                expect($scope.$$childHead.$$nextSibling.x).toEqual(0.5);
                expect($scope.$$childHead.$$nextSibling.y).toEqual(0.5);
                expect($scope.$$childHead.$$nextSibling.radius).toEqual(1);
            });
        });

        describe('given that a doughnut has now got a dataset with one value', function () {
            var $scope;
            var doughnutElement = angular.element('<div sahl-canvas><div sahl-doughnut radius="10" data="data" ></div></div>');
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                $scope.data = [{name: "Java", value: 40, color: "Navy"}];
            }));
            it('should draw when data and digest is called', function () {
                expect(function () {
                    compileAndDigest(doughnutElement, $scope);
                }).not.toThrow();
                expect($scope.$$childHead.$$nextSibling.x).toEqual(0.5);
                expect($scope.$$childHead.$$nextSibling.y).toEqual(0.5);
                expect($scope.$$childHead.$$nextSibling.radius).toEqual(10);
            });
        });


        describe('given that a doughnut has now got a dataset with one value and changes', function () {
            var $scope;
            var doughnutElement = angular.element('<div sahl-canvas><div sahl-doughnut radius="10" data="data" ></div></div>');
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                $scope.data = [{name: "Java", value: 40, color: "Navy"}];
                compileAndDigest(doughnutElement, $scope);
                $scope.$$childHead.setCanvas(Raphael($(doughnutElement)[0], 0, 0));
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

        describe('given that a doughnut has now got a dataset with multiple values', function () {
            var $scope;
            var doughnutElement = angular.element('<div sahl-canvas><div sahl-doughnut radius="10" data="data" ></div></div>');
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
                    compileAndDigest(doughnutElement, $scope);
                }).not.toThrow();
                expect($scope.$$childHead.$$nextSibling.x).toEqual(0.5);
                expect($scope.$$childHead.$$nextSibling.y).toEqual(0.5);
                expect($scope.$$childHead.$$nextSibling.radius).toEqual(10);
            });
        });


        describe('given that a doughnut has now got a dataset with multiple values and changes', function () {
            var $scope;
            var doughnutElement = angular.element('<div sahl-canvas><div sahl-doughnut radius="10" data="data" ></div></div>');
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                $scope.data = [{name: "Java", value: 40, color: "Navy"}, {
                    name: "Java",
                    value: 40,
                    color: "White"
                }, {name: "Java", value: 40, color: "Green"}];
                compileAndDigest(doughnutElement, $scope);
                $scope.$$childHead.setCanvas(Raphael($(doughnutElement)[0], 0, 0));
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
