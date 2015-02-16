using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class Calculator
    {

        public double add(double a,double b){ return a+b; }
        
        public double subtract(double a,double b){return a-b;}
        
        public double multiply(double a,double b){return a*b;}

        public double divide(double a,double b)
        {
            if(b!=0)
            return a/b;
            else throw new Exception("Cannot divide by 0");
        }

        public static void main(String[]args)
        {
            Calculator c = new Calculator();
            Console.WriteLine(c.add(1,2));
            Console.WriteLine(c.subtract(1, 2));
            Console.WriteLine(c.multiply(1, 2));
            Console.WriteLine(c.divide(1, 2));
        }

    }
}
