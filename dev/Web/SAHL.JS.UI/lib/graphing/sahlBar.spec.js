'use strict';
describe('[sahl.js.ui.graphing]', function () {
    beforeEach(module('sahl.js.ui.graphing'));
    beforeEach(module('sahl.js.core.arrays'));

    var $compile, $RootScope, $graphMathService, $canvasController, $arrayHelperService;

    beforeEach(inject(function (_$compile_, $rootScope, $injector) {
        $compile = _$compile_;
        $RootScope = $rootScope;
        $graphMathService = $injector.get('$graphMathService');
        $arrayHelperService = $injector.get('$graphMathService');
    }));

    var compileAndDigest = function (target, scope) {
        var $element = $compile(target)(scope);
        $RootScope.$digest();
        return $element.scope();
    }

    describe(' - (Directive: sahlBarChart)-', function () {
        describe('given a bar chart is not inside a canvas', function () {
            var barElement = angular.element('<div sahl-bar></div>');
            var $scope;
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
            }));
            it('should throw an error', function () {
                expect(function () { compileAndDigest(barElement, $scope) }).toThrow();
            });
        });

        describe('given a bar chart is inside a canvas', function () {
            var barElement = angular.element('<div sahl-canvas><div sahl-bar></div></div>');
            var $scope;
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
            }));
            it('should not throw an error', function () {
                expect(function () { compileAndDigest(barElement, $scope) }).not.toThrow();
            });
        });

        describe('given a bar chart has no width and height', function () {
            var barElement = angular.element('<div sahl-canvas><div sahl-bar></div></div>');
            var $scope;
            var controlScope;
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                controlScope = compileAndDigest(barElement, $scope);
            }));
            it('should not set height on scope', function () {
                expect(controlScope.$$childTail.height).toBeUndefined();
            });
            it('should not set width on scope', function () {
                expect(controlScope.$$childTail.width).toBeUndefined();
            });
            it('should not set fullWidth on scope', function () {
                expect(controlScope.$$childTail.fullWidth).toBeUndefined();
            });
            it('should not set fullHeight on scope', function () {
                expect(controlScope.$$childTail.fullHeight).toBeUndefined();
            });
            it('should not set gutter on scope', function () {
                expect(controlScope.$$childTail.gutter).toBeUndefined();
            });
            it('should not set gap on scope', function () {
                expect(controlScope.$$childTail.gap).toBeUndefined();
            });
        });

        describe('given a bar chart has width and height', function () {
            var barElement = angular.element('<div sahl-canvas><div sahl-bar width="10" height="10"></div></div>');
            var $scope;
            var controlScope;
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                controlScope = compileAndDigest(barElement, $scope);
            }));
            it('should set height on scope', function () {
                expect(controlScope.$$childTail.height).not.toBeUndefined();
            });
            it('should set width on scope', function () {
                expect(controlScope.$$childTail.width).not.toBeUndefined();
            });
            it('should set fullWidth on scope', function () {
                expect(controlScope.$$childTail.fullWidth).not.toBeUndefined();
            });
            it('should set fullHeight on scope', function () {
                expect(controlScope.$$childTail.fullHeight).not.toBeUndefined();
            });
            it('should set gutter on scope', function () {
                expect(controlScope.$$childTail.gutter).not.toBeUndefined();
            });
            it('should set gap on scope', function () {
                expect(controlScope.$$childTail.gap).not.toBeUndefined();
            });
        });

        describe('given a bar chart has no data set', function () {
            var barElement = angular.element('<div sahl-canvas><div sahl-bar width="10" height="10"></div></div>');
            var $scope;
            var controlScope;
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                controlScope = compileAndDigest(barElement, $scope);
            }));

            it('should not set the id on scope', function () {
                expect(controlScope.$$childTail.id).toBeUndefined();
            });
        });

        describe('given a bar chart has a data set', function () {
            var barElement = angular.element('<div sahl-canvas><div sahl-bar width="10" height="10" data="data"></div></div>');
            var $scope;
            var controlScope;
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                $scope.data = {};
                controlScope = compileAndDigest(barElement, $scope);
            }));

            it('should not set the id on scope', function () {
                expect(controlScope.$$childTail.id).not.toBeUndefined();
            });
        });

        describe('given a bar chart has a data set', function () {
            var barElement = angular.element('<div sahl-canvas><div sahl-bar width="10" height="10" data="data"></div></div>');
            var $scope;
            var controlScope;
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                $scope.data = [{ name: 'Javascript', value: 40, color: 'Red' },{ name: 'Perl', value: 40, color: 'LavenderBlush' }];
                controlScope = compileAndDigest(barElement, $scope);
            }));

            it('should set the id on scope', function () {
                expect(controlScope.$$childTail.id).not.toBeUndefined();
            });
        });

        describe('given a bar chart has gutter value set', function () {
            var barElement = angular.element('<div sahl-canvas><div sahl-bar width="10" height="10" gutter="10" data="data" ></div></div>');
            var $scope;
            var controlScope;
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                controlScope = compileAndDigest(barElement, $scope);
            }));

            it('should not set gutter on scope', function () {
                expect(controlScope.$$childTail.gutter).toEqual(10);
            });
        });

        describe('given a bar chart has gap value set', function () {
            var barElement = angular.element('<div sahl-canvas><div sahl-bar width="10" height="10" gap="10" data="data" ></div></div>');
            var $scope;
            var controlScope;
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                controlScope = compileAndDigest(barElement, $scope);
            }));

            it('should not set the gap on scope', function () {
                expect(controlScope.$$childTail.gap).toEqual(10);
            });
        });

        describe('given a bar chart is rendered inside Grid', function () {
            var barElement = angular.element('<div sahl-canvas><div sahl-grid showxaxis="true" showyaxis="true"><div sahl-bar width="10" height="10" gap="10" data="data" ></div></div></div>');
            var $scope;
            var controlScope;
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                $scope.data = [{ name: 'Javascript', value: 40, color: 'Red' }, { name: 'Perl', value: 40, color: 'LavenderBlush' }];
                controlScope = compileAndDigest(barElement, $scope);
            }));

            it('should not set the gap on scope', function () {

            });
        });

        describe('given a bar chart is redrawn', function () {
            var barElement = angular.element('<div sahl-canvas><div sahl-bar width="10" height="10" gap="10" data="data" ></div></div>');
            var $scope;
            var controlScope;
            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                $scope.data = [{ name: 'Javascript', value: 40, color: 'Red' }, { name: 'Perl', value: 40, color: 'LavenderBlush' }];
                controlScope = compileAndDigest(barElement, $scope);
                $scope.$$childHead.setCanvas(Raphael($(barElement)[0], 0, 0));
                $scope.$$childHead.draw();
            }));

            it('should not throw errors on digestion', function () {
                $scope.data = [{ name: 'Javascript', value: 10, color: 'Red' }, { name: 'Perl', value: 10, color: 'LavenderBlush' }];
                expect(function () { $scope.$digest(); }).not.toThrow();
            });
        });
    });
});
