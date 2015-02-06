using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITester
{
    class Program
    {
        
        public int sum(ref int a, ref int b)
        {
            return a + b;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello CI");

            Program obj = new Program();
            string testString = "Created on VS_2013";
            Console.WriteLine(testString);

            int one = 1;
            int two = 2;
            obj.sum(ref one,ref two);       //same as obj.sum(&one,&two) in C++
        }
    }
}
