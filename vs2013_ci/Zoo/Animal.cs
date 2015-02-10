using System;

namespace zoo
{
    public class Animal
    {
        private string color;
        public string Color
        {
            get;
            set;
        }

        private double weight;
        public double Weight
        {
            get;
            set;
        }
        private double height;
        public double Height
        {
            get { return this.height; }
            set { this.height = value; }
        }

        private string name;
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

        public void Display()
        {
            ListBox1.Items.Add("Color : " + this.color + " Height: " + this.height + " Weight: " + this.weight);
        }

    }
}