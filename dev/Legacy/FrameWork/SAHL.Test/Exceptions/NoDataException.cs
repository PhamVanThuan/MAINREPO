using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Test.Exceptions
{
    public class NoDataException : Exception
    {
        public NoDataException()
            : base("Test Failed as no data could be found")
        {

        }
    }
}
