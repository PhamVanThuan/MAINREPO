using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CITester
{
    public class Person : InterfacePerson
    {
        protected string name { get; set; }
        public bool FoundPerson(string name)
        {
            if(this.name == name)
                return true;

            return false;
        }

        public Person(string name)
        {
            this.name = name;
        }

        public static void main(String[] args)
        {

        }
    }
}
