using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CITester
{

    public class Person
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int age;
        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public Person(string name, int age)
        {
            this.name = name;
            this.age = age;
        }

        public void display()
        {
            Console.WriteLine("Person details: ");
            Console.WriteLine("Name: " + this.name);
            Console.WriteLine("Age: " + this.age);
        }
    }
}
