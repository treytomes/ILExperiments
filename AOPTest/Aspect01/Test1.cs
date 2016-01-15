using System;

namespace AOPTest.Aspect01
{
	public class Test1 : AspectOrientedObject
	{
		private void Output(string s)
		{
			Console.WriteLine(s);
		}

		public void Go()
		{
			object a = null;
			a.ToString();
			Output("Go");
		}
	}
}