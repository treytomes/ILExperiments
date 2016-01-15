using System;

namespace Mixins
{
	public static class Program
	{
		public static void Main()
		{
			Console.WriteLine("Testing mixin method 1...");
			Console.WriteLine();
			Method1.Program.Main();
			Console.WriteLine();
			Console.WriteLine("Press any key to continue: ");
			Console.ReadKey();
			Console.WriteLine();


			Console.WriteLine("Testing mixin method 2...");
			Console.WriteLine();
			Method2.Program.Main();
			Console.WriteLine();
			Console.WriteLine("Press any key to continue: ");
			Console.ReadKey();
			Console.WriteLine();
		}
	}
}
