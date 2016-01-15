using System;
using System.Reflection;

namespace AOPTest.Aspect02
{
	public class LogBefore : BeforeAttribute
	{
		public override object Action(object target, MethodBase method, object[] parameters, object result)
		{
			Console.WriteLine("Before");
			return null;
		}
	}
}