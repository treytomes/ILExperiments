using System;
using System.Reflection;

namespace AOPTest.Aspect02
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Interface, Inherited = true)]
	public abstract class AspectAttribute : Attribute
	{
		public abstract object Action(object target, MethodBase method, object[] parameters, object result);
	}
}