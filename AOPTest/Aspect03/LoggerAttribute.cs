using AOPTest.Aspect03.Aspects;
using System;
using System.Reflection;

namespace AOPTest.Aspect03
{
	public class LoggerAttribute : AspectAttribute
	{
		public override void Before(object target, MethodBase method, object[] parameters)
		{
			Console.WriteLine("Before");
		}

		public override void After(object target, MethodBase method, object[] parameters, object result)
		{
			Console.WriteLine("After: {0}", result);
		}

		public override bool Exception(object target, MethodBase method, object[] parameters, Exception ex)
		{
			Console.WriteLine("Error: {0}", ex.Message);
			return true;
		}
	}
}