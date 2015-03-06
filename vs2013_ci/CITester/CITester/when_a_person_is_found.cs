using System;
using Machine.Specifications;
using Machine.Fakes;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute.Core.Arguments;

namespace CITester
{
    public class when_a_person_is_found : WithFakes
    {
        private static string name;
        private static Person[] people;
        private static bool expected_answer;
        private static bool calculated_answer;
        private static int number_of_people;
        private static Person p;

        private Establish context=()=>
        {
            number_of_people = 5;
            name = "Vishav";
            //p =Param.IsAny<Person>(Param.IsAny<string>);
            people = new Person[number_of_people];
            expected_answer = true;
        };

        private Cleanup after =()=>
        {
            //p.Dispose();
        };

        private Because of = () =>
        {
            calculated_answer = p.FoundPerson(name);
        };

        private It must_return_true = () =>
        {
            Assert.AreEqual(expected_answer, calculated_answer);
        };
     }
}
