using System;
using System.io.*;

class Hello {

	private static string GetText(){
		return "Hi";
	}

	public static void doSomething(){
		Console.WriteLine("Something");
	}

	public static void Main(){
		Console.WriteLine(GetText());
		doSomething();
	}
}
