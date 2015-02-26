using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;
using NUnit.Framework;
using Machine.Fakes;
//testing accessors

namespace ConsoleApplication1
{
    public class Program
    {
        private double f;
        public double F
        {
            get
            {
                return f;
            }
            set
            {
                f = value;
            }
        }
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        private int age;
        public int Age {
            get
            {
                return age;
            }
            set
            {
                age = value;
            }
        }
        private double height;
        public double Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }
        private double weight;
        public double Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
            }
        }

        public string displayedString
        {
             get;
             set;
        }

        public Program(string name,int age,double height,double weight)
        {
            this.Name = name;
            this.age = age;
            this.height = height;
            this.weight = weight;
            displayedString = "";
        }


        public string Display()
        {
            displayedString = name + ", "
                + age + ", "
                + height + ", "
                + weight;
            return displayedString;
        }

        public double addFractions(double one, double two,double three,double four)
        {
            return (one/two)+(three/four);
        }

        public double minusFractions(double one, double two, double three, double four)
        {
            return (one / two) - (three / four);
        }

        public double divideFractions(double one, double two, double three, double four)
        {
            return (one / two) / (three / four);
        }

        public double multiplyFractions(double one, double two, double three, double four)
        {
            if((three != 0)||(four != 0))
            {
                return (one / two) * (three / four);
            }
           else throw new Exception("Can't divide by 0");
        }



        // Entry point
        public static void Main(string[] args)
        {
            string name = "Vishav";
            int age = 22;
            double weight = 65;
            double height = 1.54;
            double first = 1.1;
            double second = 2.2;
            double third = 3.3;
            double fourth = 4.4;


            Program p = new Program(name,age,weight,height);
            
            Console.Write(p.Display());
            Console.WriteLine();
            Console.WriteLine(p.addFractions(first, second, third, fourth));
            Console.WriteLine(p.divideFractions(1,2,1,0));
        }
    }
}
