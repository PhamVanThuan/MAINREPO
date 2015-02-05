using System;

//Demo program written in Notepad just to wrap my head around merges
class Hello {

<<<<<<< Updated upstream

	private string text = "";
	public string Text{ get; }
	

	private static string GetText(){
		text="Hi";
		return text;
	}

	public static void doSomething(){
		Console.WriteLine("Something silly");
		Console.WriteLine();
	}

	public static void Main(){
		Console.WriteLine("Vishav");
		doSomething();
=======
	int x = 1;

	public static void Main(){
		Console.WriteLine("Hi");
>>>>>>> Stashed changes
	}
}
