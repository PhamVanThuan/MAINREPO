describe("loan calculator", function () {
    it('calculates the divisor of the function using exponents', function (divisor) {
        var expectedAnswer = (Math.pow(divisor));
        expect(divisor).toEqual([Math.pow(divisor)]);
    });

    it('calculates the total cost of the investment correctly', function () {
        var i = 5.0;
        var p = 1000;
        var n = 12;

        var accumulated = p(1 + i * n);
        expect(accumulated).toEqual(1600);
    });

    it('calculates possible expenditure', function () {
        var monthlyIncome = 10000;
        var secondMonthlyIncome = 5000;
        var profit = 2000;
        var other = 1000;
        var sum = (monthlyIncome + secondMonthlyIncome + profit + other) / 2.5;

        expect(sum).toEqual(7200);
    });

    it('does not use negative or zero valued parameters for calculations ', function (monthlyIncome, secondMonthlyIncome, profit, other, i, p, n) {
        expect(monthlyIncome).GreaterThan(0);
        expect(secondMonthlyIncome).GreaterThan(0);
        expect(profit).GreaterThan(0);
        expect(i).GreaterThan(0);
        expect(p).GreaterThan(0);
        expect(n).GreaterThan(0);
    });


});