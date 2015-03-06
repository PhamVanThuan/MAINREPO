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
            
        }
    }
}
