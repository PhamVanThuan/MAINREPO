'use strict';
describe('[sahl.js.core.arrays]', function () {
    beforeEach(module('sahl.js.core.arrays'));
    var $arrayHelperService;
    beforeEach(inject(function ($injector, $q) {
        $arrayHelperService = $injector.get('$arrayHelperService');
    }));

    describe(' - (Service: arrayHelper)-', function () {
        describe('when getting the min value of an array', function () {
            describe('array is undefined', function () {
                it('should not throw error', function () {
                    expect(function () {
                        $arrayHelperService.min(undefined, undefined);
                    }).not.toThrow();
                });

                it('should return undefined', function () {
                    var result = $arrayHelperService.min(undefined, undefined);
                    expect(result).toEqual(undefined);
                });
            });

            describe('selector is undefined', function () {
                var array = [];
                it('should not throw error', function () {
                    expect(function () {
                        $arrayHelperService.min(array, undefined);
                    }).not.toThrow();
                });

                it('should return undefined', function () {
                    var result = $arrayHelperService.min(array, undefined);
                    expect(result).toEqual(undefined);
                });
            });

            describe('array is empty', function () {
                var selector = function (item) {
                    return item;
                };

                var array = [];
                it('should not throw error', function () {
                    expect(function () {
                        $arrayHelperService.min(array, selector);
                    }).not.toThrow();
                });

                it('should return undefined', function () {
                    var result = $arrayHelperService.min(array, selector);
                    expect(result).toEqual(undefined);
                });
            });

            describe('array is just number array', function () {
                var array = [1, 2, 3, 4, 5, 6, 7, 8, 9];
                var selector = function (item) {
                    return item;
                };

                it('should not throw error', function () {
                    expect(function () {
                        $arrayHelperService.min(array, selector);
                    }).not.toThrow();
                });

                it('should return undefined', function () {
                    var result = $arrayHelperService.min(array, selector);
                    expect(result).toEqual(1);
                });
            });
        });

        describe('when getting the max value of an array', function () {
            describe('when getting the min value of an array', function () {
                describe('array is undefined', function () {
                    it('should not throw error', function () {
                        expect(function () {
                            $arrayHelperService.max(undefined, undefined);
                        }).not.toThrow();
                    });

                    it('should return undefined', function () {
                        var result = $arrayHelperService.max(undefined, undefined);
                        expect(result).toEqual(undefined);
                    });
                });

                describe('selector is undefined', function () {
                    var array = [];
                    it('should not throw error', function () {
                        expect(function () {
                            $arrayHelperService.max(array, undefined);
                        }).not.toThrow();
                    });

                    it('should return undefined', function () {
                        var result = $arrayHelperService.max(array, undefined);
                        expect(result).toEqual(undefined);
                    });
                });

                describe('array is empty', function () {
                    var selector = function (item) {
                        return item;
                    };

                    var array = [];
                    it('should not throw error', function () {
                        expect(function () {
                            $arrayHelperService.max(array, selector);
                        }).not.toThrow();
                    });

                    it('should return undefined', function () {
                        var result = $arrayHelperService.max(array, selector);
                        expect(result).toEqual(undefined);
                    });
                });

                describe('array is just number array', function () {
                    var array = [1, 2, 3, 4, 5, 6, 7, 8, 9];
                    var selector = function (item) {
                        return item;
                    };

                    it('should not throw error', function () {
                        expect(function () {
                            $arrayHelperService.max(array, selector);
                        }).not.toThrow();
                    });

                    it('should return undefined', function () {
                        var result = $arrayHelperService.max(array, selector);
                        expect(result).toEqual(9);
                    });
                });
            });
        });
    });
});