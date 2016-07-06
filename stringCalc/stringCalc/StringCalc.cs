using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stringCalc
{
    class StringCalc
    {
        public int AddNumbers(string Numbers)
        {
            //int num = Convert.ToInt32(Numbers);
            if (Numbers == "")
            {
                return 0;
            }
            else if(Convert.ToInt32(Numbers) == 1)
            {
                return 1;
            }

            int numb1 = Convert.ToInt32(Numbers.Substring(0, 1));
            int numb2 = Convert.ToInt32(Numbers.Substring(1, 1));

            int total = numb1 + numb2;

            return total;

            throw new Exception();
        }
    }
}
