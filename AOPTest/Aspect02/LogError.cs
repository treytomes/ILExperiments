using System;
using System.Reflection;

namespace AOPTest.Aspect02
{
	public class LogError : LogExceptionAttribute
	{
		public override object Action(object target, MethodBase method, object[] parameters, object result)
		{
			Console.WriteLine("Error");
			return null;
		}
	}
}