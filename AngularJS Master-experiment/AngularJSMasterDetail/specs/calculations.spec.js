'use-strict'

describe("loan calculator", function ($scope) {
    it('calculates the divisor of the function using exponents', function (divisor) {
        divisor = 1;
        $scope.Math = window.Math;
        var expectedAnswer = (Math.pow(divisor));
        expect(divisor == [Math.pow(divisor)]);
    });

    it('calculates the total cost of the investment correctly', function () {
        var i = 5.0;
        var p = 1000;
        var n = 12;

        var accumulated = p(1 + i/100 * n);
        expect(accumulated == 12600);
    });

    it('calculates the lowest cost to client', function () {
        var monthlyIncome = 10000;
        var secondMonthlyIncome = 5000;
        var profit = 2000;
        var other = 100;
        var sum = (monthlyIncome + secondMonthlyIncome + profit + other) / 2.5;

        expect(sum == 6840);
    });

    //validation
    it('does not use negative or zero valued parameters for calculations ', function (monthlyIncome, secondMonthlyIncome, profit, other, i, p, n) {
        monthlyIncome = 1;
        secondMonthlyIncome = 1;
        profit = 1;
        i = 0.10;
        p = 1000;
        n = 12;

        expect((monthlyIncome) > (0));
        expect((secondMonthlyIncome)>(0));
        expect((profit)>(0));
        expect((i)>(0));
        expect((p)>(0));
        expect((n)>(0));
    });
});