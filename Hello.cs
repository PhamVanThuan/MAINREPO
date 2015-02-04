using System;
using System.io.*;

class Hello {


	private string text = "";
	private string Text{ get; }
	

	private static string GetText(){
		text="Hi";
		return text;
	}

	public static void doSomething(){
		Console.WriteLine("Something");
	}

	public static void Main(){
		Console.WriteLine(GetText());
		doSomething();
	}
}
