using System;


namespace Calculator {
    public class CalculatorExec
    {

        public static void main(String[] args)
        {
            Calculator c = new Calculator();
            Console.WriteLine(c.add(1, 2));
            Console.WriteLine(c.subtract(1, 2));
            Console.WriteLine(c.multiply(1, 2));
            Console.WriteLine(c.divide(1, 2));
        }
    }	
}
