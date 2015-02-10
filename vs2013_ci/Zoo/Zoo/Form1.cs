using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zoo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            const string breed = "Rotweiller";
            const string color = "black and brown";
            const double weight = 40;
            const double height = 1.5;
            Animal a1 = new Dog(breed,color,weight,height);
            listBox1.Items.Add(a1.Display());

            MessageBox.Show("Now enter details of next dog", "Input");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
            }
        }
    }

    internal class Animal
    {
        
            protected string color;
            public string Color
            {
                get;
                set;
            }

            protected double weight;
            public double Weight
            {
                get;
                set;
            }
            protected double height;
            public double Height
            {
                get { return this.height; }
                set { this.height = value; }
            }

            protected string name;
            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            public Animal()
            {
                color = "black";
                weight = 1.5;
                height = 1.5;
            }

            public Animal(string color, double weight, double height)
            {
                this.color = color;
                this.weight = weight;
                this.height = height;
            }

            public virtual string Display()
            {
                return "Color : " + this.color + " Height: " + this.height + " Weight: " + this.weight;
            }

        }

    internal class Dog : Animal
    {
        private string _breed;

        public string Breed
        {
            get { return _breed; }
            set { _breed = value; }
        }

        public Dog(string breed, string c, double w, double h)
        {
            //base(c,w,h);
            this._breed = breed;
        }

        public override string Display()
        {
            return "Color: " + base.color + " \t Weight: " + base.weight + "kg \t Height: " + base.height + "m \t Breed: " + this._breed;
        }
    }



    }

