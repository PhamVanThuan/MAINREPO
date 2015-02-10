using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute.Core;

namespace FirstAppwithTesting
{
    public class when_adding_two_numbers
    {
        private static double First;
        private static double Second;
        private static double expectedAddition;
        private static double actualResult;
        private static Program obj;

        private Establish context = () => //initialize values in establish context
        {
            First = 1;
            Second = 1;
            expectedAddition = First + Second;
            actualResult = 0;
            obj = new Program();
        };

        private Because of = () =>
        {
            actualResult = obj.Add(First, Second);
        };

        private It _shouldReturnFirstOpAddedToSecondOp = () =>
        {
            actualResult.ShouldEqual(5);
        };
    }

    public class when_subtracting_two_numbers
    {
        private static double First;
        private static double Second;
        private static double expectedSubtraction;
        private static double actualResult;
        private static Program obj;

        private Establish context = () => //initialize values in establish context
        {
            First = 1;
            Second = 1;
            expectedSubtraction = First - Second;
            actualResult = 0;
            obj = new Program();
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


