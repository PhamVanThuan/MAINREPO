using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute.Core;
using ClassLibrary1;

namespace FirstAppwithTesting
{
    public class when_subtracting_two_numbers
    {
        private static double First;
        private static double Second;
        private static double expectedSubtraction;
        private static double actualResult;
        private static Class1 obj;

        private Establish context = () => //initialize values in establish context
        {
            First = 1;
            Second = 1;
            expectedSubtraction = First - Second;
            actualResult = 0;
            obj = new Class1();
        };

        private Because of = () =>
        {
            actualResult = obj.Subtract(First, Second);
        };

        private It _shouldReturnFirstOpMinusSecondOp = () =>
        {
            actualResult.ShouldEqual(expectedSubtraction);
        };
    }
}


