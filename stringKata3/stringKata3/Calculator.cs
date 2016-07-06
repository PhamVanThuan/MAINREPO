using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stringKata3
{
    class Calculator
    {
        public int Add(string input)
        {
            if (input == "") return 0;
            int total = 0;

            //get new delimiter, split by it


            input = input.Replace("\n",",");
            string[] numbers = input.Split(',');
            foreach (var number in numbers) 
            {
                int n = Convert.ToInt32(number);
                total += n;
               
            }
            return total;
        }
    }
}