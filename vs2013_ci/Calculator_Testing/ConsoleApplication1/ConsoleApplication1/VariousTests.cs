using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;
using NUnit.Framework;
using Machine.Fakes;
using NSubstitute;
using NSubstitute.Core.Arguments;


namespace ConsoleApplication1
{
    //testing accessors
    [TestFixture]
    public class VariousTests 
    {
        private static Program p;
        private static int Age;
        private static string Name;
        private static double Height;
        private static double Weight;
        public static string displayString;


        private Establish context = () =>
        {
            Age = 22;
            Name = "Vishav";
            Height = 1.54;
            Weight = 65;
            displayString = "";
            p = new Program(Name,Age,Height,Weight);
           
            };

        private Because of = () => //'because clauses' are usually one line, focuses on specific aspects that are tested
        {
            displayString = p.Display();      
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

         
        private It should_be_kind_with_weight = () =>
        {
            Assert.GreaterOrEqual(p.Weight,65);
        };

     
        private It must_not_be_weekend = () => //just for fun
        {
            DateTime rightNow = DateTime.Now;       
            Assert.That(!rightNow.DayOfWeek.ToString().Contains("Saturday")||!rightNow.DayOfWeek.ToString().Contains("Sunday"));   
        };
        
        
        private It should_be_friday = () =>  // :P
        {
            Assert.IsTrue(DateTime.Now.DayOfWeek.ToString().Equals("Friday")==true);
        };

       
        private It should_handle_exception_when_dividing_by_0 = () =>
        {
            //Assert.That(ex.Message.Equals("Can't divide by 0")); 
          // divEx.Message.ShouldEqual("Can't divide by 0");
           
        };
     }
}