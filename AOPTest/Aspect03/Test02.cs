using System;

namespace AOPTest.Aspect03
{
	public class Test2
	{
		public Test2 Go(int i)
		{
			//object a = null;
			//a.ToString();
			Console.WriteLine("Go!");

			return this;
		}
	}
}