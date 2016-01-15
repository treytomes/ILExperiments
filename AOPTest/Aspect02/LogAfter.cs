using System;
using System.Reflection;

namespace AOPTest.Aspect02
{
	public class LogAfter : AfterAttribute
	{
		public override object Action(object target, MethodBase method, object[] parameters, object result)
		{
			Console.WriteLine("After");
			return null;
		}
	}
}