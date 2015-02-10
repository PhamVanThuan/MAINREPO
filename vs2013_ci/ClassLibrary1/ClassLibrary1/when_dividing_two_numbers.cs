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
    public class when_dividing_two_numbers
    {
            private static double First;
            private static double Second;
            private static double expectedDividend;
            private static double actualAnswer;
            private static Class1 obj;

            private Establish context = () =>
            {
                First = 2;
                Second = 1;
                expectedDividend = First/Second;
                actualAnswer = 2;
                obj = new Class1();
            };

            private Because of =()=>
            {
               actualAnswer = obj.Divide(First,Second);
            };

            private It shouldReturnFirstOpDividedBySecondOp = () =>
            {
                actualAnswer.ShouldEqual(expectedDividend);
            };
    }
}
