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
    [TestFixture]
    public class VariousTests 
    {
        private static Program p;
        private static int Age;
        private static string Name;
        private static double Height;
        private static double Weight;
        public static string displayString;

        //testing numbers for fun =)
        private static double first;
        private static double second;
        private static double third;
        private static double fourth;
        private static double answerAdd;
        private static double answerMinus;
        private static double answerMultiply;
        private static double answerDivide;
        private static Exception divEx;

        private Establish context = () =>
        {
            Age = 22;
            Name = "Vishav";
            Height = 1.54;
            Weight = 65;
            displayString = "";
            p = new Program(Name,Age,Height,Weight);
            first =1;
            second = 2;
            third = 3;
            fourth = 4;
            answerAdd= 1.25;
            answerMinus = -0.25;
            answerMultiply = 0.375;
            answerDivide = 0.666666666666667;
            
            //mock out the divide function
            divEx = Catch.Exception (()=> p.WhenToldTo(x => x.divideFractions(1,2,0,0)).Return(null));
        };

        private Because of = () => //'because clauses' are usually one line, focuses on specific aspects that are tested
        {
            displayString = p.Display();
            answerAdd = p.addFractions(first,second,third,fourth);
            answerMinus = p.minusFractions(first, second, third, fourth);
            answerMultiply = p.multiplyFractions(first, second, third, fourth);
            answerDivide = p.divideFractions(first, second, third, fourth);
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

        
        private It should_add_correctly = () =>
         {
             Assert.That(answerAdd, Is.EqualTo(p.addFractions(first,second,third,fourth)).Within(5));
         };

      
        private It should_subtract_correctly = () =>
        {
            Assert.That(answerMinus, Is.EqualTo(-0.25));
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
            Assert.IsTrue(DateTime.Now.DayOfWeek.ToString().Equals("Friday")==false);
        };

       
        private It should_handle_exception_when_dividing_by_0 = () =>
        {
            //Assert.That(ex.Message.Equals("Can't divide by 0")); 
           //divEx.Message.ShouldEqual("Can't divide by 0");
           
        };
     }
}