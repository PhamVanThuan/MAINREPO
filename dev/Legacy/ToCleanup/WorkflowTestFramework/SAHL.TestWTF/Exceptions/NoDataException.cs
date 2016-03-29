using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.TestWTF.Exceptions
{
    public class NoDataException : Exception
    {
        public NoDataException()
            : base("Test Failed as no data could be found")
        {

        }
    }
}
