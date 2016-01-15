using System.Reflection;

namespace AOPTest.Aspect02
{
	public delegate object MethodCall(object target, MethodBase method, object[] parameters, AspectAttribute[] attributes);
}