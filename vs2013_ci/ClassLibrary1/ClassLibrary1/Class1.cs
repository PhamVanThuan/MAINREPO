using FirstAppwithTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Class1 : InterfaceToTest
    {
        public double First { get; set; }

        private double Second { get; set; }


        public double Add(double firstOp, double secondOp)
        {
            return firstOp + secondOp;
        }

        public double Subtract(double firstOp, double secondOp)
        {
            return firstOp - secondOp;
        }

        public double Divide(double firstOp, double secondOp)
        {
            if (secondOp != 0)
                return firstOp / secondOp;
            else throw new Exception("Cannot divide by 0");
        }

        public double Multiply(double FirstOp, double SecondOp)
        {
            return FirstOp * SecondOp;
        }

    }
}
