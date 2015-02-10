using System;

public class Dog : Animal
{
    private string breed;

    public string Breed
    {
        get { return breed;  }
        set { breed = value; }
    }
    
    public Dog(string breed)
    {
        this.breed = breed;
    }

    public override void Display()
    {
        ListBox1.Items.add("Color: "+base.color+" Weight: "+base.weight+" Height: "+base.height+" Breed: "+this.breed);
    }
}
