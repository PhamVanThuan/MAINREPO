using System;
using System.io.*;

//Demo program written in Notepad just to wrap my head around merges
class Hello {


	private string text = "";
	private string Text{ get; }
	

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
	}
}
