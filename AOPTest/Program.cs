using System;

namespace AOPTest
{
	public static class Program
	{
		public static void Main()
		{
			Console.WriteLine("Testing duck typing...");
			Console.WriteLine();
			DuckTyping.Program.Main();
			Console.WriteLine();
			Console.Write("Press any key to continue: ");
			Console.ReadKey();

			// AOP with .NET Remoting.
			Console.WriteLine("Testing AOP Method 1...");
			Console.WriteLine();
			Aspect01.Program.Main();
			Console.WriteLine();
			Console.Write("Press any key to continue: ");
			Console.ReadKey();

			Console.WriteLine("Testing AOP Method 2...");
			Console.WriteLine();
			Aspect02.Program.Main();
			Console.WriteLine();
			Console.Write("Press any key to continue: ");
			Console.ReadKey();

			Console.WriteLine("Testing AOP Method 3...");
			Console.WriteLine();
			Aspect03.Program.Main();
			Console.WriteLine();
			Console.Write("Press any key to continue: ");
			Console.ReadKey();
		}
	}
}
