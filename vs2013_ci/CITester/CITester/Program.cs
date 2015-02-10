using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITester
{
    public class Program
    {
        
        public int sum(ref int a, ref int b)    //receives addresses (like pointers)
        {
            return a + b;
        }

        public int sum(int a, int b)
        {
            return a + b;
        }
        

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello CI");
            const  string testString = "Created on VS_2013";
            Console.WriteLine(testString);
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Enter name to search for ");
            string n = Console.ReadLine();


            Person p1 = new Person("Vishav Premlall", 22,5);
            p1.SayHi();
            p1.Display();
        }
    }
}
