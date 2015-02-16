using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;
using NUnit.Framework;
using Machine.Fakes;


namespace ConsoleApplication1
{
    //testing accessors
    public class VariousTests
    {
        private static Program p;
        private static int Age;
        private static string Name;
        private static double Height;
        private static double Weight;
        private static string displayString;

        //testing numbers for fun =)
        private static double first;
        private static double second;
        private static double answer;

        private Establish context = () =>
        {
            Age = 22;
            Name = "Vishav";
            Height = 1.54;
            Weight = 65;
            displayString = "";
            p = new Program(Name,Age,Height,Weight);
            first =1.3;
            second = 2.3;
            answer= 3.6;
        };

        private Because of = () =>
        {
            displayString = p.Display();
            answer = p.addFractions(first,second);
        };

        private It should_display_specified_weight = () =>
        {
            Assert.AreEqual(p.Weight,Weight);
        };
        
        private It should_display_specified_name =()=>
        {
            Assert.That(p.Name.Equals(Name));
            Assert.That(displayString.Contains(Name));     
        };

        private It should_display_specified_age = () =>
        {
            Assert.That(p.Age.Equals(Age));
            Assert.That(displayString.ToString().Contains(p.Age.ToString()));
        };

        private It should_display_specified_height = () =>
        {
            Assert.That(p.Height.Equals(Height));
            Assert.IsTrue(displayString.Contains(Height.ToString()));
        };

        private It should_return_a_rounded_off_value = () =>
         {
             Assert.That(answer, Is.EqualTo(p.addFractions(first,second)).Within(10));
         };

        private It should_be_kind_with_weight = () =>
        {
            Assert.GreaterOrEqual(p.Weight,60);
        };

        private It must_not_be_weekend = () => //just for fun
        {
            DateTime rightNow = DateTime.Now;       
            Assert.That(!rightNow.DayOfWeek.ToString().Contains("Saturday")||!rightNow.DayOfWeek.ToString().Contains("Sunday"));   
        };
        
        private It should_be_friday = () =>  // :P
        {
            Assert.IsTrue(DateTime.Now.DayOfWeek.ToString().Equals("Friday")==false);
        };

    }
}
