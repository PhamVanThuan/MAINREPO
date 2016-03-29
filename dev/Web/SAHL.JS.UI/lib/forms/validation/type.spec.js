'use strict';
describe('[sahl.js.ui.forms.validation]', function () {
    beforeEach(module('sahl.js.ui.forms.validation'));

    var $compile, $RootScope;
    beforeEach(inject(function (_$compile_, $rootScope) {
        $compile = _$compile_;
        $RootScope = $rootScope;
    }));

    var compileAndDigest = function (target, scope) {
        var $element = $compile(target)(scope);
        $RootScope.$digest();
        return $element.scope();
    }

    describe(' - (Directive: type)-', function () {
        describe('given that the type is set to number', function () {
            var $scope;
            var inputElement = angular.element(
                '<form name="testform">'
                + '<input sahl-type="number" name="testinput" ng-model="test"/>'
                + '</form>'
            );

            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                compileAndDigest(inputElement, $scope);
            }));

            describe('and input is a number', function () {
                beforeEach(inject(function () {
                    $scope.testform.testinput.$setViewValue(0);
                    $scope.$digest();
                }));

                it('form should be valid', function () {
                    expect($scope.testform.$valid).toBe(true);
                });
            });
            describe('and input is a word', function () {
                beforeEach(inject(function () {
                    $scope.testform.testinput.$setViewValue('test');
                    $scope.$digest();
                }));

                it('form should be invalid', function () {
                    expect($scope.testform.$valid).toBe(false);
                });
            });
        });

        describe('given that the type is set to string', function () {
            var $scope;
            var inputElement = angular.element(
                '<form name="testform">'
                + '<input sahl-type="string" name="testinput" ng-model="test"/>'
                + '</form>'
            );

            beforeEach(inject(function () {
                $scope = $RootScope.$new();
                compileAndDigest(inputElement, $scope);
            }));

            describe('and input is a number', function () {
                beforeEach(inject(function () {
                    $scope.testform.testinput.$setViewValue(0);
                    $scope.$digest();
                }));

                it('form should be valid', function () {
                    expect($scope.testform.$valid).toBe(true);
                });
            });
            describe('and input is a word', function () {
                beforeEach(inject(function () {
                    $scope.testform.testinput.$setViewValue('test');
                    $scope.$digest();
                }));

                it('form should be valid', function () {
                    expect($scope.testform.$valid).toBe(true);
                });
            });
        });
    });
});