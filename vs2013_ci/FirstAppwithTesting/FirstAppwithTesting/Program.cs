using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstAppwithTesting
{
    public class Program : InterfaceToTest
    {
        public double First { get; set; }

        private double Second { get; set; }
        
        static void Main(string[] args)
        {
        }

        public double Add(double firstOp, double secondOp) => firstOp + secondOp;

        public double Subtract(double firstOp, double secondOp)
        {
            return firstOp - secondOp;
        }
    }
}
