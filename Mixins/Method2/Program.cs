using System;
using System.Collections.Generic;
using System.Text;

namespace Mixins.Method2
{
	/// <summary>
	/// Implement mixins using extension methods, empty interfaces,
	/// and the ConditionalWeakTable to manage state.
	/// </summary>
	/// <remarks>
	/// Based on the example I found here: http://www.c-sharpcorner.com/UploadFile/b942f9/how-to-create-mixin-using-C-Sharp-4-0/
	/// </remarks>
	public static class Program
	{
		public static void Main()
		{
			var h = new Human("Jim");
			h.SetBirthDate(new DateTime(1980, 1, 1));
			Console.WriteLine("Name {0}, Age = {1}", h.Name, h.GetAge());
			var h2 = new Human("Fred");
			h2.SetBirthDate(new DateTime(1960, 6, 1));
			Console.WriteLine("Name {0}, Age = {1}", h2.Name, h2.GetAge());
			Console.ReadKey();
		}
	}
}
