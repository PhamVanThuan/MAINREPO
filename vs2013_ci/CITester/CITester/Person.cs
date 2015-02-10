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

        public Person()
        {
            this.name = "";
            this.age = 0;
        }

        public Person(string name, int age, int n)
        {
            array = new Person[n];
            for (int i = 0; i < array.Length; i++ )
            {
                array[i] = new Person();
            }
                this.FillPeople();
        }

        private Person []array;
        public Person[] Array
        {
            get { return array;  }
            set { array = value;  }         
        }
        public void SayHi()
        {
            Console.WriteLine("Hi " + this.name);
        }
        
        public bool FoundPerson(string n)
        {
           for(int i=0; i<this.array.Length; i++)
           {
               if (array[i].name == n) 
                   return true;
           }
           return false;
        }

        private int age;
        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        

        public void FillPeople()
        {
            Console.WriteLine("Enter each person's name:");
            for (int i = 0; i < this.array.Length; i++ )
            {
                Array[i].name = Console.ReadLine();
            }

            Console.WriteLine("Enter each person's age:");
            for (int i = 0; i < this.array.Length; i++)
            {
                Array[i].age = Convert.ToInt32(Console.ReadLine());
            }

        }

        public void Display()
        {
            Console.WriteLine("Person details ");
            Console.WriteLine("===============");
            Console.WriteLine();
            int positionOfSpace = this.name.IndexOf(' ');
            string firstname = this.name.Substring(0,positionOfSpace);
            string surname = this.name.Substring(positionOfSpace, this.name.Length  - positionOfSpace);
            Console.WriteLine("First Name: " + firstname);
            Console.WriteLine("Surname: " +surname);
            Console.WriteLine("Age: " + this.age);
           
        }
    }
}
