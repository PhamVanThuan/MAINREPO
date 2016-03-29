'use strict';
describe('[sahl.js.ui.graphing]', function () {
    beforeEach(module('sahl.js.ui.graphing'));
    var $graphMathService;
    beforeEach(inject(function ($injector, $q) {
        $graphMathService = $injector.get("$graphMathService");
    }));

    describe(' - (Service: graphMath)-', function () {
        describe('when getting section of a doughnut', function () {
            describe('when using zero values', function () {
                var result;
                var expected = ['M', 0, 0, 'L', 0, 0, 'A', 0, 0, 0, 0, 0, 0, 0, 'L', 0, 0, 'A', 0, 0, 0, 0, 1, 0, 0, 'z'];
                beforeEach(function () {
                    result = $graphMathService.getDoughnutSectionPath(0, 0, 0, 0, 0, 0);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using zero inner radius at 90 degrees', function () {
                var result;
                var expected = ['M', 0, 0, 'L', 10, 0, 'A', 10, 10, 0, 0, 0, 6.123233995736766e-16, -10, 'L', 0, 0, 'A', 0, 0, 0, 0, 1, 0, 0, 'z'];
                beforeEach(function () {
                    result = $graphMathService.getDoughnutSectionPath(0, 0, 10, 0, 0, 90);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when moving the center of the dougnut', function () {
                var result;
                var expected = ['M', 10, 10, 'L', 10, 10, 'A', 0, 0, 0, 0, 0, 10, 10, 'L', 10, 10, 'A', 0, 0, 0, 0, 1, 10, 10, 'z'];
                beforeEach(function () {
                    result = $graphMathService.getDoughnutSectionPath(10, 10, 0, 0, 0, 0);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });
        });

        describe('when getting section of a pie', function () {
            describe('when using zero values', function () {
                var result;
                var expected = ['M', 0, 0, 'L', 0, 0, 'A', 0, 0, 0, 0, 0, 0, 0, 'L', 0, 0, 'z'];
                beforeEach(function () {
                    result = $graphMathService.getPieSectionPath(0, 0, 0, 0, 0);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using a radius of 10 at 90 degrees', function () {
                var result;
                var expected = ['M', 0, 0, 'L', 10, 0, 'A', 10, 10, 0, 0, 0, 6.123233995736766e-16, -10, 'L', 0, 0, 'z'];
                beforeEach(function () {
                    result = $graphMathService.getPieSectionPath(0, 0, 10, 0, 90);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when setting the x and y position', function () {
                var result;
                var expected = ['M', 10, 10, 'L', 10, 10, 'A', 0, 0, 0, 0, 0, 10, 10, 'L', 10, 10, 'z'];
                beforeEach(function () {
                    result = $graphMathService.getPieSectionPath(10, 10, 0, 0, 0);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });
        });

        describe('when getting curve points', function () {
            describe('when using ascending values', function () {
                var result;
                var expected = { previous: { x: 1, y: 2 }, next: { x: 1.1913417161825448, y: 2.4619397662556435 } };
                beforeEach(function () {
                    result = $graphMathService.getCurvePoints({ x: 1, y: 1 }, { x: 1, y: 2 }, { x: 2, y: 3 });
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using descending values', function () {
                var result;
                var expected = { previous: { x: 1, y: 2 }, next: { x: 1.191341716182545, y: 1.5380602337443565 } };
                beforeEach(function () {
                    result = $graphMathService.getCurvePoints({ x: 1, y: 3 }, { x: 1, y: 2 }, { x: 2, y: 1 });
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });
        });

        describe('when getting relative point', function () {
            describe('when using zero values', function () {
                var result;
                var expected = { 'x': 0, 'y': 0 };
                beforeEach(function () {
                    result = $graphMathService.getPoint(0, 0, 0, 0, 0, 0, 0);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using using absolute values', function () {
                var result;
                var expected = { 'x': 1, 'y': -1 };
                beforeEach(function () {
                    result = $graphMathService.getPoint(1, 1, 1, 1, 0, 0, 0);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });
        });

        describe('when getting relative start point', function () {
            describe('when using zero values', function () {
                var result;
                var expected = ['M', 0, 0, 'L'];
                beforeEach(function () {
                    result = $graphMathService.getStartingPoint({ 'x': 0, 'y': 0 }, false);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using zero values and uses curves in the lines', function () {
                var result;
                var expected = ['M', 0, 0, 'C',0,0];
                beforeEach(function () {
                    result = $graphMathService.getStartingPoint({ 'x': 0, 'y': 0 }, true);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });
        });

        describe('when getting relative end point', function () {
            describe('when using zero values', function () {
                var result;
                var expected = [0, 0, 0, 0];
                beforeEach(function () {
                    result = $graphMathService.getEndPoint({ 'x': 0, 'y': 0 }, false);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using absolute values', function () {
                var result;
                var expected = [1, 2, 1, 2];
                beforeEach(function () {
                    result = $graphMathService.getEndPoint({ 'x': 1, 'y': 2 }, false);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });
        });

        describe('when getting line to points', function () {
            describe('when using ascending values without a curve', function () {
                var result;
                var expected = [1, 2];
                beforeEach(function () {
                    result = $graphMathService.lineTo({ x: 1, y: 1 }, { x: 1, y: 2 }, { x: 2, y: 3 }, false);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using ascending values with curve', function () {
                var result;
                var expected = [1, 2, 1, 2, 1.1913417161825448, 2.4619397662556435];
                beforeEach(function () {
                    result = $graphMathService.lineTo({ x: 1, y: 1 }, { x: 1, y: 2 }, { x: 2, y: 3 }, true);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });
        });

        describe('when getting horizontal line', function () {
            describe('when using zero values', function () {
                var result;
                var expected = ['M', 0, 0, 'L', 0, 0, 'z'];;
                beforeEach(function () {
                    result = $graphMathService.horizontalLine(0,0,0);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using zero values and positionY', function () {
                var result;
                var expected = ['M', 0, 1, 'L', 0, 1, 'z'];;
                beforeEach(function () {
                    result = $graphMathService.horizontalLine(1, 0, 0);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using zero values and width', function () {
                var result;
                var expected = ['M', 0, 0, 'L', 1, 0, 'z'];;
                beforeEach(function () {
                    result = $graphMathService.horizontalLine(0, 1, 0);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using zero values and gutter', function () {
                var result;
                var expected = ['M', 1, 0, 'L', -1, 0, 'z'];;
                beforeEach(function () {
                    result = $graphMathService.horizontalLine(0, 0, 1);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using absolute value of 1', function () {
                var result;
                var expected = ['M', 1, 1, 'L', 0, 1, 'z'];;
                beforeEach(function () {
                    result = $graphMathService.horizontalLine(1, 1, 1);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });
        });

        describe('when getting vertical line', function () {
            describe('when using zero values', function () {
                var result;
                var expected = ['M', 0, 0, 'L', 0, 0, 'z'];
                beforeEach(function () {
                    result = $graphMathService.verticalLine(0, 0, 0);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using zero values and positionX', function () {
                var result;
                var expected = ['M', 1, 0, 'L', 1, 0, 'z'];
                beforeEach(function () {
                    result = $graphMathService.verticalLine(1, 0, 0);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using zero values and height', function () {
                var result;
                var expected = ['M', 0, 0, 'L', 0, 1, 'z'];
                beforeEach(function () {
                    result = $graphMathService.verticalLine(0, 1, 0);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using zero values and gutter', function () {
                var result;
                var expected = ['M', 0, 1, 'L', 0,-1, 'z'];
                beforeEach(function () {
                    result = $graphMathService.verticalLine(0, 0, 1);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using absolute value of 1', function () {
                var result;
                var expected = ['M', 1, 1, 'L', 1, 0, 'z'];
                beforeEach(function () {
                    result = $graphMathService.verticalLine(1, 1, 1);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });
        });

        describe('when getting points for a bar', function () {
            describe('when using absolute zero values', function () {
                var result;
                var expected = ['M', 0, 0, 'L', 0, 0, 'L', 0, 0, 'L', 0, 0, 'L', 0, 0, 'z'];
                beforeEach(function () {
                    result = $graphMathService.getBar({ x: 0, y: 0 }, { x: 0, y: 0 }, 0);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using absolute 1 as bar value', function () {
                var result;
                var expected = ['M', 0, 0, 'L', 0, 1, 'L', 0, 1, 'L', 0, 0, 'L', 0, 0, 'z'];
                beforeEach(function () {
                    result = $graphMathService.getBar({ x: 0, y: 1 }, { x: 0, y: 0 }, 0);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using absolute 1 as bar width', function () {
                var result;
                var expected = ['M', 0, 0, 'L', 0, 0, 'L', 1, 0, 'L', 1, 0, 'L', 0, 0, 'z'];
                beforeEach(function () {
                    result = $graphMathService.getBar({ x: 0, y: 0 }, { x: 0, y: 0 }, 1);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });
        });

        describe('when getting points for a popup path', function () {
            describe('when using absolute zero values', function () {
                var result;
                var expected = ['M', 0, 0, 'L', 0, 0, 'L', 0, 0, 'L', 0, 0, 'L', 0, 1, 'L', 0, 1, 'L', 0, 1, 'z'];
                beforeEach(function () {
                    result = $graphMathService.getPopupPath(0,0,0,0,0,0,true,true);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using position of 1,1', function () {
                var result;
                var expected = ['M', 1, 1, 'L', 1, 1, 'L', 1, 1, 'L', 1, 1, 'L', 1, 2, 'L', 1, 2, 'L', 1, 2, 'z'];
                beforeEach(function () {
                    result = $graphMathService.getPopupPath(1, 1, 0, 0, 0, 0, true, true);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using width of 1,1', function () {
                var result;
                var expected = ['M', 0, 0, 'L', 1, 0, 'L', 1, 1, 'L', 0, 1, 'L', 0, 1, 'L', 0, 1, 'L', 0, 1, 'z'];
                beforeEach(function () {
                    result = $graphMathService.getPopupPath(0, 0, 1, 1, 0, 0, true, true);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using padding of 1', function () {
                var result;
                var expected = ['M', -1, -1, 'L', 1, -1, 'L', 1, 1, 'L', -1, 1, 'L', -1, 1, 'L', -1, 1, 'L', -1, 1, 'z'];
                beforeEach(function () {
                    result = $graphMathService.getPopupPath(0, 0, 0, 0, 1, 0, true, true);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using padding of 1', function () {
                var result;
                var expected = ['M', 0, 0, 'L', 0, 0, 'L', 0, 0, 'L', 0, 0, 'L', 0, 3, 'L', -1, 2, 'L', 0, 1, 'z'];
                beforeEach(function () {
                    result = $graphMathService.getPopupPath(0, 0, 0, 0, 0, 1, true, true);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using possible text size where tag is to top left', function () {
                var result;
                var expected = ['M', 94, 94, 'L', 206, 94, 'L', 206, 206, 'L', 94, 206, 'L', 94, 103, 'L', 93, 102, 'L', 94, 101, 'z'];
                beforeEach(function () {
                    result = $graphMathService.getPopupPath(100, 100, 100, 100, 6, 1, true, true);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using possible text size where tag is to top right', function () {
                var result;
                var expected = ['M', 94, 94, 'L', 206, 94, 'L', 206, 101, 'L', 207, 102, 'L', 206, 103, 'L', 206, 206, 'L', 94, 206, 'z'];
                beforeEach(function () {
                    result = $graphMathService.getPopupPath(100, 100, 100, 100, 6, 1, false, true);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using possible text size where tag is to bottom left', function () {
                var result;
                var expected = ['M', 94, 94, 'L', 206, 94, 'L', 206, 206, 'L', 94, 206, 'L', 94, 199, 'L', 93, 198, 'L', 94, 197, 'z'];
                beforeEach(function () {
                    result = $graphMathService.getPopupPath(100, 100, 100, 100, 6, 1, true, false);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });

            describe('when using possible text size where tag is to bottom right', function () {
                var result;
                var expected = ['M', 94, 94, 'L', 206, 94, 'L', 206, 197, 'L', 207, 198, 'L', 206, 199, 'L', 206, 206, 'L', 94, 206, 'z'];
                beforeEach(function () {
                    result = $graphMathService.getPopupPath(100, 100, 100, 100, 6, 1, false, false);
                });

                it('should return expected array ', function () {
                    expect(result).toEqual(expected);
                });
            });
        });
    });
});
