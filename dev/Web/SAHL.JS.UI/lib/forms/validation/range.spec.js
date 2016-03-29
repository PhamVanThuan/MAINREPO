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
    };

    describe(' - (Directive: range) - ', function () {
        describe('given that the range "from" and "to" are set', function () {
            describe('to infinity', function () {
                var $scope;
                var inputElement = angular.element(
                    '<form name="testform">' +
                    '<input sahl-range="from:*,to:*" name="testinput" ng-model="test"/>' +
                    '</form>'
                );

                beforeEach(inject(function () {
                    $scope = $RootScope.$new();
                    compileAndDigest(inputElement, $scope);
                }));

                describe('with input of 0', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(0);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });

                describe('with input of -100', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(-100);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });

                describe('with input of 100', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(100);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });
            });

            describe('to finite values of -10,10', function () {
                var $scope;
                var inputElement = angular.element(
                    '<form name="testform">' +
                    '<input sahl-range="from:-10,to:10" name="testinput" ng-model="test"/>' +
                    '</form>'
                );

                beforeEach(inject(function () {
                    $scope = $RootScope.$new();
                    compileAndDigest(inputElement, $scope);
                }));

                describe('with input of 0', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(0);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });

                describe('with input of -5', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(-5);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });

                describe('with input of 5', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(5);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });

                describe('with input of -100', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(-100);
                        $scope.$digest();
                    }));

                    it('form should be invalid', function () {
                        expect($scope.testform.$valid).toBe(false);
                    });
                });

                describe('with input of 100', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(100);
                        $scope.$digest();
                    }));

                    it('form should be invalid', function () {
                        expect($scope.testform.$valid).toBe(false);
                    });
                });
            });

            describe('to invalid values of -10,A', function () {
                var $scope;
                var inputElement = angular.element(
                    '<form name="testform">'
                    + '<input sahl-range="from:-10,to:A" name="testinput" ng-model="test"/>'
                    + '</form>'
                );

                beforeEach(inject(function () {
                    $scope = $RootScope.$new();
                }));

                it('should throw error', function () {
                    expect(function () {
                        compileAndDigest(inputElement, $scope);
                    }).toThrow();
                });
            });

            describe('to invalid values of A,A', function () {
                var $scope;
                var inputElement = angular.element(
                    '<form name="testform">'
                    + '<input sahl-range="from:A,to:A" name="testinput" ng-model="test"/>'
                    + '</form>'
                );

                beforeEach(inject(function () {
                    $scope = $RootScope.$new();
                }));

                it('should throw error', function () {
                    expect(function () {
                        compileAndDigest(inputElement, $scope);
                    }).toThrow();
                });
            });

            describe('to invalid values of A,10', function () {
                var $scope;
                var inputElement = angular.element(
                    '<form name="testform">'
                    + '<input sahl-range="from:A,to:10" name="testinput" ng-model="test"/>'
                    + '</form>'
                );

                beforeEach(inject(function () {
                    $scope = $RootScope.$new();
                }));

                it('should throw error', function () {
                    expect(function () {
                        compileAndDigest(inputElement, $scope);
                    }).toThrow();
                });
            });
        });

        describe('given that the range "from" is set', function () {
            describe('to infinity', function () {
                var $scope;
                var inputElement = angular.element(
                    '<form name="testform">'
                    + '<input sahl-range="from:*" name="testinput" ng-model="test"/>'
                    + '</form>'
                );

                beforeEach(inject(function () {
                    $scope = $RootScope.$new();
                    compileAndDigest(inputElement, $scope);
                }));

                describe('to a negative number', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(-Infinity);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });

                describe('to 0', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(0);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });

                describe('to a positive number', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(Infinity);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });
            });
            describe('to finite value of -10', function () {
                var $scope;
                var inputElement = angular.element(
                    '<form name="testform">'
                    + '<input sahl-range="from:-10" name="testinput" ng-model="test"/>'
                    + '</form>'
                );

                beforeEach(inject(function () {
                    $scope = $RootScope.$new();
                    compileAndDigest(inputElement, $scope);
                }));

                describe('to a negative number', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(-Infinity);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(false);
                    });
                });

                describe('to a negative number', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(-1);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });

                describe('to 0', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(0);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });

                describe('to a positive number', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(Infinity);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });
            });
            describe('to finite value of 10', function () {
                var $scope;
                var inputElement = angular.element(
                    '<form name="testform">'
                    + '<input sahl-range="from:10" name="testinput" ng-model="test"/>'
                    + '</form>'
                );

                beforeEach(inject(function () {
                    $scope = $RootScope.$new();
                    compileAndDigest(inputElement, $scope);
                }));

                describe('to a negative number', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(-Infinity);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(false);
                    });
                });

                describe('to a negative number', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(-1);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(false);
                    });
                });

                describe('to 0', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(0);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(false);
                    });
                });

                describe('to 20', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(20);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });

                describe('to a positive number', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(Infinity);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });
            });
            describe('to invalid value of A', function () {
                var $scope;
                var inputElement = angular.element(
                    '<form name="testform">'
                    + '<input sahl-range="from:A name="testinput" ng-model="test"/>'
                    + '</form>'
                );

                beforeEach(inject(function () {
                    $scope = $RootScope.$new();
                }));

                it('should throw error', function () {
                    expect(function () {
                        compileAndDigest(inputElement, $scope);
                    }).toThrow();
                });
            });
        });

        describe('given that the range "to" is set', function () {
            describe('to infinity', function () {
                var $scope;
                var inputElement = angular.element(
                    '<form name="testform">'
                    + '<input sahl-range="to:*" name="testinput" ng-model="test"/>'
                    + '</form>'
                );

                beforeEach(inject(function () {
                    $scope = $RootScope.$new();
                    compileAndDigest(inputElement, $scope);
                }));

                describe('to a negative number', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(-Infinity);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });

                describe('to 0', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(0);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });

                describe('to a positive number', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(Infinity);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });
            });
            describe('to finite values of -10', function () {
                var $scope;
                var inputElement = angular.element(
                    '<form name="testform">'
                    + '<input sahl-range="to:-10" name="testinput" ng-model="test"/>'
                    + '</form>'
                );

                beforeEach(inject(function () {
                    $scope = $RootScope.$new();
                    compileAndDigest(inputElement, $scope);
                }));

                describe('to a negative number', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(-Infinity);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });

                describe('to 0', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(-20);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });

                describe('to a positive number', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(0);
                        $scope.$digest();
                    }));

                    it('form should be invalid', function () {
                        expect($scope.testform.$valid).toBe(false);
                    });
                });

                describe('to a positive number', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(Infinity);
                        $scope.$digest();
                    }));

                    it('form should be invalid', function () {
                        expect($scope.testform.$valid).toBe(false);
                    });
                });
            });
            describe('to finite values of 10', function () {
                var $scope;
                var inputElement = angular.element(
                    '<form name="testform">'
                    + '<input sahl-range="to:10" name="testinput" ng-model="test"/>'
                    + '</form>'
                );

                beforeEach(inject(function () {
                    $scope = $RootScope.$new();
                    compileAndDigest(inputElement, $scope);
                }));

                describe('to a negative number', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(-Infinity);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });

                describe('to a positive number', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(0);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });

                describe('to a positive number', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(5);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });

                describe('to a positive number', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(10);
                        $scope.$digest();
                    }));

                    it('form should be valid', function () {
                        expect($scope.testform.$valid).toBe(true);
                    });
                });

                describe('to a positive number', function () {
                    beforeEach(inject(function () {
                        $scope.testform.testinput.$setViewValue(Infinity);
                        $scope.$digest();
                    }));

                    it('form should be invalid', function () {
                        expect($scope.testform.$valid).toBe(false);
                    });
                });
            });
            describe('to invalid value of A', function () {
                var $scope;
                var inputElement = angular.element(
                    '<form name="testform">'
                    + '<input sahl-range="to:A name="testinput" ng-model="test"/>'
                    + '</form>'
                );

                beforeEach(inject(function () {
                    $scope = $RootScope.$new();
                }));

                it('should throw error', function () {
                    expect(function () {
                        compileAndDigest(inputElement, $scope);
                    }).toThrow();
                });
            });
        });
    });
});